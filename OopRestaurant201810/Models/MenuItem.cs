using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace OopRestaurant201810.Models
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

        [Required(ErrorMessage = "Ezt a mezőt kötelező kitölteni!")]
        [Display(Name = "Név")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Leírás")]
        [DataType(DataType.MultilineText)]
        public string  Description { get; set; }

        [Range(1, 100000)]
        [Display(Name = "Ár")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Kategória")]
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