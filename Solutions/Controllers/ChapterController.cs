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
    public class ChapterController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chapter
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: Chapter/List
        public ActionResult List()
        {

            var chapters = db.Chapters
                .ToList();

            return View(chapters);

        }

        // GET: Chapter/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var chapter = db.Chapters
                .Where(a => a.Id == id)
                .First();

            if (chapter == null)
            {
                return HttpNotFound();
            }

            var model = new ChapterViewModel();
            model.Id = chapter.Id;
            model.Title = chapter.Title;

            return View(model);
        }

        // POST: Chapter/Edit
        [HttpPost]
        public ActionResult Edit(ChapterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var chapter = db.Chapters
                    .FirstOrDefault(a => a.Id.Equals(model.Id));

                chapter.Title = model.Title;

                db.Entry(chapter).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "Course", new { @id = chapter.CourseId });
            }

            return View(model);
        }

        // GET: Chapter/Create
        public ActionResult Create(int courseId, string courseName)
        {
            var model = new ChapterViewModel();
            model.CourseId = courseId;
            ViewBag.CourseName = courseName;

            return View(model);
        }

        // POST: Chapter/Create
        [HttpPost]
        public ActionResult Create(ChapterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var chapter = new Chapter(model.Title, model.CourseId);

                db.Chapters.Add(chapter);
                db.SaveChanges();

                return RedirectToAction("Details", "Course", new { @id = model.CourseId });
            }
            return View(model);
        }

        // GET: Chapter/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // POST: Chapter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Chapter chapter = db.Chapters.Find(id);
            var courseId = chapter.CourseId;
            db.Chapters.Remove(chapter);
            db.SaveChanges();
            return RedirectToAction("Details", "Course", new { @id = courseId });
        }

        public ActionResult Details(int? id)
        {
            if (id == null || !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            chapter.Posts = db.Posts.Where(x => x.ChapterId == id).ToList();
            return View(chapter);
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