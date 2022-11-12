using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Controllers
{
    public class VehiculosController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index", VehiculoHandler.GetVehiculos());
        }

        public ActionResult Detalles(Guid? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiculoModel vehiculoModel = VehiculoHandler.GetVehiculo(ID.Value);

            EmpleadoModel empleadoModel = EmpleadoHandler.GetEmpleado(vehiculoModel.Empleado_ID);
            ViewBag.empleado = $"{empleadoModel.Nombre} - {empleadoModel.CorreoElectronico}";

            if (vehiculoModel == null)
            {
                return HttpNotFound();
            }
            return View(vehiculoModel);
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(VehiculoModel vehiculoModel)
        {
            var Empleado_ID = ObtenerUsuario((Guid)Session["SessionGUID"]);

            if (ModelState.IsValid)
            {
                vehiculoModel.Empleado_ID = Empleado_ID;
                _context.Vehiculos.Add(vehiculoModel);
                _context.SaveChanges();
                return RedirectToAction("Detalles", new { ID = vehiculoModel.ID });
            }

            return View(vehiculoModel);
        }

        public ActionResult Editar(Guid? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiculoModel vehiculoModel = VehiculoHandler.GetVehiculo(ID.Value);

            EmpleadoModel empleadoModel = EmpleadoHandler.GetEmpleado(vehiculoModel.Empleado_ID);
            ViewBag.empleado = $"{empleadoModel.Nombre} - {empleadoModel.CorreoElectronico}";

            if (vehiculoModel == null)
            {
                return HttpNotFound();
            }
            return View(vehiculoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(VehiculoModel vehiculoModel)
        {
            if (ModelState.IsValid)
            {
                VehiculoHandler.EditarVehiculo(vehiculoModel);
                return RedirectToAction("Detalles", new { ID = vehiculoModel.ID });
            }

            return View(vehiculoModel);
        }

        public ActionResult Validar(Guid? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiculoModel vehiculoModel = VehiculoHandler.GetVehiculo(ID.Value);

            EmpleadoModel empleadoModel = EmpleadoHandler.GetEmpleado(vehiculoModel.Empleado_ID);
            ViewBag.empleado = $"{empleadoModel.Nombre} - {empleadoModel.CorreoElectronico}";

            if (vehiculoModel == null)
            {
                return HttpNotFound();
            }
            return View(vehiculoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validar(VehiculoModel vehiculoModel)
        {
            if (ModelState.IsValid)
            {
                VehiculoHandler.ValidarVehiculo(vehiculoModel);
                return RedirectToAction("Detalles", new { ID = vehiculoModel.ID });
            }
            return View(vehiculoModel);
        }

        public FileContentResult AdjuntoPoliza(Guid ID)
        {
            VehiculoModel vehiculo = VehiculoHandler.GetVehiculo(ID);            
            return File(vehiculo.AdjuntoComprobantePoliza, vehiculo.TipoImagenComprobantePoliza, vehiculo.NombreArchivoComprobantePoliza);
        }

        public FileContentResult AdjuntoCarnet(Guid ID)
        {
            VehiculoModel vehiculo = VehiculoHandler.GetVehiculo(ID);
            return File(vehiculo.AdjuntoCarnetConducir, vehiculo.TipoImagenCarnetConducir, vehiculo.NombreArchivoCarnetConducir);
        }
    }
}
