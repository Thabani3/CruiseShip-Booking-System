using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CruzeShipBooking.Models
{
    public class BookingPackage
    {
        [Key]
        public int BookingPackageId { get; set; }
        public int BookingTypeId { get; set; }
        public virtual BookingType BookingType { get; set; }
        [DisplayName("Package Description")]
        public string BookingPackageDescription { get; set; }
        [DisplayName("Package Price"), DataType(DataType.Currency)]
        public decimal PackagePrice { get; set; }
        [DisplayName("Package Acommodation")]
        public int PackageAcomodationQuantity { get; set; }
    }
}