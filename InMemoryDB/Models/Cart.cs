using System.Collections.Generic;

namespace PapaPizza.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ApplicationUserId { get; set; }  
        public List<CartItem> CartItems { get; set; }
    }

}
