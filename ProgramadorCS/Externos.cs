using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;

namespace ProgramadorCS
{
    class NativeMethods
    {
        //static Process proceso = new Process();

        //Copiado de http://p2p.wrox.com/c/929-use-vbulletin-nets-shell-function-c.html
        //Otras fuentes: https://msdn.microsoft.com/es-es/library/windows/desktop/bb762153%28v=vs.85%29.aspx
        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ShellExecute(IntPtr hwnd, string lpVerb, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]

        public static extern void LockWorkStation();

        //Copiado de http://stackoverflow.com/questions/162331/finding-the-default-application-for-opening-a-particular-file-type-on-windows
        //Otras fuentes: https://github.com/Funbit/ets2-telemetry-server/blob/master/source/Funbit.Ets.Telemetry.Server/Helpers/ProcessHelper.cs
        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra, 
            [Out] StringBuilder pszOut, ref uint pcchOut);

        [Flags]
        public enum AssocF
        {
            InitNoRemapClsid = 0x1,
            InitByExeName = 0x2,
            OpenByExeName = 0x2,
            InitDefaultToStar = 0x4,
            InitDefaultToFolder = 0x8,
            NoUserSettings = 0x10,
            NoTruncate = 0x20,
            Verify = 0x40,
            RemapRunDll = 0x80,
            NoFixUps = 0x100,
            IgnoreBaseClass = 0x200
        }

        public enum AssocStr
        {
            Command = 1,
            Executable,
            FriendlyDocName,
            FriendlyAppName,
            NoOpen,
            ShellNewValue,
            DdeCommand,
            DdeIfExec,
            DdeApplication,
            DdeTopic
        }
    }

}
