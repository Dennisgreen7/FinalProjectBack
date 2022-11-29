using Dto.AuthModel;
using Dto.UserModel;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUnitOfWork UnitOfWork { get; }
        public AuthController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<ActionResult<GetUserModel>> Register(RegistrationUser user)
        {
            var response = await UnitOfWork.AuthRepository.Register(user);

            if (response.Success == false)
            {
                return Unauthorized(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLogin userLogin)
        {
            var response = await UnitOfWork.AuthRepository.Login(userLogin);

            if (response.Success == false)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<ServiceResponse<ActiveUser>>> GetUser()
        {
            var response = UnitOfWork.AuthRepository.GetCurrentUser();

            return Ok(response);
        }
    }
}
