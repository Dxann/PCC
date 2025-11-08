using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.API.Data;
using PCConfigurator.API.Models;

namespace PCConfigurator.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GPUController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GPUController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.GPUs.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var gpu = await _context.GPUs.FindAsync(id);
            return gpu == null ? NotFound() : Ok(gpu);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GPU gpu)
        {
            _context.GPUs.Add(gpu);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = gpu.Id }, gpu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, GPU gpu)
        {
            if (id != gpu.Id) return BadRequest();
            _context.Entry(gpu).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var gpu = await _context.GPUs.FindAsync(id);
            if (gpu == null) return NotFound();
            _context.GPUs.Remove(gpu);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
