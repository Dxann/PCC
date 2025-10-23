using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.api.Models;
using PCConfigurator.API.Data;

namespace PCConfigurator.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThermalPasteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ThermalPasteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.ThermalPastes.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.ThermalPastes.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ThermalPaste tp)
        {
            _context.ThermalPastes.Add(tp);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = tp.Id }, tp);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ThermalPaste tp)
        {
            if (id != tp.Id) return BadRequest();
            _context.Entry(tp).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ThermalPastes.FindAsync(id);
            if (item == null) return NotFound();
            _context.ThermalPastes.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
