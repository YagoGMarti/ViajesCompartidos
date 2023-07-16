using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViajesCompartidos.Handlers;
using SistemaViajesCompartidos.Temporal;

namespace ViajesCompartidos.Controllers
{
    public class LoginController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(InicioSesion inicioSesion)
        {
            EmpleadoModel empleado = SesionHandler.IniciarSesion(inicioSesion);

            if (empleado != null)
            {
                var SessionGUID = Guid.NewGuid();
                ActualizarSesiones(SessionGUID, empleado.ID, empleado.EmpresaModel_ID);
                ViewBag.FailedLoggin = null;

                Session.Remove("Usuario");
                Session.Add("Usuario", empleado.Nombre);

                Session.Remove("Roles");
                Session.Add("Roles", (int)empleado.Roles);

                Session.Remove("Empleado_ID");
                Session.Add("Empleado_ID", empleado.ID.ToString());

                Session.Remove("SessionGUID");
                Session.Add("SessionGUID", SessionGUID);

                return RedireccionarPorRol();
            }

            EmpresaModel empresa = SesionHandler.IniciarSesionEmpresa(inicioSesion);
            if (empresa != null)
            {
                var SessionGUID = Guid.NewGuid();
                ActualizarSesiones(SessionGUID, empresa.ID, empresa.ID);
                ViewBag.FailedLoggin = null;

                Session.Remove("Usuario");
                Session.Add("Usuario", empresa.Nombre);

                Session.Remove("Roles");
                Session.Add("Roles", RolesEmpleadoFlag.RRHH | RolesEmpleadoFlag.CORREOINSTITUCIONAL);

                Session.Remove("Empleado_ID");
                Session.Add("Empleado_ID", empresa.ID.ToString());
                Session.Remove("Empresa_ID");
                Session.Add("Empresa_ID", empresa.ID.ToString());

                Session.Remove("SessionGUID");
                Session.Add("SessionGUID", SessionGUID);

                return RedirectToAction("Index", "Home");
            }


            ViewBag.FailedLoggin = "No se pudo iniciar sesión.";
            return View();
        }

        private ActionResult RedireccionarPorRol()
        {
            var usuarioID = ObtenerUsuario((Guid)Session["SessionGUID"]);
            RolesEmpleadoFlag rolesEmpleado = SesionHandler.GetRolEmpleado(usuarioID);

            switch (rolesEmpleado)
            {
                case RolesEmpleadoFlag.ADMINISTRADOR: return RedirectToAction("Index", "Home");
                case RolesEmpleadoFlag.RRHH: return RedirectToAction("Index", "Home");
                case RolesEmpleadoFlag.EMPLEADO: return RedirectToAction("Index", "Home");
                default: return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CambiarClave()
        {
            var inicioSesion = new InicioSesion();
            inicioSesion.Email = "placeholder@mail.com";

            return View(inicioSesion);
        }

        [HttpPost]
        public ActionResult CambiarClave(InicioSesion inicioSesion)
        {
            ViewBag.ClavesNoCoinciden = null;
            if (ModelState.IsValid)
            {
                if (inicioSesion.ConfirmarClaveNueva == inicioSesion.ClaveNueva)
                {
                    var SessionID = ObtenerUsuario((Guid)Session["SessionGUID"]);
                    var empleado = EmpleadoHandler.GetEmpleado(SessionID);
                    if (empleado != null)
                    {
                        if (EncriptadoHandler.Encriptar(inicioSesion.Clave).SequenceEqual(empleado.ClaveEncriptada))
                        {
                            SesionHandler.CambiarClave(empleado.ID, inicioSesion.ClaveNueva);
                            return RedireccionarPorRol();
                        }

                        ViewBag.ClavesNoCoinciden = "La clave provista es incorrecta.";
                    }

                    var empresa = EmpresaHandler.GetEmpresa(SessionID);
                    if (empresa != null)
                    {
                        if (EncriptadoHandler.Encriptar(inicioSesion.Clave) == empresa.ClaveEncriptada)
                        {
                            SesionHandler.CambiarClaveEmpresa(empresa.ID, inicioSesion.ClaveNueva);
                            return RedirectToAction("Index", "Home");
                        }

                        ViewBag.ClavesNoCoinciden = "La clave provista es incorrecta.";
                    }
                }
                else
                {
                    ViewBag.ClavesNoCoinciden = "Las claves no coinciden";
                }
            }

            return View(inicioSesion);
        }

        public ActionResult Logout()
        {
            Session.Remove("Usuario");
            if (Session["SessionGUID"] != null)
            {
                CerrarSesion((Guid)Session["SessionGUID"]);
                Session.Remove("SessionGUID");
            }

            return RedirectToAction("Index");
        }
    }
}