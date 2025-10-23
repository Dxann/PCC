namespace PCConfigurator.api.Models
{
    public class ThermalPaste
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal VolumeML { get; set; }
        public decimal Price { get; set; }
    }
}
