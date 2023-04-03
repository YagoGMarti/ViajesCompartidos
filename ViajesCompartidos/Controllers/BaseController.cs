using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Enums;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ViajesCompartidos.Handlers;
using ViajesCompartidos.Models.Temporal;

namespace ViajesCompartidos.Controllers
{
    public partial class BaseController : Controller
    {
        public static ViajesCompartidosContext _context = new ViajesCompartidosContext();
        public static Dictionary<Guid, UsuarioFlattened> sesionesActivas;
        public static Dictionary<Guid, RecorridoModel> recorridosActivos;

        public BaseController()
        {
            if (sesionesActivas == null)
                sesionesActivas = new Dictionary<Guid, UsuarioFlattened>();

            if (recorridosActivos == null)
                recorridosActivos = new Dictionary<Guid, RecorridoModel>();
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

        public static Guid ObtenerUsuario(Guid sessionGUID)
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

        public Guid ObtenerSesionID()
        {
            return (Guid)Session["SessionGUID"];
        }

        public void CerrarSesion(Guid sessionGUID)
        {
            sesionesActivas.Remove(sessionGUID);
        }
    }
}