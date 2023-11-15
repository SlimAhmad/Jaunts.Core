using System.Text;
using Dna;
using Jaunts.Core.Api.Brokers.Storages;
using Jaunts.Core.Api.DI;
using Jaunts.Core.Api.Models.Services.Foundations.Role;
using Jaunts.Core.Api.Models.Services.Foundations.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using static Jaunts.Core.Api.DI.FrameworkConstructionExtensions;
using JsonStringEnumConverter = Newtonsoft.Json.Converters.StringEnumConverter;

namespace Jaunts.Core.Api
{
    public class Startup
    {
        public IConfiguration configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public static void ConfigureServices(IServiceCollection services)
        {

            // Add proper cookie request to follow GDPR 
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for 
                // non-essential cookies is needed for a given request
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllers();
            AddNewtonSoftJson(services);
            services.AddLogging();
            services.AddDbContext<StorageBroker>();
            services.AddBrokers();
            services.AddEmailTemplateSender();
            services.AddFoundationServices();

            // Add JWT Authentication for Api clients
            services.AddAuthentication().
                AddJwtBearer(options =>
                {
                    // Set validation parameters
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Validate issuer
                        ValidateIssuer = true,
                        // Validate audience
                        ValidateAudience = true,
                        // Validate expiration
                        ValidateLifetime = true,
                        // Validate signature
                        ValidateIssuerSigningKey = true,

                        // Set issuer
                        ValidIssuer = Framework.Construction.Configuration["Jwt:Issuer"],
                        // Set audience
                        ValidAudience = Framework.Construction.Configuration["Jwt:Audience"],

                        // Set signing key
                        IssuerSigningKey = new SymmetricSecurityKey(
                            // Get our secret key from configuration
                            Encoding.UTF8.GetBytes(Framework.Construction.Configuration["Jwt:SecretKey"])),
                    };
                });

            // Change password policy
            services.Configure<IdentityOptions>(options =>
            {
                // Make really weak passwords possible
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                // Make sure users have unique emails
                options.User.RequireUniqueEmail = true;


            });
            services.AddIdentity<ApplicationUser, ApplicationRole>()
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

            // Force non-essential cookies to only store
            // if the user has consented
            applicationBuilder.UseCookiePolicy();
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
