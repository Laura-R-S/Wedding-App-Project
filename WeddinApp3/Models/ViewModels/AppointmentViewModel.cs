using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeddinApp3.Models.ViewModels
{
    public class AppointmentViewModel
    {

        public int? Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int Duration { get; set; }

        public string SupplierId { get; set; }

        public string CoupleId { get; set; }

        public bool SupplierApprovedApt { get; set; }

        public string AdminId { get; set; }

        public string SupplierName { get; set; }

        public string CoupleName { get; set; }

        public string AdminName { get; set; }

        public string IsForClient { get; set; }

        public string Role { get; set; }
    }
}
