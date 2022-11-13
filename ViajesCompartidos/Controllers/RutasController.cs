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

        public ActionResult ObtenerRuta(Guid Empleado_ID, RecorridoModel recorrido)
        {
            if (recorrido.LatitudCentro + recorrido.LongitudCentro == 0)
            {
                recorrido = RecorridoHandler.ObtenerRecorrido(Empleado_ID);
            }

            if (recorrido.Pasajeros.Any())
            {
                if (!recorridosActivos.ContainsKey(recorrido.ID))
                {
                    recorridosActivos.Add(recorrido.ID, recorrido);
                }

                return View("ObtenerRuta", recorrido);
            }

            else
                return this.SinPasajeros();
        }

        public ActionResult AceptarRuta(Guid recorrido_ID)
        {
            RecorridoModel recorrido = recorridosActivos[recorrido_ID];
            RecorridoHandler.RecorridoAceptado(recorrido);

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

    //        <a class="btn btn-default buttonOffset15 botonCentrado" href="@Url.Action("CancelarRuta", "Rutas", new { Recorrido_ID = Model.Recorrido.ID } )">Cancelar &raquo;</a>
    //                            @Html.ActionLink("Remover", "RemoverPasajero", "Rutas", new { Recorrido_ID = Model.Recorrido.ID, Pasajero_ID = item.ID
    //}, new { })

    }
}