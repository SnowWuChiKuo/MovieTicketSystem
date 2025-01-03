using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;

namespace ServerSide.Models.Interfaces
{
    public interface IMovieDao
    {
        void Create(MovieDto dto);
        Movie ConvertToEfEntity(MovieDto dto);

        void Edit(MovieDto dto);
    }
}
