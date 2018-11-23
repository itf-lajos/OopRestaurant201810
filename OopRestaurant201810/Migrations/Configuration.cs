using System.ComponentModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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

            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Pizz�k"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Italok"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Desszertek"));
            context.SaveChanges();

            var pizzaCategory = context.Categories.Single(x => x.Name == "Pizz�k");
            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem(name: "Kolb�szos", description: "kolb�sz, sajt, sonka", price: 150, category: pizzaCategory));
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
            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-2 (t)", Location = outdoorLocation });
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

            // Csoportok felvitele: Admin, Pincer, Fopincer
            AddRoleIfNotExists(context, "Admin");
            AddRoleIfNotExists(context, "Pincer");
            AddRoleIfNotExists(context, "Fopincer");

            // Felhaszn�l�k felvitele: admin@netacademia.hu, pincer@netacademia.hu, fopincer@netacademia.hu
            AddUserIfNotExists(context, "admin@netacademia.hu", "123456aA#", "Admin");
            AddUserIfNotExists(context, "pincer@netacademia.hu", "123456aA#", "Pincer");
            AddUserIfNotExists(context, "fopincer@netacademia.hu", "123456aA#", "Fopincer");

        }

        private static void AddUserIfNotExists(ApplicationDbContext context, string email, string pw, string role)
        {
            if (!context.Users.Any(x => x.Email == email))       // L�tezik-e ilyen felhaszn�l�
            {   // ha m�g nincs ilyen felhaszn�l�, l�trehozzuk
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser()
                {
                    Email = email,
                    UserName = email     // ez a kett� megegyezik, en�lk�l nem m�k�dik
                };
                manager.Create(user, pw);
                manager.AddToRole(user.Id, role);    // felhaszn�l�hoz hozz�rendelj�k a csoportot
            }
        }

        private static void AddRoleIfNotExists(ApplicationDbContext context, string roleName)
        {
            if (!context.Roles.Any(x => x.Name == roleName))     // L�tezik-e Admin csoport
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole(roleName);
                manager.Create(role);
            }
        }
    }
}
