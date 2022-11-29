using Dto.BookModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Models.Models;

namespace Repositories.Interfaces
{
    public interface IBookRepository
    {
        Task<ServiceResponse<AddBookModel>> AddBookAsync(AddBookModel bookModel);
        Task<ServiceResponse<GetBookModel>> DeleteBookAsync(int id);
        Task<ServiceResponse<List<GetBookModel>>> GetAllBooksAsync(IWebHostEnvironment _environment);
        Task<ServiceResponse<List<GetBookModel>>> GetAllBooksForBorrowAsync(IWebHostEnvironment _environment);
        Task<ServiceResponse<GetBookModel>> GetBookByIdAsync(int id, IWebHostEnvironment _environment);
        Task<ServiceResponse<GetBookModel>> UpdateBookAsync(int id, AddBookModel modifiedBook);
        Task<ServiceResponse<bool>> UploadImg(List<IFormFile> files, IWebHostEnvironment _environment);
        string GetBookImage(string bookCode, IWebHostEnvironment _environment);
        Task<ServiceResponse<string>> RemoveBookImage(string bookCode, IWebHostEnvironment _environment);
        Task<ServiceResponse<List<GetBookModel>>> FilteBook(int filterIndex, string searchValue, IWebHostEnvironment _environment);
    }
}