using DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class TranslatableEntity : ITranslatableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public ICollection<TranslatableEntityField> Translations { get; set; } = new List<TranslatableEntityField>();
    }
}
