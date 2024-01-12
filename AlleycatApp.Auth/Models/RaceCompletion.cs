using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlleycatApp.Auth.Models.Users;

namespace AlleycatApp.Auth.Models
{
    public class RaceCompletion : IModel<int>
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public bool HasWithdrawn { get; set; }
        [Column(TypeName = "decimal(8, 2)")] public decimal Score { get; set; }

        [Required] public string AttendeeId { get; set; } = null!;
        public Attendee Attendee { get; set; } = null!;

        public int RaceId { get; set; }
        public Race Race { get; set; } = null!;
    }
}
