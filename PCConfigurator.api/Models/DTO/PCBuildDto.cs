using PCConfigurator.api.Models;

namespace PCConfigurator.API.Models.DTO
{
    public class PCBuildDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public int Likes { get; set; }

        public CPU? CPU { get; set; }
        public GPU? GPU { get; set; }
        public RAM? RAM { get; set; }
        public Motherboard? Motherboard { get; set; }
        public SSD? SSD { get; set; }
        public HDD? HDD { get; set; }
        public PSU? PSU { get; set; }
        public Case? Case { get; set; }
        public ThermalPaste? ThermalPaste { get; set; }

        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
    }
}
