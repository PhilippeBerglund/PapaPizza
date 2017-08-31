using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PapaPizza.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaPizza.Services
{
    public class IngredientService
    {
        private readonly ApplicationDbContext _context;

        public IngredientService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get all Ingredients 
        public List<Models.Ingredient> GetIngredients()
        { 
            return  _context.Ingredients.ToList(); // .OrderBy(x => x.Name)  
        }

        // Get Checked Ingredients
        public string AddedIngredients(int id)
        {
            var ingredients = _context.DishIngredients.Include(di => di.Ingredient).Where(di => di.DishId  == id && di.checkboxAnswer);
            string checkedIngredients = "";
            foreach (var ing  in ingredients)
            {
                checkedIngredients += ing.Ingredient.Name + " "; 
            }
            return checkedIngredients;
        }
    }
}
