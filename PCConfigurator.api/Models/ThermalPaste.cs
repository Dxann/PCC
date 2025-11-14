namespace PCConfigurator.API.Models
{
    public class ThermalPaste
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Manufacturer { get; set; } = "";
        public float ThermalConductivity { get; set; } // Вт/м·К
        public float Volume { get; set; }              // граммы
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public string ShortDescription { get; set; } = "";
    }
}
