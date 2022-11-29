using Dto.AuthorModel;


namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private IUnitOfWork UnitOfWork { get; }
        public AuthorsController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetAuthorModel>>>> GetAuthor()
        {
            var response = await UnitOfWork.AuthorRepository.GetAllAuthorsAsync();

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetAuthorModel>>> GetAuthor([FromRoute] int id)
        {
            var response = await UnitOfWork.AuthorRepository.GetAuthorByIdAsync(id);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<AddAuthorModel>>> AddAuthor([FromBody] AddAuthorModel author)
        {
            var response = await UnitOfWork.AuthorRepository.AddAuthorAsync(author);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<GetAuthorModel>>> UpdateAuthor([FromRoute] int id, [FromBody] AddAuthorModel author)
        {
            var response = await UnitOfWork.AuthorRepository.UpdateAuthorAsync(id, author);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<GetAuthorModel>>> DeleteAuthor([FromRoute] int id)
        {
            var response = await UnitOfWork.AuthorRepository.DeleteAuthorAsync(id);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }
    }
}

