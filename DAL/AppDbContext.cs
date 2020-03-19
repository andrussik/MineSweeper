using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameBoard> GameBoards { get; set; } = default!;

        public AppDbContext()
        {
        }
        
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        // private static readonly ILoggerFactory MyLoggerFactory
        //     = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(
                    @"Data Source=/Users/andre/RiderProjects/icd0008-2019f/MineSweeper/GameBoards.db");
            }
        }
    }
}