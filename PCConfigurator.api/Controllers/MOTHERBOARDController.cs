using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.api.Models;
using PCConfigurator.API.Data;
using PCConfigurator.API.Models;

namespace PCConfigurator.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotherboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MotherboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Motherboards.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.Motherboards.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Motherboard motherboard)
        {
            _context.Motherboards.Add(motherboard);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = motherboard.Id }, motherboard);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Motherboard motherboard)
        {
            if (id != motherboard.Id) return BadRequest();
            _context.Entry(motherboard).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Motherboards.FindAsync(id);
            if (item == null) return NotFound();
            _context.Motherboards.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
