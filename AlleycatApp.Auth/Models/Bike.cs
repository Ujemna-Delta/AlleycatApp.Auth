using System.ComponentModel.DataAnnotations;
using AlleycatApp.Auth.Models.Users;

namespace AlleycatApp.Auth.Models
{
    public class Bike : IModel<int>
    {
        public int Id { get; set; }
        [Required, StringLength(64)] public string Name { get; set; } = string.Empty;

        [Required] public string AttendeeId { get; set; } = null!;
        public Attendee Attendee { get; set; } = null!;
    }
}
