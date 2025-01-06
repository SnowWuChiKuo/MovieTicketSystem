using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewDao _dao;
        public ReviewService(IReviewDao dao)
        {
            _dao = dao;
        }
        public List<ReviewVm> GetAllReview()
        {
            return _dao.GetAllReview();
        }

        public void Create(ReviewDto dto)
        {
            if(dto.Rating <= 0) throw new Exception("請填入正確評級!");
            if (dto.CreatedAt == default(DateTime)) dto.CreatedAt = DateTime.Now;
            dto.UpdatedAt = DateTime.Now;
            _dao.Create(dto);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(ReviewDto dto)
        {
            dto.UpdatedAt = DateTime.Now;
            _dao.Edit(dto);
        }

        public ReviewDto FindReviewById(int id)
        {
           return _dao.FindReviewById(id);
        }
    }
}
