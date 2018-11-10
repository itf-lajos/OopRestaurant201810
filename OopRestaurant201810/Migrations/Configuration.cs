using System.ComponentModel;

namespace OopRestaurant201810.Migrations
{
    using OopRestaurant201810.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OopRestaurant201810.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OopRestaurant201810.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Categories.AddOrUpdate(x=>x.Name, new Category(name: "Pizzák"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Italok"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Desszertek"));
            context.SaveChanges();

            var pizzaCategory = context.Categories.Single(x => x.Name == "Pizzák");
            context.MenuItems.AddOrUpdate(x=>x.Name, new MenuItem(name: "Kolbászos" , description: "kolbász, sajt, sonka", price: 150, category: pizzaCategory));
            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem(name: "Hawaii", description: "mozarella, sonka, ananász", price: 300, category: pizzaCategory));
            context.SaveChanges();

            // Helyiségek feltöltése
            context.Locations.AddOrUpdate(x => x.Name, new Location() { Name = "Terasz", IsOutDoor = true });        // Ha nincs megfelelõ konstruktor, akkor ez használható
            context.Locations.AddOrUpdate(x => x.Name, new Location("Belsõ terem", false));          // A konstruktort használja feltöltésre
            context.SaveChanges();

            var outdoorLocation = context.Locations
                                         .Where(x => x.Name == "Terasz") // Az összes sort visszaadja, amelyikre igaz. Ha nincs, üres lista, ha több van, hosszabb lista.
                                         .FirstOrDefault();     // Ha üres a lista, NULL-t ad vissaz, ha van elem, akkor az elsõt adja
            if (outdoorLocation == null)
            {
                throw new Exception("Nincs megfelelõ Location az adatbázisban (Terasz)");
            }

            // Asztalok feltöltése
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-1 (t)", Location = outdoorLocation });
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-2 (t)", Location = outdoorLocation});
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Jobb-1 (t)", Location = outdoorLocation });

            var indoorLocation = context.Locations
                .Where(x => x.Name == "Belsõ terem")
                .FirstOrDefault();
            if (indoorLocation == null)
            {
                throw new Exception("Nincs megfelelõ Location az adatbázisban (Belsõ terem)");
            }

            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-1 (b)", Location = indoorLocation });
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Jobb-1 (b)", Location = indoorLocation });

            context.SaveChanges();

        }
    }
}
