using Azure.Core;
using Elfie.Serialization;
using Humanizer;
using Lab7.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System;
using System.Reflection;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.PortableExecutable;
using System.Security.Policy;

namespace Lab7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            /* Define services here */
            var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<StudentDbContext>(options => options.UseSqlServer(connection));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Students API",
                    Description = @"A simple example ASP.NET Core Web API.

**Deliverable Answers**

**1. HTTP Status Codes Used:**
- 200 OK: The request has succeeded.
- 201 Created: The request has been fulfilled and resulted in a new resource being created.
- 400 Bad Request: The server could not understand the request due to invalid syntax.
- 404 Not Found: The server can not find the requested resource.
- 500 Internal Server Error: The server encountered an unexpected condition.

**2. REST Principles Compliance:**
Yes, the service adheres to REST principles:
- **Statelessness**: Each request from the client contains all necessary information.
- **Client-Server Separation**: The client and server are independent and interact via HTTP.
- **Cacheability**: Standard HTTP caching could be applied.
- **Uniform Interface**: Uses standard HTTP methods and resource URIs.
- **Layered System**: The client is unaware of whether it’s connected directly to the server or an intermediary."

                });
                options.EnableAnnotations();
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<StudentDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Students API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            /* Define routing here */
            app.MapControllers();
            app.Run();
        }
    }
}
