using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Service.MPR
{
    public class TasksByWeekService : ITasksByWeek
    {
        public List<TasksByWeekModel> GetTasksByWeek(string year, string week)
        {
            List<TasksByWeekModel> tasks = new List<TasksByWeekModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
	                    WorkingHours.user_id,
	                    gps_sale_tracking.[dbo].Sale_User.Name as user_name,
	                    WorkingHours.week_number,
	                    WorkingHours.working_date,
	                    WorkingHours.job_id,
                        Jobs.job_name,
	                    Quotation.customer,
	                    WorkingHours.task_id,
	                    Tasks.task_name,
	                    WorkingHours.start_time,
	                    WorkingHours.stop_time,
	                    WorkingHours.lunch,
	                    WorkingHours.dinner
                    FROM WorkingHours
                    LEFT JOIN gps_sale_tracking.[dbo].Sale_User ON Sale_User.login = WorkingHours.user_id
                    LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                    LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                    LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    WHERE working_date LIKE '{year}%' AND week_number = {week}
                    ORDER BY user_id, working_date");
                SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                    ConnectSQL.OpenConnect();
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TasksByWeekModel task = new TasksByWeekModel()
                        {
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["user_name"] != DBNull.Value ? dr["user_name"].ToString() : "",
                            week_number = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : 0,
                            working_date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            lunch = dr["lunch"] != DBNull.Value ? Convert.ToBoolean(dr["lunch"]) : true,
                            dinner = dr["dinner"] != DBNull.Value ? Convert.ToBoolean(dr["dinner"]) : true,
                        };
                        tasks.Add(task);
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
            return tasks;
        }
    }
}
