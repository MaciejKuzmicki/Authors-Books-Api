using ApiWebClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebClient.Controllers
{
    public class ApiController : Controller
    {
        private readonly ApiService _apiService;
        public ApiController(ApiService apiService) 
        {
            _apiService = apiService;
        }

        public async Task <IActionResult> Create()
        {
            List<Author> authorList = new List<Author>();
            authorList = await _apiService.loadAuthors();
            Book_Authors bookAuthors = new Book_Authors();
            bookAuthors.authors = authorList;
            bookAuthors.Book = new Book();
            return View(bookAuthors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title, Description")] Book book, [Bind("Id")] string Id)
        {
            
            Author author = await _apiService.loadAuthor(int.Parse(Id));
            if (book != null)
            {
                await _apiService.addBook(book.Title, author, book.Description);
                return Redirect("/Home/Index");
            }
            List<Author> authorList = new List<Author>();
            authorList = await _apiService.loadAuthors();
            Book_Authors bookAuthors = new Book_Authors();
            bookAuthors.authors = authorList;
            bookAuthors.Book = new Book();
            return View(bookAuthors);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _apiService.loadBook((int)id);
            return View(product.Data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _apiService.deleteBook((int)id);
            return Redirect("/Home/Index");

        }

        public async Task<IActionResult> Edit(int? id)
        {
            List<Author> authorList = new List<Author>();
            authorList = await _apiService.loadAuthors();
            Book_Authors bookAuthors = new Book_Authors();
            bookAuthors.authors = authorList;
            var response = await _apiService.loadBook((int) id);
            bookAuthors.Book = response.Data;
            bookAuthors.bookId = id.ToString();
            return View(bookAuthors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Title, Description")] Book book, [Bind("Id")] string Id, [Bind("bookId")] string bookId)
        {
            Author author = await _apiService.loadAuthor(int.Parse(Id));
            if (book != null)
            {

                await _apiService.updateBook(book.Title, author, book.Description, int.Parse(bookId));
                return Redirect("/Home/Index");
            }
            List<Author> authorList = new List<Author>();
            authorList = await _apiService.loadAuthors();
            Book_Authors bookAuthors = new Book_Authors();
            bookAuthors.authors = authorList;
            bookAuthors.Book = new Book();
            return View(bookAuthors);
        }

    }
}
