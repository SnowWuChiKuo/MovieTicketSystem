using ServerSide.Models.DTOs;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
    public interface ITheaterService
    {
        IEnumerable<TheaterVm> GetTheatersWithSeatsData();
        TheaterVm GetTheaterVmById(int id);
        TheaterDto FindTheaterById(int id);

        void EditTheater(TheaterDto dto);
    }
}
