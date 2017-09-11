using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaPizza.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        //// phbe04
        public CartItem CartItem { get; set; }
        //public int CartItemId { get; set; }

        public List<DishIngredient> DishIngredients { get; set; }


    }
}
