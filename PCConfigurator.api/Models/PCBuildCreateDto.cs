namespace PCConfigurator.API.Models.DTO
{
	public class PCBuildCreateDto
	{
		public string Name { get; set; } = "";
		public int? CPUId { get; set; }
		public int? GPUId { get; set; }
		public int? RAMId { get; set; }
		public int? MotherboardId { get; set; }
		public int? SSDId { get; set; }
		public int? HDDId { get; set; }
		public int? PSUId { get; set; }
		public int? CaseId { get; set; }
		public int? ThermalPasteId { get; set; }

    }
}
