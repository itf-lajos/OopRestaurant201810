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

            context.Categories.AddOrUpdate(x=>x.Name, new Category(name: "Pizz�k"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Italok"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Desszertek"));
            context.SaveChanges();

            var pizzaCategory = context.Categories.Single(x => x.Name == "Pizz�k");
            context.MenuItems.AddOrUpdate(x=>x.Name, new MenuItem(name: "Kolb�szos" , description: "kolb�sz, sajt, sonka", price: 150, category: pizzaCategory));
            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem(name: "Hawaii", description: "mozarella, sonka, anan�sz", price: 300, category: pizzaCategory));
            context.SaveChanges();

            // Helyis�gek felt�lt�se
            context.Locations.AddOrUpdate(x => x.Name, new Location() { Name = "Terasz", IsOutDoor = true });        // Ha nincs megfelel� konstruktor, akkor ez haszn�lhat�
            context.Locations.AddOrUpdate(x => x.Name, new Location("Bels� terem", false));          // A konstruktort haszn�lja felt�lt�sre
            context.SaveChanges();

            var outdoorLocation = context.Locations
                                         .Where(x => x.Name == "Terasz") // Az �sszes sort visszaadja, amelyikre igaz. Ha nincs, �res lista, ha t�bb van, hosszabb lista.
                                         .FirstOrDefault();     // Ha �res a lista, NULL-t ad vissaz, ha van elem, akkor az els�t adja
            if (outdoorLocation == null)
            {
                throw new Exception("Nincs megfelel� Location az adatb�zisban (Terasz)");
            }

            // Asztalok felt�lt�se
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-1 (t)", Location = outdoorLocation });
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-2 (t)", Location = outdoorLocation});
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Jobb-1 (t)", Location = outdoorLocation });

            var indoorLocation = context.Locations
                .Where(x => x.Name == "Bels� terem")
                .FirstOrDefault();
            if (indoorLocation == null)
            {
                throw new Exception("Nincs megfelel� Location az adatb�zisban (Bels� terem)");
            }

            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-1 (b)", Location = indoorLocation });
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Jobb-1 (b)", Location = indoorLocation });

            context.SaveChanges();

        }
    }
}
