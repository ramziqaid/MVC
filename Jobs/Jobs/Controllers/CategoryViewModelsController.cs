using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jobs.Models;

namespace Jobs.Controllers
{
    [Authorize (Roles ="Admin")]
    public class CategoryViewModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CategoryViewModels
        
        public async Task<ActionResult> Index()
        {
            return View(await db.CategoryViewModels.ToListAsync());
        }

        // GET: CategoryViewModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryViewModels categoryViewModels = await db.CategoryViewModels.FindAsync(id);
            if (categoryViewModels == null)
            {
                return HttpNotFound();
            }
            return View(categoryViewModels);
        }

        // GET: CategoryViewModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CategoryName,CategoryDescription")] CategoryViewModels categoryViewModels)
        {
            if (ModelState.IsValid)
            {
                db.CategoryViewModels.Add(categoryViewModels);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(categoryViewModels);
        }

        // GET: CategoryViewModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryViewModels categoryViewModels = await db.CategoryViewModels.FindAsync(id);
            if (categoryViewModels == null)
            {
                return HttpNotFound();
            }
            return View(categoryViewModels);
        }

        // POST: CategoryViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CategoryName,CategoryDescription")] CategoryViewModels categoryViewModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoryViewModels).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(categoryViewModels);
        }

        // GET: CategoryViewModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryViewModels categoryViewModels = await db.CategoryViewModels.FindAsync(id);
            if (categoryViewModels == null)
            {
                return HttpNotFound();
            }
            return View(categoryViewModels);
        }

        // POST: CategoryViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CategoryViewModels categoryViewModels = await db.CategoryViewModels.FindAsync(id);
            db.CategoryViewModels.Remove(categoryViewModels);
            await db.SaveChangesAsync();
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
