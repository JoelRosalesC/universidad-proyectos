using System.Security.Cryptography;
using AlquilerDeVehiculosApi.App.Application.Entities.Models;
using AlquilerDeVehiculosApi.App.Application.Enums;
using AlquilerDeVehiculosApi.App.Application.IRepositories;
using Microsoft.Extensions.Caching.Memory;


namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class VerifyCodeRepository : IVerifyCodeRepository
    {
        private readonly MemoryCache _cache;
        private const int MaxFailedAttempts = 300;
        private const int ExpirationMinutes = 100;
        private const int CodeDigits = 6;

        public VerifyCodeRepository()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public string Generate(string email, VerifyCodeType type)
        {
            string code = "123456"; //GenerateSimpleCode();
            var cacheEntryOptions = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(ExpirationMinutes) };

            VerificationCode entry = new VerificationCode()
            {
                Code = code,
                FailedAttempts = 0,
                Type = type
            };

            _cache.Set(email + entry.Type, entry, cacheEntryOptions);
            return code;
        }

        public bool Verify(string email, string code, VerifyCodeType type)
        {
            if (!_cache.TryGetValue(email  + type, out VerificationCode entry))
                return false;
            if (entry.Code == code)
            {
                _cache.Remove(email + type);
                return true;
            }
            entry.FailedAttempts++;
            if (entry.FailedAttempts >= MaxFailedAttempts)
            {
                _cache.Remove(email + type);
                return false;
            }
            _cache.Set(email + type, entry);
            return false;
        }

        private string GenerateSimpleCode()
        {
            var bytes = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            int value = BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF;
            return (value % (int)Math.Pow(10, CodeDigits)).ToString($"D{CodeDigits}");
        }
        
    }
}