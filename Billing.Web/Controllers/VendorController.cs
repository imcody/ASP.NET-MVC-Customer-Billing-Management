
using Billing.DAL;
using Billing.DAL.Parameters;
using Billing.Entities;
using Billing.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Billing.Web.Controllers
{
    [Authorize]
    public class VendorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            List<VendorListViewModel> viewModel = new VendorDA().getVendorList();
            return View(viewModel);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateVendorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                Vendor vObj = new Vendor();
                vObj.AddedOn = DateTime.Now;
                vObj.Address = model.Address;
                vObj.ApplicationUserId = User.Identity.GetUserId();
                vObj.Atol = model.Atol;
                vObj.Balance = model.Balance;
                vObj.Email = model.Email;
                vObj.FaxNo = model.FaxNo;
                vObj.Name = model.Name;
                vObj.NetSafi = model.NetSafi;
                vObj.Telephone = model.Telephone;
                db.Vendors.Add(vObj);
                db.SaveChanges();
                Response.Cookies.Add(new HttpCookie("FlashMessage", "Success! New Vendor Added.<br />A/C No: " + vObj.Id) { Path = " /" });
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index", "Vendor");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            EditVendorViewModel vVendor = new EditVendorViewModel();
            vVendor.Id = vendor.Id;
            vVendor.Address = vendor.Address;
            vVendor.Atol = vendor.Atol;
            vVendor.Email = vendor.Email;
            vVendor.FaxNo = vendor.FaxNo;
            vVendor.Name = vendor.Name;
            vVendor.NetSafi = vendor.NetSafi;
            vVendor.Telephone = vendor.Telephone;
            return View(vVendor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditVendorViewModel vVendor)
        {
            if (ModelState.IsValid)
            {

                Vendor vendor = db.Vendors.Find(vVendor.Id);
                vendor.Address = vVendor.Address;
                vendor.Atol = vVendor.Atol;
                vendor.Email = vVendor.Email;
                vendor.FaxNo = vVendor.FaxNo;
                vendor.Name = vVendor.Name;
                vendor.NetSafi = vVendor.NetSafi;
                vendor.Telephone = vVendor.Telephone;

                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();

                Response.Cookies.Add(new HttpCookie("FlashMessage", "Edit Complete!!") { Path = " /" });
                return RedirectToAction("Index", "Vendor");
            }
            return View();
        }
        public ActionResult Ledger(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<VendorLedgerViewModel> lstObj = new VendorDA().VendorsLedgerList((int)id);
            if (lstObj == null)
            {
                return HttpNotFound();
            }
            ViewBag._LedgerHead = new SelectList(db.VendorLedgerHeads, "Id", "LedgerHead");
            ViewBag.CurrentVendorId = id;
            ViewBag.VendorName = db.Vendors.Find(id).Name;
            return View(lstObj);
        }
        public PartialViewResult FilterVendorLedgerList(string searchType, int? VendorId, int? InvoiceId, int? LedgerHead, int? PaymentMethod, string sDate, string eDate)
        {
            List<VendorLedgerViewModel> lstAL = new List<VendorLedgerViewModel>();
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
                    lstAL = new VendorDA().VendorsLedgerListSearchByDateRange((int)VendorId, sDate, eDate);
                }
                #endregion
                #region Search By Date Range & Ledger Head
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
                    lstAL = new VendorDA().VendorsLedgerListSearchByDateRangeLedgerHead((int)VendorId, sDate, eDate, (int)LedgerHead);
                } 
                #endregion
            }
            catch (Exception ex)
            {

            }
            return PartialView("Vendor/VendorLedgerTable", lstAL);
        }
        public ActionResult InvoicePayment()
        {
            List<InvoicePaymentHistoryVendor> lstObj = new List<InvoicePaymentHistoryVendor>();
            ViewBag.VendorList = new SelectList(db.Vendors.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View(lstObj);
        }
        public PartialViewResult InvoicePaymentHistory(string searchType, int? VendorId, int? InvoiceId, string sDate, string eDate)
        {
            List<InvoicePaymentHistoryVendor> lstObj = new List<InvoicePaymentHistoryVendor>();
            try
            {
                #region Search By Only Agent Id
                if (searchType == "SearchByVendor")
                {
                    lstObj = new VendorDA().GetInvoicePaymentHistoryByVendor((int)VendorId);
                    return PartialView("Vendor/VendorInvoicePayment", lstObj);
                }
                #endregion
                #region Search by Only Invocie Id
                else if (searchType == "SearchByInvoice")
                {
                    lstObj = new VendorDA().GetInvoicePaymentHistoryByInvoiceAndVendor((int)InvoiceId);
                    return PartialView("Vendor/VendorInvoicePayment", lstObj);
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
                    lstObj = new VendorDA().GetInvoicePaymentHistoryByDateRangeVendor(sDate, eDate);
                    return PartialView("Vendor/VendorInvoicePayment", lstObj);
                }
                #endregion
                #region Search Only by Date Range & Agent
                else if (searchType == "SearchByDateVendor")
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
                    lstObj = new VendorDA().GetInvoicePaymentHistoryByDateVendor((int)VendorId, sDate, eDate);
                    return PartialView("Vendor/VendorInvoicePayment", lstObj);
                }
                #endregion
                #region Else Statement
                else
                {
                    return PartialView("Agent/AgentInvoicePayment", lstObj);
                }
                #endregion
            }
            catch (Exception ec)
            {
                return PartialView("Vendor/VendorInvoicePayment", lstObj);
            }
        }
        public ActionResult BulkPayment()
        {
            ViewBag.VendorList = new SelectList(db.Vendors.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult BulkPayment(FormCollection form)
        {
            try
            {
                string IndetityVals = string.Empty;
                int VendorId = (string.IsNullOrEmpty(form["VendorId"])) ? 0 : Convert.ToInt32(form["VendorId"]);
                if (VendorId > 0)
                {
                    int InvoiceCount = (string.IsNullOrEmpty(form["InvoiceCount"])) ? 0 : Convert.ToInt32(form["InvoiceCount"]);
                    int PaymentMethod = (string.IsNullOrEmpty(form["PaymentMethod"])) ? 0 : Convert.ToInt32(form["PaymentMethod"]);
                    bool Checked = Convert.ToBoolean(form["JournalPayment"].Split(',')[0]);
                    #region Vendor Bulk Payment - Payment Method * Cash
                    if (PaymentMethod == 1 && InvoiceCount > 0)
                    {
                        VendorBulkPaymentCashVoucher Obj = new VendorBulkPaymentCashVoucher();
                        Obj.VendorId = VendorId;
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment", string.Empty) : String.Format("{0} - Vendor Bulk Payment", Convert.ToString(form["Notes"]));
                        Obj.UserId = User.Identity.GetUserId();
                        if (!Checked)
                        {
                            IndetityVals = new VendorDA().VendorBulkPaymentCashVoucher(Obj);
                        }
                        else
                        {
                            IndetityVals = new VendorDA().VendorJournalPaymentCashVoucher(Obj);
                        }
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 2 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = 0;
                                if (lObj.Amount > 0)
                                {
                                    new VendorDA().BulkPaymentVendorCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                        else if (SplitVals.Length == 1 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = 0;
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = 0;
                                if (lObj.Amount > 0)
                                {
                                    new VendorDA().BulkPaymentVendorCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                    }
                    #endregion
                    #region Vendor Bulk Payment - Payment Method * Cheque
                    else if (PaymentMethod == 2)
                    {
                        VendorBulkPaymentChequeVoucher Obj = new VendorBulkPaymentChequeVoucher();
                        Obj.AccountNo = (string.IsNullOrEmpty(form["chkAccountNo"])) ? string.Empty : Convert.ToString(form["chkAccountNo"]);
                        Obj.VendorId = VendorId;
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.BankNames = (string.IsNullOrEmpty(form["chkBankId"])) ? 0 : Convert.ToInt32(form["chkBankId"]);
                        Obj.ChequeNo = (string.IsNullOrEmpty(form["chkChequeNo"])) ? string.Empty : Convert.ToString(form["chkChequeNo"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Remarks = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment", string.Empty) : String.Format("{0} - Vendor Bulk Payment", Convert.ToString(form["Notes"]));
                        Obj.SortCode = (string.IsNullOrEmpty(form["chkSortCode"])) ? string.Empty : Convert.ToString(form["chkSortCode"]);
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.BulkPayment = true;
                        new VendorDA().VendorBulkPaymentChequeVoucher(Obj);
                    }
                    #endregion
                    #region Vendor Bulk Payment - Payment Method * Credit Card
                    else if (PaymentMethod == 3 && InvoiceCount > 0)
                    {
                        VendorBulkPaymentCreditCard Obj = new VendorBulkPaymentCreditCard();
                        Obj.VendorId = VendorId;
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountIdCC"])) ? 0 : Convert.ToInt32(form["BankAccountIdCC"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment", string.Empty) : String.Format("{0} - Vendor Bulk Payment", Convert.ToString(form["Notes"]));
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.CardNo = (string.IsNullOrEmpty(form["CreditCardNo"])) ? string.Empty : Convert.ToString(form["CreditCardNo"]);
                        Obj.CardHolder = (string.IsNullOrEmpty(form["CardHolderName"])) ? string.Empty : Convert.ToString(form["CardHolderName"]);
                        Obj.ExtraAmount = (string.IsNullOrEmpty(form["ExtraAmount"])) ? string.Empty : Convert.ToString(form["ExtraAmount"]);
                        Obj.BankDate = (string.IsNullOrEmpty(form["BankDate"])) ? string.Empty : Convert.ToString(form["BankDate"]);
                        if (!Checked)
                        {
                            IndetityVals = new VendorDA().VendorBulkPaymentCreditCard(Obj); 
                        }
                        else
                        {
                            VendorBulkPaymentCashVoucher lObj = new VendorBulkPaymentCashVoucher { VendorId = VendorId, Amount = Obj.Amount, LedgerDate = Obj.LedgerDate, Notes = Obj.Notes, UserId = Obj.UserId };
                            IndetityVals = new VendorDA().VendorJournalPaymentCashVoucher(lObj);
                        }
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 3 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment", string.Empty) : String.Format("{0} - Vendor Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = Convert.ToInt32(SplitVals[2]);
                                if (lObj.Amount > 0)
                                {
                                    new VendorDA().BulkPaymentVendorCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                        else if (SplitVals.Length == 1 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = 0;
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = 0;
                                if (lObj.Amount > 0)
                                {
                                    new VendorDA().BulkPaymentVendorCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                    }
                    #endregion
                    #region Vendor Bulk Payment - Payment Method * Debit Card
                    else if (PaymentMethod == 4 && InvoiceCount > 0)
                    {
                        VendorBulkPaymentDebitCard Obj = new VendorBulkPaymentDebitCard();
                        Obj.VendorId = VendorId;
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountIdDC"])) ? 0 : Convert.ToInt32(form["BankAccountIdDC"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment", string.Empty) : String.Format("{0} - Vendor Bulk Payment", Convert.ToString(form["Notes"]));
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.CardNo = (string.IsNullOrEmpty(form["CreditCardNo"])) ? string.Empty : Convert.ToString(form["CreditCardNo"]);
                        Obj.CardHolder = (string.IsNullOrEmpty(form["CardHolderName"])) ? string.Empty : Convert.ToString(form["CardHolderName"]);
                        Obj.ExtraAmount = (string.IsNullOrEmpty(form["ExtraAmount"])) ? string.Empty : Convert.ToString(form["ExtraAmount"]);
                        Obj.BankDate = (string.IsNullOrEmpty(form["BankDate"])) ? string.Empty : Convert.ToString(form["BankDate"]);
                        if (!Checked)
                        {
                            IndetityVals = new VendorDA().VendorBulkPaymentDebitCard(Obj);
                        }
                        else
                        {
                            VendorBulkPaymentCashVoucher lObj = new VendorBulkPaymentCashVoucher { VendorId = VendorId, Amount = Obj.Amount, LedgerDate = Obj.LedgerDate, Notes = Obj.Notes, UserId = Obj.UserId };
                            IndetityVals = new VendorDA().VendorJournalPaymentCashVoucher(lObj);
                        }
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 3 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = Convert.ToInt32(SplitVals[2]);
                                if (lObj.Amount > 0)
                                {
                                    new VendorDA().BulkPaymentVendorCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                        else if (SplitVals.Length == 1 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = 0;
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = 0;
                                if (lObj.Amount > 0)
                                {
                                    new VendorDA().BulkPaymentVendorCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                    }
                    #endregion
                    #region Vendor Bulk Payment - Payment Method * Bank Deposit
                    else if (PaymentMethod == 5 && InvoiceCount > 0)
                    {
                        VendorBulkPaymentBankDeposit Obj = new VendorBulkPaymentBankDeposit();
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.VendorId = VendorId;
                        Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountIdBP"])) ? 0 : Convert.ToInt32(form["BankAccountIdBP"]);
                        Obj.BankDate = (string.IsNullOrEmpty(form["BankDateBankDeposit"])) ? string.Empty : Convert.ToString(form["BankDateBankDeposit"]);
                        if (!Checked)
                        {
                            IndetityVals = new VendorDA().VendorBulkPaymentBankDeposit(Obj);
                        }
                        else
                        {
                            VendorBulkPaymentCashVoucher lObj = new VendorBulkPaymentCashVoucher { VendorId = VendorId, Amount = Obj.Amount, LedgerDate = Obj.LedgerDate, Notes = Obj.Notes, UserId = Obj.UserId };
                            IndetityVals = new VendorDA().VendorJournalPaymentCashVoucher(lObj);
                        }
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 3 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = Convert.ToInt32(SplitVals[2]);
                                if (lObj.Amount > 0)
                                {
                                    new VendorDA().BulkPaymentVendorCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                        else if (SplitVals.Length == 1 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = 0;
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = 0;
                                if (lObj.Amount > 0)
                                {
                                    new VendorDA().BulkPaymentVendorCashVoucherInvoicePaymentInvoiceLog(lObj);
                                }
                            }
                        }
                    }
                    #endregion
                }
                return RedirectToAction("BulkPayment", "Vendor");
            }
            catch (Exception ex)
            {
                return RedirectToAction("BulkPayment", "Vendor");
            }
        }
        public ActionResult OtherInvPayment()
        {
            ViewBag.VendorList = new SelectList(db.Vendors.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult OtherInvPayment(FormCollection form)
        {
            try
            {
                string IndetityVals = string.Empty;
                int VendorId = (string.IsNullOrEmpty(form["VendorId"])) ? 0 : Convert.ToInt32(form["VendorId"]);
                if (VendorId > 0)
                {
                    int InvoiceCount = (string.IsNullOrEmpty(form["InvoiceCount"])) ? 0 : Convert.ToInt32(form["InvoiceCount"]);
                    int PaymentMethod = (string.IsNullOrEmpty(form["PaymentMethod"])) ? 0 : Convert.ToInt32(form["PaymentMethod"]);
                    #region Vendor Bulk Payment - Payment Method * Cash
                    if (PaymentMethod == 1 && InvoiceCount > 0)
                    {
                        VendorBulkPaymentCashVoucher Obj = new VendorBulkPaymentCashVoucher();
                        Obj.VendorId = VendorId;
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment Other Invoice", string.Empty) : String.Format("{0} - Vendor Bulk Payment Other Invoice", Convert.ToString(form["Notes"]));
                        Obj.UserId = User.Identity.GetUserId();
                        IndetityVals = new OtherInvoiceDA().BulkPaymentCashVoucherOtherInvoice(Obj);
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 2 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment Other Invoice", string.Empty) : String.Format("{0} - Vendor Bulk Payment Other Invoice", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = 0;
                                if (lObj.Amount > 0)
                                {
                                    new OtherInvoiceDA().InvoicePaymentInvoiceLogVendorPaymentOtherInvoice(lObj);
                                }
                            }
                        }
                    }
                    #endregion
                    #region Vendor Bulk Payment - Payment Method * Cheque
                    else if (PaymentMethod == 2)
                    {
                        VendorBulkPaymentChequeVoucher Obj = new VendorBulkPaymentChequeVoucher();
                        Obj.AccountNo = (string.IsNullOrEmpty(form["chkAccountNo"])) ? string.Empty : Convert.ToString(form["chkAccountNo"]);
                        Obj.VendorId = VendorId;
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.BankNames = (string.IsNullOrEmpty(form["chkBankId"])) ? 0 : Convert.ToInt32(form["chkBankId"]);
                        Obj.ChequeNo = (string.IsNullOrEmpty(form["chkChequeNo"])) ? string.Empty : Convert.ToString(form["chkChequeNo"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Remarks = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment Other Invoice", string.Empty) : String.Format("{0} - Vendor Bulk Payment Other Invoice", Convert.ToString(form["Notes"]));
                        Obj.SortCode = (string.IsNullOrEmpty(form["chkSortCode"])) ? string.Empty : Convert.ToString(form["chkSortCode"]);
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.BulkPayment = true;
                        new VendorDA().VendorBulkPaymentChequeVoucher(Obj);
                    }
                    #endregion
                    #region Vendor Bulk Payment - Payment Method * Credit Card
                    else if (PaymentMethod == 3 && InvoiceCount > 0)
                    {
                        VendorBulkPaymentCreditCard Obj = new VendorBulkPaymentCreditCard();
                        Obj.VendorId = VendorId;
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountIdCC"])) ? 0 : Convert.ToInt32(form["BankAccountIdCC"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment Other Invoice", string.Empty) : String.Format("{0} - Vendor Bulk Payment Other Invoice", Convert.ToString(form["Notes"]));
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.CardNo = (string.IsNullOrEmpty(form["CreditCardNo"])) ? string.Empty : Convert.ToString(form["CreditCardNo"]);
                        Obj.CardHolder = (string.IsNullOrEmpty(form["CardHolderName"])) ? string.Empty : Convert.ToString(form["CardHolderName"]);
                        Obj.ExtraAmount = (string.IsNullOrEmpty(form["ExtraAmount"])) ? string.Empty : Convert.ToString(form["ExtraAmount"]);
                        Obj.BankDate = (string.IsNullOrEmpty(form["BankDate"])) ? string.Empty : Convert.ToString(form["BankDate"]);
                        IndetityVals = new VendorDA().VendorBulkPaymentCreditCard(Obj);
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 3 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment Other Invoice", string.Empty) : String.Format("{0} - Vendor Bulk Payment Other Invoice", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = Convert.ToInt32(SplitVals[2]);
                                if (lObj.Amount > 0)
                                {
                                    new OtherInvoiceDA().InvoicePaymentInvoiceLogVendorPaymentOtherInvoice(lObj);
                                }
                            }
                        }
                    }
                    #endregion
                    #region Vendor Bulk Payment - Payment Method * Debit Card
                    else if (PaymentMethod == 4 && InvoiceCount > 0)
                    {
                        VendorBulkPaymentDebitCard Obj = new VendorBulkPaymentDebitCard();
                        Obj.VendorId = VendorId;
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountIdDC"])) ? 0 : Convert.ToInt32(form["BankAccountIdDC"]);
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment Other Invoice", string.Empty) : String.Format("{0} - Vendor Bulk Payment Other Invoice", Convert.ToString(form["Notes"]));
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.CardNo = (string.IsNullOrEmpty(form["CreditCardNo"])) ? string.Empty : Convert.ToString(form["CreditCardNo"]);
                        Obj.CardHolder = (string.IsNullOrEmpty(form["CardHolderName"])) ? string.Empty : Convert.ToString(form["CardHolderName"]);
                        Obj.ExtraAmount = (string.IsNullOrEmpty(form["ExtraAmount"])) ? string.Empty : Convert.ToString(form["ExtraAmount"]);
                        Obj.BankDate = (string.IsNullOrEmpty(form["BankDate"])) ? string.Empty : Convert.ToString(form["BankDate"]);
                        IndetityVals = new VendorDA().VendorBulkPaymentDebitCard(Obj);
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 3 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Vendor Bulk Payment Other Invoice", string.Empty) : String.Format("{0} - Vendor Bulk Payment Other Invoice", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = Convert.ToInt32(SplitVals[2]);
                                if (lObj.Amount > 0)
                                {
                                    new OtherInvoiceDA().InvoicePaymentInvoiceLogVendorPaymentOtherInvoice(lObj);
                                }
                            }
                        }
                    }
                    #endregion
                    #region Vendor Bulk Payment - Payment Method * Bank Deposit
                    else if (PaymentMethod == 5 && InvoiceCount > 0)
                    {
                        VendorBulkPaymentBankDeposit Obj = new VendorBulkPaymentBankDeposit();
                        Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                        Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                        Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.VendorId = VendorId;// (string.IsNullOrEmpty(form["AgentId"])) ? 0 : Convert.ToInt32(form["AgentId"]);
                        Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountIdBP"])) ? 0 : Convert.ToInt32(form["BankAccountIdBP"]);
                        Obj.BankDate = (string.IsNullOrEmpty(form["BankDateBankDeposit"])) ? string.Empty : Convert.ToString(form["BankDateBankDeposit"]);
                        IndetityVals = new VendorDA().VendorBulkPaymentBankDeposit(Obj);
                        string[] SplitVals = IndetityVals.Split(',');
                        if (SplitVals.Length == 3 && Convert.ToInt32(SplitVals[0]) > 0)
                        {
                            for (int i = 1; i <= InvoiceCount; i++)
                            {
                                BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                                lObj.VendorId = VendorId;
                                lObj.VendorLedgerId = Convert.ToInt32(SplitVals[1]);
                                lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                                lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                                lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                                lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0} - Agent Bulk Payment", string.Empty) : String.Format("{0} - Agent Bulk Payment", Convert.ToString(form["Notes"]));
                                lObj.UserID = User.Identity.GetUserId();
                                lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                                lObj.BankAccountLedgerId = Convert.ToInt32(SplitVals[2]);
                                if (lObj.Amount > 0)
                                {
                                    new OtherInvoiceDA().InvoicePaymentInvoiceLogVendorPaymentOtherInvoice(lObj);
                                }
                            }
                        }
                    }
                    #endregion
                }
                return RedirectToAction("OtherInvPayment", "Vendor");
            }
            catch (Exception ex)
            {
                return RedirectToAction("OtherInvPayment", "Vendor");
            }
        }
        public PartialViewResult GetVendorOutstandingInvoice(int? VendorId)
        {
            List<VendorOutstandingInvoiceListViewModel> lstObj = new List<VendorOutstandingInvoiceListViewModel>();
            try
            {
                if (VendorId != null)
                {
                    lstObj = new VendorDA().GetVendorOutstandingInvoiceList((int)VendorId);
                    return PartialView("Vendor/VendorOutstandingInvoiceList", lstObj);
                }
                else
                {
                    return PartialView("Vendor/VendorOutstandingInvoiceList", lstObj);
                }
            }
            catch (Exception)
            {
                return PartialView("Vendor/VendorOutstandingInvoiceList", lstObj);
            }
        }
        public PartialViewResult GetVendorOutstandingOtherInvoice(int? VendorId)
        {
            List<OtherInvoiceList> lstObj = new List<OtherInvoiceList>();
            try
            {
                if (VendorId != null)
                {
                    lstObj = new OtherInvoiceDA().GetVendorOutstandingOtherInvoiceList((int)VendorId);
                    return PartialView("OtherInvoice/OtherInvoicePayment", lstObj);
                }
                else
                {
                    return PartialView("OtherInvoice/OtherInvoicePayment", lstObj);
                }
            }
            catch (Exception ex)
            {
                return PartialView("OtherInvoice/OtherInvoicePayment", lstObj);
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
        public ActionResult VendorOutstanding()
        {
            List<InvoiceListViewModel> lstObj = new List<InvoiceListViewModel>();
            ViewBag.VendorList = new SelectList(db.Vendors.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View(lstObj);
        }
        public PartialViewResult GetVendorOutstanding(int VendorId)
        {
            List<InvoiceListViewModel> lstObj = new VendorDA().GetOutstandingInvoiceListByVendorId(VendorId);
            return PartialView("Vendor/OutstandingInvoiceList", lstObj);
        }
        public ActionResult ChequeRealize(string Name, int cheid, int inv)
        {
            IPChequeDetail vObj = new VendorDA().VendorBulkPaymentFloatingCheque(cheid);
            ViewBag.AccountNo = vObj.AccountNo;
            ViewBag.Amount = vObj.Amount;
            ViewBag.LedgerDate = vObj.SysCreateDate.ToString("yyyy-MM-dd");
            ViewBag.Notes = vObj.Remarks;
            ViewBag.ChekId = cheid;
            ViewBag.VendorList = new SelectList(db.Vendors.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult ChequeRealize(FormCollection form)
        {
            int ChekId = (string.IsNullOrEmpty(form["ChekId"])) ? 0 : Convert.ToInt32(form["ChekId"]);
            try
            {
                bool Checked = Convert.ToBoolean(form["OtherInvoice"].Split(',')[0]);
                string IndetityVals = string.Empty;
                int InvoiceCount = (string.IsNullOrEmpty(form["InvoiceCount"])) ? 0 : Convert.ToInt32(form["InvoiceCount"]);
                int VendorId = (string.IsNullOrEmpty(form["VendorId"])) ? 0 : Convert.ToInt32(form["VendorId"]);
                VendorBulkPaymentChequePayment Obj = new VendorBulkPaymentChequePayment();
                Obj.Amount = (string.IsNullOrEmpty(form["Amount"])) ? 0 : Convert.ToDouble(form["Amount"]);
                Obj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0}", string.Empty) : String.Format("{0}", Convert.ToString(form["Notes"]));
                Obj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                Obj.UserId = User.Identity.GetUserId();
                Obj.VendorId = VendorId;
                Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountIdBP"])) ? 0 : Convert.ToInt32(form["BankAccountIdBP"]);
                Obj.BankDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                Obj.ChekId = ChekId;
                IndetityVals = new VendorDA().VendorBulkPaymentChequePayment(Obj);
                string[] SplitVals = IndetityVals.Split(',');
                if (SplitVals.Length == 3 && Convert.ToInt32(SplitVals[0]) > 0)
                {
                    for (int i = 1; i <= InvoiceCount; i++)
                    {
                        BulkPaymentVendorCashInvPaymentInvLog lObj = new BulkPaymentVendorCashInvPaymentInvLog();
                        lObj.VendorId = VendorId;
                        lObj.VendorLedgerId = Convert.ToInt32(SplitVals[1]);
                        lObj.Amount = (string.IsNullOrEmpty(form["PaidAmount" + i])) ? 0 : Convert.ToDouble(form["PaidAmount" + i]);
                        lObj.GeneralLedgerId = Convert.ToInt32(SplitVals[0]);
                        lObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + i])) ? 0 : Convert.ToInt32(form["InvoiceId" + i]);
                        lObj.Notes = (string.IsNullOrEmpty(form["Notes"])) ? String.Format("{0}", string.Empty) : String.Format("{0}", Convert.ToString(form["Notes"]));
                        lObj.UserID = User.Identity.GetUserId();
                        lObj.LedgerDate = (string.IsNullOrEmpty(form["LedgerDate"])) ? string.Empty : Convert.ToString(form["LedgerDate"]);
                        lObj.BankAccountLedgerId = Convert.ToInt32(SplitVals[2]);
                        if (lObj.Amount > 0 && !Checked)
                        {
                            new VendorDA().BulkPaymentVendorCashVoucherInvoicePaymentInvoiceLog(lObj);
                        }
                        else if(lObj.Amount > 0 && Checked)
                        {
                            new OtherInvoiceDA().InvoicePaymentInvoiceLogVendorPaymentOtherInvoice(lObj);
                        }
                    }
                }
                return RedirectToAction("Ledger", "Vendor", new { id = VendorId });
            }
            catch (Exception ec)
            {
                return RedirectToAction("Index", "Vendor");
            }
        }
        public JsonResult GetVendorInformation(int VendorId)
        {
            try
            {
                Vendor Obj = db.Vendors.Find(VendorId);
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
                        Mobile = Obj.Telephone,
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
            ViewBag.VendorList = new SelectList(db.Vendors.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View(lstObj);
        }
        public PartialViewResult SearchVendorOutstandingOtherInvoice(int VendorId)
        {
            List<OtherInvoiceList> lstObj = new OtherInvoiceDA().GetVendorOutstandingOtherInvoiceList(VendorId);
            return PartialView("OtherInvoice/OtherInvoicePayment", lstObj);
        }
    }
}