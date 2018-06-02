using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevComponents.DotNetBar;

using Cifra2;

namespace ProgramadorCS
{
    public partial class FormMail : OfficeForm
    {
        #region " Constructor "

        public FormMail()
        {
            InitializeComponent();
            cargaSettings();
        }

        void cargaSettings()
        {
            iniciando = true;

            TopMost = siempreEncima;
            EnableGlass = activarAero;
            textBoxRemitente.Text = remitente;
            textBoxUsuario.Text = usuario;
            textBoxContrasena.Text = contrasena;
            textBoxHost.Text = host;
            textBoxPuerto.Text = puerto.ToString();
            textBoxdestinatario.Text = destinatario;
            textBoxCcCco.Text = cccco;
            textBoxAsunto.Text = asunto;
            richTextBoxCuerpo.Text = cuerpo;
            switchButtonSSL.Value = ssl;
            comboBoxCcCco.SelectedIndex = posComboCcCco ? 0 : 1;
            comboBoxConfiguracion.SelectedIndex = (byte)proveedor;
            
            iniciando = false;
        }

        #endregion

        #region " Propiedades "

        /// <summary>
        /// Se establece como true después de desencadenarse el evento SendCompleted 
        /// </summary>

        /// <summary>
        /// Propiedad que se consultará al instanciar la clase para saber si existe algún error con el archivo adjunto. Sólo
        /// devolverá error si el archivoAdjunto está establecido y no existe por haberse borrado, renombrado, etc.
        /// </summary>

        public bool EnvioAceptado
        {
            private get
            {
                return envioAceptado;
            }
            set
            {
                envioAceptado = value;
                OnEnvioAceptado(null);
            }
        }

        bool configuracionCorrecta
        {
            get
            {
                return Properties.Settings.Default.FMConfiguracionCorrecta;
            }
            set
            {
                Properties.Settings.Default.FMConfiguracionCorrecta = value;
            }
        }

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

        bool ssl
        {
            get
            {
                return Properties.Settings.Default.FMSSL;
            }
            set
            {
                Properties.Settings.Default.FMSSL = value;
            }
        }

        bool posComboCcCco
        {
            get
            {
                return Properties.Settings.Default.FMPosComboCcCco;
            }
            set
            {
                Properties.Settings.Default.FMPosComboCcCco = value;
            }
        }

        Preconfiguraciones proveedor
        {
            get
            {
                return (Preconfiguraciones)Properties.Settings.Default.FMPosComboPreconfiguracion;
            }
            set
            {
                Properties.Settings.Default.FMPosComboPreconfiguracion = (byte)value;
            }
        }

