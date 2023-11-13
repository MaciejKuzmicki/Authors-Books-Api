namespace ApiWebClient.Models
{
    public class Book_Authors
    {
        public Book Book { get; set; }
        public List<Author> authors { get; set; }
        public string Id { get; set; }
        public string bookId { get; set; }
    }
}
