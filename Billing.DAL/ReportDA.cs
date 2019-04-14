using Billing.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Billing.DAL.Parameters;
using Billing.ViewModel;

namespace Billing.DAL
{
    public partial class ReportDA
    {
        public List<TicketingReport> GetTicketingReportByDateRange(string sDate, string eDate)
        {
            List<TicketingReport> tktReport = new List<TicketingReport>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetTicketingReportByDateRange_SP");
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            TicketingReport report = new TicketingReport();
                            report.AgentName = reader["AgentName"] is DBNull ? string.Empty : Convert.ToString(reader["AgentName"]);
                            report.Apc = reader["Apc"] is DBNull ? 0 : Convert.ToDouble(reader["Apc"]);
                            report.CustomerAmount = reader["CAmount"] is DBNull ? 0 : Convert.ToDouble(reader["CAmount"]);
                            report.InvDate = reader["InvoiceDate"] is DBNull ? string.Empty : (String)reader["InvoiceDate"];
                            report.InvoiceID = reader["InvoiceID"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceID"]);
                            report.PaxName = reader["PaxName"] is DBNull ? string.Empty : Convert.ToString(reader["PaxName"]);
                            report.TicketNo = reader["TicketNo"] is DBNull ? string.Empty : Convert.ToString(reader["TicketNo"]);
                            report.TicketTax = reader["TicketTax"] is DBNull ? 0 : Convert.ToDouble(reader["TicketTax"]);
                            report.VendorCharge = reader["VendorCharge"] is DBNull ? 0 : Convert.ToDouble(reader["VendorCharge"]);
                            report.VendorName = reader["VendorName"] is DBNull ? string.Empty : Convert.ToString(reader["VendorName"]);
                            report.VNetFare = reader["VNetFare"] is DBNull ? 0 : Convert.ToDouble(reader["VNetFare"]);
                            report.Status = reader["Status"] is DBNull ? 0 : Convert.ToInt32(reader["Status"]);
                            report.VendorId = reader["VendorId"] is DBNull ? 0 : Convert.ToInt32(reader["VendorId"]);
                            report.AAtol = reader["AAtol"] is DBNull ? 0 : Convert.ToInt32(reader["AAtol"]);
                            report.PaxType = reader["PaxType"] is DBNull ? 0 : Convert.ToInt32(reader["PaxType"]);
                            report.InvType = reader["InvType"] is DBNull ? 0 : Convert.ToInt32(reader["InvType"]);
                            tktReport.Add(report);
                        }
                    }
                    reader.Close();
                    return tktReport;
                }
            }
            catch (Exception ex)
            {
                return tktReport;
            }
        }
        public List<TicketingReport> GetTicketingReportByDateRangeAndAgent(string sDate, string eDate, int AgentId)
        {
            List<TicketingReport> tktReport = new List<TicketingReport>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetTicketingReportByDateRangeAndAgent_SP");
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, AgentId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            TicketingReport report = new TicketingReport();
                            report.AgentName = reader["AgentName"] is DBNull ? string.Empty : Convert.ToString(reader["AgentName"]);
                            report.Apc = reader["Apc"] is DBNull ? 0 : Convert.ToDouble(reader["Apc"]);
                            report.CustomerAmount = reader["CAmount"] is DBNull ? 0 : Convert.ToDouble(reader["CAmount"]);
                            report.InvDate = reader["InvoiceDate"] is DBNull ? string.Empty : (String)reader["InvoiceDate"];
                            report.InvoiceID = reader["InvoiceID"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceID"]);
                            report.PaxName = reader["PaxName"] is DBNull ? string.Empty : Convert.ToString(reader["PaxName"]);
                            report.TicketNo = reader["TicketNo"] is DBNull ? string.Empty : Convert.ToString(reader["TicketNo"]);
                            report.TicketTax = reader["TicketTax"] is DBNull ? 0 : Convert.ToDouble(reader["TicketTax"]);
                            report.VendorCharge = reader["VendorCharge"] is DBNull ? 0 : Convert.ToDouble(reader["VendorCharge"]);
                            report.VendorName = reader["VendorName"] is DBNull ? string.Empty : Convert.ToString(reader["VendorName"]);
                            report.VNetFare = reader["VNetFare"] is DBNull ? 0 : Convert.ToDouble(reader["VNetFare"]);
                            tktReport.Add(report);
                        }
                    }
                    reader.Close();
                    return tktReport;
                }
            }
            catch (Exception ex)
            {
                return tktReport;
            }
        }
        public List<TicketingReport> GetTicketingReportByDateRangeAndVendor(string sDate, string eDate, int VendorId)
        {
            List<TicketingReport> tktReport = new List<TicketingReport>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetTicketingReportByDateRangeAndVendor_SP");
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, VendorId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            TicketingReport report = new TicketingReport();
                            report.AgentName = reader["AgentName"] is DBNull ? string.Empty : Convert.ToString(reader["AgentName"]);
                            report.Apc = reader["Apc"] is DBNull ? 0 : Convert.ToDouble(reader["Apc"]);
                            report.CustomerAmount = reader["CAmount"] is DBNull ? 0 : Convert.ToDouble(reader["CAmount"]);
                            report.InvDate = reader["InvoiceDate"] is DBNull ? string.Empty : (String)reader["InvoiceDate"];
                            report.InvoiceID = reader["InvoiceID"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceID"]);
                            report.PaxName = reader["PaxName"] is DBNull ? string.Empty : Convert.ToString(reader["PaxName"]);
                            report.TicketNo = reader["TicketNo"] is DBNull ? string.Empty : Convert.ToString(reader["TicketNo"]);
                            report.TicketTax = reader["TicketTax"] is DBNull ? 0 : Convert.ToDouble(reader["TicketTax"]);
                            report.VendorCharge = reader["VendorCharge"] is DBNull ? 0 : Convert.ToDouble(reader["VendorCharge"]);
                            report.VendorName = reader["VendorName"] is DBNull ? string.Empty : Convert.ToString(reader["VendorName"]);
                            report.VNetFare = reader["VNetFare"] is DBNull ? 0 : Convert.ToDouble(reader["VNetFare"]);
                            tktReport.Add(report);
                        }
                    }
                    reader.Close();
                    return tktReport;
                }
            }
            catch (Exception ex)
            {
                return tktReport;
            }
        }
        public List<TicketingReport> GetTicketingReportByDateRangeAgentVendor(string sDate, string eDate, int AgentId, int VendorId)
        {
            List<TicketingReport> tktReport = new List<TicketingReport>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetTicketingReportByDateRangeAgentVendor_SP");
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, AgentId);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, VendorId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            TicketingReport report = new TicketingReport();
                            report.AgentName = reader["AgentName"] is DBNull ? string.Empty : Convert.ToString(reader["AgentName"]);
                            report.Apc = reader["Apc"] is DBNull ? 0 : Convert.ToDouble(reader["Apc"]);
                            report.CustomerAmount = reader["CAmount"] is DBNull ? 0 : Convert.ToDouble(reader["CAmount"]);
                            report.InvDate = reader["InvoiceDate"] is DBNull ? string.Empty : (String)reader["InvoiceDate"];
                            report.InvoiceID = reader["InvoiceID"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceID"]);
                            report.PaxName = reader["PaxName"] is DBNull ? string.Empty : Convert.ToString(reader["PaxName"]);
                            report.TicketNo = reader["TicketNo"] is DBNull ? string.Empty : Convert.ToString(reader["TicketNo"]);
                            report.TicketTax = reader["TicketTax"] is DBNull ? 0 : Convert.ToDouble(reader["TicketTax"]);
                            report.VendorCharge = reader["VendorCharge"] is DBNull ? 0 : Convert.ToDouble(reader["VendorCharge"]);
                            report.VendorName = reader["VendorName"] is DBNull ? string.Empty : Convert.ToString(reader["VendorName"]);
                            report.VNetFare = reader["VNetFare"] is DBNull ? 0 : Convert.ToDouble(reader["VNetFare"]);
                            tktReport.Add(report);
                        }
                    }
                    reader.Close();
                    return tktReport;
                }
            }
            catch (Exception ex)
            {
                return tktReport;
            }
        }
        public DailyReport GetDailyCollectionSummary(string SearchDate)
        {
            DailyReport dReport = new DailyReport();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetDailyCollectionReportSummary_SP");
                    DataAccess.AddInParameter(command, "@SearchDate", SqlDbType.VarChar, SearchDate);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            dReport.CashCollection = reader["Cash"] is DBNull ? 0 : Convert.ToDouble(reader["Cash"]);
                            dReport.ChequeCollection = reader["Cheque"] is DBNull ? 0 : Convert.ToDouble(reader["Cheque"]);
                            dReport.CCardCollection = reader["CreditCard"] is DBNull ? 0 : Convert.ToDouble(reader["CreditCard"]);
                            dReport.DCardCollection = reader["DebitCard"] is DBNull ? 0 : Convert.ToDouble(reader["DebitCard"]);
                            dReport.BankDeposit = reader["BankDeposit"] is DBNull ? 0 : Convert.ToDouble(reader["BankDeposit"]);
                            dReport.DailyTotal = reader["Total"] is DBNull ? 0 : Convert.ToDouble(reader["Total"]);
                        }
                    }
                    reader.Close();
                    return dReport;
                }
            }
            catch (Exception ex)
            {
                return dReport;
            }
        }
        public List<Int32> GetInvoiceIds()
        {
            List<Int32> lstObj = new List<Int32>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetListOfUniqueInvoiceId_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Int32 InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            lstObj.Add(InvoiceId);
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
        public List<TransactionEMP> GetAllTransactionSummary()
        {
            List<TransactionEMP> transactions = new List<TransactionEMP>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetAllTransactionSummary_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            TransactionEMP transaction = new TransactionEMP();
                            transaction.acc_no = reader["acc_no"] is DBNull ? 0 : Convert.ToInt32(reader["acc_no"]);
                            transaction.adjust_inv_no = reader["adjust_inv_no"] is DBNull ? 0 : Convert.ToInt32(reader["adjust_inv_no"]);
                            transaction.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            transaction.bank_chrg = reader["bank_chrg"] is DBNull ? string.Empty : (String)reader["bank_chrg"];
                            transaction.bank_dt = reader["bank_dt"] is DBNull ? string.Empty : Convert.ToString(reader["bank_dt"]);
                            transaction.bank_id = reader["bank_id"] is DBNull ? 0 : Convert.ToInt32(reader["bank_id"]);
                            transaction.card_holder_name = reader["card_holder_name"] is DBNull ? string.Empty : Convert.ToString(reader["card_holder_name"]);
                            transaction.card_no = reader["card_no"] is DBNull ? string.Empty : Convert.ToString(reader["card_no"]);
                            transaction.chq_no = reader["chq_no"] is DBNull ? 0 : Convert.ToInt32(reader["chq_no"]);
                            transaction.dep_pur = reader["dep_pur"] is DBNull ? string.Empty : Convert.ToString(reader["dep_pur"]);
                            transaction.ex_amt = reader["ex_amt"] is DBNull ? 0 : Convert.ToDouble(reader["ex_amt"]);
                            transaction.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            transaction.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            transaction.payment_type = reader["payment_type"] is DBNull ? string.Empty : Convert.ToString(reader["payment_type"]);
                            transaction.remark = reader["remark"] is DBNull ? string.Empty : Convert.ToString(reader["remark"]);
                            transaction.sort_code = reader["sort_code"] is DBNull ? 0 : Convert.ToInt32(reader["sort_code"]);
                            transaction.SysDate = reader["SysDate"] is DBNull ? string.Empty : Convert.ToString(reader["SysDate"]);
                            transaction.SysUser = "b59ea919-d974-4ae1-92d3-c462c0e7715f";
                            transaction.tan_type = reader["tran_type"] is DBNull ? 0 : Convert.ToInt32(reader["tran_type"]);
                            transactions.Add(transaction);
                        }
                    }
                    reader.Close();
                    return transactions;
                }
            }
            catch (Exception ex)
            {
                return transactions;
            }
        }
        public List<InvoiceName> GetAllInvoiceNamesSummary()
        {
            List<InvoiceName> iNames = new List<InvoiceName>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetAllInvoiceNamesSummary_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoiceName iName = new InvoiceName();
                            iName.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            iName.Apc = reader["Apc"] is DBNull ? 0 : Convert.ToInt32(reader["Apc"]);
                            iName.BookingDate = reader["BookingDate"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["BookingDate"]);
                            iName.CNetFare = reader["CNetFare"] is DBNull ? 0 : Convert.ToDouble(reader["CNetFare"]);
                            iName.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            iName.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            iName.Name = reader["Name"] is DBNull ? string.Empty : Convert.ToString(reader["Name"]);
                            iName.TicketTax = reader["TicketTax"] is DBNull ? 0 : Convert.ToDouble(reader["TicketTax"]);
                            iName.VendorCharge = reader["VendorCharge"] is DBNull ? 0 : Convert.ToDouble(reader["VendorCharge"]);
                            iName.VNetFare = reader["VNetFare"] is DBNull ? 0 : Convert.ToDouble(reader["VNetFare"]);
                            iNames.Add(iName);
                        }
                    }
                    reader.Close();
                    return iNames;
                }
            }
            catch (Exception ex)
            {
                return iNames;
            }
        }

        //Temporary DataCRUD functions
        public bool InvoicePaymentCashVoucher2(CashTransaction model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertInvoicePaymentCashVoucher2_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Remarks);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, model.InvoiceId);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //
    }
}