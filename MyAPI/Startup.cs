using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using MyAPI.Models;
using MyAPI.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace MyAPI
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
            services
                .AddDbContextPool<MyAPIContext>(options => options
                    .UseMySQL(Configuration.GetConnectionString("DefaultConnection")))
                .AddMvcCore()
                .AddJsonFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                if(context.Request.Path == "/api")
                {
                    await next.Invoke();

                    return;
                }

                var headers = context.Request.Headers;
                if (!headers.ContainsKey("App-Key"))
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("Not found");

                    return;
                }

                var appKey = headers["App-Key"].FirstOrDefault();
                if (appKey.ToLower() != "my-api")
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("Not found");

                    return;
                }

                await next.Invoke();
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
