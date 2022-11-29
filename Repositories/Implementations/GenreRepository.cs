using AutoMapper;
using Dto.GenreModel;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repositories.Interfaces;
using Validator.Interfaces;

namespace Repositories.Implementations
{
    public class GenreRepository : IGenreRepository
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;
        private readonly IInputValidator _validator;
        public GenreRepository(LibraryContext context, IMapper mapper, IInputValidator validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<ServiceResponse<List<GetGenreModel>>> GetAllGenresAsync()
        {
            try
            {
                var records = await _context.Genres.ToListAsync();

                return new ServiceResponse<List<GetGenreModel>> { Data = _mapper.Map<List<GetGenreModel>>(records) };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetGenreModel>> { Success = false, Message = "Genres wasn't loded." };
            }
        }
        public async Task<ServiceResponse<GetGenreModel>> GetGenreByIdAsync(int id)
        {
            try
            {
                var record = await _context.Genres.Where(g => g.GenreId == id).FirstOrDefaultAsync();

                if (record != null)
                {
                    return new ServiceResponse<GetGenreModel> { Data = _mapper.Map<GetGenreModel>(record) };
                }

                return new ServiceResponse<GetGenreModel> { Success = false, Message = "Genre not found." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetGenreModel> { Success = false, Message = "Genre not found." };
            }
        }
        public async Task<ServiceResponse<AddGenreModel>> AddGenreAsync(AddGenreModel genreModel)
        {
            try
            {
                var errorMssg = _validator.GnereValidation(genreModel.GenreName);

                if (errorMssg != String.Empty)
                {
                    return new ServiceResponse<AddGenreModel> { Success = false, Message = errorMssg };
                }

                var genre = _mapper.Map<Genre>(genreModel);

                _context.Genres.Add(genre);

                return new ServiceResponse<AddGenreModel> { Data = _mapper.Map<AddGenreModel>(genre), Message = "Genre added successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddGenreModel> { Success = false, Message = "Genre wasn't added successfully." };
            }
        }
        public async Task<ServiceResponse<GetGenreModel>> UpdateGenreAsync(int id, AddGenreModel modifiedGenre)
        {
            try
            {
                var errorMssg = _validator.GnereValidation(modifiedGenre.GenreName);

                if (errorMssg != String.Empty)
                {
                    return new ServiceResponse<GetGenreModel> { Success = false, Message = errorMssg };
                }

                var genre = await _context.Genres.Where(g => g.GenreId == id).SingleOrDefaultAsync();

                if (genre == null)
                {
                    return new ServiceResponse<GetGenreModel> { Success = false, Message = "Genre not found." };
                }

                _mapper.Map(modifiedGenre, genre);

                _context.Genres.Update(genre);

                return new ServiceResponse<GetGenreModel> { Data = _mapper.Map<GetGenreModel>(genre), Message = "Genre updated successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetGenreModel> { Success = false, Message = "Genre wasn't updated successfully." };
            }
        }
        public async Task<ServiceResponse<GetGenreModel>> DeleteGenreAsync(int id)
        {
            try
            {
                var genre = await _context.Genres.FindAsync(id);

                if (genre != null)
                {
                    _context.Genres.Remove(genre);

                    return new ServiceResponse<GetGenreModel> { Message = "Genre deleted successfully." };
                }

                return new ServiceResponse<GetGenreModel> { Success = false, Message = "Genre not found." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetGenreModel> { Success = false, Message = "Genre wasn't deleted successfully." };
            }
        }
    }
}
