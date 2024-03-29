﻿using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Service;

namespace WebForecastReport.Services.MPR
{
    public class WorkingHoursService : IWorkingHours
    {
        public List<WorkingHoursModel> GetWorkingHours()
        {
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        WorkingHours.ind,
                        WorkingHours.user_id,
                        Users.Name,
                        Users.Department2,
                        WorkingHours.working_date,
                        WorkingHours.week_number,
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.task_id,
                        Tasks.task_name,
                        WorkingHours.start_time,
                        WorkingHours.stop_time,
                        WorkingHours.lunch,
                        WorkingHours.dinner,
                        WorkingHours.note
                    FROM WorkingHours
                        LEFT JOIN gps_sale_tracking.dbo.Sale_User Users ON WorkingHours.user_id = Users.Login
                        LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                        LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id");
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
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = dr["ind"] != DBNull.Value ? Convert.ToInt32(dr["ind"]) : default(Int32),
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            department = dr["Department2"] != DBNull.Value ? dr["Department2"].ToString() : "",
                            working_date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            week_number = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : default(Int32),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            lunch = dr["lunch"] != DBNull.Value ? Convert.ToBoolean(dr["lunch"].ToString()) : default(bool),
                            dinner = dr["dinner"] != DBNull.Value ? Convert.ToBoolean(dr["dinner"].ToString()) : default(bool),
                            note = dr["note"] != DBNull.Value ? dr["note"].ToString() : "",
                        };
                        whs.Add(wh);
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
            return whs;
        }

