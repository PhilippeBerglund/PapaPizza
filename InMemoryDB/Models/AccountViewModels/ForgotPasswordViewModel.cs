using System.ComponentModel.DataAnnotations;

namespace PapaPizza.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
