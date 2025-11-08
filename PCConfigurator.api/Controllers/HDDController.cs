using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.api.Models;
using PCConfigurator.API.Data;

namespace PCConfigurator.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HDDController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HDDController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.HDDs.ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.HDDs.FindAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HDD HDD)
        {
            _context.HDDs.Add(HDD);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = HDD.Id }, HDD);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, HDD HDD)
        {
            if (id != HDD.Id) return BadRequest();
            _context.Entry(HDD).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.HDDs.FindAsync(id);
            if (item == null) return NotFound();
            _context.HDDs.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
