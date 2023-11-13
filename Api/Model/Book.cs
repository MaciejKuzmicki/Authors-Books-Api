using System.ComponentModel.DataAnnotations;

namespace Api.Model
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public Author Author { get; set; }

        public string Description { get; set; }
        
    }

    public class Author
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

    }
}
