using SistemaViajesCompartidos.Models;
using System.Web.Mvc;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index(bool contacto = false)
        {
            if (contacto)
            {
                TempData["contacto"] = "<script>alert('Solicitud de contacto enviada con éxito');</script>";
            }

            return View();
        }

        [HttpGet]
        public ActionResult Acerca()
        {
            ViewBag.Message = "Beneficios de utilizar el sistema de viajes compartidos.";

            return View();
        }

        [HttpGet]
        public ActionResult Contacto()
        {
            ViewBag.Message = "Realizar un primer contacto por alguno de los siguientes medios.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contacto(ContactoModel contactoModel)
        {
            if (ModelState.IsValid)
            {
                if(ContactoHandler.GrabarContacto(contactoModel))
                    return RedirectToAction("Index", new { contacto = true });
            }

            return View(contactoModel);
        }
    }
}