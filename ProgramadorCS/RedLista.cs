using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Redes
{
    /// <summary>
    /// Resumen: Esta clase lista las interfaces de la red local en forma de Dictionary asociando la descripción de cada una  
    /// al índice que ocupan en la matriz Networks[], filtrando, si se desea, todas las que no están conectadas, las de bucle    
    /// invertido y las pseudo-interfaces.
    /// </summary>
    class ListRed
    {
        /// <summary>
        /// Constructor. Se puede indicar se se filtran las interfaces de red poco frecuentes.
        /// </summary>
        /// <param name="filtrado"></param>
        public ListRed(bool filtrado = false)
        {
            filtrar = filtrado;
        }

        /// <summary>
        /// Devuelve un Dictionary con las descripciones de las interfaces de red del equipo, asociando cada una de éstas 
        /// al índice que ocupan en la matriz Networks[]. Puede usarse como alternativa al método CreaDic().
        /// </summary>
        public Dictionary<byte, string> DicAdaptadores
        {
            get
            {
                return CreaDic();
            }
        }

        /// <summary>
        /// Establece el filtrado de todas las redes que no están conectadas, las de bucle invertido (Loopback) y las     
        /// pseudo-interfaces (como Teredo). También devuelve el estado actual del filtrado.
        /// </summary>
        public bool Filtrar
        {
            get
            {
                return filtrar;
            }
            set
            {
                filtrar = value;
            }
        }

        bool filtrar;

        /// <summary>
        /// Método que genera un Dictionary con las descripciones de las interfaces de red del equipo, asociando cada una de
        /// éstas al índice que ocupan en la matriz Networks[]. Puede usarse como alternativa a la propiedad DicAdaptadores
        /// </summary>
        /// <returns></returns>
        public Dictionary<byte, string> CreaDic()
        {
            NetworkInterface[] adaptadores = NetworkInterface.GetAllNetworkInterfaces();
            Dictionary<byte, string> dic = new Dictionary<byte, string>();

            foreach (NetworkInterface adaptador in adaptadores)
            {
                if (filtrar)
                {
                    if (adaptador.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                        adaptador.OperationalStatus != OperationalStatus.Up ||
                        new Regex("Pseudo-Interface", RegexOptions.IgnoreCase).IsMatch(adaptador.Description))
                    {

                        continue;
                    }
                }

                dic.Add((byte)Array.IndexOf(adaptadores, adaptador), adaptador.Description);
            }

            return dic;
        }

        /// <summary>
        /// Función que devuelve la clave asociada al item seleccionado del combo indicado. A efectos prácticos devuelve
        /// el índice de la interfaz de red listada en el combo.
        /// </summary>
        /// <param name="combo">ComboBox en el que se encuentra el item seleccionado</param>
        /// <returns></returns>
        public static int GetIndiceRed(ComboBox combo)
        {
            //PTE:  Buscar solución para que una misma función pueda servir para todos los controles que derivan 
            //      de ListControl (ComboBox, ListBox, DropDownList, RadioButtonList, CheckedListBox, etc)
            //NOTA: Todo este sistema está desarrollado y mejor explicado en PruebasRed?.cs
            return ((KeyValuePair<byte, string>)combo.SelectedItem).Key;
        }
    }
}
