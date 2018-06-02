using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using DevComponents.DotNetBar;
using Dolinay;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.ApplicationServices;
using Microsoft.WindowsAPICodePack.Shell; //Si sólo se utiliza para los stockicons, eliminar la directiva
using Microsoft.WindowsAPICodePack.Taskbar;
using Redes;


namespace ProgramadorCS
{
    public partial class FormPpal : RibbonForm
    {
        #region " Constructor "

        public FormPpal(string[] args)
        {
            InitializeComponent();
            setPropiedadesExtra();
            setEventosEnergia();
            setThumbnailButtons();
            cargaSettings();

            procesaParametros(args);
        }

        void setPropiedadesExtra()
        {

            // NOTA: No consigo personalizar el texto del botón. TodayButton.Text no setea la cadena.

            dateTimeInputA1Hora.MonthCalendar.TodayButton.Symbol = ""; //OBSERVACIONES: Símbolo calendario
            dateTimeInputA1Hora.MonthCalendar.TodayButton.Tooltip = "Ir a Hoy";
            dateTimeInputA1Hora.MonthCalendar.ClearButtonVisible = false;
            notifyIcon1.Text = version;
            notifyIcon1.BalloonTipTitle = version;

            //PRUEBAS: Añadiendo controles extra a la cabecera del ribbon ...
            this.ribbonControl.QuickToolbarItems.AddRange(new BaseItem[] //OBSERVACIONES: RemoveRange para eliminar 
            { 
                new ButtonItem("botonTest3", "Test 3"),
                new ComboBoxItem("comboboxItemX", "Combo"),
                new SwitchButtonItem("switchButtonItemX", "Switch 1")
            });//*/
        }

        void setEventosEnergia()
        {
            if (PowerManager.IsBatteryPresent)
            {
                PowerManager.BatteryLifePercentChanged += new EventHandler(vidaPorcentualBateria_Changed);
            }

            PowerManager.PowerPersonalityChanged += new EventHandler(planEnergia_Changed);
            PowerManager.PowerSourceChanged += new EventHandler(alimentacion_Changed);
        }

        void setThumbnailButtons()
        {
            // Constructor de Thumbnail
            Icon iconoThumbnailIniciar = Properties.Resources._64player_play;
            Icon iconoThumbnailPausa = Properties.Resources._64player_pause;
            Icon iconoThumbnailParar = Properties.Resources._64player_stop;

            ThumbnailToolBarButton botonThumbnailIniciar = new ThumbnailToolBarButton(iconoThumbnailIniciar, "Iniciar");
            ThumbnailToolBarButton botonThumbnailPausa = new ThumbnailToolBarButton(iconoThumbnailPausa, "Pausa");
            ThumbnailToolBarButton botonThumbnailParar = new ThumbnailToolBarButton(iconoThumbnailParar, "Parar");

            botonThumbnailIniciar.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(botonIniciar_Click);
            botonThumbnailPausa.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(botonPausa_Click);
            botonThumbnailParar.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(botonParar_Click);

            TaskbarManager.Instance.ThumbnailToolBars.AddButtons(Handle, botonThumbnailIniciar, botonThumbnailPausa, botonThumbnailParar);
        }

        void cargaSettings()
        {
            iniciando = true;

            EnableGlass = activarAero;
            switchButtonItemAero.Value = activarAero;
            TopMost = siempreEncima;
            setPaleta();
            setEstiloTabControl();
            setRadioAccion();
            setTabPpal();
            setRadioCarga();
            setRadioEnergia();
            setComboAlimentacion();
            setComboPlanEnergia();
            comboBox1SubBajRed.SelectedIndex = posCombo1BajadaRed ? 1 : 0; //NOTA: Debe ir antes de setCombo1Uds o integrarlo
            setCombo1Uds();
            comboBox2SubBajRed.SelectedIndex = posCombo2BajarRed ? 1 : 0;  // Idem anterior
            setCombo2UdsxSeg();
            setPosRadioSecBat();
            setPosRadioRed();
            setComboUds();
            setComboTipoUd();
            setComboMagnitudUds();
            setJumpList();
            timeSelectorCuentaAtras.SelectedTime = horaCuentaAtras;
            dateTimeInputA1Hora.Value = horaA1Hora < DateTime.Now ? DateTime.Today.AddDays(1) : horaA1Hora.Subtract(horaA1Hora.TimeOfDay);
            timeSelectorA1Hora.SelectedTime = horaA1Hora.TimeOfDay;
            dateTimeInputTiempoRestanteBat.Value = DateTime.Today.Add(tiempoRestanteBateria);
            switchButtonItemSiempreEncima.Value = siempreEncima;
            switchButtonItemMonitorizacion.Value = verMonitorizacion;
            switchButtonItemOverlayIcons.Value = verOverlayIcons;
            switchButtonItemMinimizarBandeja.Value = minimizarBandeja;
            switchButtonItemIniciarConWindows.Value = iniciarConWindows;
            comboBox1CargaEquipo.SelectedIndex = esMayorComboCargaEquipo ? 1 : 0;
            comboBoxBatMasMenos.SelectedIndex = esMasComboBateria ? 1 : 0;
            comboBoxMayorMenorRed.SelectedIndex = posComboMayorRed ? 1 : 0;
            comboBoxDesConexionUds.SelectedIndex = posComboDesConexionUd ? 1 : 0;
            comboBoxMayorMenorUds.SelectedIndex = posComboMayorUd ? 1 : 0;
            comboBoxYOUds.SelectedIndex = posComboYOUd ? 1 : 0;
            integerInputDatosCargaEquipo.Value = datosIntegerInputCargaEquipo;
            integerInputVidaUtilBatPorcent.Value = porcentajeRestanteBateria;
            integerInputMinutosCargaEquipo.Value = minutosIntegerInputCargaEquipo;
            integerInputMinutosAlimentacion.Value = minutosIntegerInputAlimentacion;
            integerInputMinutosRed.Value = minutosIntegerInputRed;
            integerInput1CantidadUdsRed.Value = cantidadUds1IntegerInputRed;
            integerInput2CantidadUdsRed.Value = cantidadUds2IntegerInputRed;
            integerInputUds.Value = cantidadUdsUd;
            integerInputRetardoAcciones.Value = segundosRetardoAcciones;
            checkBoxDuranteAlimentacion.Checked = duranteAlimentacion;
            checkBoxDuranteRed.Checked = duranteRed;
            checkBoxForzarCierre.Checked = forzarCierre;
            checkBoxReinicioAppsReg.Checked = reinicioAppsReg;
            checkBoxSiUdEs.Checked = siUdEs;
            checkBoxSiUdTieneCapacTot.Checked = siUdTieneCapacTot;
            checkBoxRetardarAccionesX.Checked = retardarAcciones;
            buttonItemInicioMinimizado.Checked = iniciarMinimizado;

            iniciando = false;
        }

        #endregion

        #region " Propiedades "

        bool siempreEncima
        {
            get
            {
                return ProgramadorCS.Properties.Settings.Default.SiempreEncima;
            }
            set
            {
                ProgramadorCS.Properties.Settings.Default.SiempreEncima = value;
            }
        }

        bool activarAero
        {
            get
            {
                return Properties.Settings.Default.ActivarAero;
            }
            set
            {
                Properties.Settings.Default.ActivarAero = value;
            }
        }

        bool verMonitorizacion
        {
            get
            {
                return Properties.Settings.Default.VerMonitorizacion;
            }
            set
            {
                Properties.Settings.Default.VerMonitorizacion = value;
            }
        }

        bool esMayorComboCargaEquipo
        {
            get
            {
                return Properties.Settings.Default.EsMayorComboCargaEquipo;
            }
            set
            {
                Properties.Settings.Default.EsMayorComboCargaEquipo = value;
            }
        }

        bool esMasComboBateria
        {
            get
            {
                return Properties.Settings.Default.EsMasComboBateria;
            }
            set
            {
                Properties.Settings.Default.EsMasComboBateria = value;
            }
        }

        bool posRadioSecBat
        {
            get
            {
                return Properties.Settings.Default.PosRadioSecBat;
            }
            set
            {
                Properties.Settings.Default.PosRadioSecBat = value;
            }
        }

        bool posRadioRed
        {
            get
            {
                return Properties.Settings.Default.PosRadioRed;
            }
            set
            {
                Properties.Settings.Default.PosRadioRed = value;
            }
        }

        bool posCombo1BajadaRed
        {
            get
            {
                return Properties.Settings.Default.PosCombo1BajadaRed;
            }
            set
            {
                Properties.Settings.Default.PosCombo1BajadaRed = value;
            }
        }

        bool posComboMayorRed
        {
            get
            {
                return Properties.Settings.Default.PosComboMayorRed;
            }
            set
            {
                Properties.Settings.Default.PosComboMayorRed = value;
            }
        }

        int cantidadUds1IntegerInputRed
        {
            get
            {
                return Properties.Settings.Default.CantidadUds1IntegerInputRed;
            }
            set
            {
                Properties.Settings.Default.CantidadUds1IntegerInputRed = value;
            }
        }

        Magnitud UdsxSegRed
        {
            get
            {
                return (Magnitud)Properties.Settings.Default.PosCombo1UdsRed;
            }
            set
            {
                Properties.Settings.Default.PosCombo1UdsRed = (byte)value;
            }
        }

        Magnitud Uds2Red
        {
            get
            {
                return (Magnitud)Properties.Settings.Default.PosCombo2UdsRed;
            }
            set
            {
                Properties.Settings.Default.PosCombo2UdsRed = (byte)value;
            }
        }

        bool posCombo2BajarRed
        {
            get
            {
                return Properties.Settings.Default.PosCombo2BajarRed;
            }
            set
            {
                Properties.Settings.Default.PosCombo2BajarRed = value;
            }
        }

        int cantidadUds2IntegerInputRed
        {
            get
            {
                return Properties.Settings.Default.CantidadUds2IntegerInputRed;
            }
            set
            {
                Properties.Settings.Default.CantidadUds2IntegerInputRed = value;
            }
        }

        byte posComboAdaptador
        {
            get
            {
                return Properties.Settings.Default.PosComboAdaptador;
            }
            set
            {
                Properties.Settings.Default.PosComboAdaptador = value;
            }
        }

        bool forzarCierre
        {
            get
            {
                return Properties.Settings.Default.ForzarCierre;
            }
            set
            {
                Properties.Settings.Default.ForzarCierre = value;
            }
        }

        bool reinicioAppsReg
        {
            get
            {
                return Properties.Settings.Default.ReinicioAppsReg;
            }
            set
            {
                Properties.Settings.Default.ReinicioAppsReg = value;
            }
        }

        bool duranteAlimentacion
        {
            get
            {
                return Properties.Settings.Default.DuranteAlimentacion;
            }
            set
            {
                Properties.Settings.Default.DuranteAlimentacion = value;
            }
        }

        bool duranteRed
        {
            get
            {
                return Properties.Settings.Default.DuranteRed;
            }
            set
            {
                Properties.Settings.Default.DuranteRed = value;
            }
        }

        bool verOverlayIcons
        {
            get
            {
                return Properties.Settings.Default.VerOverlayIcons;
            }
            set
            {
                Properties.Settings.Default.VerOverlayIcons = value;
            }
        }

        bool jumpListON
        {
            get
            {
                return Properties.Settings.Default.JumpListON;
            }
            set
            {
                Properties.Settings.Default.JumpListON = value;
            }
        }

        bool mailConfigCorrecta
        {
            get
            {
                return Properties.Settings.Default.FMConfiguracionCorrecta;
            }
        }

        bool ejecucionConfigCorrecta
        {
            get
            {
                return Properties.Settings.Default.FEConfiguracionCorrecta;
            }
        }

        int datosIntegerInputCargaEquipo
        {
            get
            {
                return Properties.Settings.Default.DatosIntegerInputCargaEquipo;
            }
            set
            {
                Properties.Settings.Default.DatosIntegerInputCargaEquipo = value;
            }
        }

        int minutosIntegerInputCargaEquipo
        {
            get
            {
                return Properties.Settings.Default.MinutosIntegerInputCargaEquipo;
            }
            set
            {
                Properties.Settings.Default.MinutosIntegerInputCargaEquipo = value;
            }
        }

        int minutosIntegerInputAlimentacion
        {
            get
            {
                return Properties.Settings.Default.MinutosIntegerInputAlimentacion;
            }
            set
            {
                Properties.Settings.Default.MinutosIntegerInputAlimentacion = value;
            }
        }

        int minutosIntegerInputRed
        {
            get
            {
                return Properties.Settings.Default.MinutosIntegerInputRed;
            }
            set
            {
                Properties.Settings.Default.MinutosIntegerInputRed = value;
            }
        }

