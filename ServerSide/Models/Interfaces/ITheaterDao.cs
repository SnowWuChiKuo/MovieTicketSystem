using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
    public interface ITheaterDao
    {
        IEnumerable<TheaterVm>GetTheatersWithSeatsData();
        TheaterVm GetTheaterVmById(int id);

        void EditTheater(TheaterDto dto);
        TheaterDto FindTheaterById(int id);

        Theater ConvertToEfEntity(TheaterDto dto);
        TheaterDto ConvertToDto(Theater entity);
    }
}
