using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Globalization;
using Vereyon.Web;
using Billing.DAL;
using Billing.Entities;
using Billing.DAL.Parameters;
using Billing.ViewModel;

namespace Billing.Web.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            List<Agent> lstLstObj = new List<Agent>();
            lstLstObj = new AgentDA().GetAgentsList();
            return View(lstLstObj);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Agent agent)
        {
            if (ModelState.IsValid)
            {
                Agent model = new Agent();
                model.Address = string.IsNullOrEmpty(agent.Address) ? string.Empty : agent.Address;
                model.ApplicationUserId = User.Identity.GetUserId();
                model.Atol = string.IsNullOrEmpty(agent.Atol) ? string.Empty : agent.Atol;
                model.Balance = Convert.ToDouble(agent.Balance);
                model.CreditLimit = Convert.ToDouble(agent.CreditLimit);
                model.Email = string.IsNullOrEmpty(agent.Email) ? string.Empty : agent.Email;
                model.FaxNo = string.IsNullOrEmpty(agent.FaxNo) ? string.Empty : agent.FaxNo;
                model.JoiningDate = DateTime.Now;
                model.Mobile = string.IsNullOrEmpty(agent.Mobile) ? string.Empty : agent.Mobile;
                model.Name = string.IsNullOrEmpty(agent.Name) ? string.Empty : agent.Name;
                model.Postcode = string.IsNullOrEmpty(agent.Postcode) ? string.Empty : agent.Postcode;
                model.ProfileType = ProfileType.Agent;
                model.Remarks = string.IsNullOrEmpty(agent.Remarks) ? string.Empty : agent.Remarks;
                model.Telephone = string.IsNullOrEmpty(agent.Telephone) ? string.Empty : agent.Telephone;
                bool status = new AgentDA().AddNewAgent(model);
                if (status)
                {
                    FlashMessage.Confirmation("New Agent Created");
                    return RedirectToAction("Index", "Agent");
                }
                else
                {
                    FlashMessage.Danger("Sorry, Something Went Wrong");
                    return RedirectToAction("Index", "Agent");
                }
            }
            return View(agent);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Agent agent)
        {
            if (ModelState.IsValid)
            {
                Agent model = db.Agents.Find(agent.Id);
                model.Address = agent.Address;
                model.Atol = agent.Atol;
                model.CreditLimit = agent.CreditLimit;
                model.Email = agent.Email;
                model.FaxNo = agent.FaxNo;
                model.Mobile = agent.Mobile;
                model.Name = agent.Name;
                model.Postcode = agent.Postcode;
                model.ProfileType = ProfileType.Agent;
                model.Remarks = agent.Remarks;
                model.Telephone = agent.Telephone;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                FlashMessage.Confirmation("Agent Informaion Updated..");
                return RedirectToAction("Index", "Agent");
            }
            return View(agent);
        }
        public ActionResult Ledger(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<AgentLedgerViewModel> lstAL = new AgentDA().AgentsLedgerList((int)id); //db.AgentLedgers.Where(x => x.AgentId == id).OrderByDescending(x => x.SystemDate).Take(150).ToList();
            if (lstAL == null)
            {
                return HttpNotFound();
            }
            ViewBag._LedgerHead = new SelectList(db.AgentLedgerHeads, "Id", "LedgerHead");
            ViewBag.CurrentAgentId = id;
            ViewBag.AgentName = db.Agents.Find(id).Name;
            return View(lstAL);
        }
        public PartialViewResult FilterAgentLedgerList(string searchType, int? AgentId, int? InvoiceId, int? LedgerHead, int? PaymentMethod, string sDate, string eDate)
        {
            List<AgentLedgerViewModel> lstAL = new List<AgentLedgerViewModel>();
            try
            {
                #region Search By Date Range
                if (searchType == "SearchByDateRange")
                {
                    DateTime StartDate;
                    DateTime FinisDate;
                    if (!DateTime.TryParseExact(sDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out StartDate))
                    {
                        sDate = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    if (!DateTime.TryParseExact(eDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FinisDate))
                    {
                        eDate = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    lstAL = new AgentDA().AgentsLedgerListSearchByDateRange((int)AgentId, sDate, eDate);
                }
                #endregion
                #region Search by Date Range & Ledger Head
                else if (searchType == "SearchByDateRangeLedgerHead")
                {
                    DateTime StartDate;
                    DateTime FinisDate;
                    if (!DateTime.TryParseExact(sDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out StartDate))
                    {
                        sDate = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    if (!DateTime.TryParseExact(eDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FinisDate))
                    {
                        eDate = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    lstAL = new AgentDA().AgentsLedgerListSearchByDateRangeLedgerHead((int)AgentId, sDate, eDate, (int)LedgerHead);
                } 
                #endregion
            }
            catch(Exception ex)
            {

            }
            return PartialView("Agent/AgentLedgerTable", lstAL);
        }
        public ActionResult BulkPayment()
        {
            ViewBag.AgentList = new SelectList(db.Agents.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult BulkPayment(FormCollection form)
        {
            try
            {
                string IndetityVals = string.Empty;
                int AgentId = (string.IsNullOrEmpty(form["AgentId"])) ? 0 : Convert.ToInt32(form["AgentId"]);
                if(AgentId > 0)
                {
                    int InvoiceCount = (string.IsNullOrEmpty(form["InvoiceCount"])) ? 0 : Convert.ToInt32(form["InvoiceCount"]);
                    int PaymentMethod = (string.IsNullOrEmpty(form["PaymentMethod"])) ? 0 : Convert.ToInt32(form["PaymentMethod"]);
                    #region Agent Bulk Payment - Payment Method * Cash
                    if (PaymentMethod == 1 && InvoiceCount > 0)
                    {
                        AgentBulkPaymentCashVoucher Obj = new AgentBulkPaymentCashVoucher();
                        Obj.AgentId = AgentId;
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                        Obj.UserId = User.Identity.GetUserId();
                        IndetityVals = new AgentDA().AgentBulkPaymentCashVoucher(Obj);
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 2 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentAgentCashInvPaymentInvLog lObj = new BulkPaymentAgentCashInvPaymentInvLog();
                                lObj.AgentId = AgentId;
                                lObj.AgentLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = 0;
                                if(lObj.Amount > 0)
                                {
                                    new AgentDA().BulkPaymentAgentCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                    }
                    #endregion
                    #region Agent Bulk Payment - Payment Method * Cheque
                    else if (PaymentMethod == 2)
                    {
                        AgentBulkPaymentChequeVoucher Obj = new AgentBulkPaymentChequeVoucher();
                        Obj.AccountNo = (string.IsNullOrEmpty(form["chkAccountNo"])) ? string.Empty : Convert.ToString(form["chkAccountNo"]);
                        Obj.AgentId = (string.IsNullOrEmpty(form["AgentId"])) ? 0 : Convert.ToInt32(form["AgentId"]);
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.BankNames = (string.IsNullOrEmpty(form["chkBankId"])) ? 0 : Convert.ToInt32(form["chkBankId"]);
                        Obj.ChequeNo = (string.IsNullOrEmpty(form["chkChequeNo"])) ? string.Empty : Convert.ToString(form["chkChequeNo"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Remarks = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                        Obj.SortCode = (string.IsNullOrEmpty(form["chkSortCode"])) ? string.Empty : Convert.ToString(form["chkSortCode"]);
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.BulkPayment = true;
                        new AgentDA().AgentBulkPaymentChequeVoucher(Obj);
                    } 
                    #endregion
                    #region Agent Bulk Payment - Payment Method * Credit Card
                    else if (PaymentMethod == 3 && InvoiceCount > 0)
                    {
                        AgentBulkPaymentCreditCard Obj = new AgentBulkPaymentCreditCard();
                        Obj.AgentId = (string.IsNullOrEmpty(form["AgentId"])) ? 0 : Convert.ToInt32(form["AgentId"]);
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountIdCC"])) ? 0 : Convert.ToInt32(form["BankAccountIdCC"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.CardNo = (string.IsNullOrEmpty(form["CreditCardNo"])) ? string.Empty : Convert.ToString(form["CreditCardNo"]);
                        Obj.CardHolder = (string.IsNullOrEmpty(form["CardHolderName"])) ? string.Empty : Convert.ToString(form["CardHolderName"]);
                        Obj.ExtraAmount = (string.IsNullOrEmpty(form["ExtraAmount"])) ? string.Empty : Convert.ToString(form["ExtraAmount"]);
                        Obj.BankDate = (string.IsNullOrEmpty(form["BankDate"])) ? string.Empty : Convert.ToString(form["BankDate"]);
                        IndetityVals = new AgentDA().AgentBulkPaymentCreditCard(Obj);
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 3 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentAgentCashInvPaymentInvLog lObj = new BulkPaymentAgentCashInvPaymentInvLog();
                                lObj.AgentId = AgentId;
                                lObj.AgentLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = Convert.ToInt32(SplitVals[2]);
                                if (lObj.Amount > 0)
                                {
                                    new AgentDA().BulkPaymentAgentCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                    }
                    #endregion
                    #region Agent Bulk Payment - Payment Method * Debit Card
                    else if (PaymentMethod == 4 && InvoiceCount > 0)
                    {
                        AgentBulkPaymentDebitCard Obj = new AgentBulkPaymentDebitCard();
                        Obj.AgentId = (string.IsNullOrEmpty(form["AgentId"])) ? 0 : Convert.ToInt32(form["AgentId"]);
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountIdDC"])) ? 0 : Convert.ToInt32(form["BankAccountIdDC"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.CardNo = (string.IsNullOrEmpty(form["CreditCardNo"])) ? string.Empty : Convert.ToString(form["CreditCardNo"]);
                        Obj.CardHolder = (string.IsNullOrEmpty(form["CardHolderName"])) ? string.Empty : Convert.ToString(form["CardHolderName"]);
                        Obj.ExtraAmount = (string.IsNullOrEmpty(form["ExtraAmount"])) ? string.Empty : Convert.ToString(form["ExtraAmount"]);
                        Obj.BankDate = (string.IsNullOrEmpty(form["BankDate"])) ? string.Empty : Convert.ToString(form["BankDate"]);
                        IndetityVals = new AgentDA().AgentBulkPaymentDebitCard(Obj);
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 3 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentAgentCashInvPaymentInvLog lObj = new BulkPaymentAgentCashInvPaymentInvLog();
                                lObj.AgentId = AgentId;
                                lObj.AgentLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = Convert.ToInt32(SplitVals[2]);
                                if (lObj.Amount > 0)
                                {
                                    new AgentDA().BulkPaymentAgentCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                    }
                    #endregion
                    #region Agent Bulk Payment - Payment Method * Bank Deposit
                    else if (PaymentMethod == 5 && InvoiceCount > 0)
                    {
                        AgentBulkPaymentBankDeposit Obj = new AgentBulkPaymentBankDeposit();
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.AgentId = (string.IsNullOrEmpty(form["AgentId"])) ? 0 : Convert.ToInt32(form["AgentId"]);
                        Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountIdBP"])) ? 0 : Convert.ToInt32(form["BankAccountIdBP"]);
                        Obj.BankDate = (string.IsNullOrEmpty(form["BankDateBankDeposit"])) ? string.Empty : Convert.ToString(form["BankDateBankDeposit"]);
                        IndetityVals = new AgentDA().AgentBulkPaymentBankDeposit(Obj);
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 3 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentAgentCashInvPaymentInvLog lObj = new BulkPaymentAgentCashInvPaymentInvLog();
                                lObj.AgentId = AgentId;
                                lObj.AgentLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = Convert.ToInt32(SplitVals[2]);
                                if (lObj.Amount > 0)
                                {
                                    new AgentDA().BulkPaymentAgentCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                    } 
                    #endregion
                }
                return RedirectToAction("BulkPayment", "Agent");
            }
            catch (Exception ex)
            {
                return RedirectToAction("BulkPayment", "Agent");
            }
        }
        public PartialViewResult GetBankAccountsCC(int BankId)
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
        public PartialViewResult GetBankAccountsDC(int BankId)
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
        public PartialViewResult GetBankAccountsBP(int BankId)
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
        public PartialViewResult GetAgentOutstandingInvoice(int? AgentId)
        {
            List<AgentOutstandingInvoiceListViewModel> lstObj = new List<AgentOutstandingInvoiceListViewModel>();
            try
            {
                if (AgentId != null)
                {
                    lstObj = new AgentDA().GetAgentOutstandingInvoiceList((int)AgentId);
                    return PartialView("Agent/AgentOutstandingInvoiceList", lstObj);
                }
                else
                {
                    return PartialView("Agent/AgentOutstandingInvoiceList", lstObj);
                }
            }
            catch (Exception)
            {
                return PartialView("Agent/AgentOutstandingInvoiceList", lstObj);
            }
        }
        public ActionResult ChequeRealize(string Name, int cheid, int inv)
        {
            IPChequeDetail vObj = new AgentDA().AgentBulkPaymentFloatingCheque(cheid);
            ViewBag.AccountNo = vObj.AccountNo;
            ViewBag.Amount = vObj.Amount;
            ViewBag.LedgerDate = vObj.SysCreateDate.ToString("yyyy-MM-dd");
            ViewBag.Notes = vObj.Remarks;
            ViewBag.ChekId = cheid;
            ViewBag.AgentList = new SelectList(db.Agents.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult ChequeRealize(FormCollection form)
        {
            int ChekId = (string.IsNullOrEmpty(form["ChekId"])) ? 0 : Convert.ToInt32(form["ChekId"]);
            try
            {
                string IndetityVals = string.Empty;
                int InvoiceCount = (string.IsNullOrEmpty(form["InvoiceCount"])) ? 0 : Convert.ToInt32(form["InvoiceCount"]);
                int AgentId = (string.IsNullOrEmpty(form["AgentId"])) ? 0 : Convert.ToInt32(form["AgentId"]);
                AgentBulkPaymentChequePayment Obj = new AgentBulkPaymentChequePayment();
                Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0}", string.Empty) : String.Format("{0}", Convert.ToString(form["Notes"]));
                Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                Obj.UserId = User.Identity.GetUserId();
                Obj.AgentId = AgentId;
                Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountIdBP"])) ? 0 : Convert.ToInt32(form["BankAccountIdBP"]);
                Obj.BankDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                Obj.ChekId = ChekId;
                IndetityVals = new AgentDA().AgentBulkPaymentChequePayment(Obj);
                string[] SplitVals = IndetityVals.Split(',');
                if (SplitVals.Length == 3 && Convert.ToInt32(SplitVals[0]) > 0)
                {
                    for (int i = 1; i <= InvoiceCount; i++)
                    {
                        BulkPaymentAgentCashInvPaymentInvLog lObj = new BulkPaymentAgentCashInvPaymentInvLog();
                        lObj.AgentId = AgentId;
                        lObj.AgentLedgerId = Convert.ToInt32(SplitVals[1]);
                        lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                        lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                        lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                        lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0}", string.Empty) : String.Format("{0}", Convert.ToString(form["Notes"]));
                        lObj.UserID = User.Identity.GetUserId();
                        lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        lObj.BankAccountLedgerId = Convert.ToInt32(SplitVals[2]);
                        if (lObj.Amount > 0)
                        {
                            new AgentDA().BulkPaymentAgentCashVoucherInvoicePaymentInvoiceLog(lObj);
                        }
                    }
                }
                return RedirectToAction("InvoicePayment", "Agent");
            }
            catch(Exception ec)
            {
                return RedirectToAction("InvoicePayment", "Agent");
            }
        }
        public ActionResult InvoicePayment()
        {
            List<InvoicePaymentHistory> lstObj = new List<InvoicePaymentHistory>();//AgentDA().GetInvoicePaymentHistory(51);
            ViewBag.AgentList = new SelectList(db.Agents.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View(lstObj);
        }
        public PartialViewResult InvoicePaymentHistory(string searchType, int? AgentId, int? InvoiceId, string sDate, string eDate)
        {
            List<InvoicePaymentHistory> lstObj = new List<InvoicePaymentHistory>();
            try
            {
                #region Search By Only Agent Id
                if (searchType == "SearchByAgent")
                {
                    lstObj = new AgentDA().GetInvoicePaymentHistoryByAgent((int)AgentId);
                    return PartialView("Agent/AgentInvoicePayment", lstObj);
                }
                #endregion
                #region Search by Only Invocie Id
                else if (searchType == "SearchByInvoice")
                {
                    lstObj = new AgentDA().GetInvoicePaymentHistoryByInvoice((int)InvoiceId);
                    return PartialView("Agent/AgentInvoicePayment", lstObj);
                }
                #endregion
                #region Search By Only Date Range
                else if (searchType == "SearchByDateRange")
                {
                    DateTime StartDate;
                    DateTime FinisDate;
                    if (!DateTime.TryParseExact(sDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out StartDate))
                    {
                        sDate = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    if (!DateTime.TryParseExact(eDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FinisDate))
                    {
                        eDate = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    lstObj = new AgentDA().GetInvoicePaymentHistoryByDateRange(sDate, eDate);
                    return PartialView("Agent/AgentInvoicePayment", lstObj);
                }
                #endregion
                #region Search Only by Date Range & Agent
                else if (searchType == "SearchByDateAgent")
                {
                    DateTime StartDate;
                    DateTime FinisDate;
                    if (!DateTime.TryParseExact(sDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out StartDate))
                    {
                        sDate = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    if (!DateTime.TryParseExact(eDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FinisDate))
                    {
                        eDate = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                    lstObj = new AgentDA().GetInvoicePaymentHistoryByDateAgent((int)AgentId, sDate, eDate);
                    return PartialView("Agent/AgentInvoicePayment", lstObj);
                }
                #endregion
                #region Else Statement
                else
                {
                    return PartialView("Agent/AgentInvoicePayment", lstObj);
                } 
                #endregion
            }
            catch(Exception ec)
            {
                return PartialView("Agent/AgentInvoicePayment", lstObj);
            }
        }
        public ActionResult AgentOutstanding()
        {
            List<InvoiceListViewModel> lstObj = new List<InvoiceListViewModel>();
            ViewBag.AgentList = new SelectList(db.Agents.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View(lstObj);
        }
        public PartialViewResult GetAgentOutstanding(int AgentId)
        {
            List<InvoiceListViewModel> lstObj = new AgentDA().GetOutstandingInvoiceListByAgentId(AgentId);
            return PartialView("Agent/OutstandingInvoiceList", lstObj);
        }
        public JsonResult GetAgentInformation(int AgentId)
        {
            try
            {
                Agent Obj = db.Agents.Find(AgentId);
                if (Obj == null)
                {
                    return Json(new
                    {
                        Name = "Not Found",
                        Address = "Not Found",
                        Mobile = "Not Found",
                        Email = "Not Found",
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Name = Obj.Name,
                        Address = Obj.Address,
                        Mobile = Obj.Mobile,
                        Email = Obj.Email,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Name = "Not Found",
                    Address = "Not Found",
                    Mobile = "Not Found",
                    Email = "Not Found",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult OutstandingOtherInvoice()
        {
            List<OtherInvoiceList> lstObj = new List<OtherInvoiceList>();
            ViewBag.AgentList = new SelectList(db.Agents.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View(lstObj);
        }
        public PartialViewResult SearchAgentOutstandingOtherInvoice(int AgentId)
        {
            List<OtherInvoiceList> lstObj = new OtherInvoiceDA().GetAgentOutstandingOtherInvoiceList(AgentId);
            return PartialView("OtherInvoice/OtherInvoiceList", lstObj);
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
