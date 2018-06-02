using System.Windows.Forms;

namespace Acinme
{
    public partial class FormInme : Form
    {
        public FormInme(string[] args)
        {
            InitializeComponent();
            ShowInTaskbar = false;
            procesaParametros(args);
        }

        void procesaParametros(string[] argumentos)
        {
            try
            {
                if (argumentos[0] == "*a")
                {
                    NativeMethods.ProcesoShutdown("-s -t 0");
                }
                else if (argumentos[0] == "*r")
                {
                    NativeMethods.ProcesoShutdown("-r -t 0");
                }
                else if (argumentos[0] == "*c")
                {
                    NativeMethods.ProcesoShutdown("-l");
                }
                else if (argumentos[0] == "*s")
                {
                    Application.SetSuspendState(PowerState.Suspend, true, true);
                }
                else if (argumentos[0] == "*h")
                {
                    NativeMethods.SetSuspendState(true, true, true);
                }
                else if (argumentos[0] == "*b")
                {
                    NativeMethods.LockWorkStation();
                }
            }

            catch (System.Exception)
            {
                //MessageBox.Show(ex.Message, "ERROR ACINME");
            }

        }

        private void FormInme_Load(object sender, System.EventArgs e)
        {
            Application.Exit();
        }
    }
}
