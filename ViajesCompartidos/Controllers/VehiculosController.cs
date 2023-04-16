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
using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Controllers
{
    [RevisarUsuarioLogueado]
    public class VehiculosController : BaseController
    {
        [RevisarRoles(RolesEmpleadoFlag.RRHH)]
        public ActionResult Index()
        {
            var EmpresaModel_ID = ObtenerEmpresa((Guid)Session["SessionGUID"]);
            return View("Index", VehiculoHandler.GetVehiculosPorEmpresa(EmpresaModel_ID));
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
            vehiculoModel.Empleado_ID = Empleado_ID;
            var Empresa_ID = EmpleadoHandler.GetEmpleado(Empleado_ID).EmpresaModel_ID;
            vehiculoModel.EmpresaModel_ID = Empresa_ID;

            try
            {
                VehiculoHandler.CrearVehiculo(vehiculoModel);
                return RedirectToAction("Editar", new { ID = vehiculoModel.ID });
            }
            catch (Exception)
            {
                return View(vehiculoModel);
            }
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
            ViewBag.asientosInsuficientes = false;
            EmpleadoModel empleadoModel = EmpleadoHandler.GetEmpleado(vehiculoModel.Empleado_ID);

            if (empleadoModel.Recorrido != null
                && vehiculoModel.Empleado_ID == empleadoModel.Recorrido.Conductor_ID
                && empleadoModel.Recorrido.Pasajeros.Count() > vehiculoModel.AsientosLibres)
            {
                ViewBag.asientosInsuficientes = true;
                ViewBag.empleado = $"{empleadoModel.Nombre} - {empleadoModel.CorreoElectronico}";
                return View(vehiculoModel);
            }

            if (ModelState.IsValid)
            {
                VehiculoHandler.EditarVehiculo(vehiculoModel);
                return RedirectToAction("Detalles", new { ID = vehiculoModel.ID });
            }

            ViewBag.empleado = $"{empleadoModel.Nombre} - {empleadoModel.CorreoElectronico}";
            return View(vehiculoModel);
        }

        [RevisarRoles(RolesEmpleadoFlag.RRHH)]
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
        [RevisarRoles(RolesEmpleadoFlag.RRHH)]
        public ActionResult Validar(VehiculoModel vehiculoModel)
        {
            bool fechasInconclusas = false;
            ViewBag.FechaVencimientoPoliza = null;
            ViewBag.FechaVencimientoCarnet = null;

            if (vehiculoModel.ComprobantePolizaValidado && vehiculoModel.FechaVencimientoComprobantePoliza == null)
            {
                ViewBag.FechaVencimientoPoliza = "Debe indicar una fecha de vencimiento";
                fechasInconclusas = true;
            }

            if (vehiculoModel.ComprobanteCarnetValidado && vehiculoModel.FechaVencimientoCarnetConducir == null)
            {
                ViewBag.FechaVencimientoCarnet = "Debe indicar una fecha de vencimiento";
                fechasInconclusas = true;
            }

            if (ModelState.IsValid && ! fechasInconclusas)
            {
                VehiculoHandler.ValidarVehiculo(vehiculoModel);
                return RedirectToAction("Detalles", new { ID = vehiculoModel.ID });
            }
            else
            {
                EmpleadoModel empleadoModel = EmpleadoHandler.GetEmpleado(vehiculoModel.Empleado_ID);
                ViewBag.empleado = $"{empleadoModel.Nombre} - {empleadoModel.CorreoElectronico}";
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
