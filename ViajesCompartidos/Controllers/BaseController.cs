using SistemaViajesCompartidos.Context;
using System.Web.Mvc;

namespace ViajesCompartidos.Controllers
{
    public class BaseController : Controller
    {
        public ViajesCompartidosContext db = new ViajesCompartidosContext();
    }
}