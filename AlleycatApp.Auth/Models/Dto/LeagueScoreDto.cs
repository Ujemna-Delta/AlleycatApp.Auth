namespace AlleycatApp.Auth.Models.Dto
{
    public class LeagueScoreDto
    {
        public int Id { get; set; }
        public decimal Score { get; set; }
        public string AttendeeId { get; set; } = null!;
        public short LeagueId { get; set; }
    }
}
