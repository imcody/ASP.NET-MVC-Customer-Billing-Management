using Billing.DAL.Parameters;
using Billing.Entities;
using Billing.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Billing.DAL
{
    public partial class AgentDA
    {
        public bool AddNewAgent(Agent model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertNewAgent_SP");
                    DataAccess.AddInParameter(command, "@ProfileType", SqlDbType.Int, (int)model.ProfileType);
                    DataAccess.AddInParameter(command, "@Name", SqlDbType.VarChar, Convert.ToString(model.Name));
                    DataAccess.AddInParameter(command, "@Address", SqlDbType.VarChar, Convert.ToString(model.Address));
                    DataAccess.AddInParameter(command, "@Email", SqlDbType.VarChar, Convert.ToString(model.Email));
                    DataAccess.AddInParameter(command, "@Postcode", SqlDbType.VarChar, Convert.ToString(model.Postcode));
                    DataAccess.AddInParameter(command, "@Telephone", SqlDbType.VarChar, Convert.ToString(model.Telephone));
                    DataAccess.AddInParameter(command, "@Mobile", SqlDbType.VarChar, Convert.ToString(model.Mobile));
                    DataAccess.AddInParameter(command, "@FaxNo", SqlDbType.VarChar, Convert.ToString(model.FaxNo));
                    DataAccess.AddInParameter(command, "@Atol", SqlDbType.VarChar, Convert.ToString(model.Atol));
                    DataAccess.AddInParameter(command, "@CreditLimit", SqlDbType.Float, Convert.ToString(model.CreditLimit));
                    DataAccess.AddInParameter(command, "@Balance", SqlDbType.Float, Convert.ToString(model.Balance));
                    DataAccess.AddInParameter(command, "@Remarks", SqlDbType.VarChar, Convert.ToString(model.Remarks));
                    DataAccess.AddInParameter(command, "@JoiningDate", SqlDbType.DateTime, Convert.ToString(model.JoiningDate));
                    DataAccess.AddInParameter(command, "@ApplicationUserId", SqlDbType.VarChar, Convert.ToString(model.ApplicationUserId));
                    status = DataAccess.ExecuteNonQuery(command);
                    if (status < 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<Agent> GetAgentsList()
        {
            List<Agent> agentList = new List<Agent>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetAgentsList_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            Agent agent = new Agent();
                            agent.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            agent.Name = reader["Name"] is DBNull ? string.Empty : Convert.ToString(reader["Name"]);
                            agent.Email = reader["Email"] is DBNull ? string.Empty : Convert.ToString(reader["Email"]);
                            agent.Telephone = reader["Telephone"] is DBNull ? string.Empty : (String)reader["Telephone"];
                            agent.Mobile = reader["Mobile"] is DBNull ? string.Empty : (String)reader["Mobile"];
                            agent.CreditLimit = reader["CreditLimit"] is DBNull ? 0 : Convert.ToDouble(reader["CreditLimit"]);
                            agent.Balance = reader["Balance"] is DBNull ? 0 : Convert.ToDouble(reader["Balance"]);
                            agentList.Add(agent);
                        }
                    }
                    reader.Close();
                    return agentList;
                }
            }
            catch (Exception ex)
            {
                return agentList;
            }
        }
        public List<AgentOutstandingInvoiceListViewModel> GetAgentOutstandingInvoiceList(int AgentId)
        {
            List<AgentOutstandingInvoiceListViewModel> lstObj = new List<AgentOutstandingInvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetLatestInvoiceListByAgentId_SP");
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, AgentId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            double Total = reader["Total"] is DBNull ? 0 : Convert.ToDouble(reader["Total"]);
                            if(Total > 0)
                            {
                                AgentOutstandingInvoiceListViewModel inv = new AgentOutstandingInvoiceListViewModel();
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
        public string AgentBulkPaymentCashVoucher(AgentBulkPaymentCashVoucher model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentAgentCashVoucher_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, model.AgentId);
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
        public bool AgentBulkPaymentChequeVoucher(AgentBulkPaymentChequeVoucher model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentAgentChequeVoucher_SP");
                    DataAccess.AddInParameter(command, "@BankNames", SqlDbType.Int, model.BankNames);
                    DataAccess.AddInParameter(command, "@AccountNo", SqlDbType.VarChar, model.AccountNo);
                    DataAccess.AddInParameter(command, "@ChequeNo", SqlDbType.VarChar, model.ChequeNo);
                    DataAccess.AddInParameter(command, "@SortCode", SqlDbType.VarChar, model.SortCode);
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.VarChar, model.Amount);
                    DataAccess.AddInParameter(command, "@Remarks", SqlDbType.VarChar, model.Remarks);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, model.AgentId);
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
        public string AgentBulkPaymentCreditCard(AgentBulkPaymentCreditCard model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentAgentCreditCard_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, model.AgentId);
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
        public string AgentBulkPaymentDebitCard(AgentBulkPaymentDebitCard model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentAgentDebitCard_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, model.AgentId);
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
        public string AgentBulkPaymentBankDeposit(AgentBulkPaymentBankDeposit model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentAgentBankDeposit_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, model.AgentId);
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
        public bool BulkPaymentAgentCashVoucherInvoicePaymentInvoiceLog(BulkPaymentAgentCashInvPaymentInvLog model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentAgentCashVoucherInvoicePaymentInvoiceLog_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, model.InvoiceId);
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, model.UserID);
                    DataAccess.AddInParameter(command, "@GeneralLedgerId", SqlDbType.Int, model.GeneralLedgerId);
                    DataAccess.AddInParameter(command, "@AgentLedgerId", SqlDbType.Int, model.AgentLedgerId);
                    DataAccess.AddInParameter(command, "@BankAccountLedgerId", SqlDbType.Int, model.BankAccountLedgerId);
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, model.AgentId);
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
        public IPChequeDetail AgentBulkPaymentFloatingCheque(int Cheid)
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
        public string AgentBulkPaymentChequePayment(AgentBulkPaymentChequePayment model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentAgentChequePayment_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, model.AgentId);
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
        public List<InvoicePaymentHistory> GetInvoicePaymentHistoryByAgent(int AgentId)
        {
            List<InvoicePaymentHistory> lstObj = new List<InvoicePaymentHistory>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoicePaymentHistoryByAgentId_SP");
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, AgentId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoicePaymentHistory inv = new InvoicePaymentHistory();
                            inv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            inv.PaymentDate = reader["PaymentDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["PaymentDate"]);
                            inv.UserName = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["UserName"]);
                            inv.Notes = reader["Notes"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            inv.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            inv.AgentId = reader["AgentId"] is DBNull ? 0 : Convert.ToInt32(reader["AgentId"]);
                            inv.AgentName = reader["AgentName"] is DBNull ? string.Empty : Convert.ToString(reader["AgentName"]);
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
        public List<InvoicePaymentHistory> GetInvoicePaymentHistoryByInvoice(int InvoiceId)
        {
            List<InvoicePaymentHistory> lstObj = new List<InvoicePaymentHistory>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoicePaymentHistoryByInvoiceId_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoicePaymentHistory inv = new InvoicePaymentHistory();
                            inv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            inv.PaymentDate = reader["PaymentDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["PaymentDate"]);
                            inv.UserName = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["UserName"]);
                            inv.Notes = reader["Notes"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            inv.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            inv.AgentId = reader["AgentId"] is DBNull ? 0 : Convert.ToInt32(reader["AgentId"]);
                            inv.AgentName = reader["AgentName"] is DBNull ? string.Empty : Convert.ToString(reader["AgentName"]);
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
        public List<InvoicePaymentHistory> GetInvoicePaymentHistoryByDateRange(string sDate, string eDate)
        {
            List<InvoicePaymentHistory> lstObj = new List<InvoicePaymentHistory>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoicePaymentHistoryByDateRange_SP");
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoicePaymentHistory inv = new InvoicePaymentHistory();
                            inv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            inv.PaymentDate = reader["PaymentDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["PaymentDate"]);
                            inv.UserName = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["UserName"]);
                            inv.Notes = reader["Notes"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            inv.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            inv.AgentId = reader["AgentId"] is DBNull ? 0 : Convert.ToInt32(reader["AgentId"]);
                            inv.AgentName = reader["AgentName"] is DBNull ? string.Empty : Convert.ToString(reader["AgentName"]);
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
        public List<InvoicePaymentHistory> GetInvoicePaymentHistoryByDateAgent(int AgentId, string sDate, string eDate)
        {
            List<InvoicePaymentHistory> lstObj = new List<InvoicePaymentHistory>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoicePaymentHistoryByDateAgent_SP");
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, AgentId);
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoicePaymentHistory inv = new InvoicePaymentHistory();
                            inv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            inv.PaymentDate = reader["PaymentDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["PaymentDate"]);
                            inv.UserName = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["UserName"]);
                            inv.Notes = reader["Notes"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            inv.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            inv.AgentId = reader["AgentId"] is DBNull ? 0 : Convert.ToInt32(reader["AgentId"]);
                            inv.AgentName = reader["AgentName"] is DBNull ? string.Empty : Convert.ToString(reader["AgentName"]);
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
        public List<AgentLedgerViewModel> AgentsLedgerList(int id)
        {
            List<AgentLedgerViewModel> lstObj = new List<AgentLedgerViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetAgentsLedgerList_SP");
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, id);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            AgentLedgerViewModel agObj = new AgentLedgerViewModel();
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
        public List<AgentLedgerViewModel> AgentsLedgerListSearchByDateRange(int AgentId, string sDate, string eDate)
        {
            List<AgentLedgerViewModel> lstObj = new List<AgentLedgerViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetAgentsLedgerListSearchByDateRange_SP");
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, AgentId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            AgentLedgerViewModel agObj = new AgentLedgerViewModel();
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
        public List<AgentLedgerViewModel> AgentsLedgerListSearchByDateRangeLedgerHead(int AgentId, string sDate, string eDate, int LedgerHead)
        {
            List<AgentLedgerViewModel> lstObj = new List<AgentLedgerViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetAgentsLedgerListSearchByDateRangeLedgerHead_SP");
                    DataAccess.AddInParameter(command, "@sDate", SqlDbType.VarChar, sDate);
                    DataAccess.AddInParameter(command, "@eDate", SqlDbType.VarChar, eDate);
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, AgentId);
                    DataAccess.AddInParameter(command, "@LedgerHead", SqlDbType.Int, LedgerHead);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            AgentLedgerViewModel agObj = new AgentLedgerViewModel();
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
        public List<InvoiceListViewModel> GetOutstandingInvoiceListByAgentId(int AgentId)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetOutstandingInvoiceListByAgentId_SP");
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, AgentId);
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
    }
}