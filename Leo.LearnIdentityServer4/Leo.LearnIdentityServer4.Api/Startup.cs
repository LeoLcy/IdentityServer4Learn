using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Leo.LearnIdentityServer4.Api
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
            services.AddControllers();

            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";
                options.RequireHttpsMetadata = false;

                options.Audience = "leo.ld4Learn.Api";
            });
            //����������
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //˳��Ҫ�ԣ���Ȼ�ᱨ401����
            //AddAuthentication adds the authentication services to DI and configures Bearer as the default scheme.
            //UseAuthentication adds the authentication middleware to the pipeline so authentication will be performed automatically on every call into the host.
            //UseAuthorization adds the authorization middleware to make sure, our API endpoint cannot be accessed by anonymous clients.
            //��֤��������
            app.UseAuthentication();
            //��֤ÿһ��API[Authorize]��ȷ�����ܱ���������
            app.UseAuthorization();

            app.UseCors("default");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
