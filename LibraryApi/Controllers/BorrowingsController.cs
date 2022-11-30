using Dto.BorrowModel;
using Microsoft.AspNetCore.JsonPatch;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BorrowingsController : ControllerBase
    {
        private IUnitOfWork UnitOfWork { get; }
        public BorrowingsController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetBorrowModel>>>> GetBorrow()
        {
            var response = await UnitOfWork.BorrowRepository.GetAllBorrowsAsync();

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetBorrowModel>>> GetBorrow([FromRoute] int id)
        {
            var response = await UnitOfWork.BorrowRepository.GetBorrowByIdAsync(id);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("ClientBorrows/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetBorrowModel>>>> GetClientBorrows([FromRoute] int id)
        {
            var response = await UnitOfWork.BorrowRepository.GetBorrowByUserAsync(id);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<ServiceResponse<AddBorrowModel>>> AddBorrow([FromBody] AddBorrowModel borrow)
        {
            var response = await UnitOfWork.BorrowRepository.AddBorrowAsync(borrow);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }


        [HttpPost("AddBorrowClient"), Authorize]
        public async Task<ActionResult<ServiceResponse<ClientBorrow>>> AddBorrowClient([FromBody] ClientBorrow borrow)
        {
            var response = await UnitOfWork.BorrowRepository.AddBorrowClient(borrow);

            if (response.Success == false)
            {
                return BadRequest(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<AddBorrowModel>>> UpdateBorrow([FromRoute] int id, [FromBody] AddBorrowModel borrow)
        {
            var response = await UnitOfWork.BorrowRepository.UpdateBorrowAsync(id, borrow);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }

        [HttpDelete("{id}"), AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<GetBorrowModel>>> DeleteBorrow([FromRoute] int id)
        {
            var response = await UnitOfWork.BorrowRepository.DeleteBorrowAsync(id);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<ServiceResponse<AddBorrowModel>>> ReturnBook([FromBody] JsonPatchDocument returnBorrow, [FromRoute] int id)
        {
            var response = await UnitOfWork.BorrowRepository.ReturnBookAsync(returnBorrow, id);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            await UnitOfWork.Save();

            return Ok(response);
        }
    }
}
