using GraGieldowa.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraGieldowa.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=C:\\Users\\user\\Desktop\\Informatyka\\GraGieldowa\\GraGieldowa.Model\\Gra.db");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
