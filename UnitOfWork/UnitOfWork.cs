using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Models.Models;
using Repositories.Implementations;
using Repositories.Interfaces;
using Validator.Interfaces;

namespace Uow
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private LibraryContext _context { get; }
        private IMapper _mapper { get; }

        private readonly IConfiguration _configuration;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IInputValidator _validator;


        public UnitOfWork(LibraryContext context, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IInputValidator validator)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }
        public IAuthorRepository AuthorRepository => new AuthorRepository(_context, _mapper, _validator);

        public IBookRepository BookRepository => new BookRepository(_context, _mapper, _validator);

        public IGenreRepository GenreRepository => new GenreRepository(_context, _mapper, _validator);

        public IUserRepository UserRepository => new UserRepository(_context, _mapper, _validator);

        public IBorrowRepository BorrowRepository => new BorrowRepository(_context, _mapper, _validator);

        public IAuthRepository AuthRepository => new AuthRepository(_context, _mapper, _configuration, _httpContextAccessor, _validator);
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
