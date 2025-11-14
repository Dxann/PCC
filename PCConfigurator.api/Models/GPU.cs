namespace PCConfigurator.API.Models
{
    public class GPU
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Manufacturer { get; set; } = "";   // NVIDIA / AMD
        public int MemorySize { get; set; }              // в ГБ
        public string MemoryType { get; set; } = "";     // GDDR6, GDDR6X и т.п.
        public int PowerConsumption { get; set; }        // Вт
        public string PowerConnector { get; set; } = ""; // 8-pin, 16-pin и т.п.
        public float Length { get; set; }                // мм
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public string ShortDescription { get; set; } = "";
    }
}
