using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.DTO;
using BLL.Services.Interfaces;
using Common.Exceptions;
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

        public async Task<int> Create(GenreDto.Base dto)
        {
            if (dto == null)
                throw new ArgumentNullException();
            if (_uow.Genres.Any(x => x.Name == dto.Name))
                throw new InnerException("Genre already exist");
            var genre = _mapper.Map<Genre>(dto);
            await _uow.Genres.CreateAsync(genre, true);
            return genre.Id;
        }

        public async Task DeleteById(int id)
        {
            if (_uow.Movies.Any(x => x.Genres.Any(g => g.Id == id)))
                throw new InnerException("There are movies in this genre, so you won't be able to delete it.");
            //var genre = await _uow.Genres.GetByIdAsync(id);
            //if (genre == null)
            //    throw new InnerException($"Genre with id: {id} does not exist");
            await _uow.Genres.DeleteAsync(id);
        }

        public async Task EditById(GenreDto.IdHasBase dto)
        {
            var genreToUpdate = _mapper.Map<Genre>(dto);
            await _uow.Genres.UpdateAsync(genreToUpdate);
        }

        public Task<List<GenreDto.IdHasBase>> GetAll()
        {
            return _uow.Genres.GetAll().ProjectTo<GenreDto.IdHasBase>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<GenreDto.IdHasBase> GetById(int id)
        {
            var genre = await _uow.Genres.GetByIdAsync(id);
            return _mapper.Map<GenreDto.IdHasBase>(genre);
        }
    }
}
