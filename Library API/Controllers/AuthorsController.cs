using Library_API.Data;
using Library_API.Models;
using Library_API.DTOs.Author;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Library_API.DTOs;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("All Authors")]
        public IActionResult GetAll()
        {
            var Auth = _context.Authors
                .Include(A => A.Books)
                .Select(A => new GetAuthorsDto
                {
                    Id = A.Id,
                    Name = A.Name,
                    Country = A.Country,
                    Books = A.Books.Select(b => b.Title).ToList()
                    
                }).ToList();
            return Ok(Auth);
        }
        
        [HttpGet("AllAuthorsById")]
        public IActionResult GetById(int id)
        {
            var Author = _context.Authors.FirstOrDefault(a => a.Id == id);
            if (Author == null) 
                return NotFound("Author Not Found");

            var Auth = _context.Authors
                .Include(a => a.Books)
                .Select(A => new GetAuthorsDto
                {
                    Id = A.Id,
                    Name = A.Name,
                    Country = A.Country
                }).ToList();
            return Ok(Auth);
        }

        [HttpPost("Create a new Author")]
        public IActionResult Create(CreateAndUpdateAuthorsDto Adto)
        {
            var author = new Author
            {
                Name = Adto.Name,
                Country = Adto.Country
            };
            _context.Authors.Add(author);
            var res = _context.SaveChanges();
            return res > 0 ? Created() : BadRequest();
        }

        [HttpPut("EditTheAuthor/{id}")]
        public IActionResult Update(int id, CreateAndUpdateAuthorsDto Adto)
        {
            var authors = _context.Authors.FirstOrDefault(a => a.Id == id);

            if (authors == null)
                return NotFound("Author Not Found");

            authors.Name = Adto.Name;
            authors.Country = Adto.Country;

            _context.Authors.Update(authors);
            var res = _context.SaveChanges();
            return res > 0 ? Ok("Updated") : BadRequest();
        }
        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var auth= _context.Authors.FirstOrDefault(a => a.Id == id);

            if (auth == null)
                return NotFound("Author Not Found");

            _context.Authors.Remove(auth);
            var res = _context.SaveChanges();

            return Ok("Deleted");
        }
        [HttpGet("books-count/{Count}")]
        public IActionResult Count()
        {
            var auth = _context.Authors
                .Include(a => a.Books)
                .Select(a => new AuthorBooksCountDto
                {
                    AuthorName = a.Name,
                    BookCount = a.Books.Count()
                }).ToList();

            return Ok(auth);
        }
    }
}
