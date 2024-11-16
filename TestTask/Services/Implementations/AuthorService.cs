using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthorService> _logger;
        public AuthorService(ApplicationDbContext context,
            ILogger<AuthorService> logger)
        {
            _context = context;
            _logger = logger;   

        }
        /// <summary>
        /// Returns the author who wrote the book with the longest title ( in case there are several such 
        /// authors, it is necessary to return the author with the smallest Id)
        /// </summary>
        /// <returns></returns>
        public async Task<Author> GetAuthor()
        {
            try
            {
                int maxLengthTitleOfBook = _context.Books.Max(book => book.Title.Length);

                var author = await _context.Authors
                    .Where(a => a.Books.Any(book => book.Title.Length == maxLengthTitleOfBook))
                    .OrderBy(a => a.Id)
                    .FirstOrDefaultAsync();

                return author;
            }
            catch(ArgumentNullException ex)
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
        /// Returns authors who have written an even number of books published after 2015
        /// </summary>
        /// <returns></returns>
        public async Task<List<Author>> GetAuthors()
        {
            try
            {

                var authors = await _context.Authors
                    .Where(a => a.Books.Where(book => book.PublishDate.Year > 2015)
                                       .Count() % 2 == 0 &&
                         // check that amount of these books is more than 0
                         a.Books.Any(book => book.PublishDate.Year > 2015))
                    .ToListAsync();

                return authors;
            }
            catch(ArgumentNullException ex)
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
