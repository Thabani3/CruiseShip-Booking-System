using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CruzeShipBooking.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        [DisplayName("Customer Email")]
        public string CustomerEmail { get; set; }
        [ForeignKey("BookingPackage")]
        [DisplayName("Choose Package")]
        public int BookingPackageId { get; set; }
        public virtual BookingPackage BookingPackage { get; set; }
        [DisplayName("Cruize Date"),Required,DataType(DataType.Date)]
        public DateTime DateBookingFor { get; set; }
       
        [DisplayName("Date Booked")]
        public DateTime DateBooked { get; set; }

        [DisplayName("Booking Amount"),DataType(DataType.Currency)]
        public decimal BookingPrice { get; set; }
        [DisplayName("Status")]
        public string BookingStatus { get; set; }
        [DisplayName("Reference Number")]
        public string ReferenceNumber { get; set; }

        [DisplayName("Food")]
        public bool Food { get; set; }

        [DisplayName("Beverages")]
        public bool Beverages{ get; set; }

        [DisplayName("Alcohol")]
        public bool Alcohol { get; set; }
       // public decimal NonAcohol { get; set; }
        public string Decor { get; set; }

        [DisplayName("Number of guests")]
        public int NumGuest { get; set; }

        [DisplayName("Total Amount for booking"), DataType(DataType.Currency)]
        public decimal TotalFood { get; set; }
        [DisplayName("Total Amount for beverages"), DataType(DataType.Currency)]
        public decimal TotalDrinks { get; set; }
        [DisplayName("Total Amount for Alcohol"), DataType(DataType.Currency)]
        public decimal TotalAcohol { get; set; }
        [DisplayName("Decoration Amount"), DataType(DataType.Currency)]
        public decimal TotalDecoration { get; set; }

        public bool IsPaid { get; set; }

        public ApplicationDbContext db = new ApplicationDbContext();



        public decimal TotalFoodCost(int numGuest)
        {
            var totalFood = (decimal)24.35 * numGuest;
            return totalFood;
        }
        
public decimal TotalDriksCost(int numGuest)
        {
            var totalDrinks = 0;

            if (Beverages == true)
            {
                 totalDrinks =  14 * numGuest;
            }
            
            return totalDrinks;
            
        }
        public decimal TotalAlcoholCost(int numGuest)
        {
            var TotalAlcohol = 0;
            if (Alcohol == true)
            {
                 TotalAlcohol = 300 * numGuest;
            }
            return TotalAlcohol;

        }
        public decimal DecorCost(string decorType)
        {
           decimal thistype = (decimal)47341.96;
            if (decorType.ToUpper() == "simple".ToUpper())
                thistype = (decimal)23670.98;

            return thistype;
        }

        public decimal GetBookingFee(string decortype,int numGuest,bool food, bool beverage, bool Alcohol)
        {
            var fee = (from bp in db.BookingPackages
                       where bp.BookingPackageId == BookingPackageId
                       select bp.PackagePrice
                     ).FirstOrDefault();
            if (food == true)
             fee +=TotalFoodCost(numGuest);
            if (beverage == true)
                fee += TotalDriksCost(numGuest);
            if(Alcohol == true)
            {
                fee += TotalAlcoholCost(numGuest);
            }

            return fee+= DecorCost(decortype);
        }

        public bool CheckBookingDate()
        {
            if (DateBookingFor < DateTime.Now.Date)
            {
                return true;
            }
            return false;
        }

       public string GenerateReferenceNumber(string userId)
        {
            string referenceNo = CustomerEmail.Substring(0, CustomerEmail.LastIndexOf("@"))+userId.Substring(0,4)+BookingId;
            return referenceNo;
        }
    }
}