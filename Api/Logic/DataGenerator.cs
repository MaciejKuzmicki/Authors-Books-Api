using Api.Model;
using Bogus;


namespace Api.Logic
{
    public class DataGenerator
    {
        private readonly Random _random;
        private readonly Faker<Author> _authorFaker;
        private readonly Faker<Book> _bookFaker;
        private HashSet<int> generatedAuthorIds = new HashSet<int>();
        private HashSet<int> generatedBookIds = new HashSet<int>();

        public DataGenerator(int seed)
        {
            _random = new Random(seed);
            _authorFaker = new Faker<Author>()
                .CustomInstantiator(f => new Author
                {
                    Id = GenerateUniqueAuthorId(),
                    Name = f.Person.FirstName,
                    Surname = f.Person.LastName
                });

            _bookFaker = new Faker<Book>()
                .CustomInstantiator(f => new Book
                {
                    Id = GenerateUniqueBookId(),
                    Title = f.Commerce.ProductName(),
                    Description = f.Lorem.Paragraph(),
                    Author = _authorFaker.Generate(1)[0]
                });
        }

        private int GenerateUniqueAuthorId()
        {
            int id;
            do
            {
                id = _random.Next(1, 1000); 
            } while (generatedAuthorIds.Contains(id));
            generatedAuthorIds.Add(id);
            return id;
        }

        private int GenerateUniqueBookId()
        {
            int id;
            do
            {
                id = _random.Next(1, 1000); 
            } while (generatedBookIds.Contains(id));
            generatedBookIds.Add(id);
            return id;
        }

        public List<Author> GenerateAuthors(int count)
        {
            return _authorFaker.Generate(count);
        }

        public List<Book> GenerateBooks(int count)
        {
            return _bookFaker.Generate(count);
        }
    }

}
