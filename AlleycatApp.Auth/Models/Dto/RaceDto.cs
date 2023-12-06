namespace AlleycatApp.Auth.Models.Dto
{
    public class RaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime BeginTime { get; set; }
        public string StartAddress { get; set; } = null!;
        public decimal? ValueModifier { get; set; }
        public bool IsActive { get; set; }
        public bool IsFreeOrder { get; set; }
    }
}
