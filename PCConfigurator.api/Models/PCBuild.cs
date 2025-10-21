using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCConfigurator.API.Models
{
    public class PCBuild
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // Связь с CPU
        public int CPUId { get; set; }
        [ForeignKey("CPUId")]
        public CPU? CPU { get; set; }

        // Связь с GPU
        public int GPUId { get; set; }
        [ForeignKey("GPUId")]
        public GPU? GPU { get; set; }

        //Связь с RAM
        public int RAMId { get; set; }
        [ForeignKey("RAMId")]
        public CPU? RAM { get; set; }
        


        // Итоговая цена сборки (можно вычислять или хранить)
        public decimal TotalPrice { get; set; }

        // Дата создания
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
