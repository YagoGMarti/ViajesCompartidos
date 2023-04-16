using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace ViajesCompartidos.Handlers
{
    public class VehiculoHandler : BaseHandler
    {
        public static VehiculoModel GetVehiculo(Guid ID)
        {
            return ViajesCompartidosContext.GetVehiculo(ID);
        }

        public static IEnumerable<VehiculoModel> GetVehiculos()
        {
            return ViajesCompartidosContext.GetVehiculos();
        }

        internal static IEnumerable<VehiculoModel> GetVehiculosPorEmpresa(Guid empresaModel_ID)
        {
            return ViajesCompartidosContext.GetVehiculosPorEmpresa(empresaModel_ID);
        }

        internal static void CrearVehiculo(VehiculoModel vehiculoModel)
        {
            EmpleadoModel empleadoModel = EmpleadoHandler.GetEmpleado(vehiculoModel.Empleado_ID);
            empleadoModel.Vehiculo = vehiculoModel;
            EmpleadoHandler.AgregarVehiculo(empleadoModel);
        }

        internal static void EditarVehiculo(VehiculoModel vehiculoModel)
        {
            if (vehiculoModel.ImagenCarnetConducir != null)
            {
                vehiculoModel.AdjuntoCarnetConducir = CargarImagen(vehiculoModel.ImagenCarnetConducir);
                vehiculoModel.TipoImagenCarnetConducir = vehiculoModel.ImagenCarnetConducir.ContentType;
                vehiculoModel.NombreArchivoCarnetConducir = vehiculoModel.ImagenCarnetConducir.FileName;
            }

            if (vehiculoModel.ImagenComprobantePoliza != null)
            {
                vehiculoModel.AdjuntoComprobantePoliza = CargarImagen(vehiculoModel.ImagenComprobantePoliza);
                vehiculoModel.TipoImagenComprobantePoliza = vehiculoModel.ImagenComprobantePoliza.ContentType;
                vehiculoModel.NombreArchivoComprobantePoliza = vehiculoModel.ImagenComprobantePoliza.FileName;
            }

            ViajesCompartidosContext.EditarVehiculo(vehiculoModel);
        }

        internal static void ValidarVehiculo(VehiculoModel vehiculoModel)
        {
            ViajesCompartidosContext.ValidarVehiculo(vehiculoModel);
        }
       
        private static byte[] CargarImagen(HttpPostedFileBase imagen)
        {
            byte[] Content = new byte[imagen.InputStream.Length];
            imagen.InputStream.Read(Content, 0, imagen.ContentLength);
            imagen.InputStream.Close();
            return Content;
        }
    }
}