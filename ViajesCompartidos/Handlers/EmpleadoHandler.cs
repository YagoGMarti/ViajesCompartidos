using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;

namespace ViajesCompartidos.Handlers
{
    public class EmpleadoHandler
    {
        public static EmpleadoModel GetEmpleado(Guid ID)
        {
            var empleado = ViajesCompartidosContext.GetEmpleado(ID);
            empleado.CorreoElectronico = EncriptadoHandler.DesEncriptar(EncriptadoHandler.StringToBytes(empleado.CorreoElectronicoEncriptado));
            if (!string.IsNullOrWhiteSpace(empleado.TelefonoEncriptado))
                empleado.Telefono = EncriptadoHandler.DesEncriptar(EncriptadoHandler.StringToBytes(empleado.TelefonoEncriptado));
            empleado.RRHH = empleado.Roles.HasFlag(RolesEmpleadoFlag.RRHH);
            return empleado;
        }

        public static IEnumerable<EmpleadoModel> GetEmpleados(Guid? EmpresaID, Guid? SucursalID)
        {
            IEnumerable<EmpleadoModel> empleados = null;

            if (SucursalID != null)
            {
                empleados = ViajesCompartidosContext.GetEmpleadosPorSucursal(SucursalID.Value);
            }

            if (EmpresaID != null)
            {
                empleados = ViajesCompartidosContext.GetEmpleadosPorEmpresa(EmpresaID.Value);
            }

            if (empleados == null)
                return null;

            foreach (var empleado in empleados)
            {
                empleado.CorreoElectronico = EncriptadoHandler.DesEncriptar(EncriptadoHandler.StringToBytes(empleado.CorreoElectronicoEncriptado));
            }

            return empleados;
        }

        public static void CrearEmpleado(EmpleadoModel empleadoModel, Guid empresaID)
        {
            empleadoModel.EmpresaModel_ID = empresaID;
            empleadoModel.ActualizarRoles();
            empleadoModel.CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empleadoModel.CorreoElectronico));
            if (!string.IsNullOrWhiteSpace(empleadoModel.Telefono))
                empleadoModel.TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empleadoModel.Telefono));
            ViajesCompartidosContext.CrearEmpleado(empleadoModel);
        }

        public static void EditarEmpleado(EmpleadoModel empleadoModel)
        {
            empleadoModel.ActualizarRoles();
            empleadoModel.CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empleadoModel.CorreoElectronico));
            if (!string.IsNullOrWhiteSpace(empleadoModel.Telefono)) 
                empleadoModel.TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empleadoModel.Telefono));
            ViajesCompartidosContext.EditarEmpleado(empleadoModel);
        }

        internal static void CambiarEstadoActivo(Guid ID, bool estado)
        {
            ViajesCompartidosContext.CambiarEstadoActivoEmpleado(ID, estado);
        }

        internal static void RestablecerClave(Guid ID)
        {
            // TODO : usar HTML client y mandar una nueva clave
            // TODO : actualizar clave del usuario con la nueva contraseña 
        }
    }
}