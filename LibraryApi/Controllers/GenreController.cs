using Dto.GenreModel;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GenreController : ControllerBase
    {
        private IUnitOfWork UnitOfWork { get; }
        public GenreController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetGenreModel>>>> GetGenre()
        {
            var response = await UnitOfWork.GenreRepository.GetAllGenresAsync();

            if (response.Success == false)
            {
                return NotFound(response);
            }
            
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetGenreModel>>> GetGenre([FromRoute] int id)
        {
            var response = await UnitOfWork.GenreRepository.GetGenreByIdAsync(id);
            
            if (response.Success == false)
            {
                return NotFound(response);
            }
            
            return Ok(response);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<AddGenreModel>>> AddGenre([FromBody] AddGenreModel genre)
        {
            var response = await UnitOfWork.GenreRepository.AddGenreAsync(genre);
            
            if (response.Success == false)
            {
                return BadRequest(response);
            }
            
            await UnitOfWork.Save();
            
            return Ok(response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<GetGenreModel>>> UpdateGenre([FromRoute] int id, [FromBody] AddGenreModel genre)
        {
            var response = await UnitOfWork.GenreRepository.UpdateGenreAsync(id, genre);
            
            if (response.Success == false)
            {
                return NotFound(response);
            }
            
            await UnitOfWork.Save();
            
            return Ok(response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<GetGenreModel>>> DeleteGenre([FromRoute] int id)
        {
            var response = await UnitOfWork.GenreRepository.DeleteGenreAsync(id);
            
            if (response.Success == false)
            {
                return NotFound(response);
            }
            
            await UnitOfWork.Save();
            
            return Ok(response);
        }
    }
}
