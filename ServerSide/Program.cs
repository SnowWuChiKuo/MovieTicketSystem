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

			//���UMovieController�����򥦪���@
			builder.Services.AddScoped<IMovieDao, MovieDao>();
			builder.Services.AddScoped<IMovieService, MovieService>();

			//���UGenreController�����򥦪���@
			builder.Services.AddScoped<IGenreDao, GenreDao>();
			builder.Services.AddScoped<IGenreService, GenreService>();

			//���UReviewController�����򥦪���@
			builder.Services.AddScoped<IReviewDao, ReviewDao>();
			builder.Services.AddScoped<IReviewService, ReviewService>();

            // �s�W Member �����A��
            builder.Services.AddScoped<MemberService>();
            builder.Services.AddScoped<MemberDao>();

            // �s�W Ticket �����A��
            builder.Services.AddScoped<TicketService>();
			builder.Services.AddScoped<TicketDao>();

			// �s�W TicketSeat �����A��
			builder.Services.AddScoped<TicketSeatService>();
			builder.Services.AddScoped<TicketSeatDao>();

            // �s�W Seat �����A��
            builder.Services.AddScoped<SeatService>();
            builder.Services.AddScoped<SeatDao>();

            // �s�W SeatStatus �����A��
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
