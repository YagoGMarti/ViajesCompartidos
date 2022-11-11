using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;


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

        private static byte[] CargarImagen(HttpPostedFileBase imagen)
        {
            byte[] Content = new byte[imagen.InputStream.Length];
            imagen.InputStream.Read(Content, 0, imagen.ContentLength);
            imagen.InputStream.Close();
            return Content;
        }
    }
}