        public List<WorkingHoursModel> GetWorkingHours(string user_name)
        {
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT 
                        WorkingHours.ind,
                        WorkingHours.user_id,
                        Users.Name,
                        Users.Department2,
                        WorkingHours.working_date,
                        WorkingHours.week_number,
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.task_id,
                        Tasks.task_name,
                        WorkingHours.start_time,
                        WorkingHours.stop_time,
                        WorkingHours.lunch,
                        WorkingHours.dinner,
                        WorkingHours.note
                    FROM WorkingHours
                        LEFT JOIN gps_sale_tracking.dbo.Sale_User Users ON WorkingHours.user_id = Users.Login 
                        LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                        LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    WHERE LOWER(Users.Name) = '{user_name}'");
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
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = dr["ind"] != DBNull.Value ? Convert.ToInt32(dr["ind"]) : default(Int32),
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            department = dr["Department2"] != DBNull.Value ? dr["Department2"].ToString() : "",
                            working_date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            week_number = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : default(Int32),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            lunch = dr["lunch"] != DBNull.Value ? Convert.ToBoolean(dr["lunch"].ToString()) : default(bool),
                            dinner = dr["dinner"] != DBNull.Value ? Convert.ToBoolean(dr["dinner"].ToString()) : default(bool),
                            note = dr["note"] != DBNull.Value ? dr["note"].ToString() : "",
                        };
                        whs.Add(wh);
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
            return whs;
        }

        public List<WorkingHoursModel> GetWorkingHours(string year, string month, string user_name)
        {
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        WorkingHours.ind,
                        WorkingHours.user_id,
                        Users.Name,
                        Users.Department2,
                        WorkingHours.working_date,
                        WorkingHours.week_number,
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.task_id,
                        Tasks.task_name,
                        WorkingHours.start_time,
                        WorkingHours.stop_time,
                        WorkingHours.lunch,
                        WorkingHours.dinner,
                        WorkingHours.note
                    FROM WorkingHours
                        LEFT JOIN gps_sale_tracking.dbo.Sale_User Users ON WorkingHours.user_id = Users.Login 
                        LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                        LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    WHERE WorkingHours.working_date like '{year}-{month}%' 
                    AND LOWER(Users.Name) ='{user_name}'");
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
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = dr["ind"] != DBNull.Value ? Convert.ToInt32(dr["ind"]) : default(Int32),
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            department = dr["Department2"] != DBNull.Value ? dr["Department2"].ToString() : "",
                            working_date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            week_number = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : default(Int32),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            lunch = dr["lunch"] != DBNull.Value ? Convert.ToBoolean(dr["lunch"].ToString()) : default(bool),
                            dinner = dr["dinner"] != DBNull.Value ? Convert.ToBoolean(dr["dinner"].ToString()) : default(bool),
                            note = dr["note"] != DBNull.Value ? dr["note"].ToString() : "",
                        };
                        whs.Add(wh);
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
            return whs;
        }

        public List<WorkingHoursModel> GetWorkingHours(string user_name, DateTime working_date)
        {
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        ind,
                        WorkingHours.user_id,
                        Users.Name,
                        WorkingHours.working_date,
                        WorkingHours.week_number,
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.task_id,
                        Tasks.task_name,
                        WorkingHours.start_time,
                        WorkingHours.stop_time,
                        WorkingHours.lunch,
                        WorkingHours.dinner,
                        WorkingHours.note
                    FROM WorkingHours
                        LEFT JOIN gps_sale_tracking.dbo.Sale_User Users ON WorkingHours.user_id = Users.Login 
                        LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                        LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    WHERE LOWER(Users.Name) = '{user_name}'
                    AND WorkingHours.working_date LIKE '{working_date.ToString("yyyy-MM-dd")}'");
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
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = dr["ind"] != DBNull.Value ? Convert.ToInt32(dr["ind"]) : default(Int32),
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            working_date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            week_number = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : default(Int32),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            lunch = dr["lunch"] != DBNull.Value ? Convert.ToBoolean(dr["lunch"].ToString()) : default(bool),
                            dinner = dr["dinner"] != DBNull.Value ? Convert.ToBoolean(dr["dinner"].ToString()) : default(bool),
                            note = dr["note"] != DBNull.Value ? dr["note"].ToString() : "",
                        };
                        whs.Add(wh);
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
            return whs;
        }

        public string AddWorkingHours(WorkingHoursModel wh)
        {
            try
            {
                string string_command = string.Format($@"
                    INSERT INTO WorkingHours(
                        user_id, working_date, week_number, job_id, task_id, start_time, stop_time, lunch, dinner, note)
                    VALUES (
                        @user_id, @working_date, (SELECT DATEPART(ISO_WEEK,@working_date)), @job_id, @task_id, @start_time, @stop_time, @lunch, @dinner, @note)");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@user_id", wh.user_id);
                    cmd.Parameters.AddWithValue("@working_date", wh.working_date);
                    cmd.Parameters.AddWithValue("@job_id", wh.job_id);
                    cmd.Parameters.AddWithValue("@task_id", wh.task_id);
                    cmd.Parameters.AddWithValue("@start_time", wh.start_time);
                    cmd.Parameters.AddWithValue("@stop_time", wh.stop_time);
                    cmd.Parameters.AddWithValue("@lunch", wh.lunch);
                    cmd.Parameters.AddWithValue("@dinner", wh.dinner);
                    cmd.Parameters.AddWithValue("@note", wh.note);
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

        public string UpdateWorkingHours(WorkingHoursModel wh)
        {
            try
            {
                string string_command = string.Format($@"
                    UPDATE WorkingHours 
                    SET
                        user_id = @user_id,
                        working_date = @working_date,
                        week_number = (SELECT DATEPART(ISO_WEEK,@working_date)),
                        job_id = @job_id,
                        task_id = @task_id,
                        start_time = @start_time,
                        stop_time = @stop_time,
                        note = @note
                    WHERE ind = @ind");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@user_id", wh.user_id);
                    cmd.Parameters.AddWithValue("@working_date", wh.working_date);
                    cmd.Parameters.AddWithValue("@job_id", wh.job_id);
                    cmd.Parameters.AddWithValue("@task_id", wh.task_id);
                    cmd.Parameters.AddWithValue("@start_time", wh.start_time);
                    cmd.Parameters.AddWithValue("@stop_time", wh.stop_time);
                    cmd.Parameters.AddWithValue("@note", wh.note);
                    cmd.Parameters.AddWithValue("@ind", wh.index);
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

        public string UpdateRestTime(WorkingHoursModel wh)
        {
            try
            {
                string string_command = string.Format($@"
                    UPDATE WorkingHours 
                    SET lunch = @lunch,
                        dinner = @dinner
                    WHERE user_id = @user_id 
                        AND working_date = @working_date 
                        AND job_id = @job_id
                        AND task_id = @task_id
                        AND start_time = @start_time
                        AND stop_time = @stop_time");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@lunch", wh.lunch);
                    cmd.Parameters.AddWithValue("@dinner", wh.dinner);
                    cmd.Parameters.AddWithValue("@user_id", wh.user_id);
                    cmd.Parameters.AddWithValue("@working_date", wh.working_date);
                    cmd.Parameters.AddWithValue("@job_id", wh.job_id);
                    cmd.Parameters.AddWithValue("@task_id", wh.task_id);
                    cmd.Parameters.AddWithValue("@start_time", wh.start_time);
                    cmd.Parameters.AddWithValue("@stop_time", wh.stop_time);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Pass");
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

        public string DeleteWorkingHours(WorkingHoursModel wh)
        {
            try
            {
                string string_command = string.Format($@"
                    DELETE FROM WorkingHours
                    WHERE user_id = @user_id
                        AND working_date = @working_date
                        AND job_id = @job_id
                        AND task_id = @task_id
                        AND start_time = @start_time
                        AND stop_time = @stop_time");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@user_id", wh.user_id);
                    cmd.Parameters.AddWithValue("@working_date", wh.working_date);
                    cmd.Parameters.AddWithValue("@job_id", wh.job_id);
                    cmd.Parameters.AddWithValue("@task_id", wh.task_id);
                    cmd.Parameters.AddWithValue("@start_time", wh.start_time);
                    cmd.Parameters.AddWithValue("@stop_time", wh.stop_time);
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

        public List<JobWeeklyWorkingHoursModel> GetAllJobWorkingHours(int year, int week)
        {
            List<JobWeeklyWorkingHoursModel> whs = new List<JobWeeklyWorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT DISTINCT 
                        WorkingHours.job_id,
                        Jobs.job_name,
                        Quotation.customer,
                        {year} as year,
                        {week} as week_number,
                        ISNULL(hours,0) AS hours 
                    FROM WorkingHours
                    LEFT JOIN ( 
                        SELECT job_id, week_number, 
                               SUM(CASE 
                                    WHEN DATEDIFF(HOUR, start_time, stop_time) > 0
                                    THEN DATEDIFF(HOUR, start_time, stop_time)
		                            ELSE DATEDIFF(HOUR, start_time, stop_time) * -1
	                               END) AS hours 
                        FROM WorkingHours 
                        WHERE working_date LIKE '{year}%' AND week_number = {week}
                        GROUP BY job_id, week_number) AS a ON WorkingHours.job_id = a.job_id
                    LEFT JOIN Jobs on WorkingHours.job_id = Jobs.job_id
                    LEFT JOIN Quotation on Jobs.quotation_no = Quotation.quotation_no");
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
                        JobWeeklyWorkingHoursModel wh = new JobWeeklyWorkingHoursModel()
                        {
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                            year = dr["year"] != DBNull.Value ? Convert.ToInt32(dr["year"]) : 0,
                            week = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : 0,
                            hours = dr["hours"] != DBNull.Value ? Convert.ToInt32(dr["hours"]) : 0
                        };
                        whs.Add(wh);
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
            return whs;
        }

        public List<EngWeeklyWorkingHoursModel> GetAllEngWorkingHours(int year, int week)
        {
            List<EngWeeklyWorkingHoursModel> whs = new List<EngWeeklyWorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT 
	                    DISTINCT user_id,
	                    {week} as week_number,
	                    {year} as year,
	                    ISNULL(a.hours, 0) AS hours
                    FROM (
	                    SELECT 
		                    user_id, 
		                    SUM(CASE 
				                    WHEN DATEDIFF(HOUR,start_time,stop_time) > 0
				                    THEN DATEDIFF(HOUR,start_time,stop_time)
				                    ELSE DATEDIFF(HOUR,start_time,stop_time) * -1
			                    END) AS hours
	                    FROM WorkingHours
	                    WHERE working_date LIKE '{year}%' AND week_number = {week}
	                    GROUP BY user_id) AS a
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
                        EngWeeklyWorkingHoursModel wh = new EngWeeklyWorkingHoursModel()
                        {
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            year = dr["year"] != DBNull.Value ? Convert.ToInt32(dr["year"]) : 0,
                            week = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : 0,
                            hours = dr["hours"] != DBNull.Value ? Convert.ToInt32(dr["hours"]) : 0
                        };
                        whs.Add(wh);
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
            return whs;
        }

        public List<WorkingHoursModel> GetWorkingHours(int year, int week)
        {
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        WorkingHours.ind,
                        WorkingHours.user_id,
                        Users.Name,
                        Users.Department2,
                        WorkingHours.working_date,
                        WorkingHours.week_number,
                        WorkingHours.job_id,
                        Jobs.job_name,
                        WorkingHours.task_id,
                        Tasks.task_name,
                        WorkingHours.start_time,
                        CASE 
                            WHEN WorkingHours.stop_time LIKE '00:00:00%' THEN '23:59:59'
                            ELSE WorkingHours.stop_time
                        END as stop_time,
                        WorkingHours.lunch,
                        WorkingHours.dinner,
                        WorkingHours.note
                    FROM WorkingHours
                        LEFT JOIN gps_sale_tracking.dbo.Sale_User Users ON WorkingHours.user_id = Users.Login
                        LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                        LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    WHERE WorkingHours.working_date LIKE '{year}%' AND WorkingHours.week_number = {week}");
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
                        WorkingHoursModel wh = new WorkingHoursModel()
                        {
                            index = dr["ind"] != DBNull.Value ? Convert.ToInt32(dr["ind"]) : default(Int32),
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                            department = dr["Department2"] != DBNull.Value ? dr["Department2"].ToString() : "",
                            working_date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            week_number = dr["week_number"] != DBNull.Value ? Convert.ToInt32(dr["week_number"]) : default(Int32),
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            start_time = dr["start_time"] != DBNull.Value ? TimeSpan.Parse(dr["start_time"].ToString()) : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? TimeSpan.Parse(dr["stop_time"].ToString()) : default(TimeSpan),
                            lunch = dr["lunch"] != DBNull.Value ? Convert.ToBoolean(dr["lunch"].ToString()) : default(bool),
                            dinner = dr["dinner"] != DBNull.Value ? Convert.ToBoolean(dr["dinner"].ToString()) : default(bool),
                            note = dr["note"] != DBNull.Value ? dr["note"].ToString() : "",
                        };
                        whs.Add(wh);
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
            return whs;
        }
    }
}
