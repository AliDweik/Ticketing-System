
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TicketingSystem.API.Handlers;
using TicketingSystem.API.Requierments;
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

            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //builder.Services.AddControllersWithViews();

            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddScoped<IAuthRepo, AuthRepo>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<ITicketRepo, TicketRepo>();
            builder.Services.AddScoped<ITicketAssignmentRepo, TicketAssignmentRepo>();
            builder.Services.AddScoped<ITicketAttachmentRepo, TicketAttachmentRepo>();
            builder.Services.AddScoped<ITicketCommnetRepo, TicketCommentRepo>();
            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
            
            
            builder.Services.AddEndpointsApiExplorer();
           
            builder.Services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Ticketing System",
                    Version = "v1",
                });
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter Token",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },[]
                    }
                });
            });

            var connectionString = builder.Configuration.GetConnectionString("Default Connection");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("TicketingSystem.Data"))
            );

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                };
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IAuthorizationHandler, UserActiveHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, UserWithoutTicketHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, UserWithTicketHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, AttachmentHandler>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("UserWithTicket", policy =>
                    policy.Requirements.Add(new UserWithTicketRequirement()));

                options.AddPolicy("UserWithoutTicket", policy =>
                    policy.Requirements.Add(new UserWithoutTicketRequirement()));

                options.AddPolicy("UserActive", policy =>
                    policy.Requirements.Add(new UserRequirement()));

                options.AddPolicy("Attachment", policy =>
                    policy.Requirements.Add(new AttachmentRequirement()));
            });

            

            builder.Services.AddHealthChecks();

            var allowedOrigins = builder.Configuration.GetValue<string>("allowedOrigins")!.Split(",");

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors();
            app.MapControllers();

            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
