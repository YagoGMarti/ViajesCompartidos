﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Controllers
{
    public class EmpleadosController : BaseController
    {
        public ActionResult Index(Guid? EmpresaID, Guid? SucursalID)
        {
            if (EmpresaID.HasValue || SucursalID.HasValue)
            {
                ViewBag.EmpresaID = EmpresaID;
                ViewBag.SucursalID = SucursalID;
                return View("Index", EmpleadoHandler.GetEmpleados(EmpresaID, SucursalID));
            }

            return View("Index", EmpleadoHandler.GetEmpleados(ObtenerEmpresa((Guid)Session["SessionGUID"]), SucursalID));
        }

        public ActionResult Detalles(Guid? ID)
        {
            if (ID == null)
            {
                ID = ObtenerUsuario((Guid)Session["SessionGUID"]);
                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

            EmpleadoModel empleadoModel = EmpleadoHandler.GetEmpleado(ID.Value);

            if (empleadoModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Sucursales = GetSucursales(empleadoModel.EmpresaModel_ID, empleadoModel.SucursalModel_ID);
            ViewBag.Latitud = empleadoModel.Ubicacion.LatitudTexto;
            ViewBag.Longitud = empleadoModel.Ubicacion.LongitudTexto;

            return View(empleadoModel);
        }

        public ActionResult Crear()
        {
            var empresaID = ObtenerEmpresa((Guid)Session["SessionGUID"]);
            ViewBag.Sucursales = GetSucursales(empresaID, null);
            ViewBag.Ingreso = 7;
            ViewBag.Salida = 16;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(EmpleadoModel empleadoModel)
        {
            var empresaID = ObtenerEmpresa((Guid)Session["SessionGUID"]);

            empleadoModel.HorarioIngreso = new TimeSpan(int.Parse(empleadoModel.HorarioIngresoTexto), 0, 0);
            empleadoModel.HorarioSalida = new TimeSpan(int.Parse(empleadoModel.HorarioSalidaTexto), 0, 0);

            if (empleadoModel.HorarioIngreso.Hours >= empleadoModel.HorarioSalida.Hours)
            {
                ViewBag.HorariosInvalidos = "El horario de ingreso debe ser anterior al de salida";
            }
            else if (ModelState.IsValid)
            {
                EmpleadoHandler.CrearEmpleado(empleadoModel, empresaID);
                return RedirectToAction("Detalles", new { ID = empleadoModel.ID });
            }

            ViewBag.Sucursales = GetSucursales(empresaID, empleadoModel.SucursalModel_ID);
            ViewBag.Ingreso = empleadoModel.HorarioIngreso.Hours.ToString();
            ViewBag.Salida = empleadoModel.HorarioSalida.Hours.ToString();

            return View(empleadoModel);
        }

        public ActionResult Editar(Guid? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EmpleadoModel empleadoModel = EmpleadoHandler.GetEmpleado(ID.Value);
            
            
            ModelState.Clear();

            if (empleadoModel == null)
            {
                return HttpNotFound();
            }

            var empresaID = ObtenerEmpresa((Guid)Session["SessionGUID"]);
            ViewBag.Sucursales = GetSucursales(empresaID, empleadoModel.SucursalModel_ID);
            ViewBag.Ingreso = empleadoModel.HorarioIngreso.Hours;
            ViewBag.Salida = empleadoModel.HorarioSalida.Hours;
            ViewBag.Latitud = empleadoModel.Ubicacion.LatitudTexto;
            ViewBag.Longitud = empleadoModel.Ubicacion.LongitudTexto;

            return View(empleadoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(EmpleadoModel empleadoModel)
        {
            empleadoModel.HorarioIngreso = new TimeSpan(int.Parse(empleadoModel.HorarioIngresoTexto), 0, 0);
            empleadoModel.HorarioSalida = new TimeSpan(int.Parse(empleadoModel.HorarioSalidaTexto), 0, 0);
            
            if(empleadoModel.HorarioIngreso.Hours >= empleadoModel.HorarioSalida.Hours)
            {
                ViewBag.HorariosInvalidos = "El horario de ingreso debe ser anterior al de salida";
            }
            else if (ModelState.IsValid)
            {
                EmpleadoHandler.EditarEmpleado(empleadoModel);
                return RedirectToAction("Detalles", new { ID = empleadoModel.ID });
            }

            var empresaID = ObtenerEmpresa((Guid)Session["SessionGUID"]);
            ViewBag.Sucursales = GetSucursales(empresaID, empleadoModel.SucursalModel_ID);
            ViewBag.Ingreso = empleadoModel.HorarioIngreso.Hours.ToString();
            ViewBag.Salida = empleadoModel.HorarioSalida.Hours.ToString();
            ViewBag.Latitud = empleadoModel.Ubicacion.LatitudTexto;
            ViewBag.Longitud = empleadoModel.Ubicacion.LongitudTexto;

            return View(empleadoModel);
        }

        public ActionResult RestablecerClave(Guid ID, Guid? EmpresaID, Guid? SucursalID)
        {
            SesionHandler.RestablecerClave(ID);
            return this.Index(EmpresaID, SucursalID);
        }

        public ActionResult Desactivar(Guid ID, Guid? EmpresaID, Guid? SucursalID)
        {
            EmpleadoHandler.CambiarEstadoActivo(ID, false);
            return this.Index(EmpresaID, SucursalID);
        }

        public ActionResult Activar(Guid ID, Guid? EmpresaID, Guid? SucursalID)
        {
            EmpleadoHandler.CambiarEstadoActivo(ID, true);
            return this.Index(EmpresaID, SucursalID);
        }

        private SelectList GetSucursales(Guid EmpresaID, Guid? SucursalID)
        {
            var sucursales = new SelectList(SucursalHandler.GetSucursalesPorEmpresa(EmpresaID), "ID", "Nombre");
            if (SucursalID.HasValue)
                foreach (var Sucursal in sucursales)
                {
                    if (Sucursal.Value == SucursalID.ToString())
                        Sucursal.Selected = true;
                }
            return sucursales;
        }
    }
}
