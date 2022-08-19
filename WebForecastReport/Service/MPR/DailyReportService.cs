using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Service.MPR
{
    public class DailyReportService : IDailyReport
    {
        public List<DailyActivityModel> GetDailyActivities(string user_name, DateTime start_date, DateTime stop_date)
        {
            List<DailyActivityModel> dlrs = new List<DailyActivityModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
	                    ind,
                        working_date,
                        start_time,
                        stop_time,
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.task_id,
                        Tasks.task_name,
                        activity,
                        problem,
                        solution,
                        tomorrow_plan,
                        WorkingHours.user_id,
	                    EngineerUsers.user_name,
                        Quotation.customer,
                        note
                    FROM WorkingHours 
                    LEFT JOIN EngineerUsers ON WorkingHours.user_id = EngineerUsers.user_id
                    LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                    LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                    WHERE EngineerUsers.user_name LIKE '{user_name}'
                    AND working_date BETWEEN '{start_date.ToString("yyyy-MM-dd")}' AND '{stop_date.ToString("yyyy-MM-dd")}'
                    ORDER BY working_date, start_time;
                ");
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
                        DailyActivityModel dlr = new DailyActivityModel()
                        {
                            ind = dr["ind"] != DBNull.Value ? Convert.ToInt32(dr["ind"]) : 0,
                            date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["user_name"] != DBNull.Value ? dr["user_name"].ToString() : "",
                            note = dr["note"] != DBNull.Value ? dr["note"].ToString() : "",
                            activity = dr["activity"] != DBNull.Value ? dr["activity"].ToString() : "",
                            problem = dr["problem"] != DBNull.Value ? dr["problem"].ToString() : "",
                            solution = dr["solution"] != DBNull.Value ? dr["solution"].ToString() : "",
                            tomorrow_plan = dr["tomorrow_plan"] != DBNull.Value ? dr["tomorrow_plan"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                        };
                        dlrs.Add(dlr);
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
            return dlrs;
        }

        public string EditDailyReport(DailyActivityModel dlr)
        {
            try
            {
                string string_command = string.Format($@"
                    UPDATE WorkingHours
                    SET
                        problem = @problem,
                        solution = @solution,
                        tomorrow_plan = @tomorrow_plan,
                        customer = @customer
                    WHERE ind = @ind");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@problem", dlr.problem);
                    cmd.Parameters.AddWithValue("@solution", dlr.solution);
                    cmd.Parameters.AddWithValue("@tomorrow_plan", dlr.tomorrow_plan);
                    cmd.Parameters.AddWithValue("@customer", dlr.customer);
                    cmd.Parameters.AddWithValue("@ind", dlr.ind);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
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
