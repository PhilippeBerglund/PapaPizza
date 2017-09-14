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
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace PapaPizza.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;
        private readonly IngredientService _ingredientService;
        //private readonly HttpContext httpContext;
        int cartID { get; set; }

        public CartController(ApplicationDbContext context, CartService cartService, IngredientService ingredientService)
        {
            _context = context;
            _cartService = cartService;
            _ingredientService = ingredientService;
        }

        // GET: CartItems
        public async Task<IActionResult> CartIndex(int? id)
        {
            id = HttpContext.Session.GetInt32("CartSession");

            if (id == null)
            {
                return NotFound();
            }

            //var cartItem = await _context.CartItems
            //        .Include(ci => ci.CartItemIngredients)
            //    .Include(c => c.Dish)
            //    .ThenInclude(d => d.DishIngredients)
            //    .SingleOrDefaultAsync(c => c.CartItemId == id);


            var catList = _context.Categories.ToListAsync();
            // test->
            //var testo = dish.Select(x => x.CartId);
            //foreach (var item in testo)
            //{
            //    var test = GetCount(Convert.ToInt32(item));
            //}
            // end test<-

            var cart = _context.Cart
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.CartItemIngredients)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Dish)
                .ThenInclude(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient);



            _context.SaveChanges();

            return View("CartIndex", cart);

        }



        // GET: Carts/ItemToCart/5
        //[HttpGet]
        public async Task<IActionResult> ItemToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
           .Include(d => d.DishIngredients)
           .ThenInclude(di => di.Ingredient)
           .FirstOrDefaultAsync(m => m.DishId == id);

            var cartID = HttpContext.Session.GetInt32("CartSession");

            Cart cart;

            if (HttpContext.Session.GetInt32("CartSession") == null)
            {
                cart = new Cart
                {
                    CartItems = new List<CartItem>()
                };

                _context.Cart.Add(cart);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetInt32("CartSession", cart.CartId);
                cartID = cart.CartId;
            }

            cart = _context.Cart
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Dish)
                .ThenInclude(d => d.DishIngredients)
                    .SingleOrDefault(c => c.CartId == cartID);

            CartItem cartItem = new CartItem
            {
                CartId = cart.CartId, // test
                Dish = dish,
                DishId = dish.DishId, // test
                Cart = cart,  // behövs??
                CartItemIngredients = new List<CartItemIngredient>(),
                Quantity = 1
            };

            foreach (var item in dish.DishIngredients.Where(di => di.checkboxAnswer))
            {
                var cartItemIngredient = new CartItemIngredient
                {
                    Enabled = item.checkboxAnswer,
                    Ingredient = item.Ingredient,
                    CartItem = cartItem   // behövs ??
                };
                cartItem.CartItemIngredients.Add(cartItemIngredient);
            }

            // mir tror 
            //var ingredients = _context.CartItemIngredients.Include(c => c.Ingredient)
            //    .ThenInclude(ci => ci.DishIngredients)
            //    .FirstOrDefault(d => d.IngredientId == id && d.Enabled);

            cart.CartItems.Add(cartItem);

            _context.SaveChanges();

            return RedirectToAction("Index", "Dishes"); // bör ev skicka med cart.CartItems

        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,ApplicationUserId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CartIndex));
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> EditCartItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems
                .Include(ci => ci.Dish)
                .Include(ci => ci.CartItemIngredients)
                .ThenInclude(cii => cii.Ingredient)
                .SingleOrDefaultAsync(m => m.CartItemId == id);

            if (cartItem == null)
            {
                return NotFound();
            }

            //foreach (var item in cartItem.CartItemIngredients)
            //{
            //    item.Dish = _context.Dishes
            //        .Include(d => d.DishIngredients)
            //        .ThenInclude(di => di.Ingredient)
            //        .FirstOrDefault(x => x.DishId == item.DishId);

            //}
            return View(cartItem);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCartItem(int id, [Bind("CartItemId, DishId")] CartItem cartItem,  IFormCollection form)
        {
            if (id != cartItem.CartItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var itemToEdit = await _context.CartItems
                  .Include(ci => ci.CartItemIngredients)
                  .Include(d => d.Dish)
                  .SingleOrDefaultAsync(m => m.CartItemId == id);

                    foreach (var dishIngredient in itemToEdit.CartItemIngredients)
                    {
                        _context.Remove(dishIngredient);
                    }
                    await _context.SaveChangesAsync();

                    cartItem = new CartItem
                    {
                        CartId = cartItem.CartId, // test
                        Cart = cartItem.Cart,  // behövs??
                        Dish = itemToEdit.Dish,
                        DishId = itemToEdit.DishId, // test

                        CartItemIngredients = new List<CartItemIngredient>(),
                        Quantity = 1
                    };

                    foreach (var ingredient in _ingredientService.GetIngredients())
                    {
                        var cartItemIngrtedient = new CartItemIngredient
                        {

                            Ingredient = ingredient,
                            Enabled = form.Keys.Any(x => x == $"checkboxes-{ingredient.IngredientId}")
                        };
                        cartItem.CartItemIngredients.Add(cartItemIngrtedient);

                    }
                    _context.CartItems.Add(cartItem);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cartItem.CartId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CartIndex));

            }
            return View(cartItem);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .SingleOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.SingleOrDefaultAsync(m => m.CartId == id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CartIndex));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartId == id);
        }

        //Todo FIX ->
        public int GetCount(int? id)
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in _context.CartItems
                          where cartItems.CartId == id
                          select (int?)cartItems.Quantity).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        //Todo FIX -> [HttpPost] 
        public async Task<IActionResult> EmptyCart(int? id)
        {
            _cartService.EmptyCart(id);
            // return View("CartIndex");
            return RedirectToAction(nameof(CartIndex));
        }

        //Todo FIX -> [HttpPost] // works only once, throws null exeption when deleting second time.. 
        public async Task<IActionResult> RemoveFromCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _cartService.RemoveCartItem(id);
            return View("CartIndex");
            //return RedirectToAction(nameof(CartIndex));
        }
    }
}
