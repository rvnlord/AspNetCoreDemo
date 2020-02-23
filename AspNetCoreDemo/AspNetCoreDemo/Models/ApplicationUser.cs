using Microsoft.AspNetCore.Identity;

namespace AspNetCoreDemo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}