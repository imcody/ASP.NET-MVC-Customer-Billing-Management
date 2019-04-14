using Billing.DAL.Parameters;
using Billing.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Billing.DAL
{
    public partial class BankDA
    {
        public List<BankAccount> GetBankAccountList()
        {
            List<BankAccount> bankList = new List<BankAccount>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetBankAccountList_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            BankAccount account = new BankAccount();
                            account.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            account.BankNames = reader["BankNames"] is DBNull ? 0 : (BankName)(reader["BankNames"]);
                            account.AccountNo = reader["AccountNo"] is DBNull ? string.Empty : Convert.ToString(reader["AccountNo"]);
                            account.AccountNames = reader["AccountNames"] is DBNull ? string.Empty : Convert.ToString(reader["AccountNames"]);
                            account.Balance = reader["Balance"] is DBNull ? 0 : Convert.ToDouble(reader["Balance"]);
                            bankList.Add(account);
                        }
                    }
                    reader.Close();
                    return bankList;
                }
            }
            catch (Exception ex)
            {
                return bankList;
            }
        }
        public List<BankAccountLedgerHead> GetBankLedgerHeadList()
        {
            List<BankAccountLedgerHead> balList = new List<BankAccountLedgerHead>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetBankLedgerHeadList_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            BankAccountLedgerHead bal = new BankAccountLedgerHead();
                            bal.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            bal.LedgerHead = reader["LedgerHead"] is DBNull ? string.Empty : Convert.ToString(reader["LedgerHead"]);
                            bal.LedgerTypes = reader["LedgerTypes"] is DBNull ? 0 : (LedgerType)(reader["LedgerTypes"]);
                            bal.Editable = reader["Editable"] is DBNull ? false : Convert.ToBoolean(reader["Editable"]);
                            bal.Status = reader["Status"] is DBNull ? false : Convert.ToBoolean(reader["Status"]);
                            balList.Add(bal);
                        }
                    }
                    reader.Close();
                    return balList;
                }
            }
            catch (Exception ex)
            {
                return balList;
            }
        }
        public bool InsertNewBankLedgerHead(BankAccountLedgerHead model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertNewBankLedgerHead_SP");
                    DataAccess.AddInParameter(command, "@LedgerHead", SqlDbType.VarChar, model.LedgerHead);
                    DataAccess.AddInParameter(command, "@LedgerType", SqlDbType.Int, (int)model.LedgerTypes);
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
        public bool InsertBankDepositCashVoucher(BankDepositCashVoucher model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBankDepositCashVoucher_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserID);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, model.BankAccountId);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@BankLedgerHeadId", SqlDbType.Int, model.BankLedgerHeadId);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertBankDepositChequeVoucher(BankDepositChequeVoucher model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBankDepositChequeVoucher_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, model.BankAccountId);
                    DataAccess.AddInParameter(command, "@BankOfChequeId", SqlDbType.Int, model.BankOfChequeId);
                    DataAccess.AddInParameter(command, "@ChequeNo", SqlDbType.VarChar, model.ChequeNo);
                    DataAccess.AddInParameter(command, "@AccountNo", SqlDbType.VarChar, model.AccountNo);
                    DataAccess.AddInParameter(command, "@SortCode", SqlDbType.VarChar, model.SortCode);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, model.UserID);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@Status", SqlDbType.Int, model.ChequeStauts);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertBankDepositCreditCardVoucher(BankDepositCreditCardVoucher model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBankDepositCreditCardVoucher_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, model.BankAccountId);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, model.UserID);
                    DataAccess.AddInParameter(command, "@LedgerHeadId", SqlDbType.Int, model.LedgerHeadId);
                    DataAccess.AddInParameter(command, "@CardNo", SqlDbType.VarChar, model.CardNo);
                    DataAccess.AddInParameter(command, "@CardHolder", SqlDbType.VarChar, model.CardHolder);
                    DataAccess.AddInParameter(command, "@ExtraAmount", SqlDbType.VarChar, model.ExtraAmount);
                    DataAccess.AddInParameter(command, "@BankDate", SqlDbType.VarChar, model.BankDate);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertBankDepositDebitCardVoucher(BankDepositDebitCardVoucher model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBankDepositDebitCardVoucher_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, model.BankAccountId);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, model.UserID);
                    DataAccess.AddInParameter(command, "@LedgerHeadId", SqlDbType.Int, model.LedgerHeadId);
                    DataAccess.AddInParameter(command, "@CardNo", SqlDbType.VarChar, model.CardNo);
                    DataAccess.AddInParameter(command, "@CardHolder", SqlDbType.VarChar, model.CardHolder);
                    DataAccess.AddInParameter(command, "@ExtraAmount", SqlDbType.VarChar, model.ExtraAmount);
                    DataAccess.AddInParameter(command, "@BankDate", SqlDbType.VarChar, model.BankDate);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertBankDepositDirectDepositVoucher(BankDepositDirectDepositVoucher model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBankDepositDirectDepositVoucher_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserID);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, model.BankAccountId);
                    DataAccess.AddInParameter(command, "@LedgerDate", SqlDbType.VarChar, model.LedgerDate);
                    DataAccess.AddInParameter(command, "@BankLedgerHeadId", SqlDbType.Int, model.BankLedgerHeadId);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertBankWithdrawalChequeCoucher(BankWithdrawalChequeVoucher model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBankWithdrawalChequeCoucher_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserID);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, model.BankAccountId);
                    DataAccess.AddInParameter(command, "@BankLedgerHeadId", SqlDbType.Int, model.BankLedgerHeadId);
                    DataAccess.AddInParameter(command, "@BankId", SqlDbType.Int, model.BankId);
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
        public bool InsertBankWithdrawalForPettyCash(BankWithdrawalChequeVoucher model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBankWithdrawalForPettyCash_SP");
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, model.Notes);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserID);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, model.BankAccountId);
                    DataAccess.AddInParameter(command, "@BankLedgerHeadId", SqlDbType.Int, model.BankLedgerHeadId);
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
        public bool UpdateBankAccountInformation(Int32 AccountId, Int32 BankId, String AccountNo, String AccountName)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "UpdateBankAccountInformation_SP");
                    DataAccess.AddInParameter(command, "@AccountId", SqlDbType.Int, AccountId);
                    DataAccess.AddInParameter(command, "@BankId", SqlDbType.Int, BankId);
                    DataAccess.AddInParameter(command, "@AccountNo", SqlDbType.Int, AccountNo);
                    DataAccess.AddInParameter(command, "@AccountName", SqlDbType.Int, AccountName);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}