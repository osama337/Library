using Library_API.Data;
using Library_API.DTOs.Categories;
using Library_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("AllCategories")]
        public IActionResult GetAll()
        {
            var categories = _context.Categories
                .Select(c => new GetCategoriesDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

            return Ok(categories);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var category = _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new GetCategoriesDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .FirstOrDefault();

            if (category == null)
                return NotFound("Category Not Found");

            return Ok(category);
        }

        [HttpPost("Create")]
        public IActionResult Create(CreateAndUpdateCategoriesDto dto)
        {
            Category category = new Category
            {
                Name = dto.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok("Category Created Successfully");
        }


        [HttpPut("Edit/{id}")]
        public IActionResult Update(int id, CreateAndUpdateCategoriesDto dto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound("Category Not Found");

            category.Name = dto.Name;

            _context.SaveChanges();

            return Ok("Category Updated Successfully");
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return NotFound("Category Not Found");

            if (_context.Books.Any(b => b.CategoryId == id))
                return BadRequest("Cannot delete category because it contains books.");

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok("Category Deleted Successfully");
        }
    }
}