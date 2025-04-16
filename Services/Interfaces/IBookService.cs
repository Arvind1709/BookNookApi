using TheBookNookApi.Model;

namespace TheBookNookApi.Services.Interfaces
{
    /// <summary>
    /// Service interface to manage books.
    /// </summary>
    public interface IBookService
    {
        Task<IEnumerable<BookModel>> GetAllBooksAsync();
        Task<BookModel?> GetBookByIdAsync(int id);
        Task<BookModel> AddBookAsync(BookModel book);
        Task<BookModel?> UpdateBookAsync(int id, BookModel updatedBook);
        Task<bool> DeleteBookAsync(int id);
    }
}
