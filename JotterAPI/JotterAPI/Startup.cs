using JotterAPI.DAL;
using JotterAPI.Helpers;
using JotterAPI.Helpers.Abstractions;
using JotterAPI.Services;
using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.Text;
using JotterAPI.Model;

namespace JotterAPI
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
			services.Configure<Hosts>(Configuration.GetSection("Hosts"));
			services.Configure<TokenConfig>(Configuration.GetSection("TokenConfig"));
			services.AddControllers();
			services.AddHttpClient();

			services.AddHttpClient<FileServerClient>();
			services.AddTransient<IFileService, FileService>();
			services.AddTransient<ICategoriesService, CategoriesService>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<INoteService, NotesService>();
			services.AddTransient<IFileWorker, FileSaverHelper>();
			services.AddTransient<IPasswordHasher, PasswordHasher>();
			services.AddTransient<IFileServerClient, FileServerClient>();

			services.AddDbContext<JotterDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("JotterDbContext")));

			var key = Encoding.UTF8.GetBytes(Configuration.GetSection("TokenConfig")["Secret"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

			IdentityModelEventSource.ShowPII = true;
		}
	
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseCors(builder => builder
										.AllowAnyOrigin()
										.AllowAnyMethod()
										.AllowAnyHeader());

			//app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
