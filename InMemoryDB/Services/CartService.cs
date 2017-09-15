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
        private readonly IngredientService _ingredientService;

        public CartService(ApplicationDbContext context, IngredientService ingredientService)
        {
            _context = context;
            _ingredientService = ingredientService;
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

        public int? CheckCart()
        {

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


        public decimal ModifiedPrice (int id, List<CartItemIngredient> cartItemIngredients)
        {
            var home = _ingredientService.ListOfHomeIngredients(id);
            var homeIngredient = home.Select(x => x.Ingredient).ToList();
            //var itemToEdit = "";

            var away = cartItemIngredients.Where(c => c.Enabled).Select(v => v.Ingredient).ToList();
            //var awayIngredient = away.Select(c => c.Ingredient).ToList();

            away.RemoveAll(a => homeIngredient.Contains(a));
            var price = 0;
            foreach (var item in away)
            {
                price += Convert.ToInt32(item.Price);
            }
            price.ToString();
            var testo = 0;
            //foreach (var item in _context.CartItems)
            //{
            //     testo  +=  Convert.ToInt32(item.Dish.Price + price);
            //}

            var newPrice =  _context.CartItems.Select(c => c.Dish.Price + price).Single();
            return newPrice;
        }

    }
}
