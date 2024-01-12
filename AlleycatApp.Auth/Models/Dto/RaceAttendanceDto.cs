namespace AlleycatApp.Auth.Models.Dto
{
    public class RaceAttendanceDto
    {
        public int Id { get; set; }
        public bool IsConfirmed { get; set; }
        public string AttendeeId { get; set; } = null!;
        public int RaceId { get; set; }
    }
}
