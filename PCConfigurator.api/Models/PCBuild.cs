using PCConfigurator.API.Models;

namespace PCConfigurator.api.Models
{
    public class PCBuild
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public int? CPUId { get; set; }
        public CPU? CPU { get; set; }

        public int? GPUId { get; set; }
        public GPU? GPU { get; set; }

        public int? RAMId { get; set; }
        public RAM? RAM { get; set; }

        public int? MotherboardId { get; set; }
        public Motherboard? Motherboard { get; set; }

        public int? SSDId { get; set; }
        public SSD? SSD { get; set; }

        public int? HDDId { get; set; }
        public HDD? HDD { get; set; }

        public int? PSUId { get; set; }
        public PSU? PSU { get; set; }

        public int? CaseId { get; set; }
        public Case? Case { get; set; }

        public int? ThermalPasteId { get; set; }
        public ThermalPaste? ThermalPaste { get; set; }

        public decimal TotalPrice { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
