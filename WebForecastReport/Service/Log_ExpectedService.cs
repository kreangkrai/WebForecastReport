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
    public class Log_ExpectedService : ILog_Expected
    {
        public List<Log_ExpectedModel> getLogByName(string name)
        {
            try
            {
                List<Log_ExpectedModel> logs = new List<Log_ExpectedModel>();
                SqlCommand cmd = new SqlCommand("select * from Log_Quotation_Expected where name='" + name + "'", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Log_ExpectedModel s = new Log_ExpectedModel()
                        {
                            quotation = dr["quotation"].ToString(),
                            project_name = dr["project_name"].ToString(),
                            date_edit = Convert.ToDateTime(dr["date_edit"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                            date_from = Convert.ToDateTime(dr["date_from"].ToString()).ToString("yyyy-MM-dd"),
                            date_to = Convert.ToDateTime(dr["date_to"].ToString()).ToString("yyyy-MM-dd"),
                            name = dr["name"].ToString()
                        };
                        logs.Add(s);
                    }
                    dr.Close();
                }
                return logs;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<Log_ExpectedModel> getLogs()
        {
            try
            {
                List<Log_ExpectedModel> logs = new List<Log_ExpectedModel>();
                SqlCommand cmd = new SqlCommand("select * from Log_Quotation_Expected", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Log_ExpectedModel s = new Log_ExpectedModel()
                        {
                            quotation = dr["quotation"].ToString(),
                            project_name = dr["project_name"].ToString(),
                            date_edit = Convert.ToDateTime(dr["date_edit"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                            date_from = Convert.ToDateTime(dr["date_from"].ToString()).ToString("yyyy-MM-dd"),
                            date_to = Convert.ToDateTime(dr["date_to"].ToString()).ToString("yyyy-MM-dd"),
                            name = dr["name"].ToString()
                        };
                        logs.Add(s);
                    }
                    dr.Close();
                }
                return logs;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public string Insert(Log_ExpectedModel model)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Log_Quotation_Expected(quotation,
                                                                                            project_name,
                                                                                            date_edit,
                                                                                            date_from,
                                                                                            date_to,name) VALUES
                                                                                            (@quotation,
                                                                                            @project_name,
                                                                                            @date_edit,
                                                                                            @date_from,
                                                                                            @date_to,
                                                                                            @name)", ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = ConnectSQL.OpenConnect();
                    cmd.Parameters.AddWithValue("@quotation", model.quotation);
                    cmd.Parameters.AddWithValue("@project_name", model.project_name);
                    cmd.Parameters.AddWithValue("@date_edit", model.date_edit);
                    cmd.Parameters.AddWithValue("@date_from", model.date_from);
                    cmd.Parameters.AddWithValue("@date_to", model.date_to);
                    cmd.Parameters.AddWithValue("@name", model.name);
                    cmd.ExecuteNonQuery();
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
    }
}
