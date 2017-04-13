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
using WeeferBookingSystem.Models;

namespace WeeferBookingSystem.Controllers
{
    public class ApplicationConfigurationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationConfigurations
        public async Task<ActionResult> Index()
        {
            ApplicationConfiguration applicationConfiguration = new ApplicationDbContext().ApplicationConfigurations.FirstOrDefault();
            if (applicationConfiguration == null)
            {
                applicationConfiguration.ApprovalUsername = "test";
                db.ApplicationConfigurations.Add(applicationConfiguration);
                db.SaveChanges();
            }
            return await Task.Run(() => RedirectToAction("Details", new { id = applicationConfiguration.Id }));
        }

        // GET: ApplicationConfigurations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationConfiguration applicationConfiguration = await db.ApplicationConfigurations.FindAsync(id);
            if (applicationConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(applicationConfiguration);
        }
        
        // GET: ApplicationConfigurations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationConfiguration applicationConfiguration = await db.ApplicationConfigurations.FindAsync(id);
            if (applicationConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(applicationConfiguration);
        }

        // POST: ApplicationConfigurations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ApprovalUsername,SmtpUsername,SmtpPassword,SmtpFromName,SmtpHost,SmtpPort,SmtpEnableSsl")] ApplicationConfiguration applicationConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationConfiguration).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(applicationConfiguration);
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
