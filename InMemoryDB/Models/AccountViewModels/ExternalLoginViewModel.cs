using System.ComponentModel.DataAnnotations;

namespace PapaPizza.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
