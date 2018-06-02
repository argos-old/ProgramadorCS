using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Cifra2;
using Nini.Config;
using NAudio.Wave;
using DevComponents.DotNetBar;
using Rep;

namespace AvisosRep
{
    public partial class FormAvisos : OfficeForm
    {
        #region " Constructor "

        public FormAvisos(string[] args)
        {
            InitializeComponent();

            //NOTAS: argumentos requeridos: simVisual siempreEncima, bool activarAero, eStyle estilo.
            //NOTAS 2: Si args[0] != *v el tipoFormulario será Formulario.Ambos si == *v será Formulario.Visual
            //OBSERVACIONES: La siguiente sentencia asigna la matrizpara ejecuciones de pruebas
            args = (args.Length > 2) ? args : new string[] { "*", "false", "false", "2" };
            argSiempreEncima = bool.Parse(args[1]); //OBSERVACIONES: Se setea la bool para establecer TopMost en Load
            simVisual = args[0] == "*v" ? true : false;
            EnableGlass = bool.Parse(args[2]);
            styleManager1.ManagerStyle = (eStyle)int.Parse(args[3]);

            cargaSettings();

        }

        void cargaSettings()
        {
        
        
        !!!!!!!! Creo que aquí está el error. Dónde se crea el archivo si no existe?? (Anotación externa.. debería producir error)
        
            iniciando = true;

            archivoConf = new IniConfigSource(rutaConf);
            archivoConf.AutoSave = true;
            config = archivoConf.Configs["Configuracion"];

            sliderVolumen.Value = (int)(volumen * 100);
            setBotonControlVolumen();
            labelMarquesina.Text = txtMarquesina;
            labelCuerpo.Text = txtCuerpo;
            labelPie.Text = txtPie;

            iniciando = false;
        }

        #endregion

        #region " Propiedades "

        bool avisoVisual
        {
            get
            {
                return config.GetBoolean("AvisoVisual", true);
            }
        }

        bool avisoSonoro
        {
            get
            {
                return config.GetBoolean("AvisoSonoro", false);
            }
        }

        TipoAvisoVisual tipoVisual
        {
            get
            {
                return (TipoAvisoVisual)config.GetInt("PosComboAvisoVisual", 0);
            }
        }

        TipoAvisoSonoro tipoSonoro
        {
            get
            {
                return (TipoAvisoSonoro)config.GetInt("PosComboAvisoSonoro", 0);
            }
        }

        bool dlgAvisoSiempreEncima
        {
            get
            {
                return config.GetBoolean("DlgAvisoSiempreEncima", false);
            }
        }

        bool coloreadoDinamico
        {
            get
            {
                return config.GetBoolean("ColoreadoDinamico", true);
            }
        }

        bool marquesinaMovil
        {
            get
            {
                return config.GetBoolean("MarquesinaMovil", true);
            }
        }

