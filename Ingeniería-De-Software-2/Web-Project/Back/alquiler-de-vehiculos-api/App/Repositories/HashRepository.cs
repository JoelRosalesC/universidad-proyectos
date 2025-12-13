
using System.Security.Cryptography;
using AlquilerDeVehiculosApi.App.Application.IRepositories;


namespace AlquilerDeVehiculosApi.App.Repositories
{
    public class HashRepository : IHashRepository
    {
        
        public (string hash, string salt) HashPassword(string password)
        {
            // Genera un salt aleatorio de 16 bytes
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            // Hashear la contraseña usando PBKDF2 con SHA-256 y 100,000 iteraciones
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32); // Generar un hash de 32 bytes

                // Combina el salt y el hash en una sola cadena
                string salt = Convert.ToBase64String(saltBytes);
                string hash = Convert.ToBase64String(hashBytes);

                return (hash, salt);
    }
        }

        public bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                string computedHash = Convert.ToBase64String(hashBytes);

                return computedHash == storedHash;
            }
        }
        
    }
}