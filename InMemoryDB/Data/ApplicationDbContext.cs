using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PapaPizza.Models;

namespace PapaPizza.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DishIngredient>()
                .HasKey(di => new { di.DishId, di.IngredientId });

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Dish)
                .WithMany(d => d.DishIngredients)
                .HasForeignKey(di => di.DishId);

            builder.Entity<DishIngredient>()
                .HasOne(di => di.Ingredient)
                .WithMany(i => i.DishIngredients)
                .HasForeignKey(di => di.IngredientId);
         
            builder.Entity<CartItemIngredient>()
                .HasKey(ci => new { ci.CartItemId, ci.IngredientId });

            builder.Entity<CartItemIngredient>()
              .HasOne(ci => ci.CartItem)
              .WithMany(c => c.CartItemIngredients)
              .HasForeignKey(ci => ci.CartItemId);

            builder.Entity<CartItemIngredient>()
               .HasOne(ci => ci.Ingredient)
               .WithMany(i => i.CartItemIngredients)
               .HasForeignKey(ci => ci.IngredientId);


            ////->
            //builder.Entity<CartItem>()
            //    .HasKey(ci => new { ci.CartId, ci.DishId });

            //builder.Entity<Cart>()
            //    .HasKey(c => new { c.CartId });

            //->


            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartItemIngredient> CartItemIngredients { get; set; }

    }
}
