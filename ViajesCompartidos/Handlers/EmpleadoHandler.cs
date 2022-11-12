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

            if (SucursalID == null && EmpresaID != null)
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

            if (ViajesCompartidosContext.GetEmpleadoByEmail(empleadoModel.CorreoElectronicoEncriptado) != null)
                throw new ArgumentException(empleadoModel.CorreoElectronico);

            if (!string.IsNullOrWhiteSpace(empleadoModel.Telefono))
                empleadoModel.TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empleadoModel.Telefono));
            // TODO : Cambiar a envio de clave automático cuando esté el cliente HTTP ( probablemente usar el restablecer clave y no redefinir )             
            empleadoModel.ClaveEncriptada = EncriptadoHandler.Encriptar("123123Aa!");

            var sucursal = SucursalHandler.GetSucursal(empleadoModel.SucursalModel_ID);
            empleadoModel.DistanciaSucursal = RutaHandler.CalcularDistancia(empleadoModel.Ubicacion, sucursal.Ubicacion);

            ViajesCompartidosContext.CrearEmpleado(empleadoModel);
        }

        public static void EditarEmpleado(EmpleadoModel empleadoModel)
        {
            empleadoModel.ActualizarRoles();
            //empleadoModel.CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empleadoModel.CorreoElectronico));
            if (!string.IsNullOrWhiteSpace(empleadoModel.Telefono)) 
                empleadoModel.TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empleadoModel.Telefono));

            var sucursal = SucursalHandler.GetSucursal(empleadoModel.SucursalModel_ID);
            empleadoModel.DistanciaSucursal = RutaHandler.CalcularDistancia(empleadoModel.Ubicacion, sucursal.Ubicacion);

            ViajesCompartidosContext.EditarEmpleado(empleadoModel);
        }

        internal static void CambiarEstadoActivo(Guid ID, bool estado)
        {
            ViajesCompartidosContext.CambiarEstadoActivoEmpleado(ID, estado);
        }
    }
}