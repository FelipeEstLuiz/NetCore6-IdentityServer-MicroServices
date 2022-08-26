using MicroServices.IdentityServer.Configuration;
using MicroServices.IdentityServer.Initializer;
using MicroServices.IdentityServer.Model;
using MicroServices.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MicroServices.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            services.AddDbContext<SqlServerContext>(
                options => options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(SqlServerContext).Assembly.FullName)
                )
            );

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SqlServerContext>()
                .AddDefaultTokenProviders();

            IIdentityServerBuilder builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResource)
            .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
            .AddInMemoryClients(IdentityConfiguration.Clients)
            .AddAspNetIdentity<ApplicationUser>();

            services.AddScoped<IDbInitializer, DbInitializer>();

            builder.AddDeveloperSigningCredential();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInitializer initializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            initializer.Initialize();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
