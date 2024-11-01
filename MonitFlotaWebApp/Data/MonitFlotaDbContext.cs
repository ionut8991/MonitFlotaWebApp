using Microsoft.EntityFrameworkCore;

namespace MonitFlotaWebApp.Data
{
    public class MonitFlotaDbContext : DbContext
    {
       // public DbSet<Job> Jobs { get; set; }
       // public DbSet<Vehicle> Vehicles { get; set; }

        public MonitFlotaDbContext(DbContextOptions<MonitFlotaDbContext> options) : base(options) { }
    }
    
}
