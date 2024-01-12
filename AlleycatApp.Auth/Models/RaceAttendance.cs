using System.ComponentModel.DataAnnotations;
using AlleycatApp.Auth.Models.Users;

namespace AlleycatApp.Auth.Models
{
    public class RaceAttendance : IModel<int>
    {
        public int Id { get; set; }
        public bool IsConfirmed { get; set; }

        [Required] public string AttendeeId { get; set; } = null!;
        public Attendee Attendee { get; set; } = null!;

        public int RaceId { get; set; }
        public Race Race { get; set; } = null!;
    }
}
