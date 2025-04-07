
using AssignmentService.Repository;
using Microsoft.EntityFrameworkCore;

namespace AssignmentService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("AssigmentDbConnect");
            builder.Services.AddDbContext<AssigmentDbContext>(options =>
                                options.UseSqlServer(connectionString)
                                       .EnableSensitiveDataLogging()
                                       .EnableDetailedErrors()
                            );
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
