using System.ComponentModel.DataAnnotations;

namespace AlleycatApp.Auth.Models
{
    public class TaskModel : IModel<int>
    {
        public int Id { get; set; }
        [Required, StringLength(64)] public string Name { get; set; } = string.Empty;
        [StringLength(256)] public string? Description { get; set; }
        [Range(0, 999999)] public int Value { get; set; }

        public int? PointId { get; set; }
        public Point? Point { get; set; }
    }
}
