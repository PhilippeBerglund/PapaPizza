using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using PapaPizza.Models;
using PapaPizza.Services;


namespace PapaPizza.Data
{
    public class DbInitializer
    {
        public static void Initializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IngredientService ingredientService, CartService cartService)
        {
            var aUser = new ApplicationUser
            {
                UserName = "student@test.co.uk",
                Email = "student@test.co.uk",
                FirstName = "Jonh",
                LastName = "Doe",
                Street = "Good street 1",
                Zip = "123 45",
                City = "Goodtown",
                PhoneNumber = "123456789",
                CreditCardNumber = "1234567890987"
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
            userManager.AddToRoleAsync(aUser, "aUser");

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

                var cheese = new Ingredient { Name = "Mozarella", Price = 10 };
                var tomato = new Ingredient { Name = "Tomato", Price = 10 };
                var ham = new Ingredient { Name = "Parma-Ham", Price = 10 };
                var mushroom = new Ingredient { Name = "Mushroom", Price = 10 };
                var pineapple = new Ingredient { Name = "Pineapple", Price = 10 };
                var ruccola = new Ingredient { Name = "Ruccola", Price = 10 };
                var salami = new Ingredient { Name = "Salami", Price = 10 };
                var sdtomato = new Ingredient { Name = "Sun dried tomatos", Price = 10 };
                var olives = new Ingredient { Name = "Olives", Price = 10 };
                var parmesan = new Ingredient { Name = "Parmesan", Price = 10 };

                var capricciosaCheese = new DishIngredient { Dish = capricciosa, Ingredient = cheese, checkboxAnswer = true };
                var capricciosaTomato = new DishIngredient { Dish = capricciosa, Ingredient = tomato, checkboxAnswer = true };
                var capricciosaHam = new DishIngredient { Dish = capricciosa, Ingredient = ham, checkboxAnswer = true };
                var capricciosaShroom = new DishIngredient { Dish = capricciosa, Ingredient = mushroom, checkboxAnswer = true };

                var margarithaCheese = new DishIngredient { Dish = margaritha, Ingredient = cheese, checkboxAnswer = true };
                var margarithaTomato = new DishIngredient { Dish = margaritha, Ingredient = tomato, checkboxAnswer = true };
                var margarithaHam = new DishIngredient { Dish = margaritha, Ingredient = ham, checkboxAnswer = true };

                var hawaiiCheese = new DishIngredient { Dish = hawaii, Ingredient = cheese, checkboxAnswer = true };
                var hawaiiTomato = new DishIngredient { Dish = hawaii, Ingredient = tomato, checkboxAnswer = true };
                var hawaiiHam = new DishIngredient { Dish = hawaii, Ingredient = ham, checkboxAnswer = true };
                var hawaiiPineapple = new DishIngredient { Dish = hawaii, Ingredient = pineapple, checkboxAnswer = true };


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

                context.AddRange(
                                cheese,
                                tomato,
                                ham,
                                mushroom,
                                pineapple,
                                ruccola,
                                salami,
                                sdtomato,
                                olives,
                                parmesan
                                );

                context.AddRange(capricciosa, margaritha, hawaii, gelatoA, gelatoB, coffeA, coffeB);
                context.SaveChanges();
            }


        }
    }
}