        string txtMarquesina
        {
            get
            {
                string valor = config.Get("TxtMarquesina", string.Empty);
                return (valor == string.Empty) ? string.Empty : Funciones.DescifraTxt(valor);
                //return valor;
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
        }

        string txtPie
        {
            get
            {
                string valor = config.Get("TxtPie", string.Empty);
                return (valor == string.Empty) ? string.Empty : Funciones.DescifraTxt(valor);
                //return valor;
            }
        }

        bool pararAvisoSonoro
        {
            get
            {
                return config.GetBoolean("PararAvisoSonoro", false);
            }
        }

        int segParadaAvSonoro
        {
            get
            {
                return config.GetInt("SegParadaAvSonoro", 20);
            }
        }

        bool reproduccionBucle
        {
            get
            {
                return config.GetBoolean("ReproduccionBucle", false);
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
        }

        #endregion

        #region " Declarariones "

        Reproductor reproductor = new Reproductor();

        bool iniciando;
        bool simVisual;
        bool argSiempreEncima;
        short xLabelMarquesina;
        Volumen nivelVolumen;
        Formulario tipoForm;

        //PTE: Cambiar ruta hacia AppConfig
        static readonly string rutaConf = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Avisos.config"; //Path.GetDirectoryName(Application.ExecutablePath);

        static IConfigSource archivoConf;
        static IConfig config;

        #endregion

        #region " Subrutinas Comunes "

        DialogResult MsgBox(string sentencia, string titulo = "", MessageBoxIcon icono = MessageBoxIcon.None, MessageBoxButtons botones = MessageBoxButtons.OK)
        {
            MessageBoxEx.EnableGlass = EnableGlass;
            return MessageBoxEx.Show(this, sentencia, titulo, botones, icono, MessageBoxDefaultButton.Button1, TopMost);
        }

        void setBotonControlVolumen()
        {
            if (volumen == 0)
            {
                botonControlVolumen.Symbol = "\xf026";
                nivelVolumen = Volumen.Minimo;
            }

            else if (volumen > 0.65f)
            {
                botonControlVolumen.Symbol = "\xf028";
                nivelVolumen = Volumen.Maximo;
            }

            else
            {
                botonControlVolumen.Symbol = "\xf027";
                nivelVolumen = Volumen.Medio;
            }
        }

        void switchTemaVisual()
        {
            switch (tipoVisual)
            {
                case TipoAvisoVisual.Monocromo:

                    if (labelCuerpo.BackColor == Color.DimGray)
                    {
                        panelArriba.BackColor = Color.DimGray;
                        panelAbajo.BackColor = Color.DimGray;
                        panelDcha.BackColor = Color.DimGray;
                        panelIzda.BackColor = Color.DimGray;
                        labelCuerpo.BackColor = Color.Silver;
                    }

                    else
                    {
                        panelArriba.BackColor = Color.Silver;
                        panelAbajo.BackColor = Color.Silver;
                        panelDcha.BackColor = Color.Silver;
                        panelIzda.BackColor = Color.Silver;
                        labelCuerpo.BackColor = Color.DimGray;
                    }
                    break;

                case TipoAvisoVisual.Azules:

                    if (labelCuerpo.BackColor == Color.Teal)
                    {
                        panelArriba.BackColor = Color.Teal;
                        panelAbajo.BackColor = Color.Teal;
                        panelDcha.BackColor = Color.Teal;
                        panelIzda.BackColor = Color.Teal;
                        labelCuerpo.BackColor = Color.Turquoise;
                    }

                    else
                    {
                        panelArriba.BackColor = Color.Turquoise;
                        panelAbajo.BackColor = Color.Turquoise;
                        panelDcha.BackColor = Color.Turquoise;
                        panelIzda.BackColor = Color.Turquoise;
                        labelCuerpo.BackColor = Color.Teal;
                    }
                    break;

                case TipoAvisoVisual.Verdes:

                    if (labelCuerpo.BackColor == Color.DarkOliveGreen)
                    {
                        panelArriba.BackColor = Color.DarkOliveGreen;
                        panelAbajo.BackColor = Color.DarkOliveGreen;
                        panelDcha.BackColor = Color.DarkOliveGreen;
                        panelIzda.BackColor = Color.DarkOliveGreen;
                        labelCuerpo.BackColor = Color.GreenYellow;
                    }

                    else
                    {
                        panelArriba.BackColor = Color.GreenYellow;
                        panelAbajo.BackColor = Color.GreenYellow;
                        panelDcha.BackColor = Color.GreenYellow;
                        panelIzda.BackColor = Color.GreenYellow;
                        labelCuerpo.BackColor = Color.DarkOliveGreen;
                    }
                    break;


                case TipoAvisoVisual.Estridente:

                    if (labelCuerpo.BackColor == Color.Lime)
                    {
                        panelArriba.BackColor = Color.Lime;
                        panelAbajo.BackColor = Color.Lime;
                        panelDcha.BackColor = Color.Lime;
                        panelIzda.BackColor = Color.Lime;
                        labelCuerpo.BackColor = Color.Orange;
                        labelCuerpo.ForeColor = Color.Green;
                        labelMarquesina.ForeColor = Color.SaddleBrown;
                        labelPie.ForeColor = Color.SaddleBrown;
                    }

                    else
                    {
                        panelArriba.BackColor = Color.Orange;
                        panelAbajo.BackColor = Color.Orange;
                        panelDcha.BackColor = Color.Orange;
                        panelIzda.BackColor = Color.Orange;
                        labelCuerpo.BackColor = Color.Lime;
                        labelCuerpo.ForeColor = Color.SaddleBrown;
                        labelMarquesina.ForeColor = Color.Green;
                        labelPie.ForeColor = Color.Green;
                    }
                    break;

                case TipoAvisoVisual.Esquela:

                    if (labelCuerpo.BackColor == Color.White)
                    {
                        panelArriba.BackColor = Color.White;
                        panelAbajo.BackColor = Color.White;
                        panelDcha.BackColor = Color.White;
                        panelIzda.BackColor = Color.White;
                        labelCuerpo.BackColor = Color.Black;
                        labelCuerpo.ForeColor = Color.White;
                        labelMarquesina.ForeColor = Color.Black;
                        labelPie.ForeColor = Color.Black;
                    }

                    else
                    {
                        panelArriba.BackColor = Color.Black;
                        panelAbajo.BackColor = Color.Black;
                        panelDcha.BackColor = Color.Black;
                        panelIzda.BackColor = Color.Black;
                        labelCuerpo.BackColor = Color.White;
                        labelCuerpo.ForeColor = Color.Black;
                        labelMarquesina.ForeColor = Color.White;
                        labelPie.ForeColor = Color.White;
                    }
                    break;
            }
        }

        /// <summary>
        /// Función creada para establecer TopMost en Load en vez de en el contructor para evitar problemas
        /// </summary>
        bool setSiempreEncima(bool parametro)
        {
            return dlgAvisoSiempreEncima ? true : parametro;
        }

        #endregion

        #region " Carga "

        private void FormAvisos_Load(object sender, EventArgs e)
        {
            TopMost = setSiempreEncima(argSiempreEncima);

            switchTemaVisual();

            if (simVisual)
            {
                tipoForm = Formulario.Visual;
            }

            else if (avisoVisual && !avisoSonoro)
            {
                tipoForm = Formulario.Visual;
            }

            else if (avisoSonoro && !avisoVisual)
            {
                tipoForm = Formulario.Sonido;
            }

            else
            {
                tipoForm = Formulario.Ambos;
            }

            if (tipoForm == Formulario.Visual)
            {
                expandablePanel1.Expanded = false;
                expandablePanel1.ExpandButtonVisible = false;
                expandablePanel1.TitleText = "Aviso Visual";
            }

            else if (tipoForm == Formulario.Sonido)
            {
                labelMarquesina.Text = "Aviso Sonoro";
                MaximumSize = new System.Drawing.Size(607, 215);
                MaximizeBox = false;
                MinimizeBox = false;
                expandablePanel1.ExpandButtonVisible = false;
                expandablePanel1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
                expandablePanel1.TitleText = tipoSonoro == TipoAvisoSonoro.TemaPersonal ||
                    string.IsNullOrEmpty(temaPersonalElegido) ? Path.GetFileName(temaPersonalElegido) : tipoSonoro.ToString();
            }

            if (tipoForm != Formulario.Visual) //Sonoros:
            {
                reproductor.CargarAvisoSonoro();
                reproductor.ReproducirAvisoSonoro();
            }

            if (tipoForm != Formulario.Sonido) //Visuales
            {
                labelMarquesina.Text = txtMarquesina;
                labelCuerpo.Text = txtCuerpo;
                labelPie.Text = txtPie;

                if (labelPie.Text.ToLower() == "fecha y hora")
                {
                    timerFechaHora.Start();
                }

                if (marquesinaMovil)
                {
                    labelMarquesina.Font = new Font("Calibri", 14F, FontStyle.Italic, GraphicsUnit.Point, 0);
                    timerMarquesinaMovil.Start();
                }

                else
                {
                    labelMarquesina.AutoSize = false;
                    labelMarquesina.Dock = DockStyle.Fill;
                }

                if (coloreadoDinamico)
                {
                    timerConmutaColores.Start();
                }
            }
        }

        #endregion

        #region " Temporización "

        private void timerFechaHora_Tick(object sender, EventArgs e)
        {
            labelPie.Text = DateTime.Now.ToString("dddd, d/MM/yyyy - HH:mm:ss");
        }

        private void timerMarquesinaMovil_Tick(object sender, EventArgs e)
        {
            xLabelMarquesina += (xLabelMarquesina < (short)540) ? (short)3 : (short)(xLabelMarquesina * -1);

            labelMarquesina.Location = new Point(xLabelMarquesina, 10);
        }

        private void timerConmutaColores_Tick(object sender, EventArgs e)
        {
            switchTemaVisual();
        }

        #endregion

        #region " Controles Sonido "

        private void botonReproducir_Click(object sender, EventArgs e)
        {
            reproductor.CargarAvisoSonoro();
            reproductor.ReproducirAvisoSonoro();
        }

        private void botonParada_Click(object sender, EventArgs e)
        {
            if (reproductor.Estado != EstadoReproductor.Parado)
            {
                reproductor.Detener();
            }
        }

        private void sliderVolumen_ValueChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                volumen = (float)sliderVolumen.Value / sliderVolumen.Maximum;

                setBotonControlVolumen();

                if (reproductor.Estado != EstadoReproductor.Parado)
                {
                    reproductor.Volumen = volumen;
                }
            }
        }

        private void botonControlVolumen_Click(object sender, EventArgs e)
        {
            switch (nivelVolumen)
            {
                case Volumen.Minimo:
                    volumen = .5f;
                    botonControlVolumen.Symbol = "\xf027";
                    nivelVolumen = Volumen.Medio;
                    break;

                case Volumen.Medio:
                    volumen = 1f;
                    botonControlVolumen.Symbol = "\xf028";
                    nivelVolumen = Volumen.Maximo;
                    break;

                case Volumen.Maximo:
                    volumen = 0f;
                    botonControlVolumen.Symbol = "\xf026";
                    nivelVolumen = Volumen.Minimo;
                    break;
            }

            sliderVolumen.Value = (int)(volumen * 100);

            if (reproductor.Estado != EstadoReproductor.Parado)
            {
                reproductor.Volumen = volumen;
            }

        }

        #endregion
    }
}
