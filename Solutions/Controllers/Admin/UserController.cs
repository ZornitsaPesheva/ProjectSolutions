using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Solutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Solutions.Controllers.Admin
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: User/List
        public ActionResult List()
        {
            using (var database = new ApplicationDbContext())
            {
                var users = database.Users
                    .ToList();

                var admins = GetAdminUserNames(users, database);
                ViewBag.Admins = admins;

                return View(users);
            }
        }

        private HashSet<string> GetAdminUserNames(List<ApplicationUser> users, ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var admin = new HashSet<string>();

            foreach (var user in users)
            {
                if (userManager.IsInRole(user.Id, "Admin"))
                {
                    admin.Add(user.UserName);
                }
            }

            return admin;
        }
    }
}