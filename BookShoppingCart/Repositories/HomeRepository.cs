using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookShoppingCart.Repositories
{
    public class HomeRepository: IHomeRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IEnumerable<Genre>> GetGenres()
        {
            return await _db.Genres.ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Book> books = await (from book in _db.Books
                         join genre in _db.Genres
                         on book.GenreId equals genre.Id
                         join stock in _db.Stocks
                         on book.Id equals stock.BookId
                         into book_stocks
                         from bookWithStock in book_stocks.DefaultIfEmpty()
                         where (string.IsNullOrWhiteSpace(sTerm) || book.BookName.ToLower().StartsWith(sTerm)) && (genreId == 0 || genreId == book.GenreId)
                         select new Book
                         {
                             Id=book.Id,
                             BookName = book.BookName,
                             Image=book.Image,
                             AuthorName=book.AuthorName,
                             GenreId = book.GenreId,
                             Price = book.Price,
                             GenreName = genre.GenreName,
                             Quantity = bookWithStock==null ? 0 : bookWithStock.Quantity,
                         }).ToListAsync();
            return books;
        }
        public bool IsUserLoggedIn()
        {
            ClaimsPrincipal User = _httpContextAccessor.HttpContext.User;
            if (User.Identity.IsAuthenticated)
                return true;
            return false;
        }
    }
}
