﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleECA.Entities;
using SimpleECA.Helpers;
using SimpleECA.Helpers.Authentication;
using SimpleECA.Repos;
using SimpleECA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleECA.WEB
{
    public class Infras
    {
        public static void AddDatabase(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<SimpleECADbContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            });
        }
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<AppSettingsHelper, AppSettingsHelper>();
            var secret = new Secret();
            configuration.Bind("Secret", secret);
            services.AddSingleton(secret);

            var googleSecret = configuration.GetSection("Authentication:Google").Get<GoogleSecrets>();
            services.AddSingleton(googleSecret);

            var fbSecret = configuration.GetSection("Authentication:FaceBook").Get<FaceBookSecrets>();
            services.AddSingleton(fbSecret);

            services.AddSession();
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //ClaimsHelper.AppAuthetication(services);

            services.AddTransient<UserResolverService>();
            services.AddTransient<ISaveFileToLocal, SaveFileToLocal>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAuthRepo, AuthRepo>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<IProductRepo, ProductRepo>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IAdminRepo, AdminRepo>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddMvc();
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/u/login";
                })
                    .AddGoogle(options =>
                    {
                        var googleAuthNSection = configuration.GetSection("Authentication:Google").Get<GoogleSecrets>();
                        options.ClientId = googleAuthNSection.client_id;
                        options.ClientSecret = googleAuthNSection.client_secret;
                    })
                    .AddFacebook(opts =>
                    {
                        var fbAuthNSection = configuration.GetSection("Authentication:FaceBook").Get<FaceBookSecrets>();
                        opts.AppId = fbAuthNSection.app_id;
                        opts.AppSecret = fbAuthNSection.app_secret;
                    });
        }
    }
}
