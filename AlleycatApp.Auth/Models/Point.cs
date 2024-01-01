using System.ComponentModel.DataAnnotations;

namespace AlleycatApp.Auth.Models
{
    public class Point : IModel<int>
    {
        public int Id { get; set; }
        [Required, StringLength(64)] public string Name { get; set; } = string.Empty;
        [Required, StringLength(256)] public string Address { get; set; } = string.Empty;
        [Range(0, 999999)] public int Value { get; set; }
        public bool IsPrepared { get; set; }
        public bool IsHidden { get; set; }
        public byte? Order { get; set; }

        public int? RaceId { get; set; }
        public Race? Race { get; set; }
    }
}
