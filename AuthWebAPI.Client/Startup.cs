using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthWebAPI.Client
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

            // Add cors
            //services.AddCors();
            
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCors", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials().Build();
                });
            });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            /*
            services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    //In a multi-tenant app, make sure the authority is:
                    o.Authority = "https://login.microsoftonline.com/common";
                    //o.Authority = Configuration["Authentication:Authority"];
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudiences = new List<string>
                        {
                            Configuration["Authentication:AppIdUri"],
                            Configuration["Authentication:ClientId"]
                        },
                        // In multi-tenant apps you should disable issuer validation:
                        // ValidateIssuer = false,
                        // In case you want to allow only specific tenants,
                        // you can set the ValidIssuers property to a list of valid issuer ids
                        // or specify a delegate for the IssuerValidator property, e.g.
                        // IssuerValidator = (issuer, token, parameters) => {}
                        // the validator should return the issuer string
                        // if it is valid and throw an exception if not
                    };
                });
            */
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    /*
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "9bbbcb03-4dc3-44a3-a63e-0b490987f4cc",

                        ValidateAudience = true,
                        ValidAudience = "e591ea5c-357c-4c60-bb3f-29bcb94272e6",
                    };
                    x.Audience = "e591ea5c-357c-4c60-bb3f-29bcb94272e6";
                    */
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidIssuer = "https://rydertruck.onmicrosoft.com/1a4a1483-ad38-493a-8e15-21d7d97eb920",

                        ValidateAudience = false,
                        ValidAudience = "e591ea5c-357c-4c60-bb3f-29bcb94272e6",
                    };
                    x.Authority = "https://login.microsoftonline.com/common";

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
                app.UseHsts();
            }
            app.UseCors("EnableCors");

            //var cors = new EnableCorsAttribute("AllowAll");

            //Configure Cors
            /*
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
            */

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
