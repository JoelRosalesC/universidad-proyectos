using AlquilerDeVehiculosApi.App.Application.Entities.Models;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class GetWeeklyIncomeUseCase
    {

        private IRentalRepository _rentalRepository;

        public GetWeeklyIncomeUseCase(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<WeeklyIncome> runAsync()
        {
            var weeklyIncome = new WeeklyIncome();
            var rentals = await _rentalRepository.GetAllAsync();
            weeklyIncome.TotalIncome = (decimal)rentals.Sum(r => r.TotalPrice);
            
            weeklyIncome.Weeks = rentals
                .GroupBy(r => GetStartOfWeek(r.RentalDate))
                .Select(g => new WeekIncome()
                {
                    Week = g.Key,
                    NumberOfReservations = g.Count()
                })
                .OrderBy(r => r.Week)
                .ToList();

            return weeklyIncome;
        }

        private static DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }
    }
}