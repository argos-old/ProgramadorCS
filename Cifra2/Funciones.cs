using System;
using System.IO;
using System.Text;
using VTDev.Libraries.CEXEngine.Crypto;
using VTDev.Libraries.CEXEngine.Crypto.Cipher.Symmetric.Block;
using VTDev.Libraries.CEXEngine.Crypto.Digest;
using VTDev.Libraries.CEXEngine.Crypto.Mode;

namespace Cifra2
{
    public class Funciones
    {
        /// <summary>
        /// Función encargada del establecimiento de la clave. Nota: Como base para la clave se ha tomado el nombre de 
        /// usuario junto con la ruta donde se aloja el ejecutable. Aunque este texto sea tratado con el algoritmo Skein512,
        /// cualquiera con acceso al código lo descifraría sin complicaciones. Su uso es privado porque está incluida en
        /// las funciones estáticas de (des)cifrado de texto.
        /// </summary>
        /// <param name="bytesClave">El tomaño en bytes de la clave</param>
        /// <returns>Una matriz de bytes dimensionada con el tamaño requerido</returns>
        private static byte[] setClave(byte bytesClave)
        {
            var hash = new Skein512();
            string entorno = Environment.UserName + Directory.GetCurrentDirectory();
            byte[] clave = hash.ComputeHash(Encoding.UTF32.GetBytes(entorno));

            Array.Resize<byte>(ref clave, bytesClave);

            return clave;
        }

        /// <summary>
        /// Función encargada de cifrar el texto proporcionado por el usuario mediante 56 rondas del algoritmo Serpent 
        /// </summary>
        /// <param name="txtPlano">Cadena con el texto en formato plano a cifrar</param>
        /// <returns>Cadena en Base64 con el texto cifrado</returns>
        public static string CifraTxt(string txtPlano)
        {
            using (var cifradoCEX = new CTR(new SPX(56)))
            {
                byte[] txtBytes = Encoding.UTF32.GetBytes(txtPlano);
                byte[] txtBytesCifrado = new byte[txtBytes.Length];

                cifradoCEX.Initialize(true, new KeyParams(setClave(64), setClave(16)));
                cifradoCEX.Transform(txtBytes, txtBytesCifrado);

                return Convert.ToBase64String(txtBytesCifrado);
            }
        }

        /// <summary>
        /// Función encargada de descifrar el texto proporcionado por el usuario mediante 56 rondas del algoritmo Serpent 
        /// </summary>
        /// <param name="txtCifrado">Cadena en Base64 con el texto cifrado</param>
        /// <returns>Cadena de texto plano con el texto descifrado</returns>
        public static string DescifraTxt(string txtCifrado)
        {
            using (var cifradoCEX = new CTR(new SPX(56)))
            {
                byte[] txtBytesCifrado = Convert.FromBase64String(txtCifrado);
                byte[] txtBytes = new byte[txtBytesCifrado.Length];

                cifradoCEX.Initialize(true, new KeyParams(setClave(64), setClave(16)));
                cifradoCEX.Transform(txtBytesCifrado, txtBytes);

                return Encoding.UTF32.GetString(txtBytes);
            }
        }
    }
}
