namespace PapaPizza.Models
{
    public class CartItemIngredient
    {
        public int CartItemId { get; set; }
        public CartItem CartItem { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public decimal Price { get; set; }
        public bool Enabled { get; set; }
    }
}
