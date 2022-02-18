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
        public static SqlConnection con_db_gps;
        public static SqlConnection OpenConnect()
        {
            con = new SqlConnection("Data Source = 192.168.15.202, 1433; Initial Catalog = MES; User Id = sa; Password = p@ssw0rd; Timeout = 120");
            //con = new SqlConnection("Data Source = 192.168.15.202, 1433; Initial Catalog = MES_TEST; User Id = sa; Password = p@ssw0rd; Timeout = 120");
            //con = new SqlConnection(@"Data Source=DESKTOP-BMFLGER\SA;Initial Catalog=MES_TEST;Integrated Security=True");
            con.Open();
            return con;
        }
        public static SqlConnection CloseConnect()
        {
            con.Close();
            return con;
        }

        public static SqlConnection Open_db_gps_Connect()
        {
            //con_db_gps = new SqlConnection(@"Data Source=DESKTOP-BMFLGER\SA;Initial Catalog=MES_TEST;Integrated Security=True");
            con_db_gps = new SqlConnection("Data Source = 192.168.15.202, 1433; Initial Catalog = gps_sale_tracking; User Id = sa; Password = p@ssw0rd; Timeout = 120");
            con_db_gps.Open();
            return con_db_gps;
        }
        public static SqlConnection Close_db_gps_Connect()
        {
            con_db_gps.Close();
            return con_db_gps;
        }
    }
}
