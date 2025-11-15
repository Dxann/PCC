using PCConfigurator.API.Models;

namespace PCConfigurator.api.Models
{
    public class PCBuildLike
    {
        public int Id { get; set; }
        public int BuildId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public PCBuild Build { get; set; }
        public ApplicationUser User { get; set; }
    }

}
