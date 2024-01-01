using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Models.Users
{
    public class NamedUser : IdentityUser
    {
        [Required, StringLength(128)] public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(128)] public string LastName { get; set; } = string.Empty;
    }
}
