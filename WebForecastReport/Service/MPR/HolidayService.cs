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
        public List<HolidayModel> GetHolidays()
        {
            try
            {
                List<HolidayModel> holidays = new List<HolidayModel>();
                string string_command = string.Format($@"SELECT * FROM Holidays");
                SqlCommand command = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                SqlDataReader data_reader = command.ExecuteReader();
                if (data_reader.HasRows)
                {
                    while (data_reader.Read())
                    {
                        HolidayModel holiday = new HolidayModel()
                        {
                            no = data_reader["no"] != DBNull.Value ? Convert.ToInt32(data_reader["no"]) : default(Int32),
                            date = data_reader["date"] != DBNull.Value ? Convert.ToDateTime(data_reader["date"]) : default(DateTime),
                            name = data_reader["name"] != DBNull.Value ? data_reader["name"].ToString() : ""
                        };
                        holidays.Add(holiday);
                    }
                    data_reader.Close();
                }
                return holidays.OrderBy(o => o.date).ToList();
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
