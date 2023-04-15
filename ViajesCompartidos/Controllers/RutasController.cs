using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Controllers
{
    [RevisarUsuarioLogueado]
    public class RutasController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AlternativaRuta(Guid Empleado_ID, int Estrategia, RecorridoModel recorrido)
        {
            EstrategiaRutaEnum estrategia = (EstrategiaRutaEnum)Estrategia;

            var intentos = recorridosActivos.Where(x => x.Value.Conductor_ID == Empleado_ID);
            if (intentos.Any(x => x.Value.EstrategiaRecorrido == estrategia))
            {
                recorrido = intentos.First(x => x.Value.EstrategiaRecorrido == estrategia).Value;
            }
            else
            {
                recorrido = RecorridoHandler.ObtenerRecorrido(Empleado_ID, estrategia);
                if (!recorridosActivos.ContainsKey(recorrido.ID))
                {
                    recorridosActivos.Add(recorrido.ID, recorrido);
                }
            }

            CargarMapa(recorrido);
            return this.ObtenerRuta(Empleado_ID, recorrido);
        }

        public ActionResult ObtenerRuta(Guid Empleado_ID, RecorridoModel recorrido)
        {
            if (recorrido.LatitudCentro + recorrido.LongitudCentro == 0)
            {
                var asociados = recorridosActivos.Where(x => x.Value.Conductor_ID == Empleado_ID).Select(x => x.Value.ID).ToList();
                foreach (Guid recID in asociados)
                {
                    recorridosActivos.Remove(recID);
                }

                recorrido = RecorridoHandler.ObtenerRecorrido(Empleado_ID, EstrategiaRutaEnum.Estandar);
            }

            if (!recorridosActivos.ContainsKey(recorrido.ID))
            {
                recorridosActivos.Add(recorrido.ID, recorrido);
            }

            if (recorrido.Pasajeros.Any())
            {
                CargarMapa(recorrido);
                ViewBag.SinPasajeros = null;
            }
            else
            {
                ViewBag.SinPasajeros = "No se encontraron pasajeros";
            }

            ViewBag.Estrategia = GetEstrategia(recorrido.EstrategiaRecorrido);

            return View("ObtenerRuta", recorrido);
        }

        private string GetEstrategia(EstrategiaRutaEnum estrategiaRecorrido)
        {
            string estrategia = string.Empty;
            switch (estrategiaRecorrido)
            {
                case EstrategiaRutaEnum.Estandar:
                    estrategia = "Estandar"; break;
                case EstrategiaRutaEnum.SoloCercanosDomicilio:
                    estrategia = "Cercanos al domicilio"; break;
                case EstrategiaRutaEnum.SinConductores:
                    estrategia = "Ignorar aquellos con auto"; break;
                case EstrategiaRutaEnum.SoloMasCercano:
                    estrategia = "Buscar por cercanía a la sucursal"; break;
                default: break;
            }

            return estrategia;
        }

        private void CargarMapa(RecorridoModel recorrido)
        {
            if (recorrido != null)
            {
                ViewBag.Ruta = true;
                ViewBag.OrigenLatitud = recorrido.Ubicaciones.First().LatitudTexto;
                ViewBag.OrigenLongitud = recorrido.Ubicaciones.First().LongitudTexto;
                ViewBag.DestinoLatitud = recorrido.Ubicaciones.Last().LatitudTexto;
                ViewBag.DestinoLongitud = recorrido.Ubicaciones.Last().LongitudTexto;
                ViewBag.CentroLatitud = recorrido.LongitudCentroString;
                ViewBag.CentroLongitud = recorrido.LongitudCentroString;

                List<string> ubicaciones = new List<string>();

                foreach (var ubicacion in recorrido.Ubicaciones)
                {
                    ubicaciones.Add(ubicacion.LatitudTexto);
                    ubicaciones.Add(ubicacion.LongitudTexto);
                }

                ViewBag.Ubicaciones = ubicaciones.ToArray();
            }
            else
            {
                ViewBag.Ruta = false;
            }
        }

        public ActionResult AceptarRuta(Guid recorrido_ID)
        {
            RecorridoModel recorrido = recorridosActivos[recorrido_ID];
            RecorridoHandler.RecorridoAceptado(recorrido);
            recorridosActivos = new Dictionary<Guid, RecorridoModel>();

            return RedirectToAction("Detalles", "Empleados", new { ID = recorrido.Conductor_ID });
        }

        public ActionResult RemoverUbicacion(Guid recorrido_ID, Guid removerUbicacion_ID)
        {
            RecorridoModel recorrido = recorridosActivos[recorrido_ID];
            recorrido = RecorridoHandler.RemoverUbicacion(recorrido, removerUbicacion_ID);

            recorridosActivos[recorrido.ID] = recorrido;

            return this.ObtenerRuta(recorrido.Conductor_ID, recorrido);
        }

        public ActionResult SinPasajeros()
        {
            return View("SinPasajeros");
        }


        protected ActionResult CancelarRuta(Guid recorrido_ID)
        {
            RecorridoHandler.CancelarRuta(recorrido_ID);

            var usuario_ID = ObtenerUsuario((Guid)Session["SessionGUID"]);
            return RedirectToAction("Detalles", "Empleados", new { ID = usuario_ID });
        }

        public ActionResult RemoverPasajero(Guid recorrido_ID, Guid Pasajero_ID)
        {
            RecorridoModel recorrido = RecorridoHandler.GetRecorrido(recorrido_ID);

            if (recorrido.Conductor_ID == Pasajero_ID)
                return this.CancelarRuta(recorrido_ID);

            if (recorrido.Pasajeros.All(x => x.ID == Pasajero_ID))
            {
                return this.CancelarRuta(recorrido_ID);
            }

            RecorridoHandler.RemoverPasajero(recorrido_ID, Pasajero_ID);

            var usuario_ID = ObtenerUsuario((Guid)Session["SessionGUID"]);
            return RedirectToAction("Detalles", "Empleados", new { ID = usuario_ID });
        }

    }
}