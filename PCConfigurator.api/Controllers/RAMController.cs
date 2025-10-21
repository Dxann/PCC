using Microsoft.AspNetCore.Mvc;
using PCConfigurator.API.Data;
using PCConfigurator.API.Models;
using Microsoft.EntityFrameworkCore;

namespace PCConfigurator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RAMController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RAMController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public IActionResult GetAll() => Ok(_context.RAMs.ToList());
    }
}
