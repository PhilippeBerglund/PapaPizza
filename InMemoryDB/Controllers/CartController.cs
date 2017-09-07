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
        //private readonly HttpContext httpContext;

        public CartController(ApplicationDbContext context)// CartService cartService
        {
            //_cartService = cartService;
            _context = context;
        }



        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            //var id = HttpContext.Session.GetInt32("CartSession");
            //if (id != null)
            //{
            //    var cartItems = await _context.CartItems
            //    .Include(c => c.Dish)
            //    .ThenInclude(d => d.DishIngredients)
            //    .SingleOrDefaultAsync(c => c.CartItemId == id);
            //}
            //var korgEnhet = cartItems;

            var catList = _context.Categories.ToListAsync();
            var dish = await _context.CartItems
                .Include(c => c.Dish)
                .ThenInclude(d=> d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .ToListAsync();

            return View("Index", dish);
        }




        // GET: Carts/ItemToCart/5
        //[HttpPost]
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

            cart = _context.Cart.Include(c => c.CartItems)
                .ThenInclude(ci => ci.Dish)
                .ThenInclude(d => d.DishIngredients)
                    .SingleOrDefault(c => c.CartId == cartID);

            CartItem cartItem = new CartItem
            {
                Dish = dish,
                Cart = cart,
                CartItemIngredients = new List<CartItemIngredient>(),
                Quantity = 1
            };

            foreach (var item in dish.DishIngredients)
            {
                var cartItemIngredient = new CartItemIngredient
                {
                    Ingredient = item.Ingredient,
                    CartItem = cartItem
                };
                cartItem.CartItemIngredients.Add(cartItemIngredient);
            }
            cart.CartItems.Add(cartItem);

            _context.SaveChanges();

            // mir tror 
            //var ingredients = _context.CartItemIngredients.Include(c => c.Ingredient)
            //    .ThenInclude(ci => ci.DishIngredients)
            //    .FirstOrDefault(d => d.IngredientId == id && d.Enabled);

            return RedirectToAction("Index", "Dishes", cart.CartItems);




            // cartItems.Add(new Dish{ });

            // todo: fix 
            //ViewBag.CartSummary = string.Join("\n", shoppingCart.Select(c => c.Name).Distinct());

            //var serializedValue = JsonConvert.SerializeObject(shoppingCart);
            //HttpContext.Session.SetInt32("CartSession", cart.CartId);




            // GetCartId(HttpContext httpContext);
            //var cartItem = await _context.CartItems.SingleOrDefaultAsync(
            //    c => c.CartId == _cartService.GetCartId(httpContext)); // _shoppingCartId


            //



            //var items = _context.Dishes;

            //var cart = _context.Carts
            //    .SingleOrDefaultAsync(m => m.CartId == id);



            //    cartItem = AddToCart(cartItem, cartItem.);

            //    if (cartItem == null)
            //    {
            //        return NotFound();
            //    }
            //    //return RedirectToAction("Index");
            //    return View(cartItem);
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
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.SingleOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,ApplicationUserId")] Cart cart)
        {
            if (id != cart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartId))
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
            return View(cart);
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
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartId == id);
        }

    }
}
