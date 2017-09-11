using Microsoft.AspNetCore.Http;
using PapaPizza.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PapaPizza.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        //private readonly string _shoppingCartId;
        public HttpContext httpContext;
        //string cartID;

        //public const string CartSessionKey = "CartSession";

        public CartService(ApplicationDbContext context) //, HttpContext HttpContext
        {
            _context = context;
            //     httpContext = HttpContext;
        }



        //    public CartService GetCart(ApplicationDbContext db, HttpContext httpContext)
        //            => GetCart(db, GetCartId(httpContext));

        //    public static CartService GetCart(ApplicationDbContext db, string cartId)
        //       => new CartService(db, cartId);



        //    // We're using HttpContextBase to allow access to sessions.
        //    public string GetCartId(HttpContext httpContext)
        //    {
        //        var cartId = httpContext.Session.GetString("Session");

        //        if (cartId == null)
        //        {
        //            //A GUID to hold the cartId. 
        //            cartId = Guid.NewGuid().ToString();

        //            // Send cart Id as a cookie to the client.
        //            httpContext.Session.SetString("Session", cartId);
        //        }

        //        return cartId;
        //    }



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

        public int ?  GetItemCount() // HttpContext httpContext
        {
            // int? id = httpContext.Session.GetInt32("CartSession");

            int count = 0;
            foreach (var item in _context.CartItems)
            {
                count = item.CartId;
            }
            return  count  ;
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
            var session = httpContext.Session.GetInt32("CartSession");
            decimal? total = 0;
            //if (httpContext == null)
            //{

            decimal? cartID = httpContext.Session.GetInt32("CartSession");

            total = (from cartItems in _context.CartItems
                     where cartItems.CartId == cartID
                     select (int?)cartItems.Quantity *
                     cartItems.Dish.Price).Sum();

            return total;
        }

    }
}
