using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.Services.Interfaces;
using BLL.DTO;
using DAL.Entities;
using Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public GenreService(IMapper mapper, IUnitOfWork uow)
        { 
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<int> CreateAsync(GenreDto.Base dto)
        {
            if (dto == null)
                throw new ArgumentNullException();
            if (_uow.Genres.Any(x => x.Name == dto.Name))
                throw new Exception("Genre already exist");
            var genre = _mapper.Map<Genre>(dto);
            await _uow.Genres.AddAsync(genre, true);
            return genre.Id;
        }

        public async Task DeleteById(int id)
        {
            if (_uow.Movies.Any(x => x.Genres.Any(g => g.Id == id)))
                throw new Exception("There are movies in this genre, so you won't be able to delete it.");
            var genre = await _uow.Genres.GetByIdAsync(id);
            if (genre == null)
                throw new Exception($"Genre with id: {id} does not exist");
            await _uow.Genres.DeleteByIdAsync(id);
        }

        public async Task UpdateAsync(GenreDto.IdHasBase dto)
        {
            if (dto == null)
                throw new ArgumentNullException();
            if (_uow.Genres.Any(x => x.Name == dto.Name && x.Id != dto.Id))
                throw new Exception("Genre already exist");
            var genreToUpdate = _mapper.Map<Genre>(dto);
            await _uow.Genres.UpdateAsync(genreToUpdate);
        }

        public Task<List<GenreDto.IdHasBase>> GetAll()
        {
            return _uow.Genres.GetAll().ProjectTo<GenreDto.IdHasBase>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<GenreDto.IdHasBase> GetById(int id)
        {
            return _mapper.Map<GenreDto.IdHasBase>(await _uow.Genres.GetByIdAsync(id));
        }
    }
}
