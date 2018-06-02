using System;
using System.IO;
using System.Windows.Forms;
using System.Text;
using DevComponents.DotNetBar;


namespace ProgramadorCS
{
    public partial class FormEjecuciones : OfficeForm
    {
        #region " Constructor "

        public FormEjecuciones()
        {
            InitializeComponent();
            cargaSettings();
            
        }

        void cargaSettings()
        {
            iniciando = true;

            TopMost = siempreEncima;
            EnableGlass = activarAero;
            textBoxProceso.Text = archivoSeleccionado;

            iniciando = false;
        }

        #endregion

        #region " Propiedades "

        bool siempreEncima
        {
            get
            {
                return Properties.Settings.Default.SiempreEncima;
            }
        }

        bool activarAero
        {
            get
            {
                return Properties.Settings.Default.ActivarAero;
            }
        }

        bool configuracionCorrecta
        {
            get
            {
                return Properties.Settings.Default.FEConfiguracionCorrecta;
            }
            set
            {
                Properties.Settings.Default.FEConfiguracionCorrecta = value;
            }
        }

        string version
        {
            get
            {
                return Properties.Settings.Default.Version;
            }
        }

        string archivoSeleccionado
        {
            get
            {
                return Properties.Settings.Default.FEArchivoSeleccionado;
            }
            set
            {
                Properties.Settings.Default.FEArchivoSeleccionado = value;
            }
        }

        byte indiceFiltro
        {
            get
            {
                return Properties.Settings.Default.FEIndiceFiltro;
            }
            set
            {
                Properties.Settings.Default.FEIndiceFiltro = value;
            }
        }

        string directorioInicial
        {
            get
            {
                string dir = Properties.Settings.Default.FEDirectorioInicial;
                  
                return string.IsNullOrEmpty(dir) || !Directory.Exists(dir) ?
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : 
                    Properties.Settings.Default.FEDirectorioInicial;
            }
            set
            {
                Properties.Settings.Default.FEDirectorioInicial = value;
            }
        }

        #endregion

        #region " Declaraciones "

        bool iniciando;

        #endregion

        #region " Carga y salida "

        private void FormEjecuciones_Load(object sender, EventArgs e)
        {
            textBoxProceso.Select(textBoxProceso.Text.Length, 0);
            textBoxProceso.ScrollToCaret();
        }

        private void FormEjecuciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!File.Exists(archivoSeleccionado))
            {
                if (MsgBox("No se encuentra el archivo seleccionado; por tanto,\nsi continúa el proceso no se ejecutará de " + 
                    "forma correcta.\n\n¿Desea continuar?", "Error con archivo", MessageBoxIcon.Error, MessageBoxButtons.YesNo) 
                    == DialogResult.No) {

                    e.Cancel = true;
                }

                configuracionCorrecta = false;
                return;
            }

            else if (Path.GetFileName(programaAsociado(Path.GetExtension(archivoSeleccionado))).ToLower() == "openwith.exe")
            {
                if (MsgBox("Parece que el archivo seleccionado no tiene asociado ningún programa.\nSi se selecciona se abr" +
                    "irá el diálogo 'Abrir Con...' y por tanto no se ejecutará.\n\n¿Desea continuar?", "Error con archivo",
                    MessageBoxIcon.Exclamation, MessageBoxButtons.YesNo) == DialogResult.No) {

                    e.Cancel = true;
                }

                configuracionCorrecta = false;
                return;
            }

            configuracionCorrecta = true;
            Properties.Settings.Default.Save();
        }
        
        #endregion

        #region " Funciones " 

        DialogResult MsgBox(string sentencia, string titulo = "", MessageBoxIcon icono = MessageBoxIcon.None, MessageBoxButtons botones = MessageBoxButtons.OK)
        {
            MessageBoxEx.EnableGlass = activarAero;
            return MessageBoxEx.Show(this, sentencia, titulo, botones, icono, MessageBoxDefaultButton.Button1, siempreEncima);
        }

        //Extraído de http://stackoverflow.com/questions/162331/finding-the-default-application-for-opening-a-particular-file-type-on-windows
        string programaAsociado(string extension)
        {
            uint length = 0;
            var ret = NativeMethods.AssocQueryString(NativeMethods.AssocF.NoUserSettings, NativeMethods.AssocStr.Executable, 
                extension, null, null, ref length);
            var sb = new StringBuilder((int)length);

            ret = NativeMethods.AssocQueryString(NativeMethods.AssocF.NoUserSettings, NativeMethods.AssocStr.Executable, 
                extension, null, sb, ref length);

            return sb.ToString();
        }

        #endregion

        #region " Controles "

        private void textBoxProceso_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    archivoSeleccionado = textBoxProceso.Text;
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void botonAuxTextBoxLimpiar_Click(object sender, EventArgs e)
        {
            textBoxProceso.Text = string.Empty;
        }

        private void botonBuscar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Elija un archivo";
            ofd.Filter = "Documentos|*.doc; *.docx; *.odt; *.pdf; *.html; *.rtf; *.txt|" +
                                     "Ejecutables|*.exe; *.com; *.bat|" +
                                     "Imágenes|*.jpg; *.png; *.svg; *.gif; *.eps; *.odi; *.ico; *.bmp|" +
                                     "OpenDocument|*.odt; *.ods; *.odp; *.odb; *.odg; *.odc; *.odf; *.odm|" +
                                     "Office|*.docx; *.docx; *.xlsx; *.xls; *.pptx; *.ppt *.mbd; *accdb|" +
                                     "Comprimidos|*.zip; *.rar; *.gz; *.7z; *.z; *.cab; *.iso; *.tar; *.jar|" +
                                     "Sonido|*.mp3; *.ogg; *.wma; *.ac3; *.mp4; *.cda; *.mid; *.wav|" +
                                     "Vídeo|*.avi; *.ogg; *.mp4; *.mpeg; *.mkv; *.mov; *.3gp; *3g2|" +
                                     "El resto|*.*";
            ofd.FilterIndex = indiceFiltro;
            ofd.InitialDirectory = directorioInicial;
            ofd.FileName = string.Empty;
            ofd.ShowDialog();

            if (ofd.CheckFileExists && ofd.FileName.Length > 0)
            {
                directorioInicial = Path.GetDirectoryName(ofd.FileName);
                textBoxProceso.Text = ofd.FileName;
                indiceFiltro = (byte)ofd.FilterIndex;
            }
        }

        private void botonProbar_Click(object sender, EventArgs e)
        {
            new Acciones().EjecutarProceso(archivoSeleccionado);
        }

        private void botonAceptar_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

    }
}
