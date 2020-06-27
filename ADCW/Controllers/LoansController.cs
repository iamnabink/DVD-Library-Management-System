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
    public class LoansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Loans
        [Authorize(Roles = "Assistant, Manager")]
        public ActionResult Index()
        {
            var loans = db.Loans.Include(l => l.Dvds).Include(l => l.LoanTypes).Include(l => l.Members);
            return View(loans.ToList());
        }

        // GET: Loans/Details/5
        [Authorize(Roles = "Assistant, Manager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // GET: Loans/Create
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            ViewBag.DvdId = new SelectList(db.DVDs, "DVDId", "DVDId");
            ViewBag.LoanTypeId = new SelectList(db.LoanTypes, "LoanTypeId", "LoanTypeTitle");
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberFirstName");
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]

        public ActionResult Create([Bind(Include = "LoanId,LoanTypeId,MemberId,DvdId,DateOut,DateDue,DateReturned")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Loans.Add(loan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DvdId = new SelectList(db.DVDs, "DVDId", "DVDId", loan.DvdId);
            ViewBag.LoanTypeId = new SelectList(db.LoanTypes, "LoanTypeId", "LoanTypeTitle", loan.LoanTypeId);
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberFirstName", loan.MemberId);
            return View(loan);
        }

        // GET: Loans/Edit/5
        [Authorize(Roles = "Manager")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            ViewBag.DvdId = new SelectList(db.DVDs, "DVDId", "DVDId", loan.DvdId);
            ViewBag.LoanTypeId = new SelectList(db.LoanTypes, "LoanTypeId", "LoanTypeTitle", loan.LoanTypeId);
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberFirstName", loan.MemberId);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit([Bind(Include = "LoanId,LoanTypeId,MemberId,DvdId,DateOut,DateDue,DateReturned")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DvdId = new SelectList(db.DVDs, "DVDId", "DVDId", loan.DvdId);
            ViewBag.LoanTypeId = new SelectList(db.LoanTypes, "LoanTypeId", "LoanTypeTitle", loan.LoanTypeId);
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberFirstName", loan.MemberId);
            return View(loan);
        }

        // GET: Loans/Delete/5
        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // POST: Loans/Delete/5
        [Authorize(Roles = "Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loan loan = db.Loans.Find(id);
            db.Loans.Remove(loan);
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
