using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.api.Models;
using PCConfigurator.API.Data;
using PCConfigurator.API.Models.DTO;

namespace PCConfigurator.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PCBuildController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PCBuildController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Получить список всех сборок
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
                .Include(b => b.User)
                .ToListAsync();

            var likes = await _context.PCBuildLikes.ToListAsync();

            var result = builds.Select(b => new PCBuildDto
            {
                Id = b.Id,
                Name = b.Name,
                TotalPrice = b.TotalPrice,
                CPU = b.CPU,
                GPU = b.GPU,
                RAM = b.RAM,
                Motherboard = b.Motherboard,
                SSD = b.SSD,
                HDD = b.HDD,
                PSU = b.PSU,
                Case = b.Case,
                ThermalPaste = b.ThermalPaste,
                UserName = b.User?.UserName ?? "Unknown",
                CreatedAt = b.CreatedAt,
                UserId = b.UserId,
                Likes = likes.Count(l => l.BuildId == b.Id)
            });

            return Ok(result);
        }


        // Получить сборку по Id
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

        // Создать сборку (правильный вариант)
        [HttpPost("save")]
        public async Task<IActionResult> SaveBuild([FromBody] PCBuildCreateDto dto)
        {
            var cpu = dto.CPUId.HasValue ? await _context.CPUs.FindAsync(dto.CPUId) : null;
            var gpu = dto.GPUId.HasValue ? await _context.GPUs.FindAsync(dto.GPUId) : null;
            var ram = dto.RAMId.HasValue ? await _context.RAMs.FindAsync(dto.RAMId) : null;
            var mb = dto.MotherboardId.HasValue ? await _context.Motherboards.FindAsync(dto.MotherboardId) : null;
            var ssd = dto.SSDId.HasValue ? await _context.SSDs.FindAsync(dto.SSDId) : null;
            var hdd = dto.HDDId.HasValue ? await _context.HDDs.FindAsync(dto.HDDId) : null;
            var psu = dto.PSUId.HasValue ? await _context.PSUs.FindAsync(dto.PSUId) : null;
            var pcCase = dto.CaseId.HasValue ? await _context.Cases.FindAsync(dto.CaseId) : null;
            var paste = dto.ThermalPasteId.HasValue ? await _context.ThermalPastes.FindAsync(dto.ThermalPasteId) : null;

            // Подсчёт итоговой цены
            decimal total = 0;
            total += cpu?.Price ?? 0;
            total += gpu?.Price ?? 0;
            total += ram?.Price ?? 0;
            total += mb?.Price ?? 0;
            total += ssd?.Price ?? 0;
            total += hdd?.Price ?? 0;
            total += psu?.Price ?? 0;
            total += pcCase?.Price ?? 0;
            total += paste?.Price ?? 0;

            var build = new PCBuild
            {
                Name = dto.Name,
                CPUId = dto.CPUId,
                GPUId = dto.GPUId,
                RAMId = dto.RAMId,
                MotherboardId = dto.MotherboardId,
                SSDId = dto.SSDId,
                HDDId = dto.HDDId,
                PSUId = dto.PSUId,
                CaseId = dto.CaseId,
                ThermalPasteId = dto.ThermalPasteId,
                TotalPrice = total,

                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            };

            _context.PCBuilds.Add(build);
            await _context.SaveChangesAsync();

            return Ok(build);
        }

        // Удаление сборки
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
