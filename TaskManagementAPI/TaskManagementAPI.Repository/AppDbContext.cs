using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskManagementAPI.Common.Response;

namespace TaskManagementAPI.Common
{
    public class AppDbContext : DbContext
    {

        public DbSet<TaskDetails> tasks { get; set; }
        public DbSet<Team> teams { get; set; }
        public DbSet<TeamMember> Teammembers { get; set; }
        public DbSet<User> users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }

}
