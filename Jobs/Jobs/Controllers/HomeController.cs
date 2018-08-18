using Jobs.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Jobs.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var cat = db.CategoryViewModels.ToList();
            return View(cat);
        }
        // GET: Jobs/Details/5
        public ActionResult Details(int id)
        {

            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            Session["JobId"] = job.Id;
            return View(job);
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

        [HttpPost]
        public ActionResult Contact(ContactModels contactModels)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("ramzisoft2014@gmail.com", "Ebtihal611izmaR");
            mail.From = new MailAddress(contactModels.Email);
            mail.To.Add(new MailAddress("ramzi_soft@hotmail.com"));
            mail.Subject = contactModels.Message;

            SmtpClient smp = new SmtpClient("smpt.gmail.com", 587);
            smp.EnableSsl = true;
            smp.Credentials = loginInfo;
            smp.Send(mail);

            return RedirectToAction("Index");
            
        }

        [Authorize]
        //apply
        public ActionResult Apply()
        {

            return View();
        }

        [HttpPost]

        public ActionResult Apply(ApplyForJob applyForJob)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                int jobid = Convert.ToInt16(Session["JobId"]);
                if (db.ApplyForJob.Where(p => p.UserId == userId && p.JobId == jobid).ToList().Count < 1)
                {
                    ApplyForJob applyForJob2 = new ApplyForJob();
                    applyForJob.UserId = userId;
                    applyForJob.JobId = Convert.ToInt16(Session["JobId"]);
                    applyForJob.ApplyDate = DateTime.Now;

                    db.ApplyForJob.Add(applyForJob);
                    db.SaveChangesAsync();
                    ViewBag.Result = "تم التقديم";
                    return RedirectToAction("Index");
                }

                else
                {
                    ViewBag.Result = "قد تم التقديم سابقا";
                }

            }


            return View();
        }
        [Authorize]
        public ActionResult ApplyDetails()
        {

            IEnumerable<ApplyForJob> job = db.ApplyForJob.Where(p => p.UserId == User.Identity.GetUserId());
            if (job == null)
            {
                return HttpNotFound();
            }

            return View(db.ApplyForJob.ToList());
        }

        [HttpGet]
        [Authorize]
        public ActionResult ApplyEdit(int Id)
        {

            ApplyForJob job = db.ApplyForJob.Find(Id);
            if (job == null)
            {
                return HttpNotFound();
            }

            return View(job);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ApplyEdit(ApplyForJob applyForJob)
        {
            if (ModelState.IsValid)
            {
                // UpdateModel(applyForJob);
                applyForJob.ApplyDate = DateTime.Now;
                db.Entry(applyForJob).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ApplyDetails");


            }


            return View();
        }
        [Authorize]
        // GET: Roles/Delete/5
        public ActionResult ApplyDelete(int Id)
        {
            var role = db.ApplyForJob.Find(Id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult ApplyDelete(ApplyForJob job)
        {
            var applyForJob = db.ApplyForJob.Find(job.Id);
            db.ApplyForJob.Remove(applyForJob);
            db.SaveChanges();
            return RedirectToAction("ApplyDetails");
        }


        #region "JobByPublisher"
        [Authorize(Roles = "Authors")]

        public ActionResult GetJobByPublisher()
        {

            var userId = User.Identity.GetUserId();
            var result = from app in db.ApplyForJob
                         join job in db.Jobs
                         on app.JobId equals job.Id
                         where job.UserId == userId
                         select app;

            var group = from j in result
                        group j by j.Job.JobTitle into p
                        select new JobsViewModel
                        {
                            JobTitle = p.Key,
                            Items = p
                        };

            return View(group.ToList());
        }
        #endregion

        #region "Search"
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string search)
        {
            if (search.Length == 0)
            {
                ViewBag.Result = "يجب ادخال محدد البحث  ";
            }
             
            var result = db.Jobs.Where(a => a.JobTitle.Contains(search)
                 || a.JobDescription.Contains(search)
                 || a.Category.CategoryName.Contains(search)
                 || a.Category.CategoryDescription.Contains(search)).ToList();
            return View(result);
        }
        #endregion
    }
}