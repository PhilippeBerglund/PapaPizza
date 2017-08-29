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
        public List<Models.Ingredient> GetIngredients()
        {
            var extraIngredients = _context.Ingredients.ToList();
            return extraIngredients;
        }
    }
}
