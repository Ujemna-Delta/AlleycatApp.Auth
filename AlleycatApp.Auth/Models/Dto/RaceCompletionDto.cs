namespace AlleycatApp.Auth.Models.Dto
{
    public class RaceCompletionDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public bool HasWithdrawn { get; set; }
        public decimal Score { get; set; }
        public string AttendeeId { get; set; } = string.Empty;
        public int RaceId { get; set; }
    }
}
