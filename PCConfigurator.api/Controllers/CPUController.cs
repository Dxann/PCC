using Microsoft.AspNetCore.Mvc;
using PCConfigurator.API.Data;
using PCConfigurator.API.Models;

namespace PCConfigurator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CPUController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CPUController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.CPUs.ToList());
    }
}
