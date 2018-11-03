﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace OopRestaurant201810.Models
{
    public class Table
    {
        /// <summary>
        /// Elsődleges kulcs mező (PK)
        /// az EF Code First ebből megcsinálja a db identity mezőt
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Az asztal azonosítása
        /// </summary>
        [Required]      // 1. az adatbázisban kötelező kitölteni, 2. ViewModel-ként a flületen is kötelező kitölteni
        public string Name { get; set; }

        [Required]
        public Location Location { get; set; }

    }
}