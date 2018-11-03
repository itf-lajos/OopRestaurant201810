using System.ComponentModel.DataAnnotations;

namespace OopRestaurant201810.Models
{
    /// <summary>
    /// Ez az osztály az éttermen belül az egyes helyiségeket jelenti
    /// pl. belső terem, kerthelyiség
    /// </summary>
    public class Location
    {
        /// <summary>
        /// PK mező a táblához
        /// </summary>
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Jelzi, hogy a terem/terület kültéri-e
        /// true - kültéri, false - beltéri
        /// </summary>
        public bool IsOutDoor { get; set; }
    }
}