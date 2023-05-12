using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace CruzeShipBooking.Models
{
    public class EmailSender
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static void SendBookingEmail(Booking booking)
        {
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(booking.CustomerEmail, GetCustomerName(booking.CustomerEmail)));
            var body = $"Good Day {GetCustomerName(booking.CustomerEmail)}," +
                $" Your booking status has changed to {booking.BookingStatus}." +
                $"<br/> This email confrims your cruise booking process, if you have anny further enquiries feel free to contact us.";

            EmailService emailService = new EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = $"{booking.BookingStatus}!!  | Ref No.:" + booking.ReferenceNumber,
                mailBody = body,
                mailFooter = $"<br/> Kind Regards, <br/> <b>Senorita Cruise Team </b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        }

        public static string GetCustomerName(string customerEmail)
        {
            var name = (from customer in db.Customers
                        where customer.Email == customerEmail
                        select customer.FirstName).FirstOrDefault();
            return name;
        }
    }
}