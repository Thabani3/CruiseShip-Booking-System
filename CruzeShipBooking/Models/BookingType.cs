using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CruzeShipBooking.Models
{
    public class BookingType
    {
        [Key]
        public int BookingTypeId { get; set; }
        [DisplayName("Booking Type Name")]
        public string BookingTypeName { get; set; }
        [DisplayName("Short Description")]
        public string BookingTypeDescription { get; set; }
    }
}