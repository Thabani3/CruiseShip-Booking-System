using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CruzeShipBooking.Models;

namespace CruzeShipBooking.Controllers
{
    public class BookingPackagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookingPackages
        public ActionResult Index()
        {
            var bookingPackages = db.BookingPackages.Include(b => b.BookingType);
            return View(bookingPackages.ToList());
        }

        // GET: BookingPackages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingPackage bookingPackage = db.BookingPackages.Find(id);
            if (bookingPackage == null)
            {
                return HttpNotFound();
            }
            return View(bookingPackage);
        }

        // GET: BookingPackages/Create
        public ActionResult Create()
        {
            ViewBag.BookingTypeId = new SelectList(db.BookingTypes, "BookingTypeId", "BookingTypeName");
            return View();
        }

        // POST: BookingPackages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingPackageId,BookingTypeId,BookingPackageDescription,PackagePrice,PackageAcomodationQuantity")] BookingPackage bookingPackage)
        {
            if (ModelState.IsValid)
            {
                db.BookingPackages.Add(bookingPackage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookingTypeId = new SelectList(db.BookingTypes, "BookingTypeId", "BookingTypeName", bookingPackage.BookingTypeId);
            return View(bookingPackage);
        }

        // GET: BookingPackages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingPackage bookingPackage = db.BookingPackages.Find(id);
            if (bookingPackage == null)
            {
                return HttpNotFound();
            }
            ViewBag.BookingTypeId = new SelectList(db.BookingTypes, "BookingTypeId", "BookingTypeName", bookingPackage.BookingTypeId);
            return View(bookingPackage);
        }

        // POST: BookingPackages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingPackageId,BookingTypeId,BookingPackageDescription,PackagePrice,PackageAcomodationQuantity")] BookingPackage bookingPackage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookingPackage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BookingTypeId = new SelectList(db.BookingTypes, "BookingTypeId", "BookingTypeName", bookingPackage.BookingTypeId);
            return View(bookingPackage);
        }

        // GET: BookingPackages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookingPackage bookingPackage = db.BookingPackages.Find(id);
            if (bookingPackage == null)
            {
                return HttpNotFound();
            }
            return View(bookingPackage);
        }

        // POST: BookingPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookingPackage bookingPackage = db.BookingPackages.Find(id);
            db.BookingPackages.Remove(bookingPackage);
            db.SaveChanges();
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
    }
}
