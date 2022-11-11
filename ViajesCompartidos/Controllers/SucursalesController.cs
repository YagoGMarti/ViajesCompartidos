using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class SucursalesController : BaseController
    {
        [RevisarRoles(RolesEmpleadoFlag.ADMINISTRADOR | RolesEmpleadoFlag.RRHH)]
        public ActionResult Index(Guid? EmpresaID)
        {
            if(EmpresaID == null)
            {
                var empleadoID = ObtenerUsuario((Guid)Session["SessionGUID"]);
                var empresaID = EmpleadoHandler.GetEmpleado(empleadoID).EmpresaModel_ID;
                return View("Index", SucursalHandler.GetSucursalesPorEmpresa(empresaID));
            }
            else
            {
                return View("Index", SucursalHandler.GetSucursalesPorEmpresa(EmpresaID.Value));
            }
        }

        public ActionResult Detalles(Guid? ID)
        {
            ID = ObtenerUsuario((Guid)Session["SessionGUID"]);
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            SucursalModel sucursalModel = SucursalHandler.GetSucursal(ID.Value);
            
            if (sucursalModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Latitud = sucursalModel.Ubicacion.LatitudTexto;
            ViewBag.Longitud = sucursalModel.Ubicacion.LongitudTexto;

            return View(sucursalModel);
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(SucursalModel sucursalModel)
        {
            if (ModelState.IsValid)
            {
                var empleadoID = ObtenerUsuario((Guid)Session["SessionGUID"]);
                var empresaID = EmpleadoHandler.GetEmpleado(empleadoID).EmpresaModel_ID;
                SucursalHandler.CrearSucursal(sucursalModel, empresaID);
                return RedirectToAction("Detalles", new { ID = sucursalModel.ID });
            }

            return View(sucursalModel);
        }

        public ActionResult Editar(Guid? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            SucursalModel sucursalModel = SucursalHandler.GetSucursal(ID.Value);
            
            if (sucursalModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Latitud = sucursalModel.Ubicacion.LatitudTexto;
            ViewBag.Longitud = sucursalModel.Ubicacion.LongitudTexto;

            return View(sucursalModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(SucursalModel sucursalModel)
        {
            if (ModelState.IsValid)
            {
                SucursalHandler.EditarSucursal(sucursalModel);
                return RedirectToAction("Detalles", new { ID = sucursalModel.ID });
            }

            ViewBag.Latitud = sucursalModel.Ubicacion.LatitudTexto;
            ViewBag.Longitud = sucursalModel.Ubicacion.LongitudTexto;

            return View(sucursalModel);
        }

        public ActionResult Desactivar(Guid ID)
        {
            SucursalHandler.CambiarEstadoActivo(ID, false);
            return this.Index(null);
        }

        public ActionResult Activar(Guid ID)
        {
            SucursalHandler.CambiarEstadoActivo(ID, true);
            return this.Index(null);
        }
    }
}
