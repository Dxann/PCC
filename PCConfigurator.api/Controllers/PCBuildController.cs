using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.API.Data;
using PCConfigurator.API.Models;

namespace PCConfigurator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PCBuildController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PCBuildController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var builds = await _context.PCBuilds
                .Include(b => b.CPU)
                .Include(b => b.GPU)
                .ToListAsync();
            return Ok(builds);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PCBuild build)
        {
            _context.PCBuilds.Add(build);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = build.Id }, build);
        }


    }
}
