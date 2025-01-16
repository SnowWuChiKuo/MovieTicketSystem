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
				WHERE m.ReleaseDate < GETDATE()
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

		public List<AllMoviesDto> GetAllMovies()
		{
			string sql = @"
						SELECT m.Id,m.Title, m.ReleaseDate, m.PosterURL
						FROM Movies m
						ORDER BY m.ReleaseDate DESC";
			using (var conn = new SqlConnection(_conn))
			{
				return conn.Query<AllMoviesDto>(sql).ToList();
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
					SELECT 
						r.Id, 
						m.Name as MemberName,
						r.Comment, 
						r.CreatedAt, 
						r.Rating
					FROM Reviews r
					JOIN Members m ON r.MemberId = m.Id
					WHERE r.MovieId = @MovieId
					ORDER BY r.CreatedAt DESC";


			using (var conn = new SqlConnection(_conn))
			{
				var movie = conn.QueryFirstOrDefault<MovieDetailDto>(movieSql, new { Id = id });

				if (movie != null)
				{
					movie.Reviews = conn.Query<ReviewDisplayVm>(reviewsSql, new { MovieId = id })
						.Select(r => new ReviewDisplayVm
						{
							Id = r.Id,
							MemberName = r.MemberName,
							Comment = r.Comment,
							CreatedAt = r.CreatedAt,
							Rating = r.Rating
						}).ToList();
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
			string checkOrderSql = @"
				SELECT COUNT(1)
				FROM Orders o
				JOIN Members m ON o.MemberId = m.Id
				JOIN OrderItems oi ON o.Id = oi.OrderId
				JOIN Tickets t ON oi.TicketId = t.Id
				JOIN Screenings s ON t.ScreeningId = s.Id
				WHERE m.Account = @Account 
				AND s.MovieId = @MovieId
				AND o.Status = 1";

			using (var conn = new SqlConnection(_conn))
			{
				int orderCount = conn.ExecuteScalar<int>(checkOrderSql, 
					new { Account = userAccount, MovieId = movieId });
				return orderCount > 0;
			}
		}


		public ReviewDisplayVm AddReview(string account,ReviewCreateDto dto)
		{

			using (var conn = new SqlConnection(_conn))
			{
				//找是否有已存在評論
				string checkReviewSql = @"
						SELECT COUNT(1)
						FROM Reviews r
						JOIN Members m ON r.MemberId = m.Id
						WHERE m.Account = @Account AND r.MovieId = @MovieId";

                //executeScalar 回傳第一列第一行的值，檢查是否有已存在的評論
                int existingReview = conn.ExecuteScalar<int>(checkReviewSql, new { Account = account,dto.MovieId});

				if(existingReview > 0)
				{
					throw new InvalidOperationException("您已評論過此電影!");
				}

				//找OrderId
				string orderSql = @"
					SELECT TOP 1 o.Id
					FROM Members m
					JOIN Orders o ON m.Id = o.MemberId
					JOIN OrderItems oi ON o.Id = oi.OrderId
					JOIN Tickets t ON oi.TicketId = t.Id
					JOIN Screenings s ON t.ScreeningId = s.Id
					WHERE m.Account = @Account
					AND s.MovieId = @MovieId
					AND o.Status = 1";
				var orderId = conn.QueryFirstOrDefault<int>(orderSql, new {Account = account,dto.MovieId});

				if(orderId == 0)
                {
                    throw new InvalidOperationException("找不到有效訂單");
                }

                //新增評論的SQL
                string insertReviewSql = @"
						INSERT INTO Reviews(MovieId,MemberId,OrderId,Rating,Comment,CreatedAt)
							SELECT
							@MovieId,
							m.Id,
							@OrderId,
							@Rating,
							@Comment,
							GETDATE()
						FROM Members m 
						WHERE m.Account = @Account
						SELECT CAST(SCOPE_IDENTITY() as int)";
				var reviewId = conn.QuerySingle<int>(insertReviewSql, new
				{
					dto.MovieId,
					Account = account,
					OrderId = orderId,
					dto.Rating,
					dto.Comment
				});
				//取得新增的評論
				string getReviewSql = @"
				        SELECT 
				            r.Id,
				            m.Name as MemberName,
				            r.Comment,
				            r.CreatedAt,
				            r.Rating,
				            r.OrderId
				        FROM Reviews r
				        JOIN Members m ON r.MemberId = m.Id
				        WHERE r.Id = @ReviewId";

				return conn.QuerySingle<ReviewDisplayVm>(getReviewSql, new { ReviewId = reviewId });

			}

		}

	}
}