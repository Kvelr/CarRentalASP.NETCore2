using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalCore2.Models
{
    public class IndvidualButtonPartial
    {
        public string ButtonType { get; set; }
        public string Action { get; set; }
        public string Glyph { get; set; }
        public string Text { get; set; }

        public int? ServiceID { get; set; }
        public string CustomerID { get; set; }
        public int? CarId { get; set; }

        public string ActionParameters
        {
            get
            {
                if (ServiceID != null && ServiceID != 0)
                {
                    return $"/{ServiceID}";
                }

                if (!string.IsNullOrWhiteSpace(CustomerID))
                {
                    return $"/{CustomerID}";
                }

                if (CarId != null && CarId != 0)
                {
                    return $"/{CarId}";
                }
                return "/";
            }
        }
    }
}
