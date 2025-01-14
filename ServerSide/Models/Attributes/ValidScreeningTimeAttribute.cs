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
			
            //檢查場次時間是否衝突
            if (value is TimeOnly startTime)
			{
				var runTime = screeningService.GetMovieRunTime(vm.MovieId);
				var endTime = startTime.AddMinutes(runTime??0);

				var hasConflict = screeningService.HasTimeConflict(vm.TheaterId,vm.Televising,startTime,endTime,vm.Id);

				if (hasConflict)
				{
                    return new ValidationResult("場次時間與既有場次衝突!");
                }
			}

			return ValidationResult.Success;
		}
	}

}