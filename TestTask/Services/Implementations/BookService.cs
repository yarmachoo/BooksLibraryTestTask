using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookService> _logger;
        public BookService(ApplicationDbContext context,
            ILogger<BookService> logger)
        {
            _context = context;
            _logger = logger;

        }
        /// <summary>
        /// Возвращает книгу с наибольшей стоимостью опубликованного тиража
        /// </summary>
        /// <returns></returns>
        public async Task<Book> GetBook()
        {
            try
            {
                var book = await _context.Books
                    .OrderByDescending(b => b.Price * b.QuantityPublished)
                    .FirstOrDefaultAsync();

                return book;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"---> Exception: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"---> Exception: {ex.Message}");
                return null;
            }
        }
        /// <summary>
        /// Возвращает книги, в названии которой содержится "Red" и которые опубликованы после выхода альбома "Carolus Rex" группы Sabaton
        /// </summary>
        /// <returns></returns>
        public async Task<List<Book>> GetBooks()
        {
            try
            {
                DateTime releaseOfCarolusRexBySabaton = new DateTime(2012, 5, 25, 0, 0, 0);
                string red = "Red";

                var books = await _context.Books
                    .Where(book => book.Title.Contains(red) && book.PublishDate > releaseOfCarolusRexBySabaton)
                    .ToListAsync();
                return books;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError($"---> Exception: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"---> Exception: {ex.Message}");
                return null;
            }
        }
    }
}
