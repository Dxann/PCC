using Microsoft.EntityFrameworkCore;
using PCConfigurator.API.Models;

namespace PCConfigurator.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<CPU> CPUs => Set<CPU>();
        public DbSet<GPU> GPUs => Set<GPU>();

        public DbSet<PCBuild> PCBuilds => Set<PCBuild>();


    }
}
