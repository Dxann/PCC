using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PCConfigurator.api.Data;
using PCConfigurator.api.Models;
using PCConfigurator.API.Data;
using PCConfigurator.API.Models;

namespace PCConfigurator.api.Data
{
    public static class DatabaseSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.GPUs.Any())
                LoadFromJson<GPU>(context, "gpu.json", context.GPUs);

            if (!context.CPUs.Any())
                LoadFromJson<CPU>(context, "cpu.json", context.CPUs);

            if (!context.RAMs.Any())
                LoadFromJson<RAM>(context, "ram.json", context.RAMs);

            if (!context.Motherboards.Any())
                LoadFromJson<Motherboard>(context, "motherboard.json", context.Motherboards);

            if (!context.Cases.Any())
                LoadFromJson<Case>(context, "case.json", context.Cases);

            if (!context.PSUs.Any())
                LoadFromJson<PSU>(context, "psu.json", context.PSUs);

            if (!context.SSDs.Any())
                LoadFromJson<SSD>(context, "ssd.json", context.SSDs);

            if (!context.HDDs.Any())
                LoadFromJson<HDD>(context, "hdd.json", context.HDDs);

            if (!context.ThermalPastes.Any())
                LoadFromJson<ThermalPaste>(context, "thermalpaste.json", context.ThermalPastes);

            context.SaveChanges();
        }

        private static void LoadFromJson<T>(ApplicationDbContext context, string fileName, DbSet<T> table)
            where T : class
        {
            var path = Path.Combine("SeedData", fileName);

            if (!File.Exists(path))
                return;

            var json = File.ReadAllText(path);
            var items = JsonSerializer.Deserialize<List<T>>(json);

            if (items != null)
            {
                table.AddRange(items);
            }
        }
    }
}
