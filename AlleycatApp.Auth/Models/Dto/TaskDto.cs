namespace AlleycatApp.Auth.Models.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Value { get; set; }
        public int? PointId { get; set; }
    }
}
