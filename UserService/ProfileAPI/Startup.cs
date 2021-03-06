﻿using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfileAPI.Services;

namespace ProfileAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<DataAccess.ProfileContext>(c => c.UseSqlServer(Configuration.GetConnectionString("ProfileDBConnectionString")));
            services.AddScoped<IProfileRepository, ProfileRepository>();

            services.AddSwaggerDocument();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                   .AddIdentityServerAuthentication(options =>
                   {
                       options.Authority = "http://localhost:53084";
                       options.RequireHttpsMetadata = false;

                       options.ApiName = "ProfileAPI";
                   }
               );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseAuthentication();
            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
