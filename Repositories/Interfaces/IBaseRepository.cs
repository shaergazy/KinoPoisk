namespace Repositories.Interfaces;


public interface IBaseRepository <T>
{
    public IEnumerable<T> GetAll();
    public Task<T> GetById(T Id);
    public Task<T> Create(T model, bool commitTransaction);
    public Task<T> Delete(T model);
    public Task<T> Update(T model);
    public Task<int> SaveChangesAsync();
}