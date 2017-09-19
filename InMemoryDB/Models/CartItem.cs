using System.Collections.Generic;

namespace PapaPizza.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }
        public Dish Dish { get; set; }
        public int DishId { get; set; }

        public int Quantity { get; set; }

        public List<CartItemIngredient> CartItemIngredients { get; set; }
    }
}