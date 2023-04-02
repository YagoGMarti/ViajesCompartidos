using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViajesCompartidos.Handlers
{
    public class SucursalHandler : BaseHandler
    {
        public static IEnumerable<SucursalModel> GetSucursalesPorEmpresa(Guid EmpresaID)
        {
            return ViajesCompartidosContext.GetSucursalesPorEmpresa(EmpresaID)
                .OrderBy(x => x.Nombre);
        }

        internal static void CambiarEstadoActivo(Guid ID, bool estado)
        {
            ViajesCompartidosContext.CambiarEstadoActivoSucursal(ID, estado);
        }

        internal static SucursalModel GetSucursal(Guid ID)
        {
            return ViajesCompartidosContext.GetSucursal(ID);
        }

            public static void EditarSucursal(SucursalModel sucursalModel)
        {
            ViajesCompartidosContext.EditarSucursal(sucursalModel);
        }

        public static void CrearSucursal(SucursalModel sucursalModel)
        {
            ViajesCompartidosContext.CrearSucursal(sucursalModel);
        }
    }
}