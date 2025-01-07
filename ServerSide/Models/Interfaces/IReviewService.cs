using ServerSide.Models.DTOs;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
    public interface IReviewService
    {
        void Create(ReviewDto dto);
        void Delete(int id);
        ReviewDto FindReviewById(int id);
        List<ReviewVm> GetAllReview();
        void Edit(ReviewDto dto);
    }
}
