using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.api.Models;
using PCConfigurator.API.Data;
using PCConfigurator.API.Models;

namespace PCConfigurator.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PCBuildController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PCBuildController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var builds = await _context.PCBuilds
                .Include(b => b.CPU)
                .Include(b => b.GPU)
                .Include(b => b.RAM)
                .Include(b => b.Motherboard)
                .Include(b => b.SSD)
                .Include(b => b.HDD)
                .Include(b => b.PSU)
                .Include(b => b.Case)
                .Include(b => b.ThermalPaste)
                .ToListAsync();

            return Ok(builds);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var build = await _context.PCBuilds
                .Include(b => b.CPU)
                .Include(b => b.GPU)
                .Include(b => b.RAM)
                .Include(b => b.Motherboard)
                .Include(b => b.SSD)
                .Include(b => b.HDD)
                .Include(b => b.PSU)
                .Include(b => b.Case)
                .Include(b => b.ThermalPaste)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (build == null) return NotFound();
            return Ok(build);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PCBuild build)
        {
            _context.PCBuilds.Add(build);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = build.Id }, build);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PCBuild build)
        {
            if (id != build.Id) return BadRequest();

            _context.Entry(build).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var build = await _context.PCBuilds.FindAsync(id);
            if (build == null) return NotFound();

            _context.PCBuilds.Remove(build);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
