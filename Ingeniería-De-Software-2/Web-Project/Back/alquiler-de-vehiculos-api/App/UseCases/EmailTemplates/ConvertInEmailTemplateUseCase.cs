
using AlquilerDeVehiculosApi.App.Application.Entities;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using AlquilerDeVehiculosApi.App.Application.Exceptions;
using AlquilerDeVehiculosApi.App.Application.ApiEntities;

namespace AlquilerDeVehiculosApi.App.UseCases
{
    public class ConvertInEmailTemplateUseCase
    {

        private IEmailTemplateRepository _emailTemplateRepository;
        public ConvertInEmailTemplateUseCase(IEmailTemplateRepository emailTemplateRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
        }

        public async Task<EmailTemplate> runAsync(string emailTemplateName, params string[] values)
        {
            EmailTemplate emailTemplate = await _emailTemplateRepository.GetByNameAsync(emailTemplateName);
            string[] injectedValues = emailTemplate.InjectedValues.Split(",");
            if(injectedValues.Length != values.Length)
                throw new ApiException(new ApiErrorResponse("Invalid number of parameters."));
            for(int i = 0; i < injectedValues.Length; i++)
                emailTemplate.Html = emailTemplate.Html.Replace("{"+injectedValues[i]+"}", values[i]);
            return emailTemplate;
        }
    }
}