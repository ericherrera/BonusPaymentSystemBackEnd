using BonusPaymentSystem.Api.Filters;
using BonusPaymentSystem.Core.Data;
using BonusPaymentSystem.Core.Model;
using BonusPaymentSystem.Service;
using BonusPaymentSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Api
{
    public class Startup
    {
        private const string ConnectionString = "DefaultConnection";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddDbContext<BpsAppContext>(options =>
                    options.UseSqlServer(
             Configuration.GetConnectionString(ConnectionString)));


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };

            });

            //Start Identity Context

            //add this: register your db context
            services.AddDbContext<BpsIdentityContext>();

            //and this: add identity and create the db
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BpsIdentityContext>()
                .AddDefaultTokenProviders();

            //End Identitty Context

            services.AddScoped<IUserService, UserService>(prop => new UserService(Configuration.GetConnectionString(ConnectionString)));
            services.AddScoped<IGenericService<Campaing>, CampaingService>(prop => new CampaingService(Configuration.GetConnectionString(ConnectionString)));
            services.AddScoped<IGenericService<Sale>, SaleService>(prop => new SaleService(Configuration.GetConnectionString(ConnectionString)));
            services.AddScoped<IGenericService<Payment>, PaymentService>(prop => new PaymentService(Configuration.GetConnectionString(ConnectionString)));
            services.AddScoped<IGenericService<Parameter>, ParameterService>(prop => new ParameterService(Configuration.GetConnectionString(ConnectionString)));

            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.Filters.Add<AuthCustomFiltercs>();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BonusPaymentSystem.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //add this
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BonusPaymentSystem.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
