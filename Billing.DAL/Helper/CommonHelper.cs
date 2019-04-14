using Billing.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Billing.DAL.Helpers
{
    public static class CommonHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static string GetUserRole(string Id)
        {
            if (Id == null)
            {
                throw new ArgumentNullException("identity");
            }
            ApplicationUser us = db.Users.Find(Id);
            if (us != null)
            {
                string m = Enum.GetName(typeof(UserRole), us.UserRoles);
                return m;// = Enum.GetName(typeof(UserRole), us.UserRoles);
            }
            return null;
        }
        public static string getShortenedName(string PersonName)
        {
            if (string.IsNullOrEmpty(PersonName))
            {
                return string.Empty;
            }
            string[] splitted = PersonName.Split(' ');
            return splitted[0][0] + " " + splitted[splitted.Length - 1][0];
        }
        public static string getFlightDate(string GDSDate)
        {
            string date = GDSDate.Substring(0, 2);
            string month = CommonHelper.getMonthNumber(GDSDate.Substring(2, 3));
            string currentMonth = DateTime.Now.ToString("MM");
            string Year = CommonHelper.getYear(currentMonth, month, date);
            return Year + "-" + month + "-" + date;
        }
        public static string getMonthNumber(string monthName)
        {
            if(monthName == "JAN")
            {
                return "01";
            }
            else if(monthName == "FEB")
            {
                return "02";
            }
            else if (monthName == "MAR")
            {
                return "03";
            }
            else if (monthName == "APR")
            {
                return "04";
            }
            else if (monthName == "MAY")
            {
                return "05";
            }
            else if (monthName == "JUN")
            {
                return "06";
            }
            else if (monthName == "JUL")
            {
                return "07";
            }
            else if (monthName == "AUG")
            {
                return "08";
            }
            else if (monthName == "SEP")
            {
                return "09";
            }
            else if (monthName == "OCT")
            {
                return "10";
            }
            else if (monthName == "NOV")
            {
                return "11";
            }
            else if (monthName == "DEC")
            {
                return "12";
            }
            else
            {
                return "00";
            }
        }
        public static string getYear(string currentMonth, string FlightMonth, string date)
        {
            string Year = DateTime.Now.ToString("yyyy");
            if(Convert.ToInt32(FlightMonth) >= Convert.ToInt32(currentMonth))
            {
                return Year;
            }
            else
            {
                Year = DateTime.Now.AddYears(1).ToString("yyyy");
                return Year;
            }
        }
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .Name;
        }
        public static bool DetectSegmentLine(string formattedLine, List<Airlines> lstAirline)
        {
            try
            {
                string[] splitted = formattedLine.Split(' ');
                if(splitted.Length > 3)
                {
                    Int32 firstDigit = Convert.ToInt32(splitted[0]);
                    if (firstDigit == 0)
                    {
                        return false;
                    }
                    if (splitted[2].Length == 2)
                    {
                        Airlines airline = lstAirline.Where(a => a.Code == splitted[2]).FirstOrDefault();
                        return airline != null ? true : false;
                    }
                    else if(splitted[2].Length == 6)
                    {
                        splitted[2] = splitted[2].Substring(0, 2);
                        Airlines airline = lstAirline.Where(a => a.Code == splitted[2]).FirstOrDefault();
                        return airline != null ? true : false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return false;
        }
        public static InvoiceSegment ParseInvoiceSegment(string GDSLine, int InvoiceID)
        {
            InvoiceSegment iSegment = new InvoiceSegment();
            try
            {
                string line = GDSLine.Remove(0, 3).Trim();
                line = line.Replace("  ", " ");
                string[] departedSegments = line.Split(' ');
                if(departedSegments[0].Length == 6)
                {
                    departedSegments[0] = departedSegments[0].ToString().Insert(2, " ");
                    line = String.Join(" ", departedSegments);
                    departedSegments = line.Split(' ');
                }
                iSegment.InvoiceId = InvoiceID;
                iSegment.AirlinesCode = departedSegments[0];
                iSegment.FlightNo = departedSegments[1];
                iSegment.SegmentClass = departedSegments[2];
                iSegment.DepartureDate = departedSegments[3];
                if (line.Contains('*'))
                {
                    iSegment.DepartureFrom = departedSegments[4].Split('*')[1].Substring(0, 3);
                    iSegment.DepartureTo = departedSegments[4].Split('*')[1].Substring(3, 3);
                    iSegment.DepartureTime = departedSegments[6];
                    iSegment.ArrivalTime = departedSegments[7];
                    iSegment.SegmentStatus = departedSegments[5];
                    iSegment.FlightDate = CommonHelper.getFlightDate(departedSegments[3]);
                }
                else
                {
                    iSegment.DepartureFrom = departedSegments[5].Substring(0, 3);
                    iSegment.DepartureTo = departedSegments[5].Substring(3, 3);
                    iSegment.DepartureTime = departedSegments[7];
                    iSegment.ArrivalTime = departedSegments[8];
                    iSegment.SegmentStatus = departedSegments[6];
                    iSegment.FlightDate = CommonHelper.getFlightDate(departedSegments[3]);
                }
                return iSegment;
            }
            catch(Exception ex)
            {
                return iSegment;
            }
        }
        public static Agent DetectProfileIdForInvoice(ProfileType pType, int? AgentId, int? AgentId2, int? CusType, string User, string Email, string Mobile, string Name, string Postcode)
        {
            int ProfileId = 0;
            Agent agent = new Agent();
            if (pType == ProfileType.Agent)
            {
                if (AgentId == null)
                {
                    return agent;
                }
                else
                {
                    ProfileId = (int)AgentId;
                    agent = db.Agents.Find(ProfileId);
                    return agent;
                }
            }
            else if (pType == ProfileType.Customer)
            {
                if (CusType == null)
                {
                    return agent;
                }
                else
                {
                    if (CusType == 1)
                    {

                        agent.Address = string.Empty;
                        agent.ApplicationUserId = User;
                        agent.Atol = string.Empty;
                        agent.Balance = 0;
                        agent.CreditLimit = 0;
                        agent.Email = Email;
                        agent.FaxNo = string.Empty;
                        agent.JoiningDate = DateTime.Now;
                        agent.Mobile = Mobile;
                        agent.Name = Name;
                        agent.Postcode = Postcode;
                        agent.ProfileType = ProfileType.Customer;
                        agent.Remarks = "Created during Invoice Creation";
                        agent.Telephone = string.Empty;

                        db.Agents.Add(agent);
                        db.SaveChanges();

                        ProfileId = agent.Id;
                        agent = db.Agents.Find(ProfileId);
                        return agent;
                    }
                    else if (CusType == 2)
                    {
                        ProfileId = (int)AgentId2;
                        agent = db.Agents.Find(ProfileId);
                        return agent;
                    }
                    else
                    {
                        return agent;
                    }
                }
            }
            else
            {
                return agent;
            }
        }
        public static InvoiceName ParseInvoiceName(List<string> formattedGDSLines, List<InvoiceSegment> segmentsList, int InvoiceId, int pline)
        {
            InvoiceName iName = new InvoiceName();
            if (!formattedGDSLines[pline].Contains("*"))
            {
                string[] paxSingleLine = null;
                string paxLine = string.Empty;
                paxSingleLine = formattedGDSLines[pline].Replace("\t", "=").Split('=');
                if (paxSingleLine.Length == 1)
                {
                    paxSingleLine = formattedGDSLines[pline].Replace("   ", "=").Split('=');
                }

                #region Two passengers in one line...
                if (paxSingleLine.Length == 2)
                {
                    InvoiceName _invName = new InvoiceName();
                    _invName.InvoiceId = InvoiceId;
                    _invName.BookingDate = DateTime.Now;
                    _invName.Name = paxSingleLine[0].Remove(0, 2).ToString() + "=" + paxSingleLine[1].Remove(0, 2).ToString();
                    _invName.Status = 1;
                    if (_invName.Name.Contains("(CHD/"))
                    {
                        _invName.PassengerTypes = PassengerType.CHD;
                    }
                    else if (_invName.Name.Contains("(INF/"))
                    {
                        _invName.PassengerTypes = PassengerType.INF;
                    }
                    else 
                    {
                        _invName.PassengerTypes = PassengerType.ADT;
                    }
                    iName = _invName;
                }
                #endregion
                #region Three passengers in one line
                else if (paxSingleLine.Length == 3)
                {
                    InvoiceName _invName = new InvoiceName();
                    _invName.InvoiceId = InvoiceId;
                    _invName.BookingDate = DateTime.Now;
                    _invName.Name = paxSingleLine[0].Remove(0, 2).ToString() + "=" + paxSingleLine[1].Remove(0, 2).ToString() + "=" + paxSingleLine[2].Remove(0, 2).ToString();
                    _invName.Status = 1;
                    if (_invName.Name.Contains("(CHD/"))
                    {
                        _invName.PassengerTypes = PassengerType.CHD;
                    }
                    else if (_invName.Name.Contains("(INF/"))
                    {
                        _invName.PassengerTypes = PassengerType.INF;
                    }
                    else
                    {
                        _invName.PassengerTypes = PassengerType.ADT;
                    }
                    iName = _invName;
                } 
                #endregion
                #region One passenger in one line...
                else
                {
                    InvoiceName _invName = new InvoiceName();
                    _invName.InvoiceId = InvoiceId;
                    _invName.BookingDate = DateTime.Now;
                    _invName.Name = paxSingleLine[0].Remove(0, 2).ToString();
                    _invName.Status = 1;
                    if (_invName.Name.Contains("(CHD/"))
                    {
                        _invName.PassengerTypes = PassengerType.CHD;
                    }
                    else if (_invName.Name.Contains("(INF/"))
                    {
                        _invName.PassengerTypes = PassengerType.INF;
                    }
                    else
                    {
                        _invName.PassengerTypes = PassengerType.ADT;
                    }
                    iName = _invName;
                } 
                #endregion
            }
            return iName;
        }
    }
}