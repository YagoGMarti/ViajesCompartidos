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
    public class EmpresasController : BaseController
    {
        // GET: EmpresasController
        public ActionResult Index()
        {
            return View(db.Empresas.ToList());
        }

        // GET: EmpresasController/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpresaModel empresaModel = db.Empresas.Find(id);
            if (empresaModel == null)
            {
                return HttpNotFound();
            }
            return View(empresaModel);
        }

        // GET: EmpresasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpresasController/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,RazonSocial,CUIT,TipoEmpresa,Logo,FormatoImagenLogo,FechaAlta,FechaBaja")] EmpresaModel empresaModel)
        {
            if (ModelState.IsValid)
            {
                empresaModel.Id = Guid.NewGuid();
                db.Empresas.Add(empresaModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empresaModel);
        }

        // GET: EmpresasController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpresaModel empresaModel = db.Empresas.Find(id);
            if (empresaModel == null)
            {
                return HttpNotFound();
            }
            return View(empresaModel);
        }

        // POST: EmpresasController/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,RazonSocial,CUIT,TipoEmpresa,Logo,FormatoImagenLogo,FechaAlta,FechaBaja")] EmpresaModel empresaModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empresaModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empresaModel);
        }

        // GET: EmpresasController/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpresaModel empresaModel = db.Empresas.Find(id);
            if (empresaModel == null)
            {
                return HttpNotFound();
            }
            return View(empresaModel);
        }

        // POST: EmpresasController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            EmpresaModel empresaModel = db.Empresas.Find(id);
            db.Empresas.Remove(empresaModel);
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
