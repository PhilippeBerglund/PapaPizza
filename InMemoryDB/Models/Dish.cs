using System.Collections.Generic;

namespace PapaPizza.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
    }
}
