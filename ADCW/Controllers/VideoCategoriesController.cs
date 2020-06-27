using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ADCW.Models;

namespace ADCW.Controllers
{
    [Authorize(Roles = "Assistant, Manager")]
    public class VideoCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VideoCategories
        public ActionResult Index()
        {
            return View(db.VideoCategories.ToList());
        }

        // GET: VideoCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoCategory videoCategory = db.VideoCategories.Find(id);
            if (videoCategory == null)
            {
                return HttpNotFound();
            }
            return View(videoCategory);
        }

        // GET: VideoCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VideoCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VidCatId,VidCatName,AgeRestricted")] VideoCategory videoCategory)
        {
            if (ModelState.IsValid)
            {
                db.VideoCategories.Add(videoCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(videoCategory);
        }

        // GET: VideoCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoCategory videoCategory = db.VideoCategories.Find(id);
            if (videoCategory == null)
            {
                return HttpNotFound();
            }
            return View(videoCategory);
        }

        // POST: VideoCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VidCatId,VidCatName,AgeRestricted")] VideoCategory videoCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(videoCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(videoCategory);
        }

        // GET: VideoCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VideoCategory videoCategory = db.VideoCategories.Find(id);
            if (videoCategory == null)
            {
                return HttpNotFound();
            }
            return View(videoCategory);
        }

        // POST: VideoCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VideoCategory videoCategory = db.VideoCategories.Find(id);
            db.VideoCategories.Remove(videoCategory);
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
