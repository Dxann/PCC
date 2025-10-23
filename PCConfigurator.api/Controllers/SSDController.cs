using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.api.Models;
using PCConfigurator.API.Data;

namespace PCConfigurator.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SSDController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SSDController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.SSDs.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.SSDs.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SSD ssd)
        {
            _context.SSDs.Add(ssd);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = ssd.Id }, ssd);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SSD ssd)
        {
            if (id != ssd.Id) return BadRequest();
            _context.Entry(ssd).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.SSDs.FindAsync(id);
            if (item == null) return NotFound();
            _context.SSDs.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
