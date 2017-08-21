using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InMemoryDBPizzeria.Data;

namespace InMemoryDBPizzeria.Controllers
{
    public class DishController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public DishController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var pizzaList = _context.Dishes.ToList();
            return View(pizzaList);
        }
    }
}