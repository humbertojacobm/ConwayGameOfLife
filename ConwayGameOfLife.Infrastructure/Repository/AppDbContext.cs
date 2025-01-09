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
            : base(options) { }

        public DbSet<Board> Boards => Set<Board>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
                .HasKey(b => b.Id);

            // Our custom ValueConverter
            var boolArrayConverter = new ValueConverter<bool[,], string>(
                // EF can handle these "method call" expressions 
                twoD => ConvertTwoDToString(twoD),
                json => ConvertStringToTwoD(json)
            );

            modelBuilder.Entity<Board>()
                .Property(b => b.Cells)
                .HasConversion(boolArrayConverter);

            base.OnModelCreating(modelBuilder);
        }

        private static string ConvertTwoDToString(bool[,]? twoD)
        {
            if (twoD is null)
                return System.Text.Json.JsonSerializer.Serialize(Array.Empty<bool[]>());

            int rows = twoD.GetLength(0);
            int cols = twoD.GetLength(1);
            var jagged = new bool[rows][];

            for (int r = 0; r < rows; r++)
            {
                jagged[r] = new bool[cols];
                for (int c = 0; c < cols; c++)
                {
                    jagged[r][c] = twoD[r, c];
                }
            }

            return System.Text.Json.JsonSerializer.Serialize(jagged);
        }

        private static bool[,] ConvertStringToTwoD(string? json)
        {
            if (string.IsNullOrEmpty(json))
                return new bool[0, 0];

            var jagged = System.Text.Json.JsonSerializer.Deserialize<bool[][]>(json);
            if (jagged == null || jagged.Length == 0)
                return new bool[0, 0];

            int rows = jagged.Length;
            int cols = jagged[0].Length;
            var twoD = new bool[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    twoD[r, c] = jagged[r][c];
                }
            }

            return twoD;
        }

    }

}
