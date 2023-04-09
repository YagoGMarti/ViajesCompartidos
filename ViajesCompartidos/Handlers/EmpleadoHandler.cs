using Microsoft.Ajax.Utilities;
using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SistemaViajesCompartidos.Temporal;

namespace ViajesCompartidos.Handlers
{
    public class EmpleadoHandler
    {
        public static EmpleadoModel GetEmpleado(Guid ID)
        {
            var empleado = ViajesCompartidosContext.GetEmpleado(ID);
            empleado = DesencriptarDatos(empleado);
            empleado.RRHH = empleado.Roles.HasFlag(RolesEmpleadoFlag.RRHH);

            if (empleado.Recorrido_ID != null && empleado.RecorridoActivo)
            {
                empleado.Recorrido = RecorridoHandler.GetRecorrido(empleado.Recorrido_ID);
                if (empleado.Recorrido.Conductor_ID == ID)
                    empleado.Recorrido.Pasajeros.ForEach(x => DesencriptarDatos(x));
                else
                    empleado.Recorrido.Pasajeros = new List<EmpleadoModel>() { GetEmpleado(empleado.Recorrido.Conductor_ID) };
            }

            return empleado;
        }

        public static EmpleadoModel GetEmpleadoPorEmail(String email)
        {
            var mailEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(email));
            var empleado = ViajesCompartidosContext.GetEmpleadoPorCorreoElectronico(mailEncriptado);
            return empleado;
        }

        public static EmpleadoModel DesencriptarDatos(EmpleadoModel empleado)
        {
            empleado.CorreoElectronico = EncriptadoHandler.DesEncriptar(EncriptadoHandler.StringToBytes(empleado.CorreoElectronicoEncriptado));
            if (!string.IsNullOrWhiteSpace(empleado.TelefonoEncriptado))
                empleado.Telefono = EncriptadoHandler.DesEncriptar(EncriptadoHandler.StringToBytes(empleado.TelefonoEncriptado));

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

            return empleados.OrderBy(x => x.CorreoElectronico);
        }

        public static void CrearEmpleado(EmpleadoModel empleadoModel, Guid empresaID)
        {
            empleadoModel.EmpresaModel_ID = empresaID;
            empleadoModel.ActualizarRoles();
            empleadoModel.CorreoElectronicoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empleadoModel.CorreoElectronico));

            if (ViajesCompartidosContext.GetEmpleadoPorCorreoElectronico(empleadoModel.CorreoElectronicoEncriptado) != null)
                throw new ArgumentException(empleadoModel.CorreoElectronico);

            if (!string.IsNullOrWhiteSpace(empleadoModel.Telefono))
                empleadoModel.TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empleadoModel.Telefono));

            var sucursal = SucursalHandler.GetSucursal(empleadoModel.SucursalModel_ID);
            empleadoModel.DistanciaSucursal = RecorridoHandler.CalcularDistancia(empleadoModel.Ubicacion, sucursal.Ubicacion);

            ViajesCompartidosContext.CrearEmpleado(empleadoModel);

            SesionHandler.RestablecerClave(empleadoModel.ID);
        }

        public static void EditarEmpleado(EmpleadoModel empleadoModel)
        {
            empleadoModel.ActualizarRoles();
            
            {
                EmpleadoModel empleado = GetEmpleado(empleadoModel.ID);
                if(empleadoModel.Ubicacion.UbicacionTexto != empleado.Ubicacion.UbicacionTexto
                    && empleado.RecorridoActivo)
                    RecorridoHandler.RemoverPasajero(empleado.Recorrido_ID, empleadoModel.ID);
            }

            if (!string.IsNullOrWhiteSpace(empleadoModel.Telefono))
                empleadoModel.TelefonoEncriptado = EncriptadoHandler.BytesToString(EncriptadoHandler.Encriptar(empleadoModel.Telefono));

            var sucursal = SucursalHandler.GetSucursal(empleadoModel.SucursalModel_ID);
            empleadoModel.DistanciaSucursal = RecorridoHandler.CalcularDistancia(empleadoModel.Ubicacion, sucursal.Ubicacion);

            ViajesCompartidosContext.EditarEmpleado(empleadoModel);
        }

        public static Tuple<string, string> ReiniciarClave(Guid empleadoID)
        {
            string clave = Guid.NewGuid().ToString();
            return ReiniciarClave(empleadoID, clave);
        }

        public static Tuple<string, string> ReiniciarClave(Guid empladoID, string clave)
        {
            var claveEncriptada = EncriptadoHandler.Encriptar(clave);
            var email = ViajesCompartidosContext.ReiniciarClave(empladoID, claveEncriptada);
            return new Tuple<string, string>(email, clave);
        }

        public static void AgregarVehiculo(EmpleadoModel empleadoModel)
        {
            empleadoModel.ActualizarRoles();
            ViajesCompartidosContext.AgregarVehiculo(empleadoModel);
        }

        internal static void CambiarEstadoActivo(Guid ID, bool estado)
        {
            var empleado = ViajesCompartidosContext.GetEmpleado(ID);
            if (!estado && empleado.RecorridoActivo)
            {
                RecorridoHandler.RemoverPasajero(empleado.Recorrido_ID, ID);
            }
            ViajesCompartidosContext.CambiarEstadoActivoEmpleado(ID, estado);
        }

        internal static IEnumerable<EmpleadoModel> GetEmpleadoPorEmpresa(Guid iD)
        {
            throw new NotImplementedException();
        }
    }
}