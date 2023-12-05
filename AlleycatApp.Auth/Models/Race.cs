using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlleycatApp.Auth.Models
{
    public class Race : ICopyable<Race>
    {
        public int Id { get; set; }
        [Required, StringLength(64)] public string Name { get; set; } = null!;
        [StringLength(256)] public string? Description { get; set; }
        [Required] public DateTime BeginTime { get; set; }
        [Required, StringLength(256)] public string StartAddress { get; set; } = null!;
        [Column(TypeName = "decimal(8, 2)")] public decimal? ValueModifier { get; set; }
        [Required] public bool IsActive { get; set; }
        [Required] public bool IsFreeOrder { get; set; }

        public void CopyTo(Race item)
        {
            item.Name = Name;
            item.Description = Description;
            item.BeginTime = BeginTime;
            item.StartAddress = StartAddress;
            item.ValueModifier = ValueModifier;
            item.IsActive = IsActive;
            item.IsFreeOrder = IsFreeOrder;
        }
    }
}
