using Api.Controllers;
using Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Api.Logic
{
    public class BookService
    {
        private readonly DatabaseContext _databaseContext;
        
        public BookService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            /*DataGenerator dg = new DataGenerator(123456);
            List<Author> authors = dg.GenerateAuthors(10);
            List<Book> books = dg.GenerateBooks(20);
            _databaseContext.Authors.AddRange(authors);
            _databaseContext.Books.AddRange(books);
            _databaseContext.SaveChanges();*/ 
        }

        public async Task<ServiceResponse<Book>> AddBook(Book book)
        {
            var response = new ServiceResponse<Book>();

            if (book == null || book.Author == null)
            {
                response.Success = false;
                response.Message = "Incorrect data";
                response.StatusCode = 400;
                return response;
            }

            var existingAuthor = await _databaseContext.Authors.FindAsync(book.Author.Id);

            if (existingAuthor == null || existingAuthor.Name != book.Author.Name || existingAuthor.Surname != book.Author.Surname)
            {
                response.Success = false;
                response.Message = "Author not found";
                response.StatusCode = 400;
                return response;
            }

            try
            {
                book.Author = existingAuthor;

                _databaseContext.Books.Add(book);
                await _databaseContext.SaveChangesAsync();

                response.Data = book;
                response.Message = "Successfully added book";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error saving data";
                response.StatusCode = 500;
            }

            return response;
        }

        public async Task<ServiceResponse<Book>> GetBook(int id)
        {
            var response = new ServiceResponse<Book>();
            var book = await _databaseContext.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                response.Success = false;
                response.StatusCode = 404;
                response.Message = "Book not found";
                response.Data = null;
                return response;
            }
            response.Data = book;
            response.Message = "Succesfully found the book";
            return response;
        }

        public async Task<ServiceResponse<Book>> DeleteBook(int id)
        {
            var response = new ServiceResponse<Book>();
            var book = await _databaseContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                response.Success = false;
                response.StatusCode = 404;
                response.Message = "Book not found";
                return response;
            }
            _databaseContext.Books.Remove(book);
            await _databaseContext.SaveChangesAsync();
            response.Message = "Succesfully Deleted";
            return response;
        }

        public async Task<ServiceResponse<Book>> UpdateBook(int id, Book updatedBook)
        {
            var response = new ServiceResponse<Book>();

            if (updatedBook == null)
            {
                response.Success = false;
                response.Message = "Invalid data";
                response.StatusCode = 400;
                return response;
            }

            var existingBook = await _databaseContext.Books.FindAsync(id);

            if (existingBook == null)
            {
                response.Success = false;
                response.Message = "Book not found";
                response.StatusCode = 404;
                return response;
            }

            existingBook.Title = updatedBook.Title;
            existingBook.Description = updatedBook.Description;

            if (updatedBook.Author != null)
            {
                var existingAuthor = await _databaseContext.Authors.FindAsync(updatedBook.Author.Id);

                if (existingAuthor == null)
                {
                    response.Success = false;
                    response.Message = "Author not found";
                    response.StatusCode = 400;
                    return response;
                }
                existingBook.Author = existingAuthor;
            }

            try
            {
                await _databaseContext.SaveChangesAsync();
                response.Data = existingBook;
                response.Message = "Successfully updated book";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error updating data";
                response.StatusCode = 500;
            }

            return response;
        }


    }
}
