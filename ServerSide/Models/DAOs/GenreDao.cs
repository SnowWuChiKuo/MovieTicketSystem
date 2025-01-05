using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
	public class GenreDao : IGenreDao
	{
		private readonly AppDbContext _db;
		public GenreDao(AppDbContext db)
		{
			_db = db;
		}


		public void Create(GenreDto dto)
		{
			var genre = ConvertToEFEntity(dto);
			_db.Genres.Add(genre);
			_db.SaveChanges();
		}

		public void Delete(int id)
		{
			var genreInDb = _db.Genres.Find(id);
			if(genreInDb == null)
			{
				throw new Exception("該筆資料不存在或已被刪除!");
			}
			_db.Genres.Remove(genreInDb);
			_db.SaveChanges();
		}

		public void Edit(GenreDto dto)
		{
			var genreInDb = _db.Genres.Find(dto.Id);

			if (genreInDb == null)
			{
				throw new Exception("該筆資料不存在或已被刪除!");
			}

			genreInDb.Name = dto.Name;
			genreInDb.DisplayOrder = dto.DisplayOrder;

			_db.SaveChanges();
		}

		public List<GenreVm> GetAll()
		{
			var indexData = _db.Genres
				.AsNoTracking()
				.Select(i => new GenreVm
				{
					Id = i.Id,
					Name = i.Name,
					DisplayOrder = i.DisplayOrder
				})
				.ToList();
			return indexData;
		}

		public Genre ConvertToEFEntity(GenreDto dto)
		{
			return new Genre
			{
				Id = dto.Id,
				Name = dto.Name,
				DisplayOrder = dto.DisplayOrder
			};
		}

		public GenreDto FindGenreById(int id)
		{
			var genreInDb = _db.Genres.Find(id);
			if (genreInDb == null)
			{
				throw new Exception("該筆資料不存在或已被刪除!");
			}
			var genreDto = ConvertToDto(genreInDb);
			return genreDto;
		}

		public GenreDto ConvertToDto(Genre genre)
		{
			return new GenreDto
			{
				Id = genre.Id,
				Name = genre.Name,
				DisplayOrder = genre.DisplayOrder
			};
		}
	}
}
