using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TweetClassifer.Web.Hubs;
using TweetClassifer.Web.Services;

namespace TweetClassifer.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader()
                        .WithOrigins("http://localhost:5000")
                        .AllowCredentials();
                }));

            services.AddSingleton<TweetTableStorage>();
            services.AddMvc();
            services.AddSignalR();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseSignalR((builder) =>
            {
                builder.MapHub<MainHub>("/tweetsClass");
            });

            app.UseMvc();
        }
    }
}
