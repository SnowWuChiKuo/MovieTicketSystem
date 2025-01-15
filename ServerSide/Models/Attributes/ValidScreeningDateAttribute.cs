using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketSystem.Models.Attributes
{
    public class ValidScreeningDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var vm = (ScreeningEditVm)validationContext.ObjectInstance;
            var screeningService = (IScreeningService)validationContext.GetService(typeof(IScreeningService));
            
            if (value is DateOnly televisingDate)
            {
                var releaseDate = screeningService.GetMovieReleaseDate(vm.MovieId);
                var today = DateOnly.FromDateTime(DateTime.Now);

                if (televisingDate < today)
                {
                    return new ValidationResult(
                       $"場次日期不能為今天日期 {releaseDate:yyyy/MM/dd} 之前 ");
                }

                var releaseDateOnly = DateOnly.FromDateTime(releaseDate);
                if (televisingDate < releaseDateOnly)
                {
                    return new ValidationResult(
                        $"場次日期不能早於電影上檔日期 {releaseDate:yyyy/MM/dd}");
                }

                var maxDate = today.AddMonths(3); // 場次日期最多就只能排3個月後
                if (televisingDate > maxDate)
                {
                    return new ValidationResult("場次日期不能超過3個月");
                }

            }

            return ValidationResult.Success;
        }
    }

}