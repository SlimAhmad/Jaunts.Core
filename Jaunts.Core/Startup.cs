using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using JsonStringEnumConverter = Newtonsoft.Json.Converters.StringEnumConverter;
using Jaunts.Core.Api.Models.Users;
using Jaunts.Core.Api.Brokers.Storages;
using static Jaunts.Core.Api.DI.FrameworkConstructionExtensions;
using Jaunts.Core.Api.DI;

namespace Jaunts.Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
           Configuration = configuration;

        public IConfiguration Configuration { get; }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            AddNewtonSoftJson(services);
            services.AddLogging();
            services.AddDbContext<StorageBroker>();
            services.AddBrokers();
            services.AddFoundationServices();

            services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<StorageBroker>()
                    .AddDefaultTokenProviders();
                    
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo
                    {
                        Title = "Jaunts.Core.Api",
                        Version = "v1"
                    }
                );
            });
        }

        public static void Configure(
            IApplicationBuilder applicationBuilder,
            IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
                applicationBuilder.UseSwagger();
                applicationBuilder.UseSwaggerUI(options =>

                options.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: "Jaunts.Core.Api v1"));
            }

            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseRouting();
            applicationBuilder.UseAuthorization();
            applicationBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
        }

       

        private static void AddNewtonSoftJson(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new JsonStringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        }
    }
}
