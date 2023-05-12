using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CruzeShipBooking.Models
{
    public class Customers
    {
        [Key]
        public string CustomerId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]

        public string LastName { get; set; }

        public string Email { get; set; }
        [DisplayName("Contact Number")]

        public string ContactNumber { get; set; }
        [DisplayName("Alernative Contact")]

        public string AlternativeNumber { get; set; }

    }
}