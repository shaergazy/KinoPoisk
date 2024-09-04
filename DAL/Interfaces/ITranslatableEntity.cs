using DAL.Models;

namespace DAL.Interfaces
{
    public interface ITranslatableEntity
    {
        ICollection<TranslatableEntityField> Translations { get; }
    }
}
