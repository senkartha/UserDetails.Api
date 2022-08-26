using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using UserDetailsBL.Services;
using UserDetailsBL.Interfaces;
using UserDetailsBL.Models;
using UserDetailsDL.Interfaces;
using UserDetailsDL.Repositories;

namespace UserDetails
{
    public class Startup
    {
        string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IConfiguration>(Configuration);

            // Logging
            ILoggerFactory loggerFactory = new LoggerFactory();
            services.AddSingleton<ILoggerFactory>(loggerFactory);
            services.AddSingleton<IRepository<User>, FileRepository<User>>();

            services.AddScoped<IUserDetailsValidator, UserDetailsValidator>();
            services.AddScoped<IDataSourceOperator<User>, DataSourceOperator<User>>();

            services.AddCors(options =>
             {
                 options.AddPolicy(MyAllowSpecificOrigins,
                                       policy =>
                                       {
                                           policy.WithOrigins("http://localhost:4200")
                                                               .AllowAnyHeader()
                                                               .AllowAnyMethod();
                                       });
             });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "User Details API",
                    Description = "An ASP.NET Core Web API for managing User Details",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "User Details Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "User Details License",
                        Url = new Uri("https://example.com/license")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("swagger/v1/swagger.json", "User Details v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
