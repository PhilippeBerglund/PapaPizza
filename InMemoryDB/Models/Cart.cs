using PapaPizza.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaPizza.Models
{
    public class Cart
    {

        //private readonly ApplicationDbContext _context;
        //public Cart(ApplicationDbContext applicationDbContext)
        //{
        //    _context = applicationDbContext;
        //}

        public int CartId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }  
        public List<CartItem> CartItems { get; set; }

        public Dish Dish { get; set; }

    }

}
