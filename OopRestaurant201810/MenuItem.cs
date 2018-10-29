using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace OopRestaurant201810
{
    /// <summary>
    /// Az étlapon szereplő tételek közül egy tétel adatait tartalmazza
    /// </summary>
    public class MenuItem
    {
        //private int category;

        public MenuItem(string name, string description, int price, Category category)
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }

        public MenuItem()
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string  Description { get; set; }

        public int Price { get; set; }

        public Category Category { get; set; }

        /// <summary>
        /// A lenyíló lista kiválasztott elemének azonosítója részére
        /// </summary>
        [NotMapped]
        public int CategoryId { get; set; }

        /// <summary>
        /// A lenyíló lista tartalma: azonosító és megjelenítendő szöveg párok
        /// </summary>
        [NotMapped]
        public SelectList AssignableCategories { get; set; }

    }
}