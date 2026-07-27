using Library_API.Models;

namespace Library_API.DTOs.Author
{
    public class GetAuthorsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }

        public List<string> Books { get; set; }
    }
}
