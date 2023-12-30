using System.ComponentModel.DataAnnotations;

namespace AlleycatApp.Auth.Models.Users
{
    public class Attendee : NamedUser
    {
        [StringLength(64)] public string Nickname { get; set; } = string.Empty;
        [StringLength(256)] public string? Marks { get; set; }
    }
}
