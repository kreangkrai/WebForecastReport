using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebForecastReport.Service
{
    public class ConnectSQL
    {
        public static SqlConnection con;
        public static SqlConnection OpenConnect()
        {
            con = new SqlConnection("Data Source = 192.168.15.202, 1433; Initial Catalog = Forecast; User Id = sa; Password = p@ssw0rd; Timeout = 120");
            con.Open();
            return con;
        }
        public static SqlConnection CloseConnect()
        {
            con.Close();
            return con;
        }
    }
}
