using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using ProgramadorCS;

namespace Redes
{
    #region " Enumeraciones "

    public enum Direccion
    {
        Subida,
        Bajada,
        Ambas
    }

    /* ELIMINAR: ENUMERACIÓN PASADA AL NAMESPACE PROGRAMADOR EN ENUMERACIONES.CS
    public enum Magnitud
    {
        Bytes,
        KB,
        MB,
        GB
    }*/
    #endregion

    class Red
    {
        #region " Contructor "
        //Cambios: Se ha reducido el nº de constructores a 1, manteniendo las sobrecargas con parámetros opcionales
        //De esta forma aunque se instancia una List aunque no sea necesario, también se previenen los errores
        //al utilizar SiguienteValorM() por no indicar la capacidad
        public Red(int indice = 0, Direccion direccion = Direccion.Bajada,  Magnitud ud = Magnitud.Bytes, int capacidad = 1)
        {
            //NOTA: Modificaciones 19/06 Modificación en param contructor. PASAR A PRUEBAS RED 3 
            //      Cambiado nombre parámetros, definir y explicar en comentarios XML
            this.direccion = direccion; //Reañadido: Tal y como está enfocado el funcionamiento del form, es más cómodo. Suprimido el parámetro del contructor. El valor por defecto se coloca en la z. privada
            this.ud = ud;
            this.indice = indice;
            valores = new List<long>(capacidad);
            tiempo = DateTime.Now;
            valorIni = Consulta();
        }

        #endregion

        #region " Propiedades "

        public int CapacidadListValores
        {
            get
            {
                return valores.Capacity;
            }
            set
            {
                valores.Capacity = value;
            }
        }

        public int IndiceAdaptador
        {
            get
            {
                return indice;
            }
            set
            {
                indice = value;
            }
        }

        public Direccion TipoDireccion
        {
            get
            {
                return direccion;
            }
            set
            {
                direccion = value;
            }
        }

        public Magnitud UdRetorno
        {
            get
            {
                return ud;
            }
            set
            {
                ud = value;
            }
        }

        #endregion

        #region " Privado "

        private long valorIni;
        private DateTime tiempo;
        private Direccion direccion;
        private Magnitud ud;
        private int indice;
        private List<long> valores;

        private double formatValorAMagnitud(double valor)
        {
            switch (ud)
            {
                case Magnitud.KB:
                    valor /= 0x400;
                    break;

                case Magnitud.MB:
                    valor /= 0x100000;
                    break;

                case Magnitud.GB:
                    valor /= 0x40000000;
                    break;
            }

            return valor;
        }

        #endregion

        public long Consulta()
        {
            IPInterfaceStatistics infoAdaptador = NetworkInterface.GetAllNetworkInterfaces()[indice].GetIPStatistics();
            long valor = 0;

            switch (direccion)
            {
                case Direccion.Subida:
                    valor = infoAdaptador.BytesSent;
                    break;

                case Direccion.Bajada:
                    valor = infoAdaptador.BytesReceived;
                    break;

                case Direccion.Ambas:
                    valor = infoAdaptador.BytesSent + infoAdaptador.BytesReceived;
                    break;
            }

            return valor;
        }

        public double ConsultaFormat()
        {
            IPInterfaceStatistics infoAdaptador = NetworkInterface.GetAllNetworkInterfaces()[indice].GetIPStatistics();
            double valor = 0;

            switch (direccion)
            {
                case Direccion.Subida:
                    valor = infoAdaptador.BytesSent;
                    break;

                case Direccion.Bajada:
                    valor = infoAdaptador.BytesReceived;
                    break;

                case Direccion.Ambas:
                    valor = infoAdaptador.BytesSent + infoAdaptador.BytesSent;
                    break;
            }

            return formatValorAMagnitud(valor);
        }

        public long SiguienteValorBytes()
        {
            long consulta = Consulta();

            if (valorIni > consulta)
            {
                //NOTA: Parche error por overflow. En W8.1, con más de 20GB recibidos y 3GB enviados, no se pudo reproducir el error.
                //PTE: Comprobación en versiones de Windows < 8.1
                //PRUEBAS: 
                //System.Windows.Forms.MessageBox.Show("Overflow superado!!!", "SiguienteValorBytes()");

                valorIni = 0;
            }

            long valor = consulta - valorIni;
            double tSeg = (DateTime.Now - tiempo).TotalSeconds;

            tiempo = DateTime.Now;
            valorIni = consulta;

            long res = (long)(valor / tSeg);

            if (valores.Capacity <= 1)
            {
                return res;
            }

            valores.Add(res);

            return (long)valores.Average();
        }

        public double SiguienteValor()
        {
            double valor = SiguienteValorBytes();

            return formatValorAMagnitud(valor);
        }
    }
}
