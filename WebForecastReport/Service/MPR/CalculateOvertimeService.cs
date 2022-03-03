using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
