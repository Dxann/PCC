namespace PCConfigurator.API.Models
{
    public class CPU
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Manufacturer { get; set; } = ""; // Intel / AMD
        public string Socket { get; set; } = "";       // LGA1700, AM5 и т.п.
        public int Cores { get; set; }
        public int Threads { get; set; }
        public float BaseFrequency { get; set; }       // в ГГц
        public float BoostFrequency { get; set; }      // в ГГц
        public int TDP { get; set; }                   // в ваттах
        public string IntegratedGraphics { get; set; } = ""; // если есть
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public string ShortDescription { get; set; } = "";
    }
}
