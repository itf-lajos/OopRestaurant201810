using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OopRestaurant201810.Models
{
    /// <summary>
    /// Ez az osztály az éttermen belül az egyes helyiségeket jelenti
    /// pl. belső terem, kerthelyiség
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Alapértelmezett paraméter nélküli konstruktor
        /// </summary>
        public Location() {}

        /// <summary>
        /// Konstruktor, ami a használhatóságot javítja
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isOutDoor"></param>
        public Location( string name, bool isOutDoor)
        {
            Name = name;
            IsOutDoor = isOutDoor;
        }

        /// <summary>
        /// PK mező a táblához
        /// </summary>
        public int Id { get; set; }

        [Required]
        [Display(Name="Megnevezés")]
        public string Name { get; set; }

        /// <summary>
        /// Jelzi, hogy a terem/terület kültéri-e
        /// true - kültéri, false - beltéri
        /// </summary>
        [Display(Name="A szabadban van?")]
        public bool IsOutDoor { get; set; }

        //Felhasználva z Asztalból ide irányuló kapcsolatot, visszafelé ide azokat az asztalokat várom. amelyek ehhez a teremhez lettek rögzítve.
        //Mivel kivettük a NotMapped-et, ezért a Code First fennhatósága alá kerül.
        //1. a CodeFirst fennhatósága alá kerül
        //2. mivel ide irányba az aasztaltól mutat kapcsolat
        //3. ezért a teremhez visszafelé ki lehet gyűjteni a hozzá tartozó asztalokat
        //Ezzel a property-vel azt a létező kapcsolatot felhasználva elérhetővé tesszük az asztalokat, amik a teremhez tartoznak.
        //[NotMapped]
        public List<Table> Tables { get; set; }
    }
}