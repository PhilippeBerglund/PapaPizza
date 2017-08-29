using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PapaPizza.Data;
using PapaPizza.Models;
using Microsoft.AspNetCore.Http;
using PapaPizza.Services;

namespace PapaPizza.Controllers
{
    public class DishesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IngredientService _ingredientService;

        public DishesController(ApplicationDbContext context, IngredientService ingredientService)
        {
            _context = context;
            _ingredientService = ingredientService;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            var catList = _context.Categories.ToList();

            //var dishes = await _context.
            return View(await _context.Dishes
                    .Include(d => d.DishIngredients)
                    .ThenInclude(di => di.Ingredient)
                    .ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id )
        {
            if (id == null)
            {
                return NotFound();
            }
            var catList = _context.Categories.ToList();
            var dish = await _context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.DishId == id);

                if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            //ViewData["ExtraList"] = new SelectList(_context.Ingredients, "IngredientId", "Name");
            //var dish = 0; //= await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
            ViewData["CatList"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishId,Name,Price, CategoryId")] Dish dish, IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                //var newDish = new Dish
                //{
                //    DishId = dish.DishId,
                //    Name = dish.Name,
                //    Price = dish.Price,
                //    CategoryId = category.CategoryId
                foreach (var ingredient in _ingredientService.GetIngredients())
                {
                    var dishIngredient = new DishIngredient
                    {
                        Ingredient = ingredient,
                        Dish = dish,
                        checkboxAnswer = form.Keys.Any(x => x == $"checkboxes-{ingredient.IngredientId}")
                    };
                    _context.DishIngredients.Add(dishIngredient);
                }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        } 
            return View(dish);
    }

    // GET: Dishes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
        ViewData["CatList"] = new SelectList(_context.Categories, "CategoryId", "Name", dish.CategoryId);
        if (dish == null)
        {
            return NotFound();
        }
        return View(dish);
    }

    // POST: Dishes/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("DishId,Name,Price")] Dish dish, Category category)
    {
        if (id != dish.DishId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var newDish = new Dish
                {
                    DishId = dish.DishId,
                    Name = dish.Name,
                    Price = dish.Price,
                    CategoryId = category.CategoryId
                ,
                    DishIngredients = dish.DishIngredients
                };
                _context.Update(newDish);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(dish.DishId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(dish);
    }

    // GET: Dishes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var dish = await _context.Dishes
            .SingleOrDefaultAsync(m => m.DishId == id);
        if (dish == null)
        {
            return NotFound();
        }

        return View(dish);
    }

    // POST: Dishes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
        _context.Dishes.Remove(dish);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DishExists(int id)
    {
        return _context.Dishes.Any(e => e.DishId == id);
    }


    // GET: Dishes/AddToCart/6
    public async Task<IActionResult> AddToCart(int? id)
    {
        var dish = await _context.Dishes
       .SingleOrDefaultAsync(m => m.DishId == id);
        if (dish == null)
        {
            return NotFound();
        }

        return View(dish);

        //return RedirectToAction("Index");
        // return RedirectToAction(nameof(Index));
    }

    // POST: Dishes/AddToCart/6
    [HttpPost, ActionName("AddToCart")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(int id)
    {
        var d = id.ToString();

        return RedirectToAction("Index");
        // return RedirectToAction(nameof(Index));
    }
}
}
