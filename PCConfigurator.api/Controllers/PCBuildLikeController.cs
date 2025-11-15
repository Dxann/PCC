using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.API.Data;
using PCConfigurator.api.Models;
using System.Security.Claims;

namespace PCConfigurator.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PCBuildLikeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PCBuildLikeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Добавить/убрать лайк
        [HttpPost("{buildId}")]
        public async Task<IActionResult> ToggleLike(int buildId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existing = await _context.PCBuildLikes
                .FirstOrDefaultAsync(l => l.BuildId == buildId && l.UserId == userId);

            if (existing != null)
            {
                // Удаляем лайк
                _context.PCBuildLikes.Remove(existing);
                await _context.SaveChangesAsync();
                return Ok(new { liked = false });
            }

            // Добавляем лайк
            var like = new PCBuildLike
            {
                BuildId = buildId,
                UserId = userId
            };

            _context.PCBuildLikes.Add(like);
            await _context.SaveChangesAsync();

            return Ok(new { liked = true });
        }

        // Список моих лайкнутых сборок
        [HttpGet("my")]
        public async Task<IActionResult> GetMyLikes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var likes = await _context.PCBuildLikes
                .Where(l => l.UserId == userId)
                .Include(l => l.Build)
                .ThenInclude(b => b.CPU)
                .Include(l => l.Build.GPU)
                .Include(l => l.Build.RAM)
                .ToListAsync();

            return Ok(likes);
        }
    }
}
