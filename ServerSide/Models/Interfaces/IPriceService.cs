using ServerSide.Models.DTOs;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
    public interface IPriceService
    {
        IEnumerable<PriceIndexVm> GetMovieTixList();
        IEnumerable<PriceVm> GetMovieTixDetail(int movieId);
        bool IsMovieExist(int movieId);
        void Create(PriceDto dto);
        void Edit(PriceDto dto);
        void Delete(int id);
        PriceDto FindPriceById(int id);
    }
}
