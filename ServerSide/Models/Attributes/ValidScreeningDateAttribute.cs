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

            if (value is DateTime screeningDate)
            {
                var releaseDate = screeningService.GetMovieReleaseDate(vm.MovieId);

                if (screeningDate.Date < releaseDate)
                {
                    return new ValidationResult(
                        $"場次日期不能早於電影上檔日期 {releaseDate:yyyy/MM/dd}");
                }
            }

            return ValidationResult.Success;
        }
    }
}