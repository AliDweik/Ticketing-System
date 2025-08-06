
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Data.Data;
using TicketingSystem.Data.Repositories.Implements;
using TicketingSystem.Data.Repositories.Interfaces;

namespace TicketingSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddScoped<IAuthRepo, AuthRepo>();
            builder.Services.AddScoped<ITicketRepo, TicketRepo>();
            builder.Services.AddScoped<ITicketAssignmentRepo, TicketAssignmentRepo>();
            builder.Services.AddScoped<ITicketAttachmentRepo, TicketAttachmentRepo>();
            builder.Services.AddScoped<ITicketCommnetRepo, TicketCommentRepo>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("Default Connection");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("TicketingSystem.Data"))
            );

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
