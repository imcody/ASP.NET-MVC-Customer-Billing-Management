using Billing.DAL;
using Billing.Entities;
using Billing.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Billing.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            List<InvoiceListViewModel> ilObj = new SearchDA().GetLatestInvoiceList();
            List<FloatChequeViewModel> fcObj = new AccountingDA().GetFloatingChequeList(1);
            List<FloatChequeViewModel> bfcObj = new AccountingDA().GetBulkFloatingChequeList(1);
            List<InvoiceListViewModel> lstLstObj = new SearchDA().GetDraftInvoiceList();
            HomeViewModel model = new HomeViewModel();
            model.InvoiceList = ilObj.Take(10).ToList();
            model.DraftList = lstLstObj.Take(10).ToList();
            model.FloatingCheques = fcObj.Take(10).ToList();
            model.BulkFloatingCheques = bfcObj.Take(10).ToList();
            ViewBag.AgentList = new SelectList(db.Agents.Where(a => a.ProfileType == ProfileType.Agent).OrderBy(a => a.Name).ToList(), "Id", "Name");
            ViewBag.UserList = new SelectList(db.Users.OrderBy(x => x.PersonName).ToList(), "Id", "PersonName");
            return View(model);
        }
        public PartialViewResult Search(int? invoiceID, string ToDate, string FromDate, string SearchBy, string SearchValue)
        {
            List<InvoiceListViewModel> lstLstObj = new List<InvoiceListViewModel>();
            #region SearchByInvoiceID
            if (invoiceID > 0 && invoiceID != null)
            {
                var invoices = db.Invoices.Include(i => i.Agents).Include(i => i.AirlinesS).Include(i => i.Vendors).Where(a => a.Id == invoiceID).FirstOrDefault(); //.OrderBy(a => a.SysCreateDate).ToList();
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

                    lstLstObj.Add(_vObj);
                    return PartialView("Search", lstLstObj);
                }
                else
                {
                    return PartialView("Search", lstLstObj);
                }
            }
            #endregion
            #region SearchByLastNamePnrTicketNo
            else if (Convert.ToInt32(SearchBy) > 0)
            {
                int InvID = 0;
                if (String.IsNullOrEmpty(SearchValue))
                {
                    return PartialView("Search", lstLstObj);
                }
                else
                {
                    if (Convert.ToInt32(SearchBy) == 1)
                    {
                        if (!string.IsNullOrEmpty(SearchBy))
                        {
                            lstLstObj = new SearchDA().SearchInvoicesByPaxLastNames(SearchValue);
                            return PartialView("Search", lstLstObj);
                        }
                        else
                        {
                            return PartialView("Search", lstLstObj);
                        }
                    }
                    else if (Convert.ToInt32(SearchBy) == 2)
                    {
                        if (!string.IsNullOrEmpty(SearchBy))
                        {
                            lstLstObj = new SearchDA().SearchInvoicesByTicketNo(SearchValue);
                            return PartialView("Search", lstLstObj);
                        }
                        else
                        {
                            return PartialView("Search", lstLstObj);
                        }
                    }
                    else if (Convert.ToInt32(SearchBy) == 3)
                    {
                        if (!string.IsNullOrEmpty(SearchBy))
                        {
                            lstLstObj = new SearchDA().SearchInvoicesByPNR(SearchValue);
                            return PartialView("Search", lstLstObj);
                        }
                        else
                        {
                            return PartialView("Search", lstLstObj);
                        }
                    }
                }
            }
            #endregion
            #region SearchByDateOrDateRange
            else if (!string.IsNullOrEmpty(FromDate))
            {
                
                if(string.IsNullOrEmpty(ToDate))
                {
                    lstLstObj = new SearchDA().SearchInvoicesByDate(FromDate);
                    return PartialView("Search", lstLstObj);
                }
                else
                {
                    lstLstObj = new SearchDA().SearchInvoicesByDateRange(FromDate, ToDate);
                    return PartialView("Search", lstLstObj);
                }
            } 
            #endregion
            return PartialView("Search", lstLstObj);
        }
        public PartialViewResult SearchForHome(string ToDate, string FromDate, string UserId)
        {
            List<InvoiceListViewModel> lstLstObj = new SearchDA().SearchInvoicesByDateRangeForHome(FromDate, ToDate, UserId);
            return PartialView("Invoice/InvoiceList", lstLstObj);
        }
        public PartialViewResult DraftSearchForHome(string ToDate, string FromDate, string UserId)
        {
            List<InvoiceListViewModel> lstLstObj = new SearchDA().SearchDraftByDateRangeForHome(FromDate, ToDate, UserId);
            return PartialView("Home/DraftList", lstLstObj);
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
        public JsonResult GetQuickAgentContact(int AgentId)
        {
            try
            {
                Agent Obj = db.Agents.Find(AgentId);
                if (Obj == null)
                {
                    return Json(new
                    {
                        Telephone = string.Empty,
                        Mobile = string.Empty,
                        FaxNo = string.Empty,
                        Address = string.Empty,
                        PostCode = string.Empty,
                        Email = string.Empty,
                        Remarks = string.Empty,
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Telephone = Obj.Telephone,
                        Mobile = Obj.Mobile,
                        FaxNo = Obj.FaxNo,
                        Address = Obj.Address,
                        PostCode = Obj.Postcode,
                        Email = Obj.Email,
                        Remarks = Obj.Remarks,
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Telephone = string.Empty,
                    Mobile = string.Empty,
                    FaxNo = string.Empty,
                    Address = string.Empty,
                    PostCode = string.Empty,
                    Email = string.Empty,
                    Remarks = string.Empty,
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}