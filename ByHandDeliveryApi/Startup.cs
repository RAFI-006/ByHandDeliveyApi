using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ByHandDeliveryApi.Models;
using ByHandDeliveryApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace ByHandDeliveryApi
{
    public class Startup

    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //  services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddCors(options =>
            {
                options.AddPolicy( MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin();
                    });
            });
            services.AddMvc(options => {


                options.OutputFormatters.Clear();
                options.OutputFormatters.Add(new JsonOutputFormatter(new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                }, ArrayPool<char>.Shared));

            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info: new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ByHandDelivery", Version = "v1" });
            });
            services.AddDbContext<db_byhanddeliveryContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OnlineDeliveryConnectionString")));
            services.Configure<ProfileImagePathSetting>(Configuration.GetSection("ProfileImagePath"));
            services.Configure<ProductImagePathSetting>(Configuration.GetSection("ProductImagePath"));
            services.Configure<DocumentImagePathSetting>(Configuration.GetSection("DocumentImagePath"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //HttpConfiguration config = new HttpConfiguration();
            //config.EnableCors();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ByHandDelivery V1");
                c.RoutePrefix = string.Empty;
            });
         //   For browsing Image file from the browser

           app.UseStaticFiles(new StaticFileOptions
           {
               FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), "images")),
               RequestPath = "/images"
           });
            app.UseMvc();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
        }
    }
}
