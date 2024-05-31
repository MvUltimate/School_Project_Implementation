
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
            //Grab the secret key in the JSON File, it's possible that it has a better method
            var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.secrets.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            var configuration = configurationBuilder.Build();
            var secretKey = configuration["JwtSettings:SecretKey"];

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //Authentication use Jwt Bearer
             builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                //Configure the different parameters 
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,        
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "http://localhost",
                    ValidAudience = "http://localhost",
                    // Set the symmetric key used to validate the signature of the token.
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
            // Authorization : Define policies for authorization based on user claims.
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminDepartement", policy => policy.RequireClaim("Departement","Admin"));
                options.AddPolicy("RequireStudentDepartement", policy => policy.RequireClaim("Departement", "Student"));
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
                // Add security definition for Bearer token.
                // Used on swagger to have graphical indication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                // Add security requirement for Bearer token.
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
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

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
