using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookAPI.Data;
using AutoMapper;
using BookAPI.Models.Author;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<AuthorsController> logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetFullAuthorDto>>> GetAuthors()
        {
            try
            {
                var dbAuthors = await _context.Authors.ToListAsync();
                var authors = mapper.Map<List<GetFullAuthorDto>>(dbAuthors);
                return authors;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error at {nameof(GetAuthors)}");
                return StatusCode(500, "An error ocurred. Please try again");
            }

        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetFullAuthorDto>> GetAuthor(int id)
        {
            try
            {
                var dbAuthor = await _context.Authors.FindAsync(id);
                if (dbAuthor == null)
                {
                    return NotFound();
                }
                var author = mapper.Map<GetFullAuthorDto>(dbAuthor);
                return Ok(author);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error at {nameof(GetAuthor)}");
                return StatusCode(500, "An error ocurred. Please try again");
            }
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, UpdateAuthorDto authorDto)
        {
            try
            {
                if (id != authorDto.Id)
                {
                    return BadRequest();
                }

                var dbAuthor = await _context.Authors.FindAsync(id);

                if (dbAuthor == null)
                {
                    return NotFound();
                }

                mapper.Map(authorDto, dbAuthor);
                _context.Entry(dbAuthor).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error at {nameof(PutAuthor)}");
                return StatusCode(500, "An error ocurred. Please try again");
            }
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetFullAuthorDto>> PostAuthor(CreateAuthorDto authorDto)
        {
            try
            {
                var author = mapper.Map<Author>(authorDto);
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error at {nameof(PostAuthor)}");
                return StatusCode(500, "An error ocurred. Please try again");
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error at {nameof(PostAuthor)}");
                return StatusCode(500, "An error ocurred. Please try again");
            }
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
