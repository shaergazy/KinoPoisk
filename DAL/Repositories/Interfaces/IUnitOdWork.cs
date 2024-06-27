namespace Data.Repositories.RepositoryInterfaces
{
    public interface IUnitOdWork : IDisposable
    {
        Task CommitAsync();
    }
}
