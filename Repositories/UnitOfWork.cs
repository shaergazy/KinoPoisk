﻿using DAL;
using Repositories.Interfaces;

namespace Repositories
{
    public class UnitOfWork : IUnitOdWork
    {
        private readonly AppDbContext _appDbContext;
        public MovieRepository MovieRepository { get; private set; }

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            MovieRepository = new MovieRepository(_appDbContext);
        }

        public async Task Commit()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}