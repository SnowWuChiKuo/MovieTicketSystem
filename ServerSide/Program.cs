using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Infra;
using ServerSide.Models.Interfaces;
using ServerSide.Models.Services;

namespace ServerSide
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

            // Configure App Configuration
            var configuration = builder.Configuration;
            HashUtility.SetConfiguration(configuration);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

			//註冊MovieController介面跟它的實作
			builder.Services.AddScoped<IMovieDao, MovieDao>();
			builder.Services.AddScoped<IMovieService, MovieService>();

			//註冊GenreController介面跟它的實作
			builder.Services.AddScoped<IGenreDao, GenreDao>();
			builder.Services.AddScoped<IGenreService, GenreService>();

			//註冊ReviewController介面跟它的實作
			builder.Services.AddScoped<IReviewDao, ReviewDao>();
			builder.Services.AddScoped<IReviewService, ReviewService>();

            // 新增 Member 相關服務
            builder.Services.AddScoped<MemberService>();
            builder.Services.AddScoped<MemberDao>();

            // 新增 Ticket 相關服務
            builder.Services.AddScoped<TicketService>();
			builder.Services.AddScoped<TicketDao>();

			// 新增 TicketSeat 相關服務
			builder.Services.AddScoped<TicketSeatService>();
			builder.Services.AddScoped<TicketSeatDao>();

            // 新增 Seat 相關服務
            builder.Services.AddScoped<SeatService>();
            builder.Services.AddScoped<SeatDao>();

            // 新增 SeatStatus 相關服務
            builder.Services.AddScoped<SeatStatusService>();
            builder.Services.AddScoped<SeatStatusDao>();

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

   //     public static IHostBuilder CreateHostBuilder(string[] args) =>
			//Host.CreateDefaultBuilder(args)
   //        .ConfigureAppConfiguration((hostingContext, config) =>
   //        {
   //            var configuration = config.Build();
   //            HashUtility.SetConfiguration(configuration);
   //        })
   //        .ConfigureWebHostDefaults(webBuilder =>
   //        {
   //            webBuilder.UseStartup<Startup>();
   //        });
    }
}
