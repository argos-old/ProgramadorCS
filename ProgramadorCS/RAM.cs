using Microsoft.VisualBasic.Devices;
using System;

namespace ProgramadorCS
{
    class RAM
    {
        #region " Enumeraciones "

        public enum FormatoRetorno
        {
            PorcentualFormateado,
            Porcentual,
            MBFormateados,
            MB,
            BytesFormateados,
            BytesTotal
        }

        public enum InfoRequerida
        {
            Disponible,
            Usada,
            Total
        }

        #endregion

        #region " Declaraciones "

        ComputerInfo vbCI = new ComputerInfo();

        #endregion

        #region " Métodos "

        public dynamic Consulta(InfoRequerida tipoConsulta, FormatoRetorno retorno)
        {
            //Método genérico. Los 3 tipos de consulta integradas
            ulong bytes;

            switch (tipoConsulta)
            {
                case InfoRequerida.Disponible:
                    bytes = vbCI.AvailablePhysicalMemory;
                    break;

                case InfoRequerida.Usada:
                    bytes = vbCI.TotalPhysicalMemory - vbCI.AvailablePhysicalMemory;
                    break;

                case InfoRequerida.Total:
                    bytes = vbCI.TotalPhysicalMemory;
                    break;

                default:
                    bytes = 0;
                    break;
            }

            switch (retorno)
            {
                case FormatoRetorno.PorcentualFormateado:
                    return Math.Round(((float)bytes / vbCI.TotalPhysicalMemory * 100), 2) + " %";

                case FormatoRetorno.Porcentual:
                    return Math.Round(((float)bytes / vbCI.TotalPhysicalMemory * 100), 2);

                case FormatoRetorno.MBFormateados:
                    return setMBFormat(bytes);

                case FormatoRetorno.MB:
                    return setMB(bytes);

                case FormatoRetorno.BytesFormateados:
                    return setBytes(bytes);

                case FormatoRetorno.BytesTotal:
                    return bytes;

                default:
                    return null;
            }
        }

        public dynamic Disponible(FormatoRetorno retorno)
        {
            ulong bytes = vbCI.AvailablePhysicalMemory;

            switch (retorno)
            {
                case FormatoRetorno.PorcentualFormateado:
                    return Math.Round(((double)bytes / vbCI.TotalPhysicalMemory * 100), 2) + " %";

                case FormatoRetorno.Porcentual:
                    return Math.Round(((float)bytes / vbCI.TotalPhysicalMemory * 100), 2);

                case FormatoRetorno.MBFormateados:
                    return setMBFormat(bytes);

                case FormatoRetorno.MB:
                    return setMB(bytes);

                case FormatoRetorno.BytesFormateados:
                    return setBytes(bytes);

                case FormatoRetorno.BytesTotal:
                    return bytes;

                default:
                    return null;
            }
        }

        public dynamic Usada(FormatoRetorno retorno)
        {
            ulong bytes = vbCI.TotalPhysicalMemory - vbCI.AvailablePhysicalMemory;

            switch (retorno)
            {
                case FormatoRetorno.PorcentualFormateado:
                    return Math.Round(((double)bytes / vbCI.TotalPhysicalMemory * 100), 2) + " %";

                case FormatoRetorno.Porcentual:
                    return Math.Round(((float)bytes / vbCI.TotalPhysicalMemory * 100), 2);

                case FormatoRetorno.MBFormateados:
                    return setMBFormat(bytes);

                case FormatoRetorno.BytesFormateados:
                    return setBytes(bytes);

                case FormatoRetorno.MB:
                    return setMB(bytes);

                case FormatoRetorno.BytesTotal:
                    return bytes;

                default:
                    return null;
            }
        }

        public dynamic Total(FormatoRetorno retorno)
        {
            ulong bytes = vbCI.TotalPhysicalMemory;

            switch (retorno)
            {
                case FormatoRetorno.MBFormateados:
                    return setMBFormat(bytes);

                case FormatoRetorno.MB:
                    return setMB(bytes);

                case FormatoRetorno.BytesFormateados:
                    return setBytes(bytes);

                case FormatoRetorno.BytesTotal:
                    return bytes;

                case FormatoRetorno.Porcentual:
                    return 100;

                default:
                    return "100 %";
            }
        }

        #endregion

        #region " Funciones de apoyo "

        public static string setBytes(ulong bytes)
        {
            string resultado;

            if (bytes >= 1099511627776) resultado = Math.Round(bytes / (float)(Math.Pow(1024, 4)), 2) + " TB";
            else if (bytes >= 1073741824) resultado = Math.Round(bytes / (float)(Math.Pow(1024, 3)), 2) + " GB";
            else if (bytes >= 1048576) resultado = Math.Round(bytes / (float)(Math.Pow(1024, 2)), 2) + " MB";
            else if (bytes >= 1024) resultado = Math.Round((bytes / 1024f), 2) + " KB";
            else resultado = bytes + " bytes";

            return resultado;
        }

        string setMBFormat(ulong bytes)
        {
            return Math.Round((float)bytes / (float)(Math.Pow(1024, 2)), 2) + " MB";
        }

        float setMB(ulong bytes)
        {
            return (float)Math.Round((float)bytes / (float)(Math.Pow(1024, 2)), 2);
        }

        #endregion
    }
}
