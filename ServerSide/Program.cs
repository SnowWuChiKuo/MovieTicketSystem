using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DAOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;
using ServerSide.Models.Services;

namespace ServerSide
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			//註冊MovieController介面跟它的實作
			builder.Services.AddScoped<IMovieService, MovieService>();
			builder.Services.AddScoped<IMovieDao, MovieDao>();

			//註冊GenreController介面跟它的實作
			builder.Services.AddScoped<IGenreDao, GenreDao>();
			builder.Services.AddScoped<IGenreService, GenreService>();

			// 新增 Ticket 相關服務
			builder.Services.AddScoped<TicketService>();
			builder.Services.AddScoped<TicketDao>();

			// 新增 TicketSeat 相關服務
			builder.Services.AddScoped<TicketSeatService>();
			builder.Services.AddScoped<TicketSeatDao>();

            // 新增 Seat 相關服務
            builder.Services.AddScoped<SeatService>();
            builder.Services.AddScoped<SeatDao>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
			builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
