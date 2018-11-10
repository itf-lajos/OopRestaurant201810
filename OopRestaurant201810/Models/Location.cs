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

        [NotMapped]
        public List<Table> Tables { get; internal set; }
    }
}