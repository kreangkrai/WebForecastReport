using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;

namespace WebForecastReport.Service
{
    public class AccessoryService : IAccessory
    {
        public List<UserModel> getAllUser()
        {
            try
            {
                List<UserModel> users = new List<UserModel>();
                SqlCommand cmd = new SqlCommand("select DISTINCT Login,Name from Sale_User order by Login", ConnectSQL.Open_db_gps_Connect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        UserModel u = new UserModel()
                        {
                            name = dr["Login"].ToString(),
                            fullname = dr["Name"].ToString()
                        };
                        users.Add(u);
                    }
                    dr.Close();
                }
                return users;
            }
            finally
            {
                if (ConnectSQL.con_db_gps.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.Close_db_gps_Connect();
                }
            }
        }

        public List<SaleModel> getSale()
        {
            try
            {
                List<SaleModel> sales = new List<SaleModel>();
                SqlCommand cmd = new SqlCommand("select DISTINCT Login from Sale_User where [Group] ='Sale' order by Login", ConnectSQL.Open_db_gps_Connect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SaleModel s = new SaleModel()
                        {
                            name = dr["Login"].ToString(),
                        };
                        sales.Add(s);
                    }
                    dr.Close();
                }
                return sales;
            }
            finally
            {
                if (ConnectSQL.con_db_gps.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.Close_db_gps_Connect();
                }
            }
        }
    }
}
