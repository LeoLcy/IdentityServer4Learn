using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Leo.LearnIdentityServer4.MVCClient
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
            services.AddControllersWithViews();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc_client";
                    options.ClientSecret = "mvc_client_secret";
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.Scope.Add("leo.ld4Learn.Api");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            /*
             * app.UseAuthentication();//��Ȩ�������û�е�¼����¼����˭����ֵ��User
             * app.UseAuthorization();//������Ȩ�����Ȩ��
             * ��.net 2.1����û��UseAuthorization�����ģ����������ʳ���ʮ�����ƣ����һ�����һ����֣��ܶ�ʱ�����׸���ˡ�
             * ��3.0֮��΢����ȷ�İ���Ȩ������ȡ����Authorization�м�������������Ҫ��UseAuthentication֮���ٴ�UseAuthorization�����򣬵���ʹ����Ȩ���ܱ���ʹ��[Authorize]���Ե�ʱ��ϵͳ�ͻᱨ��
             */
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute()
                        .RequireAuthorization();
                //equireAuthorization method disables anonymous access for the entire application.
                // You can also use the [Authorize] attribute, if you want to specify that on a per controller or action method basis.
            });
        }
    }
}
