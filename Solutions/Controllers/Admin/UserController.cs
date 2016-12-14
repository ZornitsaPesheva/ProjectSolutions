using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Solutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        // GET: User/Edit
        public ActionResult Edit(string id)
        {
            using (var database = new ApplicationDbContext())
            {
                if (id  == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = database.Users
                    .Where(u => u.Id == id)
                    .First();

                if (user == null)
                {
                    return HttpNotFound();
                }

                var viewModel = new EditUserViewModel();
                viewModel.User = user;
                viewModel.Roles = GetUserRoles(user, database);

                return View(viewModel);
            }
        }

        // POST: User/Edit
        [HttpPost]
        public ActionResult Edit(string id, EditUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {

                    var user = database.Users.FirstOrDefault(u => u.Id == id);

                    if (user == null)
                    {
                        return HttpNotFound();
                    }

                    if (!string.IsNullOrEmpty(viewModel.Password))
                    {
                        var header = new PasswordHasher();
                        var passwordHash = header.HashPassword(viewModel.Password);
                        user.PasswordHash = passwordHash;
                    }

                    user.Email = viewModel.User.Email;
                    user.FullName = viewModel.User.FullName;
                    user.UserName = viewModel.User.Email;
                    this.SetUserRoles(viewModel, user, database);

                    database.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    database.SaveChanges();

                    
                }
            }
            return RedirectToAction("List");
        }
private void SetUserRoles(EditUserViewModel model, ApplicationUser user, ApplicationDbContext db)
        {
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            foreach (var role in model.Roles)
            {
                if (role.IsSelected)
                {
                    userManager.AddToRole(user.Id, role.Name);
                }
                else if (!role.IsSelected)
                {
                    userManager.RemoveFromRoles(user.Id, role.Name);
                }
            }

        }

        private IList<Role> GetUserRoles(ApplicationUser user, ApplicationDbContext db)
        {
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            var roles = db.Roles
                .Select(r => r.Name)
                .OrderBy(r => r)
                .ToList();

            var userRoles = new List<Role>();

            foreach (var roleName in roles)
            {
                var role = new Role { Name = roleName };

                if (userManager.IsInRole(user.Id, roleName))
                {
                    role.IsSelected = true;
                }

                userRoles.Add(role);
            }

            return userRoles;
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