using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Service;
using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Services.MPR
{
    public class AnalysisService : IAnalysis
    {
        public List<TaskRatioModel> GetTaskRatio(string job_id)
        {
            List<TaskRatioModel> trs = new List<TaskRatioModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT 
	                    WorkingHours.job_id,
	                    Jobs.job_name,
	                    WorkingHours.task_id,
	                    Tasks.task_name,
	                    SUM(
	                    CASE 
		                    WHEN lunch = 1 AND dinner = 1 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 2 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 2 ELSE 0 END
		                    WHEN lunch = 1 AND dinner = 0 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 1 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 1 ELSE 0 END
		                    WHEN lunch = 0 AND dinner = 1 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 1 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 1 ELSE 0 END
		                    ELSE 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) > 0 THEN DATEDIFF(HOUR, start_time, stop_time) ELSE 0 END
	                    END) as hours
                    FROM WorkingHours
                    LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                    WHERE WorkingHours.job_id = '{job_id}'
                    GROUP BY WorkingHours.task_id, Tasks.task_name, WorkingHours.job_id, Jobs.job_name
                    ORDER BY hours desc");
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
                        TaskRatioModel tr = new TaskRatioModel()
                        {
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            hours = dr["hours"] != DBNull.Value ? Convert.ToDouble(dr["hours"]) : 0,
                            percents = 0
                        };
                        trs.Add(tr);
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
            return trs;
        }

        public List<TaskDistributionModel> GetTaskDistribution(string job_id)
        {
            List<TaskDistributionModel> tds = new List<TaskDistributionModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT 
	                    WorkingHours.job_id,
	                    Jobs.job_name,
	                    WorkingHours.task_id,
	                    Tasks.task_name,
	                    SUM(
	                    CASE 
		                    WHEN lunch = 1 AND dinner = 1 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 2 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 2 ELSE 0 END
		                    WHEN lunch = 1 AND dinner = 0 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 1 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 1 ELSE 0 END
		                    WHEN lunch = 0 AND dinner = 1 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 1 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 1 ELSE 0 END
		                    ELSE 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) > 0 THEN DATEDIFF(HOUR, start_time, stop_time) ELSE 0 END
	                    END) as hours
                    FROM WorkingHours
                    LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                    WHERE WorkingHours.job_id = '{job_id}'
                    GROUP BY WorkingHours.task_id, Tasks.task_name, WorkingHours.job_id, Jobs.job_name
                    ORDER BY hours desc");
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
                        TaskDistributionModel td = new TaskDistributionModel()
                        {
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            hours = dr["hours"] != DBNull.Value ? Convert.ToDouble(dr["hours"].ToString()) : 0,
                        };
                        tds.Add(td);
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
            return tds;
        }

        public List<ManpowerRatioModel> GetManpowerRatio(string job_id)
        {
            List<ManpowerRatioModel> mrs = new List<ManpowerRatioModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT 
	                    WorkingHours.user_id,
	                    gps_sale_tracking.[dbo].Sale_User.name,
	                    WorkingHours.job_id,
	                    Jobs.job_name,
	                    WorkingHours.task_id,
	                    Tasks.task_name,
	                    SUM(CASE
		                    WHEN lunch = 1 AND dinner = 1 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 2 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 2 ELSE 0 END
		                    WHEN lunch = 1 AND dinner = 0 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 1 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 1 ELSE 0 END
		                    WHEN lunch = 0 AND dinner = 0 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 1 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 1 ELSE 0 END
		                    ELSE 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) > 0 THEN DATEDIFF(HOUR, start_time, stop_time) ELSE 0 END
	                    END) as hours
                    FROM WorkingHours
                    LEFT JOIN gps_sale_tracking.[dbo].Sale_User ON WorkingHours.user_id = gps_sale_tracking.[dbo].Sale_User.Login
                    LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                    LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    WHERE WorkingHours.job_id = '{job_id}'
                    GROUP BY WorkingHours.user_id, gps_sale_tracking.[dbo].Sale_User.name, WorkingHours.job_id, job_name, WorkingHours.task_id, Tasks.task_name
                    ORDER BY user_id");
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
                        ManpowerRatioModel mr = new ManpowerRatioModel()
                        {
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["name"] != DBNull.Value ? dr["name"].ToString() : "",
                            hours = dr["hours"] != DBNull.Value ? Convert.ToDouble(dr["hours"]) : 0,
                            percents = 0
                        };
                        mrs.Add(mr);
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
            return mrs;
        }

        public List<ManpowerDistributionModel>  GetManpowerDistribution(string job_id)
        {
            List<ManpowerDistributionModel> mds = new List<ManpowerDistributionModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT 
	                    WorkingHours.user_id,
	                    gps_sale_tracking.[dbo].Sale_User.name,
	                    WorkingHours.job_id,
	                    Jobs.job_name,
	                    WorkingHours.task_id,
	                    Tasks.task_name,
	                    SUM(CASE
		                    WHEN lunch = 1 AND dinner = 1 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 2 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 2 ELSE 0 END
		                    WHEN lunch = 1 AND dinner = 0 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 1 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 1 ELSE 0 END
		                    WHEN lunch = 0 AND dinner = 0 THEN 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) - 1 > 0 THEN DATEDIFF(HOUR, start_time, stop_time) - 1 ELSE 0 END
		                    ELSE 
			                    CASE WHEN DATEDIFF(HOUR, start_time, stop_time) > 0 THEN DATEDIFF(HOUR, start_time, stop_time) ELSE 0 END
	                    END) as hours
                    FROM WorkingHours
                    LEFT JOIN gps_sale_tracking.[dbo].Sale_User ON WorkingHours.user_id = gps_sale_tracking.[dbo].Sale_User.Login
                    LEFT JOIN Jobs ON WorkingHours.job_id = Jobs.job_id
                    LEFT JOIN Tasks ON WorkingHours.task_id = Tasks.task_id
                    WHERE WorkingHours.job_id = '{job_id}'
                    GROUP BY WorkingHours.user_id, gps_sale_tracking.[dbo].Sale_User.name, WorkingHours.job_id, job_name, WorkingHours.task_id, Tasks.task_name
                    ORDER BY user_id");
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
                        ManpowerDistributionModel md = new ManpowerDistributionModel()
                        {
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["name"] != DBNull.Value ? dr["name"].ToString() : "",
                            task_id = dr["task_id"] != DBNull.Value ? dr["task_id"].ToString() : "",
                            task_name = dr["task_name"] != DBNull.Value ? dr["task_name"].ToString() : "",
                            hours = dr["hours"] != DBNull.Value ? Convert.ToInt32(dr["hours"].ToString()) : 0,
                        };
                        mds.Add(md);
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
            return mds;
        }
    }
}