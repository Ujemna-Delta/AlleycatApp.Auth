using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AlleycatApp.Auth.Models.Users;

namespace AlleycatApp.Auth.Models
{
    public class LeagueScore : IModel<int>
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(8, 2)")] public decimal Score { get; set; }

        [Required] public string AttendeeId { get; set; } = null!;
        public Attendee Attendee { get; set; } = null!;

        public short LeagueId { get; set; }
        public League League { get; set; } = null!;
    }
}
