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
        readonly IAccessory Accessory;
        public UserService()
        {
            Accessory = new AccessoryService();
        }
        public List<UserManagementModel> GetUsers()
        {
            try
            {
                List<UserManagementModel> users = new List<UserManagementModel>();
                SqlCommand cmd = new SqlCommand("select * from [User] order by Name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        UserManagementModel u = new UserManagementModel()
                        {
                            name = dr["Name"].ToString(),
                            department = dr["Department"].ToString(),
                            role = dr["Role"].ToString(),
                            groups = dr["Groups"].ToString()
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
                string department = "";
                SqlCommand cmd1 = new SqlCommand("select * from [User] where Name = '" + name+"'", ConnectSQL.OpenConnect());
                SqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.HasRows)
                {
                    b = true;                  
                }
                if (!b)
                {
                    department = Accessory.getAllUser().Where(w => w.name == name).Select(s => s.department).FirstOrDefault();
                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO [User](Name,Department) VALUES (@Name,@Department)", ConnectSQL.OpenConnect()))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = ConnectSQL.OpenConnect();
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Department", department);
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

        public string update(string name,string role,string group)
        {
            try
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(@"UPDATE [User] SET Role='" + role + "',Groups='" + group + "'" +
                                                                      " WHERE Name='" + name + "'");
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
