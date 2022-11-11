using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using System.Linq;
using ViajesCompartidos.Temporal;

namespace ViajesCompartidos.Handlers
{
    public class SesionHandler : BaseHandler
    {
        public static EmpleadoModel IniciarSesion(InicioSesion inicioSesion)
        {
            inicioSesion.EmailEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(inicioSesion.Email));
            EmpleadoModel empleado = ViajesCompartidosContext.GetEmpleadoByEmail(inicioSesion.EmailEncriptado);

            if (empleado == null)
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

        internal static void EnviarClave(EmpresaModel empresaModel)
        {
            // TODO : generar una nueva clave aleatoria. 
            // TODO : revisar si la combinación email y empresa existe, caso contrario crear empleado rol CORREOINSTITUCIONAL. 
            // TODO : guardar la nueva clave encriptada a nivel empleado. 
            // TODO : enviar por email nueva clave. 
            //EmailHandler.EnviarClave();
            //throw new NotImplementedException();
        }
    }
}