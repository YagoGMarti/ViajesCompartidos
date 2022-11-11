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
    [RevisarRoles(RolesEmpleadoFlag.ADMINISTRADOR)]
    public class EmpresasController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index", EmpresaHandler.GetEmpresas());
        }

        public ActionResult Detalles(Guid? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EmpresaModel empresaModel = EmpresaHandler.GetEmpresa(ID.Value);
            
            if (empresaModel == null)
            {
                return HttpNotFound();
            }

            return View(empresaModel);
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(EmpresaModel empresaModel)
        {
            if (ModelState.IsValid)
            {
                EmpresaHandler.CrearEmpresa(empresaModel);
                SesionHandler.EnviarClave(empresaModel);
                return RedirectToAction("Detalles", new { ID = empresaModel.ID });
            }

            return View(empresaModel);
        }

        public ActionResult Editar(Guid? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpresaModel empresaModel = EmpresaHandler.GetEmpresa(ID.Value);
            
            if (empresaModel == null)
            {
                return HttpNotFound();
            }

            return View(empresaModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(EmpresaModel empresaModel)
        {
            if (ModelState.IsValid)
            {
                EmpresaHandler.EditarEmpresa(empresaModel);
                return RedirectToAction("Detalles", new { ID = empresaModel.ID });
            }

            return View(empresaModel);
        }

        public ActionResult Desactivar(Guid ID)
        {
            EmpresaHandler.CambiarEstadoActivo(ID, false);
            return this.Index();
        }

        public ActionResult Activar(Guid ID)
        {
            EmpresaHandler.CambiarEstadoActivo(ID, true);
            return this.Index();
        }
    }
}
