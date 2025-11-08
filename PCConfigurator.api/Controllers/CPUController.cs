using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.API.Data;
using PCConfigurator.API.Models;

namespace PCConfigurator.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CPUController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CPUController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.CPUs.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cpu = await _context.CPUs.FindAsync(id);
            return cpu == null ? NotFound() : Ok(cpu);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CPU cpu)
        {
            _context.CPUs.Add(cpu);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = cpu.Id }, cpu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CPU cpu)
        {
            if (id != cpu.Id) return BadRequest();
            _context.Entry(cpu).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cpu = await _context.CPUs.FindAsync(id);
            if (cpu == null) return NotFound();
            _context.CPUs.Remove(cpu);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
