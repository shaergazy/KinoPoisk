namespace Repositories.Interfaces
{
    public interface IUnitOdWork : IDisposable
    {
        Task Commit();
    }
}
