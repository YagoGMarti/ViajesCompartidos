using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Controllers
{
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