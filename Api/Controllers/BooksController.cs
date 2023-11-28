using Api.Logic;
using Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<BooksController> _logger;
        private readonly BookService _bookService;

        public BooksController(DatabaseContext databaseContext, ILogger<BooksController> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
            _bookService = new BookService(databaseContext);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _databaseContext.Books.Include(b=>b.Author).ToListAsync();
        }

        [HttpPost, Authorize(Roles ="Customer")]
        public async Task<ActionResult<ServiceResponse<Book>>> AddBook([FromBody] Book book)
        {
            var result = await _bookService.AddBook(book);
            if (result.Success)
            {
                return StatusCode(result.StatusCode, result.Message);
            }
            else return StatusCode(result.StatusCode, result.Message);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Book>>> GetBook(int id)
        {
            var result = await _bookService.GetBook(id); 
            
            if(result.Success)
            {
                return result;
            }
            else return StatusCode(result.StatusCode, result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<Book>>> DeleteBook(int id)
        {
            var result = await _bookService.DeleteBook(id);
            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<Book>>> UpdateBook(int id, [FromBody] Book book)
        {
            var result = await _bookService.UpdateBook(id, book);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
