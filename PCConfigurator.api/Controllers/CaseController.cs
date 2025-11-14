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
    public class CaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Cases.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.Cases.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Case pcCase)
        {
            _context.Cases.Add(pcCase);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = pcCase.Id }, pcCase);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Case pcCase)
        {
            if (id != pcCase.Id) return BadRequest();
            _context.Entry(pcCase).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Cases.FindAsync(id);
            if (item == null) return NotFound();
            _context.Cases.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
