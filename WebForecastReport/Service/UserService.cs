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
                            fullname = dr["Fullname"].ToString(),
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

        public string insert(string fullname)
        {
            try
            {
                bool b = false;
                string department = "";
                string name = "";
                SqlCommand cmd1 = new SqlCommand("select * from [User] where Fullname = '" + fullname+"'", ConnectSQL.OpenConnect());
                SqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.HasRows)
                {
                    b = true;                  
                }
                if (!b)
                {
                    department = Accessory.getAllUser().Where(w => w.fullname == fullname.ToLower()).Select(s => s.department).FirstOrDefault();
                    name = Accessory.getAllUser().Where(w => w.fullname == fullname.ToLower()).Select(s => s.name).FirstOrDefault();
                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO [User](Fullname,Name,Department) VALUES (@Fullname,@Name,@Department)", ConnectSQL.OpenConnect()))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = ConnectSQL.OpenConnect();
                        cmd.Parameters.AddWithValue("@Fullname", fullname);
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

        public string InsertUser()
        {
            List<UserModel> users = new List<UserModel>();
            SqlCommand cmd = new SqlCommand("select (FirstName+' '+LastName) as name,UPPER(SUBSTRING(Email,12,3)) as department from [Directory].[dbo].[Cardholder] where LastName <>'' order by Name", ConnectSQL.OpenUserConnect());
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    UserModel u = new UserModel()
                    {
                        name = dr["name"].ToString(),
                        department = dr["department"].ToString()
                    };
                    users.Add(u);
                }
                dr.Close();
            }
            ConnectSQL.CloseUserConnect();

            List<UserModel> users_new = new List<UserModel>();
            SqlCommand cmd_new = new SqlCommand("select Name as name,Department2 as department from Sale_User order by name", ConnectSQL.OpenConnectSaleUser());
            SqlDataReader dr_new = cmd_new.ExecuteReader();
            if (dr_new.HasRows)
            {
                while (dr_new.Read())
                {
                    UserModel u = new UserModel()
                    {
                        name = dr_new["name"].ToString(),
                        department = dr_new["department"].ToString()
                    };
                    users_new.Add(u);
                }
                dr_new.Close();
            }
            ConnectSQL.CloseConnectSaleUser();

            var insert = users.Where(w => !users_new.Any(a => a.name == w.name)).ToList();

            if (insert.Count > 0)
            {
                using (SqlCommand com = new SqlCommand(
                            "INSERT INTO Sale_User(Login,Name,Department,Levels,Department2,PermissionApprove) Values(@Login,@Name,@Department,@Levels,@Department2,@PermissionApprove)", ConnectSQL.OpenConnectSaleUser()))
                {
                    com.CommandType = CommandType.Text;
                    com.Connection = ConnectSQL.OpenConnectSaleUser();
                    com.Parameters.Add("@Login", SqlDbType.NVarChar);
                    com.Parameters.Add("@Name", SqlDbType.NVarChar);
                    com.Parameters.Add("@Department", SqlDbType.NVarChar);
                    com.Parameters.Add("@Levels", SqlDbType.NVarChar);
                    com.Parameters.Add("@Department2", SqlDbType.NVarChar);
                    com.Parameters.Add("@PermissionApprove", SqlDbType.NVarChar);

                    for (int i = 0; i < insert.Count; i++)
                    {
                        string department = "";
                        if (insert[i].department == "RBO")
                        {
                            department = "RBO";
                        }
                        else if (insert[i].department == "KBO")
                        {
                            department = "KBO";
                        }
                        else
                        {
                            department = "HQ";
                        }
                        string name = insert[i].name.Split(" ")[0] + "." + insert[i].name.Split(" ")[1].Substring(0, 1);
                        com.Parameters[0].Value = name;
                        com.Parameters[1].Value = insert[i].name;
                        com.Parameters[2].Value = department;
                        com.Parameters[3].Value = "1";
                        com.Parameters[4].Value = insert[i].department;
                        com.Parameters[5].Value = "Operation";
                        com.ExecuteNonQuery();
                    }
                }
            }
            return "Insert Success";
        }

        public string update(string fullname,string name,string role,string group)
        {
            try
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(@"UPDATE [User] SET Role='" + role + "',Groups='" + group + "'" +
                                                                      " WHERE Fullname='" + fullname + "'");
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
