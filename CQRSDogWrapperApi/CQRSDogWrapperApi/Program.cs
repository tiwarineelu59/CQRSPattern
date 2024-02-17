using CQRSDogWrapperApi.Application.Commands;
using CQRSDogWrapperApi.Application.Queries;
using Microsoft.AspNetCore.Hosting;
using CQRSDogWrapperApi.Infrastructure.Data;
using CQRSDogWrapperApi.Application.Services;
using CQRSDogWrapperApi.Infrastructure.Contract;
using Microsoft.Extensions.Configuration;
using MediatR;
using System.Reflection;
using CQRSDogWrapperApi.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace CQRSDogWrapperApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();


            builder.Services.AddHttpClient();

            // Add MediatR for CQRS
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            builder.Services.AddScoped<IDogRepository, DogRepository>();
            builder.Services.AddScoped<DogService>();
            builder.Services.AddScoped<IRequestHandler<GetCachedDogImageUrlQuery, string>, GetCachedDogImageQueryHandler>();
            builder.Services.AddScoped<IRequestHandler<GetRandomDogImageUrlCommand, string>, GetDogImageCommandHandler>();
            builder.Services.AddSingleton<DapperContext>();

            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = false,
            //        ValidIssuer = "phoesion.devjwt", // Replace with your issuer
            //        ValidAudience = "DogApi", // Replace with your audience
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("abcdefghijklmnopqrstuvwxyz123456")) // Replace with your secret key
            //    };
            //});




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Dog API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
