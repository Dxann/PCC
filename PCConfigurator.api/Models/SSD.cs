namespace PCConfigurator.API.Models
{
    public class SSD
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public int Capacity { get; set; }              // ГБ
        public string Interface { get; set; } = "";    // SATA / NVMe
        public string FormFactor { get; set; } = "";   // 2.5", M.2
        public int ReadSpeed { get; set; }             // МБ/с
        public int WriteSpeed { get; set; }            // МБ/с
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public string ShortDescription { get; set; } = "";
    }
}
