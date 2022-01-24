using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;

namespace WebForecastReport.Service
{
    public class UserService : IUser
    {
        public List<UserManagementModel> GetUsers()
        {
            try
            {
                List<UserManagementModel> users = new List<UserManagementModel>();
                SqlCommand cmd = new SqlCommand("select * from [User] order by Department", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        UserManagementModel u = new UserManagementModel()
                        {
                            name = dr["Name"].ToString(),
                            department = dr["Department"].ToString(),
                            role = dr["Role"].ToString()
                        };
                        users.Add(u);
                    }
                    dr.Close();
                }
                return users;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public string insert(string name)
        {
            try
            {
                bool b = false;
                SqlCommand cmd1 = new SqlCommand("select * from [User] where Name = '" + name+"'", ConnectSQL.OpenConnect());
                SqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.HasRows)
                {
                    b = true;
                }
                if (!b)
                {
                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO [User](Name) VALUES (@Name)", ConnectSQL.OpenConnect()))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = ConnectSQL.OpenConnect();
                        cmd.Parameters.AddWithValue("@Name", name);

                        cmd.ExecuteNonQuery();

                        
                    }
                }
                return "Insert Success";
            }
            catch
            {
                return "Insert Failed";
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public string update(string name,string role)
        {
            try
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(@"UPDATE [User] SET Role='" + role + "'" +
                                                                      "WHERE Name='" + name + "'");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = ConnectSQL.OpenConnect();
                reader = cmd.ExecuteReader();
                reader.Close();

                return "Update Success";
            }
            catch
            {
                return "Update Failed";
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }
    }
}
