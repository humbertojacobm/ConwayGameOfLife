
using ConwayGameOfLife.Core;
using ConwayGameOfLife.Infrastructure.Mapping;
using ConwayGameOfLife.Infrastructure.Repository;
using ConwayGameOfLife.Infrastructure.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using ConwayGameOfLife.API.Middleware;

namespace ConwayGameOfLife.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<BoardDtoValidator>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
