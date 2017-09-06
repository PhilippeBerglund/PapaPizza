using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PapaPizza.Models
{

    public class DishIngredient
    {
        public int DishId { get; set; }
        [JsonIgnore]
        public Dish Dish { get; set; }
        public int IngredientId { get; set; }
        [JsonIgnore]
        public Ingredient Ingredient { get; set; }

        public bool checkboxAnswer { get; set; }


    }
}
