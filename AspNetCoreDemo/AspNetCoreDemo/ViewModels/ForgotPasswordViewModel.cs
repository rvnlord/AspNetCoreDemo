using System.ComponentModel.DataAnnotations;

namespace AspNetCoreDemo.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
