using Billing.DAL.Parameters;
using Billing.Entities;
using Billing.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Billing.DAL
{
    public class OtherInvoiceDA
    {
        public List<OtherInvocieTypeList> GetOtheInvoiceTypeList()
        {
            List<OtherInvocieTypeList> invTypeList = new List<OtherInvocieTypeList>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetOtheInvoiceType_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            OtherInvocieTypeList invType = new OtherInvocieTypeList();
                            invType.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            invType.CreatedOn = reader["CreatedOn"] is DBNull ? string.Empty : (Convert.ToString(reader["CreatedOn"]));
                            invType.InvoiceType = reader["InvoiceType"] is DBNull ? string.Empty : (String)reader["InvoiceType"];
                            invType.UserId = reader["UserId"] is DBNull ? string.Empty : (String)reader["UserId"];
                            invTypeList.Add(invType);
                        }
                    }
                    reader.Close();
                    return invTypeList;
                }
            }
            catch (Exception ex)
            {
                return invTypeList;
            }
        }
        public OtherInvoiceType GetOtherInvoiceByTypeId(int TypeID)
        {
            OtherInvoiceType invType = new OtherInvoiceType();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetOtherInvoiceByTypeId_SP");
                    DataAccess.AddInParameter(command, "@TypeID", SqlDbType.Int, TypeID);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            invType.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            invType.CreatedOn = reader["CreatedOn"] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedOn"]);
                            invType.InvoiceType = reader["InvoiceType"] is DBNull ? string.Empty : (String)reader["InvoiceType"];
                            invType.ApplicationUserId = reader["ApplicationUserId"] is DBNull ? string.Empty : (String)reader["ApplicationUserId"];
                        }
                    }
                    reader.Close();
                    return invType;
                }
            }
            catch (Exception ex)
            {
                return invType;
            }
        }
        public bool UpdateOtherInvoiceType(OtherInvoiceType model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "UpdateOtherInvoiceType_SP");
                    DataAccess.AddInParameter(command, "@TableId", SqlDbType.Int, model.Id);
                    DataAccess.AddInParameter(command, "@InvoiceType", SqlDbType.VarChar, model.InvoiceType);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertOtherInvoiceType(OtherInvoiceType model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertOtherInvoiceType_SP");
                    DataAccess.AddInParameter(command, "@InvoiceType", SqlDbType.VarChar, model.InvoiceType);
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, model.ApplicationUserId);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InsertNewOtherInvoice(OtherInvoice model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertNewOtherInvoice_SP");
                    DataAccess.AddInParameter(command, "AgentId", SqlDbType.Int, model.AgentId);
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, model.VendorId);
                    DataAccess.AddInParameter(command, "@OtherInvoiceTypeId", SqlDbType.Int, model.OtherInvoiceTypeId);
                    DataAccess.AddInParameter(command, "@Reference", SqlDbType.VarChar, model.Reference);
                    DataAccess.AddInParameter(command, "@ExpectedPayDate", SqlDbType.VarChar, model.ExpectedPayDate);
                    DataAccess.AddInParameter(command, "@VendorInvNo", SqlDbType.VarChar, model.VendorInvNo);
                    DataAccess.AddInParameter(command, "@Details", SqlDbType.VarChar, model.Details);
                    DataAccess.AddInParameter(command, "@CustomerAgentAmount", SqlDbType.Float, model.CustomerAgentAmount);
                    DataAccess.AddInParameter(command, "@VendorAmount", SqlDbType.Float, model.VendorAmount);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, model.ApplicationUserId);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<OtherInvoiceList> GetLatestOtherInvoiceList()
        {
            List<OtherInvoiceList> otherInvList = new List<OtherInvoiceList>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetLatestOtherInvoiceList_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            OtherInvoiceList otherInv = new OtherInvoiceList();
                            otherInv.ACAmount = reader["ACAmount"] is DBNull ? 0 : Convert.ToDouble(reader["ACAmount"]);
                            otherInv.Agent = reader["Agent"] is DBNull ? string.Empty : (String)reader["Agent"];
                            otherInv.CreatedOn = reader["CreatedOn"] is DBNull ? string.Empty : (Convert.ToString(reader["CreatedOn"]));
                            otherInv.Due = reader["Due"] is DBNull ? 0 : Convert.ToDouble(reader["Due"]);
                            otherInv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            otherInv.InvoiceType = reader["InvoiceType"] is DBNull ? string.Empty : Convert.ToString(reader["InvoiceType"]);
                            otherInv.Paid = reader["Paid"] is DBNull ? 0 : Convert.ToDouble(reader["Paid"]);
                            otherInv.VAmount = reader["VAmount"] is DBNull ? 0 : Convert.ToDouble(reader["VAmount"]);
                            otherInv.VDue = reader["VDue"] is DBNull ? 0 : Convert.ToDouble(reader["VDue"]);
                            otherInv.Vendor = reader["Vendor"] is DBNull ? string.Empty : (String)reader["Vendor"];
                            otherInv.VPaid = reader["VPaid"] is DBNull ? 0 : Convert.ToDouble(reader["VPaid"]);
                            otherInv.User = reader["UserName"] is DBNull ? string.Empty : (String)reader["UserName"];
                            otherInvList.Add(otherInv);
                        }
                    }
                    reader.Close();
                    return otherInvList;
                }
            }
            catch (Exception ex)
            {
                return otherInvList;
            }
        }
        public List<OtherInvoiceList> GetFilteredOtherInvoiceList(DateTime FromDate, DateTime ToDate, int? AgentId, string UsersId, int? TypesId)
        {
            List<OtherInvoiceList> otherInvList = new List<OtherInvoiceList>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetFilteredOtherInvoiceList_SP");
                    DataAccess.AddInParameter(command, "@FromDate", SqlDbType.VarChar, FromDate.ToString("MM/dd/yyyy"));
                    DataAccess.AddInParameter(command, "@ToDate", SqlDbType.VarChar, ToDate.ToString("MM/dd/yyyy"));
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, AgentId);
                    DataAccess.AddInParameter(command, "@UsersId", SqlDbType.VarChar, UsersId);
                    DataAccess.AddInParameter(command, "@TypesId", SqlDbType.Int, TypesId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            OtherInvoiceList otherInv = new OtherInvoiceList();
                            otherInv.ACAmount = reader["ACAmount"] is DBNull ? 0 : Convert.ToDouble(reader["ACAmount"]);
                            otherInv.Agent = reader["Agent"] is DBNull ? string.Empty : (String)reader["Agent"];
                            otherInv.CreatedOn = reader["CreatedOn"] is DBNull ? string.Empty : (Convert.ToString(reader["CreatedOn"]));
                            otherInv.Due = reader["Due"] is DBNull ? 0 : Convert.ToDouble(reader["Due"]);
                            otherInv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            otherInv.InvoiceType = reader["InvoiceType"] is DBNull ? string.Empty : Convert.ToString(reader["InvoiceType"]);
                            otherInv.Paid = reader["Paid"] is DBNull ? 0 : Convert.ToDouble(reader["Paid"]);
                            otherInv.VAmount = reader["VAmount"] is DBNull ? 0 : Convert.ToDouble(reader["VAmount"]);
                            otherInv.VDue = reader["VDue"] is DBNull ? 0 : Convert.ToDouble(reader["VDue"]);
                            otherInv.Vendor = reader["Vendor"] is DBNull ? string.Empty : (String)reader["Vendor"];
                            otherInv.VPaid = reader["VPaid"] is DBNull ? 0 : Convert.ToDouble(reader["VPaid"]);
                            otherInv.User = reader["UserName"] is DBNull ? string.Empty : (String)reader["UserName"];
                            otherInvList.Add(otherInv);
                        }
                    }
                    reader.Close();
                    return otherInvList;
                }
            }
            catch (Exception ex)
            {
                return otherInvList;
            }
        }
        public List<OtherInvoiceList> GetVendorOutstandingOtherInvoiceList(int VendorId)
        {
            List<OtherInvoiceList> otherInvList = new List<OtherInvoiceList>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetVendorOutstandingOtherInvoiceList_SP");
                    DataAccess.AddInParameter(command, "@VendorId", SqlDbType.Int, VendorId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            OtherInvoiceList otherInv = new OtherInvoiceList();
                            otherInv.ACAmount = reader["ACAmount"] is DBNull ? 0 : Convert.ToDouble(reader["ACAmount"]);
                            otherInv.Agent = reader["Agent"] is DBNull ? string.Empty : (String)reader["Agent"];
                            otherInv.CreatedOn = reader["CreatedOn"] is DBNull ? string.Empty : (Convert.ToString(reader["CreatedOn"]));
                            otherInv.Due = reader["Due"] is DBNull ? 0 : Convert.ToDouble(reader["Due"]);
                            otherInv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            otherInv.InvoiceType = reader["InvoiceType"] is DBNull ? string.Empty : Convert.ToString(reader["InvoiceType"]);
                            otherInv.Paid = reader["Paid"] is DBNull ? 0 : Convert.ToDouble(reader["Paid"]);
                            otherInv.VAmount = reader["VAmount"] is DBNull ? 0 : Convert.ToDouble(reader["VAmount"]);
                            otherInv.VDue = reader["VDue"] is DBNull ? 0 : Convert.ToDouble(reader["VDue"]);
                            otherInv.Vendor = reader["Vendor"] is DBNull ? string.Empty : (String)reader["Vendor"];
                            otherInv.VPaid = reader["VPaid"] is DBNull ? 0 : Convert.ToDouble(reader["VPaid"]);
                            otherInv.User = reader["UserName"] is DBNull ? string.Empty : (String)reader["UserName"];
                            otherInvList.Add(otherInv);
                        }
                    }
                    reader.Close();
                    return otherInvList;
                }
            }
            catch (Exception ex)
            {
                return otherInvList;
            }
        }
        public List<OtherInvoiceList> GetAgentOutstandingOtherInvoiceList(int AgentId)
        {
            List<OtherInvoiceList> otherInvList = new List<OtherInvoiceList>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetAgentOutstandingOtherInvoiceList_SP");
                    DataAccess.AddInParameter(command, "@AgentId", SqlDbType.Int, AgentId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            OtherInvoiceList otherInv = new OtherInvoiceList();
                            otherInv.ACAmount = reader["ACAmount"] is DBNull ? 0 : Convert.ToDouble(reader["ACAmount"]);
                            otherInv.Agent = reader["Agent"] is DBNull ? string.Empty : (String)reader["Agent"];
                            otherInv.CreatedOn = reader["CreatedOn"] is DBNull ? string.Empty : (Convert.ToString(reader["CreatedOn"]));
                            otherInv.Due = reader["Due"] is DBNull ? 0 : Convert.ToDouble(reader["Due"]);
                            otherInv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            otherInv.InvoiceType = reader["InvoiceType"] is DBNull ? string.Empty : Convert.ToString(reader["InvoiceType"]);
                            otherInv.Paid = reader["Paid"] is DBNull ? 0 : Convert.ToDouble(reader["Paid"]);
                            otherInv.VAmount = reader["VAmount"] is DBNull ? 0 : Convert.ToDouble(reader["VAmount"]);
                            otherInv.VDue = reader["VDue"] is DBNull ? 0 : Convert.ToDouble(reader["VDue"]);
                            otherInv.Vendor = reader["Vendor"] is DBNull ? string.Empty : (String)reader["Vendor"];
                            otherInv.VPaid = reader["VPaid"] is DBNull ? 0 : Convert.ToDouble(reader["VPaid"]);
                            otherInv.User = reader["UserName"] is DBNull ? string.Empty : (String)reader["UserName"];
                            otherInvList.Add(otherInv);
                        }
                    }
                    reader.Close();
                    return otherInvList;
                }
            }
            catch (Exception ex)
            {
                return otherInvList;
            }
        }
        public OtherInvoiceDetailsViewModel GetOtherInvoiceDetailsByInvoiceId(int InvoiceId)
        {
            OtherInvoiceDetailsViewModel othrInv = new OtherInvoiceDetailsViewModel();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetOtherInvoiceDetailsByInvoiceId_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceId);
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            othrInv.ACAmount = reader["ACAmount"] is DBNull ? 0 : Convert.ToDouble(reader["ACAmount"]);
                            othrInv.AgentAddress = reader["AgentAddress"] is DBNull ? string.Empty : Convert.ToString(reader["AgentAddress"]);
                            othrInv.AgentEmail = reader["AgentEmail"] is DBNull ? string.Empty : Convert.ToString(reader["AgentEmail"]);
                            othrInv.AgentName = reader["AgentName"] is DBNull ? string.Empty : Convert.ToString(reader["AgentName"]);
                            othrInv.AgentPhone = reader["AgentPhone"] is DBNull ? string.Empty : Convert.ToString(reader["AgentPhone"]);
                            othrInv.CreationDate = reader["InvoiceDate"] is DBNull ? string.Empty : Convert.ToString(reader["InvoiceDate"]);
                            othrInv.Details = reader["Details"] is DBNull ? string.Empty : Convert.ToString(reader["Details"]);
                            othrInv.Due = reader["Due"] is DBNull ? 0 : Convert.ToDouble(reader["Due"]);
                            othrInv.EDP = reader["EDP"] is DBNull ? string.Empty : Convert.ToString(reader["EDP"]);
                            othrInv.InvoiceId = reader["InvoiceId"] is DBNull ? 0 : Convert.ToInt32(reader["InvoiceId"]);
                            othrInv.InvoiceType = reader["InvoiceType"] is DBNull ? string.Empty : Convert.ToString(reader["InvoiceType"]);
                            othrInv.InvType = reader["InvType"] is DBNull ? 0 : Convert.ToInt32(reader["InvType"]); 
                            othrInv.Paid = reader["Paid"] is DBNull ? 0 : Convert.ToDouble(reader["Paid"]);
                            othrInv.PersonName = reader["PersonName"] is DBNull ? string.Empty : Convert.ToString(reader["PersonName"]);
                            othrInv.Reference = reader["Reference"] is DBNull ? string.Empty : Convert.ToString(reader["Reference"]);
                            othrInv.VAmount = reader["VAmount"] is DBNull ? 0 : Convert.ToInt32(reader["VAmount"]);
                            othrInv.VendorAddress = reader["VendorAddress"] is DBNull ? string.Empty : Convert.ToString(reader["VendorAddress"]);
                            othrInv.VendorEmail = reader["VendorEmail"] is DBNull ? string.Empty : Convert.ToString(reader["VendorEmail"]);
                            othrInv.VendorName = reader["VendorName"] is DBNull ? string.Empty : Convert.ToString(reader["VendorName"]);
                            othrInv.VendorPhone = reader["VendorPhone"] is DBNull ? string.Empty : Convert.ToString(reader["VendorPhone"]);
                            othrInv.VInvNo = reader["VInvNo"] is DBNull ? string.Empty : Convert.ToString(reader["VInvNo"]);
                        }
                    }
                    reader.Close();
                    return othrInv;
                }
            }
            catch (Exception ex)
            {
                return othrInv;
            }
        }
        public List<InvoiceDetailsLogViewModel> GetOtherInvoiceLogInfo(int InvoiceId)
        {
            List<InvoiceDetailsLogViewModel> logList = new List<InvoiceDetailsLogViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetOtherInvoiceLogInfoForInvoiceDetails_SP");
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
        public List<OtherInvoiceDetailsPaymentViewModel> GetOtherInvoicePaymentList(int InvoiceId)
        {
            List<OtherInvoiceDetailsPaymentViewModel> InvPayment = new List<OtherInvoiceDetailsPaymentViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetOtherInvoicePaymentList_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            OtherInvoiceDetailsPaymentViewModel _InvPayment = new OtherInvoiceDetailsPaymentViewModel();
                            _InvPayment.Amount = reader["Amount"] is DBNull ? 0 : Convert.ToDouble(reader["Amount"]);
                            _InvPayment.PaymentDate = reader["PaymentDate"] is DBNull ? string.Empty : Convert.ToString(reader["PaymentDate"]);
                            _InvPayment.PersonName = reader["PersonName"] is DBNull ? string.Empty : Convert.ToString(reader["PersonName"]);
                            _InvPayment.Remarks = reader["Remarks"] is DBNull ? string.Empty : Convert.ToString(reader["Remarks"]);
                            InvPayment.Add(_InvPayment);
                        }
                    }
                    reader.Close();
                    return InvPayment;
                }
            }
            catch (Exception ex)
            {
                return InvPayment;
            }
        }
        public bool InsertNewRemarksToOtherInvoice(OtherInvoiceLog model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertNewRemarksToOtherInvoice_SP");
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, (int)model.OtherInvoiceId);
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
        public bool UpdateOtherInvoiceAgent(string UserID, int InvoiceId, int NewAgent)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "UpdateOtherInvoiceAgent_SP");
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, UserID);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceId);
                    DataAccess.AddInParameter(command, "@NewAgentId", SqlDbType.Int, NewAgent);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateOtherInvoiceVendor(string UserID, int InvoiceId, int NewVendor)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "UpdateOtherInvoiceVendor_SP");
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, UserID);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceId);
                    DataAccess.AddInParameter(command, "@NewVendorId", SqlDbType.Int, NewVendor);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateOtherInvoicePaymentStatus(int InvoiceId, int PaymentStatus, string UserID)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);
                    DataAccess.CreateStoredprocedure(command, "UpdateOtherInvoicePaymentStatus_SP");
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
        public bool UpdateOtherInvoicePaymentStatusVendor(int InvoiceId, int PaymentStatus, string UserID)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);
                    DataAccess.CreateStoredprocedure(command, "UpdateOtherInvoicePaymentStatusVendor_SP");
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
        public bool UpdateOtherInvoiceBasicInfo(string UserID, int InvoiceId, int OtherInvoiceTypeId, string ExpectedPayDate, string Reference, string VendorInvNo, string Details, double CustomerAgentAmount)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);
                    DataAccess.CreateStoredprocedure(command, "UpdateOtherInvoiceBasicInfo_SP");
                    DataAccess.AddInParameter(command, "@UserID", SqlDbType.VarChar, UserID);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, InvoiceId);
                    DataAccess.AddInParameter(command, "@OtherInvoiceTypeId", SqlDbType.Int, OtherInvoiceTypeId);
                    DataAccess.AddInParameter(command, "@ExpectedPayDate", SqlDbType.VarChar, ExpectedPayDate);
                    DataAccess.AddInParameter(command, "@Reference", SqlDbType.VarChar, Reference);
                    DataAccess.AddInParameter(command, "@VendorInvNo", SqlDbType.VarChar, VendorInvNo);
                    DataAccess.AddInParameter(command, "@Details", SqlDbType.VarChar, Details);
                    DataAccess.AddInParameter(command, "@CustomerAgentAmount", SqlDbType.Float, CustomerAgentAmount);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpdateOtherInvoiceCurrentUser(string userID, int InvoiceID)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "UpdateOtherInvoiceExistingUser_SP");
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
        public bool OtherInvoicePaymentCashVoucher(CashTransaction model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertOtherInvoicePaymentCashVoucher_SP");
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
        public bool AddCreditCardPaymentDetailsOtherInvoice(CCardDetail Obj)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "AddCreditCardPaymentDetailsOtherInvoice_SP");
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
        public bool AddDebitCardPaymentDetailsOtherInvoice(DCardDetail Obj)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "AddDebitCardPaymentDetailsOtherInvoice_SP");
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
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AddBankDepositPaymentDetailsOtherInvoice(BankPaymentDetail Obj)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "AddBankDepositPaymentDetailsOtherInvoice_SP");
                    DataAccess.AddInParameter(command, "@Notes", SqlDbType.VarChar, Obj.Notes);
                    DataAccess.AddInParameter(command, "@Amount", SqlDbType.Float, Obj.Amount);
                    DataAccess.AddInParameter(command, "@InvoiceId", SqlDbType.Int, Obj.InvoiceId);
                    DataAccess.AddInParameter(command, "@BankAccountId", SqlDbType.Int, Obj.BankAccountId);
                    DataAccess.AddInParameter(command, "@BankDate", SqlDbType.VarChar, Obj.BankDate);
                    DataAccess.AddInParameter(command, "@UserId", SqlDbType.VarChar, Obj.UserId);
                    status = DataAccess.ExecuteNonQuery(command);
                    return status < 0 ? false : true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool InvoicePaymentInvoiceLogVendorPaymentOtherInvoice(BulkPaymentVendorCashInvPaymentInvLog model)
        {
            try
            {
                int status = 0;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertInvoicePaymentInvoiceLogBulkPaymentOtherInvoice_SP");
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
        public string BulkPaymentCashVoucherOtherInvoice(VendorBulkPaymentCashVoucher model)
        {
            try
            {
                string status = string.Empty;
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "InsertBulkPaymentVendorCashVoucherOtherInvoice_SP");
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
        public string VendorBulkPaymentCreditCardOtherInvoice(VendorBulkPaymentCreditCard model)
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
    }
}