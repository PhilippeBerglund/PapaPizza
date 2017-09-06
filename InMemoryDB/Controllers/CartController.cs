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

        //private readonly Cart cart; 


        public CartController(ApplicationDbContext context)// CartService cartService
        {
            //_cartService = cartService;
            _context = context;
        }

        //public int CartId { get; set; }

        //public List<CartItem> CartItems { get; set; }

        //public static Cart GetCart(IServiceProvider serviceProvider)
        //{
        //    ISession session = serviceProvider.GetRequiredService<HttpContextAccessor>()?
        //        .HttpContext.Session;

        //    var context = serviceProvider.GetService<ApplicationDbContext>();
        //    int cartId = session.GetInt32("CartId") ?? Convert.ToInt32(Guid.NewGuid());

        //    session.SetString("CartId", cartId.ToString());

        //    return new Cart(context) { CartId = cartId };
        //}

        //public Dish AddToCart(Dish dish, int amount)
        //{
        //    var cartItem = _context.CartItems.SingleOrDefault(
        //        c => c.Dish.DishId == dish.DishId && c.CartId == CartId);

        //    if (cartItem == null)
        //    {
        //        cartItem = new CartItem
        //        {
        //            CartId = CartId,
        //            Dish = dish,
        //            Quantity = 1
        //        };

        //        _context.CartItems.Add(cartItem);
        //    }

        //    else
        //    {
        //        cartItem.Quantity++;
        //    }
        //    _context.SaveChanges();
        //    return dish;
        //}


        // GET: Carts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Carts.ToListAsync());
        }




        // GET: Carts/ItemToCart/5
        //[HttpPost]
        public async Task<IActionResult> ItemToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //List<DishIngredient> ingredients = new List<DishIngredient>();
            var dish = await _context.Dishes
             .Include(d => d.DishIngredients)
             .ThenInclude(di => di.Ingredient)
             .SingleOrDefaultAsync(m => m.DishId == id);


            List<Dish> shoppingCart;

            if (HttpContext.Session.GetString("CartSession") == null)
            {
                shoppingCart = new List<Dish>();
            }
            else
            {
                var temp = HttpContext.Session.GetString("CartSession");
                shoppingCart = JsonConvert.DeserializeObject<List<Dish>>(temp);
            }
            shoppingCart.Add(dish);

            var serializedValue = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("CartSession", serializedValue);

            return RedirectToAction("Index", "Dishes");

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

            var cart = await _context.Carts.SingleOrDefaultAsync(m => m.CartId == id);
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

            var cart = await _context.Carts
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
            var cart = await _context.Carts.SingleOrDefaultAsync(m => m.CartId == id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }

    }
}
