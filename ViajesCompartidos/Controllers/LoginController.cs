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
            return View();
        }

        [HttpPost]
        public ActionResult CambiarClave(InicioSesion inicioSesion)
        {
            ViewBag.ClavesNoCoinciden = null;
            var empleadoID = ObtenerUsuario((Guid)Session["SessionGUID"]);

            if (inicioSesion.Clave == inicioSesion.ClaveNueva)
            {
                SesionHandler.CambiarClave(empleadoID, inicioSesion.Clave);
                return RedireccionarPorRol();
            }
            else
            {
                ViewBag.ClavesNoCoinciden = "Las claves no coinciden";
            }

            return View();
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