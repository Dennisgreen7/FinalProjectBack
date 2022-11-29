using Dto.AuthModel;
using Dto.AuthorModel;
using Dto.BookModel;
using Dto.BorrowModel;
using Dto.GenreModel;
using Dto.UserModel;

namespace LibraryApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Author, GetAuthorModel>();
            CreateMap<AddAuthorModel, Author>().ReverseMap();
            CreateMap<Book, GetBookModel>();
            CreateMap<AddBookModel, Book>().ReverseMap();
            CreateMap<Genre, GetGenreModel>();
            CreateMap<AddGenreModel, Genre>().ReverseMap();
            CreateMap<Borrowing, GetBorrowModel>();
            CreateMap<AddBorrowModel, Borrowing>().ReverseMap();
            CreateMap<ClientBorrow, Borrowing>().ReverseMap(); 
            CreateMap<User, GetUserModel>();
            CreateMap<AddUserModel, User>().ReverseMap();
            CreateMap<RegistrationUser,User>().ReverseMap();
        }
    }
}
