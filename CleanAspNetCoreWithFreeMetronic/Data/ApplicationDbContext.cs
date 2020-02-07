using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CleanAspNetCoreWithFreeMetronic.Models;

namespace CleanAspNetCoreWithFreeMetronic.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<JobTask> JobTasks { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TaskFile> TaskFiles { get; set; }
        
    }
}