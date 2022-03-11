using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using WebForecastReport.Service;

namespace WebForecastReport.Services.MPR
{
    public class CalculateOvertimeService : ICalculateWorkingHours
    {
        public WorkingHoursModel CalculateOvertime(WorkingHoursModel wh)
        {
            DateTime date = wh.working_date;
            TimeSpan start_time = wh.start_time;
            TimeSpan stop_time = wh.stop_time;
            bool lunch = wh.lunch;
            bool dinner = wh.dinner;

            TimeSpan normal = new TimeSpan();
            TimeSpan ot1_5 = new TimeSpan();
            TimeSpan ot3_0 = new TimeSpan();

            TimeSpan t1 = new TimeSpan(12, 0, 0);
            TimeSpan t2 = new TimeSpan(13, 0, 0);
            TimeSpan t3 = new TimeSpan(17, 30, 0);
            TimeSpan t4 = new TimeSpan(18, 0, 0);
            TimeSpan t5 = new TimeSpan(23, 59, 59);

            //00:00 -> 12:00
            if (start_time < t1)
            {
                normal += (stop_time >= t1) ? t1 - start_time : stop_time - start_time;
            }

            //12:00 -> 13:00
            if ((start_time < t2) && !lunch)
            {
                TimeSpan time_start = new TimeSpan();
                TimeSpan time_end = new TimeSpan();
                time_start = (start_time <= t1) ? t1 : start_time;
                time_end = (stop_time >= t2) ? t2 : stop_time;
                normal += time_end - time_start;
            }

            //13:00 -> 17:30
            if (start_time < t3)
            {
                TimeSpan time_start = new TimeSpan();
                TimeSpan time_end = new TimeSpan();
                time_start = (start_time <= t2) ? t2 : start_time;
                time_end = (stop_time >= t3) ? t3 : stop_time;
                normal += time_end - time_start;
            }

            //17:30 -> 18:00
            if ((start_time < t4) && !dinner)
            {
                TimeSpan time_start = new TimeSpan();
                TimeSpan time_end = new TimeSpan();
                time_start = (start_time <= t3) ? t3 : start_time;
                time_end = (stop_time >= t4) ? t4 : stop_time;
                ot1_5 += time_end - time_start;
            }

            //18:00 -> 23.59
            if (stop_time > t4)
            {
                TimeSpan time_start = new TimeSpan();
                TimeSpan time_end = new TimeSpan();
                time_start = (start_time <= t4) ? t4 : start_time;
                time_end = (stop_time >= t5) ? t5 : stop_time;
                ot1_5 += time_end - time_start;
            }

            if (date.DayOfWeek.ToString() == "Saturday" || date.DayOfWeek.ToString() == "Sunday")
            {
                ot1_5 += normal;
                normal = default(TimeSpan);
            }

            TimeSpan max_hours = new TimeSpan(8, 0, 0);
            if (normal > max_hours)
            {
                ot1_5 += normal - max_hours;
                normal = max_hours;
            }

            while (ot1_5 > max_hours)
            {
                ot3_0 += ot1_5 - max_hours;
                ot1_5 = max_hours;
            }

            wh.normal = normal;
            wh.ot1_5 = ot1_5;
            wh.ot3_0 = ot3_0;
            return wh;
        }

