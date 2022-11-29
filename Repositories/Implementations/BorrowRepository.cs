using AutoMapper;
using Dto.BorrowModel;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repositories.Interfaces;
using Validator.Interfaces;

namespace Repositories.Implementations
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;
        private readonly IInputValidator _validator;
        public BorrowRepository(LibraryContext context, IMapper mapper, IInputValidator validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ServiceResponse<List<GetBorrowModel>>> GetAllBorrowsAsync()
        {
            try
            {
                var records = await _context.Borrowings.Include(b => b.BorrowingUser).Include(b => b.BorrowingBook).ToListAsync();

                return new ServiceResponse<List<GetBorrowModel>> { Data = _mapper.Map<List<GetBorrowModel>>(records) };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetBorrowModel>> { Success = false, Message = "Borrows wasn't loaded." };
            }
        }
        public async Task<ServiceResponse<GetBorrowModel>> GetBorrowByIdAsync(int id)
        {
            try
            {
                var record = await _context.Borrowings.Include(b => b.BorrowingUser).Include(b => b.BorrowingBook).Where(b => b.BorrowingId == id).FirstOrDefaultAsync();

                if (record != null)
                {
                    return new ServiceResponse<GetBorrowModel> { Data = _mapper.Map<GetBorrowModel>(record) };
                }

                return new ServiceResponse<GetBorrowModel> { Success = false, Message = "Borrow not found." };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetBorrowModel> { Success = false, Message = "Borrow not found." };
            }
        }

        public async Task<ServiceResponse<List<GetBorrowModel>>> GetBorrowByUserAsync(int id)
        {
            try
            {
                var records = await _context.Borrowings.Where(b => b.BorrowingUserId == id).Include(b => b.BorrowingUser).Include(b => b.BorrowingBook).ToListAsync();

                if (records != null)
                {
                    return new ServiceResponse<List<GetBorrowModel>> { Data = _mapper.Map<List<GetBorrowModel>>(records) };
                }

                return new ServiceResponse<List<GetBorrowModel>> { Success = false, Message = "Borrow/s not found." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetBorrowModel>> { Success = false, Message = "Borrow/s not found." };
            }
        }

        public async Task<ServiceResponse<AddBorrowModel>> AddBorrowAsync(AddBorrowModel borrowModel)
        {
            try
            {
                
                var errorMssg = _validator.BorrowValidation(borrowModel.BorrowingReturnDate.ToString(), borrowModel.BorrowingUserId.ToString(), borrowModel.BorrowingBookId.ToString());

                if (errorMssg != String.Empty)
                {
                    return new ServiceResponse<AddBorrowModel> { Success = false, Message = errorMssg };
                }

                var borrow = _mapper.Map<Borrowing>(borrowModel);
                var book = await _context.Books.Where(b => b.BookId == borrow.BorrowingBookId).FirstOrDefaultAsync();

                borrow.BorrowingReturnDate = borrow.BorrowingReturnDate.AddDays(1);
                borrow.BorrowingDate = DateTime.Now.Date;
                book.BookCopys -= 1;

                _context.Borrowings.Add(borrow);

                return new ServiceResponse<AddBorrowModel> { Data = _mapper.Map<AddBorrowModel>(borrow), Message = "Borrow added successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddBorrowModel> { Success = false, Message = "Borrow wasn't added successfully." };
            }
        }

        public async Task<ServiceResponse<ClientBorrow>> AddBorrowClient(ClientBorrow borrowModel)
        {
            try
            {
                var borrow = _mapper.Map<Borrowing>(borrowModel);
                var book = await _context.Books.Where(b => b.BookId == borrow.BorrowingBookId).FirstOrDefaultAsync();

                borrow.BorrowingReturnDate = DateTime.Now.Date.AddDays(14);
                borrow.BorrowingDate = DateTime.Now.Date;
                book.BookCopys -= 1;

                _context.Borrowings.Add(borrow);

                return new ServiceResponse<ClientBorrow> { Data = _mapper.Map<ClientBorrow>(borrow), Message = "Book was borrowed successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ClientBorrow> { Success = false, Message = "Book wasn't borrowed successfully." };
            }
        }

        public async Task<ServiceResponse<AddBorrowModel>> UpdateBorrowAsync(int id, AddBorrowModel modifiedBorrow)
        {
            try
            {

                var errorMssg = _validator.BorrowValidation(modifiedBorrow.BorrowingReturnDate.ToString(), modifiedBorrow.BorrowingUserId.ToString(), modifiedBorrow.BorrowingBookId.ToString());

                if (errorMssg != String.Empty)
                {
                    return new ServiceResponse<AddBorrowModel> { Success = false, Message = errorMssg };
                }

                var borrow = await _context.Borrowings.Where(b => b.BorrowingId == id).SingleOrDefaultAsync();

                if (borrow == null)
                {
                    return new ServiceResponse<AddBorrowModel> { Success = false, Message = "Borrow not found." };
                }

                modifiedBorrow.BorrowingReturnDate = modifiedBorrow.BorrowingReturnDate.AddDays(1);

                _mapper.Map(modifiedBorrow, borrow);
                _context.Borrowings.Update(borrow);


                return new ServiceResponse<AddBorrowModel> { Message = "Borrow updated successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddBorrowModel> { Success = false, Message = "Borrow wasn't updated successfully." };
            }
        }
        public async Task<ServiceResponse<GetBorrowModel>> DeleteBorrowAsync(int id)
        {
            try
            {
                var borrow = await _context.Borrowings.FindAsync(id);

                if (borrow != null)
                {
                    _context.Borrowings.Remove(borrow);

                    return new ServiceResponse<GetBorrowModel> { Message = "Borrow deleted successfully." };
                }

                return new ServiceResponse<GetBorrowModel> { Success = false, Message = "Borrow not found." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetBorrowModel> { Success = false, Message = "Borrow wasn't deleted successfully." };
            }
        }

        public async Task<ServiceResponse<AddBorrowModel>> ReturnBookAsync(JsonPatchDocument returnBorrow, int id)
        {
            try
            {
                var borrow = await _context.Borrowings.Where(b => b.BorrowingId == id).FirstOrDefaultAsync();
                var book = await _context.Books.Where(b => b.BookId == borrow.BorrowingBookId).FirstOrDefaultAsync();

                if (borrow.BorrowingReturnedDate != null)
                {
                    return new ServiceResponse<AddBorrowModel> { Success = false, Message = "This Book already returned." };
                }

                else if (book == null)
                {
                    return new ServiceResponse<AddBorrowModel> { Success = false, Message = "Book not found." };
                }

                borrow.BorrowingReturnedDate = DateTime.Now.Date;
                book.BookCopys += 1;

                return new ServiceResponse<AddBorrowModel> { Data = _mapper.Map<AddBorrowModel>(borrow), Message = "Book returned successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddBorrowModel> { Success = false, Message = "Book wasn't returned successfully." };
            }
        }
    }
}
