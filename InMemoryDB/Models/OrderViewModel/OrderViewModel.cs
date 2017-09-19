using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaPizza.Models.OrderViewModel
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public Cart Cart { get; set; }
        public ApplicationUser UserVM { get; set; }
    }
}
