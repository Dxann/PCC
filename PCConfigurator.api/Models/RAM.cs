namespace PCConfigurator.API.Models
{
    public class RAM
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int MemoryGB { get; set; }
        public decimal Price { get; set; }
        public int Frequency { get; set; }
        public string TypeDDR { get; set; } = "";
    }
}
