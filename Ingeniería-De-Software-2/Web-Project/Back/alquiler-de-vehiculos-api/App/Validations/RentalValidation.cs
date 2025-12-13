using System.Threading.Tasks;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.Application.Validations
{
    public class RentalValidation
    {
        private IRentalRepository _rentalRepository;
        public RentalValidation(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }
        public void ValidateDate(DateTime startDate, DateTime endDate)
        {
            if (new DateTime(endDate.Year, endDate.Month, endDate.Day) < new DateTime(startDate.Year, startDate.Month, startDate.Day))
                throw new ApiException(new ApiErrorResponse("la fecha de fin no puede ser menor a la de inicio"));

            if (new DateTime(startDate.Year, startDate.Month, startDate.Day) < DateTime.Now)
                throw new ApiException(new ApiErrorResponse("solo es posible reservar a partir de mañana"));
        }

        public void ValidateCard(string cardNumber)
        {
            var c = cardNumber.Replace(" ", "").Replace("-", "");
            var errores = new Dictionary<string, string[]>
            {
                { "emptyCard", new[] { "la tarjeta no tiene fondos suficientes" } }
            };
            if (c == "1111222233334444")
                throw new ApiException(new ApiErrorResponse(errores));
            if (c == "1111222211112222")
                throw new ApiException(new ApiErrorResponse("la tarjeta no esta registrada"));
        }
        public void ValidateExpirationDate(string expirationDate)
        {
            if (expirationDate == "06/30")
                throw new ApiException(new ApiErrorResponse("la fecha de vencimiento es incorrecta"));
        }
        public void ValidateCvv(string cvv)
        {
            if (cvv == "111")
                throw new ApiException(new ApiErrorResponse("el CVV es invalido"));
        }

        public async Task ValidateRentalStatus(int rentalId, RentalStatus rentalStatus)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental.Status != rentalStatus)
                throw new ApiException(new ApiErrorResponse("no se puede realizar esta accion"));
        }


        public async Task CancelRentalValidateAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);

            if (DateTime.Today > rental.RentalDate.Date.AddDays(-1))
                throw new ApiException(new ApiErrorResponse("para cancelar una reserva debe ser un dia antes de la fecha de retiro del vehiculo"));

            if (rental.Status != Enums.RentalStatus.Pending)
                throw new ApiException(new ApiErrorResponse("no es posible cancelar esta reserva"));
        }
        public async Task InvalidateRentalValidateAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);

            if (rental.Status != Enums.RentalStatus.Pending)
                throw new ApiException(new ApiErrorResponse("no es posible cancelar esta reserva"));
        }
    }
}