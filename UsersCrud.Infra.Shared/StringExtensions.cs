using System.Security.Cryptography;
using System.Text;

namespace UsersCrud.Infra.Shared
{
    public static class StringExtensions
    {
        /// <summary>
        /// Método para codificação de uma string para hash.
        /// </summary>
        /// <param name="value">Objeto referenciado.</param>
        /// <returns>Valor hasheado.</returns>
        public static byte[] Encode(this string value)
        {
            var algorithm = SHA512.Create();
            var encodedValue = Encoding.UTF8.GetBytes(value);
            var encryptedPassword = algorithm.ComputeHash(encodedValue);

            var sb = new StringBuilder();
            foreach (var caracter in encryptedPassword)
            {
                sb.Append(caracter.ToString("X2"));
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
