using Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(DatabaseContext databaseContext, ILogger<AuthorsController> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _databaseContext.Authors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Author>>> GetAuthor(int id)
        {
            var response = new ServiceResponse<Author>();
            response.Data = await _databaseContext.Authors.FirstOrDefaultAsync(b => b.Id == id);
            if(response.Data == null)
            {
                response.Success = false;
                response.StatusCode = 404;
                response.Message = "Author not found";
                return NotFound("Author not found");
            }
            return response;

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Author>>> AddAuthor([FromBody]Author author)
        {
            var response = new ServiceResponse<Author>();
            if(author != null)
            {
                response.Data = author;
                _databaseContext.Authors.Add(author);
                await _databaseContext.SaveChangesAsync();
                return Ok(response);
            }
            return BadRequest(response);
            
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<Author>>> UpdateAuthor(int id,[FromBody] Author author)
        {
            var currentAuthor = await _databaseContext.Authors.FirstOrDefaultAsync(b => b.Id == id);
            var response = new ServiceResponse<Author>();

            if(currentAuthor == null)
            {
                response.Success=false;
                response.StatusCode = 404;
                response.Message = "Author not found";
                return NotFound(response);
            }
            if(author == null) {
                response.Success = false;
                response.StatusCode = 400;
                response.Message = "Invalid input Data";
                return BadRequest(response);
            }

            currentAuthor.Name = author.Name;
            currentAuthor.Surname = author.Surname;
            response.Data = currentAuthor;

            _databaseContext.Entry(currentAuthor).State = EntityState.Modified;
            await _databaseContext.SaveChangesAsync();
            return Ok(response);
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<Author>>> DeleteAuthor(int id)
        {
            var response = new ServiceResponse<Author>();
            var author = await _databaseContext.Authors.FirstOrDefaultAsync(b => b.Id == id);
            if(author == null)
            {
                response.Success=false;
                response.StatusCode = 404;
                response.Message = "Author not found";
                return response;
            }
            _databaseContext.Authors.Remove(author);
            await _databaseContext.SaveChangesAsync();
            return response;
        }
    }
}