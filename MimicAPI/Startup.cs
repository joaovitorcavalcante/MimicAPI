using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MimicAPI.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MimicAPI.V1.Repositories.Contracts;
using MimicAPI.V1.Repositories;
using AutoMapper;
using MimicAPI.Helpers;

namespace MimicAPI
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
            // Start - AutoMapper Configuration
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile(new DTOMapperProfile());
            });

            IMapper mapper = config.CreateMapper();

            services.AddSingleton(mapper);
            // End - AutoMapper Configuration

            services.AddDbContext<MimicContext>(opt => {
                opt.UseSqlite("Data Source=Database\\Mimic.db");
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddApiVersioning(cfg => {
                cfg.ReportApiVersions = true;
                cfg.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });

            services.AddSwaggerGen(cfg => {
                cfg.ResolveConflictingActions(apiDescription => apiDescription.First());
                cfg.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info() {
                    Title = "MimicAPI - V1",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePages();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(cfg => {
                cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "MimicAPI");
                cfg.RoutePrefix = String.Empty;
            });
        }
    }
}
