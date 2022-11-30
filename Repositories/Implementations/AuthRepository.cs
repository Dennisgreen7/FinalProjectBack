using AutoMapper;
using Dto.AuthModel;
using Dto.UserModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Validator.Interfaces;

namespace Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IInputValidator _validator;
        public AuthRepository(LibraryContext context, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IInputValidator validator)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }

        public async Task<ServiceResponse<GetUserModel>> Register(RegistrationUser newUser)
        {
            try
            {
                var userExsit = _validator.UserTaken(newUser.UsersUserName, "Registration");

                if (userExsit != String.Empty)
                {
                    return new ServiceResponse<GetUserModel> { Success = false, Message = "Username is already taken." };
                }

                var user = _mapper.Map<User>(newUser);

                CreatePasswordHash(newUser.UsersPassword, out byte[] passwordHash, out byte[] passwordSalt);

                user.UsersUserName = newUser.UsersUserName;
                user.UsersPasswordHash = passwordHash;
                user.UsersPasswordSalt = passwordSalt;
                if (user.UsersUserName == "Admin") {
                    user.UsersRole = "Admin";
                }
                else
                {
                    user.UsersRole = "Client";
                }
                

                var errorMssg = _validator.UserValidation("Registration", newUser.UsersUserName, newUser.UsersFirstName, newUser.UsersLastName, user.UsersRole, newUser.UsersEmail, newUser.UsersPassword);

                if (errorMssg != string.Empty)
                {
                    return new ServiceResponse<GetUserModel>
                    {
                        Success = false,
                        Message = errorMssg
                    };
                }



                _context.Add(user);

                return new ServiceResponse<GetUserModel> { Data = _mapper.Map<GetUserModel>(user), Message = "User successfully registered." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetUserModel> { Success = false, Message = "User wasn't registered." };
            }

        }

        public async Task<ServiceResponse<string>> Login(UserLogin userLogin)
        {
            try
            {
                var user = await this._context.Users.Where(user => user.UsersUserName == userLogin.UsersUserName).FirstOrDefaultAsync();

                if (user == null)
                {
                    return new ServiceResponse<string> { Success = false, Message = "You have entered an invalid username or password." };
                }

                if (!VerifyPasswordHash(userLogin.UsersPassword, user.UsersPasswordHash, user.UsersPasswordSalt))
                {
                    return new ServiceResponse<string> { Success = false, Message = "You have entered an invalid username or password." };
                }

                return new ServiceResponse<string> { Data = CreateToken(user), Message = "User logged successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string> { Success = false, Message = "User wasn't logged successfully." };
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
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UsersFirstName+" "+user.UsersLastName),
                new Claim(ClaimTypes.Role, user.UsersRole),
                new Claim(ClaimTypes.UserData,user.UsersId.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        public async Task<ServiceResponse<ActiveUser>> GetCurrentUser()
        {
            try
            {
                var result = string.Empty;
                ActiveUser logedUser = new ActiveUser();
                if (_httpContextAccessor.HttpContext != null)
                {
                    logedUser.UsersFullName = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                    logedUser.UsersRole = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
                    logedUser.UsersId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.UserData);
                }

                return new ServiceResponse<ActiveUser> { Data = logedUser };
            }
            catch
            {
                return new ServiceResponse<ActiveUser> { Success = false };
            }
        }
    }
}
