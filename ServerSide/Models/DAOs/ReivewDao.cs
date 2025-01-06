using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class ReviewDao : IReviewDao
    {
        private readonly AppDbContext _db;
        public ReviewDao(AppDbContext db)
        {
            _db = db;
        }
        public List<ReviewVm> GetAllReview()
        {
            var indexData =
                _db.Reviews.Include(r => r.Movie)
                    .Include(r => r.Member)
                    .Select(r => new ReviewVm
                    {
                        Id = r.Id,
                        MovieId = r.MovieId,
                        MovieTitle = r.Movie.Title,
                        MemberId = r.MemberId,
                        MemberName = r.Member.Name,
                        OrderId = r.OrderId,
                        Rating = r.Rating,
                        Comment = r.Comment,
                        CreatedAt = r.CreatedAt,
                        UpdatedAt = r.UpdatedAt
                    })
                    .ToList();
            return indexData;
        }

        public ReviewDto FindReviewById(int id)
        {
            //先關聯其他表格，再找出id相符的資料，並用FirstOrDefault取出第一筆
            var dbReviewToDto =
                _db.Reviews.Include(r => r.Movie).Include(r => r.Member).Where(r => r.Id == id)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    MovieId = r.MovieId,
                    MovieTitle = r.Movie.Title,
                    MemberId = r.MemberId,
                    MemberName = r.Member.Name,
                    OrderId = r.OrderId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                })
                .FirstOrDefault();
            if(dbReviewToDto == null)
            {
                throw new Exception("該筆資料不存在或已被刪除!");
            }
            return dbReviewToDto;
        }
        public void Create(ReviewDto dto)
        {
            var review = ConvertToEfEntity(dto);
            _db.Reviews.Add(review);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(ReviewDto dto)
        {
            var review = _db.Reviews.Find(dto.Id);
            if(review == null) throw new Exception("該筆資料不存在或已被刪除!");
            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            _db.SaveChanges();
        }

        public ReviewDto ConvertToDto(Review reviewInDb)
        {
            return new ReviewDto
            {
                Id = reviewInDb.Id,
                MovieId = reviewInDb.MovieId,
                MemberId = reviewInDb.MemberId,
                OrderId = reviewInDb.OrderId,
                Rating = reviewInDb.Rating,
                Comment = reviewInDb.Comment,
                CreatedAt = reviewInDb.CreatedAt,
                UpdatedAt = reviewInDb.UpdatedAt

            };
        }

        public Review ConvertToEfEntity(ReviewDto dto)
        {
            return new Review
            {
                Id = dto.Id,
                MovieId = dto.MovieId,
                MemberId = dto.MemberId,
                OrderId = dto.OrderId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

    }
}
