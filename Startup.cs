using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System;
using System.Text;
using WebApi.Dtos;
using WebApi.Extensions;
using WebApi.Helpers;
using WebApi.Refit;
using WebApi.Validation;

namespace WebApi
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

            //JWT token
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            services.AddControllers()
                .AddJsonOptions(options =>
            { 
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                //options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services
                .AddRefitClient<ICepApiService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://viacep.com.br/"));

            services                
                .AddRefitClient<IConselhoApiService>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.adviceslip.com/"));

            /*services.AddControllers()
                .AddFluentValidation(x => x
                    .RegisterValidatorsFromAssemblyContaining<Startup>());*/

            /*services.AddControllers()
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<PessoaDtoValidator>());*/

            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<PessoaDto>, PessoaDtoValidator>();

            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddAutoMapper(typeof(Startup));

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
