using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruzeShipBooking.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public DateTime DateAdded { get; set; }
    }
}