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
        public bool CheckAllowEditable(string user_id)
        {
            bool allow = false;
            try
            {
                string string_command = string.Format($@"SELECT allow_edit FROM EngineerUsers Where user_id = '{user_id}'");
                SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect());
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
                        allow = dr["allow_edit"] != DBNull.Value ? Convert.ToBoolean(dr["allow_edit"]) : false;
                    }
                    dr.Close();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return allow;
        }

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
                            allow_edit = false,
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
            return users.GroupBy(g => g.user_id).Select(s => s.FirstOrDefault()).ToList();
        }

        public List<EngUserModel> GetEngineerUsers()
        {
            List<EngUserModel> engineers = new List<EngUserModel>();
            try
            {
                string string_command = string.Format($@"SELECT * FROM EngineerUsers");
                SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect());
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
                        EngUserModel eng = new EngUserModel()
                        {
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["user_name"] != DBNull.Value ? dr["user_name"].ToString() : "",
                            department = dr["department"] != DBNull.Value ? dr["department"].ToString() : "",
                            allow_edit = dr["allow_edit"] != DBNull.Value ? Convert.ToBoolean(dr["allow_edit"].ToString()) : false,
                        };
                        engineers.Add(eng);
                    }
                    dr.Close();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return engineers;
        }

        public string CreateEngineerUser(EngUserModel engineer)
        {
            try
            {
                string string_command = string.Format($@"
                    INSERT INTO 
                        EngineerUsers(user_id, user_name, department, allow_edit)
                        VALUES(@user_id, @user_name, @department, @allow_edit)");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@user_id", engineer.user_id);
                    cmd.Parameters.AddWithValue("@user_name", engineer.user_name);
                    cmd.Parameters.AddWithValue("@department", engineer.department);
                    cmd.Parameters.AddWithValue("@allow_edit", engineer.allow_edit);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return "Success";
        }

        public string UpdateEngineerUser(EngUserModel engineer)
        {
            try
            {
                string string_command = string.Format($@"
                    UPDATE EngineerUsers 
                    SET
                        user_name = @user_name,
                        department = @department,
                        allow_edit = @allow_edit
                    WHERE user_id = @user_id");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@user_id", engineer.user_id);
                    cmd.Parameters.AddWithValue("@user_name", engineer.user_name);
                    cmd.Parameters.AddWithValue("@department", engineer.department);
                    cmd.Parameters.AddWithValue("@allow_edit", engineer.allow_edit);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return "Success";
        }
    }
}
