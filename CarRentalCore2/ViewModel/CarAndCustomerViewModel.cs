using CarRentalCore2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalCore2.ViewModel
{
    public class CarAndCustomerViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Car> Cars { get; set; }
    }
}
