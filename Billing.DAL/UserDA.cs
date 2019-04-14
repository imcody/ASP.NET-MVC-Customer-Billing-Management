using Billing.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Billing.DAL
{
    public partial class UserDA
    {
        public List<AirlinesListViewModel> getAirlinesList()
        {
            List<AirlinesListViewModel> airList = new List<AirlinesListViewModel>();
            try
            {
                using (SqlConnection connection = DataAccess.CreateConnection())
                {
                    SqlCommand command = DataAccess.CreateCommand(connection);

                    DataAccess.CreateStoredprocedure(command, "GetAirlinesList_SP");
                    SqlDataReader reader = DataAccess.ExecuteReader(command);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            AirlinesListViewModel air = new AirlinesListViewModel();
                            air.Id = reader["Id"] is DBNull ? 0 : Convert.ToInt32(reader["Id"]);
                            air.SL = reader["SL"] is DBNull ? 0 : Convert.ToInt32(reader["SL"]);
                            air.Name = reader["Name"] is DBNull ? string.Empty : Convert.ToString(reader["Name"]);
                            air.Code = reader["Code"] is DBNull ? string.Empty : (String)reader["Code"];
                            airList.Add(air);
                        }
                    }
                    reader.Close();
                    return airList;
                }
            }
            catch (Exception ex)
            {
                return airList;
            }
        }
    }
}