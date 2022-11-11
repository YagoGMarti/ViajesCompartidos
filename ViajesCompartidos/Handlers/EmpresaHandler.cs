using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ViajesCompartidos.Handlers
{
    public class EmpresaHandler : BaseHandler
    {
        public static IEnumerable<EmpresaModel> GetEmpresas()
        {
            return ViajesCompartidosContext.GetEmpresas()
                .OrderBy(x => x.Activo).ThenBy(x => x.FechaAlta);
        }

        public static EmpresaModel GetEmpresa(Guid ID)
        {
            return ViajesCompartidosContext.GetEmpresa(ID);
        }

        public static void CrearEmpresa(EmpresaModel empresaModel)
        {
            ViajesCompartidosContext.CrearEmpresa(empresaModel);
        }

        public static void EditarEmpresa(EmpresaModel empresaModel)
        {
            ViajesCompartidosContext.EditarEmpresa(empresaModel);
        }

        internal static void CambiarEstadoActivo(Guid ID, bool estado)
        {
            ViajesCompartidosContext.CambiarEstadoActivoEmpresa(ID, estado);
        }
    }
}