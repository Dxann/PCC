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
    public class PSUController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PSUController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.PSUs.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.PSUs.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PSU psu)
        {
            _context.PSUs.Add(psu);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = psu.Id }, psu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PSU psu)
        {
            if (id != psu.Id) return BadRequest();
            _context.Entry(psu).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.PSUs.FindAsync(id);
            if (item == null) return NotFound();
            _context.PSUs.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