        string remitente 
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMRemitente);
            }
            set
            {
                Properties.Settings.Default.FMRemitente = Funciones.CifraTxt(value);
            }
        }

        string usuario
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMUsuario);
            }
            set
            {
                Properties.Settings.Default.FMUsuario = Funciones.CifraTxt(value);
            }
        }

        string host 
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMHost);
            }
            set
            {
                Properties.Settings.Default.FMHost = Funciones.CifraTxt(value);
            }
        }

        string contrasena
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMContrasena);
            }
            set
            {
                Properties.Settings.Default.FMContrasena = Funciones.CifraTxt(value);
            }
        }

        string destinatario
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMDestinatario);
            }
            set
            {
                Properties.Settings.Default.FMDestinatario = Funciones.CifraTxt(value);
            }
        }

        string cccco
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMCcCco);
            }
            set
            {
                Properties.Settings.Default.FMCcCco = Funciones.CifraTxt(value);
            }
        }

        string asunto
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMAsunto);
            }
            set
            {
                Properties.Settings.Default.FMAsunto = Funciones.CifraTxt(value);
            }
        }

        string cuerpo
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMCuerpo);
            }
            set
            {
                Properties.Settings.Default.FMCuerpo = Funciones.CifraTxt(value);
            }
        }

        string archivoAdjunto
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMArchivoAdjunto);
            }
            set
            {
                Properties.Settings.Default.FMArchivoAdjunto = Funciones.CifraTxt(value);
            }
        }

        string version
        {
            get
            {
                return Properties.Settings.Default.Version;
            }
        }

        ushort puerto
        {
            get
            {
                //return ushort.Parse(Funciones.DescifraTxt(Properties.Settings.Default.FMPuerto.ToString()));
                return Properties.Settings.Default.FMPuerto; //No es un dato sensible
                //Para utilizarlo habría que "vaciar" FMPuerto. También convendría pasarlo a string

            }
            set
            {
                //Properties.Settings.Default.FMPuerto = ushort.Parse(Funciones.CifraTxt(value.ToString()));
                Properties.Settings.Default.FMPuerto = value;
                //idem arriba
            }
        }

        #endregion

        #region " Declaraciones "

        bool iniciando;
        bool envioAceptado;
        bool avisadoPreconf;
        event PropertyValueChangedEventHandler EnvioAceptadoCambiado;

        #endregion

        #region " Funciones "

        void setLabelsAdjunto()
        {
            if (File.Exists(archivoAdjunto))
            {
                var infoAdjunto = new FileInfo(archivoAdjunto);

                labelAdjuntoNombre.Text = Path.GetFileName(archivoAdjunto);
                labelAdjuntoTamano.Text = RAM.setBytes((ulong)infoAdjunto.Length);

                superTooltip1.SetSuperTooltip(labelAdjuntoNombre, new SuperTooltipInfo(string.Empty, string.Empty, archivoAdjunto,
                    null, null, eTooltipColor.Gray));

                //botonAdjunto.Image = Properties.Resources.broom16;
                botonAdjunto.Symbol = "";          
            }

            else
            {
                labelAdjuntoNombre.Text = string.Empty;
                labelAdjuntoTamano.Text = string.Empty;

                superTooltip1.SetSuperTooltip(labelAdjuntoNombre, new SuperTooltipInfo(string.Empty, string.Empty, string.Empty,
                    null, null, eTooltipColor.Gray));

                //botonAdjunto.Image = Properties.Resources.attachmentD16;
                botonAdjunto.Symbol = "";
            }
        }

        bool checkRgxMail(string direccion)
        {
            //Copiado de http://stackoverflow.com/questions/16167983/best-regular-expression-for-email-validation-in-c-sharp

            return Regex.IsMatch(direccion, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        /// <param name="obligatorio">Esta bool se debe establecer como true en caso de que la variable que cubre el 
        /// textBox es imprescindible. Ej: textBoxRemitente es obligatorio; por tanto: true. textBoxCcCco puede no
        /// necesitarse, asi que obligatorio = false</param>
        bool checkCampoMail(TextBox textBox, bool obligatorio = true)
        {
            if (!obligatorio && string.IsNullOrEmpty(textBox.Text))
            {
                return true;
            }

            if (!checkRgxMail(textBox.Text) || textBox.Text.Length > 254)
            {
                return false;
            }

            return true;
        }

        void checkMail(TextBox textBox)
        {
            string direccion = textBox.Text;

            if (!string.IsNullOrEmpty(direccion) && (!checkRgxMail(direccion) || direccion.Length > 254) &&
                MsgBox("Parece que la dirección de correo introducida no es válida.\n\n¿Desea corregirla?",
                "Dirección de mail no válida", MessageBoxIcon.Error, MessageBoxButtons.YesNo) == DialogResult.Yes) {

                textBox.Focus();
            }
        }

        DialogResult MsgBox(string sentencia, string titulo = "", MessageBoxIcon icono = MessageBoxIcon.None, MessageBoxButtons botones = MessageBoxButtons.OK)
        {
            MessageBoxEx.EnableGlass = activarAero;
            return MessageBoxEx.Show(this, sentencia, titulo, botones, icono, MessageBoxDefaultButton.Button1, siempreEncima);
        }

        #endregion

        #region " Carga y salida "

        private void FormMail_Load(object sender, EventArgs e)
        {
            try
            {
                setLabelsAdjunto();
               
                if (!File.Exists(archivoAdjunto))
                {
                    archivoAdjunto = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void FormMail_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool checkFinalSuperado = false;

            try
            {
                if (!checkCampoMail(textBoxRemitente) || !checkCampoMail(textBoxdestinatario) || !checkCampoMail(textBoxCcCco, false)
                    || string.IsNullOrEmpty(textBoxHost.Text) || string.IsNullOrEmpty(textBoxUsuario.Text) ||
                    string.IsNullOrEmpty(textBoxContrasena.Text)) {

                    checkFinalSuperado = false;
                }

                else
                {
                    ushort x;

                    if (!ushort.TryParse(textBoxPuerto.Text, out x) || puerto == 0)
                    {
                        checkFinalSuperado = false;
                    }
                    else
                    {
                        checkFinalSuperado = true;
                    }
                }

                if (!checkFinalSuperado && MsgBox("Parece que no todos los campos se han rellenado de forma correcta.\n\nSi cierr" +
                    "a la ventana de configuración, tal y como está el formulario,\nel envío del correo no se hará de forma corre" + 
                    "cta.\n\n¿Está seguro de querer salir?", "Datos incorrectos", MessageBoxIcon.Warning, MessageBoxButtons.YesNo)
                    == DialogResult.No) {

                    e.Cancel = true;
                }
               
                Properties.Settings.Default.Save();
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }

            finally
            {
                configuracionCorrecta = checkFinalSuperado;
            }
        }

        #endregion

        #region " Controles "

        private void textBoxRemitente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    remitente = textBoxRemitente.Text;
                }
            }
            catch(Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void textBoxRemitente_Leave(object sender, EventArgs e)
        {
            try
            {
                checkMail(textBoxRemitente);
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void textBoxUsuario_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    usuario = textBoxUsuario.Text;
                }                
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void textBoxContrasena_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    contrasena = textBoxContrasena.Text;
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void botonAuxiliarTextBoxContrasena_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxContrasena.UseSystemPasswordChar ^= true;
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void textBoxHost_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    host = textBoxHost.Text;
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void textBoxPuerto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    if (!string.IsNullOrEmpty(textBoxPuerto.Text))
                    {
                        puerto = ushort.Parse(textBoxPuerto.Text);
                        return;
                    }

                    puerto = 0;
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message + "\n\nEl valor ha de ser un número entero no superior a 65535", version, MessageBoxIcon.Error);

                textBoxPuerto.Text = textBoxPuerto.Text.Remove(textBoxPuerto.Text.Length - 1);
                textBoxPuerto.Select(textBoxPuerto.Text.Length, 0);
                textBoxPuerto.ScrollToCaret();
            }
        }

        private void switchButtonSSL_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    ssl = switchButtonSSL.Value;
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void textBoxdestinatario_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    destinatario = textBoxdestinatario.Text;
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }

        }

        private void textBoxdestinatario_Leave(object sender, EventArgs e)
        {
            try
            {
                checkMail(textBoxdestinatario);
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void comboBoxCcCco_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    posComboCcCco = comboBoxCcCco.SelectedIndex == 0 ? true : false;                   
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void textBoxCcCco_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    cccco = textBoxCcCco.Text;
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }

        }
       
        private void textBoxCcCco_Leave(object sender, EventArgs e)
        {
            try
            {
                checkMail(textBoxCcCco);
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void textBoxAsunto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    asunto = textBoxAsunto.Text;
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void botonAdjunto_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                if (!File.Exists(archivoAdjunto)) //OBSERVACIONES: Este condicionamiento está para el switch adjunto/borrar
                {
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
                    ofd.FilterIndex = 0; //PTE: Crear propiedad y pasar a settings
                    ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //idem
                    ofd.FileName = string.Empty;
                    ofd.ShowDialog();

                    if (ofd.CheckFileExists)
                    {
                        archivoAdjunto = ofd.FileName;
                    }
                }

                else
                {
                    archivoAdjunto = string.Empty;
                }

                setLabelsAdjunto();
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void richTextBoxCuerpo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    cuerpo = richTextBoxCuerpo.Text;
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }
        }

        private void botonAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }      
        }

        private void botonAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Title = "Elija un archivo de texto plano o enriquecido";
                ofd.Filter = "Documentos texto plano|*.txt; *.log; *.dic|" +
                                         "Texto enriquecido|*.rtf|" +
                                         "Otros|*.xml; *.htm; *.html; *.ini; *.key; *.reg; *.diz;";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                ofd.ShowDialog();

                if (ofd.CheckFileExists)
                {
                    string archivoTxt = ofd.FileName;

                    if (Path.GetExtension(archivoTxt) != ".rtf")
                    {
                        using (StreamReader sr= new StreamReader(archivoTxt, Encoding.Default))
                        {
                            richTextBoxCuerpo.Text = sr.ReadToEnd();
                        }
                    }

                    else
                    {
                        richTextBoxCuerpo.LoadFile(archivoTxt);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }

        }

        private void botonProbar_Click(object sender, EventArgs e)
        {
            try
            {
                new Mail(true).EnviarMail();  
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }     
        }

        private void comboBoxConfiguracion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    switch (comboBoxConfiguracion.SelectedItem.ToString())
                    {
                        case "Otro":
                            proveedor = Preconfiguraciones.Ninguna;
                            textBoxHost.Text = string.Empty;
                            textBoxPuerto.Text = "25";
                            textBoxHost.WatermarkText = "smpt..";
                            textBoxRemitente.WatermarkText = "mimail@mail.com";
                            textBoxUsuario.WatermarkText = "mimail@mail.com";
                            textBoxdestinatario.WatermarkText = "unmail@mail.com";
                            textBoxCcCco.WatermarkText = "otromail@mail.com";
                            switchButtonSSL.Value = true;
                            break;

                        case "Gmail":
                            proveedor = Preconfiguraciones.Gmail;
                            textBoxHost.Text = "smtp.gmail.com";
                            textBoxPuerto.Text = "25";
                            textBoxHost.WatermarkText = "smtp.gmail.com";
                            textBoxRemitente.WatermarkText = "mimail@gmail.com";
                            textBoxUsuario.WatermarkText = "mimail@gmail.com";
                            textBoxdestinatario.WatermarkText = "unmail@gmail.com";
                            textBoxCcCco.WatermarkText = "otromail@gmail.com";
                            switchButtonSSL.Value = true;
                            break;

                        case "Yahoo!":
                            proveedor = Preconfiguraciones.Yahoo;
                            textBoxHost.Text = "smtp.mail.yahoo.com";
                            textBoxPuerto.Text = "25";
                            textBoxHost.WatermarkText = "smtp.mail.yahoo.com";
                            textBoxRemitente.WatermarkText = "mimail@yahoo.com";
                            textBoxUsuario.WatermarkText = "mimail@yahoo.com";
                            textBoxdestinatario.WatermarkText = "unmail@yahoo.com";
                            textBoxCcCco.WatermarkText = "otromail@yahoo.com";
                            switchButtonSSL.Value = true;
                            break;

                        case "Hotmail":
                            proveedor = Preconfiguraciones.Hotmail;
                            textBoxHost.Text = "smtp.live.com";
                            textBoxPuerto.Text = "25";
                            textBoxHost.WatermarkText = "smtp.live.com";
                            textBoxRemitente.WatermarkText = "mimail@hotmail.com";
                            textBoxUsuario.WatermarkText = "mimail@hotmail.com";
                            textBoxdestinatario.WatermarkText = "unmail@hotmail.com";
                            textBoxCcCco.WatermarkText = "otromail@hotmail.com";
                            switchButtonSSL.Value = true;
                            break;

                        case "iCloud":
                            proveedor = Preconfiguraciones.iCloud;
                            textBoxHost.Text = "smtp.mail.me.com";
                            textBoxPuerto.Text = "587";
                            textBoxHost.WatermarkText = "smtp.mail.me.com";
                            textBoxRemitente.WatermarkText = "mimail@icloud.com";
                            textBoxUsuario.WatermarkText = "mimail@icloud.com";
                            textBoxdestinatario.WatermarkText = "unmail@icloud.com";
                            textBoxCcCco.WatermarkText = "otromail@icloud.com";
                            switchButtonSSL.Value = true;
                            break;

                        case "Ono":
                            proveedor = Preconfiguraciones.Ono;
                            textBoxHost.Text = "smtp.ono.com";
                            textBoxPuerto.Text = "25";
                            textBoxHost.WatermarkText = "smtp.ono.com";
                            textBoxRemitente.WatermarkText = "mimail@ono.com";
                            textBoxUsuario.WatermarkText = "mimail@ono.com";
                            textBoxdestinatario.WatermarkText = "unmail@ono.com";
                            textBoxCcCco.WatermarkText = "otromail@ono.com";
                            switchButtonSSL.Value = true;
                            break;

                        case "Telefónica":
                            proveedor = Preconfiguraciones.Telefonica;
                            textBoxHost.Text = "smtp.telefonica.net";
                            textBoxPuerto.Text = "25";
                            textBoxHost.WatermarkText = "smtp.telefonica.net";
                            textBoxRemitente.WatermarkText = "mimail@telefonica.net";
                            textBoxUsuario.WatermarkText = "mimail@telefonica.net";
                            textBoxdestinatario.WatermarkText = "unmail@telefonica.net";
                            textBoxCcCco.WatermarkText = "otromail@telefonica.net";
                            switchButtonSSL.Value = true;
                            break;

                        case "Movistar":
                            proveedor = Preconfiguraciones.Movistar;
                            textBoxHost.Text = "smtp.movistar.es";
                            textBoxPuerto.Text = "25";
                            textBoxHost.WatermarkText = "smtp.movistar.es";
                            textBoxRemitente.WatermarkText = "mimail@movistar.es";
                            textBoxUsuario.WatermarkText = "mimail@movistar.es";
                            textBoxdestinatario.WatermarkText = "unmail@movistar.es";
                            textBoxCcCco.WatermarkText = "otromail@movistar.es";
                            switchButtonSSL.Value = true;
                            break;

                        case "Vodafone":
                            proveedor = Preconfiguraciones.Vodafone;
                            textBoxHost.Text = "smtp.vodafone.es";
                            textBoxPuerto.Text = "25";
                            textBoxHost.WatermarkText = "smtp.vodafone.es";
                            textBoxRemitente.WatermarkText = "mimail@vodafone.es";
                            textBoxUsuario.WatermarkText = "mimail (sin @vodafone.es)";
                            textBoxdestinatario.WatermarkText = "unmail@vodafone.es";
                            textBoxCcCco.WatermarkText = "otromail@vodafone.es";
                            switchButtonSSL.Value = true;
                            break;

                        case "Orange":
                            proveedor = Preconfiguraciones.Orange;
                            textBoxHost.Text = "smtp.orange.es";
                            textBoxPuerto.Text = "25";
                            textBoxHost.WatermarkText = "smtp.orange.es";
                            textBoxRemitente.WatermarkText = "mimail@orange.es";
                            textBoxUsuario.WatermarkText = "mimail@orange.es";
                            textBoxdestinatario.WatermarkText = "unmail@orange.es";
                            textBoxCcCco.WatermarkText = "otromail@orange.es";
                            switchButtonSSL.Value = true;
                            break;

                        case "Euskastel":
                            proveedor = Preconfiguraciones.Euskastel;
                            textBoxHost.Text = "smtp.live.com";
                            textBoxPuerto.Text = "25";
                            textBoxHost.WatermarkText = "smtp.live.com";
                            textBoxRemitente.WatermarkText = "mimail@euskaltel.net";
                            textBoxUsuario.WatermarkText = "mimail@euskaltel.net";
                            textBoxdestinatario.WatermarkText = "unmail@euskaltel.net";
                            textBoxCcCco.WatermarkText = "otromail@euskaltel.net";
                            switchButtonSSL.Value = true;
                            break;
                    }

                    if (!avisadoPreconf)
                    {
                        MsgBox("Los datos de preconfiguración introducidos son orientativos.\nTenga en cuenta que pueden existir " + 
                            "variaciones.", version, MessageBoxIcon.Information);
                        avisadoPreconf = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Error);
            }

        }

        #endregion

        #region " Varios "

        enum Preconfiguraciones
        {
            Ninguna,
            Gmail,
            Yahoo,
            Hotmail,
            iCloud,
            Ono,
            Telefonica,
            Movistar,
            Vodafone,
            Orange,
            Euskastel
        }

        protected virtual void OnEnvioAceptado(PropertyValueChangedEventArgs e)
        {
            var handler = EnvioAceptadoCambiado;

            if (handler != null)
            {
                handler(this, e);
            }

            //NOTA: Se puede dar el caso de que al enviar un mensaje al destinatio con un nº de puerto incorrecto el evento
            //     SendCompleted se desencadene. ¿Responsabilidad del Host o de NET? pero sobre todo, ¿cómo solventarlo?
            MsgBox(host + " admitió el envío del mensaje\n\nNota: No es lo habitual, pero si ha introducido un\nnúmero de " + 
                "puerto no válido, puede que el envío\nno llegue a completarse", "Envío e-mail correcto", 
                MessageBoxIcon.Information);


            configuracionCorrecta = true; 
            envioAceptado = false; //OBSERVACIONES: Cambia a false a la espera de otro cambio
        }

        #endregion

    }
        
}
