using AutoMapper;
using ConwayGameOfLife.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConwayGameOfLife.Infrastructure.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Board> Boards => Set<Board>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
                .HasKey(b => b.Id);

            var boolArrayConverter = new ValueConverter<bool[,], string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<bool[,]>(v, (JsonSerializerOptions)null)
            );

            modelBuilder.Entity<Board>()
                .Property(b => b.Cells)
                .HasConversion(boolArrayConverter);

            base.OnModelCreating(modelBuilder);
        }
    }
}
