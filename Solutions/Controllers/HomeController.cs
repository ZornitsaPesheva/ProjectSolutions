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
                    .Include(m => m.Courses)
                    .OrderBy(m => m.Priority)
                    .ToList();

                return View(modules);
            }
        }

        private ApplicationDbContext database = new ApplicationDbContext();
        public ActionResult ListCourses(int? moduleId)
        {

            if (moduleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            

        //using (var database = new ApplicationDbContext())
        //{
        //    //var courses = database.Courses
        //    .Include(c => c.Chapters)
        //    .Where(a => a.ModuleId == moduleId)
        //    .ToList();

        Module module = database.Modules.Find(moduleId);
                if (module == null)
                {
                    return HttpNotFound();
                }
                module.Courses = database.Courses.Where(x => x.ModuleId == module.Id)
                    .ToList();
                return View(module);

            //   // return View(courses);
            //}
        }


        public ActionResult ListChapters(int? courseId)
        {
            if (courseId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //using (var database = new ApplicationDbContext())
            //{
            //    var chapters = database.Chapters
            //        .Include(p => p.Posts)
            //        .Where(a => a.CourseId == courseId)
            //        .ToList();

            //    return View(chapters);
            //}

            Course course = database.Courses.Find(courseId);
            if (course == null)
            {
                return HttpNotFound();
            }
            course.Chapters = database.Chapters.Where(x => x.CourseId == course.Id)
                .ToList();
            return View(course);
        }

        public ActionResult ListPosts(int? chapterId)
        {
            if (chapterId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           Chapter chapter = database.Chapters.Find(chapterId);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            chapter.Posts = database.Posts.Where(x => x.ChapterId == chapter.Id)
                .Include(p => p.Author)
                .ToList();
            return View(chapter);

            //using (var database = new ApplicationDbContext())
            //{
            //    var posts = database.Posts
            //        .Where(a => a.ChapterId == chapterId)
            //        .Include(a => a.Author)
            //        .ToList();

            //    return View(posts);
            //}
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                database.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}