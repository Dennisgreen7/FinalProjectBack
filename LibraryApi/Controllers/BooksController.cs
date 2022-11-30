using Dto.BookModel;


namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private IUnitOfWork UnitOfWork { get; }
        private readonly IWebHostEnvironment _environment;
        public BooksController(IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            UnitOfWork = unitOfWork;
            _environment = environment;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetBookModel>>>> GetBooks()
        {
            var response = await UnitOfWork.BookRepository.GetAllBooksAsync(_environment);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("BooksForBorrow"), AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<List<GetBookModel>>>> GetBooksForBorrow()
        {
            var response = await UnitOfWork.BookRepository.GetAllBooksForBorrowAsync(_environment);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        [HttpGet("{id}"), AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<GetBookModel>>> GetBook([FromRoute] int id)
        {
            var response = await UnitOfWork.BookRepository.GetBookByIdAsync(id, _environment);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("FilterBooks/{filterIndex}"), AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<List<GetBookModel>>>> FilterBooks([FromRoute] int filterIndex, [FromQuery] string searchValue)
        {
            var response = await UnitOfWork.BookRepository.FilteBook(filterIndex, searchValue, _environment);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<AddBookModel>>> AddBook([FromBody] AddBookModel book)
        {
            var response = await UnitOfWork.BookRepository.AddBookAsync(book);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<AddBookModel>>> UpdateBook([FromRoute] int id, [FromBody] AddBookModel book)
        {
            var response = await UnitOfWork.BookRepository.UpdateBookAsync(id, book);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<GetBookModel>>> DeleteBook([FromRoute] int id)
        {
            var response = await UnitOfWork.BookRepository.DeleteBookAsync(id);

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
            var response = await UnitOfWork.BookRepository.UploadImg(uploadedFiles, _environment);
            if (response.Success == false)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

    }

}

