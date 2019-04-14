
using Billing.DAL;
using Billing.Entities;
using Billing.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Billing.Web.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TicketReport()
        {
            TicketingReportViewModel model = new TicketingReportViewModel();
            model.TicketReport = new ReportDA().GetTicketingReportByDateRange(DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("MM/dd/yyyy"));
            model.VoidReport = new ReportDA().GetTicketingReportByDateRange(DateTime.Now.ToString("MM/dd/yyyy"), DateTime.Now.ToString("MM/dd/yyyy"));
            model.DailyStatement = new ReportDA().GetDailyCollectionSummary(DateTime.Now.ToString("MM/dd/yyyy"));
            ViewBag.VendorList = new SelectList(db.Vendors.OrderBy(a => a.Name).ToList(), "Id", "Name");
            ViewBag.AgentList = new SelectList(db.Agents.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View(model);
        }
        public PartialViewResult FilterTicketingReport(int AgentId, int VendorId, string Starting, string Finising)
        {
            TicketingReportViewModel model = new TicketingReportViewModel();
            if(AgentId == 0 && VendorId == 0)
            {
                model.TicketReport = new ReportDA().GetTicketingReportByDateRange(Starting, Finising);
            }
            else if (AgentId > 0 && VendorId == 0)
            {
                model.TicketReport = new ReportDA().GetTicketingReportByDateRangeAndAgent(Starting, Finising, AgentId);
            }
            else if (AgentId == 0 && VendorId > 0)
            {
                model.TicketReport = new ReportDA().GetTicketingReportByDateRangeAndVendor(Starting, Finising, VendorId);
            }
            else if (AgentId > 0 && VendorId > 0)
            {
                model.TicketReport = new ReportDA().GetTicketingReportByDateRangeAgentVendor(Starting, Finising, AgentId, VendorId);
            }
            return PartialView("Report/_TicketingReport", model.TicketReport);
        }
        public PartialViewResult FakeDailyStatement(string SearchDate)
        {
            TicketingReportViewModel Obj = new TicketingReportViewModel();
            Obj.DailyStatement = new ReportDA().GetDailyCollectionSummary(SearchDate);
            return PartialView("Report/FakeDailyStatement", Obj);
        }
        public ActionResult MakeInvoicePayment()
        {
            List<InvoicePaymentViewModel> lstObj = new List<InvoicePaymentViewModel>();
            List<Int32> InvIds = new ReportDA().GetInvoiceIds();
            TicketingReportViewModel model = new TicketingReportViewModel();
            model.TicketReport = new ReportDA().GetTicketingReportByDateRange("08/01/2014", "07/31/2015");
            List<TransactionEMP> transactions = new ReportDA().GetAllTransactionSummary();
            List<InvoiceName> iNames = new ReportDA().GetAllInvoiceNamesSummary();
            for (int i = 0; i < 10; i++)
            {
                InvoicePaymentViewModel Obj = new InvoicePaymentViewModel();
                Obj.Id = i + 1;
                Obj.InvoiceId = InvIds[i];
                Obj.Amount = iNames.Where(a => a.InvoiceId == InvIds[i]).Select(x => x.Amount).Sum();
                Obj.Received = transactions.Where(a => a.InvoiceId == InvIds[i]).Select(x => x.Amount).Sum();
                lstObj.Add(Obj);
            }
            return View();
        }        
    }
}