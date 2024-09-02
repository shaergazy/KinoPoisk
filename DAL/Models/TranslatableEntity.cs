using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class TranslatableEntity
    {
        [Key]
        public int Id { get; set; }
        public ICollection<TranslatableEntityField> Translations { get; set; } = new List<TranslatableEntityField>();
    }
}
