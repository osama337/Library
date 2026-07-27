using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_API.DTOs.Books;
using Library_API.Data;
using Microsoft.EntityFrameworkCore;
using Library_API.Models;


namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("AllBooks")]
        public IActionResult GetAll()
        {
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Select(b => new GetBooksDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    CategoryName = b.Category.Name,
                    PublishDate = b.PublishDate,
                    AuthorName = b.Author.Name
                }).ToList();
            return Ok(book);
        }
        [HttpGet("BookById")]
        public IActionResult GetAll(int id)
        {
            var books = _context.Books.FirstOrDefault(b => b.Id == id);
            if(books == null)
                return NotFound("Book not found");
           
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Select(b => new GetBooksDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    CategoryName = b.Category.Name,
                    PublishDate = b.PublishDate,
                    AuthorName = b.Author.Name,
                }).ToList();
            return Ok(book);
        }
        [HttpPost("Create")]
        public IActionResult Create(CreateAndUpdateBooksDto bdto)
        {
            var author = _context.Authors
                .FirstOrDefault(a => a.Name == bdto.AuthorName);

            if (author == null)
                return BadRequest("Author not found.");

            var category = _context.Categories
                .FirstOrDefault(c => c.Name == bdto.CategoryName);

            if (category == null)
                return BadRequest("Category not found.");
            var book = new Book
            {
                Title = bdto.Title,
                Price = bdto.Price,
                PublishDate = bdto.PublishDate,
                AuthorId = author.Id,
                CategoryId = category.Id
            };
            _context.Books.Add(book);
            var res = _context.SaveChanges();
            return res > 0 ? Created() : BadRequest();
        }
        [HttpPut("Edit/{id}")]
        public IActionResult Update(int id , CreateAndUpdateBooksDto bdto)
        {
            var books = _context.Books.FirstOrDefault(b => b.Id == id);
            if (books == null)
                return NotFound("Book not found");
            var author = _context.Authors
                .FirstOrDefault(a => a.Name == bdto.AuthorName);

            if (author == null)
                return BadRequest("Author not found.");

            var category = _context.Categories
                .FirstOrDefault(c => c.Name == bdto.CategoryName);

            if (category == null)
                return BadRequest("Category not found.");

            books.Title = bdto.Title;
            books.Price = bdto.Price;
            books.AuthorId = author.Id;
            books.CategoryId = category.Id;

            _context.Update(books);
            var res = _context.SaveChanges();
            return res > 0 ? Ok("Updated") : BadRequest();
        }
        [HttpGet("Price/{min}/{max}")]
        public IActionResult GetBooksByPrice(decimal min, decimal max)
        {
            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.Price >= min && b.Price <= max)
                .Select(b => new GetBooksDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,
                    CategoryName = b.Category.Name,
                    PublishDate = b.PublishDate,
                    AuthorName = b.Author.Name
                }).ToList();

            if (!books.Any())
                return NotFound("No books found in this price range.");

            return Ok(books);
        }
    }
}
