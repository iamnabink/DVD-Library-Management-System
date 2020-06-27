using ADCW.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ADCW.Controllers
{
    [Authorize(Roles = "Manager")]
    public class UserAndRolesController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: UserAndRoles
        public ActionResult Index()
        {
            return RedirectToAction("viewUsers");
        }

        public async Task<ActionResult> AddUserToRole()
        {
            ApplicationUserRoleViewModel vm = new ApplicationUserRoleViewModel();
            var Users = await db.Users.ToListAsync();
            var roles = await db.Roles.ToListAsync();
            ViewBag.UserId = new SelectList(Users, "Id", "UserName");
            ViewBag.RoleId = new SelectList(roles, "Id", "Name");
            return View(vm);
        }

        [HttpPost]
        public ActionResult AddUserToRole(ApplicationUserRoleViewModel model)
        {
            var role = db.Roles.Find(model.RoleId);
            if (role != null)
            {
                //await UserManager.AddToRoleAsync(model.UserId, role.Name);
                // SQL Insert into Inline query db.database.execute query
                var query = "INSERT INTO ASPNetUserRoles (UserId, RoleId) VALUES (\'" + model.UserId + "\',\'" + role.Id + "\')";
                try
                {
                    db.Database.ExecuteSqlCommand(query);
                    db.SaveChanges();
                    TempData["SuccessMsg"] = "Assigned the role!";
                }
                catch (System.Data.SqlClient.SqlException)
                {
                    TempData["ErrorMsg"] = "User already has role.";
                }
                return RedirectToAction("viewUsers");
            }
            return RedirectToAction("AddUserToRole");
        }

        // ----- USER CRUD ---

        public ActionResult viewUsers()
        {
            TempData["Roles"] = db.Roles.ToList();
            return View(db.Users.ToList());
        }


        public ActionResult UserDetails(string Id)
        {
            if (String.IsNullOrEmpty(Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        public ActionResult EditUser(string Id)
        {
            if (String.IsNullOrEmpty(Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include =
            "Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed ,TwoFactorEnabled ,LockoutEndDateUtc ,LockoutEnabled,AccessFailedCount,Roles,Claims,Logins,Id,UserName"
            )] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("viewUsers");
            }
            return View(user);
        }


        public ActionResult DeleteUser(string Id)
        {
            if (String.IsNullOrEmpty(Id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("viewUsers");
        }


        public ActionResult viewRoles()
        {
            //Microsoft.AspNet.Identity.EntityFramework.IdentityRole
            var roles = db.Roles.ToList();
            var x = roles.Count();
            return View(roles);
        }
    }
}