using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PapaPizza.Data;
using PapaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PapaPizza.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        public HttpContext httpContext;
        private readonly IngredientService _ingredientService;

        public CartService(ApplicationDbContext context, IngredientService ingredientService)
        {
            _context = context;
            _ingredientService = ingredientService;
        }

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
            _context.SaveChanges();
        }

        public int? CheckCart()
        {

            int id = 0;
            foreach (var item in _context.CartItems)
            {
                id = item.CartId;
            }
            return id;
        }

        public void RemoveCartItem(int? id)
        {
            var item = _context.CartItems.FirstOrDefault(m => m.CartItemId == id);
            _context.CartItems.Remove(item);
            _context.SaveChangesAsync();
        }
        public decimal ModifiedCartItemPrice ( int cartItemId, List<CartItemIngredient> cartItemIngredients, int dishID)
        {
           
            var dishIngredients = _ingredientService.ListOfDishIngredients(dishID);
            var ingredient = dishIngredients.Select(x => x.Ingredient).ToList();

            var addedIngredients = cartItemIngredients.Where(c => c.Enabled).Select(v => v.Ingredient).ToList();

            addedIngredients.RemoveAll(a => ingredient.Contains(a));
            var price = 0;
            foreach (var item in addedIngredients)
            {
                price += Convert.ToInt32(item.Price);
            }

            var newPrice =  _context.CartItems.Where(d => d.CartItemId == cartItemId).Select(c => c.Dish.Price + price).FirstOrDefault();
            return newPrice;
        }


        public decimal? TotalCartSum(int cartItemId)
        {
           decimal? totalPrice = 0;

           decimal ? price = _context.CartItems.Where(i => i.CartItemId == cartItemId)
                     .Select(q => q.Quantity * q.Dish.Price).Sum();

            foreach (var itemPrice in _context.CartItems)
            {
                totalPrice += itemPrice.Dish.Price;

                totalPrice += itemPrice.CartItemIngredients.Where(s => s.Enabled).Sum(cii => itemPrice.Dish
                    .DishIngredients.Any(di => di.IngredientId == cii.IngredientId) ? 0 : cii.Ingredient.Price);
            }
            return totalPrice;
        }

        public CartItem IsCartItem()
        {
            return _context.CartItems.SingleOrDefault();  
        }

    }
}
