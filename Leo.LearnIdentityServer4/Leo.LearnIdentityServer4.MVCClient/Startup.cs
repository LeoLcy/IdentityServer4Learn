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
             * app.UseAuthentication();//鉴权，检测有没有登录，登录的是谁，赋值给User
             * app.UseAuthorization();//就是授权，检测权限
             * 在.net 2.1中是没有UseAuthorization方法的，这两个单词长的十分相似，而且还经常一起出现，很多时候容易搞混了。
             * 在3.0之后微软明确的把授权功能提取到了Authorization中间件里，所以我们需要在UseAuthentication之后再次UseAuthorization。否则，当你使用授权功能比如使用[Authorize]属性的时候系统就会报错。
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