        int porcentajeRestanteBateria
        {
            get
            {
                return Properties.Settings.Default.PorcentajeRestanteBateria;
            }
            set
            {
                Properties.Settings.Default.PorcentajeRestanteBateria = (byte)value;
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

        byte posComboUds
        {
            get
            {
                return Properties.Settings.Default.PosComboUds;
            }
            set
            {
                Properties.Settings.Default.PosComboUds = value;
            }
        }

        bool posComboDesConexionUd
        {
            get
            {
                return Properties.Settings.Default.PosComboDesConexionUd;
            }
            set
            {
                Properties.Settings.Default.PosComboDesConexionUd = value;
            }
        }

        bool siUdEs
        {
            get
            {
                return Properties.Settings.Default.SiUdEs;
            }
            set
            {
                Properties.Settings.Default.SiUdEs = value;
            }
        }

        byte posComboTipoUd
        {
            get
            {
                return Properties.Settings.Default.PosComboTipoUd;
            }
            set
            {
                Properties.Settings.Default.PosComboTipoUd = value;
            }
        }

        bool siUdTieneCapacTot
        {
            get
            {
                return Properties.Settings.Default.SiUdTieneCapacTot;
            }
            set
            {
                Properties.Settings.Default.SiUdTieneCapacTot = value;
            }
        }

        bool posComboMayorUd
        {
            get
            {
                return Properties.Settings.Default.PosComboMayorUd;
            }
            set
            {
                Properties.Settings.Default.PosComboMayorUd = value;
            }
        }

        byte cantidadUdsUd
        {
            get
            {
                return Properties.Settings.Default.CantidadUdsIntegerInputUd;
            }
            set
            {
                Properties.Settings.Default.CantidadUdsIntegerInputUd = value;
            }
        }

        bool posComboYOUd
        {
            get
            {
                return Properties.Settings.Default.PosComboYOUd;
            }
            set
            {
                Properties.Settings.Default.PosComboYOUd = value;
            }
        }

        bool retardarAcciones
        {
            get
            {
                return Properties.Settings.Default.RetardarAcciones;
            }
            set
            {
                Properties.Settings.Default.RetardarAcciones = value;
            }
        }

        short segundosRetardoAcciones
        {
            get
            {
                return Properties.Settings.Default.SegundosIntegerInputRetardarAcciones;
            }
            set
            {
                Properties.Settings.Default.SegundosIntegerInputRetardarAcciones = value;
            }
        }

        bool minimizarBandeja
        {
            get
            {
                return Properties.Settings.Default.MinimizarBandeja;
            }
            set
            {
                Properties.Settings.Default.MinimizarBandeja = value;
            }
        }

        bool iniciarConWindows
        {
            get
            {
                return Properties.Settings.Default.IniciarConWindows;
            }
            set
            {
                Properties.Settings.Default.IniciarConWindows = value;
            }
        }

        bool iniciarMinimizado
        {
            get
            {
                return Properties.Settings.Default.IniciarMinimizado;
            }
            set
            {
                Properties.Settings.Default.IniciarMinimizado = value;
            }
        }

        Magnitud MagnitudUd
        {
            get
            {
                return (Magnitud)Properties.Settings.Default.PosComboMagnitudUd;
            }
            set
            {
                Properties.Settings.Default.PosComboMagnitudUd = (byte)value;
            }
        }

        tipoAccion radioAccion
        {
            get
            {
                return (tipoAccion)Properties.Settings.Default.PosRadioAccion;
            }
            set
            {
                Properties.Settings.Default.PosRadioAccion = (byte)value;
            }
        }

        /*public tipoAccion Accion
        {
            get;
            set;
        } */

        opcionCarga carga
        {
            get
            {
                return (opcionCarga)Properties.Settings.Default.PosRadioCarga;
            }
            set
            {
                Properties.Settings.Default.PosRadioCarga = (byte)value;
            }
        }

        opcionEnergia energia
        {
            get
            {
                return (opcionEnergia)Properties.Settings.Default.PosRadioEnergia;
            }
            set
            {
                Properties.Settings.Default.PosRadioEnergia = (byte)value;
            }
        }

        /*opcionAlimentacion alimentacion
        {
            get
            {
                return (opcionAlimentacion)Properties.Settings.Default.PosComboAlimentacion;
            }
            set
            {
                Properties.Settings.Default.PosComboAlimentacion = (byte)value;
            }
        }*/

        PowerSource alimentacion
        {
            get
            {
                return (PowerSource)Properties.Settings.Default.PosComboAlimentacion;
            }
            set
            {
                Properties.Settings.Default.PosComboAlimentacion = (byte)value;
            }
        }

        PowerPersonality planEnergia
        {
            get
            {
                return (PowerPersonality)Properties.Settings.Default.PosComboPlanEnergia;
            }
            set
            {
                Properties.Settings.Default.PosComboPlanEnergia = (byte)value;
            }
        }

        tipoCondicion condicion
        {
            get
            {
                return (tipoCondicion)Properties.Settings.Default.PosTabPpal;
            }
            set
            {
                Properties.Settings.Default.PosTabPpal = (byte)value;
            }
        }

        tabRibbon posRibbonTab
        {
            get
            {
                return (tabRibbon)Properties.Settings.Default.PosRibbonTab;
            }
            set
            {
                Properties.Settings.Default.PosRibbonTab = (byte)value;
            }
        }

        eStyle paleta
        {
            get
            {
                return (eStyle)Properties.Settings.Default.Paletas;
            }
            set
            {
                Properties.Settings.Default.Paletas = (byte)value;
            }
        }

        eSuperTabStyle estiloTabControl
        {
            get
            {
                return (eSuperTabStyle)Properties.Settings.Default.EstiloTabControl;
            }
            set
            {
                Properties.Settings.Default.EstiloTabControl = (byte)value;
            }
        }

        TimeSpan horaCuentaAtras
        {
            get
            {
                return Properties.Settings.Default.HoraCuentaAtras;
            }
            set
            {
                Properties.Settings.Default.HoraCuentaAtras = value;
            }
        }

        DateTime horaA1Hora
        {
            get
            {
                return Properties.Settings.Default.HoraA1Hora;
            }
            set
            {
                Properties.Settings.Default.HoraA1Hora = value;
            }
        }

        TimeSpan tiempoRestanteBateria
        {
            get
            {
                return Properties.Settings.Default.TiempoRestanteBateria;
            }
            set
            {
                Properties.Settings.Default.TiempoRestanteBateria = value;
            }
        }

        #endregion

        #region " Declaraciones "

        bool alimentacionEnabled = false;
        bool iniciando = true;
        bool pausaT1;
        bool planEnergiaEnabled = false;
        bool vidaBateriaEnabled = false;
        double cuentaConsultalRed;
        int cuentaAlarmaCarga;
        int cuentaAlarmaEnergia;
        int cuentaAlarmaRed;
        int indiceRed = 0;
        int retardo;
        long valorCapacidadUds;
        long tamanoUd;

        const string version = "Aracne 3.0.1";

        Acciones clAcciones = new Acciones();
        CPU cpuCarga;
        CPU cpuMon;
        RAM ramCarga;
        RAM ramMon;
        Red red;
        TaskbarManager barraTareas = TaskbarManager.Instance;
        Temp tempMon;
        Temp tempCarga;
        DriveDetector driveDetector;

        Direccion dirRed;
        Magnitud udRed;
        DriveType tipoUd = 0;
        Estado estadoProgramador;

        //PRUEBAS: Utilizada por switchButtonTestEmulacion y ejecutaAccion
        bool emulacion = true;
        bool deshabilitaciones = true;
        #endregion

        #region " Carga y salida "

        private void Form1_Load(object sender, EventArgs e)
        {
            setTabRibbon();
            setComboInterfacesRed();
            setJumpList();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            guardaSettings();
        }

        #endregion

        #region " Funciones Comunes "

        DialogResult MsgBox(string sentencia, string titulo = "", MessageBoxIcon icono = MessageBoxIcon.None, MessageBoxButtons botones = MessageBoxButtons.OK)
        {
            MessageBoxEx.EnableGlass = activarAero;
            return MessageBoxEx.Show(this, sentencia, titulo, botones, icono, MessageBoxDefaultButton.Button1, siempreEncima);

        }

        DialogResult dlgAplicableDespuesConReinicioApp()
        {
            if (MsgBox("Esta opción será aplicable después de reiniciar la aplicación\n\n ¿Desea reiniciar la aplicación ahora?",
                version, MessageBoxIcon.Question, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                reinicioAplicacion(!Temp.sinPermisos);
                return DialogResult.Yes;
            }

            return DialogResult.No;
        }

        DialogResult dlgSinPermisosTemperaturaConReinicioApp()
        {
            if (MsgBox("Para poder monitorizar la temperatura de la CPU se necesitan permisos administrativos.\nActualmente no " +
                "se dispone de estos permisos.\n\n¿Desea reiniciar la aplicación como administrador?", "LECTURA DE TEMPERATURA",
                MessageBoxIcon.Exclamation, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                reinicioAplicacion(true);
                return DialogResult.Yes;
            }

            return DialogResult.No;
        }

        string setSegundos(int segundos)
        {
            int hor, min, seg, sig;
            // Math.Abs??
            sig = Math.Sign(segundos);
            hor = segundos / 3600 * sig;
            min = (segundos % 3600) / 60 * sig;
            seg = segundos % 3600 % 60 * sig;

            return (segundos < 0 ? "- " : string.Empty) + string.Format("{0:00}:{1:00}:{2:00}", hor, min, seg);
        }

        bool checkPermisosAdministrador()
        {
            return new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent()).IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        bool checkParam(string texto, string parametrosAceptados = "arshbcev", byte longitud = 2)
        {
            if (texto.Length != longitud)
            {
                return false;
            }

            var rgx = new Regex(@"^[-/][" + parametrosAceptados + "]", RegexOptions.IgnoreCase);

            return rgx.IsMatch(texto);
        }

        string quitaPrefijo(string parametro)
        {
            Regex rgx = new Regex(@"[-/]");

            return rgx.Replace(parametro, string.Empty).ToLower();
        }

        void setPaleta()
        {
            switch (paleta)
            {
                case eStyle.Metro:
                    styleManager1.ManagerStyle = eStyle.Metro;
                    break;

                case eStyle.Office2007Black:
                    styleManager1.ManagerStyle = eStyle.Office2007Black;
                    break;

                case eStyle.Office2007Blue:
                    styleManager1.ManagerStyle = eStyle.Office2007Blue;
                    break;

                case eStyle.Office2007Silver:
                    styleManager1.ManagerStyle = eStyle.Office2007Silver;
                    break;

                case eStyle.Office2007VistaGlass:
                    styleManager1.ManagerStyle = eStyle.Office2007VistaGlass;
                    break;

                case eStyle.Office2010Black:
                    styleManager1.ManagerStyle = eStyle.Office2010Black;
                    break;

                case eStyle.Office2010Blue:
                    styleManager1.ManagerStyle = eStyle.Office2010Blue;
                    break;

                case eStyle.Office2010Silver:
                    styleManager1.ManagerStyle = eStyle.Office2010Silver;
                    break;

                case eStyle.VisualStudio2010Blue:
                    styleManager1.ManagerStyle = eStyle.VisualStudio2010Blue;
                    break;

                case eStyle.VisualStudio2012Dark:
                    styleManager1.ManagerStyle = eStyle.VisualStudio2012Dark;
                    break;

                case eStyle.VisualStudio2012Light:
                    styleManager1.ManagerStyle = eStyle.VisualStudio2012Light;
                    break;

                case eStyle.Windows7Blue:
                    styleManager1.ManagerStyle = eStyle.Windows7Blue;
                    break;
            }

        }

        void setEstiloTabControl()
        {
            switch (estiloTabControl)
            {
                case eSuperTabStyle.Office2007:
                    tabControlPpal.TabStyle = eSuperTabStyle.Office2007;
                    break;
                
                case eSuperTabStyle.Office2010BackstageBlue:
                    tabControlPpal.TabStyle = eSuperTabStyle.Office2010BackstageBlue;
                    break;
                
                case eSuperTabStyle.OneNote2007:
                    tabControlPpal.TabStyle = eSuperTabStyle.OneNote2007;
                    break;
                
                case eSuperTabStyle.VisualStudio2008Dock:
                    tabControlPpal.TabStyle = eSuperTabStyle.VisualStudio2008Dock;
                    break;
                
                case eSuperTabStyle.VisualStudio2008Document:
                    tabControlPpal.TabStyle = eSuperTabStyle.VisualStudio2008Document;
                    break;
                
                case eSuperTabStyle.WinMediaPlayer12:
                    tabControlPpal.TabStyle = eSuperTabStyle.WinMediaPlayer12;
                    break;
            }
        }

        void setRadioAccion()
        {
            switch (radioAccion)
            {
                case tipoAccion.Apagar:
                    radioApagar.Checked = true;
                    break;

                case tipoAccion.Reiniciar:
                    radioReiniciar.Checked = true;
                    break;

                case tipoAccion.CerrarSesion:
                    radioCerrarSesion.Checked = true;
                    break;

                case tipoAccion.Suspender:
                    radioSuspender.Checked = true;
                    break;

                case tipoAccion.Hibernar:
                    radioHibernar.Checked = true;
                    break;

                case tipoAccion.Bloquear:
                    radioBloquear.Checked = true;
                    break;

                case tipoAccion.Avisar:
                    radioAvisar.Checked = true;
                    break;

                case tipoAccion.EnviarMail:
                    radioEnviarMail.Checked = true;
                    break;

                case tipoAccion.Ejecutar:
                    radioEjecutar.Checked = true;
                    break;

                case tipoAccion.ReiniciarAppsRegistradas:
                    radioReiniciar.Checked = true;
                    break;
            }
        }

        void setRadioCarga()
        {
            switch (carga)
            {
                case opcionCarga.RAMPorcentual:
                    radioRAMPorcentual.Checked = true;
                    break;

                case opcionCarga.RAMDatos:
                    radioRAMDatos.Checked = true;
                    break;

                case opcionCarga.CPU:
                    radioCPU.Checked = true;
                    break;

                case opcionCarga.Temperatura:
                    radioTemperatura.Checked = true;
                    break;
            }
        }

        void setRadioEnergia()
        {
            switch (energia)
            {
                case opcionEnergia.Alimentacion:
                    radioAlimentacion.Checked = true;
                    break;

                case opcionEnergia.PlanEnergia:
                    radioPlanEnergia.Checked = true;
                    break;

                case opcionEnergia.Bateria:
                    radioBateria.Checked = true;
                    break;
            }
        }

        void setComboAlimentacion()
        {
            switch (alimentacion)
            {
                case PowerSource.AC:
                    comboBoxAlimentacionTipo.SelectedIndex = 0;
                    break;
                case PowerSource.Battery:
                    comboBoxAlimentacionTipo.SelectedIndex = 1;
                    break;
                case PowerSource.Ups:
                    comboBoxAlimentacionTipo.SelectedIndex = 2;
                    break;
                default:
                    break;
            }
        }

        void setComboPlanEnergia()
        {
            switch (planEnergia)
            {
                case PowerPersonality.HighPerformance:
                    comboBoxPlanEnergiaTipo.SelectedIndex = 0;
                    break;

                case PowerPersonality.PowerSaver:
                    comboBoxPlanEnergiaTipo.SelectedIndex = 1;
                    break;

                case PowerPersonality.Automatic:
                    comboBoxPlanEnergiaTipo.SelectedIndex = 2;
                    break;

                default:
                    comboBoxPlanEnergiaTipo.SelectedIndex = 0;
                    break;
            }
        }

        void setPosRadioSecBat()
        {
            if (posRadioSecBat)
            {
                radioSecBatTiempoRestante.Checked = true;
                return;
            }

            radioSecBatVidaUtilPorcent.Checked = true;
        }

        void setPosRadioRed()
        {
            if (posRadioRed)
            {
                radioRedVelocidadRed.Checked = true;
                return;
            }

            radioRedSeAcabeRed.Checked = true;
        }

        void setCombo1Uds()
        {
            switch (UdsxSegRed)
            {
                case Magnitud.Bytes:
                    comboBox1UDsegRed.SelectedIndex = 2;
                    break;

                case Magnitud.KB:
                    comboBox1UDsegRed.SelectedIndex = 1;
                    break;

                case Magnitud.MB:
                    comboBox1UDsegRed.SelectedIndex = 0;
                    break;
            }
        }

        void setCombo2UdsxSeg()
        {
            switch (Uds2Red)
            {
                case Magnitud.KB:
                    comboBox2UDRed.SelectedIndex = 2;
                    break;

                case Magnitud.MB:
                    comboBox2UDRed.SelectedIndex = 1;
                    break;

                case Magnitud.GB:
                    comboBox2UDRed.SelectedIndex = 0;
                    break;
            }
        }

        void setTabPpal()
        {
            switch (condicion)
            {
                case tipoCondicion.A1Hora:
                    tabControlPpal.SelectedTabIndex = 0;
                    break;

                case tipoCondicion.CuentaAtras:
                    tabControlPpal.SelectedTabIndex = 1;
                    break;

                case tipoCondicion.CargaEquipo:
                    tabControlPpal.SelectedTabIndex = 2;
                    break;

                case tipoCondicion.Energia:
                    tabControlPpal.SelectedTabIndex = 3;
                    break;

                case tipoCondicion.EventosRed:
                    tabControlPpal.SelectedTabIndex = 4;
                    break;

                case tipoCondicion.Unidades:
                    tabControlPpal.SelectedTabIndex = 5;
                    break;

                default:
                    tabControlPpal.SelectedTabIndex = 0;
                    break;
            }
        }

        void setTemp1(bool onoff)
        {
            if (onoff)
            {
                timerTempor.Start();
                circularProgressPpal.Maximum = retardo;
                circularProgressPpal.Value = 0;
                circularProgressPpal.ProgressTextVisible = true;
                //barraTareas.SetProgressState(TaskbarProgressBarState.Normal);
                return;
            }

            timerTempor.Stop();
            timerTempor.Enabled = false;
            circularProgressPpal.Maximum = 100;
            circularProgressPpal.Value = 100;
            circularProgressPpal.ProgressTextVisible = false;
            barraTareas.SetProgressValue(0, 1);
            barraTareas.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        void setPosRibbonTab()
        {
            /* ARREGLAR: setPosRibbonTab() "Seteo del byte en Setting" 
               Se setea la posición al cierre del form en vez de en SelectedRibbonTabChanged porque desde InitializeComponent() 
               se fija la posición del tab, desencadenando ese evento y NO VEO otra solución. 
               Sé que la posición en Form1.Designer.cs se determina según la última posición fijada en el Diseño, pero 
               NO SÉ CÓMO LO HACE. También sé que se produce algún tipo de cambio en el fichero de recursos Form1.resx, pero no
               sé cuáles  */


            if (ribbonControl.SelectedRibbonTabItem == ribbonTabItemPrincipal)
            {
                posRibbonTab = tabRibbon.Principal;
            }
            else if (ribbonControl.SelectedRibbonTabItem == ribbonTabItemInmediato)
            {
                posRibbonTab = tabRibbon.Inmediato;
            }
            else if (ribbonControl.SelectedRibbonTabItem == ribbonTabItemVer)
            {
                posRibbonTab = tabRibbon.Ver;
            }
            else if (ribbonControl.SelectedRibbonTabItem == ribbonTabItemApariencia)
            {
                posRibbonTab = tabRibbon.Apariencia;
            }
            else if (ribbonControl.SelectedRibbonTabItem == ribbonTabItemConfigGral)
            {
                posRibbonTab = tabRibbon.Configuracion;
            }

        }

        void setTabRibbon()
        {
            // Seteado en Form_Load en de en CargaSettings() para hacerlo después de InitializeComponent(). 
            // LEE EL COMENTARIO setPosRibbonTab()

            switch (posRibbonTab)
            {
                case tabRibbon.Principal:
                    ribbonControl.SelectedRibbonTabItem = ribbonTabItemPrincipal;
                    break;

                case tabRibbon.Inmediato:
                    ribbonControl.SelectedRibbonTabItem = ribbonTabItemInmediato;
                    break;

                case tabRibbon.Ver:
                    ribbonControl.SelectedRibbonTabItem = ribbonTabItemVer;
                    break;

                case tabRibbon.Apariencia:
                    ribbonControl.SelectedRibbonTabItem = ribbonTabItemApariencia;
                    break;

                case tabRibbon.Configuracion:
                    ribbonControl.SelectedRibbonTabItem = ribbonTabItemConfigGral;
                    break;
            }
        }

        void setIconosOverlay(Icon icono, string identificador)
        {
            if (verOverlayIcons)
            {
                barraTareas.SetOverlayIcon(icono, identificador);
            }
        }

        void setJumpList()
        {
            try
            {
                if (!jumpListON)
                {
                    string dirApp = Application.StartupPath;
                    string dirInmediatoRc = dirApp + @"\InmediatoRc.dll";

                    var jumpList = JumpList.CreateJumpListForIndividualWindow(barraTareas.ApplicationId, Handle);
                    var catInmediato = new JumpListCustomCategory("Inmediato");

                    var JumpListLinkApagar = new JumpListLink(dirApp + @"\Acinme.exe", "Apagar");
                    JumpListLinkApagar.Arguments = "*a";
                    JumpListLinkApagar.IconReference = new IconReference(dirInmediatoRc, 0);

                    var JumpListLinkReiniciar = new JumpListLink(dirApp + @"\Acinme.exe", "Reiniciar");
                    JumpListLinkReiniciar.Arguments = "*r";
                    JumpListLinkReiniciar.IconReference = new IconReference(dirInmediatoRc, 1);

                    var JumpListLinkSuspender = new JumpListLink(dirApp + @"\Acinme.exe", "Suspender");
                    JumpListLinkSuspender.Arguments = "*s";
                    JumpListLinkSuspender.IconReference = new IconReference(dirInmediatoRc, 2);

                    var JumpListLinkHibernar = new JumpListLink(dirApp + @"\Acinme.exe", "Hibernar");
                    JumpListLinkHibernar.Arguments = "*h";
                    JumpListLinkHibernar.IconReference = new IconReference(dirInmediatoRc, 3);

                    var JumpListLinkBloquear = new JumpListLink(dirApp + @"\Acinme.exe", "Bloquear Pantalla");
                    JumpListLinkBloquear.Arguments = "*b";
                    JumpListLinkBloquear.IconReference = new IconReference(dirInmediatoRc, 5);

                    var JumpListLinkCerrarSesion = new JumpListLink(dirApp + @"\Acinme.exe", "Cerrar Sesión");
                    JumpListLinkCerrarSesion.Arguments = "*c";
                    JumpListLinkCerrarSesion.IconReference = new IconReference(dirInmediatoRc, 6); //OBSERVACIONES: 6=Azul, 4=Rojo

                    catInmediato.AddJumpListItems(JumpListLinkApagar, JumpListLinkReiniciar, JumpListLinkSuspender,
                        JumpListLinkHibernar, JumpListLinkCerrarSesion, JumpListLinkBloquear);

                    jumpList.AddCustomCategories(catInmediato);
                    jumpList.Refresh();

                    jumpListON = true;
                }
            }

            catch (Exception)
            {
                jumpListON = false;
            }
        }

        void setComboInterfacesRed()
        {
            iniciando = true;
            //PTE: Habilitar desde configuración la posibilidad de filtrar o no las redes poco usuales (la bool del constructor)
            comboBoxAdaptadoresRed.DataSource = new BindingSource(new ListRed(false).DicAdaptadores, null);
            comboBoxAdaptadoresRed.DisplayMember = "Value";
            comboBoxAdaptadoresRed.ValueMember = "Key";
            comboBoxAdaptadoresRed.SelectedIndex = posComboAdaptador <= comboBoxAdaptadoresRed.Items.Count ? posComboAdaptador : 0;

            indiceRed = ListRed.GetIndiceRed(comboBoxAdaptadoresRed);

            iniciando = false;
        }

        void setComboUds()
        {
            switch (posComboUds)
            {
                case 0:
                    comboBoxListUds.SelectedIndex = 0;
                    break;
                case 1:
                    comboBoxListUds.SelectedIndex = 1;
                    break;
                case 2:
                    comboBoxListUds.SelectedIndex = 2;
                    break;
                case 3:
                    comboBoxListUds.SelectedIndex = 3;
                    break;
                case 4:
                    comboBoxListUds.SelectedIndex = 4;
                    break;
                case 5:
                    comboBoxListUds.SelectedIndex = 5;
                    break;
                case 6:
                    comboBoxListUds.SelectedIndex = 6;
                    break;
                case 7:
                    comboBoxListUds.SelectedIndex = 7;
                    break;
                case 8:
                    comboBoxListUds.SelectedIndex = 8;
                    break;
                case 9:
                    comboBoxListUds.SelectedIndex = 9;
                    break;
                case 10:
                    comboBoxListUds.SelectedIndex = 10;
                    break;
                case 11:
                    comboBoxListUds.SelectedIndex = 11;
                    break;
                case 12:
                    comboBoxListUds.SelectedIndex = 12;
                    break;
                case 13:
                    comboBoxListUds.SelectedIndex = 13;
                    break;
                case 14:
                    comboBoxListUds.SelectedIndex = 14;
                    break;
                case 15:
                    comboBoxListUds.SelectedIndex = 15;
                    break;
                case 16:
                    comboBoxListUds.SelectedIndex = 16;
                    break;
                case 17:
                    comboBoxListUds.SelectedIndex = 17;
                    break;
                case 18:
                    comboBoxListUds.SelectedIndex = 18;
                    break;
                case 19:
                    comboBoxListUds.SelectedIndex = 19;
                    break;
                case 20:
                    comboBoxListUds.SelectedIndex = 20;
                    break;
                case 21:
                    comboBoxListUds.SelectedIndex = 21;
                    break;
                case 22:
                    comboBoxListUds.SelectedIndex = 22;
                    break;
                case 23:
                    comboBoxListUds.SelectedIndex = 23;
                    break;
                case 24:
                    comboBoxListUds.SelectedIndex = 24;
                    break;
                case 25:
                    comboBoxListUds.SelectedIndex = 25;
                    break;
                case 26:
                    comboBoxListUds.SelectedIndex = 26;
                    break;
            }
        }

        void setComboTipoUd()
        {
            switch (posComboTipoUd)
            {
                case 0:
                    comboBoxTipoUds.SelectedIndex = 0;
                    break;

                case 1:
                    comboBoxTipoUds.SelectedIndex = 1;
                    break;

                case 2:
                    comboBoxTipoUds.SelectedIndex = 2;
                    break;

                case 3:
                    comboBoxTipoUds.SelectedIndex = 3;
                    break;

                case 4:
                    comboBoxTipoUds.SelectedIndex = 4;
                    break;

                case 5:
                    comboBoxTipoUds.SelectedIndex = 5;
                    break;
            }
        }

        void setComboMagnitudUds()
        {
            switch (MagnitudUd)
            {
                case Magnitud.KB:
                    comboBoxMagnitudUds.SelectedIndex = 0;
                    break;

                case Magnitud.MB:
                    comboBoxMagnitudUds.SelectedIndex = 1;
                    break;

                case Magnitud.GB:
                    comboBoxMagnitudUds.SelectedIndex = 2;
                    break;

                case Magnitud.TB:
                    comboBoxMagnitudUds.SelectedIndex = 3;
                    break;
            }
        }

        void setTimeSelectorACero(DevComponents.Editors.DateTimeAdv.TimeSelector timeSelector)
        {
            if (timeSelector.SelectedTime == TimeSpan.Zero)
            {
                timeSelector.SelectedTime = TimeSpan.Zero.Add(TimeSpan.FromSeconds(1));
            }
        }

        void reinicioAplicacion(bool admin)
        {
            if (admin)
            {
                Acciones.EjecutarPrograma(Application.ExecutablePath, true);  //REGISTRO: Se debe controlar la excepción Win32Exception
            }

            else
            {
                Acciones.EjecutarPrograma(Application.ExecutablePath, false);
            }

            guardaSettings();
            Close();
            Application.Exit();
        }

        void guardaSettings()
        {
            setPosRibbonTab();
            Properties.Settings.Default.Save();
        }

        void preEjecutaAccion()
        {
            if (retardarAcciones)
            {
                timerRetardo.Interval = integerInputRetardoAcciones.Value * 1000;
                timerRetardo.Start();

                notifyIcon1.BalloonTipText = "La acción: " + radioAccion + " retardada " + integerInputRetardoAcciones.Value + " seg.";
                notifyIcon1.ShowBalloonTip(1000);

                return;
            }

            ejecutaAccion();
        }

        void ejecutaAccion()
        {
            //PRUEBAS: El siguiente bloque y simulacion están para la realización de pruebas; se puede prescindir de ambos.
            bool simulacion = emulacion; //PRUEBAS: emulación declarada al inicio SI NO TE ACUERDAS LEE COMENTARIOS EN LA DECLARACIÓN.

            barraTareas.SetProgressValue(0, 1);
            barraTareas.SetProgressState(TaskbarProgressBarState.Indeterminate);
            setBotones(estadoProgramador = Estado.Parado);

            if (simulacion)
            {
                switch (radioAccion)
                {
                    case tipoAccion.Hibernar:
                        MsgBox("Hibernado!!!", "PRUEBAS: SIMULACIÓN", MessageBoxIcon.Exclamation);
                        break;
                    case tipoAccion.Suspender:
                        MsgBox("Suspendido!!!", "PRUEBAS: SIMULACIÓN", MessageBoxIcon.Exclamation);
                        break;
                    case tipoAccion.Bloquear:
                        MsgBox("Bloqueado!!!", "PRUEBAS: SIMULACIÓN", MessageBoxIcon.Exclamation);
                        break;
                    case tipoAccion.Avisar:
                        MsgBox("Estás Avisado!!!", "PRUEBAS: SIMULACIÓN", MessageBoxIcon.Exclamation);
                        break;
                    case tipoAccion.Ejecutar:
                        MsgBox("Ejecución!!!", "PRUEBAS: SIMULACIÓN", MessageBoxIcon.Exclamation);
                        break;
                    case tipoAccion.EnviarMail:
                        MsgBox("Mail enviado!!!", "PRUEBAS: SIMULACIÓN", MessageBoxIcon.Exclamation);
                        break;
                    default:
                        MsgBox("Apagado, Reiniciado o Sesión Cerrada", "PRUEBAS: SIMULACIÓN SHUTDOWN", MessageBoxIcon.Exclamation);
                        break;
                }

                barraTareas.SetProgressState(TaskbarProgressBarState.NoProgress);
                setIconosOverlay(new StockIcon((StockIconIdentifier)115).Icon, string.Empty);
                return;
            }
            switch (radioAccion)
            {
                /* case tipoAccion.Apagar:
                    break;
                case tipoAccion.Reiniciar:
                    break;
                case tipoAccion.CerrarSesion:
                    break; // COMO COMPARTEN INSTRUCCIÓN INCLUIDO EN DEFAULT:  clAcciones.Shutdown(radioAccion) */

                case tipoAccion.Hibernar:
                    clAcciones.Hibernar();
                    break; // PUEDE APLICARSE IDEM ANTERIOR.

                case tipoAccion.Suspender:
                    clAcciones.Suspender();
                    break;

                case tipoAccion.Bloquear:
                    clAcciones.Bloquear();
                    break;

                case tipoAccion.Avisar:
                    //NOTA: En vez de llamar al ejecutable con parámetros en versiones > FW 3.5 los .exe pueden tener el 
                    //      mismo TTo. que las dll. Por tanto, como está añadido como ref. se puede instanciar el FormAvisos
                    //      instanciándolo. OBSERVACIONES: Habría que modificar el constructor
                    var psi = new ProcessStartInfo(Application.StartupPath + @"\AvisosRep.exe");
                    psi.Arguments = "* " + siempreEncima.ToString() + " " + activarAero.ToString() + " " +
                    ((int)paleta).ToString();

                    Process.Start(psi);
                    break;

                case tipoAccion.Ejecutar:

                    new Acciones().EjecutarProceso(archivoSeleccionado);
                    break;

                case tipoAccion.EnviarMail:

                    new Mail(true).EnviarMail();
                    break;

                default:
                    clAcciones.Shutdown(radioAccion);
                    break;
            }

            barraTareas.SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        void condicionBateria()
        {
            //PRUEBAS:
            richTextBoxDepuracion.Text = "%Bateria:\t" + PowerManager.BatteryLifePercent + "\n" +
                                      "tEstimado\t" + PowerManager.GetCurrentBatteryState().EstimatedTimeRemaining.ToString("hh\\:mm\\:ss");

            if (radioSecBatVidaUtilPorcent.Checked)
            {
                if ((esMasComboBateria && PowerManager.BatteryLifePercent > porcentajeRestanteBateria) ||
                    (!esMasComboBateria && PowerManager.BatteryLifePercent < porcentajeRestanteBateria))
                {

                    vidaBateriaEnabled = false;
                    preEjecutaAccion();
                }
                /*else if (!esMasComboBateria && PowerManager.BatteryLifePercent < porcentajeRestanteBateria)
                {
                    vidaBateriaEnabled = false;
                    ejecutaAccion();
                }//*/
            }
            else
            {
                TimeSpan tFab = PowerManager.GetCurrentBatteryState().EstimatedTimeRemaining;
                TimeSpan tBat = new TimeSpan(0, 0, (int)tFab.Subtract(new TimeSpan(tFab.Days, 0, 0, 0)).TotalSeconds);

                if ((esMasComboBateria && tBat > tiempoRestanteBateria) || (!esMasComboBateria && tBat <= tiempoRestanteBateria))
                {
                    vidaBateriaEnabled = false;
                    preEjecutaAccion();
                }
                /*else if (!esMasComboBateria && tBat <= tiempoRestanteBateria)
                {
                    vidaBateriaEnabled = false;
                    ejecutaAccion();
                }//*/
            }
        }

        void condicionAliminentacion()
        {
            if (!checkBoxDuranteAlimentacion.Checked)
            {
                if (alimentacion == PowerManager.PowerSource)
                {
                    alimentacionEnabled = false;
                    preEjecutaAccion();
                }

                return;
            }

            timerAlimentacion.Start();
        }

        void condicionPlanEnergia()
        {
            if (planEnergia == PowerManager.PowerPersonality)
            {
                planEnergiaEnabled = false;
                preEjecutaAccion();
            }
        }

        bool iniciaParametros(string[] argumentos)
        {
            if (argumentos.Length == 0)
            {
                //richTextBoxInfoSec.Text = "No hay parámetros";
                Console.WriteLine("No hay parámetros");
                return false;
            }
            if (!checkParam(argumentos[0]) || !checkParam(argumentos[1], "tix") || (quitaPrefijo(argumentos[1]) != "t" & argumentos.Length > 2))
            //PTE: Arriba falta añadir: Ayuda y length != 1, length > 3 en cualquier caso,
            {
                //richTextBoxInfoSec.Text = "Parámetros no válidos";
                //PTE: "Refinar" un poco devolviendo qué es lo que está mal. 
                //NOTA: Console.WriteLine no retorna nada a la consola.
                Console.WriteLine("La sintaxis utilizada no es válida. No se reconocen alguno de los parámetros");
                return false;
            }
            return true;
        }
        /// <summary>
        /// Función creada para procesar parámetros. Está incompleta. Sólo están operativos los argunmentos inmediatos ("i" como
        /// 2º argumento) 
        /// </summary>
        /// <param name="argumentos"></param>
        /// <returns></returns>
        bool procesaParametros(string[] argumentos)
        {
            if (argumentos.Length == 0)
            {
                //richTextBoxInfoSec.Text = "No hay parámetros";
                Console.WriteLine("No hay parámetros");
                return false;
            }
            /*if (!checkParam(argumentos[0]) || !checkParam(argumentos[1], "tix") || (quitaPrefijo(argumentos[1]) != "t" & argumentos.Length > 2))
            //PTE: Arriba falta añadir: Ayuda y length != 1, length > 3 en cualquier caso,
            {
                //richTextBoxInfoSec.Text = "Parámetros no válidos";
                //PTE: "Refinar" un poco devolviendo qué es lo que está mal. 
                //NOTA: Console.WriteLine no retorna nada a la consola.
                Console.WriteLine("La sintaxis utilizada no es válida. No se reconocen alguno de los parámetros");
                return false;
            }*/
            //PTE: Eliminar la parte "inmediata" de quitaPrefijo(argumentos[0]) == arsh...
            //NOTA: Queda operativo para las JumpList de WACP
            //NOTA: La 1ª parte (los 2 1ºs if) deberían ir en una bool y el resto (los 2 bloques if/ if else) en una void 
            //      independiente. Los 2 en el constructor: la bool que controle si los parámetros son válidos al principio (es
            //      posible que desde ahí, antes de ejecutar el form, sí lleguen los mensajes a la consola. El void después de cargasettings() 


            if (argumentos[0] == "/MINIMIZADO")
            {

                WindowState = FormWindowState.Minimized;
                richTextBoxDepuracion.Text = "Inicio minimizado";
                return true;
            }

            if (quitaPrefijo(argumentos[0]) == "a")
            {
                if (quitaPrefijo(argumentos[1]) == "i")
                {
                    clAcciones.Shutdown(tipoAccion.Apagar);
                }

                radioApagar.Checked = true;
                richTextBoxDepuracion.Text = "Apagar";
            }
            else if (quitaPrefijo(argumentos[0]) == "r")
            {
                if (quitaPrefijo(argumentos[1]) == "i")
                {
                    clAcciones.Shutdown(tipoAccion.Reiniciar);
                }

                radioReiniciar.Checked = true;
                richTextBoxDepuracion.Text = "Reiniciar";
            }
            else if (quitaPrefijo(argumentos[0]) == "s")
            {
                if (quitaPrefijo(argumentos[1]) == "i")
                {
                    clAcciones.Suspender();
                }

                radioSuspender.Checked = true;
                richTextBoxDepuracion.Text = "Suspender";
            }
            else if (quitaPrefijo(argumentos[0]) == "h")
            {
                if (quitaPrefijo(argumentos[1]) == "i")
                {
                    clAcciones.Hibernar();
                }

                radioHibernar.Checked = true;
                richTextBoxDepuracion.Text = "Hibernar";
            }
            else if (quitaPrefijo(argumentos[0]) == "b")
            {
                if (quitaPrefijo(argumentos[1]) == "i")
                {
                    clAcciones.Bloquear();
                }

                radioBloquear.Checked = true;
                richTextBoxDepuracion.Text = "Bloquear";
            }
            else if (quitaPrefijo(argumentos[0]) == "e")
            {
                radioEjecutar.Checked = true;
                richTextBoxDepuracion.Text = "Ejecutar";
            }
            else if (quitaPrefijo(argumentos[0]) == "v")
            {
                radioAvisar.Checked = true;
                richTextBoxDepuracion.Text = "Avisar";
            }
            else if (quitaPrefijo(argumentos[0]) == "?")
            {
                richTextBoxDepuracion.Text = "Ayuda";
            }
            //PRUEBAS:
            if (quitaPrefijo(argumentos[1]) == "i")
            {
                Application.Exit();
                return true;
            }

            if (quitaPrefijo(argumentos[1]) == "t")
            {
                richTextBoxDepuracion.Text += " Temporizado";
            }
            else if (quitaPrefijo(argumentos[1]) == "c")
            {
                richTextBoxDepuracion.Text += " Cronometrado";
            }
            //OBSERVACIONES: SE PODRÍA HACER ASÍ CON TODO: CARGAS POR RAM, TEMP, ENERGÍA, ETC
            else if (quitaPrefijo(argumentos[1]) == "x")
            {
                richTextBoxDepuracion.Text = "Cancelar";
            }
            return true;
        }

        void setEventosUd(bool desactivacion = false)
        {
            if (desactivacion)
            {
                driveDetector.DeviceArrived -= new DriveDetectorEventHandler(Unidad_Arrived);
                driveDetector.DeviceRemoved -= new DriveDetectorEventHandler(Unidad_Removed);

                return;
            }

            driveDetector.DeviceArrived += new DriveDetectorEventHandler(Unidad_Arrived);
            driveDetector.DeviceRemoved += new DriveDetectorEventHandler(Unidad_Removed);
        }

        void setValorCapacidadUds()
        {
            switch (MagnitudUd)
            {
                case Magnitud.KB:
                    valorCapacidadUds = integerInputUds.Value * 1024;
                    break;
                case Magnitud.MB:
                    valorCapacidadUds = integerInputUds.Value * (long)Math.Pow(1024, 2);
                    break;
                case Magnitud.GB:
                    valorCapacidadUds = integerInputUds.Value * (long)Math.Pow(1024, 3);
                    break;
                case Magnitud.TB:
                    valorCapacidadUds = integerInputUds.Value * (long)Math.Pow(1024, 4);
                    break;
            }
        }

        bool checkTipoUd(int indice)
        {
            if (indice == 0 && tipoUd == DriveType.Removable)
            {
                return true;
            }
            else if (indice == 1 && tipoUd == DriveType.CDRom)
            {
                return true;
            }
            else if (indice == 2 && tipoUd == DriveType.Fixed)
            {
                return true;
            }
            else if (indice == 3 && tipoUd == DriveType.Network)
            {
                return true;
            }
            else if (indice == 4 && tipoUd == DriveType.Ram)
            {
                return true;
            }
            else if (indice == 5 && (tipoUd == DriveType.Unknown || tipoUd == DriveType.NoRootDirectory))
            {
                return true;
            }

            return false;
        }

        void setInfoUnidad()
        {
            DriveInfo[] Uds = DriveInfo.GetDrives();

            foreach (DriveInfo ud in Uds)
            {
                if (ud.Name == comboBoxListUds.SelectedItem.ToString())
                {
                    tipoUd = ud.DriveType;
                    tamanoUd = ud.TotalSize;

                    break;
                }
            }
        }

        void resaltaRich(string texto)
        {
            var sb = new System.Text.StringBuilder();

            sb.Append(@"{\rtf\ansi");
            sb.Append(@"{\colortbl ;\red255\green0\blue0;\red0\green176\blue80;}");
            sb.Append(texto);
            sb.Append(@"}");

            //NOTAS: /cf0 = Negro, Cf1 = Rojo, cf2 = verde, b-b0 negrita, i-i0 

            richTextBoxDepuracion.Rtf = sb.ToString();
        }

        void setEstadoNotifyIcon()
        {
            string info = "Estado: " + estadoProgramador + "\nAcción: " + radioAccion;

            info += (estadoProgramador == Estado.Iniciado) ? "\nInfo: " + labelData.Text : string.Empty;

            notifyIcon1.BalloonTipText = info;
            notifyIcon1.ShowBalloonTip(1000);
        }

        /// <summary>
        /// Subrutina creada para incluir una entrada en el registro de Windows para que el programa se inicie con éste.
        /// </summary>
        /// <param name="entrada">Indica si se incluye o se excluye la entrada en el registro</param>
        /// <param name="currentUser">Indica si la clave se incluirá en CurrentUser o en LocalMachine</param>
        void subIniciarConWindows(bool entrada, bool currentUser = true)
        {
            // Depende de Microsoft.Win32;
            try
            {
                string directorio = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                RegistryKey clave = currentUser ?
                    Registry.CurrentUser.CreateSubKey(directorio, RegistryKeyPermissionCheck.ReadWriteSubTree) :
                    Registry.LocalMachine.CreateSubKey(directorio, RegistryKeyPermissionCheck.ReadWriteSubTree);

                string subclave = Application.ProductName;
                string valor = Application.ExecutablePath + (iniciarMinimizado ? " /MINIMIZADO" : string.Empty);

                clave.OpenSubKey(directorio, true);

                if (entrada == true)
                {
                    if (clave.GetValue(subclave) == null)
                    {
                        clave.SetValue(subclave, valor);
                        MsgBox(version + " se ejecutará al inicio de Windows", version, MessageBoxIcon.Information);
                        //PRUEBAS
                        //MessageBox.Show(valor, "Añadido con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else
                    {
                        MsgBox(subclave + " ya se iniciaba con Windows", version, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                else
                {
                    if (clave.GetValue(subclave) != null)
                    {
                        clave.DeleteValue(subclave);
                        MsgBox(version + " se no ejecutará al inicio de Windows", version, MessageBoxIcon.Information);
                        //PRUEBAS
                        //MessageBox.Show(valor, "Borrado con éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else
                    {
                        MsgBox(subclave + " no se puede borrar porque no se iniciaba con Windows", version,
                            MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion

        #region " Controles Ribbon "

        #region Inmediato

        private void buttonItemApagarInmediato_Click(object sender, EventArgs e)
        {
            try
            {
                clAcciones.Shutdown(tipoAccion.ApagarInmediato);
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void buttonItemReiniciarNormalInmediato_Click(object sender, EventArgs e)
        {
            try
            {
                clAcciones.Shutdown(tipoAccion.Reiniciar);
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void buttonItemReiniciarAppsRegistradasInmediato_Click(object sender, EventArgs e)
        {
            try
            {
                clAcciones.Shutdown(tipoAccion.ReiniciarAppsRegistradas);
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void buttonItemReiniciarMenuOpcionesInmediato_Click(object sender, EventArgs e)
        {
            try
            {
                clAcciones.Shutdown(tipoAccion.ReiniciarMenuOpciones);
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void buttonItemSuspenderInmediato_Click(object sender, EventArgs e)
        {
            try
            {
                clAcciones.Suspender();
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void buttonItemHibernarInmediato_Click(object sender, EventArgs e)
        {
            try
            {
                clAcciones.Hibernar();
                //clAcciones.Shutdown(tipoAccion.Hibernar); Opción B
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void buttonItemCerrarSesionInmediato_Click(object sender, EventArgs e)
        {
            try
            {
                clAcciones.Shutdown(tipoAccion.CerrarSesion);
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void buttonItemBloquearInmediato_Click(object sender, EventArgs e)
        {
            try
            {
                clAcciones.Bloquear();
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Ver

        private void switchButtonItemMonitorizacion_ValueChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                verMonitorizacion = switchButtonItemMonitorizacion.Value;
            }

            if (timerMon.Enabled && switchButtonItemMonitorizacion.Value == false) //Cuidado con el 1er parámetro
            {
                timerMon.Stop();

                labelMonCpu.Text = string.Empty;
                labelMonRamMB.Text = string.Empty;
                labelMonRamPC.Text = string.Empty;
                labelMonTemp.Text = string.Empty;

                return;
            }

            ramMon = new RAM();

            cpuMon = new CPU();
            cpuMon.InicioCounter();

            if (!Temp.sinPermisos)
            {
                tempMon = new Temp();
                tempMon.Iniciar();
            }

            else if (!iniciando && dlgSinPermisosTemperaturaConReinicioApp() == DialogResult.Yes)
            {

                reinicioAplicacion(true);
                return;
                //REGISTRO: Cuando se empiece con el control de excepciones, capturar Win32Exception (dlg UAC cancelado)
            }

            timerMon.Interval = 1000; //PTE: Crear propiedad para que sea personalizable desde menú Config.
            timerMon.Start();
        }

        private void switchButtonItemOverlayIcons_ValueChanged(object sender, EventArgs e)
        {
            verOverlayIcons = switchButtonItemOverlayIcons.Value;

            if (!verOverlayIcons)
            {
                barraTareas.SetOverlayIcon(new StockIcon((StockIconIdentifier)115).Icon, string.Empty);
            }
        }

        #endregion

        #region Apariencia

        private void itemOf07Blue_Click(object sender, EventArgs e)
        {
            paleta = eStyle.Office2007Blue;
            setPaleta();
        }

        private void itemOf07Silver_Click(object sender, EventArgs e)
        {
            paleta = eStyle.Office2007Silver;
            setPaleta();
        }

        private void itemOf07Black_Click(object sender, EventArgs e)
        {
            paleta = eStyle.Office2007Black;
            setPaleta();
        }

        private void itemOf07VistaGlass_Click(object sender, EventArgs e)
        {
            paleta = eStyle.Office2007VistaGlass;
            setPaleta();
        }

        private void itemOf10Blue_Click(object sender, EventArgs e)
        {
            paleta = eStyle.Office2010Blue;
            setPaleta();
        }

        private void itemOf10Silver_Click(object sender, EventArgs e)
        {
            paleta = eStyle.Office2010Silver;
            setPaleta();
        }

        private void itemOf10Black_Click(object sender, EventArgs e)
        {
            paleta = eStyle.Office2010Black;
            setPaleta();
        }

        private void itemW7Blue_Click(object sender, EventArgs e)
        {
            paleta = eStyle.Windows7Blue;
            setPaleta();
        }

        private void itemW8Metro_Click(object sender, EventArgs e)
        {
            paleta = eStyle.Metro;
            setPaleta();
        }

        private void itemVS10Blue_Click(object sender, EventArgs e)
        {
            paleta = eStyle.VisualStudio2010Blue;
            setPaleta();
        }

        private void itemVS12Light_Click(object sender, EventArgs e)
        {
            paleta = eStyle.VisualStudio2012Light;
            setPaleta();
        }

        private void itemVS12Dark_Click(object sender, EventArgs e)
        {
            paleta = eStyle.VisualStudio2012Dark;
            setPaleta();
        }

        private void switchButtonItemAero_ValueChanging(object sender, EventArgs e)
        {
            //PTE: ¿Aquí qué pasa?
            /*if (!iniciando && MsgBox("Esta opción requiere reiniciar.\n\n¿Desea continuar?", "Habilitar Aero", MessageBoxIcon.Asterisk,
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                activarAero = !switchButtonItemAero.Value;
                reinicioAplicacion(!Temp.sinPermisos);
                return;
            }*/

        }

        private void switchButtonItemAero_ValueChanged(object sender, EventArgs e)
        {
            activarAero = switchButtonItemAero.Value;
            //EnableGlass = activarAero;

            if (!iniciando && dlgAplicableDespuesConReinicioApp() != DialogResult.Yes)
            {
                string des = !activarAero ? "des" : string.Empty;
                MsgBox("Aero se " + des + "habilitará la próxima vez que inicie la aplicación", version, MessageBoxIcon.Information);
            }

        }

        private void switchButtonItemSiempreEncima_ValueChanged(object sender, EventArgs e)
        {
            siempreEncima = switchButtonItemSiempreEncima.Value;
            TopMost = siempreEncima;
        }

        private void itemCPBLineas_Click(object sender, EventArgs e)
        {
            circularProgressPpal.ProgressBarType = eCircularProgressType.Line;
        }

        private void itemCPBPuntos_Click(object sender, EventArgs e)
        {
            circularProgressPpal.ProgressBarType = eCircularProgressType.Dot;
        }

        private void itemCPBDonut_Click(object sender, EventArgs e)
        {
            circularProgressPpal.ProgressBarType = eCircularProgressType.Donut;
        }

        private void itemCPBRadios_Click(object sender, EventArgs e)
        {
            circularProgressPpal.ProgressBarType = eCircularProgressType.Spoke;
        }

        private void itemCPBTarta_Click(object sender, EventArgs e)
        {
            circularProgressPpal.ProgressBarType = eCircularProgressType.Pie;
        }

        private void itemTCOf07_Click(object sender, EventArgs e)
        {
            estiloTabControl = eSuperTabStyle.Office2007;
            setEstiloTabControl();
        }

        private void itemTCOf10BS_Click(object sender, EventArgs e)
        {
            estiloTabControl = eSuperTabStyle.Office2010BackstageBlue;
            setEstiloTabControl();
        }

        private void itemTCON07_Click(object sender, EventArgs e)
        {
            estiloTabControl = eSuperTabStyle.OneNote2007;
            setEstiloTabControl();
        }

        private void itemTCVS08Dock_Click(object sender, EventArgs e)
        {
            estiloTabControl = eSuperTabStyle.VisualStudio2008Dock;
            setEstiloTabControl();
        }

        private void itemTCVS08Document_Click(object sender, EventArgs e)
        {
            estiloTabControl = eSuperTabStyle.VisualStudio2008Document;
            setEstiloTabControl();
        }

        private void itemTCWMP_Click(object sender, EventArgs e)
        {
            estiloTabControl = eSuperTabStyle.WinMediaPlayer12;
            setEstiloTabControl();
        }

        #endregion

        #region Configuración

        private void checkBoxForzarCierre_Click(object sender, EventArgs e)
        {
            try
            {
                forzarCierre = checkBoxForzarCierre.Checked;
                clAcciones.ParametrosExtra = forzarCierre ? " -f" : string.Empty; // Suprimido operador += los comandos se repiten: Ej: shutdown -f -f
                // Ej: shutdown -f -f y string.Empty no vale para nada
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }                                                                     // ARREGLAR: BUSCAR OTRA SOLUCIÓN.

        private void checkBoxReinicioAppsReg_Click(object sender, EventArgs e)
        {
            try
            {
                reinicioAppsReg = checkBoxReinicioAppsReg.Checked;

                if (radioReiniciar.Checked)
                {
                    radioAccion = !reinicioAppsReg ? tipoAccion.Reiniciar : tipoAccion.ReiniciarAppsRegistradas;
                }
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void switchButtonTestEmulacion_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //TEST: Botón temporal de pruebas. Controla la bool emulación para el método ejecutaAccion
                emulacion = switchButtonTestEmulacion.Value;
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        #endregion

        #endregion

        #region " Controles Form "

        #region Acciones

        private void radioApagar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                radioAccion = tipoAccion.Apagar;
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void radioReiniciar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                radioAccion = !reinicioAppsReg ? tipoAccion.Reiniciar : tipoAccion.ReiniciarAppsRegistradas;
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void radioCerrarSesion_CheckedChanged(object sender, EventArgs e)
        {
            radioAccion = tipoAccion.CerrarSesion;
        }

        private void radioSuspender_CheckedChanged(object sender, EventArgs e)
        {
            radioAccion = tipoAccion.Suspender;
        }

        private void radioHibernar_CheckedChanged(object sender, EventArgs e)
        {
            radioAccion = tipoAccion.Hibernar;
        }

        private void radioBloquear_CheckedChanged(object sender, EventArgs e)
        {
            radioAccion = tipoAccion.Bloquear;
        }

        private void radioAvisar_CheckedChanged(object sender, EventArgs e)
        {
            radioAccion = tipoAccion.Avisar;
        }

        private void radioEnviarMail_CheckedChanged(object sender, EventArgs e)
        {
            if (!iniciando && radioEnviarMail.Checked)
            {
                if (!mailConfigCorrecta)
                {
                    if (MsgBox("Parece que no se pueden enviar mails.\n\n¿Desea abrir el diálogo de configuración?",
                        "Configuración de mail incorrecta", MessageBoxIcon.Error, MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        setRadioAccion();
                        return;
                    }

                    abreFormMail();
                }

                radioAccion = tipoAccion.EnviarMail;
            }
        }

        private void radioEjecutar_CheckedChanged(object sender, EventArgs e)
        {
            if (!iniciando && radioEjecutar.Checked)
            {
                if (!ejecucionConfigCorrecta)
                {
                    if (MsgBox("Parece que la configuración de las ejecuciones no es correcta.\n\n¿Desea abrir el diálogo " +
                        "de configuración?", "Configuración de ejecuciones incorrecta", MessageBoxIcon.Error, MessageBoxButtons.YesNo)
                        == DialogResult.No)
                    {

                        setRadioAccion();
                        return;
                    }

                    abreFormEjecuciones();
                }

                radioAccion = tipoAccion.Ejecutar;
            }
        }

        #endregion

        #region Tab A1Hora

        private void tabA1Hora_Click(object sender, EventArgs e)
        {
            //OBSERVACIONES: El evento click en el superTab NO produce "efecto eco", por tanto no es necesario encapsular.
            condicion = tipoCondicion.A1Hora;
        }

        private void timeSelectorA1Hora_SelectedTimeChanged(object sender, EventArgs e)
        {
            try
            {
                //PTE: Solucionar problema timeSelectors: "pulsar 'v' estando a 00:00"
                setTimeSelectorACero(timeSelectorA1Hora);
                horaA1Hora = dateTimeInputA1Hora.Value.Add(timeSelectorA1Hora.SelectedTime);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MsgBox("System.ArgumentOutOfRangeException capturado en timeSelectorA1Hora\n" + ex.Message);
            }
            catch (Exception ex)
            {
                MsgBox("excepción capturada en timeSelectorA1Hora\n" + ex.Message);
            }
        }

        private void botonAhoraTimeSelectorA1Hora_Click(object sender, EventArgs e)
        {
            timeSelectorA1Hora.SelectedTime = new TimeSpan(0, (int)DateTime.Now.TimeOfDay.TotalMinutes, 0);
            dateTimeInputA1Hora.Value = DateTime.Today;
        }

        private void dateTimeInputA1Hora_ValueChanged(object sender, EventArgs e)
        {
            setTimeSelectorACero(timeSelectorA1Hora);
            horaA1Hora = dateTimeInputA1Hora.Value.Add(timeSelectorA1Hora.SelectedTime);
        }

        private void dateTimeInputA1Hora_MonthCalendar_DateChanged(object sender, EventArgs e)
        {
            horaA1Hora = dateTimeInputA1Hora.Value.Add(timeSelectorA1Hora.SelectedTime);
        }

        #endregion

        #region Tab Temporización

        private void tabCuentaAtras_Click(object sender, EventArgs e)
        {
            condicion = tipoCondicion.CuentaAtras;
        }

        private void timeSelectorCuentaAtras_SelectedTimeChanged(object sender, EventArgs e)
        {
            setTimeSelectorACero(timeSelectorCuentaAtras);
            horaCuentaAtras = timeSelectorCuentaAtras.SelectedTime;
        }

        #endregion

        #region Tab Carga

        private void tabCargaEquipo_Click(object sender, EventArgs e)
        {
            condicion = tipoCondicion.CargaEquipo;
        }

        private void radioRAMPorcentual_CheckedChanged(object sender, EventArgs e)
        {
            carga = opcionCarga.RAMPorcentual;
            labelInfoCargas1.Text = "carga RAM";
            labelInfoCargas2.Text = "del";
            labelInfoCargas3.Text = "%";
            integerInputDatosCargaEquipo.MaxValue = 100;
            integerInputDatosCargaEquipo.Increment = 1;
        }

        private void radioRAMDatos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                carga = opcionCarga.RAMDatos;
                labelInfoCargas1.Text = "carga RAM";
                labelInfoCargas2.Text = "de";
                labelInfoCargas3.Text = "MB";

                integerInputDatosCargaEquipo.MaxValue = (int)ramMon.Consulta(RAM.InfoRequerida.Total, RAM.FormatoRetorno.MB);
                integerInputDatosCargaEquipo.Increment = 50;
            }

            catch (NullReferenceException)
            {
                // REGISTRO: Excepción no controlada del tipo 'System.NullReferenceException'. Se produjo al estar haciendo pruebas 
                // con el radioTemp  sin permisos y reiniciando.  (int)ramMon.Consulta daba null. datosIntegerInputCargaEquipo = 50
                // intento de reproducción del error de nuevo sin éxito.
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "ERROR", MessageBoxIcon.Error);
            }
        }

        private void radioCPU_CheckedChanged(object sender, EventArgs e)
        {
            carga = opcionCarga.CPU;
            labelInfoCargas1.Text = "carga CPU";
            labelInfoCargas2.Text = "del";
            labelInfoCargas3.Text = "%";
            integerInputDatosCargaEquipo.MaxValue = 100;
            integerInputDatosCargaEquipo.Increment = 1;
        }

        private void radioTemperatura_CheckedChanged(object sender, EventArgs e)
        {
            if (radioTemperatura.Checked)
            {
                carga = opcionCarga.Temperatura;
                labelInfoCargas1.Text = "temp. CPU";
                labelInfoCargas2.Text = "de";
                labelInfoCargas3.Text = "⁰C";
                integerInputDatosCargaEquipo.MaxValue = 100;
                integerInputDatosCargaEquipo.Increment = 1;

                if (Temp.sinPermisos)
                {
                    if (!iniciando && dlgSinPermisosTemperaturaConReinicioApp() == DialogResult.No)
                    {
                        radioTemperatura.Enabled = false;
                    }

                    radioCPU.Checked = true;
                }
            }
        }

        private void comboBox1CargaEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1CargaEquipo.SelectedIndex)
            {
                case 0:
                    esMayorComboCargaEquipo = false;
                    break;
                case 1:
                    esMayorComboCargaEquipo = true;
                    break;
            }
        }

        private void integerInputDatosCargaEquipo_ValueChanged(object sender, EventArgs e)
        {
            datosIntegerInputCargaEquipo = integerInputDatosCargaEquipo.Value;
        }

        private void integerInputMinutosCargaEquipo_ValueChanged(object sender, EventArgs e)
        {
            minutosIntegerInputCargaEquipo = integerInputMinutosCargaEquipo.Value;
        }

        #endregion

        #region Tab Energía

        private void tabEnergia_Click(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                condicion = tipoCondicion.Energia;
            }
        }

        private void radioAlimentacion_CheckedChanged(object sender, EventArgs e)
        {
            if (!iniciando && radioAlimentacion.Checked)
            {
                energia = opcionEnergia.Alimentacion;
            }
        }

        private void comboBoxAlimentacionTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                switch (comboBoxAlimentacionTipo.SelectedIndex)
                {
                    case 0:
                        alimentacion = PowerSource.AC;
                        break;

                    case 1:
                        alimentacion = PowerSource.Battery;
                        break;

                    case 2:
                        alimentacion = PowerSource.Ups;
                        break;
                }
            }
        }

        private void checkBoxDuranteAlimentacion_CheckedChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                duranteAlimentacion = checkBoxDuranteAlimentacion.Checked;
            }
        }

        private void integerInputMinutosAlimentacion_ValueChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                minutosIntegerInputAlimentacion = integerInputMinutosAlimentacion.Value;
            }
        }

        private void radioPlanEnergia_CheckedChanged(object sender, EventArgs e)
        {
            if (!iniciando && radioPlanEnergia.Checked)
            {
                energia = opcionEnergia.PlanEnergia;
            }
        }

        private void comboBoxPlanEnergiaTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxPlanEnergiaTipo.SelectedIndex)
            {
                case 0:
                    planEnergia = PowerPersonality.HighPerformance;
                    break;

                case 1:
                    planEnergia = PowerPersonality.PowerSaver;
                    break;

                case 2:
                    planEnergia = PowerPersonality.Automatic;
                    break;
            }
        }

        private void radioBateria_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBateria.Checked)
            {
                energia = opcionEnergia.Bateria;
            }
        }

        private void comboBoxBatMasMenos_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxBatMasMenos.SelectedIndex)
            {
                case 0:
                    esMasComboBateria = false;
                    integerInputVidaUtilBatPorcent.MaxValue = 100;
                    integerInputVidaUtilBatPorcent.MinValue = 1;
                    break;

                case 1:
                    esMasComboBateria = true;
                    integerInputVidaUtilBatPorcent.MaxValue = 99;
                    integerInputVidaUtilBatPorcent.MinValue = 0;
                    break;
            }
        }

        private void radioSecBatTiempoRestante_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSecBatTiempoRestante.Checked)
            {
                posRadioSecBat = true; // bool: 1 = arriba ; 0 = abajo
                labelInfoBateria1.Text = "de";
            }
        }

        private void dateTimeInputTiempoRestanteBat_ValueChanged(object sender, EventArgs e)
        {
            TimeSpan tFab = PowerManager.GetCurrentBatteryState().EstimatedTimeRemaining;
            TimeSpan tBat = new TimeSpan(0, 0, (int)tFab.Subtract(new TimeSpan(tFab.Days, 0, 0, 0)).TotalSeconds);

            if (dateTimeInputTiempoRestanteBat.Value.TimeOfDay > tBat)
            {
                if (!iniciando)
                {
                    MsgBox("Tiempo seleccionado no válido.\n\n Tiempo estimado restante de vida de batería según el fabricante: " + tBat.ToString(), version, MessageBoxIcon.Information);
                }

                dateTimeInputTiempoRestanteBat.Value = DateTime.Today.Add(tBat);

            }
            tiempoRestanteBateria = dateTimeInputTiempoRestanteBat.Value.TimeOfDay;
        }

        private void radioSecBatVidaUtilPorcent_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSecBatVidaUtilPorcent.Checked)
            {
                posRadioSecBat = false; // bool: 1 = arriba ; 0 = abajo
                labelInfoBateria1.Text = "del";
            }
        }

        private void integerInputVidaUtilBatPorcent_ValueChanged(object sender, EventArgs e)
        {
            porcentajeRestanteBateria = integerInputVidaUtilBatPorcent.Value;
        }

        #endregion

        #region Tab Red

        private void tabEventosRed_Click(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                condicion = tipoCondicion.EventosRed;
            }
        }

        private void comboBoxAdaptadoresRed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                posComboAdaptador = (byte)comboBoxAdaptadoresRed.SelectedIndex;
                indiceRed = ListRed.GetIndiceRed(comboBoxAdaptadoresRed);
            }
        }

        private void radioRedVelocidad_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRedVelocidadRed.Checked)
            {
                if (!iniciando)
                {
                    posRadioRed = true;
                }

                switch (comboBox1SubBajRed.SelectedIndex)
                {
                    case 0:
                        dirRed = Direccion.Subida;
                        break;

                    case 1:
                        dirRed = Direccion.Bajada;
                        break;
                }

                switch (comboBox1UDsegRed.SelectedIndex) //OBSERVACIONES: Necesario para al inicio para establecer udRed
                {
                    case 0:
                        udRed = Magnitud.Bytes;
                        break;

                    case 1:
                        udRed = Magnitud.KB;
                        break;

                    case 2:
                        udRed = Magnitud.MB;
                        break;
                }
            }
        }

        private void comboBox1SubBajRed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                switch (comboBox1SubBajRed.SelectedIndex)
                {
                    case 0:
                        posCombo1BajadaRed = false;

                        if (radioRedVelocidadRed.Checked)
                        {
                            dirRed = Direccion.Subida;
                        }

                        break;

                    case 1:
                        posCombo1BajadaRed = true;

                        if (radioRedVelocidadRed.Checked)
                        {
                            dirRed = Direccion.Bajada;
                        }

                        break;
                }
            }
        }

        private void comboBoxMayorMenorRed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                switch (comboBoxMayorMenorRed.SelectedIndex)
                {
                    case 0:
                        posComboMayorRed = false;
                        break;

                    default:
                        posComboMayorRed = true;
                        break;
                }
            }
        }

        private void integerInput2CantidadUdsRed_ValueChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                cantidadUds2IntegerInputRed = integerInput2CantidadUdsRed.Value;
            }
        }

        private void comboBox1UDsegRed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                switch (comboBox1UDsegRed.SelectedIndex)
                {
                    case 0:
                        UdsxSegRed = Magnitud.Bytes;
                        break;

                    case 1:
                        UdsxSegRed = Magnitud.KB;
                        break;

                    case 2:
                        UdsxSegRed = Magnitud.MB;
                        break;
                }

                if (radioRedVelocidadRed.Checked)
                {
                    udRed = UdsxSegRed;
                }
            }
        }

