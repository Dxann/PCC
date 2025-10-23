namespace PCConfigurator.api.Models
{
    public class Case
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string FormFactor { get; set; } = "";
        public decimal Price { get; set; }
    }
}
