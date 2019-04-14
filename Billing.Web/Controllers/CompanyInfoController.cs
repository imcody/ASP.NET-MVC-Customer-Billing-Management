using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using Billing.DAL;
using Billing.Entities;

namespace Billing.Web.Controllers
{
    [Authorize]
    public class CompanyInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CompanyInfo
        public ActionResult Index()
        {
            return RedirectToAction("Edit", "CompanyInfo", new { id = 1 }); //View(db.CompanyInfos.ToList());
        }

        // GET: CompanyInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInfo companyInfo = db.CompanyInfos.Find(id);
            if (companyInfo == null)
            {
                return HttpNotFound();
            }
            return View(companyInfo);
        }

        // POST: CompanyInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyInfo companyInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "CompanyInfo", new { id = 1 });
            }
            return View(companyInfo);
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
