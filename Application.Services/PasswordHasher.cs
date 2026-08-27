using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    /// <summary>
    /// Hashing simple de contraseñas (SHA-256, sin salt) para no guardarlas en texto
    /// plano. Es intencionalmente básico: alcanza para el TP, pero para un sistema
    /// real conviene usar BCrypt/Argon2/PBKDF2 con salt por usuario. Lo dejamos anotado
    /// acá para que quede claro que es una simplificación consciente, no un olvido.
    /// </summary>
    internal static class PasswordHasher
    {
        public static string Hash(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public static bool Verify(string password, string hash)
        {
            return Hash(password) == hash;
        }
    }
}
