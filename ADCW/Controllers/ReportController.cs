using ADCW.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Services.Description;

namespace ADCW.Controllers
{
    [Authorize(Roles = "Assistant, Manager")]
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FilterTitleByLastName(string LastName)
        {
            // Task 1
            if (String.IsNullOrEmpty(LastName))
            {
                return View(new List<CastMember>());
            }
            else
            {
                var data = db.CastMembers.Include("Actors")
                    .Where(x => x.Actors.ActorLastName == LastName).ToList();
                return View(data);
            }
        }

        public ActionResult FilterShelfDVDByLastName(string LastName)
        {
            // Task 2
            if (String.IsNullOrEmpty(LastName))
            {
                return View(new List<VideoDVDCount>());
            }
            else
            {
                var videoList = db.CastMembers.Include("Actors")
                    .Where(x => x.Actors.ActorLastName == LastName)
                    .ToList().Select(z => z.VideoId);
                var dvdList = db.DVDs
                    .Include("Videos")
                    .Where(x => videoList.Contains(x.VideoId))
                    .ToList();
                var loanList = loanedDvdIds();
                var answer = dvdList
                    .Where(dvd => !loanList.Contains(dvd.DVDId))
                    .GroupBy(dvd => dvd.VideoId)
                    .Select(grp => new VideoDVDCount
                    {
                        VideoId = grp.Key,
                        Title = null,
                        DvdCount = grp.Count()
                    })
                    .Join(
                        db.Videos,
                        dvd => dvd.VideoId,
                        vid => vid.VideoId,
                        (dvd, vid) => new VideoDVDCount
                        {
                            VideoId = dvd.VideoId,
                            Title = vid.Title,
                            DvdCount = dvd.DvdCount
                        }
                    );
                return View(answer);
            }
        }

        private List<int> loanedDvdIds()
        {
            return db.Loans.Join(db.DVDs,
                        loan => loan.DvdId,
                        dvd => dvd.DVDId,
                        (loan, dvd) => new
                        {
                            returned = loan.DateReturned != null,
                            dvd.DVDId
                        }
                    )
                    .Where(x => !x.returned)
                    .Select(x => x.DVDId).ToList();
        }

        private List<DVD> shelfDVDs()
        {
            var loanList = loanedDvdIds();
            return db.DVDs
                .Where(x => !loanList.Contains(x.DVDId))
                .ToList();
        }

        public ActionResult FilterByMember(int? MemberId, string MemberLN)
        {
            // Task 3
            if (!MemberId.HasValue && String.IsNullOrEmpty(MemberLN))
            {
                return View(new List<DVD>());
            }
            else
            {
                var dateRange = DateTime.Today.AddDays(-31);
                var dvdList = db.Loans.Include("Members")
                    .Where(loan => (
                        loan.DateOut >= dateRange)
                        && (loan.MemberId == MemberId
                        || loan.Members.MemberLastName == MemberLN)
                    )
                    .Select(x => x.DvdId)
                    .ToList();
                var result = db.DVDs
                    .Where(x => dvdList.Contains(x.DVDId))
                    .ToList();
                return View(result);
            }
        }

        public ActionResult VideoDetails()
        {
            // Task 4
            var data = db.Videos.ToList()
                .Join(
                    db.CastMembers,
                    vid => vid.VideoId,
                    cast => cast.VideoId,
                    (vid, cast) => new VideoDetail
                    {
                        Title = vid.Title,
                        Producer = (vid.Producers.ProducerFirstName + ' ' + vid.Producers.ProducerLastName),
                        Studio = vid.Studios.StudioName,
                        DateReleased = vid.DateReleased,
                        ActorFN = cast.Actors.ActorFirstName,
                        ActorLN = cast.Actors.ActorLastName
                    }
                )
                .OrderBy(x => x.DateReleased).ThenBy(x => x.ActorLN)
                .ToList();
            return View(data);
        }

        public ActionResult LastLoan(int? DvdId)
        {
            // Task 5
            if (DvdId.HasValue)
            {
                var result = db.Loans.Include("Members").Include("DVDs")
                .Where(loan => loan.DvdId == DvdId)
                .OrderByDescending(x => x.DateOut)
                .FirstOrDefault();
                return View(result);
            }
            else
            {
                return View();
            }
        }

