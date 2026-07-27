using Library_API.Models;

namespace Library_API.DTOs.Books
{
    public class GetBooksDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public DateTime PublishDate { get; set; }

        public string AuthorName { get; set; }

        public string CategoryName { get; set; }
    }
}
