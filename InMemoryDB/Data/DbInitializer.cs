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
        public static void Initializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser
            {
                UserName = "student@test.co.uk",
                Email = "student@test.co.uk",
            };
            var r = userManager.CreateAsync(aUser, "Pa$$w0rd");

            var adminRole = new IdentityRole { Name = "Admin" };
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
                var pizza = new Category { Name = "Pizza" };
                var gelato = new Category { Name = "Gelato" };
                var coffee = new Category { Name = "Kaffe" };

                var capricciosa = new Dish { Name = "Capricciosa", Price = 79, Category = pizza };
                var margaritha = new Dish { Name = "Margaritha", Price = 72, Category = pizza };
                var hawaii = new Dish { Name = "Hawaii", Price = 75, Category = pizza };

                var gelatoA = new Dish { Name = "Vanilla Gelato", Price = 45, Category = gelato };
                var gelatoB = new Dish { Name = "Pistachio Gelato", Price = 45, Category = gelato };

                var coffeA = new Dish { Name = "Espresso", Category = coffee, Price = 21 };
                var coffeB = new Dish { Name = "Cappuccino", Category = coffee, Price = 37 };

                var cheese = new Ingredient { Name = "Cheese" };
                var tomato = new Ingredient { Name = "Tomato" };
                var ham = new Ingredient { Name = "Ham" };
                var mushroom = new Ingredient { Name = "Mushroom" };
                var pineapple = new Ingredient { Name = "Pineapple" };

                var capricciosaCheese = new DishIngredient { Dish = capricciosa, Ingredient = cheese };
                var capricciosaTomato = new DishIngredient { Dish = capricciosa, Ingredient = tomato };
                var capricciosaHam = new DishIngredient { Dish = capricciosa, Ingredient = ham };
                var capricciosaShroom = new DishIngredient { Dish = capricciosa, Ingredient = mushroom };

                var margarithaCheese = new DishIngredient { Dish = margaritha, Ingredient = cheese };
                var margarithaTomato = new DishIngredient { Dish = margaritha, Ingredient = tomato };
                var margarithaHam = new DishIngredient { Dish = margaritha, Ingredient = ham };

                var hawaiiCheese = new DishIngredient { Dish = hawaii, Ingredient = cheese };
                var hawaiiTomato = new DishIngredient { Dish = hawaii, Ingredient = tomato };
                var hawaiiHam = new DishIngredient { Dish = hawaii, Ingredient = ham };
                var hawaiiPineapple = new DishIngredient { Dish = hawaii, Ingredient = pineapple };


                capricciosa.DishIngredients = new List<DishIngredient> {
                    capricciosaTomato, capricciosaCheese, capricciosaHam, capricciosaShroom
                };

                margaritha.DishIngredients = new List<DishIngredient>
                {
                   margarithaTomato, margarithaCheese, margarithaHam
                };

                hawaii.DishIngredients = new List<DishIngredient>
                {
                    hawaiiTomato, hawaiiCheese, hawaiiHam, hawaiiPineapple
                };

                //context.AddRange(  capricciosaTomato, capricciosaCheese, capricciosaHam
                //                 , margarithaCheese, margarithaHam, margarithaTomato
                //                 , hawaiiTomato, hawaiiCheese, hawaiiHam, hawaiiPineapple);

                context.AddRange(capricciosa, margaritha, hawaii, gelatoA, gelatoB, coffeA, coffeB);
                context.SaveChanges();
            }


        }
    }
}
