namespace Library_API.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public DateTime PublishDate { get; set; }

        public int AuthorId { get; set; }

        public int CategoryId { get; set; }

        public Author Author { get; set; }

        public Category Category { get; set; }


    }
}
