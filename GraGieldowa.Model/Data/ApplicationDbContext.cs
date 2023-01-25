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
        public DbSet<OpenPosition> OpenPositions { get; set; }
        public DbSet<ClosedPosition> ClosedPositions { get; set; }
        public DbSet<Config> Configs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dataSource = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Gra.db");
            //optionsBuilder.UseSqlite($"Data Source=C:\\Users\\user\\Desktop\\Informatyka\\GraGieldowa\\GraGieldowa.Model\\Gra.db");
            optionsBuilder.UseSqlite($"Data Source={dataSource}");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
