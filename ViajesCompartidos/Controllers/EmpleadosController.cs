using System;
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
                ViewBag.SucursalID = SucursalID;
                var empleados = EmpleadoHandler.GetEmpleados(EmpresaID, SucursalID);
                if (EmpresaID.HasValue)
                {
                    ViewBag.EmpresaID = EmpresaID;
                }
                else
                {
                    var empresa_ID = EmpresaHandler.GetEmpresaBySucursal(SucursalID.Value);
                    ViewBag.EmpresaID = empresa_ID;
                }
                return View("Index", empleados);
            }
            else
            {
                var empresa_ID = ObtenerEmpresa((Guid)Session["SessionGUID"]);
                ViewBag.EmpresaID = empresa_ID;
                return View("Index", EmpleadoHandler.GetEmpleados(empresa_ID, SucursalID));

            }

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

            CargarMapa(empleadoModel.Recorrido);


            return View(empleadoModel);
        }

        private void CargarMapa(RecorridoModel recorrido)
        {
            if (recorrido != null && recorrido.EstadoRecorrido == SistemaViajesCompartidos.Enums.EstadoRecorridoEnum.ACEPTADO)
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

        public ActionResult Crear(Guid EmpresaModel_ID)
        {
            if (EmpresaModel_ID == null)
            {
                EmpresaModel_ID = ObtenerEmpresa((Guid)Session["SessionGUID"]);
            }
            ViewBag.Sucursales = GetSucursales(EmpresaModel_ID, null);
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

            if (empleadoModel.HorarioIngreso.Hours >= empleadoModel.HorarioSalida.Hours)
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
