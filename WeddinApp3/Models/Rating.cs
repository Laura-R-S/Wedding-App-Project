using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WeddinApp3.Models
{
    public class Rating
    {
        public int? Id  { get; set; }
        public string UserID { get; set; }
        public string VenID { get; set; }
        public int Scores { get; set; }
    }
}
