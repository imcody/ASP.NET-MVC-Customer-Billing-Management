using Billing.ViewModel;
using Billing.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Billing.DAL.Parameters;

namespace Billing.DAL
{
    public partial class SearchDA
    {
        //Initially named as SearchDA, but this Data Access SP execution class will mostly interact with Invoice Controller for data manipulation
        public List<InvoiceListViewModel> SearchInvoicesByInvoicesID(int? InvoiceID)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchInvoicesByInvoicesID_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, (int)InvoiceID);
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
        public List<InvoiceListViewModel> SearchDraftByDraftID(int? InvoiceID)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchDraftByDraftID_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, (int)InvoiceID);
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
        public List<InvoiceListViewModel> SearchInvoicesByPaxLastNames(string nameString)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchInvoicesByPaxLastNames_SP");
                    DataAccess.AddInParameter(command, "@LastName", SqlDbType.VarChar, nameString);
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
        public List<InvoiceListViewModel> SearchDraftByPaxLastNames(string nameString)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchDraftByPaxLastNames_SP");
                    DataAccess.AddInParameter(command, "@LastName", SqlDbType.VarChar, nameString);
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
        public List<InvoiceListViewModel> SearchInvoicesByPNR(string PNR)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchInvoicesByPNR_SP");
                    DataAccess.AddInParameter(command, "@Pnr", SqlDbType.VarChar, PNR);
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
        public List<InvoiceListViewModel> SearchInvoicesByTicketNo(string TicketNo)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchInvoicesByTicketNo_SP");
                    DataAccess.AddInParameter(command, "@TicketNo", SqlDbType.VarChar, TicketNo);
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
        public List<InvoiceListViewModel> SearchInvoicesByDate(string SysDate)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchInvoicesByDate_SP");
                    DataAccess.AddInParameter(command, "@SysDate", SqlDbType.VarChar, SysDate);
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
        public List<InvoiceListViewModel> SearchDraftByDate(string SysDate)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchDraftByDate_SP");
                    DataAccess.AddInParameter(command, "@SysDate", SqlDbType.VarChar, SysDate);
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
        public List<InvoiceListViewModel> SearchInvoicesByDateRange(string FromDate, string ToDate)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchInvoicesByDateRange_SP");
                    DataAccess.AddInParameter(command, "@FromDate", SqlDbType.VarChar, FromDate);
                    DataAccess.AddInParameter(command, "@ToDate", SqlDbType.VarChar, ToDate);
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
        public List<InvoiceListViewModel> SearchDraftByDateRange(string FromDate, string ToDate)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchDraftByDateRange_SP");
                    DataAccess.AddInParameter(command, "@FromDate", SqlDbType.VarChar, FromDate);
                    DataAccess.AddInParameter(command, "@ToDate", SqlDbType.VarChar, ToDate);
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
        public List<InvoiceListViewModel> GetLatestInvoiceList()
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetLatestInvoiceList_SP");
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
        public List<InvoiceListViewModel> GetDraftInvoiceList()
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetDraftInvoiceList_SP");
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
        public List<InvoiceListViewModel> SearchInvoicesByDateRangeForHome(string FromDate, string ToDate, string UserId)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchInvoicesByDateRangeAndUserId_SP");
                    DataAccess.AddInParameter(command, "@FromDate", SqlDbType.VarChar, FromDate);
                    DataAccess.AddInParameter(command, "@ToDate", SqlDbType.VarChar, ToDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, UserId);
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
        public List<InvoiceListViewModel> SearchDraftByDateRangeForHome(string FromDate, string ToDate, string UserId)
        {
            List<InvoiceListViewModel> invList = new List<InvoiceListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "SearchDraftByDateRangeAndUserId_SP");
                    DataAccess.AddInParameter(command, "@FromDate", SqlDbType.VarChar, FromDate);
                    DataAccess.AddInParameter(command, "@ToDate", SqlDbType.VarChar, ToDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, UserId);
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
        public bool InsertNewRemarksToInvoice(InvoiceLog model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertNewRemarksToInvoice_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, (int)model.InvoiceId);
                    DataAccess.AddInParameter(command, "@ApplicationUserId", SqlDbType.VarChar, model.ApplicationUserId);
                    DataAccess.AddInParameter(command, "@Remarks", SqlDbType.VarChar, model.Remarks);
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
        public bool UpdateInvoiceFareInfo(InvoiceName model, string UserID)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "UpdateInvoiceFareInfo_SP");
                    DataAccess.AddInParameter(command, "@TableId", SqlDbType.Int, model.Id);
                    DataAccess.AddInParameter(command, "@Name", SqlDbType.VarChar, model.Name);
                    DataAccess.AddInParameter(command, "@TicketNo", SqlDbType.VarChar, model.TicketNo);
                    DataAccess.AddInParameter(command, "@PassengerType", SqlDbType.Int, (int)model.PassengerTypes);
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, model.Amount);
                    DataAccess.AddInParameter(command, "@TicketTax", SqlDbType.Float, model.TicketTax);
                    DataAccess.AddInParameter(command, "@CNetFare", SqlDbType.Float, model.CNetFare);
                    DataAccess.AddInParameter(command, "@VNetFare", SqlDbType.Float, model.VNetFare);
                    DataAccess.AddInParameter(command, "@VendorCharge", SqlDbType.Float, model.VendorCharge);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, model.InvoiceId);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, UserID);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateInvoiceSegments(SegmentUpdateParams model, string UserID)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "UpdateInvoiceSegments_SP");
                    DataAccess.AddInParameter(command, "@TableId", SqlDbType.Int, model.TableId);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, model.InvoiceId);
                    DataAccess.AddInParameter(command, "@AirlinesCode", SqlDbType.VarChar, model.AirlinesCode);
                    DataAccess.AddInParameter(command, "@ArrivalTime", SqlDbType.VarChar, model.ArrivalTime);
                    DataAccess.AddInParameter(command, "@DepartureDate", SqlDbType.VarChar, model.DepartureDate);
                    DataAccess.AddInParameter(command, "@DepartureFrom", SqlDbType.VarChar, model.DepartureFrom);
                    DataAccess.AddInParameter(command, "@DepartureTime", SqlDbType.VarChar, model.DepartureTime);
                    DataAccess.AddInParameter(command, "@DepartureTo", SqlDbType.VarChar, model.DepartureTo);
                    DataAccess.AddInParameter(command, "@FlightDate", SqlDbType.VarChar, model.FlightDate);
                    DataAccess.AddInParameter(command, "@FlightNo", SqlDbType.VarChar, model.FlightNo);
                    DataAccess.AddInParameter(command, "@SegmentClass", SqlDbType.VarChar, model.SegmentClass);
                    DataAccess.AddInParameter(command, "@SegmentStatus", SqlDbType.VarChar, model.SegmentStatus);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, UserID);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool FinalizeFareInfoUpdate(FinalizeFareInfoUpdate model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "FinalizeFareInfoUpdate_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, model.InvoiceId);
                    DataAccess.AddInParameter(command, "@ApplicationUserId", SqlDbType.VarChar, model.ApplicationUserId);
                    DataAccess.AddInParameter(command, "@Remarks", SqlDbType.VarChar, model.Remarks);
                    DataAccess.AddInParameter(command, "@Total", SqlDbType.Float, model.Total);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool FinalizeTicketSegmentUpdate(FinalizeFareInfoUpdate model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "FinalizeInvoiceSegmentUpdate_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, model.InvoiceId);
                    DataAccess.AddInParameter(command, "@ApplicationUserId", SqlDbType.VarChar, model.ApplicationUserId);
                    DataAccess.AddInParameter(command, "@Remarks", SqlDbType.VarChar, model.Remarks);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertNewBankAccount(InsertNewBankAccount model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertNewBankAccount_SP");
                    DataAccess.AddInParameter(command, "@BankNames", SqlDbType.Int, model.BankNames);
                    DataAccess.AddInParameter(command, "@AccountNo", SqlDbType.VarChar, model.AccountNo);
                    DataAccess.AddInParameter(command, "@AccountNames", SqlDbType.VarChar, model.AccountNames);
                    DataAccess.AddInParameter(command, "@Balance", SqlDbType.Float, model.Balance);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.UserId);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InvoicePaymentCashVoucher(CashTransaction model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertInvoicePaymentCashVoucher_SP");
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
        public bool AddCreditCardPaymentDetails(CCardDetail Obj)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "AddCreditCardPaymentDetails_SP");
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, Obj.Notes);
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, Obj.Amount);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, Obj.InvoiceId);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, Obj.BankAccountId);
                    DataAccess.AddInParameter(command, "@CardNo", SqlDbType.VarChar, Obj.CardNo);
                    DataAccess.AddInParameter(command, "@CardHolder", SqlDbType.VarChar, Obj.CardHolder);
                    DataAccess.AddInParameter(command, "@ExtraAmount", SqlDbType.VarChar, Obj.ExtraAmount);
                    DataAccess.AddInParameter(command, "@BankDate", SqlDbType.VarChar, Obj.BankDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, Obj.UserId);
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
        public bool AddDebitCardPaymentDetails(DCardDetail Obj)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "AddDebitCardPaymentDetails_SP");
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, Obj.Notes);
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, Obj.Amount);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, Obj.InvoiceId);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, Obj.BankAccountId);
                    DataAccess.AddInParameter(command, "@CardNo", SqlDbType.VarChar, Obj.CardNo);
                    DataAccess.AddInParameter(command, "@CardHolder", SqlDbType.VarChar, Obj.CardHolder);
                    DataAccess.AddInParameter(command, "@ExtraAmount", SqlDbType.VarChar, Obj.ExtraAmount);
                    DataAccess.AddInParameter(command, "@BankDate", SqlDbType.VarChar, Obj.BankDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, Obj.UserId);
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
        public bool AddBankDepositPaymentDetails(BankPaymentDetail Obj)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "AddBankDepositPaymentDetails_SP");
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, Obj.Notes);
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, Obj.Amount);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, Obj.InvoiceId);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, Obj.BankAccountId);
                    DataAccess.AddInParameter(command, "@BankDate", SqlDbType.VarChar, Obj.BankDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, Obj.UserId);
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
        public bool UpdateInvoicePaymentStatus(int InvoiceId, int PaymentStatus, string UserID)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);
                    DataAccess.CreateStoredprocedure(command, "UpdateInvoicePaymentStatus_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Float, InvoiceId);
                    DataAccess.AddInParameter(command, "@PaymentStatus", SqlDbType.Int, PaymentStatus);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, UserID);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateDraftToInvoice(int InvoiceId, int InvoiceStatus, string UserID)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);
                    DataAccess.CreateStoredprocedure(command, "UpdateDraftToInvoice_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Float, InvoiceId);
                    DataAccess.AddInParameter(command, "@InvoiceStatus", SqlDbType.Int, InvoiceStatus);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, UserID);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateInvoicePaymentStatusVendor(int InvoiceId, int PaymentStatus, string UserID)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);
                    DataAccess.CreateStoredprocedure(command, "UpdateInvoicePaymentStatusVendor_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Float, InvoiceId);
                    DataAccess.AddInParameter(command, "@PaymentStatus", SqlDbType.Int, PaymentStatus);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, UserID);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<InvoiceDetailsPaxViewModel> GetInvoicePaxInfo(int InvoiceId)
        {
            List<InvoiceDetailsPaxViewModel> paxList = new List<InvoiceDetailsPaxViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoicePaxInfoForInvoiceDetails_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoiceDetailsPaxViewModel passenger = new InvoiceDetailsPaxViewModel();
                            passenger.BookingDate = reader["BookingDate"] is DBNull ? string.Empty : (Convert.ToDateTime(reader["BookingDate"])).ToString("yyyy-MM-dd");
                            passenger.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            passenger.TableId = reader["TableId"] is DBNull ? 0 : Convert.ToInt32(reader["TableId"]);
                            passenger.InvoiceId = InvoiceId;
                            passenger.PaxType = reader["PaxType"] is DBNull ? string.Empty : ((PassengerType)reader["PaxType"]).ToString();
                            passenger.Name = reader["Name"] is DBNull ? string.Empty : (String)reader["Name"];
                            passenger.TicketNo = reader["TicketNo"] is DBNull ? string.Empty : (String)reader["TicketNo"];
                            passenger.Fare = reader["Fare"] is DBNull ? 0 : Convert.ToDouble(reader["Fare"]);
                            passenger.Tax = reader["Tax"] is DBNull ? 0 : Convert.ToDouble(reader["Tax"]);
                            passenger.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            paxList.Add(passenger);
                        }
                    }
                    reader.Close();
                    return paxList;
                }
            }
            catch (Exception ex)
            {
                return paxList;
            }
        }
        public List<InvoiceDetailsSegmentViewModel> GetInvoiceSegmentInfo(int InvoiceId)
        {
            List<InvoiceDetailsSegmentViewModel> segList = new List<InvoiceDetailsSegmentViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoiceSegmentInfoForInvoiceDetails_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoiceDetailsSegmentViewModel segment = new InvoiceDetailsSegmentViewModel();
                            segment.AirlinesCode = reader["AirlinesCode"] is DBNull ? string.Empty : Convert.ToString(reader["AirlinesCode"]);
                            segment.FlightNo = reader["FlightNo"] is DBNull ? string.Empty : Convert.ToString(reader["FlightNo"]);
                            segment.SegmentClass = reader["SegmentClass"] is DBNull ? string.Empty : Convert.ToString(reader["SegmentClass"]);
                            segment.DepartureDate = reader["DepartureDate"] is DBNull ? string.Empty : Convert.ToString(reader["DepartureDate"]);
                            segment.DepartureFrom = reader["DepartureFrom"] is DBNull ? string.Empty : Convert.ToString(reader["DepartureFrom"]);
                            segment.DepartureTo = reader["DepartureTo"] is DBNull ? string.Empty : Convert.ToString(reader["DepartureTo"]);
                            segment.DepartureTime = reader["DepartureTime"] is DBNull ? string.Empty : Convert.ToString(reader["DepartureTime"]);
                            segment.ArrivalTime = reader["ArrivalTime"] is DBNull ? string.Empty : Convert.ToString(reader["ArrivalTime"]);
                            segment.SegmentStatus = reader["SegmentStatus"] is DBNull ? string.Empty : Convert.ToString(reader["SegmentStatus"]);
                            segment.FlightDate = reader["FlightDate"] is DBNull ? string.Empty : Convert.ToString(reader["FlightDate"]);
                            segment.InvoiceId = InvoiceId;
                            segment.TableId = reader["TableId"] is DBNull ? 0 : Convert.ToInt32(reader["TableId"]);
                            segList.Add(segment);
                        }
                    }
                    reader.Close();
                    return segList;
                }
            }
            catch (Exception ex)
            {
                return segList;
            }
        }
        public List<InvoiceDetailsLogViewModel> GetInvoiceLogInfo(int InvoiceId)
        {
            List<InvoiceDetailsLogViewModel> logList = new List<InvoiceDetailsLogViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoiceLogInfoForInvoiceDetails_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            InvoiceDetailsLogViewModel log = new InvoiceDetailsLogViewModel();
                            log.Date = reader["Date"] is DBNull ? string.Empty : Convert.ToString(reader["Date"]);
                            log.Remarks = reader["Remarks"] is DBNull ? string.Empty : Convert.ToString(reader["Remarks"]);
                            log.Time = reader["Time"] is DBNull ? string.Empty : Convert.ToString(reader["Time"]);
                            log.UserID = reader["UserID"] is DBNull ? string.Empty : Convert.ToString(reader["UserID"]);
                            logList.Add(log);
                        }
                    }
                    reader.Close();
                    return logList;
                }
            }
            catch (Exception ex)
            {
                return logList;
            }
        }
        public bool UpdateInvoiceCurrentUser(string userID, int InvoiceID)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "UpdateInvoiceExistingUser_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceID);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, userID);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateInvoiceTicketIssueDate(string IssueDate, int InvoiceID, int TableID)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "UpdateInvoiceTicketIssueDate_SP");
                    DataAccess.AddInParameter(command, "@TableId", SqlDbType.Int, TableID);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceID);
                    DataAccess.AddInParameter(command, "@IssueDate", SqlDbType.VarChar, IssueDate);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public double GetInvoicePaidAmount(int InvoiceID)
        {
            double PaidAmount = 0;
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoicePaidAmountByAgentCustomer_SP");
                    DataAccess.AddInParameter(command, "@InvoiceID", SqlDbType.Int, InvoiceID);
                    var returnParameter = command.Parameters.Add("@PaidAmount", SqlDbType.Float);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    command.ExecuteNonQuery();
                    PaidAmount = Convert.ToDouble(returnParameter.Value);

                    return PaidAmount;
                }
            }
            catch (Exception ex)
            {
                return PaidAmount;
            }
        }
        public double GetTicketAmountOfAgentCustomer(int TicketId)
        {
            double TicketAmount = 0;
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetTicketAmountByAgentCustomer_SP");
                    DataAccess.AddInParameter(command, "@TicketId", SqlDbType.Int, TicketId);
                    var returnParameter = command.Parameters.Add("@TicketAmount", SqlDbType.Float);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    command.ExecuteNonQuery();
                    TicketAmount = Convert.ToDouble(returnParameter.Value);

                    return TicketAmount;
                }
            }
            catch (Exception ex)
            {
                return TicketAmount;
            }
        }
        public bool DeleteTicketFromInvoice(int TicketId, int InvoiceID, string UserID)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "DeleteTicketFromInvoice_SP");
                    DataAccess.AddInParameter(command, "@TicketId", SqlDbType.Int, TicketId);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceID);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, UserID); 
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public int GetInvoiceAgentCustomerPaymentStatus(int InvoiceId)
        {
            int PaymentStaus = 0;
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetInvoiceAgentCustomerPaymentStatus_SP");
                    DataAccess.AddInParameter(command, "@InvoiceID", SqlDbType.Int, InvoiceId);
                    var returnParameter = command.Parameters.Add("@PaymentStaus", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    command.ExecuteNonQuery();
                    PaymentStaus = Convert.ToInt32(returnParameter.Value);

                    return PaymentStaus;
                }
            }
            catch (Exception ex)
            {
                return PaymentStaus;
            }
        }

    }
}