using System.ComponentModel.DataAnnotations;

namespace AlleycatApp.Auth.Models.Dto
{
    public class PointDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Value { get; set; }
        public bool IsPrepared { get; set; }
        public bool IsHidden { get; set; }
        public byte? Order { get; set; }
        public int? RaceId { get; set; }
    }
}
