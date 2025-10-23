namespace PCConfigurator.api.Models
{
    public class PSU
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Wattage { get; set; }
        public decimal Price { get; set; }
    }
}
