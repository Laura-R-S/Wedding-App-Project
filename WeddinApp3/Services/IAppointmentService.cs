    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeddinApp3.Models.ViewModels;

namespace WeddinApp3.Services
{
    public interface IAppointmentService 
    {

        public List<SupplierViewModel> GetSupplierList(string userId);

        public List<CoupleViewModel> GetCoupleList();

        public Task<int> AddUpdate(AppointmentViewModel model, string loginId);

        public List<AppointmentViewModel> SupplierBookings(string supplierId, string role);

        public List<AppointmentViewModel> CoupleBookings(string coupleId);

        public AppointmentViewModel GetById(int id, string role);
        public Task<int> Delete(int id);

        public Task<int> ConfirmEvent(int id);

    }
}
