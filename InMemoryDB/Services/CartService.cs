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
        private readonly string _shoppingCartId;


        public CartService(ApplicationDbContext context, string id )
        {
            _context = context;
            _shoppingCartId = id;
        }

        public CartService GetCart(ApplicationDbContext db, HttpContext httpContext)
                => GetCart(db, GetCartId(httpContext));

        public static CartService GetCart(ApplicationDbContext db, string cartId)
           => new CartService(db, cartId);



        // We're using HttpContextBase to allow access to sessions.
        public string GetCartId(HttpContext httpContext)
        {
            var cartId = httpContext.Session.GetString("Session");

            if (cartId == null)
            {
                //A GUID to hold the cartId. 
                cartId = Guid.NewGuid().ToString();

                // Send cart Id as a cookie to the client.
                httpContext.Session.SetString("Session", cartId);
            }

            return cartId;
        }



    }
}
