using ServerSide.Models.DTOs;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
    public interface IScreeningService
    {
        
        
        public IEnumerable<ScreeningVm> GetScreeningList();
		//public ScreeningDto GetScreeningDetail(int screeningId);
		public ScreeningEditVm GetEditList(int id);
		public bool IsScreeningExist(int id);
        public void Create(ScreeningDto dto);
        public void Edit(ScreeningDto dto);
        public void Delete(int id);
    }
}
