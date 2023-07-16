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
        public ActionResult ReportePorEmpresa(Guid? EmpresaID)
        {
            if (EmpresaID == null)
            {
                var empleadoID = ObtenerUsuario((Guid)Session["SessionGUID"]);
                var empleado = EmpleadoHandler.GetEmpleado(empleadoID);
                if (empleado != null)
                {
                    EmpresaID = empleado.EmpresaModel_ID;
                }
                else { EmpresaID = empleadoID; }
            }

            return View("ReportePorEmpresa", ReporteHandler.GetReportePorEmpresa(EmpresaID.Value));
        }

        [RevisarRoles(RolesEmpleadoFlag.ADMINISTRADOR | RolesEmpleadoFlag.RRHH)]
        public ActionResult ReportePorSucursal(Guid SucursalID)
        {
            return View("ReportePorSucursal", ReporteHandler.GetReportePorSucursal(SucursalID));
        }
    }
}