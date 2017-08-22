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
        public int Price { get; set; }
        public List<DishIngredient> DishIngredients { get; set; } 
    }
}
