using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Service.MPR
{
    public class EngineerScoreService : IScore
    {
        public List<EngineerScoreModel> GetScores(string user_id, string year)
        {
            List<EngineerScoreModel> scores = new List<EngineerScoreModel>();
            try
            {
                string string_command = string.Format($@"with t2 as (
					SELECT 
		                    WorkingHours.job_id, 
		                    SUM(case when lunch = 1
							 then case when dinner = 1 
								then 
									DATEDIFF(HOUR,start_time,stop_time) - 2
								else 
									DATEDIFF(HOUR,start_time,stop_time) - 1 
							 end
							 else case when dinner = 1 
								then 
									DATEDIFF(HOUR,start_time,stop_time) - 1 
								else 
									DATEDIFF(HOUR,start_time,stop_time)
							 end
						end ) AS total_manpower 
	                    FROM WorkingHours
	                    GROUP BY job_id
					),
					t3 as (
					SELECT 
		                    WorkingHours.job_id, 
		                    SUM(case when lunch = 1
							 then case when dinner = 1 
								then 
									DATEDIFF(HOUR,start_time,stop_time) - 2
								else 
									DATEDIFF(HOUR,start_time,stop_time) - 1 
							 end
							 else case when dinner = 1 
								then 
									DATEDIFF(HOUR,start_time,stop_time) - 1 
								else 
									DATEDIFF(HOUR,start_time,stop_time)
							 end
						end ) AS working_hours 
	                    FROM WorkingHours
						WHERE user_id = '{user_id}'
	                    GROUP BY user_id,job_id
					)
					
					SELECT distinct
	                    t1.user_id AS user_id,
	                    t1.job_id AS job_id,
	                    Jobs.job_name AS job_name,
                        cost,
	                    md_rate AS md_rate,
	                    pd_rate AS pd_rate,
	                    (md_rate + pd_rate) AS factor,						
	                    t2.total_manpower AS total_manpower,
	                    (cost / t2.total_manpower) AS cost_per_tmp,
	                    t3.working_hours AS manpower,
	                    (CAST(t3.working_hours AS FLOAT) / (CAST(t2.total_manpower AS FLOAT))) AS manpower_per_tmp,
	                    (cost * (md_rate + pd_rate) * (cost / t2.total_manpower) * (CAST(t3.working_hours AS FLOAT) / (CAST(t2.total_manpower AS FLOAT)))) AS score
                    FROM WorkingHours  As t1  
					LEFT JOIN Jobs ON t1.job_id = Jobs.job_id		     
                    INNER JOIN t2	                     
                    ON t1.job_id = t2.job_id
                    INNER JOIN t3					
                    ON t1.job_id = t3.job_id
                 WHERE t1.user_id = '{user_id}'");
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
                        EngineerScoreModel score = new EngineerScoreModel()
                        {
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            cost = dr["cost"] != DBNull.Value ? Convert.ToInt32(dr["cost"]) : 0,
                            md_rate = dr["md_rate"] != DBNull.Value ? Convert.ToDouble(dr["md_rate"]) : 0,
                            pd_rate = dr["pd_rate"] != DBNull.Value ? Convert.ToDouble(dr["pd_rate"]) : 0,
                            factor = dr["factor"] != DBNull.Value ? Convert.ToDouble(dr["factor"]) : 0,
                            total_manpower = dr["total_manpower"] != DBNull.Value ? Convert.ToInt32(dr["total_manpower"]) : 0,
                            cost_per_tmp = dr["cost_per_tmp"] != DBNull.Value ? Convert.ToDouble(dr["cost_per_tmp"]) : 0,
                            manpower = dr["manpower"] != DBNull.Value ? Convert.ToInt32(dr["manpower"]) : 0,
                            manpower_per_tmp = dr["manpower_per_tmp"] != DBNull.Value ? Convert.ToDouble(dr["manpower_per_tmp"]) : 0,
                            score = dr["score"] != DBNull.Value ? Convert.ToDouble(dr["score"]) : 0,
                        };
                        scores.Add(score);
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
            return scores;
        }
    }
}
