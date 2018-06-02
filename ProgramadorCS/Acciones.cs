using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProgramadorCS
{
    class Acciones : NativeMethods
    {
        //PTE ORDENAR

        public string ParametrosExtra
        {
            get
            {
                return parametrosExtra;
            }
            set
            {
                parametrosExtra = value;
            }
        }

        /* SIN HACER: EN PREVISIÓN DE IMPLEMENTAR TEMPORIZACIÓN DE LA SHELL DE WINDOWS EN APAGADOS Y REINICIOS
        public uint ArgumentoTiempo
        {
            get;
            set;
        }

        private string parametroTiempo //IDEM ANTERIOR
        //SE UTILIZA PROPIEDAD PRIVADA PARA PODER CONCATENAR LA PROPIEDAD ANTERIOR. LAS VARIABLES NO PERMITEN CITAR PROPIEDADES.
        {
            get
            {
                return " -t" + ArgumentoTiempo;
            }
        }
        */

        private string parametrosExtra = string.Empty; // En reinicios y apagados: -f, -c, o en caso de Temp.Shell, -t xxx
        
        public void Shutdown(tipoAccion Accion)
        {
            string parametros = null;
            
            switch (Accion)
            {
                case tipoAccion.Apagar:
                    parametros = "-s -t 0";
                    break;
                
                case tipoAccion.Reiniciar:
                    parametros = "-r -t 0";
                    break;

                case tipoAccion.CerrarSesion:
                    parametros = "-l";
                    break;

                case tipoAccion.Hibernar:
                    parametros = "-h";
                    break;

                case tipoAccion.ApagarInmediato:
                    parametros = "-p";
                    break;

                case tipoAccion.ReiniciarAppsRegistradas:
                    parametros = "-g -t 0";
                    break;

                case tipoAccion.ReiniciarMenuOpciones:
                    parametros = "-r -o -t 0";
                    break;

                case tipoAccion.Anular:
                    parametros = "-a";
                    parametrosExtra = string.Empty;
                    break;
            }
            /*PRUEBAS:
            if (parametros != "-p")
            {
                MessageBox.Show("Bipbipbiiip\nshutdown " + parametros + parametrosExtra, "Comando ejecutable Procesos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            } //*/
            ProcesoShutdown(parametros + parametrosExtra); // (*) CANDIDATO FINAL
        }

        public void Suspender()
        {
            //Proceso.EjecutarExterno(this.Handle, "rundll32.exe", "powrprof.dll,SetSuspendState 0,1,1"); //Sólo hiberna. Lee Notas 
            //Proceso.Suspender(true);
            Application.SetSuspendState(PowerState.Suspend, true, true); // (*) CANDIDATO FINAL
        }

        public void Hibernar()
        {
            //PRUEBAS:
            //MessageBox.Show("Bipbipbiiip\n Hibernando", "Hibernado en clAcciones desde Proceso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            //Proceso.EjecutarExterno(this.Handle, "rundll32.exe", "powrprof.dll,SetSuspendState 1,1,1");
            //Proceso.Hibernar(true);
            //Proceso.Shutdown("-h");
            Application.SetSuspendState(PowerState.Hibernate, true, true); // (*) CANDIDATO FINAL
            //Shutdown(tipoAccion.Hibernar);
        }

        public void Bloquear()
        {
            //PRUEBAS:
            //MessageBox.Show("Bipbipbiiip\n Bloqueando", "Bloqueado en clAcciones desde Proceso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            //Proceso.EjecutarExterno(this.Handle, "rundll32.exe", "user32.dll LockWorkStation");
            Bloquea2(); // (*) CANDIDATO FINAL
        }

        public static void EjecutarPrograma(string rutaArchivo, bool admin = false)
        {
            // Adaptado de http://stackoverflow.com/questions/2282448/windows-7-and-vista-uac-programatically-requesting-elevation-in-c-sharp

            var psInfo = new ProcessStartInfo(rutaArchivo);

            psInfo.Verb = admin ? "runas" : string.Empty;

            Process.Start(psInfo);

            //REGISTRO Win32Exception: Excepción producida normalmente al cancelar el diálogo del control de cuentas de usuario (UAC)
        }

        public void Bloquea2()
        {
            LockWorkStation();
        }

        public void EjecutarExterno(IntPtr puntero, string archivo, string argumentos, int modoVentana = 0)
        {
            ShellExecute(puntero, "open", archivo, argumentos, string.Empty, modoVentana);
        }

        public void Suspender(bool forzado, bool deshabilitarTempEncendido = true)
        {
            SetSuspendState(false, forzado, deshabilitarTempEncendido);
        }

        public void Hibernar(bool forzado, bool deshabilitarTempEncendido = true)
        {
            SetSuspendState(true, forzado, deshabilitarTempEncendido);
        }

        public void ProcesoShutdown(string argumentos)
        {
            var proceso = new Process();
            string args = string.Empty;

            proceso.StartInfo.UseShellExecute = false;
            proceso.StartInfo.FileName = "shutdown";
            proceso.StartInfo.Arguments = argumentos;
            proceso.StartInfo.CreateNoWindow = true;
            proceso.Start();
        }

        public void EjecutarProceso(string fichero)
        {
            Process.Start(fichero);           
        }

    }
}
