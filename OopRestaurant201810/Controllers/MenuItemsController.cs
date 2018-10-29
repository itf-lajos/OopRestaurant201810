using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OopRestaurant201810;
using OopRestaurant201810.Models;

namespace OopRestaurant201810.Controllers
{
    public class MenuItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MenuItems
        public ActionResult Index()
        {
            return View(db.MenuItems.Include(x=>x.Category).ToList());
        }

        // GET: MenuItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // GET: MenuItems/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price")] MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(menuItem);
        }

        // GET: MenuItems/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }

            // Be kell töltenünk a menuItem Category property-jét, amit az Entity Framework magától nem tölt be
            db.Entry(menuItem)
                .Reference(x=>x.Category)
                .Load();
            // hogy be tudjuk állítani a lenyílót, megadjuk az aktuális kategória azonosítóját
            menuItem.CategoryId = menuItem.Category.Id;
            // letöltjük a Categories tábla tartalmát (db.Categories.Tolist())
            // megadjuk, hogy melyik mező azonosítja a sort, és adja azt az értéket, ami a végeredmény (Id)
            // megadjuk, hogy a lenyíló egyes soraiba melyik property értékei kerüljenek (Name)
            menuItem.AssignableCategories = new SelectList(db.Categories.OrderBy(x=>x.Name).ToList(), "Id", "Name");

            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,CategoryId")] MenuItem menuItem)    // Be kell engedni a kiválasztott azonosítót
        {
            if (ModelState.IsValid)
            {
                var category = db.Categories.Find(menuItem.CategoryId);

                // html formról jövő adatokat bemutatjuk az adatbázisnak
                db.MenuItems.Attach(menuItem);

                // az adatbázissal kapcsolatos dolgok eléréséhez kell az Entry
                var entry = db.Entry(menuItem);

                // ennek segítségével betöltjük a Category tábla adatait a menuItem.Category property-be
                entry.Reference(x => x.Category).Load();

                // majd felülírjuk azzal, ami bejött a html formon
                menuItem.Category = category;

                entry.State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem menuItem = db.MenuItems.Find(id);
            db.MenuItems.Remove(menuItem);
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
