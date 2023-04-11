using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using System.Linq;
using SistemaViajesCompartidos.Temporal;

namespace ViajesCompartidos.Handlers
{
    public class SesionHandler : BaseHandler
    {
        public static EmpleadoModel IniciarSesion(InicioSesion inicioSesion)
        {
            inicioSesion.EmailEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(inicioSesion.Email));
            EmpleadoModel empleado = ViajesCompartidosContext.GetEmpleadoPorCorreoElectronico(inicioSesion.EmailEncriptado);

            if (empleado == null)
                return null;

            if (! empleado.Activo)
                return null;

            inicioSesion.ClaveEncriptada = EncriptadoHandler.Encriptar(inicioSesion.Clave);
            if (inicioSesion.ClaveEncriptada.SequenceEqual(empleado.ClaveEncriptada))
                return empleado;

            return null;
        }

        public static RolesEmpleadoFlag GetRolEmpleado(Guid empleadoID)
        {
            return ViajesCompartidosContext.GetRolEmpleado(empleadoID);
        }

        internal static void RestablecerClave(Guid empleadoID)
        {
            var ReinicioClave = EmpleadoHandler.ReiniciarClave(empleadoID);
            var mailsHandler = new CorreoElectronicoHandler();
            var destinatario = EncriptadoHandler.DesEncriptar(EncriptadoHandler.StringToBytes(ReinicioClave.Item1));
            mailsHandler.EnviarClave(destinatario, ReinicioClave.Item2); // email - clave
        }

        internal static void CambiarClave(Guid empleadoID, string nuevaClave)
        {
            EmpleadoHandler.ReiniciarClave(empleadoID, nuevaClave);
        }
    }
}