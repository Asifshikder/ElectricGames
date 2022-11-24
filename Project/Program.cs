using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Services;

namespace Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services= builder.Services;
            var Configuration=builder.Configuration;
            services.AddDbContext<ProjectContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("mssql"));
                option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            services.AddSingleton<IFileHandler, FileHandler>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}