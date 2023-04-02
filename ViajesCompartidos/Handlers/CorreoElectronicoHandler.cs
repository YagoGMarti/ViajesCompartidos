using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using System.Net.Mail;
using System.Net;
using System.Web.Configuration;

namespace ViajesCompartidos.Handlers
{
    public class CorreoElectronicoHandler
    {
        private bool mailsEnabled;
        private string cuenta;
        private string clave;

        public bool GetMailsEnabled => mailsEnabled;
        public void SetMailsEnabled(bool isEnabled) { mailsEnabled = isEnabled; }
        public string GetCuenta => cuenta;
        public string GetClave => clave;

        public CorreoElectronicoHandler()
        {
            mailsEnabled = Boolean.Parse(WebConfigurationManager.AppSettings["MailsEnabled"]);
            cuenta = EncriptadoHandler.DesEncriptar(EncriptadoHandler.StringToBytes(WebConfigurationManager.AppSettings["MailsCuenta"]));
            clave = EncriptadoHandler.DesEncriptar(EncriptadoHandler.StringToBytes(WebConfigurationManager.AppSettings["MailsClave"]));
        }

        public void EnviarClave(string destinatario, string clave)
        {
            var correoElectronico = new CorreoElectronicoModel()
            {
                TipoCorreoEnum = TipoCorreoEnum.ReinicioClave,
                Destinatario = destinatario
            };

            // Para evitar bloqueo por spam...
            correoElectronico.Destinatario = cuenta;

            correoElectronico.Mensaje = $"<p>Le informamos que su nueva clave es {clave}.</p>";

            correoElectronico = Enviar(correoElectronico);
            GrabarCorreoElectronico(correoElectronico);
        }

        public void EnviarCorreoElectronico(string destinatario, string apodo, TipoCorreoEnum tipoCorreo)
        {
            var correoElectronico = new CorreoElectronicoModel()
            {
                TipoCorreoEnum = tipoCorreo,
                Destinatario = destinatario
            };

            // Para evitar bloqueo por spam...
            correoElectronico.Destinatario = cuenta;

            switch (tipoCorreo)
            {
                case TipoCorreoEnum.NuevaRuta:
                    correoElectronico.Mensaje = $"<h3>Estimado {apodo}.</h3>" +
                        $"<p>Le informamos que tiene una nueva ruta asociada en el Sistema de Viajes Compartidos.</p>";
                    correoElectronico.Asunto = "Nueva ruta!";
                    break;
                case TipoCorreoEnum.RutaCancelada:
                    correoElectronico.Mensaje = $"<h3>Estimado {apodo}.</h3>" +
                        $"<p>Lamentamos informarle que la ruta en el Sistema de Viajes Compartidos ya NO está disponible.</p>";
                    correoElectronico.Asunto = "Ruta cancelada!";
                    break;
                case TipoCorreoEnum.BajaRuta:
                    correoElectronico.Mensaje = $"<h3>Estimado {apodo}.</h3>" +
                        $"<p>Le informamos que un pasajero se desasoció de la ruta en el Sistema de Viajes Compartidos.</p>";
                    correoElectronico.Asunto = "Baja de pasajero!";
                    break;
                default:
                    break;
            }

            correoElectronico = Enviar(correoElectronico);
            GrabarCorreoElectronico(correoElectronico);
        }

        private CorreoElectronicoModel Enviar(CorreoElectronicoModel correoElectronico)
        {
            if (mailsEnabled)
            {
                try
                {
                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com") { Port = 587, Credentials = new NetworkCredential(cuenta, clave), EnableSsl = true })
                    {
                        var mailMessage = new MailMessage
                        {
                            From = new MailAddress(cuenta, "ViajesCompartidos"),
                            Subject = correoElectronico.Asunto,
                            Body = correoElectronico.Mensaje,
                            IsBodyHtml = true,
                        };
                        mailMessage.To.Add(correoElectronico.Destinatario);

                        smtpClient.Send(mailMessage);
                    };

                    correoElectronico.Enviado = true;
                }
                catch (Exception ex)
                {
                    correoElectronico.FalloEnvio = true;
                    correoElectronico.Excepcion = ex.ToString();
                }
            }

            return correoElectronico;
        }

        public bool GrabarCorreoElectronico(CorreoElectronicoModel correoElectrónicoModel)
        {
            try
            {
                correoElectrónicoModel.CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(correoElectrónicoModel.Destinatario));
                ViajesCompartidosContext.GrabarCorreoElectronico(correoElectrónicoModel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}