using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
    public interface IPriceDao
    {
        IEnumerable<PriceIndexVm> GetMovieTixList();
        IEnumerable<PriceVm> GetMovieTixDetail(int id);
        bool IsMovieExist(int movieId);
        void Create(PriceDto dto);
        void Edit(PriceDto dto);
        void Delete(int id);
        PriceDto FindPriceById(int id);
        PriceDto ConvertToDto(Price price);
        Price ConvertToEFEntity(PriceDto dto);

    }
}
