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

			//���UMoviesController�����򥦪���@
			builder.Services.AddScoped<IMovieDao, MovieDao>();
			builder.Services.AddScoped<IMovieService, MovieService>();

			//���UGenresController�����򥦪���@
			builder.Services.AddScoped<IGenreDao, GenreDao>();
			builder.Services.AddScoped<IGenreService, GenreService>();

			//���UReviewsController�����򥦪���@
			builder.Services.AddScoped<IReviewDao, ReviewDao>();
			builder.Services.AddScoped<IReviewService, ReviewService>();

            // ���UTheatersController�����򥦪���@
            builder.Services.AddScoped<ITheaterDao, TheaterDao>();
            builder.Services.AddScoped<ITheaterService, TheaterService>();

            // �s�W Member �����A��
            builder.Services.AddScoped<MemberService>();
            builder.Services.AddScoped<MemberDao>();

            // �s�W User �����A��
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<UserDao>();

			//���UPriceController�����򥦪���@
			builder.Services.AddScoped<IPriceService, PriceService>();
			builder.Services.AddScoped<IPriceDao,PriceDao>();

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

            // �s�W Cart �����A��
            builder.Services.AddScoped<CartService>();
            builder.Services.AddScoped<CartDao>();

            // �s�W Order �����A��
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<OrderDao>();

            // �s�W OrderItem �����A��
            builder.Services.AddScoped<OrderItemService>();
            builder.Services.AddScoped<OrderItemDao>();

            // �s�W Coupon �����A��
            builder.Services.AddScoped<CouponService>();
            builder.Services.AddScoped<CouponDao>();


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
			builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            // �]�w Cookie ������� ---------------------
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, 
                options =>
                {
                    options.Cookie.Name = "mySite";
                    options.LoginPath = "/Users/Login";
                    options.AccessDeniedPath = "/Users/AccessDenied"; // �p�G�ݭn�]�w�ڵ��X�ݭ���
                    options.ReturnUrlParameter = "returnUrl";
                    //options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // �]�w cookie ���Įɶ�
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

            app.UseAuthentication(); // �ҥ�����
            app.UseAuthorization(); // �ҥα��v

            app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}

    }
}
