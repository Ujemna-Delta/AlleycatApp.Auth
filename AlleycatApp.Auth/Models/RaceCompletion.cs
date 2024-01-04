using System.ComponentModel.DataAnnotations;
using AlleycatApp.Auth.Models.Users;

namespace AlleycatApp.Auth.Models
{
    public class RaceCompletion : IModel<int>
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public bool HasWithdrawn { get; set; }
        [Range(0, 999999)] public int Score { get; set; }

        public string AttendeeId { get; set; } = null!;
        public Attendee Attendee { get; set; } = null!;

        public int RaceId { get; set; }
        public Race Race { get; set; } = null!;
    }
}
