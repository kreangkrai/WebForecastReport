using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Service;

namespace WebForecastReport.Services.MPR
{
    public class EngUserService : IEngUser
    {
        public List<EngUserModel> GetUsers()
        {
            List<EngUserModel> users = new List<EngUserModel>();
            try
            {
                string string_command = string.Format($@"SELECT * FROM Sale_User");
                SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.Open_db_gps_Connect());
                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Connection.Open();
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        EngUserModel user = new EngUserModel()
                        {
                            user_id = dr["Login"] != DBNull.Value ? dr["Login"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            department = dr["Department2"] != DBNull.Value ? dr["Department2"].ToString() : "",
                        };
                        users.Add(user);
                    }
                    dr.Close();
                }
            }
            finally
            {
                if (ConnectSQL.con_db_gps.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.Close_db_gps_Connect();
                }
            }
            return users;
        }
    }
}
