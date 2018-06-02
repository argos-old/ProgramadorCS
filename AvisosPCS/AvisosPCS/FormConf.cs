using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Cifra2;
using Nini.Config;
using DevComponents.DotNetBar;
using Rep;


namespace AvisosPCS
{
    public partial class FormConf : OfficeForm
    {

        public FormConf(string[] args)
        {
            InitializeComponent();

            //NOTAS: argumentos requeridos: siempreEncima, bool activarAero, eStyle estilo
            //OBSERVACIONES: La siguiente sentencia asigna la matrizpara ejecuciones de pruebas
            args = args.Length > 2 ? args : new string[] { "false", "false", "2" };

            TopMost = bool.Parse(args[0]);
            EnableGlass = bool.Parse(args[1]);
            styleManager1.ManagerStyle = (eStyle)int.Parse(args[2]);

            cargaSettings();
        }

        void cargaSettings()
        {
            iniciando = true;

            if (!File.Exists(rutaConf))
                resetConfig();

            archivoConf = new IniConfigSource(rutaConf);
            archivoConf.AutoSave = true;
            config = archivoConf.Configs["Configuracion"];

            checkBoxVisual.Checked = avisoVisual;
            checkBoxSonoro.Checked = avisoSonoro;
            comboBoxAvisosVisuales.SelectedIndex = (int)tipoVisual;
            comboBoxAvisosSonoros.SelectedIndex = (int)tipoSonoro;
            checkBoxAlarmaSiempreEncima.Checked = dlgAvisoSiempreEncima;
            checkBoxMarquesinaMovil.Checked = marquesinaMovil;
            checkBoxColoreadoDinamico.Checked = coloreadoDinamico;
            textBoxMarquesina.Text = txtMarquesina;
            textBoxCuerpo.Text = txtCuerpo;
            textBoxPie.Text = txtPie;
            checkBoxPararAvisoSonoro.Checked = pararAvisoSonoro;
            checkBoxReproducirEnBucle.Checked = reproduccionBucle;
            integerInputSegundos.Value = segParadaAvSonoro;
            sliderVolumen.Value = (int)(volumen * 100);
            comboItemPersonal.Text = string.IsNullOrEmpty(temaPersonalElegido) ? "Otro (elegir)" : Path.GetFileNameWithoutExtension(temaPersonalElegido);
            superTabControl1.SelectedTabIndex = !indiceTab ? 0 : 1;

            iniciando = false;
        }

        #region " Propiedades "

        bool avisoVisual
        {
            get
            {
                return config.GetBoolean("AvisoVisual", true);
            }
            set
            {
                config.Set("AvisoVisual", value);
            }
        }

        bool avisoSonoro
        {
            get
            {
                return config.GetBoolean("AvisoSonoro", false);
            }
            set
            {
                config.Set("AvisoSonoro", value);
            }
        }

        TipoAvisoVisual tipoVisual
        {
            get
            {
                return (TipoAvisoVisual)config.GetInt("PosComboAvisoVisual", 0);
            }
            set
            {
                config.Set("PosComboAvisoVisual", (int)value);
            }
        }

        TipoAvisoSonoro tipoSonoro
        {
            get
            {
                //BORRAR: Si no se utiliza el getter
                return (TipoAvisoSonoro)config.GetInt("PosComboAvisoSonoro", 0);
            }
            set
            {
                config.Set("PosComboAvisoSonoro", (int)value);
            }
        }

        bool dlgAvisoSiempreEncima
        {
            get
            {
                return config.GetBoolean("DlgAvisoSiempreEncima", false);
            }
            set
            {
                config.Set("DlgAvisoSiempreEncima", value);
            }
        }

        bool coloreadoDinamico
        {
            get
            {
                return config.GetBoolean("ColoreadoDinamico", true);
            }
            set
            {
                config.Set("ColoreadoDinamico", value);

            }
        }

        bool marquesinaMovil
        {
            get
            {
                return config.GetBoolean("MarquesinaMovil", true);
            }
            set
            {
                config.Set("MarquesinaMovil", value);

            }
        }

        string txtMarquesina
        {
            get
            {
                string valor = config.Get("TxtMarquesina", string.Empty);
                return (valor == string.Empty) ? string.Empty : Funciones.DescifraTxt(valor);
            }
            set
            {
                config.Set("TxtMarquesina", Funciones.CifraTxt(value));
            }
        }

