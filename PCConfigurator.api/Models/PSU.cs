namespace PCConfigurator.API.Models
{
    public class PSU
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public int Power { get; set; }                 // Вт
        public string FormFactor { get; set; } = "";   // ATX, SFX
        public string Efficiency { get; set; } = "";   // 80+ Bronze, Gold и т.д.
        public bool Modular { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public string ShortDescription { get; set; } = "";
    }
}
