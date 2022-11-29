using AutoMapper;
using Dto.BookModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repositories.Interfaces;
using Validator.Interfaces;

namespace Repositories.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;
        private readonly IInputValidator _validator;
        public BookRepository(LibraryContext context, IMapper mapper, IInputValidator validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<ServiceResponse<List<GetBookModel>>> GetAllBooksAsync(IWebHostEnvironment _environment)
        {
            try
            {
                var records = await _context.Books.Include(b => b.Author).Include(b => b.Genre).ToListAsync();
                var books = _mapper.Map<List<GetBookModel>>(records);
                if (books != null && books.Count > 0)
                {
                    books.ForEach(book =>
                    {
                        book.ImageSrc = GetBookImage(book.BookId.ToString(), _environment);
                    });
                }

                return new ServiceResponse<List<GetBookModel>> { Data = books };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetBookModel>> { Success = false, Message = "Books wasn't loaded." };
            }
        }
        public async Task<ServiceResponse<List<GetBookModel>>> GetAllBooksForBorrowAsync(IWebHostEnvironment _environment)
        {
            try
            {
                var records = await _context.Books.Where(b => b.BookCopys > 0).Include(b => b.Author).Include(b => b.Genre).ToListAsync();
                var books = _mapper.Map<List<GetBookModel>>(records);
                if (books != null && books.Count > 0)
                {
                    books.ForEach(book =>
                    {
                        book.ImageSrc = GetBookImage(book.BookId.ToString(), _environment);
                    });
                }

                return new ServiceResponse<List<GetBookModel>> { Data = books };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetBookModel>> { Success = false, Message = "Books wasn't loaded." };
            }
        }
        public async Task<ServiceResponse<GetBookModel>> GetBookByIdAsync(int id, IWebHostEnvironment _environment)
        {
            try
            {
                var record = await _context.Books.Include(b => b.Author).Include(b => b.Genre).Where(b => b.BookId == id).FirstOrDefaultAsync();
                var book = _mapper.Map<GetBookModel>(record);

                if (record != null)
                {
                    book.ImageSrc = GetBookImage(book.BookId.ToString(), _environment);
                    return new ServiceResponse<GetBookModel> { Data = book };
                }
                else
                {
                    return new ServiceResponse<GetBookModel> { Success = false, Message = "Book not found." };
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetBookModel> { Success = false, Message = "Book not found." };
            }
        }

        public async Task<ServiceResponse<List<GetBookModel>>> FilteBook(int filterIndex, string searchValue, IWebHostEnvironment _environment)
        {
            try
            {
                if (filterIndex == 0 || searchValue == "")
                {
                    return new ServiceResponse<List<GetBookModel>> { Success = false, Message = "Error, one of the parameters invalid." };
                }

                var records = await _context.Books.Include(b => b.Author).Include(b => b.Genre).ToListAsync();
                var books = _mapper.Map<List<GetBookModel>>(records);
                var filterdList = new List<GetBookModel>();

                switch (filterIndex)
                {
                    case 1:
                        filterdList = books.Where(b => b.BookName == searchValue).ToList();
                        break;
                    case 2:
                        filterdList = books.Where(b => b.GenreName == searchValue).ToList();
                        break;
                    case 3:
                        filterdList = books.Where(b => b.BookPublishedYear == int.Parse(searchValue)).ToList();
                        break;
                    case 4:
                        filterdList = books.Where(b => b.BookNumOfPages == int.Parse(searchValue)).ToList();
                        break;
                    case 5:
                        filterdList = books.Where(b => b.BookLanguage == searchValue).ToList();
                        break;
                    case 6:
                        filterdList = books.Where(b => b.AuthorName == searchValue).ToList();
                        break;
                }


                if (filterdList != null && filterdList.Count > 0)
                {
                    filterdList.ForEach(filterdList =>
                    {
                        filterdList.ImageSrc = GetBookImage(filterdList.BookId.ToString(), _environment);
                    });
                    return new ServiceResponse<List<GetBookModel>> { Data = filterdList };
                }

                return new ServiceResponse<List<GetBookModel>> { Success = false, Message = "Book/s not found." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetBookModel>> { Success = false, Message = "Book/s not found." };
            }
        }

        public async Task<ServiceResponse<AddBookModel>> AddBookAsync(AddBookModel bookModel)
        {
            try
            {
                var errorMssg = _validator.BookValidation(bookModel.BookName, bookModel.BookLanguage, bookModel.BookNumOfPages.ToString(), bookModel.BookCopys.ToString()
                    , bookModel.AuthorId.ToString(), bookModel.GenreId.ToString(), bookModel.BookPublishedYear.ToString());

                if (errorMssg != String.Empty)
                {
                    return new ServiceResponse<AddBookModel> { Success = false, Message = errorMssg };
                }

                var book = _mapper.Map<Book>(bookModel);

                _context.Books.Add(book);

                return new ServiceResponse<AddBookModel> { Data = _mapper.Map<AddBookModel>(book), Message = "Book added successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddBookModel> { Success = false, Message = "Book wasn't added successfully." };
            }
        }
        public async Task<ServiceResponse<GetBookModel>> UpdateBookAsync(int id, AddBookModel modifiedBook)
        {
            try
            {
                var errorMssg = _validator.BookValidation(modifiedBook.BookName, modifiedBook.BookLanguage, modifiedBook.BookNumOfPages.ToString(), modifiedBook.BookCopys.ToString()
                    , modifiedBook.AuthorId.ToString(), modifiedBook.GenreId.ToString(), modifiedBook.BookPublishedYear.ToString());

                if (errorMssg != String.Empty)
                {
                    return new ServiceResponse<GetBookModel> { Success = false, Message = errorMssg };
                }

                var book = await _context.Books.Include(b => b.Author).Include(b => b.Genre).Where(b => b.BookId == id).SingleOrDefaultAsync();

                if (book == null)
                {
                    return new ServiceResponse<GetBookModel> { Success = false, Message = "Book not found." };
                }

                _mapper.Map(modifiedBook, book);

                _context.Books.Update(book);

                return new ServiceResponse<GetBookModel> { Data = _mapper.Map<GetBookModel>(book), Message = "Book updated successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetBookModel> { Success = false, Message = "Book wasn't updated successfully." };
            }
        }
        public async Task<ServiceResponse<GetBookModel>> DeleteBookAsync(int id)
        {
            try
            {
                var openBorrow = await _context.Borrowings.FirstOrDefaultAsync(b => b.BorrowingBookId == id && b.BorrowingReturnedDate == null);

                if (openBorrow != null)
                {
                    return new ServiceResponse<GetBookModel> { Success = false, Message = "This Book have open Borrowings, close them to delete." };
                }

                var bookBorrows = await _context.Borrowings.Where(b => b.BorrowingBookId == id).ToListAsync();
                var book = await _context.Books.FindAsync(id);


                if (book != null && bookBorrows != null)
                {
                    for (int i = 0; i < bookBorrows.Count; i++)
                    {
                        _context.Borrowings.Remove(bookBorrows[i]);
                    }
                    _context.Books.Remove(book);

                    return new ServiceResponse<GetBookModel> { Message = "Book deleted successfully." };
                }

                return new ServiceResponse<GetBookModel> { Success = false, Message = "Book not found." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetBookModel> { Success = false, Message = "Book wasn't deleted successfully." };
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

        private string GetFilePath(string bookCode, IWebHostEnvironment _environment)
        {
            return _environment.WebRootPath + "\\Uploads\\Books\\" + bookCode;
        }
        public string GetBookImage(string bookCode, IWebHostEnvironment _environment)
        {
            try
            {
                string imageUrl = string.Empty;
                string hostUrl = "https://localhost:7034/";
                string filePath = GetFilePath(bookCode, _environment);
                string imagePath = filePath + "\\image.png";

                if (!System.IO.File.Exists(imagePath))
                {
                    imageUrl = hostUrl + "/Uploads/common//noimage.png";
                }
                else
                {
                    imageUrl = hostUrl + "/Uploads/Books/" + bookCode + "/image.png";
                }
                return imageUrl;
            }
            catch
            {
                return "https://localhost:7034//Uploads/common//noimage.png";
            }
        }
        public async Task<ServiceResponse<string>> RemoveBookImage(string bookCode, IWebHostEnvironment _environment)
        {
            try
            {
                string filePath = GetFilePath(bookCode, _environment);
                string imagePath = filePath + "\\image.png";

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                return new ServiceResponse<string> { Data = imagePath };
            }
            catch
            {
                return new ServiceResponse<string> { Data = "https://localhost:7034//Uploads/Common//noimage.png" };
            }
        }
    }
}
