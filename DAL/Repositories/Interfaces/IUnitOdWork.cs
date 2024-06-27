namespace Data.Repositories.RepositoryInterfaces
{
    public interface IUnitOdWork : IDisposable
    {
        public IMovieRepository MovieRepository { get; set; }
        Task CommitAsync();
    }
}
