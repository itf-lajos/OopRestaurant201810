using System.ComponentModel;

namespace OopRestaurant201810.Migrations
{
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

            var pizzaCategory = context.Categories.Single(x => x.Name == "Pizz�k");
            context.MenuItems.AddOrUpdate(x=>x.Name, new MenuItem(name: "Kolb�szos" , description: "kolb�sz, sajt, sonka", price: 150, category: pizzaCategory));
            context.MenuItems.AddOrUpdate(x => x.Name, new MenuItem(name: "Hawaii", description: "mozarella, sonka, anan�sz", price: 300, category: pizzaCategory));

            //var category = new Category();
        }
    }
}
