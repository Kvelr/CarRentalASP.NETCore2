using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalCore2.Models
{
    public class Service
    {

        public int Id { get; set; }
        public double Miles { get; set; }
        public double Price { get; set; }

        [Required]
        public string Details { get; set; }

        [DisplayFormat(DataFormatString ="{0:d}")]
        public DateTime DateAdded { get; set; }

        public int carId { get; set; }

        public int  ServiceTypeId { get; set; }

        [ForeignKey("carId")]
        public Car Car { get; set; }

        [ForeignKey("ServiceTypeId")]
        public ServiceType ServiceType { get; set; }
    }
}
