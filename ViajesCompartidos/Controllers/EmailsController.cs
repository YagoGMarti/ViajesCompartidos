using SistemaViajesCompartidos.Enums;
using System;
using System.Web.Mvc;
using ViajesCompartidos.Handlers;
using static ViajesCompartidos.Controllers.BaseController;

namespace ViajesCompartidos.Controllers
{
    [RevisarUsuarioLogueado]
    [RevisarRoles(RolesEmpleadoFlag.ADMINISTRADOR)]
    public class EmailsController : Controller
    {
        public ActionResult Index()
        {
            return View("Index", CorreoElectronicoHandler.GetCorreos());
        }

        public ActionResult ForzarEnvio(Guid ID)
        {
            new CorreoElectronicoHandler().EnviarCorreo(ID);
            return this.Index();
        }
    }
}