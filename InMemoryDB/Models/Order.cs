namespace PapaPizza.Models
{
    public class Order
    {
        public int id { get; set; }
        public ApplicationUser User { get; set; }
        public string ApplicationUserId { get; set; }

        public Cart MyCart { get; set; }
        public int? CartId { get; set; }
    }
}
