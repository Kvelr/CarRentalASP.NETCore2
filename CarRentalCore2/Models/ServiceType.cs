using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalCore2.Models
{
    public class ServiceType
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
