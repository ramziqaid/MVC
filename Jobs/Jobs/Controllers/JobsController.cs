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
using System.IO;
using Microsoft.AspNet.Identity;
using System.Net.Http;

namespace Jobs.Controllers
{
    [Authorize(Roles = "Authors")]
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Jobs
        public  ActionResult Index()
        {
            IEnumerable<Job> jobs = null;
            HttpResponseMessage result = GlobalVaribales.WebApiClient.GetAsync("Jobs").Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IEnumerable<Job>>();
                readTask.Wait();

                jobs = readTask.Result;
            }
            else //web api sent error response 
            {

                jobs = Enumerable.Empty<Job>();

                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

           // return View(jobs);

           // var jobs = db.Jobs.Include(j => j.Category);
            return View(  jobs);
        }

        // GET: Jobs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            Job job = null; 

            var postTask = GlobalVaribales.WebApiClient.GetAsync("Jobs/" + id.ToString());
            postTask.Wait();
            var result = postTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Job>();
                readTask.Wait();

                job =   readTask.Result;
            }
            else //web api sent error response 
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            return View(  job);
 
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.CategoryViewModels, "Id", "CategoryName");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Job job,HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                job.JobImgPath = upload.FileName;
                upload.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), upload.FileName));
                job.UserId = User.Identity.GetUserId();
                //db.Jobs.Add(job);
                //await db.SaveChangesAsync();
               // return RedirectToAction("Index");

                var postTask = GlobalVaribales.WebApiClient.PostAsJsonAsync("Jobs", job);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["Message"] = "تم حفظ الوظيفة!";
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            ViewBag.CategoryID = new SelectList(db.CategoryViewModels, "Id", "CategoryName", job.CategoryID);
            return View(job);
             
        }

        // GET: Jobs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Job job = null;

            var postTask = GlobalVaribales.WebApiClient.GetAsync("Jobs/" + id.ToString());
            postTask.Wait();
            var result = postTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Job>();
                readTask.Wait();

                job = readTask.Result;
            }
            else //web api sent error response 
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }
            
            ViewBag.CategoryID = new SelectList(db.CategoryViewModels, "Id", "CategoryName", job.CategoryID);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( Job job, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
              
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Uploads"), job.JobImgPath)); 

                if(upload != null)
                {
                    job.JobImgPath = upload.FileName;
                    upload.SaveAs(Path.Combine(Server.MapPath("~/Uploads"), upload.FileName));

                }
                else
                {

                }
                var postTask = GlobalVaribales.WebApiClient.PutAsJsonAsync("Jobs", job);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    TempData["Message"] = "تم حفظ التعديل!";
                    return RedirectToAction("Index");
                }
                //db.Entry(job).State = EntityState.Modified;
                //await db.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.CategoryViewModels, "Id", "CategoryName", job.CategoryID);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = null;
            var postTask = GlobalVaribales.WebApiClient.GetAsync("Jobs/" + id.ToString());
            postTask.Wait();
            var result = postTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<Job>();
                readTask.Wait();

                job = readTask.Result;
            } 
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            var postTask = GlobalVaribales.WebApiClient.DeleteAsync("Jobs/" + id.ToString());
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else //web api sent error response 
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

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
