//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using BLL.Services.Interfaces;
//using DAL.Models;
//using Data.Repositories.RepositoryInterfaces;
//using Microsoft.EntityFrameworkCore;
//using BLL.DTO.Genre;

//namespace BLL.Services.Implementation
//{
//    public class GenreService : GenericService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, int>, IGenreService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, int>
//    {
//        public GenreService(IMapper mapper, IUnitOfWork<Genre, int> unitOfWork) : base(mapper, unitOfWork) { }
//        public GenreService()
//        {

//        }

//        public Task DeleteById(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<List<ListGenreDto>> GetAll()
//        {
//            throw new NotImplementedException();
//        }

//        public Task<GetGenreDto> GetById(int id)
//        {
//            throw new NotImplementedException();
//        }

//        Task<int> IGenreService<ListGenreDto, AddGenreDto, EditGenreDto, GetGenreDto, Genre, int>.CreateAsync(AddGenreDto dto)
//        {
//            throw new NotImplementedException();
//        }

//        public GenreService()
//        {

//        }

//        public async Task<int> CreateAsync(AddGenreDto dto)
//        {
//            if (dto == null)
//                throw new ArgumentNullException();
//            if (_uow.Genres.Any(x => x.Name == dto.Name))
//                throw new Exception("Genre already exist");
//            var genre = _mapper.Map<Genre>(dto);
//            await _uow.Genres.AddAsync(genre, true);
//            return genre.Id;
//        }

//        public async Task DeleteById(int id)
//        {
//            if (_uow.Movies.Any(x => x.Genres.Any(g => g.Id == id)))
//                throw new Exception("There are movies in this genre, so you won't be able to delete it.");
//            var genre = await _uow.Genres.GetByIdAsync(id);
//            if (genre == null)
//                throw new Exception($"Genre with id: {id} does not exist");
//            await _uow.Genres.DeleteByIdAsync(id);
//        }

//        public async Task UpdateAsync(EditGenreDto dto)
//        {
//            if (dto == null)
//                throw new ArgumentNullException();
//            if (_uow.Genres.Any(x => x.Name == dto.Name && x.Id != dto.Id))
//                throw new Exception("Genre already exist");
//            var genreToUpdate = _mapper.Map<Genre>(dto);
//            await _uow.Genres.UpdateAsync(genreToUpdate);
//        }

//        public async Task<List<ListGenreDto>> GetAll()
//        {
//            return _mapper.Map<List<ListGenreDto>>(_uow.Genres.GetAll().ToListAsync().Result);
//        }

//        public async Task<GetGenreDto> GetById(int id)
//        {
//            return _mapper.Map<GetGenreDto>(await _uow.Genres.GetByIdAsync(id));
//        }
//    }
//}
