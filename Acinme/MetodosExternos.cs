using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Acinme
{
    class NativeMethods
    {
        public static void ProcesoShutdown(string argumentos)
        {
            var proceso = new ProcessStartInfo("shutdown");

            proceso.Arguments = argumentos;
            proceso.UseShellExecute = false;
            proceso.CreateNoWindow = true;

            Process.Start(proceso);
        }

        //Copiado de http://p2p.wrox.com/c/929-use-vbulletin-nets-shell-function-c.html
        //Otras fuentes: https://msdn.microsoft.com/es-es/library/windows/desktop/bb762153%28v=vs.85%29.aspx

        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]

        public static extern void LockWorkStation();

    }
}
