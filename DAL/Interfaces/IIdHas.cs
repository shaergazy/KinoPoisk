namespace DAL.Interfaces;

public interface IIdHas<TKey>
{
    TKey Id { get; set; }
}
