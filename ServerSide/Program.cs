using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
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

			//註冊MoviesController介面跟它的實作
			builder.Services.AddScoped<IMovieDao, MovieDao>();
			builder.Services.AddScoped<IMovieService, MovieService>();

			//註冊GenresController介面跟它的實作
			builder.Services.AddScoped<IGenreDao, GenreDao>();
			builder.Services.AddScoped<IGenreService, GenreService>();

			//註冊ReviewsController介面跟它的實作
			builder.Services.AddScoped<IReviewDao, ReviewDao>();
			builder.Services.AddScoped<IReviewService, ReviewService>();

            // 註冊TheatersController介面跟它的實作
            builder.Services.AddScoped<ITheaterDao, TheaterDao>();
            builder.Services.AddScoped<ITheaterService, TheaterService>();

            // 新增 Member 相關服務
            builder.Services.AddScoped<MemberService>();
            builder.Services.AddScoped<MemberDao>();

            // 新增 User 相關服務
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserDao>();

			//註冊PriceController介面跟它的實作
			builder.Services.AddScoped<IPriceService, PriceService>();
			builder.Services.AddScoped<IPriceDao,PriceDao>();

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

            // 新增 Cart 相關服務
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<CartDao>();

            // 新增 Order 相關服務
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<OrderDao>();

            // 新增 OrderItem 相關服務
            builder.Services.AddScoped<OrderItemService>();
            builder.Services.AddScoped<OrderItemDao>();

            // 新增 Coupon 相關服務
            builder.Services.AddScoped<CouponService>();
            builder.Services.AddScoped<CouponDao>();


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
			builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            // 設定 Cookie 表單驗證 ---------------------
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, 
                options =>
                {
                    options.Cookie.Name = "mySite";
                    options.LoginPath = "/Users/Login";
                    options.AccessDeniedPath = "/Users/AccessDenied"; // 如果需要設定拒絕訪問頁面
                    options.ReturnUrlParameter = "returnUrl";
                    //options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // 設定 cookie 有效時間
                });
            // ---------------------------------------

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

            app.UseAuthentication(); // 啟用驗證
            app.UseAuthorization(); // 啟用授權

            app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}

    }
}
