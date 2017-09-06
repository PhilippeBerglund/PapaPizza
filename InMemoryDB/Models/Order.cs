using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaPizza.Models
{
    public class Order
    {
        public int id { get; set; }
        public List<CartItem> CartItems { get; set; }

    }
}
