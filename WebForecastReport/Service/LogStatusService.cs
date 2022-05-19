using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;

namespace WebForecastReport.Service
{
    public class LogStatusService : ILogStatus
    {
        public List<Log_StatusModel> GetStatus()
        {
            try
            {
                List<Log_StatusModel> logs = new List<Log_StatusModel>();
                SqlCommand cmd = new SqlCommand("select * from Log_Status", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Log_StatusModel s = new Log_StatusModel()
                        {
                            quotation = dr["quotation"].ToString(),
                            project_name = dr["project_name"].ToString(),
                            date_edit = Convert.ToDateTime(dr["date_edit"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                            status_from = dr["status_from"].ToString(),
                            status_to = dr["status_to"].ToString(),
                            reason = dr["reason"].ToString(),
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

        public List<Log_StatusModel> GetStatusByName(string name)
        {
            try
            {
                List<Log_StatusModel> logs = new List<Log_StatusModel>();
                SqlCommand cmd = new SqlCommand("select * from Log_Status where name='" + name + "'", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Log_StatusModel s = new Log_StatusModel()
                        {
                            quotation = dr["quotation"].ToString(),
                            project_name = dr["project_name"].ToString(),
                            date_edit = Convert.ToDateTime(dr["date_edit"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                            status_from = dr["status_from"].ToString(),
                            status_to = dr["status_to"].ToString(),
                            reason = dr["reason"].ToString(),
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
        public string Insert(Log_StatusModel model)
        {
            try
            {
                string command = string.Format($@"INSERT INTO Log_Status VALUES(
                                                        '{model.quotation}',
                                                        '{model.project_name}',
                                                        '{model.date_edit}',
                                                        '{model.status_from}',
                                                        '{model.status_to}',
                                                        '{model.reason}',
                                                        '{model.name}')");
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                cmd.ExecuteNonQuery();

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
