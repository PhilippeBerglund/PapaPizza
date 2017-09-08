using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PapaPizza.Data;
using PapaPizza.Models;
using PapaPizza.Services;
using Microsoft.AspNetCore.Http;
using PapaPizza.Controllers;
using Newtonsoft.Json;

namespace PapaPizza
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("DefaultConnection"));

            //--
            services.Configure<IISOptions>(options =>
            {

            });
            //--
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<UserManager<ApplicationUser>>();
            services.AddTransient<RoleManager<IdentityRole>>();  // behövs?? 
            services.AddTransient<IngredientService>();
            services.AddTransient<CartService>();

            //->
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddScoped(sp => CartController.GetCart(sp));
            //<-
            services.AddMvc();

            ////->
            //services.AddMemoryCache();
            services.AddSession();
            ////<-
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app
                             , IHostingEnvironment env
                             , UserManager<ApplicationUser> userManager
                             , ApplicationDbContext context
                             , RoleManager<IdentityRole> roleManager
                             , IngredientService ingredientService
                             , CartService cartService
                            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            ////->
            app.UseSession();
            ////<-
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Dishes}/{action=Index}/{id?}");
            });

            DbInitializer.Initializer(context, userManager, roleManager, ingredientService, cartService);
        }
    }
}
