using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class ScreeningService : IScreeningService
    {
        private readonly IScreeningDao _dao;
        public ScreeningService(IScreeningDao dao)
        {
            _dao = dao;
        }
        public void Create(ScreeningDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ScreeningVm> GetScreeningList()
        {
            return _dao. GetScreeningList();
        }

        public bool IsScreeningExist(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ScreeningDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
