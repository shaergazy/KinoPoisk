using DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class TranslatableEntityField
    {
        public int Id { get; set; }
        public Guid TranslatableEntityId { get; set; }
        public TranslatableEntity TranslatableEntity { get; set; }
        public LanguageCode LanguageCode { get; set; }
        public TranslatableFieldType FieldType { get; set; }
        public string Value { get; set; }
    }
}
