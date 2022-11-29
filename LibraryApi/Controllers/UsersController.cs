using Dto.UserModel;


namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IUnitOfWork UnitOfWork { get; }
        private readonly IWebHostEnvironment _environment;
        public UsersController(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            UnitOfWork = unitOfWork;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetUserModel>>> GetUsers()
        {
            var response = await UnitOfWork.UserRepository.GetAllUsersAsync(_environment);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserModel>> GetUser([FromRoute] int id)
        {
            var response = await UnitOfWork.UserRepository.GetUserByIdAsync(id, _environment);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<AddUserModel>> AddUser(AddUserModel user)
        {
            var response = await UnitOfWork.UserRepository.AddUsersAsync(user);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] AddUserModel user)
        {
            var response = await UnitOfWork.UserRepository.UpdateUserAsync(id, user);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var response = await UnitOfWork.UserRepository.DeleteUserAsync(id);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }

        [HttpPost("UploadImage")]
        public async Task<ActionResult> UploadImage()
        {
            var uploadedFiles = Request.Form.Files.ToList();
            var response = await UnitOfWork.UserRepository.UploadImg(uploadedFiles, _environment);
            if (response.Success == false)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
