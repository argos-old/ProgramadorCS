using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

using Cifra2;

namespace ProgramadorCS
{
    class Mail
    {
        #region " Constructor "

        public Mail (bool eventoEnvioCompletado)
        {
            this.eventoEnvioCompletado = eventoEnvioCompletado;
        }

        #endregion

        #region " Propiedades "

        bool ssl
        {
            get
            {
                return Properties.Settings.Default.FMSSL;
            }
        }

        bool posComboCcCco
        {
            get
            {
                return Properties.Settings.Default.FMPosComboCcCco;
            }
        }

        string remitente
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMRemitente);
            }            
        }

        string usuario
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMUsuario);
            }
        }

        string host
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMHost);
            }
        }

        string contrasena
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMContrasena);
            }
        }

        string destinatario
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMDestinatario);
            }
        }

        string cccco
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMCcCco);
            }
        }

        string asunto
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMAsunto);
            }
        }

        string cuerpo
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMCuerpo);
            }
        }

        string archivoAdjunto
        {
            get
            {
                return Funciones.DescifraTxt(Properties.Settings.Default.FMArchivoAdjunto);
            }
        }

        ushort puerto
        {
            get
            {
                //Para utilizarlo habría que "vaciar" FMPuerto. También convendría pasarlo a string
                //return ushort.Parse(Funciones.DescifraTxt(Properties.Settings.Default.FMPuerto.ToString()));
                return Properties.Settings.Default.FMPuerto;
            }
        }

        #endregion

        #region " Variables "

        bool eventoEnvioCompletado;

        #endregion

        #region " Métodos "

        /// <summary>
        /// Método para enviar el correo electrónico. Requiere que previamente las propiedades de configuración del e-mail
        /// hayan sido establecidas. Al final de éste, si se establece en el constructor, se generará  un evento
        /// SmptClient.SendCompleted
        /// </summary>
        public void EnviarMail()
        {
            //Creación del mensaje:
            MailMessage msjMail = new MailMessage();

            msjMail.From = new MailAddress(remitente);
            msjMail.To.Add(new MailAddress(destinatario)); //Ver como funciona la colección MailAdressCollection
            msjMail.Subject = asunto;
            msjMail.Body = cuerpo;

            //Si hay destinatario CC o CCO
            if (!string.IsNullOrEmpty(cccco))
            {
                if (posComboCcCco)
                {
                    msjMail.CC.Add(new MailAddress(cccco));
                }
                else
                {
                    msjMail.Bcc.Add(new MailAddress(cccco));
                }
            }

            //Si hay que enviar algún adjunto:
            if (File.Exists(archivoAdjunto))
            {
                Attachment adjunto = new Attachment(archivoAdjunto, MediaTypeNames.Application.Octet);

                ContentDisposition encabezadoMime = adjunto.ContentDisposition;

                encabezadoMime.CreationDate = File.GetCreationTime(archivoAdjunto);
                encabezadoMime.ModificationDate = File.GetLastWriteTime(archivoAdjunto);
                encabezadoMime.ReadDate = File.GetLastAccessTime(archivoAdjunto);

                msjMail.Attachments.Add(adjunto);
            }

            //Datos SMTP del proveedor
            SmtpClient clienteSmtp = new SmtpClient();

            clienteSmtp.Host = host;
            clienteSmtp.Port = puerto;
            clienteSmtp.UseDefaultCredentials = false;
            clienteSmtp.Credentials = new NetworkCredential(usuario, contrasena);
            clienteSmtp.EnableSsl = ssl;
            clienteSmtp.Timeout = 7500; //Al utilizar el envío asíncrono esta propiedad en principio no valdría para nada.
            clienteSmtp.DeliveryFormat = SmtpDeliveryFormat.International;
            clienteSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            //Se envía el mensaje:
            clienteSmtp.SendAsync(msjMail, true);

            //Si se requiere en el contructor, se genera un evento para verificar si el envío ha sido correcto:
            if (eventoEnvioCompletado)
            {
                clienteSmtp.SendCompleted += new SendCompletedEventHandler(EnvioCompletado);
            }
        }

        /// <summary>
        /// PTE DE MEJORAR: Lo que se hace en este método es establecer como true la propiedad FormMail.EnvioCorrecto
        /// para que una vez allí se genere un evento que capture su cambio
        /// </summary>
        private void EnvioCompletado(object sender, AsyncCompletedEventArgs e)
        {
            FormMail fm = new FormMail();
            fm.EnvioAceptado = true;
        }

        #endregion

    }
}
