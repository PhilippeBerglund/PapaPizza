using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaPizza.Models
{
    public class CartItemIngredient
    {
        //[JsonIgnore]
        public int CartItemId { get; set; }
        public CartItem CartItem { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
            
        public bool Enabled { get; set; }
    }
}
