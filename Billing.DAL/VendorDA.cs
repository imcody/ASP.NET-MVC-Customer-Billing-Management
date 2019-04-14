using Billing.ViewModel;
using Billing.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Billing.DAL.Parameters;

namespace Billing.DAL
{
    public partial class VendorDA
    {
        public List<VendorListViewModel> getVendorList()
        {
            List<VendorListViewModel> lstObj = new List<VendorListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetVendorList_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            VendorListViewModel vendor = new VendorListViewModel();
                            vendor.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            vendor.Name = reader["Name"] is DBNull ? string.Empty : Convert.ToString(reader["Name"]);
                            vendor.Email = reader["Email"] is DBNull ? string.Empty : Convert.ToString(reader["Email"]);
                            vendor.Address = reader["Address"] is DBNull ? string.Empty : Convert.ToString(reader["Address"]);
                            vendor.Telephone = reader["Telephone"] is DBNull ? string.Empty : (String)reader["Telephone"];
                            vendor.FaxNo = reader["FaxNo"] is DBNull ? string.Empty : (String)reader["FaxNo"];
                            vendor.NetSafi = reader["NetSafi"] is DBNull ? 0 : Convert.ToDouble(reader["NetSafi"]);
                            vendor.Balance = reader["Balance"] is DBNull ? 0 : Convert.ToDouble(reader["Balance"]);
                            lstObj.Add(vendor);
                        }
                    }
                    reader.Close();
                    return lstObj;
                }
            }
            catch (Exception ex)
            {
                return lstObj;
            }
        }
        public List<VendorLedgerViewModel> VendorsLedgerList(int id)
        {
            List<VendorLedgerViewModel> lstObj = new List<VendorLedgerViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetVendorsLedgerList_SP");
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, id);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            VendorLedgerViewModel vendor = new VendorLedgerViewModel();
                            vendor.LedgerId = reader["LedgerId"] is DBNull ? 0 : Convert.ToInt32(reader["LedgerId"]);
                            vendor.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            vendor.Debit = reader["Debit"] is DBNull ? 0 : Convert.ToDouble(reader["Debit"]);
                            vendor.Credit = reader["Credit"] is DBNull ? 0 : Convert.ToDouble(reader["Credit"]);
                            vendor.Balance = reader["Balance"] is DBNull ? 0 : Convert.ToDouble(reader["Balance"]);
                            vendor.LedgerDate = reader["LedgerDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["LedgerDate"]);
                            vendor.LedgerHead = reader["LedgerHead"] is DBNull ? string.Empty : Convert.ToString(reader["LedgerHead"]);
                            vendor.LedgerType = reader["LedgerType"] is DBNull ? string.Empty : Convert.ToString(reader["LedgerType"]);
                            vendor.Notes = reader["Notes"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            lstObj.Add(vendor);
                        }
                    }
                    reader.Close();
                    return lstObj;
                }
            }
            catch (Exception ex)
            {
                return lstObj;
            }
        }
        public List<VendorOutstandingInvoiceListViewModel> GetVendorOutstandingInvoiceList(int VendorId)
        {
            List<VendorOutstandingInvoiceListViewModel> lstObj = new List<VendorOutstandingInvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetLatestOutstandingInvoiceListByVendorId_SP");
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, VendorId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            double Total = reader["Total"] is DBNull ? 0 : Convert.ToDouble(reader["Total"]);
                            if (Total > 0)
                            {
                                VendorOutstandingInvoiceListViewModel inv = new VendorOutstandingInvoiceListViewModel();
                                inv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                                inv.InvoiceDate = reader["InvoiceDate"] is DBNull ? string.Empty : Convert.ToString(reader["InvoiceDate"]);
                                inv.Total = Total;
                                inv.Paid = reader["Paid"] is DBNull ? 0 : Convert.ToDouble(reader["Paid"]);
                                inv.Due = (inv.Total - inv.Paid);
                                lstObj.Add(inv);
                            }
                        }
                    }
                    reader.Close();
                    return lstObj;
                }
            }
            catch (Exception ex)
            {
                return lstObj;
            }
        }
        public string VendorBulkPaymentCashVoucher(VendorBulkPaymentCashVoucher model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentVendorCashVoucher_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, model.VendorId);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);

                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            status = reader["IndentityVals"] is DBNull ? string.Empty : Convert.ToString(reader["IndentityVals"]);
                        }
                    }
                    reader.Close();
                    return status;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public string VendorJournalPaymentCashVoucher(VendorBulkPaymentCashVoucher model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertJournalPaymentVendorCashVoucher_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, model.VendorId);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);

                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            status = reader["IndentityVals"] is DBNull ? string.Empty : Convert.ToString(reader["IndentityVals"]);
                        }
                    }
                    reader.Close();
                    return status;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public bool VendorBulkPaymentChequeVoucher(VendorBulkPaymentChequeVoucher model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentVendorChequeVoucher_SP");
                    DataAccess.AddInParameter(command, "@BankNames", SqlDbType.Int, model.BankNames);
                    DataAccess.AddInParameter(command, "@AccountNo", SqlDbType.VarChar, model.AccountNo);
                    DataAccess.AddInParameter(command, "@ChequeNo", SqlDbType.VarChar, model.ChequeNo);
                    DataAccess.AddInParameter(command, "@SortCode", SqlDbType.VarChar, model.SortCode);
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.VarChar, model.Amount);
                    DataAccess.AddInParameter(command, "@Remarks", SqlDbType.VarChar, model.Remarks);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, model.VendorId);
                    DataAccess.AddInParameter(command, "@BulkPayment", SqlDbType.Int, Convert.ToInt32(model.BulkPayment));
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string VendorBulkPaymentCreditCard(VendorBulkPaymentCreditCard model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentVendorCreditCard_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, model.VendorId);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, model.BankAccountId);
                    DataAccess.AddInParameter(command, "@CardNo", SqlDbType.VarChar, model.CardNo);
                    DataAccess.AddInParameter(command, "@CardHolder", SqlDbType.VarChar, model.CardHolder);
                    DataAccess.AddInParameter(command, "@ExtraAmount", SqlDbType.VarChar, model.ExtraAmount);
                    DataAccess.AddInParameter(command, "@BankDate", SqlDbType.VarChar, model.BankDate);

                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            status = reader["IndentityVals"] is DBNull ? string.Empty : Convert.ToString(reader["IndentityVals"]);
                        }
                    }
                    reader.Close();
                    return status;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public string VendorBulkPaymentDebitCard(VendorBulkPaymentDebitCard model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentVendorDebitCard_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, model.VendorId);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, model.BankAccountId);
                    DataAccess.AddInParameter(command, "@CardNo", SqlDbType.VarChar, model.CardNo);
                    DataAccess.AddInParameter(command, "@CardHolder", SqlDbType.VarChar, model.CardHolder);
                    DataAccess.AddInParameter(command, "@ExtraAmount", SqlDbType.VarChar, model.ExtraAmount);
                    DataAccess.AddInParameter(command, "@BankDate", SqlDbType.VarChar, model.BankDate);

                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            status = reader["IndentityVals"] is DBNull ? string.Empty : Convert.ToString(reader["IndentityVals"]);
                        }
                    }
                    reader.Close();
                    return status;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public string VendorBulkPaymentBankDeposit(VendorBulkPaymentBankDeposit model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentVendorBankDeposit_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, model.VendorId);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, model.BankAccountId);
                    DataAccess.AddInParameter(command, "@BankDate", SqlDbType.VarChar, model.BankDate);

                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            status = reader["IndentityVals"] is DBNull ? string.Empty : Convert.ToString(reader["IndentityVals"]);
                        }
                    }
                    reader.Close();
                    return status;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public bool BulkPaymentVendorCashVoucherInvoicePaymentInvoiceLog(BulkPaymentVendorCashInvPaymentInvLog model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentVendorCashVoucherInvoicePaymentInvoiceLog_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, model.InvoiceId);
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, model.UserID);
                    DataAccess.AddInParameter(command, "@GeneralLedgerId", SqlDbType.Int, model.GeneralLedgerId);
                    DataAccess.AddInParameter(command, "@VendorLedgerId", SqlDbType.Int, model.VendorLedgerId);
                    DataAccess.AddInParameter(command, "@BankAccountLedgerId", SqlDbType.Int, model.BankAccountLedgerId);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, model.VendorId);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<VendorLedgerViewModel> VendorsLedgerListSearchByDateRange(int VendorId, string sDate, string eDate)
        {
            List<VendorLedgerViewModel> lstObj = new List<VendorLedgerViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetVendorsLedgerListSearchByDateRange_SP");
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, VendorId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            VendorLedgerViewModel agObj = new VendorLedgerViewModel();
                            agObj.LedgerId = reader["LedgerId"] is DBNull ? 0 : Convert.ToInt32(reader["LedgerId"]);
                            agObj.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            agObj.Debit = reader["Debit"] is DBNull ? 0 : Convert.ToDouble(reader["Debit"]);
                            agObj.Credit = reader["Credit"] is DBNull ? 0 : Convert.ToDouble(reader["Credit"]);
                            agObj.Balance = reader["Balance"] is DBNull ? 0 : Convert.ToDouble(reader["Balance"]);
                            agObj.LedgerDate = reader["LedgerDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["LedgerDate"]);
                            agObj.LedgerHead = reader["LedgerHead"] is DBNull ? string.Empty : Convert.ToString(reader["LedgerHead"]);
                            agObj.LedgerType = reader["LedgerType"] is DBNull ? string.Empty : Convert.ToString(reader["LedgerType"]);
                            agObj.Notes = reader["Notes"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            lstObj.Add(agObj);
                        }
                    }
                    reader.Close();
                    return lstObj;
                }
            }
            catch (Exception ex)
            {
                return lstObj;
            }
        }
        public List<VendorLedgerViewModel> VendorsLedgerListSearchByDateRangeLedgerHead(int VendorId, string sDate, string eDate, int LedgerHead)
        {
            List<VendorLedgerViewModel> lstObj = new List<VendorLedgerViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetVendorsLedgerListSearchByDateRangeLedgerHead_SP");
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, VendorId);
                    DataAccess.AddInParameter(command, "@LedgerHead", SqlDbType.Int, LedgerHead);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            VendorLedgerViewModel agObj = new VendorLedgerViewModel();
                            agObj.LedgerId = reader["LedgerId"] is DBNull ? 0 : Convert.ToInt32(reader["LedgerId"]);
                            agObj.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            agObj.Debit = reader["Debit"] is DBNull ? 0 : Convert.ToDouble(reader["Debit"]);
                            agObj.Credit = reader["Credit"] is DBNull ? 0 : Convert.ToDouble(reader["Credit"]);
                            agObj.Balance = reader["Balance"] is DBNull ? 0 : Convert.ToDouble(reader["Balance"]);
                            agObj.LedgerDate = reader["LedgerDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["LedgerDate"]);
                            agObj.LedgerHead = reader["LedgerHead"] is DBNull ? string.Empty : Convert.ToString(reader["LedgerHead"]);
                            agObj.LedgerType = reader["LedgerType"] is DBNull ? string.Empty : Convert.ToString(reader["LedgerType"]);
                            agObj.Notes = reader["Notes"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            lstObj.Add(agObj);
                        }
                    }
                    reader.Close();
                    return lstObj;
                }
            }
            catch (Exception ex)
            {
                return lstObj;
            }
        }
        public List<InvoicePaymentHistoryVendor> GetInvoicePaymentHistoryByVendor(int VendorId)
        {
            List<InvoicePaymentHistoryVendor> lstObj = new List<InvoicePaymentHistoryVendor>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoicePaymentHistoryByVendorId_SP");
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, VendorId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoicePaymentHistoryVendor inv = new InvoicePaymentHistoryVendor();
                            inv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            inv.PaymentDate = reader["PaymentDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["PaymentDate"]);
                            inv.UserName = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["UserName"]);
                            inv.Notes = reader["Notes"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            inv.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            inv.VendorId = reader["VendorId"] is DBNull ? 0 : Convert.ToInt32(reader["VendorId"]);
                            inv.VendorName = reader["VendorName"] is DBNull ? string.Empty : Convert.ToString(reader["VendorName"]);
                            inv.PaymentMethod = reader["PaymentMethod"] is DBNull ? string.Empty : Enum.GetName(typeof(PaymentMethod), Convert.ToInt32(reader["PaymentMethod"]));
                            lstObj.Add(inv);
                        }
                    }
                    reader.Close();
                    return lstObj;
                }
            }
            catch (Exception ex)
            {
                return lstObj;
            }
        }
        public List<InvoicePaymentHistoryVendor> GetInvoicePaymentHistoryByInvoiceAndVendor(int InvoiceId)
        {
            List<InvoicePaymentHistoryVendor> lstObj = new List<InvoicePaymentHistoryVendor>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoicePaymentHistoryByInvoiceIdVendorId_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoicePaymentHistoryVendor inv = new InvoicePaymentHistoryVendor();
                            inv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            inv.PaymentDate = reader["PaymentDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["PaymentDate"]);
                            inv.UserName = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["UserName"]);
                            inv.Notes = reader["Notes"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            inv.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            inv.VendorId = reader["VendorId"] is DBNull ? 0 : Convert.ToInt32(reader["VendorId"]);
                            inv.VendorName = reader["VendorName"] is DBNull ? string.Empty : Convert.ToString(reader["VendorName"]);
                            inv.PaymentMethod = reader["PaymentMethod"] is DBNull ? string.Empty : Enum.GetName(typeof(PaymentMethod), Convert.ToInt32(reader["PaymentMethod"]));
                            lstObj.Add(inv);
                        }
                    }
                    reader.Close();
                    return lstObj;
                }
            }
            catch (Exception ex)
            {
                return lstObj;
            }
        }
        public List<InvoicePaymentHistoryVendor> GetInvoicePaymentHistoryByDateRangeVendor(string sDate, string eDate)
        {
            List<InvoicePaymentHistoryVendor> lstObj = new List<InvoicePaymentHistoryVendor>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoicePaymentHistoryByDateRangeVendor_SP");
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoicePaymentHistoryVendor inv = new InvoicePaymentHistoryVendor();
                            inv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            inv.PaymentDate = reader["PaymentDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["PaymentDate"]);
                            inv.UserName = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["UserName"]);
                            inv.Notes = reader["Notes"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            inv.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            inv.VendorId = reader["VendorId"] is DBNull ? 0 : Convert.ToInt32(reader["VendorId"]);
                            inv.VendorName = reader["VendorName"] is DBNull ? string.Empty : Convert.ToString(reader["VendorName"]);
                            inv.PaymentMethod = reader["PaymentMethod"] is DBNull ? string.Empty : Enum.GetName(typeof(PaymentMethod), Convert.ToInt32(reader["PaymentMethod"]));
                            lstObj.Add(inv);
                        }
                    }
                    reader.Close();
                    return lstObj;
                }
            }
            catch (Exception ex)
            {
                return lstObj;
            }
        }
        public List<InvoicePaymentHistoryVendor> GetInvoicePaymentHistoryByDateVendor(int VendorId, string sDate, string eDate)
        {
            List<InvoicePaymentHistoryVendor> lstObj = new List<InvoicePaymentHistoryVendor>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoicePaymentHistoryByDateVendor_SP");
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, VendorId);
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoicePaymentHistoryVendor inv = new InvoicePaymentHistoryVendor();
                            inv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            inv.PaymentDate = reader["PaymentDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["PaymentDate"]);
                            inv.UserName = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["UserName"]);
                            inv.Notes = reader["Notes"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            inv.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            inv.VendorId = reader["VendorId"] is DBNull ? 0 : Convert.ToInt32(reader["VendorId"]);
                            inv.VendorName = reader["VendorName"] is DBNull ? string.Empty : Convert.ToString(reader["VendorName"]);
                            inv.PaymentMethod = reader["PaymentMethod"] is DBNull ? string.Empty : Enum.GetName(typeof(PaymentMethod), Convert.ToInt32(reader["PaymentMethod"]));
                            lstObj.Add(inv);
                        }
                    }
                    reader.Close();
                    return lstObj;
                }
            }
            catch (Exception ex)
            {
                return lstObj;
            }
        }
        public List<InvoiceListViewModel> GetOutstandingInvoiceListByVendorId(int VendorId)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetOutstandingInvoiceListByVendorId_SP");
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, VendorId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoiceListViewModel invoice = new InvoiceListViewModel();
                            invoice.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            invoice.SysCreateDate = reader["SysCreateDate"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["SysCreateDate"]);
                            invoice.AirlineCode = reader["AirlineCode"] is DBNull ? string.Empty : (String)reader["AirlineCode"];
                            invoice.AgentCust = reader["AgentCust"] is DBNull ? string.Empty : (String)reader["AgentCust"];
                            invoice.Total = reader["Total"] is DBNull ? 0 : Convert.ToDouble(reader["Total"]);
                            invoice.Paid = reader["Paid"] is DBNull ? 0 : Convert.ToDouble(reader["Paid"]);
                            invoice.Refund = Convert.ToDouble(0);
                            invoice.Due = (invoice.Total - invoice.Paid);
                            invoice.PersonName = reader["Person"] is DBNull ? string.Empty : (String)reader["Person"];
                            invList.Add(invoice);
                        }
                    }
                    reader.Close();
                    return invList;
                }
            }
            catch (Exception ex)
            {
                return invList;
            }
        }
        public IPChequeDetail VendorBulkPaymentFloatingCheque(int Cheid)
        {
            IPChequeDetail Obj = new IPChequeDetail();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetAgentBulkPaymentFloatingCheque_SP");
                    DataAccess.AddInParameter(command, "@Cheid", SqlDbType.Int, Cheid);

                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Obj.AccountNo = reader["AccountNo"] is DBNull ? string.Empty : Convert.ToString(reader["AccountNo"]);
                            Obj.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            //Obj.BankNames = reader["BankNames"] is DBNull ? 0 : (BankName)reader["BankNames"];
                            Obj.SortCode = reader["SortCode"] is DBNull ? string.Empty : Convert.ToString(reader["SortCode"]);
                            Obj.SysCreateDate = reader["LedgerDate"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["LedgerDate"]);
                            Obj.Remarks = reader["Remarks"] is DBNull ? string.Empty : Convert.ToString(reader["Remarks"]);
                        }
                    }
                    reader.Close();
                    return Obj;
                }
            }
            catch (Exception ex)
            {
                return Obj;
            }
        }
        public string VendorBulkPaymentChequePayment(VendorBulkPaymentChequePayment model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentVendorChequePayment_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, model.VendorId);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, model.BankAccountId);
                    DataAccess.AddInParameter(command, "@BankDate", SqlDbType.VarChar, model.BankDate);
                    DataAccess.AddInParameter(command, "@ChekId", SqlDbType.Int, model.ChekId);

                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            status = reader["IndentityVals"] is DBNull ? string.Empty : Convert.ToString(reader["IndentityVals"]);
                        }
                    }
                    reader.Close();
                    return status;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}