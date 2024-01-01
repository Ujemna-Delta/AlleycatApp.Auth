using System.ComponentModel.DataAnnotations;

namespace AlleycatApp.Auth.Models
{
    public class League : IModel<short>
    {
        public short Id { get; set; }
        [Required, StringLength(64)] public string Name { get; set; } = string.Empty;
        [StringLength(256)] public string? Description { get; set; }
    }
}
