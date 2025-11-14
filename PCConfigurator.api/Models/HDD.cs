namespace PCConfigurator.API.Models
{
    public class HDD
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public int Capacity { get; set; }              // ГБ или ТБ
        public int RPM { get; set; }                   // скорость вращения
        public int Cache { get; set; }                 // МБ
        public string Interface { get; set; } = "";    // SATA, SAS
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public string ShortDescription { get; set; } = "";
    }
}
