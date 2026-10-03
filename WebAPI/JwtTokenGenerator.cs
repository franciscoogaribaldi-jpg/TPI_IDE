using Domain.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(int idUsuario, string nombreUsuario, int rol)
        {
            var seccionJwt = _configuration.GetSection("Jwt"); // va al archivo json y saca la seccion Jwt
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(seccionJwt["Key"]!)); // envuelve esos bytes en un objeto que el resto del código de seguridad entiende como "una clave"
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Junta dos cosas: la clave de arriba, y qué algoritmo matemático usar con ella para firmar

            string nombreRol = ((RolUsuario)rol).ToString(); // "Administrador" o "Cliente". lo que hce es el rol de administrador que viene como numero a string

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
                new Claim(ClaimTypes.Name, nombreUsuario),
                new Claim(ClaimTypes.Role, nombreRol)
            };

            int minutos = int.Parse(seccionJwt["ExpiracionMinutos"] ?? "120");

            var token = new JwtSecurityToken(
                issuer: seccionJwt["Issuer"],
                audience: seccionJwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutos),
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}