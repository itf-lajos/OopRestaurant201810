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

        private void CreateUpdateOrDeleteTable(Table table, CreateUpdateOrDeleteOperation op)
        {
            switch (op)
            {
                case CreateUpdateOrDeleteOperation.Create:
                    table.Location = db.Locations.Find(table.LocationId);
                    db.Tables.Add(table);
                    break;
                case CreateUpdateOrDeleteOperation.Update:
                    // 1. be kell mutatni a modellt az adatbázisnak
                    db.Tables.Attach(table);
                    // 2. be kell tölteni a hozzátartozó eredeti teremadatokat
                    db.Entry(table)                     // kérem az EF adatbázis elérő részét
                        .Reference(x => x.Location)     // kérem a csatlakozó táblák közül a Location-t
                        .Load();                        // onnan betöltöm az adazokat
                    // 3. módosítani kell az új teremadatot
                    table.Location = db.Locations.Find(table.LocationId);
                    // 4. jelezni kell, hogy változott, így a többi érték (Name, stb.) változást is figyelembe veszi az EF
                    db.Entry(table).State = EntityState.Modified;
                    break;
                case CreateUpdateOrDeleteOperation.Delete:
                    db.Tables.Remove(table);
                    break;
                default:
                    throw new Exception($"Erre a műveletre nam vagyunk felkészítve: {op}");
                    break;
            }
        }

        private Table ReadOrNewTable(int? id, ReadOrNewOperation op)
        {
            Table table;

            switch (op)
            {
                case ReadOrNewOperation.Read:
                    table = db.Tables.Find(id);
                    if (table == null)
                    {
                        return null;
                    }
                    // szólni kell az Entity Framework-nek, hogy töltse be az asztalhoz a termet is
                    db.Entry(table)
                        .Reference(x => x.Location)
                        .Load();
                    break;
                case ReadOrNewOperation.New:
                    table = new Table();
                    break;
                default:
                    throw new Exception($"Erre a műveletre nem vagyunk felkészítve: {op}");
            }

            // A lenyíló mező adatainak kitöltése
            // ?: feltételes null operátor, ha a kifejezés értéke null, akkor megáll és a végeredmény null,
            // ha pedig nem null, akkor folytatódik a kiértékelés és megy tovább
            //int? eredmeny;
            //if (table.Location == null)
            //{
            //    eredmeny = null;
            //}
            //else
            //{
            //    eredmeny = table.Location.Id;
            //}
            // ?? null operátor
            //if (eredmeny == null)
            //{
            //    eredmeny = 0;
            //}

            table.LocationId = table.Location?.Id ?? 0;
            LoadAssignableLocations(table);

            return table;
        }

        private void LoadAssignableLocations(Table table)
        {
            table.AssignableLocations = new SelectList(db.Locations.ToList(), "Id", "Name");
        }

        // GET: Tables
        public ActionResult Index()
        {
//            //lekérdezzük az adatbázisból az asztalok listáját és egy változóba mentjük
//            var tables = db.Tables
//                          .Include(x => x.Location)
////                          .OrderBy(x => x.Location.IsOutDoor)
//                          .ToList();

//            // elkészítjük a ViewModel
//            // 1. kelleni fog a termek listája
            var locations = db.Locations
                              .Include(x => x.Tables)
                              .ToList();
            //foreach (var location in locations)
            //{
            //    location.Tables = tables.Where(x => x.Location.Id == location.Id)
            //                            .ToList();      // Ekkor kéri le az adatokat ténylegesen
            //    //Ha nem készítenénk saját változót, akkor egyből az adatbázisból is tölthetnénk a tables listáját
            //    //location.Tables = db.Tables
            //    //                    .Include(x => x.Location)
            //    //                    .Where(x => x.Location.Id == location.Id)
            //    //                    .ToList();      // Ekkor kéri le az adatokat ténylegesen
            //}

            //// majd ezt elküldjük a nézethez
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

            Table table = ReadOrNewTable(id, ReadOrNewOperation.Read);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }


        // GET: Tables/Create
        public ActionResult Create()
        {
            var table = ReadOrNewTable(null, ReadOrNewOperation.New);
            return View(table);
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LocationId")] Table table)
        {
            CreateUpdateOrDeleteTable(table, CreateUpdateOrDeleteOperation.Create);
            ModelState.Clear();
            TryValidateModel(table);

            if (ModelState.IsValid)
            {
                db.Tables.Add(table);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadAssignableLocations(table);
            return View(table);
        }


        // GET: Tables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Table table = db.Tables.Find(id);
            Table table = ReadOrNewTable(id, ReadOrNewOperation.Read);
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
        public ActionResult Edit([Bind(Include = "Id,Name,LocationId")] Table table)
        {
            CreateUpdateOrDeleteTable(table, CreateUpdateOrDeleteOperation.Update);

            ModelState.Clear();
            TryValidateModel(table);

            if (ModelState.IsValid)
            {
                db.Entry(table).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadAssignableLocations(table);
            return View(table);
        }

        // GET: Tables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Table table = db.Tables.Find(id);
            Table table = ReadOrNewTable(id, ReadOrNewOperation.Read);
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
            CreateUpdateOrDeleteTable(table, CreateUpdateOrDeleteOperation.Delete);
            //db.Tables.Remove(table);
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
