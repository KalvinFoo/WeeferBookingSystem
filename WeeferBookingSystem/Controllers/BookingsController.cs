using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WeeferBookingSystem;
using WeeferBookingSystem.Entity;
using WeeferBookingSystem.Models;

namespace WeeferBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bookings
        public async Task<ActionResult> Index()
        {
            //return View(db.Bookings.ToList());
            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Bookings"), Name = "Bookings" });
            ViewBag.Breadcrumbs = Breadcrumbs;
            return await Task.Run(() => View());
        }

        // GET: Bookings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            //Category category = db.Categories.Find(booking.CategoryId);
            //ViewBag.CategoryName = category.Name;
            if (booking == null)
            {
                return HttpNotFound();
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Bookings"), Name = "Bookings" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Details", "Bookings"), Name = "Detail" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Bookings"), Name = "Bookings" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Create", "Bookings"), Name = "Create" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            ViewBag.CustomerId = new SelectList(db.Categories, "Id", "Name");
            Booking booking = new Booking();
            booking.DateFrom = DateTime.Now;
            booking.DateTo = DateTime.Now;
            booking.Status = Status.Draft;
            return View(booking);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CategoryId,Description,DateFrom,DateTo,CreatedBy,CreateDateTime,UpdatedBy")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                if (booking.DateFrom > booking.DateTo)
                {
                    TempData["ErrorNote"] = "Date From cannot be greater than Date To!";
                    return RedirectToAction("Index");
                }
                booking.CreatedBy = User.Identity.GetUserId();
                booking.CreateDateTime = DateTime.Now;
                booking.UpdatedBy = User.Identity.GetUserId();
                TempData["InfoNote"] = "Successfully created!";
                await db.SaveChangesAsync();

                switch (Request.Form["submit"])
                {
                    case "btnSave":
                        return RedirectToAction("Details", new { id = booking.Id });
                    case "btnSaveAndClose":
                        return RedirectToAction("Index");
                }

                List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
                Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Bookings"), Name = "Bookings" });
                Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "Bookings"), Name = "Create" });
                ViewBag.Breadcrumbs = Breadcrumbs;

                return RedirectToAction("Index");
            }

            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);

            if (booking.Status!= Status.Draft)
                TempData["ErrorNote"] = "Only transaction with 'Draft' status can be edited";
                return RedirectToAction("Index");

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", booking.CategoryId);
            if (booking == null)
            {
                return HttpNotFound();
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Bookings"), Name = "Bookings" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "Bookings"), Name = "Edit" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CategoryId,Description,DateFrom,DateTo,CreatedBy,CreateDateTime,UpdatedBy,UpdateDateTime")] Booking booking)
        {
            try
            {
                Booking objBooking = await db.Bookings.FindAsync(booking.Id);
                if (objBooking == null)
                {
                    return HttpNotFound();
                }
                if (ModelState.IsValid)
                {
                    objBooking.CategoryId = booking.CategoryId;
                    objBooking.Description = booking.Description;
                    objBooking.DateFrom = booking.DateFrom;
                    objBooking.DateTo = booking.DateTo;
                    TempData["InfoNote"] = "Successfully modified!";
                    await db.SaveChangesAsync();

                    switch (Request.Form["submit"])
                    {
                        case "btnSave":
                            return RedirectToAction("Details", new { Id = booking.Id });
                        case "btnSaveAndClose":
                            return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorNote"] = ex.Message;
                return RedirectToAction("Index");
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Bookings"), Name = "Bookings" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "Bookings"), Name = "Edit" });
            ViewBag.Breadcrumbs = Breadcrumbs;

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

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Bookings"), Name = "Bookings" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Delete", "Bookings"), Name = "Delete" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            TempData["InfoNote"] = "Successfully deleted!";
            return RedirectToAction("Index");
        }

        public static MetronicDatatableSetting<Booking> IndexListDatatableSetting()
        {
            ApplicationDbContext dbcontext = new ApplicationDbContext();
            MetronicDatatableSetting<Booking> datatableSetting = new MetronicDatatableSetting<Booking>();
            datatableSetting.BaseCollection = dbcontext.Bookings;
            datatableSetting.ColumnSettings.Add(new ColumnSetting<Booking>()
            {
                Name = "CategoryId",
                Caption = "Category",
                ColumnWidth = 17,
                Type = FieldType.ForeignObject,
                SelectList = new SelectList(dbcontext.Categories, "Id", "Name"),
                GetValueMethod = model => model.Category.Name
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<Booking>()
            {
                Name = "Description",
                Caption = "Description",
                ColumnWidth = 17,
                Type = FieldType.Text,
                GetValueMethod = model => model.Description
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<Booking>()
            {
                Name = "DateFrom",
                Caption = "Date From",
                ColumnWidth = 17,
                Type = FieldType.Date,
                GetValueMethod = model => model.DateFrom.ToString(GlobalVariable.DATETIMEFORMAT)
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<Booking>()
            {
                Name = "DateTo",
                Caption = "Date To",
                ColumnWidth = 17,
                Type = FieldType.Date,
                GetValueMethod = model => model.DateTo.ToString(GlobalVariable.DATETIMEFORMAT)
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<Booking>()
            {
                Name = "Status",
                Caption = "Status",
                ColumnWidth = 10,
                Type = FieldType.Enum,
                EnumType = typeof(Status),
                GetValueMethod = model => model.Status.ToString()
            });
            return datatableSetting;
        }

        public async Task<ActionResult> ShowIndexList()
        {
            MetronicDatatableSetting<Booking> dtSetting = IndexListDatatableSetting();
            return Content(await dtSetting.GetJsonContent(this));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Submit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            var bookings = await db.Bookings
                .Join(db.Categories,
                    b => b.CategoryId,
                    c => c.Id,
                    (b, c) => new { b, c })
                    .Where(cnb => cnb.b.CategoryId == booking.CategoryId
                                    && (booking.DateFrom >= cnb.b.DateFrom && booking.DateFrom <= cnb.b.DateTo ||
                                    booking.DateTo >= cnb.b.DateFrom && booking.DateTo <= cnb.b.DateTo)
                                    && cnb.b.Status == Status.PendingForApproval).ToListAsync();


            if (bookings.Count > 0)
            {
                //var category = db.Categories.Where(c => c.Id == bookings.FirstOrDefault().CategoryId).FirstOrDefault();
                string errorNote = "There are booked " + bookings.FirstOrDefault().c.Name + " from " + bookings.FirstOrDefault().b.DateFrom.ToString() + " to " + bookings.FirstOrDefault().b.DateTo.ToString();
                TempData["ErrorNote"] = errorNote;
                return RedirectToAction("Index");
            }
            //DbContextTransaction dbContextTransaction = db.Database.BeginTransaction();
            try
            {
                booking.Status = Status.PendingForApproval;
                await db.SaveChangesAsync();

                //var detailUrl = @"<a href ='" + Url.Action("Details", "Bookings", new { id = booking.Id }, protocol: Request.Url.Scheme) + "'>link</a>";
                //await Notification.SendEmail(db, "CreateDiscountGroupCreation", new string[] { detailUrl, discountGroupCreation.Name, discountGroupCreation.EffectiveDate.ToString(GlobalVariable.DATEFORMAT) });

                TempData["InfoNote"] = "Booking has been confirmed and is waiting for approval!";
                return RedirectToAction("Details", new { id = booking.Id });
            }
            catch (InvalidOperationException ex)
            {
                //dbContextTransaction.Rollback();
                TempData["ErrorNote"] = ex.Message.ToString();
                return RedirectToAction("Details", new { id = booking.Id });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking= await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            //DbContextTransaction dbContextTransaction = db.Database.BeginTransaction();
            try
            {
                booking.Status = Status.Approved;
                await db.SaveChangesAsync();
                TempData["InfoNote"] = "Booking has been approved!";

                //eVoucherContext context = new eVoucherContext();
                //BatamFastApplicationUser batamFastApplicationUsers = context.BatamFastApplicationUsers.FirstOrDefault(c => c.Name == discountGroupCreation.CreatedBy);

                var detailUrl = @"<a href ='" + Url.Action("Details", "Bookings", new { id = booking.Id }, protocol: Request.Url.Scheme) + "'>link</a>";
                //await Notification.SendEmail(db, "DiscountGroupCreationApproved", batamFastApplicationUsers.Email.ToString(), new string[] { detailUrl, discountGroupCreation.Name, discountGroupCreation.EffectiveDate.ToString(GlobalVariable.DATEFORMAT) });

                return RedirectToAction("Details", new { id = booking.Id });
            }
            catch (InvalidOperationException ex)
            {
                //dbContextTransaction.Rollback();
                TempData["ErrorNote"] = ex.Message.ToString();
                return RedirectToAction("Details", new { id = booking.Id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = await db.Bookings.FindAsync(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            try
            {
                booking.Status = Status.Cancelled;
                await db.SaveChangesAsync();
                TempData["InfoNote"] = "Data has been cancelled!";

                //eVoucherContext context = new eVoucherContext();
                //BatamFastApplicationUser batamFastApplicationUsers = context.BatamFastApplicationUsers.FirstOrDefault(c => c.Name == discountGroupCreation.CreatedBy);

                //var detailUrl = @"<a href ='" + Url.Action("Details", "DiscountGroupCreations", new { id = discountGroupCreation.Id }, protocol: Request.Url.Scheme) + "'>link</a>";
                //await Notification.SendEmail(db, "DiscountGroupCreationCancelled", batamFastApplicationUsers.Email.ToString(), new string[] { detailUrl, discountGroupCreation.Name, discountGroupCreation.EffectiveDate.ToString(GlobalVariable.DATEFORMAT) });

                return RedirectToAction("Details", new { id = booking.Id });
            }
            catch (InvalidOperationException ex)
            {
                //dbContextTransaction.Rollback();
                TempData["ErrorNote"] = ex.Message.ToString();
                return RedirectToAction("Details", new { id = booking.Id });
            }
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
