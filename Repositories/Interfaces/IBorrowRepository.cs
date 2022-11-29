using Dto.BorrowModel;
using Microsoft.AspNetCore.JsonPatch;
using Models.Models;

namespace Repositories.Interfaces
{
    public interface IBorrowRepository
    {
        Task<ServiceResponse<AddBorrowModel>> AddBorrowAsync(AddBorrowModel borrow);
        Task<ServiceResponse<GetBorrowModel>> DeleteBorrowAsync(int id);
        Task<ServiceResponse<List<GetBorrowModel>>> GetAllBorrowsAsync();
        Task<ServiceResponse<GetBorrowModel>> GetBorrowByIdAsync(int id);
        Task<ServiceResponse<AddBorrowModel>> UpdateBorrowAsync(int id, AddBorrowModel modifiedBorrow);
        Task<ServiceResponse<AddBorrowModel>> ReturnBookAsync(JsonPatchDocument returnBorrow, int id);
        Task<ServiceResponse<ClientBorrow>> AddBorrowClient(ClientBorrow borrowModel);
        Task<ServiceResponse<List<GetBorrowModel>>> GetBorrowByUserAsync(int id);
    }
}