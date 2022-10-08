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

namespace ViajesCompartidos.Controllers
{
    public class VehiculosController : BaseController
    {
        // GET: Vehiculos
        public ActionResult Index()
        {
            return View(db.Vehiculos.ToList());
        }

        // GET: Vehiculos/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiculoModel vehiculoModel = db.Vehiculos.Find(id);
            if (vehiculoModel == null)
            {
                return HttpNotFound();
            }
            return View(vehiculoModel);
        }

        // GET: Vehiculos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vehiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Patente,AsientosLibres,ComprobantePoliza,TipoImagenComprobantePoliza,FechaVencimientoComprobantePoliza,FechaVencimientoComprobantePolizaActiva,ComprobanteCarnetConducir,TipoImagenCarnetConducir,FechaVencimientoCarnetConducir,FechaVencimientoCarnetConducirActiva,FechaAlta,FechaBaja")] VehiculoModel vehiculoModel)
        {
            if (ModelState.IsValid)
            {
                vehiculoModel.Id = Guid.NewGuid();
                db.Vehiculos.Add(vehiculoModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehiculoModel);
        }

        // GET: Vehiculos/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiculoModel vehiculoModel = db.Vehiculos.Find(id);
            if (vehiculoModel == null)
            {
                return HttpNotFound();
            }
            return View(vehiculoModel);
        }

        // POST: Vehiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Patente,AsientosLibres,ComprobantePoliza,TipoImagenComprobantePoliza,FechaVencimientoComprobantePoliza,FechaVencimientoComprobantePolizaActiva,ComprobanteCarnetConducir,TipoImagenCarnetConducir,FechaVencimientoCarnetConducir,FechaVencimientoCarnetConducirActiva,FechaAlta,FechaBaja")] VehiculoModel vehiculoModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehiculoModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehiculoModel);
        }

        // GET: Vehiculos/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehiculoModel vehiculoModel = db.Vehiculos.Find(id);
            if (vehiculoModel == null)
            {
                return HttpNotFound();
            }
            return View(vehiculoModel);
        }

        // POST: Vehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VehiculoModel vehiculoModel = db.Vehiculos.Find(id);
            db.Vehiculos.Remove(vehiculoModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
