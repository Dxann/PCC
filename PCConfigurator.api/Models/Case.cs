namespace PCConfigurator.API.Models
{
    public class Case
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string FormFactor { get; set; } = "";   // ATX, mATX, ITX
        public float MaxGPULength { get; set; }        // мм
        public float MaxCPUCoolerHeight { get; set; }  // мм
        public int FanSupport { get; set; }            // количество вентиляторов
        public bool HasRGB { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public string ShortDescription { get; set; } = "";
    }
}
