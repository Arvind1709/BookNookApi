using Microsoft.AspNetCore.Mvc;
using TheBookNookApi.Model;
using TheBookNookApi.Services.Interfaces;

namespace TheBookNookApi.Controllers
{
    /// <summary>
    /// Controller to handle CRUD operations for books.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        #region Fields

        private readonly IBookService _bookService;

        #endregion

        #region Constructor

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Get all books.
        /// </summary>
        [HttpGet("getAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        /// <summary>
        /// Get a specific book by ID.
        /// </summary>
        [HttpGet("getBookById/{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid book ID.");

            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                    return NotFound($"Book with ID {id} not found.");
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        /// <summary>
        /// Add a new book.
        /// </summary>
        [HttpPost("addBook")]
        public async Task<IActionResult> AddBook([FromBody] BookModel book)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdBook = await _bookService.AddBookAsync(book);
                return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to add book: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing book.
        /// </summary>
        [HttpPut("updateBook/{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookModel book)
        {
            if (id <= 0)
                return BadRequest("Invalid book ID.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedBook = await _bookService.UpdateBookAsync(id, book);
                if (updatedBook == null)
                    return NotFound($"Book with ID {id} not found.");
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update book: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a book by ID.
        /// </summary>
        [HttpDelete("deleteBook/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid book ID.");

            try
            {
                var deleted = await _bookService.DeleteBookAsync(id);
                if (!deleted)
                    return NotFound($"Book with ID {id} not found or already deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to delete book: {ex.Message}");
            }
        }

        #endregion
    }
}
