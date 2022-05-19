using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;

namespace WebForecastReport.Service
{
    public class LogStagesService : ILogStages
    {
        public List<Log_StagesModel> GetStages()
        {
            try
            {
                List<Log_StagesModel> logs = new List<Log_StagesModel>();
                SqlCommand cmd = new SqlCommand("select * from Log_Stages", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Log_StagesModel s = new Log_StagesModel()
                        {
                            quotation = dr["quotation"].ToString(),
                            project_name = dr["project_name"].ToString(),
                            date_edit = Convert.ToDateTime(dr["date_edit"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                            stages_from = dr["stages_from"].ToString(),
                            stages_to = dr["stages_to"].ToString(),
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

        public List<Log_StagesModel> GetStagesByName(string name)
        {
            try
            {
                List<Log_StagesModel> logs = new List<Log_StagesModel>();
                SqlCommand cmd = new SqlCommand("select * from Log_Stages where name='" + name + "'", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Log_StagesModel s = new Log_StagesModel()
                        {
                            quotation = dr["quotation"].ToString(),
                            project_name = dr["project_name"].ToString(),
                            date_edit = Convert.ToDateTime(dr["date_edit"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                            stages_from = dr["stages_from"].ToString(),
                            stages_to = dr["stages_to"].ToString(),
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

        public string Insert(Log_StagesModel model)
        {
            try
            {
                string command = string.Format($@"INSERT INTO Log_Stages VALUES(
                                                        '{model.quotation}',
                                                        '{model.project_name}',
                                                        '{model.date_edit}',
                                                        '{model.stages_from}',
                                                        '{model.stages_to}',
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
