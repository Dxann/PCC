using Microsoft.AspNetCore.Mvc;
using PCConfigurator.API.Data;
using PCConfigurator.API.Models;
using Microsoft.EntityFrameworkCore;

namespace PCConfigurator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GPUController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GPUController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.GPUs.ToList());
    }
}
