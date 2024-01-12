using System.ComponentModel.DataAnnotations;
using AlleycatApp.Auth.Models.Users;

namespace AlleycatApp.Auth.Models
{
    public class PointOrderOverride : IModel<int>
    {
        public int Id { get; set; }
        public byte Order { get; set; }

        [Required] public string AttendeeId { get; set; } = null!;
        public Attendee Attendee { get; set; } = null!;

        public int PointId { get; set; }
        public Point Point { get; set; } = null!;
    }
}
