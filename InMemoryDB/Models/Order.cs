using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaPizza.Models
{
    public class Order
    {
        public int id { get; set; }
        public ApplicationUser User { get; set; }
        public int ApplicationUserId { get; set; }

        public Cart MyCart { get; set; }
        public int CartId { get; set; }

       // public List<CartItem> CartItems { get; set; }
    }
}
