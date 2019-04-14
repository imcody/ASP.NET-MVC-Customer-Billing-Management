using Billing.DAL.Parameters;
using Billing.Entities;
using Billing.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Billing.DAL
{
    public partial class AccountingDA
    {
        public double GetCurrentCashInHand()
        {
            double Balance = 0;
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetCurrentCashInHand_SP");
                    //SqlDataReader reader = DataAccess.ExecuteReader(command);
                    var returnParameter = command.Parameters.Add("@NewSeqVal", SqlDbType.Float);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    //connection.Open();
                    command.ExecuteNonQuery();
                    Balance = Convert.ToDouble(returnParameter.Value);

                    return Balance;
                }
            }
            catch (Exception ex)
            {
                return Balance;
            }
        }
        public List<FloatChequeViewModel> GetFloatingChequeList(int chequeStatus)
        {
            List<FloatChequeViewModel> Obj = new List<FloatChequeViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetFloatingChequeList_SP");
                    DataAccess.AddInParameter(command, "@Status", SqlDbType.Int, chequeStatus);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            FloatChequeViewModel cheques = new FloatChequeViewModel();
                            cheques.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            cheques.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            cheques.IssueDate = reader["SysCreateDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["SysCreateDate"]);
                            cheques.AccountNo = reader["AccountNo"] is DBNull ? string.Empty : (String)reader["AccountNo"];
                            cheques.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            cheques.BankName = reader["BankNames"] is DBNull ? 0 : (BankName)reader["BankNames"];
                            cheques.CheuqueNo = reader["CheuqueNo"] is DBNull ? string.Empty : Convert.ToString(reader["CheuqueNo"]);
                            cheques.Remarks = reader["Remarks"] is DBNull ? string.Empty : Convert.ToString(reader["Remarks"]);
                            cheques.SortCode = reader["SortCode"] is DBNull ? string.Empty : Convert.ToString(reader["SortCode"]);
                            cheques.ProfileType = reader["ProfileType"] is DBNull ? 0 : Convert.ToInt32(reader["ProfileType"]);
                            Obj.Add(cheques);
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
        public List<FloatChequeViewModel> GetBulkFloatingChequeList(int chequeStatus)
        {
            List<FloatChequeViewModel> Obj = new List<FloatChequeViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetBulkFloatingChequeList_SP");
                    DataAccess.AddInParameter(command, "@Status", SqlDbType.Int, chequeStatus);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            FloatChequeViewModel cheques = new FloatChequeViewModel();
                            cheques.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            cheques.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            cheques.IssueDate = reader["SysCreateDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["SysCreateDate"]);
                            cheques.AccountNo = reader["AccountNo"] is DBNull ? string.Empty : (String)reader["AccountNo"];
                            cheques.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            cheques.BankName = reader["BankNames"] is DBNull ? 0 : (BankName)reader["BankNames"];
                            cheques.CheuqueNo = reader["CheuqueNo"] is DBNull ? string.Empty : Convert.ToString(reader["CheuqueNo"]);
                            cheques.Remarks = reader["Remarks"] is DBNull ? string.Empty : Convert.ToString(reader["Remarks"]);
                            cheques.SortCode = reader["SortCode"] is DBNull ? string.Empty : Convert.ToString(reader["SortCode"]);
                            cheques.ProfileType = reader["ProfileType"] is DBNull ? 0 : Convert.ToInt32(reader["ProfileType"]);
                            Obj.Add(cheques);
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
        public bool InsertGeneralVoucherPosting(GeneralVoucherPosting model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertGeneralVoucherPosting_SP");
                    DataAccess.AddInParameter(command, "@LedgerHeadId", SqlDbType.Int, model.LedgerHeadId);
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserID);
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
        public List<GeneralLedgerListViewModel> GetGeneralLedgerList(int StatementType)
        {
            List<GeneralLedgerListViewModel> ledgerList = new List<GeneralLedgerListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetGeneralLedgerList_SP");
                    DataAccess.AddInParameter(command, "@StatementType", SqlDbType.Int, StatementType);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            GeneralLedgerListViewModel Obj = new GeneralLedgerListViewModel();
                            Obj.LedgerDate = reader["LedgerDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["LedgerDate"]);
                            Obj.UserName = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["UserName"]);
                            Obj.LedgerHead = reader["LedgerHead"] is DBNull ? string.Empty : Convert.ToString(reader["LedgerHead"]);
                            Obj.PaymentMethod = reader["PaymentMethod"] is DBNull ? string.Empty : ((PaymentMethod)reader["PaymentMethod"]).ToString();
                            Obj.Notes = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            Obj.LedgerType = reader["LedgerType"] is DBNull ? string.Empty : Convert.ToString(reader["LedgerType"]);
                            Obj.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            ledgerList.Add(Obj);
                        }
                    }
                    reader.Close();
                    return ledgerList;
                }
            }
            catch (Exception ex)
            {
                return ledgerList;
            }
        }
        public List<GeneralLedgerListViewModel> FilterGeneralLedgerList(string Query)
        {
            List<GeneralLedgerListViewModel> ledgerList = new List<GeneralLedgerListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = new SqlCommand(Query, connection);
                    //connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            GeneralLedgerListViewModel Obj = new GeneralLedgerListViewModel();
                            Obj.LedgerDate = reader["LedgerDate"] is DBNull ? "0000-00-00" : Convert.ToString(reader["LedgerDate"]);
                            Obj.UserName = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["UserName"]);
                            Obj.LedgerHead = reader["LedgerHead"] is DBNull ? string.Empty : Convert.ToString(reader["LedgerHead"]);
                            Obj.PaymentMethod = reader["PaymentMethod"] is DBNull ? string.Empty : ((PaymentMethod)reader["PaymentMethod"]).ToString();
                            Obj.Notes = reader["UserName"] is DBNull ? string.Empty : Convert.ToString(reader["Notes"]);
                            Obj.LedgerType = reader["LedgerType"] is DBNull ? string.Empty : Convert.ToString(reader["LedgerType"]);
                            Obj.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            ledgerList.Add(Obj);
                        }
                    }
                    reader.Close();
                    return ledgerList;
                }
            }
            catch (Exception ex)
            {
                return ledgerList;
            }
        }
        public List<CompanyInfo> GetCompanyInformation()
        {
            List<CompanyInfo> lstObj = new List<CompanyInfo>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetCompanyInformation_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            CompanyInfo Obj = new CompanyInfo();
                            Obj.Address = reader["Address"] is DBNull ? string.Empty : Convert.ToString(reader["Address"]);
                            Obj.AgentSafi = reader["AgentSafi"] is DBNull ? 0 : Convert.ToDouble(reader["AgentSafi"]);
                            Obj.Apc = reader["Apc"] is DBNull ? 0 : Convert.ToDouble(reader["Apc"]);
                            Obj.Atol = reader["Atol"] is DBNull ? 0 : Convert.ToDouble(reader["Atol"]);
                            Obj.CreditCardCharge = reader["CreditCardCharge"] is DBNull ? 0 : Convert.ToDouble(reader["CreditCardCharge"]);
                            Obj.CustomerSafi = reader["CustomerSafi"] is DBNull ? 0 : Convert.ToDouble(reader["CustomerSafi"]);
                            Obj.DebitCardCharge = reader["DebitCardCharge"] is DBNull ? 0 : Convert.ToDouble(reader["DebitCardCharge"]);
                            Obj.Email = reader["Email"] is DBNull ? string.Empty : Convert.ToString(reader["Email"]);
                            Obj.FaxNo = reader["FaxNo"] is DBNull ? string.Empty : Convert.ToString(reader["FaxNo"]);
                            Obj.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            Obj.Name = reader["Name"] is DBNull ? string.Empty : Convert.ToString(reader["Name"]);
                            Obj.Telephone = reader["Telephone"] is DBNull ? string.Empty : Convert.ToString(reader["Telephone"]);
                            Obj.WebAddress = reader["WebAddress"] is DBNull ? string.Empty : Convert.ToString(reader["WebAddress"]);
                            lstObj.Add(Obj);
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
    }
}