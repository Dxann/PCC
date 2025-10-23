using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.API.Data;
using PCConfigurator.API.Models;

namespace PCConfigurator.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RAMController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RAMController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.RAMs.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ram = await _context.RAMs.FindAsync(id);
            return ram == null ? NotFound() : Ok(ram);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RAM ram)
        {
            _context.RAMs.Add(ram);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = ram.Id }, ram);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RAM ram)
        {
            if (id != ram.Id) return BadRequest();
            _context.Entry(ram).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ram = await _context.RAMs.FindAsync(id);
            if (ram == null) return NotFound();
            _context.RAMs.Remove(ram);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
