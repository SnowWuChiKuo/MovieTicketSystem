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
            if (value is TimeOnly startTime)
			{
				var runTime = screeningService.GetMovieRunTime(vm.MovieId);
				var endTime = startTime.AddMinutes(runTime??0);

				var hasConflict = screeningService.HasTimeConflict(vm.TheaterId,vm.Televising,startTime,endTime,vm.Id);

				if (hasConflict)
				{
                    return new ValidationResult("�����ɶ��P�J�������Ĭ�!");
                }
			}

			return ValidationResult.Success;
		}
	}

}