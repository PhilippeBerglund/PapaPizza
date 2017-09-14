using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PapaPizza.Data;
using PapaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PapaPizza.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        public HttpContext httpContext;

        public CartService(ApplicationDbContext context) //, HttpContext HttpContext
        {
            _context = context;
            //     httpContext = HttpContext;
        }


        //Todo FIX->
        public void EmptyCart(int? id)
        {
            if (id == null)
            {
                return;
            }
            var cartItems = _context.CartItems.Where(
                cart => cart.CartId == Convert.ToInt32(id));

            foreach (var item in cartItems)
            {
                _context.CartItems.Remove(item);
            }
            // Save changes
            _context.SaveChanges();
        }

        public int? CheckCart() // HttpContext httpContext
        {
            // int? id = httpContext.Session.GetInt32("CartSession");

            int id = 0;
            foreach (var item in _context.CartItems)
            {
                id = item.CartId;
            }
            return id;
        }

        //Todo FIX->
        public void RemoveCartItem(int? id)
        {
            var item = _context.CartItems.FirstOrDefault(m => m.CartItemId == id);
            _context.CartItems.Remove(item);
            _context.SaveChangesAsync();
        }


        public decimal? GetTotal(HttpContext httpContext)
        {
            decimal? total = 0;
         
            decimal? cartID = httpContext.Session.GetInt32("CartSession");

            total = (from cartItems in _context.CartItems
                     where cartItems.CartId == cartID
                     select (int?)cartItems.Quantity *
                     cartItems.Dish.Price).Sum();

            return total;
        }

        //public decimal? GetDishTotal(HttpContext httpContext, int cartItemId )
        //{
        //    var id = _context.CartItems.Where(c => c.CartItemId == cartItemId);
        //    decimal total = 0;

        //    total = (from DishIngredients in _context.DishIngredients
        //             where cartItemId = id
        //             select (int? )DishIngredients.Price += CartItemIngredients.Price)
        //    return total;
        //}

     

        //public List<CartItem> AddToCart(int?  dishId, HttpContext httpContext)
        //{


        //    if (dishId == null)
        //    {
        //        //Gör något här!

        //        //return NotFound();
        //    }

        //    var dish = _context.Dishes
        //        .Include(d => d.DishIngredients)
        //        .ThenInclude(di => di.Ingredient)
        //        .FirstOrDefault(m => m.DishId == dishId);

        //    var cartID = httpContext.Session.GetInt32("CartSession");

        //    Cart cart;

        //    if (httpContext.Session.GetInt32("CartSession") == null)
        //    {
        //        cart = new Cart
        //        {
        //            CartItems = new List<CartItem>()
        //        };

        //        _context.Cart.Add(cart);
        //        _context.SaveChanges();
        //        httpContext.Session.SetInt32("CartSession", cart.CartId);
        //        cartID = cart.CartId;
        //    }

        //    cart = _context.Cart.Include(c => c.CartItems)
        //        .ThenInclude(ci => ci.Dish)
        //        .ThenInclude(d => d.DishIngredients)
        //        .SingleOrDefault(c => c.CartId == cartID);

        //    CartItem cartItem = new CartItem
        //    {
        //        Dish = dish,
        //        Cart = cart,
        //        CartItemIngredients = new List<CartItemIngredient>(),
        //        Quantity = 1
        //    };

        //    foreach (var item in dish.DishIngredients)
        //    {
        //        var cartItemIngredient = new CartItemIngredient
        //        {
        //            Ingredient = item.Ingredient,
        //            CartItem = cartItem
        //        };
        //        cartItem.CartItemIngredients.Add(cartItemIngredient);
        //    }
        //    cart.CartItems.Add(cartItem);

        //    _context.SaveChanges();

        //    return cart.CartItems;
        //}

    }
}
