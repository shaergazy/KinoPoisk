using DAL.Enums;

namespace DAL.Models
{
    public class TranslatableEntityField
    {
        public int Id { get; set; }
        public int TranslatableEntityId { get; set; }
        public TranslatableEntity TranslatableEntity { get; set; }
        public LanguageCode LanguageCode { get; set; }
        public TranslatableFieldType FieldType { get; set; }
        public string Value { get; set; }
    }
}
