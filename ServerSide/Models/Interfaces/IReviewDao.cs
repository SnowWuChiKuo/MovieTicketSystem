using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
    public interface IReviewDao
    {
        void Create(ReviewDto dto);
        void Delete(int id);
        List<ReviewVm> GetAllReview();
        void Edit(ReviewDto dto);

        ReviewDto FindReviewById(int id);
        //List<ReviewDto> GetReviewsByMovieTitle(string movieTitle);
        //List<ReviewDto> GetReviewsByMemberAcc(string memberAcc);
        //List<ReviewDto> SearchReviewsByKeyword(string keyword);
        ReviewDto ConvertToDto(Review reviewInDb);
        Review ConvertToEfEntity(ReviewDto dto);
    }
}
