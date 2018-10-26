using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OopRestaurant201810
{
    public class Category
    {
        /// <summary>
        /// Az adatbázisba írás miatt kell Primary Key (PK) a táblába
        /// </summary>
        public int Id { get; set; }

        public string Name { get; set; }
    }
}