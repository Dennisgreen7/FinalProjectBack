using Repositories.Interfaces;

namespace Uow
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }
        IBookRepository BookRepository { get; }
        IBorrowRepository BorrowRepository { get; }
        IGenreRepository GenreRepository { get; }
        IUserRepository UserRepository { get; }
        IAuthRepository AuthRepository { get; }
        void Dispose();
        Task Save();
    }
}