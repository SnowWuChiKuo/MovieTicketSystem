using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
    public interface IScreeningDao
    {

        public IEnumerable<ScreeningVm> GetScreeningList();
        public ScreeningEditVm GetEditList(int id);
        public bool IsScreeningExist(int id);
        public void Create(ScreeningDto dto);
        public void Edit(ScreeningDto dto);
        public void Delete(int id);

        public ScreeningDto GetById(int id);
        ScreeningDto ConvertToDto(Screening screening);
        Screening ConvertToEfEntity(ScreeningDto dto);
    }
}
