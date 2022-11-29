using Dto.AuthModel;
using Dto.UserModel;
using Models.Models;

namespace Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<string>> Login(UserLogin userLogin);
        Task<ServiceResponse<GetUserModel>> Register(RegistrationUser newUser);
        Task<ServiceResponse<ActiveUser>> GetCurrentUser();
    }
}