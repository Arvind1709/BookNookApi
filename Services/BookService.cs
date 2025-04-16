using Microsoft.EntityFrameworkCore;
using TheBookNookApi.AppDbContext;
using TheBookNookApi.Model;
using TheBookNookApi.Services.Interfaces;

namespace TheBookNookApi.Services
{
    /// <summary>
    /// Service implementation for managing books.
    /// </summary>
    public class BookService : IBookService
    {
        #region Fields

        private readonly BookNookDbContext _context;

        #endregion

        #region Constructor

        public BookService(BookNookDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Public Methods

        public async Task<IEnumerable<BookModel>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<BookModel?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<BookModel> AddBookAsync(BookModel book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<BookModel?> UpdateBookAsync(int id, BookModel updatedBook)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return null;

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Price = updatedBook.Price;
            book.Description = updatedBook.Description;
            book.Stock = updatedBook.Stock;
            book.Category = updatedBook.Category;
            book.BookCover = updatedBook.BookCover;

            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion
    }
}
