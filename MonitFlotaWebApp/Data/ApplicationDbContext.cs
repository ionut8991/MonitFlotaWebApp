using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonitFlotaWebApp.Models;

namespace MonitFlotaWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
