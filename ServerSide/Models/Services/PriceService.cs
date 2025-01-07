using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class PriceService : IPriceService
    {
        private readonly IPriceDao _dao;
        public PriceService(IPriceDao dao)
        {
            _dao = dao;
        }
        public void Create(PriceDto dto)
        {

            _dao.Create(dto);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(PriceDto dto)
        {
            _dao.Edit(dto);
        }

        public PriceDto FindPriceById(int id)
        {
            return _dao.FindPriceById(id);
        }

        public IEnumerable<PriceVm> GetMovieTixDetail(int movieId)
        {
            return _dao.GetMovieTixDetail(movieId);
        }

        public IEnumerable<PriceIndexVm> GetMovieTixList()
        {
            return _dao.GetMovieTixList();
        }

        public bool IsMovieExist(int movieId)
        {
            return _dao.IsMovieExist(movieId);
        }
    }
}
