using System.ComponentModel.DataAnnotations;

namespace PapaPizza.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Street No")]
        public string StreetNo { get; set; }

        [Required]
        [Display(Name = "Zip code")]
        public string Zip { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [CreditCard]
        [Display(Name = "Card No")]
        public string CardNo { get; set; }
    }
}
