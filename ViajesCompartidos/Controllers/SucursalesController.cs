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
    public class SucursalesController : BaseController
    {
        // GET: Sucursales
        public ActionResult Index()
        {
            return View(db.Sucursales.ToList());
        }

        // GET: Sucursales/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SucursalModel sucursalModel = db.Sucursales.Find(id);
            if (sucursalModel == null)
            {
                return HttpNotFound();
            }
            return View(sucursalModel);
        }

        // GET: Sucursales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sucursales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,FechaAlta,FechaBaja")] SucursalModel sucursalModel)
        {
            if (ModelState.IsValid)
            {
                sucursalModel.Id = Guid.NewGuid();
                db.Sucursales.Add(sucursalModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sucursalModel);
        }

        // GET: Sucursales/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SucursalModel sucursalModel = db.Sucursales.Find(id);
            if (sucursalModel == null)
            {
                return HttpNotFound();
            }
            return View(sucursalModel);
        }

        // POST: Sucursales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,FechaAlta,FechaBaja")] SucursalModel sucursalModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sucursalModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sucursalModel);
        }

        // GET: Sucursales/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SucursalModel sucursalModel = db.Sucursales.Find(id);
            if (sucursalModel == null)
            {
                return HttpNotFound();
            }
            return View(sucursalModel);
        }

        // POST: Sucursales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SucursalModel sucursalModel = db.Sucursales.Find(id);
            db.Sucursales.Remove(sucursalModel);
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
