namespace PCConfigurator.API.Models
{
    public class CPU
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Cores { get; set; }
        public float Frequency { get; set; }
        public decimal Price { get; set; }
    }
}
