using ClientSide.Models.EFModels;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ClientSide.Models.DTOs;
using Dapper;
using System.Web.ModelBinding;

namespace ClientSide.Models.Repository
{
	public class MovieRepository
	{
		private readonly AppDbContext _db;
		private string _conn;

		public MovieRepository()
		{
			_db = new AppDbContext();
			_conn = System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext11"].ToString();
		}

		/// <summary>
		/// 取得首頁電影資訊
		/// </summary>
		/// <returns></returns>
		public List<MovieIndexDto >GetIndexMovie()
		{
			string sql = @"SELECT 
							m.Id, m.Title, m.ReleaseDate, m.PosterURL, 
							(SELECT COUNT(*) FROM Reviews WHERE MovieId = m.Id) as ReviewCount
						FROM Movies m
						ORDER BY m.ReleaseDate DESC";
			using (var conn = new SqlConnection(_conn))
			{
				return conn.Query<MovieIndexDto>(sql).ToList();
			}
		}


		/// <summary>
		/// 獲取最新電影
		/// </summary>
		/// <param name="count">要獲取的電影數量</param>
		/// <returns></returns>
		public List<MovieIndexDto> GetLatestMovies()
		{
			//CARD數量固定所以寫死count 取固定筆數的資料
			int count = 8;
			string sql = @"
                SELECT TOP (@Count)
                    m.Id, m.Title, m.ReleaseDate, m.PosterURL,
                    (SELECT COUNT(*) FROM Reviews WHERE MovieId = m.Id) AS ReviewCount
                FROM Movies m
                ORDER BY m.ReleaseDate DESC";

			using (var conn = new SqlConnection(_conn))
			{
				return conn.Query<MovieIndexDto>(sql, new { Count = count }).ToList();
			}
		}

		/// <summary>
		/// 獲取最多評論的電影
		/// </summary>
		/// <param name="count">要獲取的電影數量</param>
		/// <returns></returns>
		public List<MovieIndexDto> GetMostReviewedMovies()
		{
			int count = 8;
			string sql = @"
                SELECT TOP (@Count)
                    m.Id, m.Title, m.ReleaseDate, m.PosterURL,
                    (SELECT COUNT(*) FROM Reviews WHERE MovieId = m.Id) AS ReviewCount
                FROM Movies m
                ORDER BY ReviewCount DESC, m.ReleaseDate DESC";

			using (var conn = new SqlConnection(_conn))
			{
				return conn.Query<MovieIndexDto>(sql, new { Count = count }).ToList();
			}
		}
		/// <summary>
		/// 獲取即將上映的電影
		/// </summary>
		/// <returns></returns>
		public List<MovieIndexDto> GetUpcomingMovies()
		{
			int count = 8;
			string sql = @"
					SELECT TOP (@Count)
						m.Id,m.Title,m.PosterURL,m.ReleaseDate,
						(SELECT COUNT(*) FROM Reviews WHERE Movieid = m.Id) as ReviewCount
						FROM Movies m
					WHERE m.ReleaseDate > GETDATE()
					ORDER BY m.ReleaseDate ASC";
			using (var conn = new SqlConnection(_conn))
			{
				return conn.Query<MovieIndexDto>(sql, new { Count = count }).ToList();
			}
		}


		/// <summary>
		/// 獲取單頁電影資訊
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public MovieDetailDto GetMovieById(int id)
		{
			string movieSql = @"
					SELECT 
						m.Id, m.Title, m.Description, m.Director, m.Cast, 
						m.RunTime, m.ReleaseDate, m.PosterURL,
						g.Name as GenreName,
						r.Name as RatingName,
						(SELECT COUNT(*) FROM Reviews WHERE MovieId = m.Id) as ReviewCount,
						(SELECT AVG(CAST(Rating as FLOAT)) FROM Reviews WHERE MovieId = m.Id) as AverageRating
					FROM Movies m 
					JOIN Genres g ON m.GenreId = g.Id 
					JOIN Ratings r ON m.RatingId = r.Id
					WHERE m.Id = @Id";

			string reviewsSql = @"
					SELECT r.Id, m.Name as MemberName,
						r.Comment, r.CreatedAt, r.Rating
					FROM Reviews r
					JOIN Members m ON r.MemberId = m.Id
					WHERE r.MovieId = @MovieId
					ORDER BY r.CreatedAt DESC";


			using (var conn = new SqlConnection(_conn))
			{
				var  movie = conn.QueryFirstOrDefault<MovieDetailDto>(movieSql, new { Id = id });

				if (movie != null)
				{
					movie.Reviews = conn.Query<ReviewDisplayVm>(reviewsSql, new { MovieId = id }).ToList();
				}

				return movie;
			}
		}


		/// <summary>
		/// 檢查使用者是否有有效訂單，有才可以評論
		/// </summary>
		/// <param name="userAccount"></param>
		/// <param name="movieId"></param>
		/// <returns></returns>
		public bool CheckUserHasValidOrder(string userAccount, int movieId)
		{
			return _db.Orders
				.Any(o => o.Member.Account == userAccount
					 && o.OrderItems.Any(oi => oi.Ticket.Screening.MovieId == movieId)
					 && o.Status == true); // 假設 Status=true 代表訂單完成
		}


		public ReviewDisplayVm AddReview(ReviewCreateDto dto)
		{

			using (var conn = new SqlConnection(_conn))
			{
				//找OrderId
				string orderSql = @"SELECT o.Id 
									FROM Orders o
									JOIN OrderItems oi ON o.Id = oi.OrderId
									JOIN Tickets t ON oi.TicketId = t.Id
									JOIN Screenings s ON t.ScreeningId = s.Id
									WHERE o.MemberId = (SELECT Id FROM Members WHERE Account = @Account)
									AND s.MovieId = @MovieId
									AND o.Status = 1";
				var orderId = conn.QueryFirstOrDefault<int>(orderSql, new { dto.Account,dto.MovieId});


				// 取得會員ID的SQL
				//string memberIdSql = "SELECT Id FROM Members WHERE Account = @Account";
				//var memberId = conn.QuerySingle<int>(memberIdSql,new { Account = dto.Account});

				//新增評論的SQL
				string insertReviewSql = @"INSERT INTO Reviews(MovieId,MemberId,OrderId,Rating,Comment,CreatedAt)
											VALUES(
												@MovieId,
												(SELECT Id FROM Members WHERE Account = @Account),
												@OrderId,
												@Rating,
												@Comment,
												GETDATE()
											)
											SELECT CAST(SCOPE_IDENTITY() as int)";
				var reviewId = conn.QuerySingle<int>(insertReviewSql, new
				{
					MovieId = dto.MovieId,
					Account = dto.Account,
					OrderId = orderId,
					Rating = dto.Rating,
					Comment = dto.Comment
				});
				// 取得新增的評論
				//string reviewSql = @"
				//        SELECT 
				//            r.Id,
				//            m.Name as MemberName,
				//            r.Comment,
				//            r.CreatedAt,
				//            r.Rating,
				//            r.OrderId
				//        FROM Reviews r
				//        JOIN Members m ON r.MemberId = m.Id
				//        WHERE r.Id = @ReviewId";

				//return conn.QuerySingle<ReviewDisplayVm>(reviewSql, new { ReviewId = reviewId });

				return null;

			}

		}

	}
}