using System;
using System.Diagnostics;
using System.Threading;

namespace ProgramadorCS
{
    class CPU // PTE: Implementar IDisposable para Counter
    {
        public PerformanceCounter Contador = new PerformanceCounter();

        public dynamic CargaPorcentual(int msIntervaloSleep = 1000, bool formateado = true, int decimales = 2)
        {
            InicioCounter();

            dynamic inicio = Contador.NextValue();

            Thread.Sleep(msIntervaloSleep);

            dynamic final = Contador.NextValue();

            return !formateado ? final : Math.Round(final, decimales) + " %";
        }

        public void InicioCounter()
        {
            Contador.CategoryName = "Processor";
            Contador.CounterName = "% Processor Time";
            Contador.InstanceName = "_Total";
        }
    }
}
