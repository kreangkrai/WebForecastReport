using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Service;

namespace WebForecastReport.Services.MPR
{
    public class HolidayService : IHoliday
    {
        public List<HolidayModel> GetHolidays(string year)
        {
            List<HolidayModel> holidays = new List<HolidayModel>();
            try
            {
                string string_command = string.Format($@"SELECT * FROM Holidays WHERE date LIKE '{year}%'");
                SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                if(ConnectSQL.con.State != System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                    ConnectSQL.OpenConnect();
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        HolidayModel holiday = new HolidayModel()
                        {
                            no = dr["no"] != DBNull.Value ? Convert.ToInt32(dr["no"]) : default(Int32),
                            date = dr["date"] != DBNull.Value ? Convert.ToDateTime(dr["date"]) : default(DateTime),
                            name = dr["name"] != DBNull.Value ? dr["name"].ToString() : ""
                        };
                        holidays.Add(holiday);
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
            return holidays.OrderBy(o => o.date).ToList();
        }
    }
}
