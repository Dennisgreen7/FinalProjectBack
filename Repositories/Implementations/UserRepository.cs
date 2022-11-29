using AutoMapper;
using Dto.UserModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repositories.Interfaces;
using System.Security.Cryptography;
using Validator.Interfaces;

namespace Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;
        private readonly IInputValidator _validator;

        public UserRepository(LibraryContext context, IMapper mapper, IInputValidator validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<ServiceResponse<List<GetUserModel>>> GetAllUsersAsync(IWebHostEnvironment _environment)
        {
            try
            {
                var records = await _context.Users.ToListAsync();
                var users = _mapper.Map<List<GetUserModel>>(records);
                if (users != null && users.Count > 0)
                {
                    users.ForEach(user =>
                    {
                        user.ImageSrc = GetUserImage(user.UsersId.ToString(), _environment);
                    });
                }

                return new ServiceResponse<List<GetUserModel>> { Data = users };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetUserModel>> { Success = false, Message = "Users wasn't loded." };
            }
        }
        public async Task<ServiceResponse<GetUserModel>> GetUserByIdAsync(int id, IWebHostEnvironment _environment)
        {
            try
            {
                var record = await _context.Users.Where(u => u.UsersId == id).FirstOrDefaultAsync();

                var user = _mapper.Map<GetUserModel>(record);

                if (user != null)
                {
                    user.ImageSrc = GetUserImage(user.UsersId.ToString(), _environment);
                }

                if (record != null)
                {
                    return new ServiceResponse<GetUserModel> { Data = user };
                }

                return new ServiceResponse<GetUserModel> { Success = false, Message = "User not found." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetUserModel> { Success = false, Message = "User not found." };
            }
        }
        public async Task<ServiceResponse<AddUserModel>> AddUsersAsync(AddUserModel userModel)
        {
            try
            {
                var userExsit = await _context.Users.FirstOrDefaultAsync(u => u.UsersUserName == userModel.UsersUserName);

                if (userExsit != null)
                {
                    var isUsernameFree = _validator.UserTaken(userExsit.UsersUserName, "Add");
                    if (_validator.UserTaken(userExsit.UsersUserName, "Add") != string.Empty)
                    {
                        return new ServiceResponse<AddUserModel> { Success = false, Message = "Username is already taken." };
                    }
                }

                var errorMssg = _validator.UserValidation("Add", userModel.UsersUserName, userModel.UsersFirstName, userModel.UsersLastName, userModel.UsersRole, userModel.UsersEmail);

                if (errorMssg != string.Empty)
                {
                    return new ServiceResponse<AddUserModel>
                    {
                        Success = false,
                        Message = errorMssg
                    };
                }

                var user = _mapper.Map<User>(userModel);
                CreatePasswordHash(userModel.UsersPassword, out byte[] passwordHash, out byte[] passwordSalt);
                user.UsersPasswordHash = passwordHash;
                user.UsersPasswordSalt = passwordSalt;
                _context.Users.Add(user);

                return new ServiceResponse<AddUserModel> { Data = userModel, Message = "User added successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddUserModel> { Success = false, Message = "User wasn't added successfully." };
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<ServiceResponse<AddUserModel>> UpdateUserAsync(int id, AddUserModel modifiedUser)
        {
            try
            {
                var userExsit = await _context.Users.FirstOrDefaultAsync(u => u.UsersUserName == modifiedUser.UsersUserName);

                if (userExsit != null)
                {
                    var userId = modifiedUser.UsersId;

                    if (_validator.UserTaken(userExsit.UsersUserName, "Update", userId == null ? default(int) : userId.Value) != string.Empty)
                    {
                        return new ServiceResponse<AddUserModel> { Success = false, Message = "Username is already taken." };
                    }
                }

                var errorMssg = _validator.UserValidation("Update", modifiedUser.UsersUserName, modifiedUser.UsersFirstName, modifiedUser.UsersLastName, modifiedUser.UsersRole, modifiedUser.UsersEmail);

                if (errorMssg != string.Empty)
                {
                    return new ServiceResponse<AddUserModel>
                    {
                        Success = false,
                        Message = errorMssg
                    };
                }

                var user = await _context.Users.Where(u => u.UsersId == id).SingleOrDefaultAsync();

                if (user == null)
                {
                    return new ServiceResponse<AddUserModel> { Success = false, Message = "User not found." };
                }

                _mapper.Map(modifiedUser, user);

                _context.Users.Update(user);

                return new ServiceResponse<AddUserModel> { Data = modifiedUser, Message = "User updated successfully." };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddUserModel> { Success = false, Message = "User wasn't updated successfully." };
            }

        }

        public async Task<ServiceResponse<GetUserModel>> DeleteUserAsync(int id)
        {
            try
            {
                var openBorrow = await _context.Borrowings.FirstOrDefaultAsync(b => b.BorrowingUserId == id && b.BorrowingReturnedDate == null);

                if (openBorrow != null)
                {
                    return new ServiceResponse<GetUserModel> { Success = false, Message = "This User have open Borrowings, close them to delete." };
                }

                var userBorrows = await _context.Borrowings.Where(b => b.BorrowingUserId == id).ToListAsync();
                var user = await _context.Users.FindAsync(id);

                if (user != null && userBorrows != null)
                {
                    for (int i = 0; i < userBorrows.Count; i++)
                    {
                        _context.Borrowings.Remove(userBorrows[i]);
                    }
                    _context.Users.Remove(user);

                    return new ServiceResponse<GetUserModel> { Message = "User deleted successfully." };
                }

                return new ServiceResponse<GetUserModel> { Success = false, Message = "User not found." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetUserModel> { Success = false, Message = "User wasn't deleted successfully." };
            }
        }

        public async Task<ServiceResponse<bool>> UploadImg(List<IFormFile> files, IWebHostEnvironment _environment)
        {
            try
            {
                foreach (IFormFile source in files)
                {
                    string fileName = source.FileName;
                    string filePath = GetFilePath(fileName.Substring(0, fileName.LastIndexOf(".")), _environment);

                    if (!System.IO.Directory.Exists(filePath))
                    {
                        System.IO.Directory.CreateDirectory(filePath);
                    }
                    else
                    {
                        return new ServiceResponse<bool> { Success = false, Message = "Img wasn't uploaded succesfully." };
                    }
                    string imagePath = filePath + "\\image.png";

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await source.CopyToAsync(stream);
                    }
                }
                return new ServiceResponse<bool> { Message = "Img uploaded succesfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = "Img wasn't uploaded succesfully." };
            }
        }

        private string GetFilePath(string userCode, IWebHostEnvironment _environment)
        {
            return _environment.WebRootPath + "\\Uploads\\Users\\" + userCode;
        }

        public string GetUserImage(string userCode, IWebHostEnvironment _environment)
        {
            try
            {
                string imageUrl = string.Empty;
                string hostUrl = "https://localhost:7034/";
                string filePath = GetFilePath(userCode, _environment);
                string imagePath = filePath + "\\image.png";

                if (!System.IO.File.Exists(imagePath))
                {
                    imageUrl = hostUrl + "/Uploads/common//noimage.png";
                }
                else
                {
                    imageUrl = hostUrl + "/Uploads/Users/" + userCode + "/image.png";
                }
                return imageUrl;
            }
            catch
            {
                return "https://localhost:7034//Uploads/common//noimage.png";
            }
        }

        public async Task<ServiceResponse<string>> RemoveUserImage(string userCode, IWebHostEnvironment _environment)
        {
            try
            {
                string filePath = GetFilePath(userCode, _environment);
                string imagePath = filePath + "\\image.png";

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                return new ServiceResponse<string> { Data = imagePath };
            }
            catch
            {
                return new ServiceResponse<string> { Data = "https://localhost:7034//Uploads/common//noimage.png" };
            }
        }
    }
}
