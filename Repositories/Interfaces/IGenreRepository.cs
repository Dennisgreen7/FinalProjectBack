using Dto.GenreModel;
using Models.Models;

namespace Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task<ServiceResponse<AddGenreModel>> AddGenreAsync(AddGenreModel genreModel);
        Task<ServiceResponse<GetGenreModel>> DeleteGenreAsync(int id);
        Task<ServiceResponse<List<GetGenreModel>>> GetAllGenresAsync();
        Task<ServiceResponse<GetGenreModel>> GetGenreByIdAsync(int id);
        Task<ServiceResponse<GetGenreModel>> UpdateGenreAsync(int id, AddGenreModel modifiedGenre);
    }
}