using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WeddinApp3.Models.ViewModels
{
    public class RatingViewModel
    {
        [Required]
        [StringLength(10, ErrorMessage = "Name is too long.")]
        public int NoOfGuests { get; set; }
        public string County { get; set; }
        public string Genre { get; set; }
        public string Colour { get; set; }
        public string Flower { get; set; }
        public int Outdoor { get; set; }

    }
}
