using SistemaViajesCompartidos.Context;
using SistemaViajesCompartidos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SistemaViajesCompartidos.Models.Temporal;

namespace ViajesCompartidos.Handlers
{
    internal class ReporteHandler
    {
        public static IEnumerable<ReporteAdminModel> GetReporteAdmin()
        {
            var empresas = EmpresaHandler.GetEmpresas();
            var empresasReporte = new List<ReporteAdminModel>();

            foreach (var empresa in empresas)
            {
                empresa.Sucursales = SucursalHandler.GetSucursalesPorEmpresa(empresa.ID);
                empresa.Empleados = EmpleadoHandler.GetEmpleados(empresa.ID, null);
                empresa.Recorridos = RecorridoHandler.GetRecorridosPorEmpresa(empresa.ID);

                empresasReporte.Add(new ReporteAdminModel()
                {
                    Empresa = empresa.RazonSocial,
                    EmpresaID = empresa.ID,
                    Sucursales = $"{empresa.Sucursales.Count()} ({empresa.Sucursales.Count(x => !x.Activo)})",
                    Recorridos = $"{empresa.Recorridos.Count()}",
                    Empleados = $"{empresa.Empleados.Count()} ({empresa.Empleados.Count(x => !x.Activo)})",
                    EmpleadosRecorrido = $"{empresa.Recorridos.SelectMany(x => x.RecorridoEmpleado).Count() + empresa.Recorridos.Count()}"
                });
            }

            return empresasReporte;
        }

        public static IEnumerable<ReporteEmpresaModel> GetReportePorEmpresa(Guid EmpresaID)
        {
            var sucursales = ViajesCompartidosContext.GetSucursalesPorEmpresa(EmpresaID);
            var sucursalesReporte = new List<ReporteEmpresaModel>();

            foreach (var sucursal in sucursales)
            {
                sucursal.Empleados = EmpleadoHandler.GetEmpleados(null, sucursal.ID);
                var recorridos = RecorridoHandler.GetRecorridosPorSucursal(sucursal.ID);
                sucursalesReporte.Add(new ReporteEmpresaModel()
                {
                    Sucursal = sucursal.Nombre,
                    SucursalID = sucursal.ID,
                    Activo = sucursal.Activo,
                    SucursalDireccion = sucursal.Ubicacion.UbicacionTexto,
                    Recorridos = $"{recorridos.Count()}",
                    Empleados = $"{sucursal.Empleados.Count()} ({sucursal.Empleados.Count(x => !x.Activo)})",
                    EmpleadosRecorrido = $"{recorridos.SelectMany(x => x.RecorridoEmpleado).Count() + recorridos.Count()}"
                });
            }

            return sucursalesReporte;
        }

        public static IEnumerable<ReporteSucursalModel> GetReportePorSucursal(Guid SucursalID)
        {
            var recorridos = ViajesCompartidosContext.GetRecorridosPorSucursal(SucursalID);
            var recorridosReporte = new List<ReporteSucursalModel>();

            foreach (var recorrido in recorridos)
            {
                recorridosReporte.Add(new ReporteSucursalModel()
                {
                    
                });
            }

            return recorridosReporte;
        }
        
    }
}