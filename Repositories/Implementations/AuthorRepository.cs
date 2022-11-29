using AutoMapper;
using Dto.AuthorModel;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repositories.Interfaces;
using Validator.Interfaces;

namespace Repositories.Implementations
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;
        private readonly IInputValidator _validator;

        public AuthorRepository(LibraryContext context, IMapper mapper, IInputValidator validator)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<ServiceResponse<List<GetAuthorModel>>> GetAllAuthorsAsync()
        {
            try
            {
                var records = await _context.Authors.ToListAsync();

                return new ServiceResponse<List<GetAuthorModel>> { Data = _mapper.Map<List<GetAuthorModel>>(records) };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<GetAuthorModel>> { Success = false, Message = "Authours wasn't loded." };
            }
        }
        public async Task<ServiceResponse<GetAuthorModel>> GetAuthorByIdAsync(int id)
        {
            try
            {
                var record = await _context.Authors.Where(a => a.AuthorId == id).FirstOrDefaultAsync();

                if (record != null)
                {
                    return new ServiceResponse<GetAuthorModel> { Data = _mapper.Map<GetAuthorModel>(record) };
                }

                return new ServiceResponse<GetAuthorModel> { Success = false, Message = "Author not found." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetAuthorModel> { Success = false, Message = "Author not found." };
            }
        }
        public async Task<ServiceResponse<AddAuthorModel>> AddAuthorAsync(AddAuthorModel authorModel)
        {
            try
            {
                var errorMssg = _validator.AuthorValidation(authorModel.AuthorName, authorModel.AuthorCountry);

                if (errorMssg != String.Empty)
                {
                    return new ServiceResponse<AddAuthorModel> { Success = false, Message = errorMssg };
                }

                var author = _mapper.Map<Author>(authorModel);

                _context.Authors.Add(author);

                return new ServiceResponse<AddAuthorModel> { Data = _mapper.Map<AddAuthorModel>(author), Message = "Author added successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<AddAuthorModel> { Success = false, Message = "Author wasn't added successfully." };
            }
        }
        public async Task<ServiceResponse<GetAuthorModel>> UpdateAuthorAsync(int id, AddAuthorModel modifiedAuthor)
        {
            try
            {
                var errorMssg = _validator.AuthorValidation(modifiedAuthor.AuthorName, modifiedAuthor.AuthorCountry);

                if (errorMssg != string.Empty)
                {
                    return new ServiceResponse<GetAuthorModel> { Success = false, Message = errorMssg };
                }

                var author = await _context.Authors.Where(a => a.AuthorId == id).SingleOrDefaultAsync();

                if (author == null)
                {
                    return new ServiceResponse<GetAuthorModel> { Success = false, Message = "Author not found." };
                }

                _mapper.Map(modifiedAuthor, author);

                _context.Authors.Update(author);

                return new ServiceResponse<GetAuthorModel> { Data = _mapper.Map<GetAuthorModel>(author), Message = "Author updated successfully." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetAuthorModel> { Success = false, Message = "Author wasn't updated successfully." };
            }
        }
        public async Task<ServiceResponse<GetAuthorModel>> DeleteAuthorAsync(int id)
        {
            try
            {
                var authorBooks = await _context.Books.Where(b => b.AuthorId == id).ToListAsync();
                var author = await _context.Authors.FindAsync(id);

                if (author != null && authorBooks != null)
                {
                    for (int i = 0; i < authorBooks.Count; i++)
                    {
                        _context.Books.Remove(authorBooks[i]);
                    }
                    _context.Authors.Remove(author);

                    return new ServiceResponse<GetAuthorModel> { Message = "Author deleted successfully." };
                }

                return new ServiceResponse<GetAuthorModel> { Success = false, Message = "Author not found." };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<GetAuthorModel> { Success = false, Message = "Author wasn't deleted successfully." };
            }
        }
    }
}
