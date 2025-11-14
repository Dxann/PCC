namespace PCConfigurator.API.Models
{
    public class RAM
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public string Type { get; set; } = "";         // DDR4 / DDR5
        public int Capacity { get; set; }              // ГБ (на модуль)
        public int Modules { get; set; }               // количество модулей в комплекте
        public int Frequency { get; set; }             // МГц
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public string ShortDescription { get; set; } = "";
    }
}
