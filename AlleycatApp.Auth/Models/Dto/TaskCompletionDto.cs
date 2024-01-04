namespace AlleycatApp.Auth.Models.Dto
{
    public class TaskCompletionDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string AttendeeId { get; set; } = string.Empty;
        public int TaskId { get; set; }
    }
}
