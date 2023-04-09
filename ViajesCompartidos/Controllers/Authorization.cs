using SistemaViajesCompartidos.Enums;
using System;
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
                if(filterContext.HttpContext.Session["SessionGUID"] == null)
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
                var roles = EmpleadoHandler.GetEmpleado(usuarioID).Roles;

                if (!rolesRequeridos.HasFlag(roles))
                    throw new UnauthorizedAccessException("No tiene permisos para acceder a esta sección");
            }
        }
    }
}