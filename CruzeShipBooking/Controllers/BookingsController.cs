using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CruzeShipBooking.Models;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using PayFast;
//using PayFast;

namespace CruzeShipBooking.Controllers
{
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly PayFastSettings payFastSettings;
        public BookingsController()
        {

            this.payFastSettings = new PayFastSettings();
            this.payFastSettings.MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            this.payFastSettings.MerchantKey = ConfigurationManager.AppSettings["MerchantKey"];
            this.payFastSettings.PassPhrase = ConfigurationManager.AppSettings["PassPhrase"];
            this.payFastSettings.ProcessUrl = ConfigurationManager.AppSettings["ProcessUrl"];
            this.payFastSettings.ValidateUrl = ConfigurationManager.AppSettings["ValidateUrl"];
            this.payFastSettings.ReturnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            this.payFastSettings.CancelUrl = ConfigurationManager.AppSettings["CancelUrl"];
            this.payFastSettings.NotifyUrl = ConfigurationManager.AppSettings["NotifyUrl"];
      
        }

        // GET: Bookings
        public ActionResult Index()
        {
            var userName = User.Identity.GetUserName();
            var bookings = db.Bookings;//.Include(a => a.BookingPackage);
            if (!User.IsInRole("Admin"))
            {
                return View(bookings.ToList().Where(x => x.CustomerEmail == userName));
            }
            else
            {
                return View(bookings.ToList());
            }
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        public ActionResult BookingConfirmation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            var packageID = booking.BookingPackageId;
            var packagePrice = db.BookingPackages.Find(packageID);
            ViewBag.packages = packagePrice.PackagePrice;

            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        public ActionResult AcceptBooking(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Booking booking = db.Bookings.Find(id);
            if (booking != null)
            {
                return Redirect(OnceOff(booking));
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

        }
        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.BookingPackageId = new SelectList(db.BookingPackages, "BookingPackageId", "BookingPackageDescription", "PackageAcomodationQuantity");
            var booking = db.BookingPackages;  //.ToListAsync<BookingPackage>;
            ViewBag.booking = booking;
          
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,CustomerEmail,BookingPackageId,DateBookingFor,DateBooked,BookingPrice,BookingStatus,ReferenceNumber,Food,Beverages,Alcohol,Decor,NumGuest,BasicCost,IsPaid")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.GetUserName();
                if (booking.CheckBookingDate() == false)
                {
                    var userID = User.Identity.GetUserId();
                    booking.DateBooked = DateTime.Now;
                    booking.BookingPrice = booking.GetBookingFee(booking.Decor,booking.NumGuest,booking.Food,booking.Beverages, booking.Alcohol);
                    booking.TotalDrinks = booking.TotalDriksCost(booking.NumGuest);
                    booking.TotalAcohol = booking.TotalAlcoholCost(booking.NumGuest);
                    booking.TotalDecoration = booking.DecorCost(booking.Decor);
                    booking.TotalFood = booking.TotalFoodCost(booking.NumGuest);
                    booking.BookingStatus = "Awaiting Approval";
                    booking.CustomerEmail = userName;
                    booking.ReferenceNumber = booking.GenerateReferenceNumber(userID);
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    EmailSender.SendBookingEmail(booking);
                    return RedirectToAction("BookingConfirmation", new { Id = booking.BookingId });
                }
                else
                {
                    ModelState.AddModelError("", "You can not book for a date that has already passed");
                    ViewBag.BookingPackageId = new SelectList(db.BookingPackages, "BookingPackageId", "BookingPackageDescription", booking.BookingPackageId);
                    return View(booking);

                }

            }

            ViewBag.BookingPackageId = new SelectList(db.BookingPackages, "BookingPackageId", "BookingPackageDescription", booking.BookingPackageId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookingPackageId = new SelectList(db.BookingPackages, "BookingPackageId", "BookingPackageDescription", booking.BookingPackageId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,CustomerEmail,BookingPackageId,DateBookingFor,DateBooked,BookingPrice,BookingStatus,ReferenceNumber,Food,Beverages,Decor,NumGuest,BasicCost,IsPaid")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookingPackageId = new SelectList(db.BookingPackages, "BookingPackageId", "BookingPackageDescription", booking.BookingPackageId);
            return View(booking);
        }
        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }
        public ActionResult CancelPayment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult ConfirmBooking(int? id)
        {
            var dbRecord = db.Bookings.Find(id);
            dbRecord.BookingStatus = "Booking Confirmed";
            db.Entry(dbRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            EmailSender.SendBookingEmail(dbRecord);
            return RedirectToAction("Index");
        }
        public ActionResult CheckInBooking(int? id)
        {
            var dbRecord = db.Bookings.Find(id);
            dbRecord.BookingStatus = "Booking Checked In";
            db.Entry(dbRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            EmailSender.SendBookingEmail(dbRecord);
            return RedirectToAction("Index");
        }

        public ActionResult CancelBooking(int? id)
        {
            var dbRecord = db.Bookings.Find(id);
            dbRecord.BookingStatus = "Booking Canceled";
            dbRecord.DateBooked = DateTime.Now;
            db.Entry(dbRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            EmailSender.SendBookingEmail(dbRecord);
            return RedirectToAction("Index");
        }
        public ActionResult CheckOutBooking(int? id)
        {
            var dbRecord = db.Bookings.Find(id);
            dbRecord.BookingStatus = "Booking Checked Out";
            dbRecord.DateBooked = DateTime.Now;
            db.Entry(dbRecord).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            EmailSender.SendBookingEmail(dbRecord);
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public string OnceOff(Booking booking)
        {
            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

            //// Merchant Details
            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

            // Buyer Details
            onceOffRequest.email_address = "sbtu01@payfast.co.za";

            //// Transaction Details
            onceOffRequest.m_payment_id = "8d00bf49-e979-4004-228c-08d452b86380";
            onceOffRequest.amount = Convert.ToInt32(booking.BookingPrice);
            onceOffRequest.item_name = booking.BookingPackage.BookingPackageDescription;
            onceOffRequest.item_description = "Package Booking.";

            //// Transaction Options
            onceOffRequest.email_confirmation = true;
            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

            booking.IsPaid = true;
            booking.BookingStatus = "Booking Confirmed";
            db.SaveChanges();
            //////////////////////////////////////////////// //// /// / / //     / / / / / / / / / / / / / / / / / / / / / / / / / / / / // / / / / 
            var userName = User.Identity.GetUserName();

            string subject = "Hello " + userName + " Thank you for Booking. ";

            string message = "This message was sent to " + userName + " \n " +
                "If it is not yours just ignore or contact us on amocybertech@gmail.com \n \n " +
                "Hello you've booked \'"+ booking.BookingPackage.BookingPackageDescription + "\' and your total payment is " + Convert.ToDouble(booking.BookingPrice) + "" +
                " \n" +
                "We hank you \n";

            /// mail
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            MailAddress to = new MailAddress(userName);
            MailAddress from = new MailAddress("amocybertech@gmail.com");

            MailMessage mm = new MailMessage(from, to);
            mm.Subject = subject;
            mm.Body = message;
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("amocybertech@gmail.com", "@Dut123456");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = nc;
            smtp.Send(mm);

            return redirectUrl;
            //return null;
        }
    }
}
