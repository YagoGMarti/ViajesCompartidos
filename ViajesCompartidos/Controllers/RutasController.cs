using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Controllers
{
    public class RutasController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ObtenerRuta(Guid Empleado_ID)
        {
            var recorrido = RutaHandler.ObtenerRecorrido(Empleado_ID);

            return View(recorrido);
        }
    }
}