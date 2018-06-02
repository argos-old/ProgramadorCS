using System;
using System.IO;
using System.Timers;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Nini.Config;

namespace Rep
{
    public class Reproductor
    {
        #region " Constructor "

        public Reproductor()
        {

        }

        #endregion

        #region " Declaraciones "

        public IWavePlayer reproductorWaveOut = new WaveOutEvent();
        public SampleChannel canal;
        AudioFileReader lectorFicheroAudio;
        Mp3FileReader lectorMp3;
        WaveFileReader lectorWav;

        static readonly string rutaConf = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Avisos.config"; //Path.GetDirectoryName(Application.ExecutablePath);

        Timer temporizador;

        TipoLectorNAudio tipoAudio;
        #endregion

        #region " Propiedades "

        bool reproduccionBucle
        {
            get
            {
                return new IniConfigSource(rutaConf).Configs["Configuracion"].GetBoolean("ReproduccionBucle", false);
            }
        }

        bool pararAvisoSonoro
        {
            get
            {
                return new IniConfigSource(rutaConf).Configs["Configuracion"].GetBoolean("PararAvisoSonoro", false);
            }
        }

        int segParadaAvSonoro
        {
            get
            {
                return new IniConfigSource(rutaConf).Configs["Configuracion"].GetInt("SegParadaAvSonoro", 20);
            }
        }

        float volumenConfig
        {
            get
            {
                return new IniConfigSource(rutaConf).Configs["Configuracion"].GetFloat("Volumen", 0.9f);
            }
        }

        string temaPersonalElegido
        {
            get
            {
                string valor = new IniConfigSource(rutaConf).Configs["Configuracion"].Get("TemaPersonalElegido", string.Empty);
                return (valor == string.Empty) ? string.Empty : Cifra2.Funciones.DescifraTxt(valor);
                //return valor;
            }
        }

        TipoAvisoSonoro tipoSonoro
        {
            get
            {
                return (TipoAvisoSonoro)new IniConfigSource(rutaConf).Configs["Configuracion"].GetInt("PosComboAvisoSonoro", 0);
            }
        }

        public float Volumen
        {
            get
            {
                return canal.Volume;
            }
            set
            {
                canal.Volume = value;
            }
        }

        public EstadoReproductor Estado
        {
            get
            {
                return (EstadoReproductor)reproductorWaveOut.PlaybackState;
            }
        }

        public TimeSpan TiempoTotal
        {
            get
            {
                switch (tipoAudio)
                {
                    case TipoLectorNAudio.AudioFileReader:
                        return lectorFicheroAudio.TotalTime;

                    case TipoLectorNAudio.Mp3Reader:
                        return lectorMp3.TotalTime;

                    case TipoLectorNAudio.WavReader:
                        return lectorWav.TotalTime;

                    default:
                        return TimeSpan.Zero;
                }
            }
        }

        public TimeSpan TiempoActual
        {
            get
            {
                switch (tipoAudio)
                {
                    case TipoLectorNAudio.AudioFileReader:
                        return lectorFicheroAudio.CurrentTime;

                    case TipoLectorNAudio.Mp3Reader:
                        return lectorMp3.CurrentTime;

                    case TipoLectorNAudio.WavReader:
                        return lectorWav.CurrentTime;

                    default:
                        return TimeSpan.Zero;
                }
            }
        }

        #endregion

        #region " Funciones de reproducción "

