using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Billing.DAL;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Vereyon.Web;
using Billing.ViewModel;
using Billing.Entities;
using Billing.DAL.Helpers;
using Billing.DAL.Parameters;

namespace Billing.Web.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        #region LocalVariables
        private ApplicationDbContext db = new ApplicationDbContext();
        private List<string[]> primaryGDSLines = new List<string[]>();
        private List<string> formattedGDSLines = new List<string>();
        private List<InvoiceSegment> segmentsList = new List<InvoiceSegment>();
        public List<InvoiceName> paxList = new List<InvoiceName>();
        private string formattedAirlineCode = string.Empty;
        private Agent newCustomer = new Agent();
        private Airlines AirlineCode = new Airlines();
        private string GdsBookingDate { get; set; }
        private string GDSUserId { get; set; }
        private string Pnr { get; set; }
        private InvoiceBookingViewModel vModel = new InvoiceBookingViewModel(); 
        #endregion
        public ActionResult Index()
        {
            List<InvoiceListViewModel> lstLstObj = new SearchDA().GetLatestInvoiceList();
            return View(lstLstObj);
        }
        public ActionResult DraftList()
        {
            List<InvoiceListViewModel> lstLstObj = new SearchDA().GetDraftInvoiceList();
            return View(lstLstObj);
        }
        [HttpPost]
        public ActionResult Confirm(GDS GDSs, string bookin, ProfileType ProfileType, int? AgentId, int? AgentId2, int? CusType, string Name, string Mobile, string Email, string Postcode, int VendorId)
        {
            Agent invAgent = CommonHelper.DetectProfileIdForInvoice(ProfileType, AgentId, AgentId2, CusType, User.Identity.GetUserId(), Email, Mobile, Name, Postcode);
            if(invAgent == null) { return RedirectToAction("Index", "Invoice"); }

            #region Amadeus Invoice
            if (GDSs == GDS.Amadeus)
            {
                try
                {
                    #region Parse GDS Code
                    string[] lines = bookin.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] _line = lines[i].Split('\r');
                        primaryGDSLines.Add(_line);
                    }
                    for (int j = 0; j < primaryGDSLines.Count; j++)
                    {
                        string secondaryLine = primaryGDSLines[j][0].ToString().Trim();
                        formattedGDSLines.Add(secondaryLine);
                    }

                    Pnr = formattedGDSLines[0].Substring(formattedGDSLines[0].Length - 6).ToString().Trim();
                    GDSUserId = formattedGDSLines[1].Split('/')[0].ToString().Trim();
                    GdsBookingDate = formattedGDSLines[1].Split('/')[2].Substring(0, formattedGDSLines[1].Split('/')[2].Length - 2).ToString().Trim();
                    string[] AirlineRawCode = formattedGDSLines[formattedGDSLines.Count - 1].Split(' ');
                    int sampleInt;
                    for (int ai = 0; ai < AirlineRawCode.Length; ai++)
                    {
                        if (AirlineRawCode[ai].Length == 2 && !Int32.TryParse(AirlineRawCode[ai], out sampleInt))
                        {
                            formattedAirlineCode = AirlineRawCode[ai];
                            break;
                        }
                    }
                    List<Airlines> lstAirline = db.Airliness.ToList();
                    AirlineCode = lstAirline.Where(al => al.Code == formattedAirlineCode).FirstOrDefault();
                    ViewBag.Airlines = AirlineCode != null ? AirlineCode.Name : string.Empty;
                    #endregion
                    #region Create Invoice
                    Invoice invObj = new Invoice { AgentId = invAgent.Id, AirlinesId = AirlineCode.Id, VendorId = VendorId, ApplicationUserId = User.Identity.GetUserId(), GdsBookingDate = GdsBookingDate, GDSs = GDSs, GDSUserId = GDSUserId, InvoiceStatusS = InvoiceStatus.Draft, InvoiceType = ProfileType, Pnr = Pnr, SysCreateDate = DateTime.Now, ExtraCharge = 0, PaidByAgent = false, PaidToVendor = false };
                    db.Invoices.Add(invObj);
                    db.SaveChanges();
                    #endregion
                    #region Parsing Segments
                    for (int seg = 0; seg < formattedGDSLines.Count; seg++)
                    {
                        if(CommonHelper.DetectSegmentLine(formattedGDSLines[seg], lstAirline))
                        {
                            InvoiceSegment iSegment = CommonHelper.ParseInvoiceSegment(formattedGDSLines[seg], invObj.Id);
                            segmentsList.Add(iSegment);
                            db.InvoiceSegments.Add(iSegment);
                            db.SaveChanges();
                        }
                    }
                    #endregion
                    #region Parsing Pessengers
                    for (int pline = 2; pline < (formattedGDSLines.Count - segmentsList.Count); pline++)
                    {
                        InvoiceName paxObj = CommonHelper.ParseInvoiceName(formattedGDSLines, segmentsList, invObj.Id, pline);
                        if(paxObj != null && !string.IsNullOrEmpty(paxObj.Name))
                        {
                            
                            #region 2 pax in 1 line with infant
                            if (paxObj.Name.Contains("="))
                            {
                                string[] sPaxLine = paxObj.Name.Split('=');
                                for (int e = 0; e < sPaxLine.Length; e++)
                                {
                                    if (sPaxLine[e].Contains("(INF"))
                                    {
                                        sPaxLine[e] = sPaxLine[e].Replace("(INF", "~");
                                        string[] sPaxLine2 = sPaxLine[e].Split('~');
                                        int Place = sPaxLine2[1].LastIndexOf(')');
                                        sPaxLine2[1] = sPaxLine2[1].Remove(Place, 1);
                                        InvoiceName fObj = new InvoiceName { InvoiceId = invObj.Id, Name = sPaxLine2[0], BookingDate = DateTime.Now, Status = 1, PassengerTypes = PassengerType.INF };
                                        paxList.Add(fObj);
                                        db.InvoiceNames.Add(fObj);
                                        db.SaveChanges();
                                        InvoiceName sObj = new InvoiceName { InvoiceId = invObj.Id, Name = sPaxLine2[1], BookingDate = DateTime.Now, Status = 1, PassengerTypes = PassengerType.ADT };
                                        paxList.Add(sObj);
                                        db.InvoiceNames.Add(sObj);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        InvoiceName sObj = new InvoiceName { InvoiceId = invObj.Id, Name = sPaxLine[e], BookingDate = DateTime.Now, Status = 1, PassengerTypes = paxObj.PassengerTypes };
                                        paxList.Add(sObj);
                                        db.InvoiceNames.Add(sObj);
                                        db.SaveChanges();
                                    }
                                }
                                if (sPaxLine[0].Contains("(INF"))
                                {
                                    sPaxLine[0] = sPaxLine[0].Replace("(INF", "~");
                                }
                            }
                            #endregion
                            #region One pax in one line with infant
                            else if (paxObj.Name.Contains("(INF"))
                            {
                                paxObj.Name = paxObj.Name.Replace("(INF", "~");
                                string[] sPaxLine = paxObj.Name.Split('~');
                                int Place = sPaxLine[1].LastIndexOf(')');
                                sPaxLine[1] = sPaxLine[1].Remove(Place, 1);
                                InvoiceName fObj = new InvoiceName { InvoiceId = invObj.Id, Name = sPaxLine[0], BookingDate = DateTime.Now, Status = 1, PassengerTypes = PassengerType.INF };
                                paxList.Add(fObj);
                                db.InvoiceNames.Add(fObj);
                                db.SaveChanges();
                                InvoiceName sObj = new InvoiceName { InvoiceId = invObj.Id, Name = sPaxLine[1], BookingDate = DateTime.Now, Status = 1, PassengerTypes = PassengerType.ADT };
                                paxList.Add(sObj);
                                db.InvoiceNames.Add(sObj);
                                db.SaveChanges();
                            }
                            #endregion
                            #region 1 pax 1 line without infant
                            else
                            {
                                paxList.Add(paxObj);
                                db.InvoiceNames.Add(paxObj);
                                db.SaveChanges();
                            } 
                            #endregion

                        }
                    }
                    #region Blocked
                    //for (int pline = 2; pline < (formattedGDSLines.Count - segmentsList.Count); pline++)
                    //{
                    //    if (!formattedGDSLines[pline].Contains("*"))
                    //    {
                    //        string[] paxSingleLine = null;
                    //        string paxLine = string.Empty;
                    //        paxSingleLine = formattedGDSLines[pline].Replace("\t", "=").Split('=');
                    //        if (paxSingleLine.Length == 1)
                    //        {
                    //            paxSingleLine = formattedGDSLines[pline].Replace("   ", "=").Split('=');
                    //        }

                    //        if (paxSingleLine.Length > 1)
                    //        {
                    //            for (int sLine = 0; sLine < paxSingleLine.Length; sLine++)
                    //            {
                    //                InvoiceName _invName = new InvoiceName();
                    //                _invName.InvoiceId = invObj.Id;
                    //                _invName.BookingDate = DateTime.Now;
                    //                _invName.Name = paxSingleLine[sLine].Remove(0, 2).ToString();
                    //                _invName.Status = 1;
                    //                paxList.Add(_invName);
                    //                db.InvoiceNames.Add(_invName);
                    //                db.SaveChanges();
                    //            }
                    //        }
                    //        else
                    //        {
                    //            //if (paxSingleLine[0].Contains("(INF"))
                    //            //{
                    //            //    paxLine = paxSingleLine[0].Replace("(INF", "~");
                    //            //    string[] sPaxLine = paxLine.Split('~');
                    //            //    int Place = sPaxLine[1].LastIndexOf(')');
                    //            //    sPaxLine[1] = sPaxLine[1].Remove(Place, 1);

                    //            //    for (int sLine = 0; sLine < sPaxLine.Length; sLine++)
                    //            //    {
                    //            //        InvoiceName _invName = new InvoiceName();
                    //            //        _invName.InvoiceId = invObj.Id;
                    //            //        _invName.BookingDate = DateTime.Now;
                    //            //        _invName.Name = sPaxLine[sLine].Remove(0, 2).ToString();
                    //            //        _invName.Status = 1;
                    //            //        paxList.Add(_invName);
                    //            //        db.InvoiceNames.Add(_invName);
                    //            //        db.SaveChanges();
                    //            //    }
                    //            //}
                    //            //else
                    //            //{
                    //            //    paxLine = paxSingleLine[0];
                    //            //}

                    //            InvoiceName _invName = new InvoiceName();
                    //            _invName.InvoiceId = invObj.Id;
                    //            _invName.BookingDate = DateTime.Now;
                    //            _invName.Name = paxSingleLine[0].Remove(0, 2).ToString();
                    //            _invName.Status = 1;
                    //            paxList.Add(_invName);
                    //            db.InvoiceNames.Add(_invName);
                    //            db.SaveChanges();
                    //        }
                    //    }
                    //} 
                    #endregion
                    #endregion
                    #region Send Data to View File
                    vModel.AgentAddress = invAgent.Address;
                    vModel.AgentEmail = invAgent.Email == null ? string.Empty : invAgent.Email;
                    vModel.AgentFax = invAgent.FaxNo == null ? string.Empty : invAgent.FaxNo;
                    vModel.AgentId = invAgent.Id;
                    vModel.AgentMobile = invAgent.Mobile == null ? string.Empty : invAgent.Mobile;
                    vModel.AgentName = invAgent.Name == null ? string.Empty : invAgent.Name;
                    vModel.AgentPostCode = invAgent.Postcode == null ? string.Empty : invAgent.Postcode;
                    vModel.AgentTelephone = invAgent.Telephone == null ? string.Empty : invAgent.Telephone;
                    vModel.AirlinesId = AirlineCode.Id;
                    vModel.AirlinesName = AirlineCode.Name == null ? string.Empty : AirlineCode.Name;
                    vModel.GdsBookingDate = GdsBookingDate;
                    vModel.GDSs = GDSs;
                    vModel.InvoiceType = ProfileType;
                    vModel.PaxList = paxList;
                    vModel.Pnr = Pnr;
                    vModel.Segments = segmentsList;
                    vModel.SysCreateDate = DateTime.Now;
                    vModel.VendorId = VendorId;
                    vModel.VendorName = db.Vendors.Find(VendorId).Name.ToString();
                    vModel.InvoiceId = invObj.Id;
                    ViewBag.CreditLimit = invAgent.CreditLimit;
                    ViewBag.Balance = invAgent.Balance;
                    ViewBag.InvType = ProfileType.ToString();
                    #endregion
                    #region Create Invoice Log
                    InvoiceLog invLogObj = new InvoiceLog { ApplicationUserId = invObj.ApplicationUserId, InvoiceId = invObj.Id, Remarks = "Invoice Drafted", SysDateTime = DateTime.Now };
                    db.InvoiceLogs.Add(invLogObj);
                    db.SaveChanges();
                    #endregion

                }
                catch (Exception ex)
                {
                    FlashMessage.Danger("Invoice Couldn't be created now!!");
                    return RedirectToAction("Index", "Invoice");
                }
            } 
            #endregion
            else if (false)
            {

            }
            FlashMessage.Confirmation("Invoice created!!");
            return View(vModel);
        }
        [HttpPost]
        public ActionResult Booking(FormCollection collection)
        {
            try
            {
                var Date = collection["ExpectedPaymentDate"];
                DateTime Date2 = new DateTime();
                if (string.IsNullOrEmpty(Date))
                {
                    Date2 = DateTime.Now.AddDays(30);
                }
                else
                {
                    Date2 = DateTime.ParseExact(Date, "yyyy/MM/dd", null);
                }
                Invoice _baseinv = db.Invoices.Find(Convert.ToInt32(collection["InvoiceId"]));
                _baseinv.VendorInvId = collection["VendorInvNo"].ToString();
                _baseinv.VendorId = (string.IsNullOrEmpty(collection["VendorId"])) ? 0 : Convert.ToInt32(collection["VendorId"]);
                _baseinv.ExpectedDatePayment = Date2;
                _baseinv.ExtraCharge = 0;//(string.IsNullOrEmpty(collection["ExtraCharge"])) ? 0 : Convert.ToDouble(collection["ExtraCharge"]);
                _baseinv.CancellationChargeBefore = (string.IsNullOrEmpty(collection["CancellationChargeBefore"])) ? "." : Convert.ToString(collection["CancellationChargeBefore"]);
                _baseinv.CancellationChargeAfter = (string.IsNullOrEmpty(collection["CancellationChargeAfter"])) ? "." : Convert.ToString(collection["CancellationChargeAfter"]);
                _baseinv.CancellationDateBefore = (string.IsNullOrEmpty(collection["CancellationDateBefore"])) ? string.Empty : collection["CancellationDateBefore"];
                _baseinv.CancellationDateAfter = (string.IsNullOrEmpty(collection["CancellationDateAfter"])) ? string.Empty : collection["CancellationDateAfter"];
                _baseinv.NoShowBefore = (string.IsNullOrEmpty(collection["NoShowBefore"])) ? "." : Convert.ToString(collection["NoShowBefore"]);
                _baseinv.NoShowAfter = (string.IsNullOrEmpty(collection["NoShowAfter"])) ? "." : Convert.ToString(collection["NoShowAfter"]);
                _baseinv.InvoiceStatusS = InvoiceStatus.Raised;
                db.Entry(_baseinv).State = EntityState.Modified;
                db.SaveChanges();

                double agBalance = 0;
                double vnBalance = 0;
                for (int i = 1; i <= Convert.ToInt32(collection["PassengerCount"]); i++)
                {
                    int InvNameId = Convert.ToInt32(collection["InvoiceName" + i]);
                    InvoiceName _baseInvName = db.InvoiceNames.Find(InvNameId);
                    _baseInvName.Amount = (string.IsNullOrEmpty(collection["tAmount" + i])) ? 0 : Convert.ToDouble(collection["tAmount" + i]);
                    _baseInvName.CNetFare = (string.IsNullOrEmpty(collection["CNatFare" + i])) ? 0 : Convert.ToDouble(collection["CNatFare" + i]);
                    _baseInvName.VNetFare = 0;
                    _baseInvName.TicketNo = collection["TicketNo" + i];
                    _baseInvName.TicketTax = (string.IsNullOrEmpty(collection["Tax" + i])) ? 0 : Convert.ToDouble(collection["Tax" + i]);
                    _baseInvName.VendorCharge = 0;
                    if (string.IsNullOrEmpty(_baseinv.Agents.Atol))
                    {
                        _baseInvName.Apc = 0;
                    }
                    else
                    {
                        _baseInvName.Apc = 2.5;
                    }
                    _baseInvName.PassengerTypes = (PassengerType)Convert.ToInt32(collection["PassengerType" + i]);
                    db.Entry(_baseInvName).State = EntityState.Modified;
                    db.SaveChanges();

                    vnBalance = (double)(vnBalance + _baseInvName.VNetFare + _baseInvName.TicketTax + _baseInvName.Apc);
                    agBalance = (double)(agBalance + _baseInvName.Amount);
                }
                if (_baseinv.Vendors != null)
                {
                    Vendor vn = db.Vendors.Find(_baseinv.VendorId);
                    vn.Balance = vn.Balance + vnBalance;

                    VendorLedger vl = new VendorLedger();
                    vl.Amount = vnBalance;
                    vl.ApplicationUserId = User.Identity.GetUserId();
                    vl.Balance = vn.Balance;
                    vl.Remarks = "Invoice Created";
                    vl.SystemDate = DateTime.Now;
                    vl.VendorId = vn.Id;
                    vl.VendorLedgerHeadId = 1;  //Invoice Created
                    vl.GeneralLedgerId = null;
                    vl.BankAccountLedgerId = null;
                    vl.PaymentMethods = PaymentMethod.Cash;

                    db.VendorLedgers.Add(vl);
                    db.Entry(vn).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (_baseinv.Agents != null)
                {
                    Agent ag = db.Agents.Find(_baseinv.AgentId);
                    ag.Balance = ag.Balance + agBalance + _baseinv.ExtraCharge;

                    AgentLedger al = new AgentLedger();
                    al.AgentId = ag.Id;
                    al.AgentLedgerHeadId = 1;   //Invoice Created
                    al.Amount = (agBalance + _baseinv.ExtraCharge);
                    al.ApplicationUserId = User.Identity.GetUserId();
                    al.Balance = ag.Balance;
                    al.Remarks = "Invoice Created";
                    al.SystemDate = DateTime.Now;
                    al.GeneralLedgerId = null;
                    al.BankAccountLedgerId = null;

                    db.AgentLedgers.Add(al);
                    db.Entry(ag).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Details", "Invoice", new { id = _baseinv.Id });
            }
            catch (Exception ex)
            {
                throw;    
            }

        }
        public ActionResult ViewSegments(int id)
        {
            List<InvoiceSegment> lstSegments = db.InvoiceSegments.Where(x => x.InvoiceId == id).ToList();
            return PartialView(lstSegments);
        }
        [HttpPost]
        public ActionResult GetAgentList(int type)
        {
            List<Agent> lstAgent = new List<Agent>();
            if(type == 1)
            {
                lstAgent = (db.Agents.Where(x => x.ProfileType == ProfileType.Agent)).OrderBy(x => x.Name).ToList<Agent>();
            }
            else if(type == 2)
            {
                lstAgent = (db.Agents.Where(x => x.ProfileType == ProfileType.Customer)).OrderBy(x => x.Name).ToList<Agent>();
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstAgent);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetVendorList()
        {
            List<Vendor> lstVendor = new List<Vendor>();
            
            lstVendor = db.Vendors.OrderBy(x => x.Name).ToList<Vendor>();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstVendor);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateInvoice(FormCollection col)
        {
            int InvoiceId = (string.IsNullOrEmpty(col["InvoiceId"])) ? 0 : Convert.ToInt32(col["InvoiceId"]);
            try
            {
                #region Update the agent of an invoice
                if (col["trigger"].ToString() == "ChangeAgent")
                {
                    Invoice inv = db.Invoices.Find(InvoiceId);
                    if (inv != null && inv.Agents != null)
                    {
                        inv.AgentId = (string.IsNullOrEmpty(col["AgentId"])) ? 0 : Convert.ToInt32(col["AgentId"]);
                        string currentName = inv.Agents.Name;
                        Agent ag = db.Agents.Find(inv.AgentId);
                        string newName = ag.Name;
                        InvoiceLog il = new InvoiceLog();
                        il.InvoiceId = InvoiceId;
                        il.Remarks = String.Format("Agent changed from {0} to {1}", currentName.ToString(), newName.ToString());
                        il.SysDateTime = DateTime.Now;
                        il.ApplicationUserId = User.Identity.GetUserId();
                        db.InvoiceLogs.Add(il);
                        db.Entry(inv).State = EntityState.Modified;
                        db.SaveChanges();
                        #region Redirection
                        if (inv.InvoiceStatusS == InvoiceStatus.Raised)
                        {
                            return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
                        }
                        else if (inv.InvoiceStatusS == InvoiceStatus.Draft)
                        {
                            return RedirectToAction("Draft", "Invoice", new { id = InvoiceId });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Invoice");
                        }
                        #endregion
                    }
                    else
                    {
                        return RedirectToAction("Index", "Invoice");
                    }
                } 
                #endregion
                #region Update the vendor of an invoice
                else if (col["trigger"].ToString() == "ChangeVendor")
                {
                    InvoiceId = (string.IsNullOrEmpty(col["InvoiceId2"])) ? 0 : Convert.ToInt32(col["InvoiceId2"]);
                    Invoice inv = db.Invoices.Find(InvoiceId);
                    if (inv != null && inv.Vendors != null)
                    {
                        inv.VendorId = (string.IsNullOrEmpty(col["VendorId"])) ? 0 : Convert.ToInt32(col["VendorId"]);
                        string currentName = inv.Vendors.Name;
                        Vendor ve = db.Vendors.Find(inv.VendorId);
                        string newName = ve.Name;
                        InvoiceLog il = new InvoiceLog();
                        il.InvoiceId = InvoiceId;
                        il.Remarks = String.Format("Vendor changed from {0} to {1}", currentName.ToString(), newName.ToString());
                        il.SysDateTime = DateTime.Now;
                        il.ApplicationUserId = User.Identity.GetUserId();
                        db.InvoiceLogs.Add(il);
                        db.Entry(inv).State = EntityState.Modified;
                        db.SaveChanges();
                        #region Redirection
                        if (inv.InvoiceStatusS == InvoiceStatus.Raised)
                        {
                            return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
                        }
                        else if (inv.InvoiceStatusS == InvoiceStatus.Draft)
                        {
                            return RedirectToAction("Draft", "Invoice", new { id = InvoiceId });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Invoice");
                        }
                        #endregion
                    }
                    else
                    {
                        return RedirectToAction("Index", "Invoice");
                    }
                } 
                #endregion
                #region Update invoice primary information
                else if (col["trigger"].ToString() == "ChangeInvoiceInfo")
                {
                    InvoiceId = (string.IsNullOrEmpty(col["InvoiceId3"])) ? 0 : Convert.ToInt32(col["InvoiceId3"]);
                    Invoice inv = db.Invoices.Find(InvoiceId);
                    if (inv != null)
                    {
                        var Date = col["ExpectedPaymentDate"];
                        DateTime Date2 = new DateTime();
                        if (string.IsNullOrEmpty(Date))
                        {
                            Date2 = DateTime.Now.AddDays(30);
                        }
                        else
                        {
                            Date2 = DateTime.ParseExact(Date, "MM/dd/yyyy", null);
                        }
                        inv.Pnr = (string.IsNullOrEmpty(col["PNRNo"])) ? string.Empty : Convert.ToString(col["PNRNo"]);
                        inv.VendorInvId = col["VendorInvNo"].ToString();
                        inv.ExpectedDatePayment = Date2;
                        inv.CancellationChargeBefore = (string.IsNullOrEmpty(col["CancellationChargeBefore"])) ? "." : Convert.ToString(col["CancellationChargeBefore"]);
                        inv.CancellationChargeAfter = (string.IsNullOrEmpty(col["CancellationChargeAfter"])) ? "." : Convert.ToString(col["CancellationChargeAfter"]);
                        inv.CancellationDateBefore = (string.IsNullOrEmpty(col["CancellationDateBefore"])) ? string.Empty : col["CancellationDateBefore"];
                        inv.CancellationDateAfter = (string.IsNullOrEmpty(col["CancellationDateAfter"])) ? string.Empty : col["CancellationDateAfter"];
                        inv.NoShowBefore = (string.IsNullOrEmpty(col["NoShowBefore"])) ? "." : Convert.ToString(col["NoShowBefore"]);
                        inv.NoShowAfter = (string.IsNullOrEmpty(col["NoShowAfter"])) ? "." : Convert.ToString(col["NoShowAfter"]);

                        InvoiceLog il = new InvoiceLog();
                        il.InvoiceId = InvoiceId;
                        il.Remarks = String.Format("Invoice information changed, Vendor Invoice No, Expected Date of Payment, Cancellation Charge, No Show, Cancellation Dat");
                        il.SysDateTime = DateTime.Now;
                        il.ApplicationUserId = User.Identity.GetUserId();
                        db.InvoiceLogs.Add(il);

                        db.Entry(inv).State = EntityState.Modified;
                        db.SaveChanges();
                        #region Redirection
                        if (inv.InvoiceStatusS == InvoiceStatus.Raised)
                        {
                            return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
                        }
                        else if (inv.InvoiceStatusS == InvoiceStatus.Draft)
                        {
                            return RedirectToAction("Draft", "Invoice", new { id = InvoiceId });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Invoice");
                        }
                        #endregion
                    }
                    else
                    {
                        return RedirectToAction("Index", "Invoice");
                    }
                } 
                #endregion
                #region Add new Remarks to invoice
                else if (col["trigger"].ToString() == "addRemarks")
                {
                    InvoiceLog ilObj = new InvoiceLog();
                    ilObj.ApplicationUserId = User.Identity.GetUserId();
                    ilObj.InvoiceId = InvoiceId;
                    ilObj.Remarks = (string.IsNullOrEmpty(col["InvoiceRemakrs"])) ? string.Empty : col["InvoiceRemakrs"];
                    new SearchDA().InsertNewRemarksToInvoice(ilObj);
                    return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
                }
                #endregion
                #region Change User of the Invoice
                else if (col["trigger"].ToString() == "ChangeUser")
                {
                    string NewUserID = (string.IsNullOrEmpty(col["ApplicationUserId"])) ? string.Empty : col["ApplicationUserId"];
                    new SearchDA().UpdateInvoiceCurrentUser(NewUserID, InvoiceId);
                    return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
                }
                #endregion
                #region If nothing
                else
                {
                    return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
                } 
                #endregion
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
            }
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                InvoiceDetailsViewModel _invDetails = this.InvoiceDetails((int)id);
                if(_invDetails == null)
                {
                    return RedirectToAction("Draft", "Invoice", new { id = id });
                }
                else
                {
                    return View(_invDetails);
                }
            }
        }
        private InvoiceDetailsViewModel InvoiceDetails(int InvoiceId)
        {
            try
            {
                InvoiceDetailsViewModel invDetails = new InvoiceDetailsViewModel();

                Invoice _baseInv = db.Invoices.Find(InvoiceId);
                if (_baseInv.InvoiceStatusS == InvoiceStatus.Draft)
                {
                    return null;
                }
                Airlines _airlines = db.Airliness.Find(_baseInv.AirlinesId);
                Agent _agent = db.Agents.Find(_baseInv.AgentId);
                Vendor _vendor = db.Vendors.Find(_baseInv.VendorId);
                List<InvoiceDetailsPaxViewModel> _invPaxList = new SearchDA().GetInvoicePaxInfo(InvoiceId);
                List<InvoiceDetailsSegmentViewModel> _invSegments = new SearchDA().GetInvoiceSegmentInfo(InvoiceId);
                List<InvoiceDetailsLogViewModel> _invLogs = new SearchDA().GetInvoiceLogInfo(InvoiceId);
                List<InvoicePaymentHistory> _invPaid = new AgentDA().GetInvoicePaymentHistoryByInvoice(_baseInv.Id);

                invDetails.InvoiceId = _baseInv.Id;
                invDetails.InvType = (int)_baseInv.InvoiceType;
                invDetails.SysCreateDate = _baseInv.SysCreateDate;
                invDetails.PersonName = _baseInv.ApplicationUsers.PersonName;
                invDetails.AgentName = _agent.Name;
                invDetails.Pnr = _baseInv.Pnr;
                invDetails.GdsBookingDate = _baseInv.GdsBookingDate;
                invDetails.AirlinesName = _airlines.Name;
                invDetails.VendorName = _vendor.Name;
                invDetails.VendorInvNo = _baseInv.VendorInvId;
                if (_baseInv.ExpectedDatePayment != null)
                {
                    invDetails.ExpectedPaymentDate = Convert.ToDateTime(_baseInv.ExpectedDatePayment);
                }
                else
                {
                    invDetails.ExpectedPaymentDate = DateTime.MinValue;
                }
                invDetails.GDSs = _baseInv.GDSs;
                invDetails.AgentAddress = _agent.Address;
                invDetails.AgentPostCode = _agent.Postcode;
                invDetails.AgentTelephone = _agent.Telephone;
                invDetails.AgentMobile = _agent.Mobile;
                invDetails.AgentFax = _agent.FaxNo;
                invDetails.AgentEmail = _agent.Email;
                invDetails.CancellationChargeBefore = _baseInv.CancellationChargeBefore;
                invDetails.CancellationChargeAfter = _baseInv.CancellationChargeAfter;
                invDetails.CancellationDateBefore = _baseInv.CancellationDateBefore;
                invDetails.CancellationDateAfter = _baseInv.CancellationDateAfter;
                invDetails.AmountReceived = db.InvoicePayments.Where(a => a.InvoiceId == _baseInv.Id).Select(a => a.Amount).DefaultIfEmpty(0).Sum();
                invDetails.ExtraCharge = _baseInv.ExtraCharge;
                invDetails.NoShowBefore = _baseInv.NoShowBefore;
                invDetails.NoShowAfter = _baseInv.NoShowAfter;
                invDetails.PaxList = _invPaxList;
                invDetails.invSegments = _invSegments.OrderBy(a => a.FlightDate).ToList();
                invDetails.invLog = _invLogs;
                invDetails.invPaid = _invPaid;
                invDetails.Total = 0;
                return invDetails;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public ActionResult Draft(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceDetailsViewModel invDetails = new InvoiceDetailsViewModel();

            Invoice _baseInv = db.Invoices.Find(id);
            if (_baseInv.InvoiceStatusS == InvoiceStatus.Raised)
            {
                return RedirectToAction("Details", "Invoice", new { id = id });
            }
            Airlines _airlines = db.Airliness.Find(_baseInv.AirlinesId);
            Agent _agent = db.Agents.Find(_baseInv.AgentId);
            Vendor _vendor = db.Vendors.Find(_baseInv.VendorId);
            List<InvoiceDetailsPaxViewModel> _invPaxList = new SearchDA().GetInvoicePaxInfo((int)id); //db.InvoiceNames.Where(x => x.InvoiceId == _baseInv.Id).ToList();
            List<InvoiceDetailsSegmentViewModel> _invSegments = new SearchDA().GetInvoiceSegmentInfo((int)id);//db.InvoiceSegments.Where(s => s.InvoiceId == _baseInv.Id).ToList();
            List<InvoiceDetailsLogViewModel> _invLogs = new SearchDA().GetInvoiceLogInfo((int)id); //db.InvoiceLogs.Where(l => l.InvoiceId == _baseInv.Id).ToList();

            invDetails.InvoiceId = _baseInv.Id;
            invDetails.InvType = (int)_baseInv.InvoiceType;
            invDetails.SysCreateDate = _baseInv.SysCreateDate;
            invDetails.AgentName = _agent.Name;
            invDetails.Pnr = _baseInv.Pnr;
            invDetails.GdsBookingDate = _baseInv.GdsBookingDate;
            invDetails.AirlinesName = _airlines.Name;
            invDetails.VendorName = _vendor.Name;
            invDetails.VendorInvNo = _baseInv.VendorInvId;
            if (_baseInv.ExpectedDatePayment != null)
            {
                invDetails.ExpectedPaymentDate = Convert.ToDateTime(_baseInv.ExpectedDatePayment);
            }
            else
            {
                invDetails.ExpectedPaymentDate = DateTime.MinValue;
            }
            invDetails.GDSs = _baseInv.GDSs;
            invDetails.AgentAddress = _agent.Address;
            invDetails.AgentPostCode = _agent.Postcode;
            invDetails.AgentTelephone = _agent.Telephone;
            invDetails.AgentMobile = _agent.Mobile;
            invDetails.AgentFax = _agent.FaxNo;
            invDetails.AgentEmail = _agent.Email;
            invDetails.CancellationChargeBefore = _baseInv.CancellationChargeBefore;
            invDetails.CancellationChargeAfter = _baseInv.CancellationChargeAfter;
            invDetails.CancellationDateBefore = _baseInv.CancellationDateBefore;
            invDetails.CancellationDateAfter = _baseInv.CancellationDateAfter;
            invDetails.AmountReceived = db.InvoicePayments.Where(a => a.InvoiceId == _baseInv.Id).Select(a => a.Amount).DefaultIfEmpty(0).Sum();
            invDetails.ExtraCharge = _baseInv.ExtraCharge;
            invDetails.NoShowBefore = _baseInv.NoShowBefore;
            invDetails.NoShowAfter = _baseInv.NoShowAfter;
            invDetails.PaxList = _invPaxList;
            invDetails.invSegments = _invSegments;
            invDetails.invLog = _invLogs;


            return View(invDetails);
        }
        [HttpGet]
        public ActionResult UpdatePaxInfo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Response.Write(id);
            return View();
        }
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Invoice invoice = db.Invoices.Find(id);

                List<InvoiceLog> il = db.InvoiceLogs.Where(a => a.InvoiceId == id).ToList();
                if (il.Count > 0) { db.InvoiceLogs.RemoveRange(il); db.SaveChanges(); }

                List<InvoiceName> inPax = db.InvoiceNames.Where(a => a.InvoiceId == id).ToList();
                if (inPax.Count > 0) { db.InvoiceNames.RemoveRange(inPax); db.SaveChanges(); }

                List<InvoicePayment> inPay = db.InvoicePayments.Where(a => a.InvoiceId == id).ToList();
                if (inPay.Count > 0) { db.InvoicePayments.RemoveRange(inPay); db.SaveChanges(); }

                List<InvoiceSegment> inSeg = db.InvoiceSegments.Where(a => a.InvoiceId == id).ToList();
                if (inSeg.Count > 0) { db.InvoiceSegments.RemoveRange(inSeg); db.SaveChanges(); }

                if (invoice != null) { db.Invoices.Remove(invoice); db.SaveChanges(); }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            List<InvoicePaymentHistory> invPaid = new AgentDA().GetInvoicePaymentHistoryByInvoice(id);
            if(invPaid.Count > 0)
            {
                db.Invoices.Remove(invoice);
                db.SaveChanges();
                return RedirectToAction("DraftList", "Invoice");
            }
            else
            {
                return RedirectToAction("Draft", "Invoice", new { id = id });
            }
        }
        [HttpPost, ActionName("Search")]
        public PartialViewResult InvoiceSearch(FormCollection collection)
        {
            List<InvoiceListViewModel> lstLstObj = new List<InvoiceListViewModel>();
            var invoices = db.Invoices.Include(i => i.Agents).Include(i => i.AirlinesS).Include(i => i.Vendors).OrderBy(a => a.SysCreateDate).Take(5);
            foreach (Invoice inv in invoices)
            {
                var iTotal = db.InvoiceNames.Where(x => x.InvoiceId == inv.Id).GroupBy(x => x.InvoiceId == inv.Id).Select(g => new
                {
                    Total = g.Sum(s => s.Amount)
                });
                double vTotal = 0;
                if (iTotal.Count() > 0)
                {
                    vTotal = Convert.ToDouble(iTotal.FirstOrDefault().Total);
                }
                var iPaid = db.InvoicePayments.Where(x => x.InvoiceId == inv.Id).GroupBy(x => x.InvoiceId == inv.Id).Select(g => new
                {
                    Paid = g.Sum(s => s.Amount)
                });
                double vPaid = 0;
                if (iPaid.Count() > 0)
                {
                    vPaid = iPaid.FirstOrDefault().Paid;
                }
                InvoiceListViewModel _vObj = new InvoiceListViewModel();
                _vObj.InvoiceId = inv.Id;
                _vObj.SysCreateDate = inv.SysCreateDate;
                _vObj.AirlineCode = inv.AirlinesS.Code;
                _vObj.AgentCust = inv.Agents.Name;
                _vObj.Total = vTotal;
                _vObj.Paid = vPaid;
                _vObj.Refund = Convert.ToDouble(0);
                _vObj.Due = Convert.ToDouble(vTotal) - Convert.ToDouble(vPaid);
                _vObj.PersonName = inv.ApplicationUsers.PersonName;

                lstLstObj.Add(_vObj);
            }
            return PartialView("Search", lstLstObj);
        }
        public ActionResult updateFareInfo(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<InvoiceName> _paxList = db.InvoiceNames.Where(a => a.InvoiceId == id).ToList();
            ViewBag.airlineCode = db.Invoices.Find(id).AirlinesS.Code;
            List<UpdateFareInfo> paxList = new List<UpdateFareInfo>(); 
            foreach(InvoiceName iName in _paxList)
            {
                UpdateFareInfo Obj = new UpdateFareInfo();
                Obj.Id = iName.Id;
                Obj.PassengerTypes = iName.PassengerTypes == null ? PassengerType.ADT : (PassengerType)iName.PassengerTypes;
                Obj.Amount = Convert.ToDouble(iName.Amount);
                Obj.Tax = Convert.ToDouble(iName.TicketTax);
                Obj.Apc = Convert.ToDouble(iName.Apc);
                Obj.CNetFare = Convert.ToDouble(iName.CNetFare);
                Obj.VNetFare = Convert.ToDouble(iName.VNetFare);
                Obj.VendorCharge = Convert.ToDouble(iName.VendorCharge);

                Obj.Profit = Obj.Amount - (Obj.Tax + Obj.Apc + Obj.VNetFare + Obj.VendorCharge);

                Obj.InvoiceId = iName.InvoiceId;
                Obj.Name = iName.Name;
                Obj.TicketNo = iName.TicketNo;

                paxList.Add(Obj);
            }
            ViewBag.InvoiceId = id;
            return View(paxList);
        }
        [HttpPost]
        public ActionResult updateFareInfo(FormCollection form)
        {
            try
            {
                int InvoiceID = (string.IsNullOrEmpty(form["InvoiceId"])) ? 0 : Convert.ToInt32(form["InvoiceId"]);
                //Invoice _baseInv = db.Invoices.Find(InvoiceID);
                double Total = 0;
                bool score = false;
                for (int sl = 1; sl <= Convert.ToInt32(form["RowCount"]); sl++)
                {
                    InvoiceName iName = new InvoiceName();
                    iName.Id = (string.IsNullOrEmpty(form["Id" + sl]) ? 0 : Convert.ToInt32(form["Id" + sl]));
                    iName.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId" + sl]) ? 0 : Convert.ToInt32(form["InvoiceId" + sl]));
                    iName.Name = (string.IsNullOrEmpty(form["Name" + sl])) ? string.Empty : Convert.ToString(form["Name" + sl]);
                    iName.PassengerTypes = (PassengerType)(string.IsNullOrEmpty(form["PassengerTypes" + sl]) ? 0 : Convert.ToInt32(form["PassengerTypes" + sl]));
                    iName.TicketNo = (string.IsNullOrEmpty(form["TicketNo" + sl])) ? string.Empty : Convert.ToString(form["TicketNo" + sl]);
                    iName.Amount = (string.IsNullOrEmpty(form["Amount" + sl])) ? 0 : Convert.ToDouble(form["Amount" + sl]);
                    iName.Apc = (string.IsNullOrEmpty(form["Apc" + sl])) ? 0 : Convert.ToDouble(form["Apc" + sl]);
                    iName.CNetFare = (string.IsNullOrEmpty(form["CNetFare" + sl])) ? 0 : Convert.ToDouble(form["CNetFare" + sl]);
                    iName.TicketTax = (string.IsNullOrEmpty(form["Tax" + sl])) ? 0 : Convert.ToDouble(form["Tax" + sl]);
                    iName.VendorCharge = (string.IsNullOrEmpty(form["VendorCharge" + sl])) ? 0 : Convert.ToDouble(form["VendorCharge" + sl]);
                    iName.VNetFare = (string.IsNullOrEmpty(form["VNetFare" + sl])) ? 0 : Convert.ToDouble(form["VNetFare" + sl]);
                    Total = Total + (iName.VNetFare + iName.VendorCharge);
                    score = new SearchDA().UpdateInvoiceFareInfo(iName, User.Identity.GetUserId());
                    if (!score) { break; }
                }
                if (score)
                {
                    FinalizeFareInfoUpdate Obj = new FinalizeFareInfoUpdate();
                    Obj.ApplicationUserId = User.Identity.GetUserId();
                    Obj.InvoiceId = InvoiceID;
                    Obj.Remarks = String.Format("Fare Info Updated by {0}", User.Identity.GetDisplayName());
                    Obj.Total = Total;
                    score = new SearchDA().FinalizeFareInfoUpdate(Obj);
                    FlashMessage.Confirmation("Fare info/ticket no updated..");
                    return RedirectToAction("Details", "Invoice", new { id = InvoiceID });
                }
                else
                {
                    FinalizeFareInfoUpdate Obj = new FinalizeFareInfoUpdate();
                    Obj.ApplicationUserId = User.Identity.GetUserId();
                    Obj.InvoiceId = InvoiceID;
                    Obj.Remarks = String.Format("Something went wrong while updating the fare info of the Invoice by {0}", User.Identity.GetDisplayName());
                    Obj.Total = Total;
                    score = new SearchDA().FinalizeFareInfoUpdate(Obj);
                    FlashMessage.Danger(String.Format("Something went wrong while updating the fare info of the Invoice by {0}", User.Identity.GetDisplayName()));
                    return RedirectToAction("Details", "Invoice", new { id = InvoiceID });
                }
            }
            catch(Exception ex)
            {
                FlashMessage.Danger(ex.Message.ToString());
                throw;
            }
        }
        [HttpPost]
        public ActionResult updateSegment(FormCollection form)
        {
            int InvoiceID = (string.IsNullOrEmpty(form["InvoiceId"])) ? 0 : Convert.ToInt32(form["InvoiceId"]);
            try
            {
                bool score = false;
                for (int sl = 1; sl <= Convert.ToInt32(form["RowCount"]); sl++)
                {
                    SegmentUpdateParams iSeg = new SegmentUpdateParams();
                    iSeg.TableId = (string.IsNullOrEmpty(form["Id" + sl]) ? 0 : Convert.ToInt32(form["Id" + sl]));
                    iSeg.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId"]) ? 0 : Convert.ToInt32(form["InvoiceId"]));
                    iSeg.AirlinesCode = (string.IsNullOrEmpty(form["AirlinesCode" + sl])) ? string.Empty : Convert.ToString(form["AirlinesCode" + sl]);
                    iSeg.ArrivalTime = (string.IsNullOrEmpty(form["ArrivalTime" + sl]) ? string.Empty : Convert.ToString(form["ArrivalTime" + sl]));
                    iSeg.DepartureDate = (string.IsNullOrEmpty(form["DepartureDate" + sl])) ? string.Empty : Convert.ToString(form["DepartureDate" + sl]);
                    iSeg.DepartureFrom = (string.IsNullOrEmpty(form["DepartureFrom" + sl])) ? string.Empty : Convert.ToString(form["DepartureFrom" + sl]);
                    iSeg.DepartureTime = (string.IsNullOrEmpty(form["DepartureTime" + sl])) ? string.Empty : Convert.ToString(form["DepartureTime" + sl]);
                    iSeg.DepartureTo = (string.IsNullOrEmpty(form["DepartureTo" + sl])) ? string.Empty : Convert.ToString(form["DepartureTo" + sl]);
                    iSeg.FlightDate = (string.IsNullOrEmpty(form["FlightDate" + sl])) ? string.Empty : Convert.ToString(form["FlightDate" + sl]);
                    iSeg.FlightNo = (string.IsNullOrEmpty(form["FlightNo" + sl])) ? string.Empty : Convert.ToString(form["FlightNo" + sl]);
                    iSeg.SegmentClass = (string.IsNullOrEmpty(form["SegmentClass" + sl])) ? string.Empty : Convert.ToString(form["SegmentClass" + sl]);
                    iSeg.SegmentStatus = (string.IsNullOrEmpty(form["SegmentStatus" + sl])) ? string.Empty : Convert.ToString(form["SegmentStatus" + sl]);
                    string[] Splitted = iSeg.FlightDate.Split('/');
                    iSeg.FlightDate = String.Join("-", Splitted[2], Splitted[0], Splitted[1]);
                    score = new SearchDA().UpdateInvoiceSegments(iSeg, User.Identity.GetUserId());
                    if (!score) { break; }
                }
                if (score)
                {
                    FinalizeFareInfoUpdate Obj = new FinalizeFareInfoUpdate();
                    Obj.ApplicationUserId = User.Identity.GetUserId();
                    Obj.InvoiceId = InvoiceID;
                    Obj.Remarks = String.Format("Ticket Segments Updated by {0}", User.Identity.GetDisplayName());
                    score = new SearchDA().FinalizeTicketSegmentUpdate(Obj);
                    FlashMessage.Confirmation("Ticket segments updated..");
                    return RedirectToAction("Details", "Invoice", new { id = InvoiceID });
                }
                else
                {
                    FinalizeFareInfoUpdate Obj = new FinalizeFareInfoUpdate();
                    Obj.ApplicationUserId = User.Identity.GetUserId();
                    Obj.InvoiceId = InvoiceID;
                    Obj.Remarks = String.Format("Something went wrong while updating the Segments of the ticket by {0}", User.Identity.GetDisplayName());
                    score = new SearchDA().FinalizeFareInfoUpdate(Obj);
                    FlashMessage.Danger(String.Format("Something went wrong while updating the Segments of the ticket by {0}", User.Identity.GetDisplayName()));
                    return RedirectToAction("Details", "Invoice", new { id = InvoiceID });
                }
            }
            catch(Exception ex)
            {
                FlashMessage.Danger(ex.Message.ToString());
                return RedirectToAction("Details", "Invoice", new { id = InvoiceID });
            }
            return View();
        }
        public ActionResult IssueDate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<InvoiceDetailsPaxViewModel> paxList = new SearchDA().GetInvoicePaxInfo((int)id);
            ViewBag.InvoiceId = id;
            return View(paxList);
        }
        [HttpPost]
        public ActionResult IssueDate(FormCollection form)
        {
            try
            {
                int InvoiceID = (string.IsNullOrEmpty(form["InvoiceId"])) ? 0 : Convert.ToInt32(form["InvoiceId"]);
                Invoice _baseInv = db.Invoices.Find(InvoiceID);
                double Total = 0;
                bool score = false;
                for (int sl = 1; sl <= Convert.ToInt32(form["RowCount"]); sl++)
                {
                    int TableId = (string.IsNullOrEmpty(form["TableId" + sl]) ? 0 : Convert.ToInt32(form["TableId" + sl]));
                    int InvoiceId = (string.IsNullOrEmpty(form["InvoiceId"]) ? 0 : Convert.ToInt32(form["InvoiceId"]));
                    string IssueDate = (string.IsNullOrEmpty(form["BookingDate" + sl])) ? string.Empty : Convert.ToString(form["BookingDate" + sl]);

                    score = new SearchDA().UpdateInvoiceTicketIssueDate(IssueDate, InvoiceId, TableId);
                    if (!score) { break; }
                }
                if (score)
                {
                    InvoiceLog ilObj = new InvoiceLog();
                    ilObj.ApplicationUserId = User.Identity.GetUserId();
                    ilObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId"]) ? 0 : Convert.ToInt32(form["InvoiceId"]));
                    ilObj.Remarks = String.Format("Ticket Issue Date Updated by {0}", User.Identity.GetDisplayName());
                    ilObj.SysDateTime = DateTime.Now;
                    score = new SearchDA().InsertNewRemarksToInvoice(ilObj);
                }
                else
                {
                    InvoiceLog ilObj = new InvoiceLog();
                    ilObj.ApplicationUserId = User.Identity.GetUserId();
                    ilObj.InvoiceId = (string.IsNullOrEmpty(form["InvoiceId"]) ? 0 : Convert.ToInt32(form["InvoiceId"]));
                    ilObj.Remarks = String.Format("Something went wrong while updating the ticket issue date of the Invoice by {0}", User.Identity.GetDisplayName());
                    ilObj.SysDateTime = DateTime.Now;
                    score = new SearchDA().InsertNewRemarksToInvoice(ilObj);
                }
                return RedirectToAction("Details", "Invoice", new { id = InvoiceID });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ActionResult VendorCharges(int id)
        {
            List<InvoiceName> lstSegments = db.InvoiceNames.Where(x => x.InvoiceId == id).ToList();
            return PartialView(lstSegments);
        }
        [HttpPost]
        public ActionResult InvoiceTransaction (FormCollection form)
        {
            int InvoiceId = (string.IsNullOrEmpty(form["InvoiceId"])) ? 0 : Convert.ToInt32(form["InvoiceId"]);
            try
            {
                int PaymentMode = (string.IsNullOrEmpty(form["TransactionMode"])) ? 0 : Convert.ToInt32(form["TransactionMode"]);
                if (PaymentMode == 1)
                {
                    //Payment Received
                    int TransactionMethod = (string.IsNullOrEmpty(form["TransactionMethod"])) ? 0 : Convert.ToInt32(form["TransactionMethod"]);
                    #region Payment by Cash
                    if (TransactionMethod == 1)
                    {
                        //Cash Transaction
                        CashTransaction Obj = new CashTransaction();
                        Obj.Amount = (string.IsNullOrEmpty(form["PaymentAmount"])) ? 0 : Convert.ToDouble(form["PaymentAmount"]);
                        Obj.Remarks = (string.IsNullOrEmpty(form["PaymentRemarks"])) ? string.Empty : Convert.ToString(form["PaymentRemarks"]);
                        Obj.UserId = User.Identity.GetUserId();
                        Obj.InvoiceId = InvoiceId;
                        new SearchDA().InvoicePaymentCashVoucher(Obj);
                    } 
                    #endregion
                    #region Payment by Bank Cheque
                    else if (TransactionMethod == 2)
                    {
                        //Cheque Payment
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
                        new SearchDA().AddCreditCardPaymentDetails(Obj);
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
                        new SearchDA().AddDebitCardPaymentDetails(Obj);
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
                        new SearchDA().AddBankDepositPaymentDetails(Obj);
                    } 
                    #endregion
                }
                else if(PaymentMode == 2)
                {
                    //Adjustment
                }
                return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
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
            ipcObj.InvoiceId = model.InvoiceId;
            ipcObj.InvoicePaymentId = null;
            ipcObj.OtherInvoiceId = null;
            ipcObj.Remarks = model.Remarks;
            ipcObj.SortCode = model.SortCode;
            ipcObj.SysCreateDate = DateTime.Now;
            ipcObj.Status = ChequeStatus.Floating;
            ipcObj.BulkPayment = false;
            db.IPChequeDetails.Add(ipcObj);
            db.SaveChanges();

            InvoiceLog ilObj = new InvoiceLog();
            ilObj.ApplicationUserId = ipcObj.ApplicationUserId;
            ilObj.InvoiceId = model.InvoiceId;
            ilObj.Remarks = "Cheque Payment Received - " + model.Remarks;
            ilObj.SysDateTime = ipcObj.SysCreateDate;
            db.InvoiceLogs.Add(ilObj);
            db.SaveChanges();

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
        public ActionResult InvoicePrint(int id)
        {
            InvoicePrintViewModel vmObj = new InvoicePrintViewModel();
            vmObj.CompInfo = new AccountingDA().GetCompanyInformation()[0];
            vmObj.InvDetails = this.InvoiceDetails(id);
            return View(vmObj);
        }
        public ActionResult AgentPaid(int? id)
        {
            bool status = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            status = new SearchDA().UpdateInvoicePaymentStatus((int)id, (int)InvoicePaymentStatus.Paid, User.Identity.GetUserId());
            return RedirectToAction("Details", "Invoice", new { id = id });
        }
        public ActionResult VendorPaid(int? id)
        {
            bool status = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            status = new SearchDA().UpdateInvoicePaymentStatusVendor((int)id, (int)InvoicePaymentStatus.Paid, User.Identity.GetUserId());
            return RedirectToAction("Details", "Invoice", new { id = id });
        }
        public ActionResult CreateInvoice(int? id)  //Change the status of an Invoice from Draft to Invoice
        {
            bool status = false;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            status = new SearchDA().UpdateDraftToInvoice((int)id, (int)InvoiceStatus.Raised, User.Identity.GetUserId());
            return RedirectToAction("Details", "Invoice", new { id = id });
        }
        public PartialViewResult FilterInvoiceList(int? invoiceID, string ToDate, string FromDate, string SearchBy, string SearchValue)
        {
            List<InvoiceListViewModel> lstLstObj = new List<InvoiceListViewModel>();
            #region Search Invoice By Invoice ID
            if (invoiceID > 0 && invoiceID != null)
            {
                lstLstObj = new SearchDA().SearchInvoicesByInvoicesID(invoiceID);
            }
            #endregion
            #region SearchInvoice By Last Name OR Pnr OR TicketNo
            else if (Convert.ToInt32(SearchBy) > 0)
            {
                if (String.IsNullOrEmpty(SearchValue))
                {
                }
                else
                {
                    #region Search Invoice by Passenger Last Name
                    if (Convert.ToInt32(SearchBy) == 1)
                    {
                        if (!string.IsNullOrEmpty(SearchBy))
                        {
                            lstLstObj = new SearchDA().SearchInvoicesByPaxLastNames(SearchValue);
                        }
                    }
                    #endregion
                    #region Search Invoice by Ticket No.
                    else if (Convert.ToInt32(SearchBy) == 2)
                    {
                        if (!string.IsNullOrEmpty(SearchBy))
                        {
                            lstLstObj = new SearchDA().SearchInvoicesByTicketNo(SearchValue);
                        }
                    }
                    #endregion
                    #region Search Invoice by PNR
                    else if (Convert.ToInt32(SearchBy) == 3)
                    {
                        if (!string.IsNullOrEmpty(SearchBy))
                        {
                            lstLstObj = new SearchDA().SearchInvoicesByPNR(SearchValue);
                        }
                    }
                    #endregion
                }
            }
            #endregion
            #region Search Invoice By Specific Date Or Date Range
            else if (!string.IsNullOrEmpty(FromDate))
            {

                if (string.IsNullOrEmpty(ToDate))
                {
                    lstLstObj = new SearchDA().SearchInvoicesByDate(FromDate);
                }
                else
                {
                    lstLstObj = new SearchDA().SearchInvoicesByDateRange(FromDate, ToDate);
                }
            }
            #endregion
            return PartialView("Invoice/InvoiceList", lstLstObj);
        }
        public PartialViewResult FilterDraftList(int? invoiceID, string ToDate, string FromDate, string SearchBy, string SearchValue)
        {
            List<InvoiceListViewModel> lstLstObj = new List<InvoiceListViewModel>();
            #region Search Invoice By Invoice ID
            if (invoiceID > 0 && invoiceID != null)
            {
                lstLstObj = new SearchDA().SearchDraftByDraftID(invoiceID);
                return PartialView("Invoice/DraftList", lstLstObj);
            }
            #endregion
            #region SearchInvoice By Last Name OR Pnr OR TicketNo
            else if (Convert.ToInt32(SearchBy) > 0)
            {
                if (String.IsNullOrEmpty(SearchValue))
                {
                    return PartialView("Invoice/DraftList", lstLstObj);
                }
                else
                {
                    #region Search Invoice by Passenger Last Name
                    if (Convert.ToInt32(SearchBy) == 1)
                    {
                        if (!string.IsNullOrEmpty(SearchBy))
                        {
                            lstLstObj = new SearchDA().SearchDraftByPaxLastNames(SearchValue);
                            return PartialView("DraftList", lstLstObj);
                        }
                        else
                        {
                            return PartialView("DraftList", lstLstObj);
                        }
                    }
                    #endregion
                    #region Search Invoice by Ticket No.
                    else if (Convert.ToInt32(SearchBy) == 2)
                    {
                        if (!string.IsNullOrEmpty(SearchBy))
                        {
                            lstLstObj = new SearchDA().SearchInvoicesByTicketNo(SearchValue);
                            return PartialView("InvoiceList", lstLstObj);
                        }
                        else
                        {
                            return PartialView("InvoiceList", lstLstObj);
                        }
                    }
                    #endregion
                    #region Search Invoice by PNR
                    else if (Convert.ToInt32(SearchBy) == 3)
                    {
                        if (!string.IsNullOrEmpty(SearchBy))
                        {
                            lstLstObj = new SearchDA().SearchInvoicesByPNR(SearchValue);
                            return PartialView("InvoiceList", lstLstObj);
                        }
                        else
                        {
                            return PartialView("InvoiceList", lstLstObj);
                        }
                    }
                    #endregion
                }
            }
            #endregion
            #region Search Invoice By Specific Date Or Date Range
            else if (!string.IsNullOrEmpty(FromDate))
            {

                if (string.IsNullOrEmpty(ToDate))
                {
                    lstLstObj = new SearchDA().SearchDraftByDate(FromDate);
                    return PartialView("Invoice/DraftList", lstLstObj);
                }
                else
                {
                    lstLstObj = new SearchDA().SearchDraftByDateRange(FromDate, ToDate);
                    return PartialView("Invoice/DraftList", lstLstObj);
                }
            }
            #endregion
            return PartialView("Invoice/DraftList", lstLstObj);
        }
        private List<InvoiceListViewModel> searchInvoiceByID(int InvID)
        {
            List<InvoiceListViewModel> _lstLstObj = new List<InvoiceListViewModel>();
            var invoices = db.Invoices.Include(i => i.Agents).Include(i => i.AirlinesS).Include(i => i.Vendors).Where(a => a.Id == InvID).FirstOrDefault();
            if (invoices != null)
            {
                var iTotal = db.InvoiceNames.Where(x => x.InvoiceId == invoices.Id).GroupBy(x => x.InvoiceId == invoices.Id).Select(g => new
                {
                    Total = g.Sum(s => s.Amount)
                });
                double vTotal = 0;
                if (iTotal.Count() > 0)
                {
                    vTotal = Convert.ToDouble(iTotal.FirstOrDefault().Total);
                }
                var iPaid = db.InvoicePayments.Where(x => x.InvoiceId == invoices.Id).GroupBy(x => x.InvoiceId == invoices.Id).Select(g => new
                {
                    Paid = g.Sum(s => s.Amount)
                });
                double vPaid = 0;
                if (iPaid.Count() > 0)
                {
                    vPaid = iPaid.FirstOrDefault().Paid;
                }
                InvoiceListViewModel _vObj = new InvoiceListViewModel();
                _vObj.InvoiceId = invoices.Id;
                _vObj.SysCreateDate = invoices.SysCreateDate;
                _vObj.AirlineCode = invoices.AirlinesS.Code;
                _vObj.AgentCust = invoices.Agents.Name;
                _vObj.Total = vTotal;
                _vObj.Paid = vPaid;
                _vObj.Refund = Convert.ToDouble(0);
                _vObj.Due = Convert.ToDouble(vTotal) - Convert.ToDouble(vPaid);
                _vObj.PersonName = invoices.ApplicationUsers.PersonName;

                _lstLstObj.Add(_vObj);
                return _lstLstObj;
            }
            else
            {
                return _lstLstObj;
            }
        }
        public ActionResult DeleteTicket(int? InvoiceId, int? TicketId)
        {
            if(InvoiceId == null || TicketId == null)
            {
                FlashMessage.Danger("Invoice ID or Ticket ID can't be null");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            double PaidAmount = new SearchDA().GetInvoicePaidAmount((int)InvoiceId);
            double TicketAmount = new SearchDA().GetTicketAmountOfAgentCustomer((int)TicketId);
            int PaymentStatus = new SearchDA().GetInvoiceAgentCustomerPaymentStatus((int)InvoiceId);
            if (PaidAmount > TicketAmount)
            {
                FlashMessage.Danger("Paid amount is greater then the amount of this ticket.");
                return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
            }
            else if(PaymentStatus == 1)
            {
                FlashMessage.Danger("Invoice is already paid.");
                return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
            }
            else
            {
                bool status = new SearchDA().DeleteTicketFromInvoice((int)TicketId, (int)InvoiceId, User.Identity.GetUserId());
                if (status)
                {
                    FlashMessage.Confirmation("Ticket Deleted from this Invoice.");
                    return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
                }
                else
                {
                    FlashMessage.Danger("Something went wrong!! Try again later.");
                    return RedirectToAction("Details", "Invoice", new { id = InvoiceId });
                }
            }
        }
        public ActionResult updateSegment(int? InvoiceId)
        {
            if (InvoiceId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<InvoiceDetailsSegmentViewModel> SegmentList = new SearchDA().GetInvoiceSegmentInfo((int)InvoiceId);
            ViewBag.AirlinesCode = new SelectList(db.Airliness.OrderBy(a => a.Code).ToList(), "Code", "Code");
            ViewBag.InvoiceID = (int)InvoiceId;
            return View(SegmentList);
        }
        #region NotNeccessary
        public ActionResult Create()
        {
            ViewBag.AgentId = new SelectList(db.Agents, "Id", "Name");
            ViewBag.AirlinesId = new SelectList(db.Airliness, "Id", "Name");
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvoiceType,AgentId,GdsBookingDate,AirlinesId,SysCreateDate,ApplicationUserId,VendorId,VendorInvId,Pnr,ExpectedDatePayment,GDSs,GDSUserId,CancellationChargeBefore,CancellationChargeAfter,CancellationDateBefore,CancellationDateAfter,NoShowBefore,NoShowAfter,InvoiceStatusS")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgentId = new SelectList(db.Agents, "Id", "Name", invoice.AgentId);
            ViewBag.AirlinesId = new SelectList(db.Airliness, "Id", "Name", invoice.AirlinesId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", invoice.VendorId);
            return View(invoice);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentId = new SelectList(db.Agents, "Id", "Name", invoice.AgentId);
            ViewBag.AirlinesId = new SelectList(db.Airliness, "Id", "Name", invoice.AirlinesId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", invoice.VendorId);
            return View(invoice);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InvoiceType,AgentId,GdsBookingDate,AirlinesId,SysCreateDate,ApplicationUserId,VendorId,VendorInvId,Pnr,ExpectedDatePayment,GDSs,GDSUserId,CancellationChargeBefore,CancellationChargeAfter,CancellationDateBefore,CancellationDateAfter,NoShowBefore,NoShowAfter,InvoiceStatusS")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentId = new SelectList(db.Agents, "Id", "Name", invoice.AgentId);
            ViewBag.AirlinesId = new SelectList(db.Airliness, "Id", "Name", invoice.AirlinesId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", invoice.VendorId);
            return View(invoice);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        } 
        #endregion
    }
}
