using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ToastApiReact.Services;

namespace ToastApiReact
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000")
                                             .AllowAnyHeader()
                                             .AllowAnyMethod();
                    });
            });

            services.Configure<ToastersDatabaseSettings>(
                Configuration.GetSection(nameof(ToastersDatabaseSettings)));

            services.AddSingleton<IToastersDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ToastersDatabaseSettings>>().Value);

            services.AddSingleton<ToasterService>();

            /* 

            In the preceding code, the ToasterService class is registered with DI
            to support constructor injection in consuming classes. The singleton
            service lifetime is most appropriate because ToasterService takes a
            direct dependency on MongoClient.
            Per the official Mongo Client reuse guidelines, MongoClient should be registered in DI with a singleton service lifetime.

            placeholder: more neeed here.

            */
            services.AddControllers()
                .AddNewtonsoftJson(Options => Options.UseMemberCasing());
            // this changes default property camel case to pascal case
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
