using System.Collections.Generic;

namespace PapaPizza.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<Dish> Dishes { get; set; } 
    }
}
