using Solutions.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Solutions.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // return RedirectToAction("Index", "Modules");
            return RedirectToAction("ListModules");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ListModules()
        {
            using (var database = new ApplicationDbContext())
            {
                var modules = database.Modules
                    .Include(c => c.Courses)
                 //   .OrderBy(c => c.Name)
                    .ToList();

                return View(modules);
            }
        }

        public ActionResult ListCourses(int? moduleId)
        {
            if (moduleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var courses = database.Courses
                    .Include(c => c.Chapters)
                    .Where(a => a.ModuleId == moduleId)
                    .ToList();

                return View(courses);
            }
        }

        public ActionResult ListChapters(int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var chapters = database.Chapters
                    .Include(p => p.Posts)
                    .Where(a => a.CourseId == courseId)
                    .ToList();

                return View(chapters);
            }
        }

        public ActionResult ListPosts(int? chapterId)
        {
            if (chapterId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new ApplicationDbContext())
            {
                var posts = database.Posts
                    .Where(a => a.ChapterId == chapterId)
                    .Include(a => a.Author)
                    .ToList();

                return View(posts);
            }
        }
    }
}