        public ActionResult LoanToMember()
        {
            // Task 6
            ViewBag.DvdId = new SelectList(shelfDVDs(), "DVDId", "DVDId");
            ViewBag.LoanTypeId = new SelectList(db.LoanTypes, "LoanTypeId", "LoanTypeTitle");
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberFirstName");
            return View(new Loan
            {
                DateOut = DateTime.Today,
                DateDue = DateTime.Today
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoanToMember([Bind(Include = "LoanId,LoanTypeId,MemberId,DvdId,DateOut,DateDue,DateReturned")] Loan loan)
        {
            // Task 6
            if (ModelState.IsValid)
            {
                var Member = db.Members
                    .Where(x => x.MemberId == loan.MemberId)
                    .FirstOrDefault();
                // Can member borrow more dvds
                var loans = db.Loans
                    .Where(x => x.MemberId == loan.MemberId && x.DateReturned == null).Count();
                if (loans < Member.MemberCategories.TotalLoans)
                {
                    // Is Member of age
                    var dvdRestricted = db.DVDs.Include("Videos")
                        .Where(x => x.DVDId == loan.DvdId)
                        .FirstOrDefault().Videos.VidCategories.AgeRestricted;
                    var age = DateTime.Today.Subtract(Member.DateOfBirth).TotalDays / 365; // Convert to years
                    if (dvdRestricted && age < 18)
                    {
                        TempData["ErrorMsg"] = "Cannot loan DVD - Age Restricted";
                    }
                    else
                    {
                        //db.Loans.Add(loan);
                        //db.SaveChanges();
                        TempData["Loan"] = loan;
                        return RedirectToAction("ShowBill", loan);
                    }
                }
                else
                {
                    TempData["ErrorMsg"] = "Cannot loan DVD - Loan limit reached";
                }
            }
            ViewBag.DvdId = new SelectList(shelfDVDs(), "DVDId", "DVDId", loan.DvdId);
            ViewBag.LoanTypeId = new SelectList(db.LoanTypes, "LoanTypeId", "LoanTypeTitle", loan.LoanTypeId);
            ViewBag.MemberId = new SelectList(db.Members, "MemberId", "MemberFirstName", loan.MemberId);
            return View(loan);
        }

        public ActionResult ShowBill(Loan data, int z = 5)
        {
            // Task 6
            try
            {
                var video = getVideo(data.DvdId);
                TempData["VideoName"] = video.Title;
                TempData["MemberName"] = db.Members.Where(x => x.MemberId == data.MemberId).FirstOrDefault().MemberFirstName;
                TempData["Charge"] = (video.StandardCharge * data.DateDue.Subtract(data.DateOut).TotalDays).ToString();
            }
            catch
            {
                return RedirectToAction("LoanToMember");
            }
            return View(data);
        }

        private Video getVideo(int dvdId)
        {
            return db.DVDs.Include("Videos").Where(x => x.DVDId == dvdId).FirstOrDefault().Videos;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShowBill([Bind(Include = "LoanId,LoanTypeId,MemberId,DvdId,DateOut,DateDue,DateReturned")] Loan loan)
        {
            // Task 6
            if (ModelState.IsValid)
            {
                db.Loans.Add(loan);
                db.SaveChanges();
                TempData["SuccessMsg"] = "Loan Added!";
                return Redirect("/Loans");
            }
            return View(loan);
        }

        public ActionResult ReturnDVD(int? id, int extra = 5)
        {
            // Task 7
            if (id.HasValue)
            {
                var loan = db.Loans.Where(x => x.LoanId == id).FirstOrDefault();
                loan.DateReturned = DateTime.Today.Date;
                var video = getVideo(loan.DvdId);
                TempData["VideoName"] = video.Title;
                if (DateTime.Today > loan.DateDue)
                {
                    TempData["Penalty"] = (video.PenaltyCharge * DateTime.Today.Subtract(loan.DateDue).TotalDays).ToString();
                }
                return View(loan);
            }
            else
            {
                return Redirect("/Loans");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnDVD(int id)
        {
            // Task 7
            var loan = db.Loans.Where(x => x.LoanId == id).FirstOrDefault();
            loan.DateReturned = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Loans.AddOrUpdate(loan);
                db.SaveChanges();
                TempData["SuccessMsg"] = "DVD Returned!";
            }
            return Redirect("/Loans");
        }

        public ActionResult showMemberLoanDetails()
        {
            // Task 8
            var loanedDvd = db.Loans
                .Where(x => x.DateReturned == null)
                .GroupBy(x => x.MemberId)
                .Select(x => new
                {
                    MemberId = x.Key,
                    TotalLoaned = x.Count()
                }).ToList();
            var members = db.Members.ToList().Join(
                    loanedDvd,
                    MEM => MEM.MemberId,
                    DVD => DVD.MemberId,
                    (x, loan) => new MemberLoans
                    {
                        MemberId = x.MemberId,
                        MemberFirstName = x.MemberFirstName,
                        MemberLastName = x.MemberLastName,
                        Address = x.Address,
                        PhoneNumber = x.PhoneNumber,
                        DateOfBirth = x.DateOfBirth,
                        category = x.MemberCategories,
                        TotalLoaned = loan.TotalLoaned
                    }).ToList();
            return View(members);
        }

        public ActionResult addVideoTitle()
        {
            // Task 9
            var data = new VideoDetailsModel();
            data.ActorIds = new int[0];
            data.DateReleased = DateTime.Today;
            data.Actors = new SelectList(db.Actors, "ActorId", "ActorFirstName");
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "ProducerFirstName");
            ViewBag.StudioID = new SelectList(db.Studios, "StudioId", "StudioName");
            ViewBag.VidCatId = new SelectList(db.VideoCategories, "VidCatId", "VidCatName");
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addVideoTitle([Bind(Include = "VideoId,Title,DateReleased,StandardCharge,PenaltyCharge,ProducerId,StudioId,VidCatId,ActorIds")]VideoDetailsModel data)
        {
            // Task 9
            if (ModelState.IsValid)
            {
                if (data.ActorIds == null)
                {
                    TempData["ErrorMsg"] = "At least one cast member required";
                } 
                else
                {
                    var video = new Video
                    {
                        Title = data.Title,
                        ProducerId = data.ProducerId,
                        StudioId = data.StudioId,
                        VidCatId = data.VidCatId,
                        DateReleased = data.DateReleased,
                        StandardCharge = data.StandardCharge,
                        PenaltyCharge = data.PenaltyCharge
                    };
                    db.Videos.Add(video);
                    db.SaveChanges();
                    var cast = data.ActorIds.Select(x => new CastMember { 
                        ActorId = x,
                        VideoId = video.VideoId
                    });
                    db.CastMembers.AddRange(cast);
                    db.SaveChanges();
                    TempData["SuccessMsg"] = "Added Video!";
                    return RedirectToAction("VideoDetails");
                }
            }
            data.Actors = new SelectList(db.Actors, "ActorId", "ActorFirstName", data.ActorIds);
            ViewBag.ProducerId = new SelectList(db.Producers, "ProducerId", "ProducerFirstName", data.ProducerId);
            ViewBag.StudioID = new SelectList(db.Studios, "StudioId", "StudioName", data.StudioId);
            ViewBag.VidCatId = new SelectList(db.VideoCategories, "VidCatId", "VidCatName", data.VidCatId);
            return View(data);
        }
    
        public ActionResult deleteOldDVDs()
        {
            // Task 10
            var loanList = loanedDvdIds();
            var dvds = db.DVDs.ToList()
                .Where(x => !loanList.Contains(x.DVDId) && DateTime.Today.Subtract(x.DateAdded).TotalDays >= 365);
            return View(dvds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult deleteOldDVDs(List<DVD> test)
        {
            // Task 10
            var loanList = loanedDvdIds();
            var dvds = db.DVDs.ToList()
                .Where(x => !loanList.Contains(x.DVDId) && DateTime.Today.Subtract(x.DateAdded).TotalDays >= 365);
            if (dvds.Count() > 0)
            {
                db.DVDs.RemoveRange(dvds);
                db.SaveChanges();
                TempData["SuccessMsg"] = "Deleted the entries";
            }
            else
            {
                TempData["ErrorMsg"] = "No entries in list";
            }
            return View(new List<DVD>());
        }

        public ActionResult showLoanedDVDs()
        {
            // Task 11
            var count = db.Loans.GroupBy(x => x.DateOut).Select(x => new {
                DateOut = x.Key,
                Count = x.Count()
            });
            var loans = db.Loans.ToList()
                .Where(x => x.DateReturned == null)
                .OrderBy(x => x.DateOut).ThenBy(x => x.Dvds.Videos.Title);
            var dictionary = new Dictionary<int, int> { };
            loans.ForEach(x => dictionary[x.DvdId] = count.Where(z => z.DateOut == x.DateOut).FirstOrDefault().Count);
            TempData["loanCount"] = dictionary;
            return View(loans);
        }

        public ActionResult showMemberLoans()
        {
            // Task 12
            // Find the loanIds of members with their latest loans
            var loanIds = db.Loans.ToList()
                .GroupBy(x => x.MemberId)
                .Select(mem => mem.OrderByDescending(x => x.DateOut).FirstOrDefault().LoanId)
                .ToList();
            var limitDate = DateTime.Today.AddDays(-31);
            var loans = db.Loans
                .Where(x => loanIds.Contains(x.LoanId) && x.DateOut < limitDate)
                .OrderBy(x => x.DateOut)
                .ToList();
            return View(loans);
        }

        public ActionResult showUnloanedVideos()
        {
            // Task 13
            var loanIds = db.Loans.ToList()
                .GroupBy(x => x.DvdId)
                .Select(loan => loan.OrderByDescending(x => x.DateOut).FirstOrDefault().LoanId)
                .ToList();
            var limitDate = DateTime.Today.AddDays(-31);
            var dvdsLoaned = db.Loans
                .Where(x => loanIds.Contains(x.LoanId) && x.DateOut > limitDate)
                .Select(x => x.DvdId)
                .ToList();
            var videoIds = db.DVDs.ToList()
                .GroupBy(x => x.VideoId)
                .Where(x => x.All(z => !dvdsLoaned.Contains(z.DVDId)))
                .Select(x => x.Key).ToList();
            var videos = db.Videos.Where(x => videoIds.Contains(x.VideoId)).ToList();
            return View(videos);
        }

    }
}
