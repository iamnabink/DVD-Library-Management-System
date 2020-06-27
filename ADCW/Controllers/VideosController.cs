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
    public class VideosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Videos
        public ActionResult Index()
        {
            var videos = db.Videos.Include(v => v.Producers).Include(v => v.Studios).Include(v => v.VidCategories);
            return View(videos.ToList());
        }

        // GET: Videos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // GET: Videos/Create
        public ActionResult Create()
        {
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "ProducerFirstName");
            ViewBag.StudioId = new SelectList(db.Studios, "StudioId", "StudioName");
            ViewBag.VidCatId = new SelectList(db.VideoCategories, "VidCatId", "VidCatName");
            return View();
        }

        // POST: Videos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VideoId,Title,DateReleased,StandardCharge,PenaltyCharge,ProducerId,StudioId,VidCatId")] Video video)
        {
            if (ModelState.IsValid)
            {
                db.Videos.Add(video);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "ProducerFirstName", video.ProducerId);
            ViewBag.StudioId = new SelectList(db.Studios, "StudioId", "StudioName", video.StudioId);
            ViewBag.VidCatId = new SelectList(db.VideoCategories, "VidCatId", "VidCatName", video.VidCatId);
            return View(video);
        }

        // GET: Videos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "ProducerFirstName", video.ProducerId);
            ViewBag.StudioId = new SelectList(db.Studios, "StudioId", "StudioName", video.StudioId);
            ViewBag.VidCatId = new SelectList(db.VideoCategories, "VidCatId", "VidCatName", video.VidCatId);
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VideoId,Title,DateReleased,StandardCharge,PenaltyCharge,ProducerId,StudioId,VidCatId")] Video video)
        {
            if (ModelState.IsValid)
            {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "ProducerFirstName", video.ProducerId);
            ViewBag.StudioId = new SelectList(db.Studios, "StudioId", "StudioName", video.StudioId);
            ViewBag.VidCatId = new SelectList(db.VideoCategories, "VidCatId", "VidCatName", video.VidCatId);
            return View(video);
        }

        // GET: Videos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Video video = db.Videos.Find(id);
            db.Videos.Remove(video);
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
