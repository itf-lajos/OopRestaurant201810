using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OopRestaurant201810.Models;

namespace OopRestaurant201810.Controllers
{
    public class TablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tables
        public ActionResult Index()
        {
            //lekérdezzük az adatbázisból az asztalok listáját és egy változóba mentjük
            var tables = db.Tables
                          .Include(x => x.Location)
//                          .OrderBy(x => x.Location.IsOutDoor)
                          .ToList();

            // elkészítjük a ViewModel
            // 1. kelleni fog a termek listája
            var locations = db.Locations
                              .ToList();
            foreach (var location in locations)
            {
                location.Tables = tables.Where(x => x.Location.Id == location.Id)
                                        .ToList();      // Ekkor kéri le az adatokat ténylegesen
                //Ha nem készítenénk saját változót, akkor egyből az adatbázisból is tölthetnénk a tables listáját
                //location.Tables = db.Tables
                //                    .Include(x => x.Location)
                //                    .Where(x => x.Location.Id == location.Id)
                //                    .ToList();      // Ekkor kéri le az adatokat ténylegesen
            }

            // majd ezt elküldjük a nézethez
            return View(locations);

            //return View(db.Tables
            //    .Include(x => x.Location)
            //    .OrderBy(x => x.Location.IsOutDoor)
            //    .ToList());
        }

        // GET: Tables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // GET: Tables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Table table)
        {
            if (ModelState.IsValid)
            {
                db.Tables.Add(table);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(table);
        }

        // GET: Tables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Table table)
        {
            if (ModelState.IsValid)
            {
                db.Entry(table).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(table);
        }

        // GET: Tables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Table table = db.Tables.Find(id);
            db.Tables.Remove(table);
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
