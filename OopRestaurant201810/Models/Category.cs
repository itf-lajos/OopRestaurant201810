using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OopRestaurant201810.Models
{
    public class Category
    {
        /// <summary>
        /// Paraméter nélküli konstruktor is kell, ha van paraméteres.
        /// Ezt a fordító legíártja mindig, ha nincs saját, paraméteres konstruktorom.
        /// </summary>
        public Category()
        {
        }

        /// <summary>
        /// Konstruktor, ez gyártja le az osztály új példányát.
        /// </summary>
        /// <param name="name"></param>
        public Category(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Az adatbázisba írás miatt kell Primary Key (PK) a táblába
        /// </summary>
        public int Id { get; set; }

        public string Name { get; set; }
    }
}