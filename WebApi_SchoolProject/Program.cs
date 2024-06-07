
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi_SchoolProject.Services;

namespace WebApi_SchoolProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Retrieve the secret key in the JSON File
            var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables();
            var configuration = configurationBuilder.Build();

            var builder = WebApplication.CreateBuilder(args);

            // Authorization : Define policies for authorization based on user claims.
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminDepartement", policy => policy.RequireClaim("Departement","Admin"));
                options.AddPolicy("RequireStudentDepartement", policy => policy.RequireClaim("Departement", "Students"));
            });


            builder.Services.AddControllers();
            //Add the DB context to use the database
            builder.Services.AddDbContext<SchoolContext>(options =>
            {
                options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SchoolProject",
                    providerOptions => providerOptions.EnableRetryOnFailure());
            }
            );

            // Add scoped services : AuthService and AccountService.
            // need to be here to be used in the constructor of each Controller.
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<StudentService>();
            builder.Services.AddScoped<TransactionManagerService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "School Project API", Version = "v1" });

            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
