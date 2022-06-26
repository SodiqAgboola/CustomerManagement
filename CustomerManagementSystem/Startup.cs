using CustomerManagementSystem.Data;
using CustomerManagementSystem.HelperMethods;
using CustomerManagementSystem.Interfaces;
using CustomerManagementSystem.Repository;
using CustomerManagementSystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerManagementSystem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConfig = configuration;
        }

        public IConfiguration Configuration { get; }
        public static IConfiguration StaticConfig { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("CustomerManagementSystemConString")));

            services.AddLogging();
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerManagementSystem", Version = "v1" });
                c.EnableAnnotations();
                c.OperationFilter<RequiredHeaderFilter>();
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddScoped<ILocation, LocationService>();
            services.AddScoped<ICustomer, CustomerService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<IBankService, BankService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if ((Convert.ToBoolean(Configuration["EnableSwagger"])))
            {
                if (env.IsProduction())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/v1/swagger.json", "CentralLoanBookingAPI v1"));
                }
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CentralLoanBookingAPI v1"));
                }
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
