using ConwayGameOfLife.Core;
using ConwayGameOfLife.Infrastructure.Mapping;
using ConwayGameOfLife.Infrastructure.Repository;
using ConwayGameOfLife.Infrastructure.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using ConwayGameOfLife.API.Middleware;
using Microsoft.EntityFrameworkCore;

namespace ConwayGameOfLife.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string dbPasswordPath = "/run/secrets/ConwayDbPassword";
            string dbPassword = string.Empty;

            if (File.Exists(dbPasswordPath))
            {
                dbPassword = File.ReadAllText(dbPasswordPath).Trim();
            }
            else
            {
                throw new FileNotFoundException($"Secret not found at {dbPasswordPath}");
            }

            var rawConnectionString = builder.Configuration.GetConnectionString("ConwayDb");
            var connectionString = rawConnectionString.Replace("{DB_PASSWORD}", dbPassword);

            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<BoardDtoValidator>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            builder.Services.AddScoped<IGameOfLifeService, GameOfLifeService>();
            builder.Services.AddScoped<IBoardStateRepository, BoardStateRepository>();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<BoardMappingProfile>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
