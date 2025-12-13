using AlquilerDeVehiculosApi.App.Application.Enums;

namespace AlquilerDeVehiculosApi.App.Application.IRepositories
{
    public interface IVerifyCodeRepository
    {
        string Generate(string email, VerifyCodeType type);
        bool Verify(string email, string code, VerifyCodeType type);
    }
}