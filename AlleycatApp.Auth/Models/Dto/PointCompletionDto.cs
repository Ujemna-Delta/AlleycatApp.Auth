namespace AlleycatApp.Auth.Models.Dto
{
    public class PointCompletionDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string AttendeeId { get; set; } = string.Empty;
        public int PointId { get; set; }
    }
}