        public void ReproducirAvisoSonoro()
        {
            switch (tipoSonoro)
            {
                case TipoAvisoSonoro.Bip:
                    ReproducirRecursoWav(Resource1.bip);
                    break;

                case TipoAvisoSonoro.Gallo:
                    ReproducirRecursoWav(Resource1.gallo);
                    break;

                case TipoAvisoSonoro.AlarmaIncendios:
                    ReproducirRecursoMp3(Resource1.alarmaIncendios);
                    break;

                case TipoAvisoSonoro.Aplausos:
                    ReproducirRecursoMp3(Resource1.aplausos);
                    break;

                case TipoAvisoSonoro.Bip2:
                    ReproducirRecursoMp3(Resource1.bip2);
                    break;

                case TipoAvisoSonoro.Bip3:
                    ReproducirRecursoMp3(Resource1.bip3);
                    break;

                case TipoAvisoSonoro.Burro:
                    ReproducirRecursoMp3(Resource1.burro);
                    break;

                case TipoAvisoSonoro.Campanillas:
                    ReproducirRecursoMp3(Resource1.campanillas);
                    break;

                case TipoAvisoSonoro.CorazonMonitorizado:
                    ReproducirRecursoMp3(Resource1.corazon);
                    break;

                case TipoAvisoSonoro.CorazonLatiendo:
                    ReproducirRecursoMp3(Resource1.corazonLatidoBucle);
                    break;

                case TipoAvisoSonoro.DespertadorDigital:
                    ReproducirRecursoMp3(Resource1.despertador);
                    break;

                case TipoAvisoSonoro.DespertadorAntiguo:
                    ReproducirRecursoMp3(Resource1.despertadorAntiguo);
                    break;

                case TipoAvisoSonoro.DoceCampanadas:
                    ReproducirRecursoWav(Resource1.doceCampanadas);
                    break;

                case TipoAvisoSonoro.LlamadaEnterprise:
                    ReproducirRecursoMp3(Resource1.llamadaEnterprise);
                    break;

                case TipoAvisoSonoro.Metralleta:
                    ReproducirRecursoWav(Resource1.metralleta);
                    break;

                case TipoAvisoSonoro.RisaBebe:
                    ReproducirRecursoMp3(Resource1.risaBebe);
                    break;

                case TipoAvisoSonoro.RisaFemenina:
                    ReproducirRecursoMp3(Resource1.risafemeninaMiedo);
                    break;

                case TipoAvisoSonoro.RisaMasculina:
                    ReproducirRecursoMp3(Resource1.risaMaslulinaMiedo);
                    break;

                case TipoAvisoSonoro.RitmoPercusion1:
                    ReproducirRecursoMp3(Resource1.ritmoBaseBateria);
                    break;

                case TipoAvisoSonoro.RitmoPercusion2:
                    ReproducirRecursoMp3(Resource1.ritmoBaseBateria2);
                    break;

                case TipoAvisoSonoro.RitmoPercusion3:
                    ReproducirRecursoMp3(Resource1.ritmoBaseBateria4);
                    break;

                case TipoAvisoSonoro.RitmoPercusion4:
                    ReproducirRecursoMp3(Resource1.ritmoBaseBateria6);
                    break;

                case TipoAvisoSonoro.RitmoMilitar:
                    ReproducirRecursoMp3(Resource1.ritmoMilitar);
                    break;

                case TipoAvisoSonoro.RitmoRedoble:
                    ReproducirRecursoMp3(Resource1.ritmoRedoble);
                    break;

                case TipoAvisoSonoro.RitmoTimbales:
                    ReproducirRecursoMp3(Resource1.ritmoTimbales);
                    break;

                case TipoAvisoSonoro.Robot:
                    ReproducirRecursoMp3(Resource1.robot);
                    break;

                case TipoAvisoSonoro.SirenaMaderos:
                    ReproducirRecursoMp3(Resource1.sirenaMaderos2);
                    break;

                case TipoAvisoSonoro.TicTac:
                    ReproducirRecursoMp3(Resource1.tictac);
                    break;

                case TipoAvisoSonoro.TelefonoAntiguo:
                    ReproducirRecursoMp3(Resource1.telefonoAntiguo2);
                    break;

                case TipoAvisoSonoro.TelefonoDigital:
                    ReproducirRecursoMp3(Resource1.telefono8Tonos);
                    break;

                case TipoAvisoSonoro.TemaPersonal:

                    if (File.Exists(temaPersonalElegido))
                    {
                        if (temaPersonalElegido.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                        {
                            ReproducirMp3(temaPersonalElegido);
                        }
                        else if (temaPersonalElegido.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                        {
                            ReproducirWav(temaPersonalElegido);
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Subrutina creada para cargar y leer los avisos y poder leer el tiempo total del tema en el form de configuración.
        /// Nota: Al ser necesario establecer lectorMp3 o lectorWav aquí, deja de ser necesario hacerlo en ReproducirRecursoXX
        /// </summary>
        public void CargarAvisoSonoro()
        {
            switch (tipoSonoro)
            {
                case TipoAvisoSonoro.Bip:
                    tipoAudio = TipoLectorNAudio.WavReader;
                    lectorWav = new WaveFileReader(Resource1.bip);
                    break;

                case TipoAvisoSonoro.Gallo:
                    tipoAudio = TipoLectorNAudio.WavReader;
                    lectorWav = new WaveFileReader(Resource1.gallo);
                    break;

                case TipoAvisoSonoro.AlarmaIncendios:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.alarmaIncendios));
                    break;

                case TipoAvisoSonoro.Aplausos:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.aplausos));
                    break;

                case TipoAvisoSonoro.Bip2:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.bip2));
                    break;

                case TipoAvisoSonoro.Bip3:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.bip3));
                    break;

