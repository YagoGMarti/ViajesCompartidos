using SistemaViajesCompartidos.Enums;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Controllers
{
    public partial class BaseController : Controller
    {
        internal class RevisarUsuarioLogueado : FilterAttribute, IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationContext filterContext)
            {
                if (filterContext.HttpContext.Session["SessionGUID"] != null) 
                {
                    var sessionID = (Guid)filterContext.HttpContext.Session["SessionGUID"];
                    var usuarioID = ObtenerUsuario(sessionID);

                    if (sessionID == null || usuarioID == null)
                    {
                        filterContext.Result = new RedirectResult("/Login");
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Login");
                }
            }
        }

        internal class RevisarRolesAttribute : FilterAttribute, IAuthorizationFilter
        {
            RolesEmpleadoFlag rolesRequeridos;

            public RevisarRolesAttribute(RolesEmpleadoFlag rolesAutorizados)
            {
                rolesRequeridos = rolesAutorizados;
            }

            public void OnAuthorization(AuthorizationContext filterContext)
            {
                var sessionID = (Guid)filterContext.HttpContext.Session["SessionGUID"];
                var usuarioID = ObtenerUsuario(sessionID);
                var empleado = EmpleadoHandler.GetEmpleado(usuarioID);
                var enabled = false;

                if (empleado != null)
                {
                    var rolesUsuario = empleado.Roles.ToString()
                    .Split(new[] { ", " }, StringSplitOptions.None);

                    foreach (var rol in rolesRequeridos.ToString().Split(new[] { ", " }, StringSplitOptions.None))
                    {
                        if (rolesUsuario.Any(x => x.Equals(rol)))
                            enabled = true;
                    }
                }
                else
                {
                    var empresa = EmpresaHandler.GetEmpresa(usuarioID);
                    if (empresa != null)
                    {
                        var roles = new string[1] { "RRHH" };

                        foreach (var rol in rolesRequeridos.ToString().Split(new[] { ", " }, StringSplitOptions.None))
                        {
                            if (roles.Any(x => x.Equals(rol)))
                                enabled = true;
                        }
                    }
                }
               
                //if (!rolesRequeridos.HasFlag(roles))
                if (!enabled)
                    throw new UnauthorizedAccessException("No tiene permisos para acceder a esta sección");
            }
        }
    }
}