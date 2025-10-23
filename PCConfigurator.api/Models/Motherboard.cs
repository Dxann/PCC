namespace PCConfigurator.api.Models
{
    public class Motherboard
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Cores { get; set; }
        public float Frequency { get; set; }
        public decimal Price { get; set; }
    }
}
