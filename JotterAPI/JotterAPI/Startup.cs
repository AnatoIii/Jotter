using JotterAPI.DAL;
using JotterAPI.Helpers;
using JotterAPI.Helpers.Abstractions;
using JotterAPI.Services;
using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;

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
			services.AddControllers();

			services.AddTransient<IFileService, FileService>();
			services.AddTransient<ICategoriesService, CategoriesService>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<INoteService, NotesService>();
			services.AddTransient<IFileWorker, FileSaverHelper>();
			services.AddTransient<IPasswordHasher, PasswordHasher>();

			services.AddDbContext<JotterDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("JotterDbContext")));
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

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
