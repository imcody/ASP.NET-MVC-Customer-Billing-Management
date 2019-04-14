using Billing.DAL;
using Billing.DAL.Helpers;
using Billing.DAL.Parameters;
using Billing.Entities;
using Billing.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Vereyon.Web;

namespace Billing.Web.Controllers
{
    [Authorize]
    public class AccountingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LedgerHead()
        {
            return View(db.GeneralLeaderHeads.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LedgerHead(GeneralLedgerHead model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GeneralLedgerHead obj = new GeneralLedgerHead();
                    obj.GeneralLedgerHeadName = model.GeneralLedgerHeadName;
                    obj.GeneralLedgerType = model.GeneralLedgerType;
                    obj.Editable = true;
                    obj.Status = true;
                    db.GeneralLeaderHeads.Add(obj);
                    db.SaveChanges();
                    FlashMessage.Confirmation("New Ledger Head Added..");
                    return RedirectToAction("LedgerHead", "Accounting");
                }
                catch (Exception ex)
                {
                    FlashMessage.Danger(ex.Message.ToString());
                    return RedirectToAction("LedgerHead", "Accounting");
                }
            }
            return View();
        }
        public ActionResult Ledger()
        {
            GeneralVoucherPostingViewModel model = new GeneralVoucherPostingViewModel();
            model.GeneralLedgerHeads = db.GeneralLeaderHeads.Where(a => a.Editable == true).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Ledger(GeneralVoucherPostingViewModel model)
        {
            bool status = false;
            if (!ModelState.IsValid)
            {
                FlashMessage.Danger(ModelState.Values.ToString());
                return RedirectToAction("Ledger", "Accounting");
            }
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                GeneralVoucherPosting Obj = new GeneralVoucherPosting();
                Obj.Amount = model.Amount;
                Obj.Notes = model.Notes;
                Obj.LedgerHeadId = model.GeneralLedgerHeadId;
                Obj.UserID = User.Identity.GetUserId();
                Obj.LedgerDate = model.LedgerDate;
                status = new AccountingDA().InsertGeneralVoucherPosting(Obj);
                if (status) { FlashMessage.Confirmation("New Ledger Posted"); } else { FlashMessage.Danger("Some error occured!!"); }
                return RedirectToAction("Ledger", "Accounting");
            }
        }
        public ActionResult LedgerList(string type)
        {
            if(type == "Income")
            {
                List<GeneralLedgerListViewModel> model = new AccountingDA().GetGeneralLedgerList(1);
                ViewBag._LedgerHead = new SelectList(db.GeneralLeaderHeads, "Id", "GeneralLedgerHeadName");
                ViewBag.Type = "Income";
                return View(model);
            }
            else
            {
                List<GeneralLedgerListViewModel> model = new AccountingDA().GetGeneralLedgerList(2);
                ViewBag._LedgerHead = new SelectList(db.GeneralLeaderHeads, "Id", "GeneralLedgerHeadName");
                ViewBag.Type = "Expense";
                return View(model);
            }
        }
        public PartialViewResult FilterLedgerList(string FromDate, string ToDate, int? LedgerHead, int? PaymentMethod, int? StatementType)
        {
            List<GeneralLedgerListViewModel> model = new List<GeneralLedgerListViewModel>();
            string locFromDate = string.Empty;
            string locToDate = string.Empty;
            string locLedgerHead = string.Empty;
            string locPaymentMethod = string.Empty;
            string locStatementType = string.Empty;

            if (!string.IsNullOrEmpty(FromDate))
            {
                if (string.IsNullOrEmpty(ToDate) && LedgerHead == null && PaymentMethod == null)
                {
                    locFromDate = String.Format("WHERE CONVERT(varchar(10), [dbo].[GeneralLedgers].SysDateTime, 101) >= CONVERT(varchar(10), '{0}', 101)", FromDate);
                }
                else
                {
                    locFromDate = String.Format("WHERE CONVERT(varchar(10), [dbo].[GeneralLedgers].SysDateTime, 101) >= CONVERT(varchar(10), '{0}', 101) AND ", FromDate);
                }
            }
            if (!string.IsNullOrEmpty(ToDate))
            {
                if(LedgerHead == null && PaymentMethod == null)
                {
                    if (string.IsNullOrEmpty(locFromDate))
                    {
                        locToDate = String.Format("WHERE CONVERT(varchar(10), [dbo].[GeneralLedgers].SysDateTime, 101) <= CONVERT(varchar(10), '{0}', 101) ", ToDate);
                    }
                    else
                    {
                        locToDate = String.Format("CONVERT(varchar(10), [dbo].[GeneralLedgers].SysDateTime, 101) <= CONVERT(varchar(10), '{0}', 101) ", ToDate);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(locFromDate))
                    {
                        locToDate = String.Format("WHERE CONVERT(varchar(10), [dbo].[GeneralLedgers].SysDateTime, 101) <= CONVERT(varchar(10), '{0}', 101) ", ToDate);
                    }
                    else
                    {
                        locToDate = String.Format("CONVERT(varchar(10), [dbo].[GeneralLedgers].SysDateTime, 101) <= CONVERT(varchar(10), '{0}', 101) AND ", ToDate);
                    }
                }
            }
            if(LedgerHead != null)
            {
                if(PaymentMethod == null)
                {
                    if(string.IsNullOrEmpty(locFromDate) && string.IsNullOrEmpty(locToDate))
                    {
                        locLedgerHead = String.Format("WHERE [dbo].[GeneralLedgers].GeneralLedgerHeadId = {0} ", LedgerHead);
                    }
                    else
                    {
                        locLedgerHead = String.Format("[dbo].[GeneralLedgers].GeneralLedgerHeadId = {0} ", LedgerHead);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(locFromDate) && string.IsNullOrEmpty(locToDate))
                    {
                        locLedgerHead = String.Format("WHERE [dbo].[GeneralLedgers].GeneralLedgerHeadId = {0} AND ", LedgerHead);
                    }
                    else
                    {
                        locLedgerHead = String.Format("[dbo].[GeneralLedgers].GeneralLedgerHeadId = {0} AND ", LedgerHead);
                    }
                }
            }
            if (PaymentMethod != null)
            {
                if(string.IsNullOrEmpty(locFromDate) && string.IsNullOrEmpty(locToDate) && locLedgerHead == null)
                {
                    locPaymentMethod = String.Format("WHERE [dbo].[GeneralLedgers].PaymentMethods = {0} ", PaymentMethod);
                }
                else
                {
                    locPaymentMethod = String.Format("[dbo].[GeneralLedgers].PaymentMethods = {0} ", PaymentMethod);
                }
            }
            if(string.IsNullOrEmpty(locFromDate) && string.IsNullOrEmpty(locToDate) && locLedgerHead == null && locPaymentMethod == null)
            {
                locStatementType = String.Format("WHERE [dbo].[GeneralLedgers].StatementTypes = {0} ", StatementType);
            }
            else
            {
                locStatementType = String.Format("[dbo].[GeneralLedgers].StatementTypes = {0} ", StatementType);
            }

            string Query = String.Format("USE [TravelBilling] GO SELECT FORMAT([dbo].[GeneralLedgers].[SysDateTime], N'yyyy-MM-dd') AS LedgerDate, [dbo].[AspNetUsers].PersonName AS UserName, [dbo].[GeneralLedgerHeads].GeneralLedgerHeadName AS LedgerHead, [dbo].[GeneralLedgers].PaymentMethods AS PaymentMethod, [dbo].[GeneralLedgers].Notes AS Notes, (CASE WHEN[dbo].[GeneralLedgers].GeneralLedgerType = 1 THEN 'Debit' WHEN[dbo].[GeneralLedgers].GeneralLedgerType = 2 THEN 'Credit' END) AS LedgerType, [dbo].[GeneralLedgers].Amount AS Amount FROM[dbo].[GeneralLedgers] INNER JOIN[dbo].[AspNetUsers] ON[dbo].[GeneralLedgers].ApplicationUserId = [dbo].[AspNetUsers].Id INNER JOIN[dbo].[GeneralLedgerHeads] ON[dbo].[GeneralLedgers].GeneralLedgerHeadId = [dbo].[GeneralLedgerHeads].Id {0} {1} {2} {3} {4} ORDER BY[dbo].[GeneralLedgers].SysDateTime DESC END", locFromDate, locToDate, locLedgerHead, locPaymentMethod, locStatementType);
            model = new AccountingDA().FilterGeneralLedgerList(Query);
            return PartialView("LedgerListPartial", model);
        }
        public ActionResult FloatingCheque()
        {
            List<FloatChequeViewModel> model = new AccountingDA().GetFloatingChequeList((int)ChequeStatus.Floating);
            return View(model);
        }
        [HttpGet]
        public ActionResult ChequeRealize(int inv, string Name, int cheid)
        {
            try
            {
                if (inv < 1 || string.IsNullOrEmpty(Name) || cheid < 1)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    IPChequeDetail chequeObj = db.IPChequeDetails.Find(cheid);
                    Invoice invObj = db.Invoices.Find(inv);
                    if(chequeObj != null && invObj != null)
                    {
                        ChequeRealizeViewModel Obj = new ChequeRealizeViewModel();
                        Obj.AccountNo = chequeObj.AccountNo;
                        Obj.Agents = invObj.Agents;
                        Obj.Amount = chequeObj.Amount;
                        Obj.BankName = BankHelper.getBankName(chequeObj.BankNames.ToString());
                        Obj.BankNames = chequeObj.BankNames;
                        Obj.ChequeNo = chequeObj.ChequeNo;
                        Obj.InvoiceId = inv;
                        Obj.IPChequeDetailId = cheid;
                        Obj.Remarks = chequeObj.Remarks;
                        Obj.SortCode = chequeObj.SortCode;
                        return View(Obj);
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult ChequeRealize(ChequeRealizeViewModel model)
        {
            if (ModelState.IsValid)
            {
                int GeneralLedgerHeadId = (int)db.BankAccountLedgerHeads.Find(1).GeneralLedgerHeadId;
                GeneralLedger glObj = new GeneralLedger { Amount = model.Amount, ApplicationUserId = User.Identity.GetUserId(), GeneralLedgerHeadId = GeneralLedgerHeadId, Notes = model.RealizationRemarks + " - " + model.Remarks + " - " + "Cheque Realization", PaymentMethods = PaymentMethod.Cheque, StatementTypes = TransactionType.Income, SysDateTime = DateTime.Now, GeneralLedgerType = LedgerType.Credit };
                db.GeneralLedgers.Add(glObj);
                db.SaveChanges();

                Agent agent = db.Agents.Find(model.Agents.Id);
                agent.Balance = (agent.Balance - model.Amount);
                db.Entry(agent).State = EntityState.Modified;
                db.SaveChanges();

                AgentLedger alObj = new AgentLedger { AgentId = model.Agents.Id, AgentLedgerHeadId = 3, Amount = model.Amount, ApplicationUserId = glObj.ApplicationUserId, Balance = agent.Balance, Remarks = glObj.Notes, SystemDate = glObj.SysDateTime };
                db.AgentLedgers.Add(alObj);
                db.SaveChanges();

                BankAccount baObj = db.BankAccounts.Find(model.BankAccountId);
                baObj.Balance = (baObj.Balance + model.Amount);
                db.Entry(baObj).State = EntityState.Modified;
                db.SaveChanges();

                BankAccountLedger bclObj = new BankAccountLedger { Amount = model.Amount, ApplicationUserId = glObj.ApplicationUserId, Balance = baObj.Balance, BankAccountId = model.BankAccountId, BankAccountLedgerHeadId = 1, LedgerTypes = LedgerType.Credit, Notes = glObj.Notes, PaymentMethods = PaymentMethod.Cheque, RelationId = null, SysDateTime = glObj.SysDateTime };
                db.BankAccountLedgers.Add(bclObj);
                db.SaveChanges();

                InvoicePayment ipObj = new InvoicePayment { Amount = model.Amount, ApplicationUserId = glObj.ApplicationUserId, GeneralLedgerId = glObj.Id, InvoiceId = model.InvoiceId, PaymentMethods = PaymentMethod.Cheque, Remarks = model.RealizationRemarks + " - " + model.Remarks + " - " + "Cheque Realization", SysDateTime = glObj.SysDateTime, AgentLedgerId = alObj.Id, BankAccountLedgerId = bclObj.Id };
                db.InvoicePayments.Add(ipObj);
                db.SaveChanges();

                InvoiceLog ilObj = new InvoiceLog { ApplicationUserId = glObj.ApplicationUserId, InvoiceId = model.InvoiceId, Remarks = "Payment Received by Cheque Transaction - Realization", SysDateTime = glObj.SysDateTime };
                db.InvoiceLogs.Add(ilObj);
                db.SaveChanges();

                IPChequeDetail ipchObj = db.IPChequeDetails.Find(model.IPChequeDetailId);
                ipchObj.GeneralLedgerId = glObj.Id;
                ipchObj.InvoicePaymentId = ipObj.Id;
                ipchObj.BulkPayment = false;
                ipchObj.Status = ChequeStatus.Passed;
                db.Entry(ipchObj).State = EntityState.Modified;
                db.SaveChanges();

                bclObj.RelationId = ipchObj.Id;
                db.Entry(ipchObj).State = EntityState.Modified;
                db.SaveChanges();
                FlashMessage.Confirmation("Floating Cheque Realized");
                return RedirectToAction("FloatingCheque", "Accounting");
            }
            else
            {
                return RedirectToAction("FloatingCheque", "Accounting");
            }
        }
        public PartialViewResult GetBankAccounts(int BankId)
        {
            try
            {
                string sBankId = Convert.ToString(BankId);
                var result = (BankName)Enum.Parse(typeof(BankName), sBankId);
                List<BankAccount> Obj = db.BankAccounts.Where(e => e.BankNames == result).ToList();
                return PartialView(Obj);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult CashInHand()
        {
            ViewBag.CashInHand = new AccountingDA().GetCurrentCashInHand();
            return View();
        }
        
        public ActionResult EditHead(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralLedgerHead glHead = db.GeneralLeaderHeads.Find(id);
            if (glHead == null)
            {
                return HttpNotFound();
            }
            return View(glHead);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHead(GeneralLedgerHead glHead)
        {
            if (ModelState.IsValid)
            {
                glHead.Editable = true;
                db.Entry(glHead).State = EntityState.Modified;
                int i = db.SaveChanges();
                if (i > 0) { FlashMessage.Confirmation("Ledger Head Updated.."); } else { FlashMessage.Danger("Sorry, Something Went Wrong.."); }
                return RedirectToAction("LedgerHead");
            }
            return View(glHead);
        }
    }
}