using Microsoft.EntityFrameworkCore;
using PCConfigurator.api.Models;
using PCConfigurator.API.Models;

namespace PCConfigurator.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<CPU> CPUs => Set<CPU>();
        public DbSet<GPU> GPUs => Set<GPU>();
        public DbSet<RAM> RAMs => Set<RAM>();
        public DbSet<SSD> SSDs => Set<SSD>();
        public DbSet<HDD> HDDs => Set<HDD>();
        public DbSet<Motherboard> Motherboards => Set<Motherboard>();
        public DbSet<PSU> PSUs => Set<PSU>();
        public DbSet<ThermalPaste> ThermalPastes => Set<ThermalPaste>();
        public DbSet<Case> Cases => Set<Case>();

        public DbSet<PCBuild> PCBuilds => Set<PCBuild>();


    }
}