        string txtCuerpo
        {
            get
            {
                string valor = config.Get("TxtCuerpo", string.Empty);
                return (valor == string.Empty) ? string.Empty : Funciones.DescifraTxt(valor);
                //return valor;
            }
            set
            {
                config.Set("TxtCuerpo", Funciones.CifraTxt(value));
                //config.Set("TxtCuerpo", value);
            }
        }

        string txtPie
        {
            get
            {
                string valor = config.Get("TxtPie", string.Empty);
                return (valor == string.Empty) ? string.Empty : Funciones.DescifraTxt(valor);
                //return valor;
            }
            set
            {
                config.Set("TxtPie", Funciones.CifraTxt(value));
                //config.Set("TxtPie", value);
            }
        }

        bool pararAvisoSonoro
        {
            get
            {
                return config.GetBoolean("PararAvisoSonoro", false);
            }
            set
            {
                config.Set("PararAvisoSonoro", value);
            }
        }

        int segParadaAvSonoro
        {
            get
            {
                return config.GetInt("SegParadaAvSonoro", 20);
            }
            set
            {
                config.Set("SegParadaAvSonoro", value);
            }
        }

        bool reproduccionBucle
        {
            get
            {
                return config.GetBoolean("ReproduccionBucle", false);
            }
            set
            {
                config.Set("ReproduccionBucle", value);
            }
        }

        float volumen
        {
            get
            {
                return config.GetFloat("Volumen", 0.9f);
            }
            set
            {
                config.Set("Volumen", value);
            }
        }

        string temaPersonalElegido
        {
            get
            {
                string valor = config.Get("TemaPersonalElegido", Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)) ?? string.Empty;
                return (valor == string.Empty) ? string.Empty : Funciones.DescifraTxt(valor);
                //return valor;
            }
            set
            {
                config.Set("TemaPersonalElegido", Funciones.CifraTxt(value));
                //config.Set("TemaPersonalElegido", value);
            }
        }

        bool indiceTab
        {
            get
            {
                return config.GetBoolean("IndiceTab", false);
            }
            set
            {
                config.Set("IndiceTab", value);
            }
        }

        #endregion

        #region " Declaraciones "

        Reproductor reproductor = new Reproductor();

        bool iniciando;
        DateTime controlParadaTema;
        //PTE: Cambiar ruta hacia AppConfig
        static readonly string rutaConf = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Avisos.config"; //Path.GetDirectoryName(Application.ExecutablePath);

        static IConfigSource archivoConf;
        static IConfig config;

        #endregion

        #region " Funciones "

        DialogResult MsgBox(string sentencia, string titulo = "", MessageBoxIcon icono = MessageBoxIcon.None, MessageBoxButtons botones = MessageBoxButtons.OK)
        {
            MessageBoxEx.EnableGlass = EnableGlass;
            return MessageBoxEx.Show(this, sentencia, titulo, botones, icono, MessageBoxDefaultButton.Button1, TopMost);
        }

