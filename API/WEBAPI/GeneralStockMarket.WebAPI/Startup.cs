using System;

using GeneralStockMarket.ApiShared.ExtensionMethods;
using GeneralStockMarket.ApiShared.Filters;
using GeneralStockMarket.Bll.Containers.MicrosoftIOC;
using GeneralStockMarket.WebAPI.Services;
using GeneralStockMarket.WebAPI.Services.Interfaces;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace GeneralStockMarket.WebAPI
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDependencies(Configuration, Environment);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    opt.Authority = Environment.GetIdentityServerUrl(Configuration);
                    opt.Audience = "resource_webapi";
                    opt.RequireHttpsMetadata = !Environment.IsDevelopment();
                });


            services.AddHttpClient<IImageService, ImageService>(opt =>
            {
                opt.BaseAddress = new(Environment.GetWebApiUrl(Configuration));
            });

            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter());
                opt.Filters.Add<ValidateModelAttribute>();
            }).AddValidationDependencies();

            services.AddCustomValidationResponse();


            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GeneralStockMarket.WebAPI",
                    Version = "v1",
                    Description = "GeneralStockMarket Ýçin WebApi"
                    //License = new OpenApiLicense
                    //{
                    //    Name = "MIT",
                    //    Url = new Uri("https://github.com/ignaciojvig/ChatAPI/blob/master/LICENSE")
                    //}

                });


                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                s.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });




        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeneralStockMarket.WebAPI v1"));

            app.UseCustomExceptionHandler();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
