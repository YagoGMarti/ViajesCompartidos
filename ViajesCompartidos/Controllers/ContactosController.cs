using SistemaViajesCompartidos.Enums;
using System;
using System.Web.Mvc;
using ViajesCompartidos.Handlers;
using static ViajesCompartidos.Controllers.BaseController;

namespace ViajesCompartidos.Controllers
{
    [RevisarUsuarioLogueado]
    [RevisarRoles(RolesEmpleadoFlag.ADMINISTRADOR)]
    public class ContactosController : Controller
    {
        public ActionResult Index()
        {
            return View("Index", ContactoHandler.GetContactos(false));
        }

        public ActionResult MarcarContactoRealizado(Guid ID)
        {
            ContactoHandler.MarcarContactoRealizado(ID);
            return this.Index();
        }
    }
}