using Dto.UserModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Models.Models;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ServiceResponse<GetUserModel>> DeleteUserAsync(int id);
        Task<ServiceResponse<List<GetUserModel>>> GetAllUsersAsync(IWebHostEnvironment _environment);
        Task<ServiceResponse<GetUserModel>> GetUserByIdAsync(int id, IWebHostEnvironment _environment);
        Task<ServiceResponse<AddUserModel>> UpdateUserAsync(int id, AddUserModel modifiedUser);
        Task<ServiceResponse<AddUserModel>> AddUsersAsync(AddUserModel userModel);
        Task<ServiceResponse<bool>> UploadImg(List<IFormFile> files, IWebHostEnvironment _environment);
        string GetUserImage(string userCode, IWebHostEnvironment _environment);
        Task<ServiceResponse<string>> RemoveUserImage(string userCode, IWebHostEnvironment _environment);
    }
}