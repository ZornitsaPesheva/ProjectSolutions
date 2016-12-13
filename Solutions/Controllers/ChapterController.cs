using Solutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Solutions.Controllers
{
    public class ChapterController : Controller
    {
        // GET: Chapter
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: Chapter/List
        public ActionResult List()
        {
            using (var database = new ApplicationDbContext())
            {
                var chapters = database.Chapters
                    .ToList();

                return View(chapters);
            }
        }

        // GET: Chapter/Create
        public ActionResult Create()
        {
            using (var database = new ApplicationDbContext())
            {
                var model = new ChapterViewModel();
                model.Courses = database.Courses
                    .OrderBy(c => c.Name)
                    .ToList();

                return View(model);
            }
        }

        // POST: Chapter/Create
        [HttpPost]
        public ActionResult Create(ChapterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new ApplicationDbContext())
                {
                    var chapter = new Chapter(model.Title, model.CourseId);

                    database.Chapters.Add(chapter);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
    }
}