using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Enums;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ViajesCompartidos.Handlers;

namespace ViajesCompartidos.Controllers
{
    public class BaseController : Controller
    {
        public static ViajesCompartidosContext _context = new ViajesCompartidosContext();
        public static Dictionary<Guid, UsuarioFlattened> sesionesActivas;

        public BaseController()
        {
            if (sesionesActivas == null)
                sesionesActivas = new Dictionary<Guid, UsuarioFlattened>();
        }

        public void ActualizarSesiones(Guid sessionGUID, Guid usuarioID, Guid empresaID)
        {
            if (!sesionesActivas.ContainsKey(sessionGUID))
            {
                sesionesActivas.Add(sessionGUID, new UsuarioFlattened(usuarioID, empresaID));
            }
            else
            {
                sesionesActivas[sessionGUID] = new UsuarioFlattened(usuarioID, empresaID);
            }
        }

        public Guid ObtenerUsuario(Guid sessionGUID)
        {
            if (sesionesActivas.ContainsKey(sessionGUID))
            {
                return sesionesActivas[sessionGUID].UsuarioID;
            }

            return new Guid();
        }

        public Guid ObtenerEmpresa(Guid sessionGUID)
        {
            if (sesionesActivas.ContainsKey(sessionGUID))
            {
                return sesionesActivas[sessionGUID].EmpresaID;
            }

            return new Guid();
        }

        public void CerrarSesion(Guid sessionGUID)
        {
            sesionesActivas.Remove(sessionGUID);
        }

        internal class RevisarRolesAttribute : Attribute
        {
            public RevisarRolesAttribute(RolesEmpleadoFlag rolesAutorizados)
            {
                var asd = rolesAutorizados;
            }
        }

        public class UsuarioFlattened {
            public UsuarioFlattened(Guid usuarioID, Guid empresaID)
            {
                UsuarioID = usuarioID;
                EmpresaID = empresaID;
            }
            
            public Guid UsuarioID { get; set; }
            public Guid EmpresaID { get; set; }
        }
    }
}