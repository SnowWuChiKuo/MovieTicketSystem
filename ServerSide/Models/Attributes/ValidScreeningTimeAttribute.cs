using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketSystem.Models.Attributes
{
	public class ValidScreeningTimeAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var vm = (ScreeningEditVm)validationContext.ObjectInstance;
			var screeningService = (IScreeningService)validationContext.GetService(typeof(IScreeningService));

			//�ˬd�����ɶ��O�_�Ĭ�
			if (value is TimeOnly televisingDate)
			{
				var releaseDate = screeningService.GetMovieReleaseDate(vm.MovieId);

				//if ()
				//{
					
				//}
			}

			return ValidationResult.Success;
		}
	}

}