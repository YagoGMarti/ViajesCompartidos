using SistemaViajesCompartidos.Enums;
using System;
using System.Web.Mvc;
using ViajesCompartidos.Handlers;
using static ViajesCompartidos.Controllers.BaseController;

namespace ViajesCompartidos.Controllers
{
    [RevisarUsuarioLogueado]
    public class ReportesController : Controller
    {
        [RevisarRoles(RolesEmpleadoFlag.ADMINISTRADOR)]
        public ActionResult Index()
        {
            return View("Index", ReporteHandler.GetReporteAdmin());
        }

        [RevisarRoles(RolesEmpleadoFlag.ADMINISTRADOR | RolesEmpleadoFlag.RRHH)]
        public ActionResult ReportePorEmpresa(Guid EmpresaID)
        {
            return View("ReportePorEmpresa", ReporteHandler.GetReportePorEmpresa(EmpresaID));
        }

        [RevisarRoles(RolesEmpleadoFlag.ADMINISTRADOR | RolesEmpleadoFlag.RRHH)]
        public ActionResult ReportePorSucursal(Guid SucursalID)
        {
            return View("ReportePorSucursal", ReporteHandler.GetReportePorSucursal(SucursalID));
        }
    }
}