        public List<WorkingHoursModel> GetWorkingHours(string user_id)
        {
            List<WorkingHoursModel> whs = new List<WorkingHoursModel>();
            try
            {
                string string_command = string.Format($@"
		            WITH main AS (
		                SELECT 
                            user_id,
			                CONVERT(VARCHAR,working_date,126) AS working_date,
			                start_time,
			                stop_time,
			                wh_type, 
			                DATEDIFF(MINUTE,start_time,stop_time) AS minute,
			                '' as REG,
			                '' as OT1_5,
			                '' as OT3
		                FROM WorkingHours 
		                WHERE working_date LIKE '2022-03%' AND user_id = 'Kriangkrai.R'
		                UNION ALL
		                    SELECT s1.user_id,
			                CONVERT(VARCHAR,s1.working_date,126) AS working_date,
			                '' AS start_time,
			                '' AS stop_time,
			                'Total' AS wh_type,
			                '' AS minute,
			                SUM(s1.REG) AS REG ,
			                SUM(s1.OT1_5) AS OT1_5,
			                SUM(s1.OT3) AS OT3
			                FROM (
				                SELECT 
                                    s.user_id,
					                s.working_date,
					                CASE WHEN s.wh_type = 'REG' THEN SUM(s.minute) ELSE 0 END AS REG,
					                CASE WHEN s.wh_type = 'OT1_5' THEN SUM(s.minute) ELSE 0 END AS OT1_5,
					                CASE WHEN s.wh_type = 'OT3' THEN SUM(s.minute) ELSE 0 END AS OT3
				                FROM(
					                SELECT 
                                        user_id + '_Total' AS user_id,
						                working_date,
						                wh_type,
						                SUM(DATEDIFF(MINUTE,start_time,stop_time)) AS minute
					                FROM WorkingHours 
				                    WHERE working_date LIKE '2022-03%' AND user_id = 'Kriangkrai.R'
				                    GROUP BY user_id, working_date, wh_type) AS s
			                    GROUP BY s.user_id, s.working_date, s.wh_type ) AS s1
		                    GROUP BY s1.user_id, s1.working_date
		                    UNION ALL
		                        SELECT 
                                    'Total' AS user_id,
			                        '2022-03' AS working_date,
			                        '' as start_time,
			                        '' as stop_time,
			                        '' AS wh_type,
			                        '' AS minute,
			                        SUM(s1.REG) AS REG ,
			                        SUM(s1.OT1_5) AS OT1_5,
			                        SUM(s1.OT3) AS OT3
			                    FROM (
				                    SELECT 
                                        s.user_id,
					                    s.working_date,
					                    CASE WHEN s.wh_type = 'REG' THEN SUM(s.minute) ELSE 0 END AS REG,
					                    CASE WHEN s.wh_type = 'OT1_5' THEN SUM(s.minute) ELSE 0 END AS OT1_5,
					                    CASE WHEN s.wh_type = 'OT3' THEN SUM(s.minute) ELSE 0 END AS OT3
				                    FROM(
					                    SELECT 
                                            user_id + '_Total' AS user_id,
						                    working_date,
						                    wh_type,
						                    SUM(DATEDIFF(MINUTE,start_time,stop_time)) AS minute
					                    FROM WorkingHours 
					                    WHERE working_date LIKE '2022%' AND user_id = 'Kriangkrai.R'
					                    GROUP BY user_id, working_date, wh_type) AS s
			                    GROUP BY s.user_id, s.working_date, s.wh_type ) AS s1
		                    GROUP BY s1.user_id )
		            SELECT * FROM main ORDER BY main.working_date DESC, main.user_id, main.start_time");
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
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            working_date = dr["working_date"] != DBNull.Value ? Convert.ToDateTime(dr["working_date"]) : default(DateTime),
                            start_time = dr["start_time"] != DBNull.Value ? Convert.ToDateTime((dr["start_time"])).TimeOfDay : default(TimeSpan),
                            stop_time = dr["stop_time"] != DBNull.Value ? Convert.ToDateTime(dr["stop_time"]).TimeOfDay : default(TimeSpan),
                            normal = dr["REG"] != DBNull.Value ? Convert.ToDateTime(dr["REG"]).TimeOfDay : default(TimeSpan),
                            ot1_5 = dr["OT1_5"] != DBNull.Value ? Convert.ToDateTime(dr["OT1_5"]).TimeOfDay : default(TimeSpan),
                            ot3_0 = dr["OT3"] != DBNull.Value ? Convert.ToDateTime(dr["OT3"]).TimeOfDay : default(TimeSpan)
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
