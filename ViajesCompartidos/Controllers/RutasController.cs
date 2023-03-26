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
    public class RutasController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AlternativaRuta(Guid Empleado_ID, RecorridoModel recorrido)
        {
            EstrategiaRutaEnum estrategia;
            var intentos = recorridosActivos.Where(x => x.Value.Conductor_ID == Empleado_ID).Select(x => x.Value.ID);
            switch (intentos.Count())
            {
                case 1:
                    estrategia = EstrategiaRutaEnum.SoloCercanosDomicilio; break;
                case 2:
                    estrategia = EstrategiaRutaEnum.SoloMasCercano; break;
                case 3:
                    estrategia = EstrategiaRutaEnum.SinConductores; break;
                default: return this.SinPasajeros();
            }

            recorrido = RecorridoHandler.ObtenerRecorrido(Empleado_ID, estrategia);
            if (!recorridosActivos.ContainsKey(recorrido.ID))
            {
                recorridosActivos.Add(recorrido.ID, recorrido);
            }

            if (recorrido.Pasajeros.Any())
            {
                CargarMapa(recorrido);
                return this.ObtenerRuta(Empleado_ID, recorrido);
            }
            else
            {
                if (intentos.Count() > 3)
                    return this.SinPasajeros();
                else
                    return this.AlternativaRuta(Empleado_ID, recorrido);
            }
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

            if (recorrido.Pasajeros.Any())
            {
                if (!recorridosActivos.ContainsKey(recorrido.ID))
                {
                    recorridosActivos.Add(recorrido.ID, recorrido);
                }

                CargarMapa(recorrido);
                return View("ObtenerRuta", recorrido);
            }

            else
                return this.SinPasajeros();
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