using OpenHardwareMonitor.Hardware;
using System.Security.Principal;

// (*) Sensores admitidos: carga, temp y potencia. Relojes no válidos para la media aritmética porque se incluye la velocidad del Bus

namespace ProgramadorCS
{
    class Temp
    {
        public SensorType tipoSensor { get; set; } // (*)

        public static bool sinPermisos
        {
            get
            {
                return !new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public Computer equipo = new Computer();

        public void Iniciar()
        {
            equipo.CPUEnabled = true; // (*)
            equipo.Open();
        }

        public float? ValorMedioSensor(SensorType tipoSensor = SensorType.Temperature) // (*)
        {
            //if (sinPermisos) return "n/a"; // ANULADO: Se supone que la propiedad debe consultarse antes evitando la ejecución de
                                             // esta función normalmente temporizada. Además precisa dynamic.

            foreach (IHardware hardware in equipo.Hardware)
            {
                hardware.Update();
                byte num = 0;
                float? resultado = 0;

                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (sensor.SensorType == tipoSensor && sensor.Value != null)
                    {
                        resultado += sensor.Value;
                        num++;
                    }
                }

                return num != 0 ? resultado / num : null;
            }

            return null;
        }
    }
}
