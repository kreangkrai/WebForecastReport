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
    public class User2Service : IUser2
    {
        public List<UserModel2> GetUsers()
        {
            try
            {
                List<UserModel2> users = new List<UserModel2>();
                string string_command = string.Format($@"SELECT * FROM [Sale_User]");
                SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.Open_db_gps_Connect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        UserModel2 user = new UserModel2()
                        {
                            //user_no = dr["user_no"] != DBNull.Value ? Convert.ToInt32(dr["user_no"]) : default(Int32),
                            user_id = dr["Login"] != DBNull.Value ? dr["Login"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            department = dr["Department2"] != DBNull.Value ? dr["Department2"].ToString() : "",
                        };
                        users.Add(user);
                    }
                    dr.Close();
                }
                return users;
            }
            finally
            {
                if (ConnectSQL.con_db_gps.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.Close_db_gps_Connect();
                }
            }
        }

        //public string CreateUser(UserModel2 user)
        //{
        //    try
        //    {
        //        string string_commamnd = string.Format($@"
        //        INSERT INTO [Users]
        //            (user_id, user_name, department) 
        //        VALUES
        //            (@user_id, @user_name, @department)");
        //        using (SqlCommand command = new SqlCommand(string_commamnd, ConnectSQL.OpenConnect()))
        //        {
        //            command.CommandType = System.Data.CommandType.Text;
        //            command.Parameters.AddWithValue("@user_id", user.user_id);
        //            command.Parameters.AddWithValue("@user_name", user.user_name);
        //            command.Parameters.AddWithValue("@department", user.department);
        //            command.ExecuteNonQuery();
        //        }
        //        return "Success";
        //    }
        //    finally
        //    {
        //        if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
        //        {
        //            ConnectSQL.CloseConnect();
        //        }
        //    }
        //}

        //public string UpdateUser(UserModel2 user)
        //{
        //    try
        //    {
        //        string string_command = string.Format($@"
        //        UPDATE [Users] 
        //        SET
        //            user_id = @user_id,
        //            user_name = @user_name,
        //            department = @department,
        //        WHERE user_no = @user_no");
        //        using (SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
        //        {
        //            command.CommandType = System.Data.CommandType.Text;
        //            command.Parameters.AddWithValue("@user_id", user.user_id);
        //            command.Parameters.AddWithValue("@user_name", user.user_name);
        //            command.Parameters.AddWithValue("@department", user.department);
        //            command.Parameters.AddWithValue("@user_no", user.user_no);
        //            command.ExecuteNonQuery();
        //        }
        //        return "Success";
        //    }
        //    finally
        //    {
        //        if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
        //        {
        //            ConnectSQL.CloseConnect();
        //        }
        //    }
        //}
    }
}
