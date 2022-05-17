using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Service.MPR
{
    public class ENG_DailyReportService : IDRService
    {
        public List<ENG_DailyReportModel> GetDailyReport(string user_id, string month, string job_id)
        {
            List<ENG_DailyReportModel> dlrs = new List<ENG_DailyReportModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        ind,
	                    date,
                        start_time,
                        stop_time,
                        job_id,
                        user_id,
                        activity,
                        problem,
                        solution,
                        tomorrow_plan,
                        customer,
                        status
                    FROM ENG_DAILY_REPORTS
                    WHERE user_id = '{user_id}' AND date LIKE '{month}%' AND job_id = '{job_id}'
                    ORDER BY date, start_time");
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
                        ENG_DailyReportModel dlr = new ENG_DailyReportModel()
                        {
                            index = dr["ind"] != DBNull.Value ? Convert.ToInt32(dr["ind"]) : 0,
                            date = dr["date"] != DBNull.Value ? Convert.ToDateTime(dr["date"]) : default(DateTime),
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            //job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            //user_name = dr["user_name"] != DBNull.Value ? dr["user_name"].ToString() : "",
                            activity = dr["activity"] != DBNull.Value ? dr["activity"].ToString() : "",
                            problem = dr["problem"] != DBNull.Value ? dr["problem"].ToString() : "",
                            solution = dr["solution"] != DBNull.Value ? dr["solution"].ToString() : "",
                            tomorrow_plan = dr["tomorrow_plan"] != DBNull.Value ? dr["tomorrow_plan"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                            status = dr["status"] != DBNull.Value ? dr["status"].ToString() : "",
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

        public string AddDailyReport(ENG_DailyReportModel dlr)
        {
            try
            {
                string string_command = string.Format($@"
                    INSERT INTO ENG_DAILY_REPORTS(
                        date, start_time, stop_time, job_id, user_id, activity, problem, solution, tomorrow_plan, customer)
                    VALUES (
                        @date, @start_time, @stop_time, @job_id, @user_id, @activity, @problem, @solution, @tomorrow_plan, @customer)");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@date", dlr.date);
                    cmd.Parameters.AddWithValue("@start_time", dlr.start_time);
                    cmd.Parameters.AddWithValue("@stop_time", dlr.stop_time);
                    cmd.Parameters.AddWithValue("@job_id", dlr.job_id);
                    cmd.Parameters.AddWithValue("@user_id", dlr.user_id);
                    cmd.Parameters.AddWithValue("@activity", dlr.activity);
                    cmd.Parameters.AddWithValue("@problem", dlr.problem);
                    cmd.Parameters.AddWithValue("@solution", dlr.solution);
                    cmd.Parameters.AddWithValue("@tomorrow_plan", dlr.tomorrow_plan);
                    cmd.Parameters.AddWithValue("@customer", dlr.customer);
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

        public string EditDailyReport(ENG_DailyReportModel dlr)
        {
            try
            {
                string string_command = string.Format($@"
                    UPDATE ENG_DAILY_REPORTS
                    SET
                        date = @date,
                        start_time = @start_time,
                        stop_time = @stop_time,
                        job_id = @job_id,
                        user_id = @user_id,
                        activity = @activity,
                        problem = @problem,
                        solution = @solution,
                        tomorrow_plan = @tomorrow_plan,
                        customer = @customer
                    WHERE ind = @ind");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@date", dlr.date);
                    cmd.Parameters.AddWithValue("@start_time", dlr.start_time);
                    cmd.Parameters.AddWithValue("@stop_time", dlr.stop_time);
                    cmd.Parameters.AddWithValue("@job_id", dlr.job_id);
                    cmd.Parameters.AddWithValue("@user_id", dlr.user_id);
                    cmd.Parameters.AddWithValue("@activity", dlr.activity);
                    cmd.Parameters.AddWithValue("@problem", dlr.problem);
                    cmd.Parameters.AddWithValue("@solution", dlr.solution);
                    cmd.Parameters.AddWithValue("@tomorrow_plan", dlr.tomorrow_plan);
                    cmd.Parameters.AddWithValue("@customer", dlr.customer);
                    cmd.Parameters.AddWithValue("@ind", dlr.index);
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