        private void checkBoxDuranteRed_CheckedChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                duranteRed = checkBoxDuranteRed.Checked;
            }
        }

        private void integerInputMinutosRed_ValueChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                minutosIntegerInputRed = integerInputMinutosRed.Value;
            }
        }

        private void radioRedSeAcabe_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRedSeAcabeRed.Checked)
            {
                if (!iniciando)
                {
                    posRadioRed = false;
                }

                switch (comboBox2SubBajRed.SelectedIndex)
                {
                    case 0:
                        dirRed = Direccion.Subida;
                        break;

                    case 1:
                        dirRed = Direccion.Bajada;
                        break;
                }

                switch (comboBox2UDRed.SelectedIndex) //OBSERVACIONES: Necesario para al inicio para establecer udRed
                {
                    case 0:
                        udRed = Magnitud.KB;
                        break;

                    case 1:
                        udRed = Magnitud.MB;
                        break;

                    case 2:
                        udRed = Magnitud.GB;
                        break;
                }
            }
        }

        private void comboBox2SubBajRed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                switch (comboBox2SubBajRed.SelectedIndex)
                {
                    case 0:
                        posCombo2BajarRed = false;

                        if (radioRedSeAcabeRed.Checked)
                        {
                            dirRed = Direccion.Subida;
                        }

                        break;

                    case 1:
                        posCombo2BajarRed = true;

                        if (radioRedSeAcabeRed.Checked)
                        {
                            dirRed = Direccion.Bajada;
                        }

                        break;
                }
            }
        }

        private void integerInput1CantidadUdsRed_ValueChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                cantidadUds1IntegerInputRed = integerInput1CantidadUdsRed.Value;
            }
        }

        private void comboBox2UDRed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                switch (comboBox2UDRed.SelectedIndex)
                {
                    case 0:
                        Uds2Red = Magnitud.GB;
                        break;

                    case 1:
                        Uds2Red = Magnitud.MB;
                        break;

                    case 2:
                        Uds2Red = Magnitud.KB;
                        break;
                }

                if (radioRedSeAcabeRed.Checked)
                {
                    udRed = Uds2Red;
                }
            }
        }

        #endregion

        #region Tab Unidades

        private void tabUnidades_Click(object sender, EventArgs e)
        {
            condicion = tipoCondicion.Unidades;
        }

        private void comboBoxListUds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                switch (comboBoxListUds.SelectedIndex)
                {
                    case 0:
                        posComboUds = 0;
                        break;
                    case 1:
                        posComboUds = 1;
                        break;
                    case 2:
                        posComboUds = 2;
                        break;
                    case 3:
                        posComboUds = 3;
                        break;
                    case 4:
                        posComboUds = 4;
                        break;
                    case 5:
                        posComboUds = 5;
                        break;
                    case 6:
                        posComboUds = 6;
                        break;
                    case 7:
                        posComboUds = 7;
                        break;
                    case 8:
                        posComboUds = 8;
                        break;
                    case 9:
                        posComboUds = 9;
                        break;
                    case 10:
                        posComboUds = 10;
                        break;
                    case 11:
                        posComboUds = 11;
                        break;
                    case 12:
                        posComboUds = 12;
                        break;
                    case 13:
                        posComboUds = 13;
                        break;
                    case 14:
                        posComboUds = 14;
                        break;
                    case 15:
                        posComboUds = 15;
                        break;
                    case 16:
                        posComboUds = 16;
                        break;
                    case 17:
                        posComboUds = 17;
                        break;
                    case 18:
                        posComboUds = 18;
                        break;
                    case 19:
                        posComboUds = 19;
                        break;
                    case 20:
                        posComboUds = 20;
                        break;
                    case 21:
                        posComboUds = 21;
                        break;
                    case 22:
                        posComboUds = 22;
                        break;
                    case 23:
                        posComboUds = 23;
                        break;
                    case 24:
                        posComboUds = 24;
                        break;
                    case 25:
                        posComboUds = 25;
                        break;
                    case 26:
                        posComboUds = 26;
                        break;



                }
            }
        }

        private void comboBoxDesConexionUds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                switch (comboBoxDesConexionUds.SelectedIndex)
                {
                    case 0:
                        posComboDesConexionUd = false;
                        break;

                    case 1:
                        posComboDesConexionUd = true;
                        break;
                }
            }
        }

        private void checkBoxSiUdEs_CheckedChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                siUdEs = checkBoxSiUdEs.Checked;
            }
        }

        private void comboBoxTipoUds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                switch (comboBoxTipoUds.SelectedIndex)
                {
                    case 0:
                        posComboTipoUd = 0;
                        break;

                    case 1:
                        posComboTipoUd = 1;
                        break;

                    case 2:
                        posComboTipoUd = 2;
                        break;

                    case 3:
                        posComboTipoUd = 3;
                        break;

                    case 4:
                        posComboTipoUd = 4;
                        break;

                    case 5:
                        posComboTipoUd = 5;
                        break;


                }
            }
        }

        private void checkBoxSiUdTieneCapacTot_CheckedChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                siUdTieneCapacTot = checkBoxSiUdTieneCapacTot.Checked;
            }
        }

        private void comboBoxMayorMenorUds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                switch (comboBoxMayorMenorUds.SelectedIndex)
                {
                    case 0:
                        posComboMayorUd = false;
                        break;

                    case 1:
                        posComboMayorUd = true;
                        break;
                }
            }
        }

        private void integerInputUds_ValueChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                cantidadUdsUd = (byte)integerInput1CantidadUdsRed.Value;
            }
        }

        private void comboBoxMagnitudUds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                switch (comboBoxMagnitudUds.SelectedIndex)
                {
                    case 0:
                        MagnitudUd = Magnitud.KB;
                        break;

                    case 1:
                        MagnitudUd = Magnitud.MB;
                        break;

                    case 2:
                        MagnitudUd = Magnitud.GB;
                        break;

                    case 3:
                        MagnitudUd = Magnitud.TB;
                        break;

                }
            }
        }

        #endregion

        #endregion

        #region " Temporización "

        private void timerTempor_Tick(object sender, EventArgs e)
        {
            // PRUEBAS
            //richTextBoxInfoSec.Text += "\nPausa: " + pausaT1.ToString(); 
            if (retardo <= 0)
            {
                setTemp1(false);
                preEjecutaAccion();
                return;
            }

            retardo--;

            circularProgressPpal.Value = retardo;
            circularProgressPpal.ProgressTextFormat = string.Format("{0:P0}", (float)retardo / (float)circularProgressPpal.Maximum);
            labelData.Text = setSegundos(retardo);
            barraTareas.SetProgressValue(circularProgressPpal.Maximum - retardo, circularProgressPpal.Maximum);
        }

        private void timerMon_Tick(object sender, EventArgs e)
        {
            labelMonRamPC.Text = ramMon.Usada(RAM.FormatoRetorno.PorcentualFormateado);
            labelMonRamMB.Text = ramMon.Usada(RAM.FormatoRetorno.BytesFormateados);
            labelMonCpu.Text = Math.Round(cpuMon.Contador.NextValue(), 2) + " %";
            labelMonTemp.Text = !Temp.sinPermisos ? Math.Round((float)tempMon.ValorMedioSensor(), 2) + " ⁰C" : "n/a";
        }

        private void timerCarga_Tick(object sender, EventArgs e)
        {
            int valorCarga;

            switch (carga)
            {
                case opcionCarga.RAMPorcentual:
                    valorCarga = (int)ramCarga.Consulta(RAM.InfoRequerida.Usada, RAM.FormatoRetorno.Porcentual);
                    break;

                case opcionCarga.RAMDatos:
                    valorCarga = (int)ramCarga.Consulta(RAM.InfoRequerida.Usada, RAM.FormatoRetorno.MB);
                    break;

                case opcionCarga.CPU:
                    valorCarga = (int)cpuCarga.Contador.NextValue();
                    //REGISTRO: Excepción no controlada del tipo 'System.NullReferenceException'. Se produjo al estar la tempo-
                    //rización activada en RAMPorcentual y pasarla sin parar a CPU. Con la deshabilitación de controles no 
                    //debería pasar.
                    break;

                case opcionCarga.Temperatura:
                    float? valorTemp = tempCarga.ValorMedioSensor() ?? 0;
                    valorCarga = (int)valorTemp;
                    break;

                default:
                    valorCarga = 0;
                    break;
            }
            //PRUEBAS:
            richTextBoxDepuracion.Text = "Valor Actual:\t" + valorCarga + " " + labelInfoCargas3.Text + "\n" +
                                      "Cuenta Alarma:\t" + cuentaAlarmaCarga + "seg.";

            cuentaAlarmaCarga += esMayorComboCargaEquipo ? valorCarga > datosIntegerInputCargaEquipo ? 1 : cuentaAlarmaCarga * -1 :
                valorCarga < datosIntegerInputCargaEquipo ? 1 : cuentaAlarmaCarga * -1;

            barraTareas.SetProgressValue(cuentaAlarmaCarga, minutosIntegerInputCargaEquipo * 60);
            labelData.Text = labelInfoCargas1.Text + ": " + valorCarga + " " + labelInfoCargas3.Text;

            if (cuentaAlarmaCarga >= integerInputMinutosCargaEquipo.Value * 60)
            {
                timerCarga.Stop();
                cuentaAlarmaCarga = 0;
                preEjecutaAccion();
            }
        }

        private void timerAlimentacion_Tick(object sender, EventArgs e)
        {
            cuentaAlarmaEnergia += alimentacion == PowerManager.PowerSource ? 1 : cuentaAlarmaEnergia * -1;
            barraTareas.SetProgressValue(cuentaAlarmaEnergia, minutosIntegerInputAlimentacion * 60);

            //PRUEBAS: 
            richTextBoxDepuracion.Text = "Valor Actual:\t" + PowerManager.PowerSource + "\n" +
                                      "CuentaAlarma:\t" + cuentaAlarmaEnergia;

            labelData.Text = "Alimentación: " + PowerManager.PowerSource;

            if (cuentaAlarmaEnergia >= integerInputMinutosAlimentacion.Value * 60)
            {
                alimentacionEnabled = false;
                timerAlimentacion.Stop();
                cuentaAlarmaEnergia = 0;
                preEjecutaAccion();
            }
        }

        private void timerRed_Tick(object sender, EventArgs e)
        {
            double valor = red.SiguienteValor();

            if (radioRedVelocidadRed.Checked)
            {
                if ((comboBoxMayorMenorRed.SelectedIndex == 0 && valor > integerInput1CantidadUdsRed.Value) ||
                    (comboBoxMayorMenorRed.SelectedIndex == 1 && valor < integerInput1CantidadUdsRed.Value))
                {

                    if (duranteRed)
                    {
                        cuentaAlarmaRed += 1;

                        if (cuentaAlarmaRed >= integerInputMinutosRed.Value * 60)
                        {
                            timerRed.Stop();
                            preEjecutaAccion();
                        }
                    }
                    else
                    {
                        cuentaAlarmaRed = 0;

                        timerRed.Stop();
                        preEjecutaAccion();
                    }
                }
                else
                {
                    cuentaAlarmaRed = 0;
                }

                barraTareas.SetProgressValue(cuentaAlarmaRed, integerInputMinutosRed.Value * 60);
                labelData.Text = comboBox1SubBajRed.SelectedItem + ": " + valor.ToString("F1") + " " + udRed + "/seg.";
            }

            else
            {

                cuentaConsultalRed += valor;

                if (cuentaConsultalRed >= integerInput2CantidadUdsRed.Value)
                {
                    timerRed.Stop();
                    preEjecutaAccion();
                }

                labelData.Text = comboBox2SubBajRed.SelectedItem + ": " + cuentaConsultalRed.ToString("F1") + " " + udRed;
            }
            //PRUEBAS:
            richTextBoxDepuracion.Text = "Valor Actual  : " + valor + " " + udRed + "\n" +
                                      "Cuenta Alarma : " + cuentaAlarmaRed + "\n" +
                                      "CuentaConsulta: " + cuentaConsultalRed + " " + udRed;

            //labelData.Text = valor + " " + udRed;
        }

        #endregion

        #region " Energía "

        private void alimentacion_Changed(object sender, EventArgs e)
        {
            if (alimentacionEnabled)
            {
                condicionAliminentacion();
            }
        }

        private void planEnergia_Changed(object sender, EventArgs e)
        {
            try
            {
                if (planEnergiaEnabled)
                {
                    condicionPlanEnergia();
                }
            }
            catch (PowerManagerException ex)
            //OBSERVACIONES: (PASAR A NOTAS) Aunque ya lo había descartado por no volver a reproducir el problema (hasta ahora), 
            //introducir el try/catch, SIN DUDA, evita que la aplicación, en ocasiones y sin generar excepciones, se quede 
            //colgada al capturar este evento. No sé por qué.
            {
                MsgBox("Se produjo un error durante la captación del evento:\n'Cambio de Plan de Energía'\n\n" + ex.Message,
                    version, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, version, MessageBoxIcon.Exclamation);
            }
        }

        private void vidaPorcentualBateria_Changed(object sender, EventArgs e)
        {
            if (vidaBateriaEnabled)
            {
                condicionBateria();
            }
        }

        #endregion

        #region " Unidades "

        private void Unidad_Removed(object sender, DriveDetectorEventArgs e)
        {
            if (comboBoxDesConexionUds.SelectedIndex == 1 && comboBoxListUds.SelectedItem.ToString() == e.Drive)
            {
                if ((checkBoxSiUdEs.Checked && checkTipoUd(comboBoxTipoUds.SelectedIndex)) || (checkBoxSiUdTieneCapacTot.Checked &&
                   ((comboBoxMayorMenorUds.SelectedIndex == 0 && tamanoUd > valorCapacidadUds) ||
                    (comboBoxMayorMenorUds.SelectedIndex == 1 && tamanoUd < valorCapacidadUds))) ||
                    (!checkBoxSiUdEs.Checked && !checkBoxSiUdTieneCapacTot.Checked))
                {

                    preEjecutaAccion();
                }
            }
        }

        private void Unidad_Arrived(object sender, DriveDetectorEventArgs e)
        {
            if (comboBoxDesConexionUds.SelectedIndex == 0 && comboBoxListUds.SelectedItem.ToString() == e.Drive)
            {
                if (checkBoxSiUdEs.Checked)
                {
                    setInfoUnidad();

                    if (checkTipoUd(comboBoxTipoUds.SelectedIndex))
                    {
                        preEjecutaAccion();
                        return;
                    }
                }

                if (checkBoxSiUdTieneCapacTot.Checked)
                {
                    //NOTA: En caso de estar 'checkados' los dos checkboxes checkUnidad() se ejecutaría 2 veces
                    setInfoUnidad();

                    if ((comboBoxMayorMenorUds.SelectedIndex == 0 && tamanoUd > valorCapacidadUds) ||
                        (comboBoxMayorMenorUds.SelectedIndex == 1 && tamanoUd < valorCapacidadUds))
                    {

                        preEjecutaAccion();
                        return;
                    }
                }

                preEjecutaAccion();
            }
        }

        #endregion

        #region " NotifyIcon "

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Show();
            BringToFront();
        }

        private void contextMenuStripNotify_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolStripMenuItemRestMinim.Text = WindowState == FormWindowState.Minimized ? "Mostrar" : "Minimizar";
        }

        private void toolStripMenuItemEstado_Click(object sender, EventArgs e)
        {
            setEstadoNotifyIcon();
        }

        private void toolStripMenuItemRestMinim_Click(object sender, EventArgs e)
        {
            WindowState = WindowState == FormWindowState.Minimized ? FormWindowState.Normal : FormWindowState.Minimized;
        }

        private void toolStripMenuItemIniciar_Click(object sender, EventArgs e)
        {
            botonIniciar_Click(sender, e);
        }

        private void toolStripMenuItemPausar_Click(object sender, EventArgs e)
        {
            botonPausa_Click(sender, e);
        }

        private void toolStripMenuItemParar_Click(object sender, EventArgs e)
        {
            botonParar_Click(sender, e);
        }

        private void toolStripMenuItemApagar_Click(object sender, EventArgs e)
        {
            clAcciones.Shutdown(tipoAccion.ApagarInmediato);
        }

        private void toolStripMenuItemReiniciar_Click(object sender, EventArgs e)
        {
            clAcciones.Shutdown(tipoAccion.Reiniciar);
        }

        private void toolStripMenuItemSuspender_Click(object sender, EventArgs e)
        {
            clAcciones.Suspender();
        }

        private void toolStripMenuItemHibernar_Click(object sender, EventArgs e)
        {
            clAcciones.Hibernar();
        }

        private void toolStripMenuItemCerrarSesion_Click(object sender, EventArgs e)
        {
            clAcciones.Shutdown(tipoAccion.CerrarSesion);
        }

        private void toolStripMenuItemBloquearPantalla_Click(object sender, EventArgs e)
        {
            clAcciones.Bloquear();
        }

        private void toolStripMenuItemCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        private void botonTest2_Click(object sender, EventArgs e)
        {
            subIniciarConWindows(false);
        }

        private void botonPausa_Click(object sender, EventArgs e)
        {
            try
            {
                estadoProgramador = Estado.Pausado;

                switch (condicion)
                {
                    case tipoCondicion.A1Hora:

                        if (!pausaT1)
                        {
                            timerTempor.Stop();
                            pausaT1 = true;
                            //barraTareas.SetProgressState(TaskbarProgressBarState.Paused);
                        }

                        break;

                    case tipoCondicion.CuentaAtras:

                        if (!pausaT1)
                        {
                            timerTempor.Stop();
                            pausaT1 = true;
                            //barraTareas.SetProgressState(TaskbarProgressBarState.Paused);
                        }

                        break;

                    case tipoCondicion.CargaEquipo:
                        timerCarga.Stop();
                        break;

                    case tipoCondicion.Energia:

                        switch (energia)
                        {
                            case opcionEnergia.Alimentacion:

                                if (timerAlimentacion.Enabled)
                                {
                                    timerAlimentacion.Stop();
                                }
                                alimentacionEnabled = false;
                                break;

                            case opcionEnergia.PlanEnergia:
                                planEnergiaEnabled = true;
                                break;

                            case opcionEnergia.Bateria:
                                vidaBateriaEnabled = true;
                                break;
                        }
                        break;

                    case tipoCondicion.EventosRed:
                        timerRed.Stop();
                        break;

                    case tipoCondicion.Unidades:
                        barraTareas.SetProgressValue(1, 1);
                        setEventosUd(true);
                        break;
                }

                setBotones(estadoProgramador);
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void botonParar_Click(object sender, EventArgs e)
        {
            try
            {
                estadoProgramador = Estado.Parado;

                switch (condicion)
                {
                    case tipoCondicion.A1Hora:
                        pausaT1 = false;
                        setTemp1(false);
                        break;

                    case tipoCondicion.CuentaAtras:
                        cuentaAlarmaCarga = 0;
                        pausaT1 = false;
                        setTemp1(false);
                        break;

                    case tipoCondicion.CargaEquipo:
                        cuentaAlarmaCarga = 0;
                        timerCarga.Stop();
                        break;

                    case tipoCondicion.Energia:

                        switch (energia)
                        {
                            case opcionEnergia.Alimentacion:

                                if (timerAlimentacion.Enabled)
                                {
                                    cuentaAlarmaEnergia = 0;
                                    timerAlimentacion.Stop();
                                }
                                alimentacionEnabled = false;
                                break;

                            case opcionEnergia.PlanEnergia:
                                planEnergiaEnabled = true;
                                condicionPlanEnergia();
                                break;

                            case opcionEnergia.Bateria:
                                vidaBateriaEnabled = true;
                                condicionBateria();
                                break;
                        }
                        break;

                    case tipoCondicion.EventosRed:
                        cuentaAlarmaRed = 0;
                        cuentaConsultalRed = 0;
                        timerRed.Stop();
                        break;

                    case tipoCondicion.Unidades:
                        setEventosUd(true);
                        break;
                }

                setBotones(estadoProgramador);
            }
            catch (Exception ex)
            {
                MsgBox(ex.Message, "Error", MessageBoxIcon.Error);
            }
        }

        private void botonIniciar_Click(object sender, EventArgs e)
        {
            try
            {
                estadoProgramador = Estado.Iniciado;

                switch (condicion)
                {
                    case tipoCondicion.A1Hora:

                        setIconosOverlay(Properties.Resources._48Alarm_clock, "A 1 Hora");

                        retardo = (int)Math.Ceiling(horaA1Hora.Subtract(DateTime.Now).TotalSeconds); //DateTime.Now.Subtract(horaA1Hora).TotalSeconds;

                        if (retardo <= 0) // Y si se pulsa pausa?
                        {
                            //PTE: Crear método independiente. Está repedido en A1Hora y CuentaAtras
                            barraTareas.SetProgressState(TaskbarProgressBarState.Error);
                            barraTareas.SetProgressValue(1, 1);
                            MsgBox("La hora seleccionada no es válida. Debe ser posterior al momento actual", version, MessageBoxIcon.Exclamation);
                            barraTareas.SetProgressState(TaskbarProgressBarState.NoProgress);
                            return;
                        }

                        if (pausaT1)
                        {
                            // if retardo <= 0 el tiempo ya ha pasado, quiere ejecutar la acción, síno
                            timerTempor.Start();
                            pausaT1 = false;
                            return;

                        }

                        setTemp1(true);
                        //PTE: Crear método independiente y Mejorar info panel sec. y formato (colores, negritas..)
                        richTextBoxDepuracion.Text = "Hora seleccionada:\n" + DateTime.Now.AddSeconds(retardo).ToString();

                        break;

                    case tipoCondicion.CuentaAtras:

                        setIconosOverlay(Properties.Resources._48Timer, "Cuenta Atrás");

                        if (pausaT1)
                        {
                            timerTempor.Start();
                            pausaT1 = false;
                            return;
                        }

                        retardo = (int)horaCuentaAtras.TotalSeconds - 1; //NOTA: PARECE que el control añade 1 segundo a la hora seleccionada
                        //      CREO que se hace para diferenciar --:-- de 00:00
                        if (retardo <= 0)
                        {
                            //PTE: Crear método independiente. Está repedido en A1Hora y CuentaAtras
                            barraTareas.SetProgressState(TaskbarProgressBarState.Error);
                            barraTareas.SetProgressValue(1, 1);
                            MsgBox("La hora seleccionada no es válida. Debe ser mayor que 0", version, MessageBoxIcon.Exclamation);
                            barraTareas.SetProgressState(TaskbarProgressBarState.NoProgress);
                            return;
                        }
                        setTemp1(true);
                        //PTE: Crear método independiente y Mejorar info panel sec. y formato (colores, negritas..)
                        richTextBoxDepuracion.Text = "Hora seleccionada:\n" + DateTime.Now.AddSeconds(retardo).ToString();

                        break;

                    case tipoCondicion.CargaEquipo:

                        //labelCargaEquipoMin.Text = "seg."; //PRUEBAS: (seteado como segundos)
                        switch (carga)
                        {
                            case opcionCarga.CPU:

                                setIconosOverlay(Properties.Resources._64HardwareChip, "CPU");

                                cpuCarga = new CPU();
                                cpuCarga.InicioCounter();
                                break;

                            case opcionCarga.Temperatura:

                                setIconosOverlay(Properties.Resources._64Thermometer_Full, "Temperatura");

                                tempCarga = new Temp();
                                tempCarga.Iniciar();
                                break;

                            default:

                                setIconosOverlay(Properties.Resources._64RAM_Drive, "RAM");

                                ramCarga = new RAM();
                                break;
                        }

                        cuentaAlarmaCarga = 0; //NOTA: Para una pausa "real" habría que controlarlo con una bool if(tempCrg) cuentaAlarma=0
                        timerCarga.Start();
                        break;

                    case tipoCondicion.Energia:

                        switch (energia)
                        {
                            case opcionEnergia.Alimentacion:

                                setIconosOverlay(Properties.Resources._48laptop_charge, "Alimentación");

                                alimentacionEnabled = true;
                                condicionAliminentacion();

                                labelData.Text = "Alimentación: " + PowerManager.PowerSource;
                                break;

                            case opcionEnergia.PlanEnergia:

                                setIconosOverlay(Properties.Resources._64power_management, "Plan de energía");

                                planEnergiaEnabled = true;
                                condicionPlanEnergia();

                                labelData.Text = "Plan Energía: " + PowerManager.PowerPersonality;
                                break;

                            case opcionEnergia.Bateria:

                                setIconosOverlay(Properties.Resources._64battery, "Batería");

                                vidaBateriaEnabled = true;
                                condicionBateria();

                                if (radioSecBatVidaUtilPorcent.Checked)
                                {
                                    labelData.Text = "Batería: " + PowerManager.BatteryLifePercent;
                                }

                                else if (radioSecBatTiempoRestante.Checked)
                                {
                                    labelData.Text = "Batería: " +
                                        PowerManager.GetCurrentBatteryState().EstimatedTimeRemaining.ToString("hh\\:mm\\:ss");
                                }

                                break;
                        }

                        break;

                    case tipoCondicion.EventosRed:

                        setIconosOverlay(Properties.Resources._64access_point, "Redes");
                        red = new Red(indiceRed, dirRed, udRed);
                        timerRed.Start();

                        //PRUEBAS:
                        //MsgBox("indiceRed: " + indiceRed + "\ndirRed: " + dirRed + "\nudRed: " + udRed);
                        //cuentaConsultalRed = red.ConsultaFormat();//NOTA: Vigilar si el 1er valor del tempRed debe ser corregido
                        break;

                    case tipoCondicion.Unidades:

                        setIconosOverlay(Properties.Resources._32Unidad, "Unidades");
                        driveDetector = new DriveDetector();
                        setEventosUd();

                        if (checkBoxSiUdEs.Checked || checkBoxSiUdTieneCapacTot.Checked)
                        {
                            setInfoUnidad();
                        }

                        if (checkBoxSiUdTieneCapacTot.Checked)
                        {
                            setValorCapacidadUds();
                        }

                        labelData.Text = string.Empty;
                        break;
                }

                //PRUEBAS:
                //retardo = 20;
                ribbonControl.SelectFirstVisibleRibbonTab();

                setBotones(estadoProgramador);
                //setEstadoNotifyIcon();
            }

            catch (Exception ex)
            {
                MsgBox(ex.Message, "Erro", MessageBoxIcon.Error);
            }
        }

        private void botonTest_Click(object sender, EventArgs e)
        {
            subIniciarConWindows(true);
        }

        private void buttonItemConfMail_Click(object sender, EventArgs e)
        {
            abreFormMail();
        }

        void abreFormMail()
        {
            //Copiado de http://ltuttini.blogspot.com.es/2013/02/winforms-verificar-si-el-form-esta.html

            Form frm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is FormMail);

            if (frm != null)
            {
                frm.BringToFront();
                return;
            }

            frm = new FormMail();
            frm.Show();
        }

        void abreFormEjecuciones()
        {
            //Copiado de http://ltuttini.blogspot.com.es/2013/02/winforms-verificar-si-el-form-esta.html

            Form frm = Application.OpenForms.Cast<Form>().FirstOrDefault(x => x is FormEjecuciones);

            if (frm != null)
            {
                frm.BringToFront();
                return;
            }

            frm = new FormEjecuciones();
            frm.Show();
        }

        private void buttonItemConfEjecuciones_Click(object sender, EventArgs e)
        {
            abreFormEjecuciones();
        }

        private void buttonItemConfAvisos_Click(object sender, EventArgs e)
        {
            //bool siempreEncima = false, bool activarAero = false, eStyle estilo = eStyle.Office2007Black

            var psi = new ProcessStartInfo(Application.StartupPath + @"\AvisosPCS.exe");
            psi.Arguments = siempreEncima.ToString() + " " + activarAero.ToString() + " " + ((int)paleta).ToString();

            Process.Start(psi);
        }

        private void ribbonTabItemInmediato_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Setea el estado del progreso en la barra de tareas, los iconos overlay y las deshabilitaciones
        /// </summary>
        /// <param name="estado">Indica si Programador esta en Parada, Pausa o Iniciado</param>
        void setBotones(Estado estado)
        {
            if (!deshabilitaciones) return;

            switch (estado)
            {
                case Estado.Parado:
                    barraTareas.SetProgressState(TaskbarProgressBarState.NoProgress);
                    setIconosOverlay(new StockIcon((StockIconIdentifier)115).Icon, string.Empty);
                    panelInfoPrimaria.Style.BackColor1.Color = System.Drawing.Color.Firebrick;
                    panelInfoPrimaria.Style.BackColor2.Color = System.Drawing.Color.Maroon;
                    labelActivacion.Text = "Desactivado";
                    labelData.Text = string.Empty;
                    tabControlPpal.Enabled = true;
                    grupoAcciones.Enabled = true;
                    botonIniciar.Enabled = true;
                    botonPausa.Enabled = false;
                    botonParar.Enabled = false;
                    break;

                case Estado.Pausado:
                    barraTareas.SetProgressState(TaskbarProgressBarState.Paused);
                    panelInfoPrimaria.Style.BackColor1.Color = System.Drawing.Color.Orange;
                    panelInfoPrimaria.Style.BackColor2.Color = System.Drawing.Color.DarkOrange;
                    labelActivacion.Text = "Pausado";
                    tabControlPpal.Enabled = false;
                    grupoAcciones.Enabled = false;
                    botonIniciar.Enabled = true;
                    botonPausa.Enabled = false;
                    botonParar.Enabled = true;
                    break;

                case Estado.Iniciado:
                    barraTareas.SetProgressState(TaskbarProgressBarState.Normal);
                    panelInfoPrimaria.Style.BackColor1.Color = Color.Lime;
                    panelInfoPrimaria.Style.BackColor2.Color = Color.Lime;
                    //labelActivacion.BackColor = Color.Lime;
                    //labelData.BackColor = Color.Lime;
                    labelActivacion.Text = "Activado";
                    tabControlPpal.Enabled = false;
                    grupoAcciones.Enabled = false;
                    botonIniciar.Enabled = false;
                    botonPausa.Enabled = true;
                    botonParar.Enabled = true;
                    break;
            }
        }

        private void comboBoxYOUds_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxYOUds.SelectedIndex)
            {
                case 0:
                    posComboYOUd = false;
                    break;

                case 1:
                    posComboYOUd = true;
                    break;
            }
        }

        private void checkBoxRetardarEjecucion_Click(object sender, EventArgs e)
        {
            retardarAcciones = checkBoxRetardarAccionesX.Checked;
        }

        private void integerInputRetardoAcciones_ValueChanged(object sender, EventArgs e)
        {
            segundosRetardoAcciones = (short)integerInputRetardoAcciones.Value;
        }

        private void timerRetardo_Tick(object sender, EventArgs e)
        {
            timerRetardo.Stop();
            ejecutaAccion();
        }

        private void switchButtonItemMinimizarBandeja_ValueChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                minimizarBandeja = switchButtonItemMinimizarBandeja.Value;
                ShowInTaskbar = !minimizarBandeja;
            }
        }

        private void switchButtonItemIniciarConWindows_ValueChanged(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                iniciarConWindows = switchButtonItemIniciarConWindows.Value;
                subIniciarConWindows(iniciarConWindows);
            }
        }

        private void FormPpal_Resize(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                ShowInTaskbar = minimizarBandeja ? false : true;
            }
        }

        private void buttonItemInicioMinimizado_Click(object sender, EventArgs e)
        {
            if (!iniciando)
            {
                iniciarMinimizado = buttonItemInicioMinimizado.Checked;
            }
        }








    }
}

