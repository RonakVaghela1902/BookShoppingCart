using Microsoft.EntityFrameworkCore;

namespace BookShoppingCart.Repositories
{
    public class HomeRepository: IHomeRepository
    {
        private readonly ApplicationDbContext db;

        public HomeRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Book> books = await (from book in db.Books
                         join genre in db.Genres
                         on book.GenreId equals genre.Id
                         where string.IsNullOrWhiteSpace(sTerm) || (book!= null && book.BookName.ToLower().StartsWith(sTerm)) || (genreId > 0 && genreId == book.GenreId)
                         select new Book
                         {
                             Id=book.Id,
                             Image=book.Image,
                             AuthorName=book.AuthorName,
                             GenreId = book.GenreId,
                             Price = book.Price,
                             GenreName = genre.GenreName
                         }).ToListAsync();
            return books;
        }
    }
}
