
using Billing.DAL;
using Billing.DAL.Helpers;
using Billing.DAL.Parameters;
using Billing.Entities;
using Billing.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Billing.Web.Controllers
{
    [Authorize]
    public class OtherInvoiceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            List<OtherInvoiceList> vModel = new OtherInvoiceDA().GetLatestOtherInvoiceList();
            ViewBag.AgentList = new SelectList(db.Agents.OrderBy(a => a.Name).ToList(), "Id", "Name");
            ViewBag.UserList = new SelectList(db.Users.OrderBy(x => x.PersonName).ToList(), "Id", "PersonName");
            ViewBag.InvoiceType = new SelectList(db.OtherInvoiceTypes.OrderBy(a => a.InvoiceType), "Id", "InvoiceType");
            return View(vModel);
        }
        public PartialViewResult GetFilteredOtherInvoiceList(string FromDate, string ToDate, int? AgentId, string UsersId, int? TypesId)
        {
            DateTime StartDate;
            DateTime FinisDate;
            if (!DateTime.TryParseExact(FromDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out StartDate))
            {
                FromDate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            if (!DateTime.TryParseExact(ToDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out FinisDate))
            {
                ToDate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            List<OtherInvoiceList> vModel = new OtherInvoiceDA().GetFilteredOtherInvoiceList(StartDate, FinisDate, AgentId, UsersId, TypesId);
            return PartialView("OtherInvoice/OtherInvoiceList", vModel);
        }
        public ActionResult InvoiceTypes()
        {
            List<OtherInvocieTypeList> lstObj = new OtherInvoiceDA().GetOtheInvoiceTypeList();
            return View(lstObj);
        }
        public ActionResult EditType(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherInvoiceType invType = new OtherInvoiceDA().GetOtherInvoiceByTypeId((int)id);
            if (invType == null)
            {
                return HttpNotFound();
            }
            return View(invType);
        }
        [HttpPost]
        public ActionResult EditType(OtherInvoiceType model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("InvoiceTypes", "OtherInvoice");
            }
            bool status = new OtherInvoiceDA().UpdateOtherInvoiceType(model);
            return RedirectToAction("InvoiceTypes", "OtherInvoice");
        }
        public ActionResult CreateType(string InvoiceType)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("InvoiceTypes", "OtherInvoice");
            }
            OtherInvoiceType model = new OtherInvoiceType();
            model.CreatedOn = DateTime.Now;
            model.InvoiceType = InvoiceType;
            model.ApplicationUserId = User.Identity.GetUserId();
            bool status = new OtherInvoiceDA().InsertOtherInvoiceType(model);
            return RedirectToAction("InvoiceTypes", "OtherInvoice");
        }
        public ActionResult CreateInvoice()
        {
            ViewBag.InvoiceType = new SelectList(db.OtherInvoiceTypes.OrderBy(a => a.InvoiceType), "Id", "InvoiceType");
            ViewBag.VendorList = new SelectList(db.Vendors.OrderBy(a => a.Name).ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult CreateInvoice(CreateOtherInvoice model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("CreateInvoice", "OtherInvoice");
            }
            OtherInvoice Obj = new OtherInvoice();
            Obj.AgentId = model.AgentId;
            Obj.ApplicationUserId = User.Identity.GetUserId();
            Obj.CreatedOn = DateTime.Now;
            Obj.CustomerAgentAmount = model.CustomerAgentAmount;
            Obj.CustomerAgentPaid = false;
            Obj.Details = model.Details;
            Obj.ExpectedPayDate = model.ExpectedPayDate;
            Obj.OtherInvoiceTypeId = model.OtherInvoiceTypeId;
            Obj.Reference = model.Reference;
            Obj.Status = InvoiceStatus.Raised;
            Obj.VendorAmount = model.VendorAmount;
            Obj.VendorId = model.VendorId;
            Obj.VendorInvNo = model.VendorInvNo;
            Obj.VendorPaid = false;
            bool ret = new OtherInvoiceDA().InsertNewOtherInvoice(Obj);
            return RedirectToAction("CreateInvoice", "OtherInvoice");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherInvoiceDetailsViewModel vmModel = new OtherInvoiceDA().GetOtherInvoiceDetailsByInvoiceId((int)id);
            if (vmModel == null)
            {
                return HttpNotFound();
            }
            vmModel.InvLogs = new OtherInvoiceDA().GetOtherInvoiceLogInfo((int)id);
            vmModel.InvPayments = new OtherInvoiceDA().GetOtherInvoicePaymentList((int)id);
            return View(vmModel);
        }
        public ActionResult UpdateOtherInvoice(FormCollection col)
        {
            int InvoiceId = (string.IsNullOrEmpty(col["InvoiceId"])) ? 0 : Convert.ToInt32(col["InvoiceId"]);
            try
            {
                #region Add new Remarks to invoice
                if (col["trigger"].ToString() == "addRemarks")
                {
                    OtherInvoiceLog ilObj = new OtherInvoiceLog();
                    ilObj.ApplicationUserId = User.Identity.GetUserId();
                    ilObj.OtherInvoiceId = InvoiceId;
                    ilObj.Remarks = (string.IsNullOrEmpty(col["InvoiceRemakrs"])) ? string.Empty : col["InvoiceRemakrs"];
                    new OtherInvoiceDA().InsertNewRemarksToOtherInvoice(ilObj);
                    return RedirectToAction("Details", "OtherInvoice", new { id = InvoiceId });
                }
                #endregion
                #region Update the agent of an invoice
                else if (col["trigger"].ToString() == "ChangeAgent")
                {
                    int AgentId = (string.IsNullOrEmpty(col["AgentId"])) ? 0 : Convert.ToInt32(col["AgentId"]);
                    string userID = User.Identity.GetUserId();
                    new OtherInvoiceDA().UpdateOtherInvoiceAgent(userID, InvoiceId, AgentId);
                    return RedirectToAction("Details", "OtherInvoice", new { id = InvoiceId });
                }
                #endregion
                #region Update the vendor of an invoice
                else if (col["trigger"].ToString() == "ChangeVendor")
                {
                    int VendorId = (string.IsNullOrEmpty(col["VendorId"])) ? 0 : Convert.ToInt32(col["VendorId"]);
                    string userID = User.Identity.GetUserId();
                    new OtherInvoiceDA().UpdateOtherInvoiceVendor(userID, InvoiceId, VendorId);
                    return RedirectToAction("Details", "OtherInvoice", new { id = InvoiceId });
                }
                #endregion
                #region Update invoice basic information
                else if (col["trigger"].ToString() == "ChangeInvoiceInfo")
                {
                    string UserID = User.Identity.GetUserId();
                    int OtherInvoiceTypeId = (string.IsNullOrEmpty(col["OtherInvoiceTypeId"])) ? 0 : Convert.ToInt32(col["OtherInvoiceTypeId"]);
                    string ExpectedPayDate = (string.IsNullOrEmpty(col["ExpectedPayDate"])) ? string.Empty : Convert.ToString(col["ExpectedPayDate"]);
                    string Reference = (string.IsNullOrEmpty(col["Reference"])) ? string.Empty : Convert.ToString(col["Reference"]);
                    string VendorInvNo = (string.IsNullOrEmpty(col["VendorInvNo"])) ? string.Empty : Convert.ToString(col["VendorInvNo"]);
                    string Details = (string.IsNullOrEmpty(col["Details"])) ? string.Empty : Convert.ToString(col["Details"]);
                    double Amount = (string.IsNullOrEmpty(col["Amount"])) ? 0 : Convert.ToDouble(col["Amount"]);
                    double PaidAmount = (string.IsNullOrEmpty(col["PaidAmount"])) ? 0 : Convert.ToDouble(col["PaidAmount"]);
                    if(PaidAmount > Amount)
                    {
                        return RedirectToAction("Details", "OtherInvoice", new { id = InvoiceId });
                    }
                    else
                    {
                        new OtherInvoiceDA().UpdateOtherInvoiceBasicInfo(UserID, InvoiceId, OtherInvoiceTypeId, ExpectedPayDate, Reference, VendorInvNo, Details, Amount);
                        return RedirectToAction("Details", "OtherInvoice", new { id = InvoiceId });
                    }
                }
                #endregion
                #region Change User of the Invoice
                else if (col["trigger"].ToString() == "ChangeUser")
                {
                    string NewUserID = (string.IsNullOrEmpty(col["ApplicationUserId"])) ? string.Empty : col["ApplicationUserId"];
                    new OtherInvoiceDA().UpdateOtherInvoiceCurrentUser(NewUserID, InvoiceId);
                    return RedirectToAction("Details", "OtherInvoice", new { id = InvoiceId });
                }
                #endregion
                #region When nothing found
                else
                {
                    return RedirectToAction("Details", "OtherInvoice", new { id = InvoiceId });
                } 
                #endregion
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details", "OtherInvoice", new { id = InvoiceId });
            }
        }
        public ActionResult AgentPaid(int? id)
        {
            bool status = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            status = new OtherInvoiceDA().UpdateOtherInvoicePaymentStatus((int)id, (int)InvoicePaymentStatus.Paid, User.Identity.GetUserId());
            return RedirectToAction("Details", "OtherInvoice", new { id = id });
        }
        public ActionResult VendorPaid(int? id)
        {
            bool status = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            status = new OtherInvoiceDA().UpdateOtherInvoicePaymentStatusVendor((int)id, (int)InvoicePaymentStatus.Paid, User.Identity.GetUserId());
            return RedirectToAction("Details", "OtherInvoice", new { id = id });
        }
        public ActionResult GetOtherInvoiceTypeList()
        {
            List<OtherInvoiceType> lstType = new List<OtherInvoiceType>();
            lstType = (db.OtherInvoiceTypes.OrderBy(x => x.InvoiceType).ToList<OtherInvoiceType>());
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstType);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AgtCustTransaction(FormCollection form)
        {
            int InvoiceId = (string.IsNullOrEmpty(form["InvoiceId"])) ? 0 : Convert.ToInt32(form["InvoiceId"]);
            try
            {
                int TransactionMethod = (string.IsNullOrEmpty(form["TransactionMethod"])) ? 0 : Convert.ToInt32(form["TransactionMethod"]);
                #region Payment by Cash
                if (TransactionMethod == 1)
                {
                    CashTransaction Obj = new CashTransaction();
                    Obj.Amount = (string.IsNullOrEmpty(form["PaymentAmount"])) ? 0 : Convert.ToDouble(form["PaymentAmount"]);
                    Obj.Remarks = (string.IsNullOrEmpty(form["PaymentRemarks"])) ? string.Empty : Convert.ToString(form["PaymentRemarks"]);
                    Obj.UserId = User.Identity.GetUserId();
                    Obj.InvoiceId = InvoiceId;
                    new OtherInvoiceDA().OtherInvoicePaymentCashVoucher(Obj);
                }
                #endregion
                #region Payment by Bank Cheque
                else if (TransactionMethod == 2)
                {
                    ChequeDetails Obj = new ChequeDetails();
                    Obj.AccountNo = (string.IsNullOrEmpty(form["AccountNo"])) ? string.Empty : Convert.ToString(form["AccountNo"]);
                    Obj.Amount = (string.IsNullOrEmpty(form["PaymentAmount"])) ? 0 : Convert.ToDouble(form["PaymentAmount"]);
                    Obj.BankNames = (BankName)Convert.ToInt32(((string.IsNullOrEmpty(form["BankId"])) ? "0" : Convert.ToString(form["BankId"])).Replace(",", ""));
                    Obj.ChequeNo = (string.IsNullOrEmpty(form["ChequeNo"])) ? string.Empty : Convert.ToString(form["ChequeNo"]);
                    Obj.InvoiceId = InvoiceId;
                    Obj.Remarks = (string.IsNullOrEmpty(form["PaymentRemarks"])) ? string.Empty : Convert.ToString(form["PaymentRemarks"]);
                    Obj.SortCode = (string.IsNullOrEmpty(form["SortCode"])) ? string.Empty : Convert.ToString(form["SortCode"]);
                    this.chequeTransaction(Obj);
                }
                #endregion
                #region Payment By Credit Card
                else if (TransactionMethod == 3)
                {
                    CCardDetail Obj = new CCardDetail();
                    Obj.Amount = (string.IsNullOrEmpty(form["PaymentAmount"])) ? 0 : Convert.ToDouble(form["PaymentAmount"]);
                    Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountId"])) ? 0 : Convert.ToInt32(form["BankAccountId"]);
                    Obj.BankDate = (string.IsNullOrEmpty(form["BankDate"])) ? string.Empty : Convert.ToString(form["BankDate"]);
                    Obj.CardHolder = (string.IsNullOrEmpty(form["CardHolderName"])) ? string.Empty : Convert.ToString(form["CardHolderName"]);
                    Obj.CardNo = (string.IsNullOrEmpty(form["CreditCardNo"])) ? string.Empty : Convert.ToString(form["CreditCardNo"]);
                    Obj.ExtraAmount = (string.IsNullOrEmpty(form["ExtraAmount"])) ? string.Empty : Convert.ToString(form["ExtraAmount"]);
                    Obj.InvoiceId = InvoiceId;
                    Obj.Notes = String.Format("Credit Card Invoice Payment by {0}. - {1}", User.Identity.GetDisplayName(), (string.IsNullOrEmpty(form["PaymentRemarks"])) ? string.Empty : Convert.ToString(form["PaymentRemarks"]));
                    Obj.UserId = User.Identity.GetUserId();
                    new OtherInvoiceDA().AddCreditCardPaymentDetailsOtherInvoice(Obj);
                }
                #endregion
                #region Payment by Debit Card
                else if (TransactionMethod == 4)
                {
                    DCardDetail Obj = new DCardDetail();
                    Obj.Amount = (string.IsNullOrEmpty(form["PaymentAmount"])) ? 0 : Convert.ToDouble(form["PaymentAmount"]);
                    Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountId"])) ? 0 : Convert.ToInt32(form["BankAccountId"]);
                    Obj.BankDate = (string.IsNullOrEmpty(form["BankDateDebitCard"])) ? string.Empty : Convert.ToString(form["BankDateDebitCard"]);
                    Obj.CardHolder = (string.IsNullOrEmpty(form["CardHolderNameDebitCard"])) ? string.Empty : Convert.ToString(form["CardHolderNameDebitCard"]);
                    Obj.CardNo = (string.IsNullOrEmpty(form["DebitCardNo"])) ? string.Empty : Convert.ToString(form["DebitCardNo"]);
                    Obj.ExtraAmount = (string.IsNullOrEmpty(form["ExtraAmountDebitCard"])) ? string.Empty : Convert.ToString(form["ExtraAmountDebitCard"]);
                    Obj.InvoiceId = InvoiceId;
                    Obj.Notes = String.Format("Debit Card Invoice Payment by {0}. - {1}", User.Identity.GetDisplayName(), (string.IsNullOrEmpty(form["PaymentRemarks"])) ? string.Empty : Convert.ToString(form["PaymentRemarks"]));
                    Obj.UserId = User.Identity.GetUserId();
                    new OtherInvoiceDA().AddDebitCardPaymentDetailsOtherInvoice(Obj);
                }
                #endregion
                #region Payment by Bank Deposit
                else if (TransactionMethod == 5)
                {
                    BankPaymentDetail Obj = new BankPaymentDetail();
                    Obj.Amount = (string.IsNullOrEmpty(form["PaymentAmount"])) ? 0 : Convert.ToDouble(form["PaymentAmount"]);
                    Obj.BankAccountId = (string.IsNullOrEmpty(form["BankAccountId"])) ? 0 : Convert.ToInt32(form["BankAccountId"]);
                    Obj.BankDate = (string.IsNullOrEmpty(form["BankDateBankDeposit"])) ? string.Empty : Convert.ToString(form["BankDateBankDeposit"]);
                    Obj.InvoiceId = InvoiceId;
                    Obj.Notes = String.Format("Bank Deposit/Transfer Invoice Payment by {0}. - {1}", User.Identity.GetDisplayName(), (string.IsNullOrEmpty(form["PaymentRemarks"])) ? string.Empty : Convert.ToString(form["PaymentRemarks"]));
                    Obj.UserId = User.Identity.GetUserId();
                    new OtherInvoiceDA().AddBankDepositPaymentDetailsOtherInvoice(Obj);
                }
                #endregion
                return RedirectToAction("Details", "OtherInvoice", new { id = InvoiceId });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Details", "OtherInvoice", new { id = InvoiceId });
            }
        }
        private void chequeTransaction(ChequeDetails model)
        {
            IPChequeDetail ipcObj = new IPChequeDetail();
            ipcObj.AccountNo = model.AccountNo;
            ipcObj.Amount = model.Amount;
            ipcObj.ApplicationUserId = User.Identity.GetUserId();
            ipcObj.BankNames = model.BankNames;
            ipcObj.ChequeNo = model.ChequeNo;
            ipcObj.GeneralLedgerId = null;
            ipcObj.InvoiceId = null;
            ipcObj.InvoicePaymentId = null;
            ipcObj.OtherInvoiceId = model.InvoiceId;
            ipcObj.Remarks = model.Remarks;
            ipcObj.SortCode = model.SortCode;
            ipcObj.SysCreateDate = DateTime.Now;
            ipcObj.Status = ChequeStatus.Floating;
            ipcObj.BulkPayment = false;
            db.IPChequeDetails.Add(ipcObj);
            db.SaveChanges();

            OtherInvoiceLog ilObj = new OtherInvoiceLog();
            ilObj.ApplicationUserId = ipcObj.ApplicationUserId;
            ilObj.OtherInvoiceId = model.InvoiceId;
            ilObj.Remarks = "Cheque Payment Received - " + model.Remarks;
            ilObj.SysDateTime = ipcObj.SysCreateDate;
            db.OtherInvoiceLogs.Add(ilObj);
            db.SaveChanges();

        }
    }
}