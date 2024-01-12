namespace AlleycatApp.Auth.Models.Dto
{
    public class PointOrderOverrideDto
    {
        public int Id { get; set; }
        public byte Order { get; set; }
        public string AttendeeId { get; set; } = null!;
        public int PointId { get; set; }
    }
}