                case TipoAvisoSonoro.Burro:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.burro));
                    break;

                case TipoAvisoSonoro.Campanillas:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.campanillas));
                    break;

                case TipoAvisoSonoro.CorazonMonitorizado:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.corazon));
                    break;

                case TipoAvisoSonoro.CorazonLatiendo:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.corazonLatidoBucle));
                    break;

                case TipoAvisoSonoro.DespertadorDigital:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.despertador));
                    break;

                case TipoAvisoSonoro.DespertadorAntiguo:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.despertadorAntiguo));
                    break;

                case TipoAvisoSonoro.DoceCampanadas:
                    tipoAudio = TipoLectorNAudio.WavReader;
                    lectorWav = new WaveFileReader(Resource1.doceCampanadas);
                    break;

                case TipoAvisoSonoro.LlamadaEnterprise:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.llamadaEnterprise));
                    break;

                case TipoAvisoSonoro.Metralleta:
                    tipoAudio = TipoLectorNAudio.WavReader;
                    lectorWav = new WaveFileReader(Resource1.metralleta);
                    break;

                case TipoAvisoSonoro.RisaBebe:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.risaBebe));
                    break;

                case TipoAvisoSonoro.RisaFemenina:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.risafemeninaMiedo));
                    break;

                case TipoAvisoSonoro.RisaMasculina:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.risaMaslulinaMiedo));
                    break;

                case TipoAvisoSonoro.RitmoPercusion1:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.ritmoBaseBateria));
                    break;

                case TipoAvisoSonoro.RitmoPercusion2:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.ritmoBaseBateria2));
                    break;

                case TipoAvisoSonoro.RitmoPercusion3:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.ritmoBaseBateria4));
                    break;

                case TipoAvisoSonoro.RitmoPercusion4:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.ritmoBaseBateria6));
                    break;

                case TipoAvisoSonoro.RitmoMilitar:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.ritmoMilitar));
                    break;

                case TipoAvisoSonoro.RitmoRedoble:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.ritmoRedoble));
                    break;

                case TipoAvisoSonoro.RitmoTimbales:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.ritmoTimbales));
                    break;

                case TipoAvisoSonoro.Robot:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.robot));
                    break;

                case TipoAvisoSonoro.SirenaMaderos:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.sirenaMaderos2));
                    break;

                case TipoAvisoSonoro.TicTac:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.tictac));
                    break;

                case TipoAvisoSonoro.TelefonoAntiguo:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.telefonoAntiguo2));
                    break;

                case TipoAvisoSonoro.TelefonoDigital:
                    tipoAudio = TipoLectorNAudio.Mp3Reader;
                    lectorMp3 = new Mp3FileReader(new MemoryStream(Resource1.telefono8Tonos));
                    break;

                case TipoAvisoSonoro.TemaPersonal:

                    if (File.Exists(temaPersonalElegido))
                    {
                        if (temaPersonalElegido.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                        {
                            tipoAudio = TipoLectorNAudio.Mp3Reader;
                            lectorMp3 = new Mp3FileReader(new FileStream(temaPersonalElegido, FileMode.Open));
                        }
                        else if (temaPersonalElegido.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                        {
                            tipoAudio = TipoLectorNAudio.WavReader;
                            lectorWav = new WaveFileReader(new FileStream(temaPersonalElegido, FileMode.Open));
                        }
                    }

                    break;
            }
        }

        public void ReproducirAudioGenerico(string temaAudio)
        {
            tipoAudio = TipoLectorNAudio.AudioFileReader;

            lectorFicheroAudio = new AudioFileReader(temaAudio);
            canal = new SampleChannel(lectorFicheroAudio, true);

            setPlay(canal);
        }

        private void ReproducirRecursoMp3(byte[] recursoMp3)
        {
            canal = new SampleChannel(new LoopStream(lectorMp3, reproduccionBucle), true);

            setPlay(canal);
        }

        public void ReproducirMp3(string archivoMp3)
        {
            canal = new SampleChannel(new LoopStream(lectorMp3, reproduccionBucle), true);

            setPlay(canal);
        }

        public void ReproducirRecursoWav(Stream recursoWav)
        {
            canal = new SampleChannel(new LoopStream(lectorWav, reproduccionBucle), true);

            setPlay(canal);
        }

        public void ReproducirWav(string archivoWav)
        {
            canal = new SampleChannel(new LoopStream(lectorWav, reproduccionBucle), true);

            setPlay(canal);
        }

        public void Detener()
        {
            reproductorWaveOut.Stop();
            reproductorWaveOut.Dispose();

            if (lectorFicheroAudio != null)
            {
                lectorFicheroAudio.Dispose();
            }
            else if (lectorMp3 != null)
            {
                lectorMp3.Dispose();
            }
            else if (lectorWav != null)
            {
                lectorWav.Dispose();
            }

            /*switch (tipoAudio)
            {
                case TipoLectorNAudio.AudioFileReader:
                    lectorFicheroAudio.Dispose();
                    break;

                case TipoLectorNAudio.Mp3Reader:
                    lectorMp3.Dispose();
                    break;

                case TipoLectorNAudio.WavReader:
                    lectorWav.Dispose();
                    break;

                default:
                    lectorFicheroAudio.Dispose();
                    break;
            }//*/

            
        }

        public void Pausar()
        {
            reproductorWaveOut.Pause();
        }

        public void Continuar()
        {
            reproductorWaveOut.Play();
        }

        private void setPlay(SampleChannel cnl)
        {
            cnl.Volume = volumenConfig;

            reproductorWaveOut.Init(cnl);
            reproductorWaveOut.Play();

            if (pararAvisoSonoro)
            {
                temporizador = new Timer(segParadaAvSonoro * 1000);
                temporizador.Elapsed += new ElapsedEventHandler(temporizador_Elapsed);
                temporizador.Enabled = true;
            }
        }

        #endregion

        #region " Temporización "

        void temporizador_Elapsed(object sender, ElapsedEventArgs e)
        {
            Detener();
            temporizador.Stop();
        }

        #endregion

    }
}
