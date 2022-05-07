using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace LITTLERP
{

    class useful
    {
        private const String CRITERIOS_CARACTERES = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const String CRITERIOS_NUMERICOS = "0123456789";
        private const String CRITERIOS_ESPECIALES = "!\"#$%&'()*+,-./:;=?@[]^_`{|}~";

        /// <summary>
        /// Gets the sha256 encrypting.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        // (No se utiliza de momento, por comodidad a la hora de probar la aplicacion) 
        // Sirve para comprobar que la contraseña cumple los requisitos de seguridad minimos        
        /// <summary>
        /// Check the password force
        /// </summary>
        /// <param name="contraseñaSinVerificar">The contraseña sin verificar.</param>
        /// <returns></returns>
        public Boolean passwordForce(String contraseñaSinVerificar)
        {
            Boolean cumpleCriterios = false;

            if (contraseñaSinVerificar.Contains(CRITERIOS_CARACTERES))
            {
                if (contraseñaSinVerificar.Contains(CRITERIOS_NUMERICOS))
                {
                    if (contraseñaSinVerificar.Contains(CRITERIOS_ESPECIALES))
                        cumpleCriterios = true;
                }
            }
            return (cumpleCriterios);
        }

        /// <summary>
        /// Fechas en formato cientifico.
        /// </summary>
        /// <returns></returns>
        public static int fechaFormatoCientifico()
        {
            string Date = DateTime.Now.ToString("dd/MM/yyyy");
            String[] fechaFormateada = Date.Split('/');

            String anio = fechaFormateada[2];
            String mes = fechaFormateada[1];
            String dia = fechaFormateada[0];

            if (dia.Length == 1)
                dia = "0" + dia;

            String fechaFormada = anio + mes + dia;

            return Convert.ToInt32(fechaFormada);
        }
    }
} 