        void guardaTxt(string archivo, string texto)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(archivo)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(archivo));
                }

                FileMode fm = File.Exists(archivo) ? FileMode.Truncate : FileMode.CreateNew;

                using (var fs = new FileStream(archivo, fm))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(texto);
                        sw.Close();
                    }
                }
            }

            catch (UnauthorizedAccessException)
            {
                //PTE Terminar
                MsgBox("Se ha producido un error al intentar crear el archivo de configuración en " +
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\n\n Se in" +
                    "tentará crear en " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Error", MessageBoxIcon.Error);
            }
        }

        void resetConfig()
        {
            guardaTxt(rutaConf, ";Configuracion Avisos - ProgramadorCS");

            var iniConf = new IniConfigSource(rutaConf);

            iniConf.AddConfig("Configuracion");

            var seccion = iniConf.Configs["Configuracion"];

            seccion.Set("AvisoVisual", true);
            seccion.Set("AvisoSonoro", false);
            seccion.Set("PosComboAvisosVisual", 0);
            seccion.Set("PosComboAvisosSonoro", 1);
            seccion.Set("DlgAvisoSiempreEncima", false);
            seccion.Set("ColoreadoDinamico", true);
            seccion.Set("MarquesinaMovil", true);
            seccion.Set("TxtMarquesina", string.Empty);
            seccion.Set("TxtCuerpo", string.Empty);
            seccion.Set("TxtPie", Funciones.CifraTxt("Fecha y hora"));
            seccion.Set("PararAvisoSonoro", true);
            seccion.Set("SegParadaAvSonoro", 20);
            seccion.Set("ReproduccionBucle", false);
            seccion.Set("Volumen", 0.9f);
            seccion.Set("TemaPersonalElegido", string.Empty);
            seccion.Set("IndiceTab", false);

            iniConf.Save();
        }

        void checkCheckBoxAvisos(DevComponents.DotNetBar.Controls.CheckBoxX checkBoxAviso)
        {
            if (!checkBoxVisual.Checked && !checkBoxSonoro.Checked)
            {
                MsgBox("Al menos uno de los dos tipos de avisos debe estar seleccionado", "INFORMACIÓN", MessageBoxIcon.Information); ;
                checkBoxAviso.Checked = true;
            }
        }

        #endregion

        private void botonBuscarTemaPersonal_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Title = "Elija un archivo";
                ofd.Filter = "MP3|*.mp3|" +
                             "WAV|*.wav|" +
                             "El resto|*.*";
                ofd.FilterIndex = 0;
                ofd.InitialDirectory = string.IsNullOrEmpty(temaPersonalElegido) ?
                    Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) : Path.GetDirectoryName(temaPersonalElegido);
                ofd.FileName = string.Empty;
                ofd.ShowDialog();

                if (ofd.CheckFileExists && ofd.FileName.Length > 0)
                {
                    temaPersonalElegido = ofd.FileName;
                    comboItemPersonal.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
                    labelTemaSeleccionado.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
                    comboBoxAvisosSonoros.SelectedIndex = 30;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR BBTM", MessageBoxIcon.Error);
            }
        }

        private void checkBoxVisual_CheckedChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                try
                {
                    avisoVisual = checkBoxVisual.Checked;

                    checkCheckBoxAvisos(checkBoxVisual);
                }

                catch (Exception ex)
                {
                    MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
                }

            }
        }

        private void checkBoxSonoro_CheckedChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                try
                {
                    avisoSonoro = checkBoxSonoro.Checked;

                    checkCheckBoxAvisos(checkBoxSonoro);
                }

                catch (Exception ex)
                {
                    MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
                }
            }
        }

        private void comboBoxAvisosVisuales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                try
                {
                    switch (comboBoxAvisosVisuales.SelectedIndex)
                    {
                        case 0:
                            tipoVisual = TipoAvisoVisual.Monocromo;
                            break;
                        case 1:
                            tipoVisual = TipoAvisoVisual.Azules;
                            break;
                        case 2:
                            tipoVisual = TipoAvisoVisual.Verdes;
                            break;
                        case 3:
                            tipoVisual = TipoAvisoVisual.Estridente;
                            break;
                        case 4:
                            tipoVisual = TipoAvisoVisual.Esquela;
                            break;
                    }
                }

                catch (Exception ex)
                {
                    MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
                }
            }
        }

        private void comboBoxAvisosSonoros_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    switch (comboBoxAvisosSonoros.SelectedItem.ToString().ToLower())
                    {
                        case "bip":
                            tipoSonoro = TipoAvisoSonoro.Bip;
                            break;
                        case "gallo":
                            tipoSonoro = TipoAvisoSonoro.Gallo;
                            break;
                        case "alarma incendios":
                            tipoSonoro = TipoAvisoSonoro.AlarmaIncendios;
                            break;
                        case "aplausos":
                            tipoSonoro = TipoAvisoSonoro.Aplausos;
                            break;
                        case "bip 2":
                            tipoSonoro = TipoAvisoSonoro.Bip2;
                            break;
                        case "bip 3":
                            tipoSonoro = TipoAvisoSonoro.Bip3;
                            break;
                        case "burro":
                            tipoSonoro = TipoAvisoSonoro.Burro;
                            break;
                        case "campanillas":
                            tipoSonoro = TipoAvisoSonoro.Campanillas;
                            break;
                        case "corazón monitorizado":
                            tipoSonoro = TipoAvisoSonoro.CorazonLatiendo;
                            break;
                        case "corazón latiendo":
                            tipoSonoro = TipoAvisoSonoro.CorazonMonitorizado;
                            break;
                        case "despertador digital":
                            tipoSonoro = TipoAvisoSonoro.DespertadorDigital;
                            break;
                        case "despertador antiguo":
                            tipoSonoro = TipoAvisoSonoro.DespertadorAntiguo;
                            break;
                        case "doce campanadas":
                            tipoSonoro = TipoAvisoSonoro.DoceCampanadas;
                            break;
                        case "llamada enterprise":
                            tipoSonoro = TipoAvisoSonoro.LlamadaEnterprise;
                            break;
                        case "metralleta":
                            tipoSonoro = TipoAvisoSonoro.Metralleta;
                            break;
                        case "risa bebé":
                            tipoSonoro = TipoAvisoSonoro.RisaBebe;
                            break;
                        case "risa femenina":
                            tipoSonoro = TipoAvisoSonoro.RisaFemenina;
                            break;
                        case "risa masculina":
                            tipoSonoro = TipoAvisoSonoro.RisaMasculina;
                            break;
                        case "ritmo percusión 1":
                            tipoSonoro = TipoAvisoSonoro.RitmoPercusion1;
                            break;
                        case "ritmo percusión 2":
                            tipoSonoro = TipoAvisoSonoro.RitmoPercusion2;
                            break;
                        case "ritmo percusión 3":
                            tipoSonoro = TipoAvisoSonoro.RitmoPercusion3;
                            break;
                        case "ritmo percusión 4":
                            tipoSonoro = TipoAvisoSonoro.RitmoPercusion4;
                            break;
                        case "ritmo militar":
                            tipoSonoro = TipoAvisoSonoro.RitmoMilitar;
                            break;
                        case "ritmo redoble":
                            tipoSonoro = TipoAvisoSonoro.RitmoRedoble;
                            break;
                        case "ritmo timbales":
                            tipoSonoro = TipoAvisoSonoro.RitmoTimbales;
                            break;
                        case "robot":
                            tipoSonoro = TipoAvisoSonoro.Robot;
                            break;
                        case "sirena maderos":
                            tipoSonoro = TipoAvisoSonoro.SirenaMaderos;
                            break;
                        case "teléfono antiguo":
                            tipoSonoro = TipoAvisoSonoro.TelefonoAntiguo;
                            break;
                        case "teléfono digital":
                            tipoSonoro = TipoAvisoSonoro.TelefonoDigital;
                            break;
                        case "tic tac":
                            tipoSonoro = TipoAvisoSonoro.TicTac;
                            break;
                        default:
                            tipoSonoro = TipoAvisoSonoro.TemaPersonal;
                            break;
                    }
                }
                if (tipoSonoro == TipoAvisoSonoro.TemaPersonal && string.IsNullOrEmpty(temaPersonalElegido))
                {
                    labelTiempoTotal.Text = "00:00";
                    labelMomentoActual.Text = "00:00";
                    labelTemaSeleccionado.Text = comboBoxAvisosSonoros.SelectedItem.ToString();
                    setParadaPlayback();
                    return;
                }

                setParadaPlayback();
                reproductor = new Reproductor();
                reproductor.CargarAvisoSonoro();
                labelTiempoTotal.Text = reproductor.TiempoTotal.ToString(@"mm\:ss");
                labelTemaSeleccionado.Text = comboBoxAvisosSonoros.SelectedItem.ToString();
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR CBAS", MessageBoxIcon.Error);
                comboBoxAvisosSonoros.SelectedIndex = 29;
            }
        }

        private void textBoxMarquesina_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    txtMarquesina = textBoxMarquesina.Text;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void textBoxCuerpo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    txtCuerpo = textBoxCuerpo.Text;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void textBoxPie_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    txtPie = textBoxPie.Text;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void checkBoxAlarmaSiempreEncima_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    dlgAvisoSiempreEncima = checkBoxAlarmaSiempreEncima.Checked;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void checkBoxMarquesinaMovil_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    marquesinaMovil = checkBoxMarquesinaMovil.Checked;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void checkBoxPararAvisoSonoro_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    pararAvisoSonoro = checkBoxPararAvisoSonoro.Checked;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void integerInputSegundos_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    segParadaAvSonoro = integerInputSegundos.Value;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void checkBoxReproducirEnBucle_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    reproduccionBucle = checkBoxReproducirEnBucle.Checked;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void sliderVolumen_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    volumen = (float)sliderVolumen.Value / sliderVolumen.Maximum;

                    if (reproductor.Estado != EstadoReproductor.Parado)
                    {
                        reproductor.Volumen = volumen;
                    }
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void botonReproducirAvisoVisual_Click(object sender, EventArgs e)
        {
            try
            {
                superTabControl1.SelectedTabIndex = 0;

                var psi = new ProcessStartInfo(Application.StartupPath + @"\AvisosRep.exe");
                psi.Arguments = "*v " + TopMost.ToString() + " " + EnableGlass.ToString() + " " +
                    ((int)styleManager1.ManagerStyle).ToString(); ;

                Process.Start(psi);
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void botonReproducirAvisoSonoro_Click(object sender, EventArgs e)
        {
            try
            {
                superTabControl1.SelectedTabIndex = 1;

                if (reproductor.Estado != EstadoReproductor.Reproduciendo)
                {
                    reproductor.ReproducirAvisoSonoro();
                    botonReproducirAvisoSonoro.Symbol = "\xf04d";
                    labelTiempoTotal.Text = reproductor.TiempoTotal.ToString(@"mm\:ss");
                    timerControlPlayback.Interval = 250;
                    timerControlPlayback.Start();

                    if (pararAvisoSonoro)
                    {
                        controlParadaTema = DateTime.Now;
                    }
                }
                else
                {
                    setParadaPlayback();
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR BRAS", MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Este temporizador suple las carencias del evento PlaybackStopped de IWavePlayer.El autor, Mark Heath, desaconseja
        /// su utilización. Básicamente monitoriza el playback para detener el reproductor.
        /// </summary>
        private void timerControlPlayback_Tick(object sender, EventArgs e)
        {
            if (reproductor.Estado != EstadoReproductor.Reproduciendo)
            {
                setParadaPlayback();
            }
            else
            {
                if (pararAvisoSonoro && DateTime.Now >= controlParadaTema.AddSeconds(integerInputSegundos.Value))
                {
                    setParadaPlayback();
                    return;
                }

                labelMomentoActual.Text = reproductor.TiempoActual.ToString(@"mm\:ss");
            }
        }

        void setParadaPlayback()
        {
            reproductor.Detener();
            botonReproducirAvisoSonoro.Symbol = "\xf04b";
            labelMomentoActual.Text = "00:00";
            timerControlPlayback.Stop();
        }

        private void tabAvisosVisuales_Click(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    indiceTab = false;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void tabAvisosSonoros_Click(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    indiceTab = true;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void botonProbar_Click(object sender, EventArgs e)
        {
            try
            {
                var psi = new ProcessStartInfo(Application.StartupPath + @"\AvisosRep.exe");
                psi.Arguments = "* " + TopMost.ToString() + " " + EnableGlass.ToString() + " " +
                    ((int)styleManager1.ManagerStyle).ToString();

                Process.Start(psi);
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }            
        }

        private void checkBoxColoreadoDinamico_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!iniciando)
                {
                    coloreadoDinamico = checkBoxColoreadoDinamico.Checked;
                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void textBoxMarquesina_ButtonCustomClick(object sender, EventArgs e)
        {
            try
            {
                textBoxMarquesina.Text = string.Empty;
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void textBoxCuerpo_ButtonCustomClick(object sender, EventArgs e)
        {
            try
            {
                textBoxCuerpo.Text = string.Empty;
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void textBoxPie_ButtonCustomClick(object sender, EventArgs e)
        {
            try
            {
                textBoxPie.Text = string.Empty;
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void textBoxPie_ButtonCustom2Click(object sender, EventArgs e)
        {
            try
            {
                textBoxPie.Text = "Fecha y hora";
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
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
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void botonReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (MsgBox("Se volverá a los valores predeterminados.\nperdiendo lo que hubiera cambiado\n\n¿Desea continuar?",
                    "RESET", MessageBoxIcon.Exclamation, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {

                    checkBoxVisual.Checked = true;
                    checkBoxSonoro.Checked = false;
                    comboBoxAvisosVisuales.SelectedIndex = 0;
                    comboBoxAvisosSonoros.SelectedIndex = 1;
                    checkBoxAlarmaSiempreEncima.Checked = false;
                    checkBoxColoreadoDinamico.Checked = true;
                    checkBoxMarquesinaMovil.Checked = true;
                    textBoxMarquesina.Text = string.Empty;
                    textBoxCuerpo.Text = string.Empty;
                    textBoxPie.Text = "";
                    checkBoxPararAvisoSonoro.Checked = true;
                    integerInputSegundos.Value = 20;
                    checkBoxReproducirEnBucle.Checked = false;
                    sliderVolumen.Value = 90;
                    comboItemPersonal.Text = "Otro (elegir)";
                    superTabControl1.SelectedTabIndex = 0;

                }
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }


    }
}
