using Dto.AuthorModel;
using Models.Models;

namespace Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        Task<ServiceResponse<AddAuthorModel>> AddAuthorAsync(AddAuthorModel authorModel);
        Task<ServiceResponse<GetAuthorModel>> DeleteAuthorAsync(int id);
        Task<ServiceResponse<List<GetAuthorModel>>> GetAllAuthorsAsync();
        Task<ServiceResponse<GetAuthorModel>> GetAuthorByIdAsync(int id);
        Task<ServiceResponse<GetAuthorModel>> UpdateAuthorAsync(int id, AddAuthorModel modifiedAuthor);
    }
}