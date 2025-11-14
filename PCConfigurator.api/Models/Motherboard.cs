namespace PCConfigurator.API.Models
{
    public class Motherboard
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string Socket { get; set; } = "";
        public string Chipset { get; set; } = "";
        public string FormFactor { get; set; } = "";    // ATX, mATX, ITX
        public int RAMSlots { get; set; }
        public string RAMType { get; set; } = "";       // DDR4 / DDR5
        public int MaxRAM { get; set; }                 // ГБ
        public bool HasM2 { get; set; }
        public bool HasPCIe5 { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public string ShortDescription { get; set; } = "";
    }
}
