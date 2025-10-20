namespace PCConfigurator.API.Models
{
    public class GPU
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int MemoryGB { get; set; }
        public decimal Price { get; set; }
    }
}
