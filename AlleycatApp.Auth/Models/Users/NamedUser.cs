using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AlleycatApp.Auth.Models.Users
{
    public class NamedUser : IdentityUser
    {
        [StringLength(128)] public string FirstName { get; set; } = string.Empty;
        [StringLength(128)] public string LastName { get; set; } = string.Empty;
    }
}
