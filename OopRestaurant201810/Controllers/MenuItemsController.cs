﻿using System;
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

        /// <summary>
        /// Ez a függvény felelős a model betöltéséért minden megjelenítő (get) action esetén
        /// </summary>
        /// <param name="id">MenuItem azonosító, lehet null érték is</param>
        /// <param name="op">Művelet: Read or New</param>
        /// <returns></returns>
        private MenuItem ReadOrNewMenuItem(int? id, ReadOrNewOperation op)
        {
            MenuItem menuItem;

            switch (op)
            {
                case ReadOrNewOperation.Read:
                    // 1. Adatok betöltése az adatbázisból (Model)
                    menuItem = db.MenuItems.Find(id);
                    if (menuItem == null)
                    {
                        // return menuItem;
                        return null;
                    }
                    //return db.MenuItems.Find(id);
                    // Be kell töltenünk a menuItem Category property-jét, amit az Entity Framework magától nem tölt be
                    db.Entry(menuItem)
                        .Reference(x => x.Category)
                        .Load();
                    break;

                case ReadOrNewOperation.New:
                    // 1. Adat (model) példányosítása
                    menuItem = new MenuItem();
                    break;

                default:
                    throw new Exception($"Erre nem készültünk fel: {op}");
            }

            // 2. Megjelenítési adatok feltöltése (ViewModel)
            // hogy be tudjuk állítani a lenyílót, megadjuk az aktuális kategória azonosítóját
            if (menuItem.Category != null)
            {
                menuItem.CategoryId = menuItem.Category.Id;
            }
            // letöltjük a Categories tábla tartalmát (db.Categories.Tolist())
            // megadjuk, hogy melyik mező azonosítja a sort, és adja azt az értéket, ami a végeredmény (Id)
            // megadjuk, hogy a lenyíló egyes soraiba melyik property értékei kerüljenek (Name)
            LoadAssignableCategories(menuItem);
            // menuItem.AssignableCategories = new SelectList(db.Categories.OrderBy(x=>x.Name).ToList(), "Id", "Name");

            return menuItem;
        }

        private void CreateUpdateOrDeleteMenuItem(MenuItem menuItem, CreateUpdateOrDeleteOperation op)
        {
            switch (op)
            {
                case CreateUpdateOrDeleteOperation.Create:
                    var categoryCreate = db.Categories.Find(menuItem.CategoryId);
                    // todo: ezt a részt vissza kell integrálni
                    //if (category == null)
                    //{   // ha nincs ilyen kategória, akkor nem tudok mit tenni, visszaküldöm módosításra
                    //    LoadAssignableCategories(menuItem);
                    //    return View(menuItem);
                    //}

                    db.MenuItems.Attach(menuItem);
                    // mivel ez egy vadonatúj elem, ami még nem volt az adatbázisban. ezért nem tudunk property-t tölteni, mert nincs honnan,
                    // ezért az Edit-tel ellentétben a következő sor nem kell
                    // db.Entry(menuItem).Reference(x => x.Category).Load();
                    menuItem.Category = categoryCreate;

                    return;    // nincs szükség entry-re
                case CreateUpdateOrDeleteOperation.Update:
                    var categoryUpdate = db.Categories.Find(menuItem.CategoryId);
                    //todo: ezt a részt vissza kell integrálni
                    //if (category == null)
                    //{   // ha nincs ilyen kategória, akkor nem tudok mit tenni, visszaküldöm módosításra
                    //    LoadAssignableCategories(menuItem);
                    //    return View(menuItem);
                    //}

                    // html formról jövő adatokat bemutatjuk az adatbázisnak
                    db.MenuItems.Attach(menuItem);

                    // az adatbázissal kapcsolatos dolgok eléréséhez kell az Entry
                    var entry = db.Entry(menuItem);     // entry.state beállítása miatt kell

                    // ennek segítségével betöltjük a Category tábla adatait a menuItem.Category property-be
                    entry.Reference(x => x.Category).Load();

                    // majd felülírjuk azzal, ami bejött a html formon
                    menuItem.Category = categoryUpdate;
                    entry.State = EntityState.Modified;

                    return;
                case CreateUpdateOrDeleteOperation.Delete:
                    db.MenuItems.Remove(menuItem);

                    return;    // nincs entry
                default:
                    throw new Exception($"Erre nem vagyunk felkészülve {op}");
            }
        }

        private void LoadAssignableCategories(MenuItem menuItem)
        {
            menuItem.AssignableCategories = new SelectList(db.Categories.OrderBy(x => x.Name).ToList(), "Id", "Name");
        }

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
            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
            //MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            //LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        // GET: MenuItems/Create
        [Authorize(Roles = "Fopincer,Admin")]   // Csak a főpincér ás az Admin csoport tagjai használhatják
        // [Authorize(Users = "itf.lajos@gmail.com")]   // Csak a megadott felhasználó használhatja
        public ActionResult Create()
        {
            var menuItem = ReadOrNewMenuItem(null, ReadOrNewOperation.New);
            //var menuItem = new MenuItem();
            //LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Fopincer,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price,CategoryId")] MenuItem menuItem)
        {
            CreateUpdateOrDeleteMenuItem(menuItem, CreateUpdateOrDeleteOperation.Create);

            // Újra kell az adatok ellenőrzését végezni, hiszen megmódosítottam az egyes property-ket
            ModelState.Clear();     // előző törlése
            TryValidateModel(menuItem);     // újra validálás
            if (ModelState.IsValid)
            {
                //todo: ki kellene venni
                db.MenuItems.Add(menuItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadAssignableCategories(menuItem);
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
            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
            if (menuItem == null)
            {
                return HttpNotFound();
            }

            //// Be kell töltenünk a menuItem Category property-jét, amit az Entity Framework magától nem tölt be
            //db.Entry(menuItem)
            //    .Reference(x => x.Category)
            //    .Load();
            //// hogy be tudjuk állítani a lenyílót, megadjuk az aktuális kategória azonosítóját
            //menuItem.CategoryId = menuItem.Category.Id;
            //// letöltjük a Categories tábla tartalmát (db.Categories.Tolist())
            //// megadjuk, hogy melyik mező azonosítja a sort, és adja azt az értéket, ami a végeredmény (Id)
            //// megadjuk, hogy a lenyíló egyes soraiba melyik property értékei kerüljenek (Name)
            //LoadAssignableCategories(menuItem);
            //// menuItem.AssignableCategories = new SelectList(db.Categories.OrderBy(x=>x.Name).ToList(), "Id", "Name");

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
            CreateUpdateOrDeleteMenuItem(menuItem, CreateUpdateOrDeleteOperation.Update);

            // módosítás után az adatellenőrzést újra el kell végezni
            ModelState.Clear();
            TryValidateModel(menuItem);

            if (ModelState.IsValid)
            {
                //entry.State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            LoadAssignableCategories(menuItem);
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
            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
            //MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            //LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
            //MenuItem menuItem = db.MenuItems.Find(id);
            CreateUpdateOrDeleteMenuItem(menuItem, CreateUpdateOrDeleteOperation.Delete);
            //db.MenuItems.Remove(menuItem);
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
