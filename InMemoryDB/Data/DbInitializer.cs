using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PapaPizza.Models;

namespace PapaPizza.Data
{
    public class DbInitializer
    {
        public static void Initializer(ApplicationDbContext context,  UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser
            {
                UserName = "student@test.co.uk",
                Email = "student@test.co.uk",
            };
            var r = userManager.CreateAsync(aUser, "Pa$$w0rd");

            var adminRole = new IdentityRole { Name = "Admin"};
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser
            {
                UserName = "admin@test.co.uk",
                Email = "admin@test.co.uk"
            };
            var adminUserResult = userManager.CreateAsync(adminUser, "Pa$$w0rd").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");

            if (context.Dishes.ToList().Count == 0)
            {
                var cheese = new Ingredient {Name = "Cheese"};
                var tomato = new Ingredient { Name = "Tomato" };
                var ham = new Ingredient    { Name = "Ham" };

                var capricciosa = new Dish  { Name = "Capricciosa", Price = 79 };
                var margaritha = new Dish   { Name = "Margaritha", Price = 72 };
                var hawaii = new Dish       { Name = "Hawaii", Price = 75 };

                var capricciosaCheese = new DishIngredient {Dish = capricciosa, Ingredient = cheese};
                var capricciosaTomato = new DishIngredient { Dish = capricciosa, Ingredient = tomato };
                var capricciosaHam = new DishIngredient    { Dish = capricciosa, Ingredient = ham };

                capricciosa.DishIngredients = new List<DishIngredient>();
                capricciosa.DishIngredients.Add(capricciosaTomato);
                capricciosa.DishIngredients.Add(capricciosaHam);
                capricciosa.DishIngredients.Add(capricciosaCheese);

                context.AddRange(capricciosaCheese, capricciosaHam, capricciosaTomato);
                context.AddRange(capricciosa, margaritha, hawaii);
                context.SaveChanges();
            }


        }
    }
}
