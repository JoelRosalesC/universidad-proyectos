

using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class GenerateLoginCodeUseCase
    {
        private IVerifyCodeRepository _verifyCodeRepository;
        private IEmailQueueRepository _emailQueueRepository;
        private ConvertInEmailTemplateUseCase _convertInEmailTemplateUseCase;

        public GenerateLoginCodeUseCase(IVerifyCodeRepository verifyCodeRepository, IEmailQueueRepository emailQueueRepository, ConvertInEmailTemplateUseCase convertInEmailTemplateUseCase)
        {
            _verifyCodeRepository = verifyCodeRepository;
            _emailQueueRepository = emailQueueRepository;
            _convertInEmailTemplateUseCase = convertInEmailTemplateUseCase;
        }

        public async Task runAsync(string email)
        {
            string code = _verifyCodeRepository.Generate(email, VerifyCodeType.Login);
            System.Console.WriteLine($"| EL CODIGO DE VERIFICACION ES: '{code}' GENERADO A LAS {DateTime.Now.ToString("HH:mm:ss")}, EXPIRARA EN 10 MINUTOS |");

            EmailTemplate emailTemplate = await _convertInEmailTemplateUseCase.runAsync("2FA", code);
            await _emailQueueRepository.EnqueueAsync(email, "Solicitud de código de validación", emailTemplate.Html);
        }
    }
}