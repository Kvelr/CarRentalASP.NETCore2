using CarRentalCore2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalCore2.ViewModel
{
    public class CarAndServicesViewModel
    {

        public int CarId { get; set; }
        public string Vin { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Style { get; set; }
        public int Year { get; set; }
        public string UserId { get; set; }

        public Service Service { get; set; }

        public IEnumerable<Service> PastServices { get; set; }

        public List<ServiceType> ServiceTypes { get; set; }
    }
}
