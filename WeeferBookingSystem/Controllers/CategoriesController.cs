using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WeeferBookingSystem;
using WeeferBookingSystem.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace WeeferBookingSystem.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public async Task<ActionResult> Index()
        {
            //return View(db.Categories.ToList());
            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Categories"), Name = "Categories" });
            ViewBag.Breadcrumbs = Breadcrumbs;
            return await Task.Run(() => View());
        }

        // GET: Categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Categories"), Name = "Categories" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Details", "Categories"), Name = "Detail" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Categories"), Name = "Categories" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Create", "Categories"), Name = "Create" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            Category category = new Category();
            category.IsRequiredApproval = false;
            return View(category);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,CreatedBy,CreateDateTime,UpdatedBy,IsRequiredApproval")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                category.CreatedBy = User.Identity.GetUserId();
                category.CreateDateTime = DateTime.Now;
                category.UpdatedBy = User.Identity.GetUserId();
                TempData["InfoNote"] = "Successfully created!";
                await db.SaveChangesAsync();

                switch (Request.Form["submit"])
                {
                    case "btnSave":
                        return RedirectToAction("Details", new { Id = category.Id });
                    case "btnSaveAndClose":
                        return RedirectToAction("Index");
                }
                //category.CreatedBy = User.Identity.GetUserName();
                //category.CreateDateTime = DateTime.Now;
                //category.UpdatedBy = User.Identity.GetUserName();
                //db.Categories.Add(category);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Categories"), Name = "Categories" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "Categories"), Name = "Create" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            //Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Categories"), Name = "Categories" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "Categories"), Name = "Edit" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,CreatedBy,CreateDateTime,UpdatedBy,UpdatedTime,IsRequiredApproval")] Category category)
        {
            try
            {
                Category objCategory = await db.Categories.FindAsync(category.Id);
                if (objCategory == null)
                {
                    return HttpNotFound();
                }
                if (ModelState.IsValid)
                {
                    objCategory.Name = category.Name;
                    objCategory.Description = category.Description;
                    objCategory.IsRequiredApproval = category.IsRequiredApproval;
                    TempData["InfoNote"] = "Successfully modified!";
                    await db.SaveChangesAsync();

                    switch (Request.Form["submit"])
                    {
                        case "btnSave":
                            return RedirectToAction("Details", new { Id = category.Id });
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
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Categories"), Name = "Categories" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Edit", "Categories"), Name = "Edit" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);


            var categories = from obj in db.Bookings
                             where obj.CategoryId == id
                             select new { Id = obj.Id, CategoryId = obj.CategoryId };

            if (categories.Count() > 0)
            {
                TempData["ErrorNote"] = "Cannot delete category which has been used!";
                return RedirectToAction("Details", new { Id = category.Id });
            }

            if (category == null)
            {
                return HttpNotFound();
            }

            List<Breadcrumb> Breadcrumbs = new List<Breadcrumb>();
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Index", "Categories"), Name = "Categories" });
            Breadcrumbs.Add(new Breadcrumb() { Link = Url.Action("Delete", "Categories"), Name = "Delete" });
            ViewBag.Breadcrumbs = Breadcrumbs;

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["InfoNote"] = "Successfully deleted!";
            return RedirectToAction("Index");
        }

        public static MetronicDatatableSetting<Category> IndexListDatatableSetting()
        {
            ApplicationDbContext dbcontext = new ApplicationDbContext();
            MetronicDatatableSetting<Category> datatableSetting = new MetronicDatatableSetting<Category>();
            datatableSetting.BaseCollection = dbcontext.Categories;

            datatableSetting.ColumnSettings.Add(new ColumnSetting<Category>()
            {
                Name = "Name",
                Caption = "Name",
                ColumnWidth = 17,
                Type = FieldType.Text,
                GetValueMethod = model => model.Name
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<Category>()
            {
                Name = "Description",
                Caption = "Description",
                ColumnWidth = 17,
                Type = FieldType.Text,
                GetValueMethod = model => model.Description
            });
            datatableSetting.ColumnSettings.Add(new ColumnSetting<Category>()
            {
                Name = "RequestApproval",
                Caption = "Request Approval",
                ColumnWidth = 17,
                Type = FieldType.Boolean,
                GetValueMethod = model => Convert.ToString(model.IsRequiredApproval)
            });
            return datatableSetting;
        }

        public async Task<ActionResult> ShowIndexList()
        {
            MetronicDatatableSetting<Category> dtSetting = IndexListDatatableSetting();
            return Content(await dtSetting.GetJsonContent(this));
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
