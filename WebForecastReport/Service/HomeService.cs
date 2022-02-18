using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;

namespace WebForecastReport.Service
{
    public class HomeService : IHome
    {
        public List<Home_DataModel> getData(string name)
        {
            try
            {
                List<Home_DataModel> datas = new List<Home_DataModel>();
                SqlCommand cmd = new SqlCommand(@"select 'Type' as [group],sale_name,product_type as [type], '' as stages, format(sum(cast(replace(quoted_price,',','') as float))/1000000,'N2') as mb
                                                  from Quotation where sale_name='" + name + "' group by sale_name,product_type having product_type <>'' union all " +

                                                  "select 'Stages' as [group],sale_name,product_type,stages,format(sum(cast(replace(quoted_price,',','') as float))/1000000,'N2') as mb " +
                                                  "from Quotation " +
                                                  "where sale_name='" + name + "' " +
                                                  "group by sale_name,stages,product_type having product_type <>'' and stages in('Closed(Won)','Closed(Lost)','No go') union all " +

                                                  "select s1.[group],s1.sale_name,s1.product_type,'Pending' as stages, sum(cast(s1.mb as float)) as mb " +
                                                  "from (select 'Stages' as [group],sale_name,product_type,stages,format(sum(cast(replace(quoted_price,',','') as float))/1000000,'N2') as mb " +
                                                        "from Quotation where sale_name='" + name + "' " +
                                                        "group by sale_name,stages,product_type " +
                                                        "having product_type <>'' and stages not in('','Closed(Won)','Closed(Lost)','No go')) as s1 " +
                                                  "group by s1.[group],s1.sale_name,s1.product_type", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Home_DataModel p = new Home_DataModel()
                        {
                            group = dr["group"].ToString(),
                            sale_name = dr["sale_name"].ToString(),
                            type = dr["type"].ToString(),
                            stages = dr["stages"].ToString(),
                            mb = dr["mb"].ToString()
                        };
                        datas.Add(p);
                    }
                    dr.Close();
                }
                return datas;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public Home_DayModel getDataDay(string name)
        {
            try
            {
                Home_DayModel day = new Home_DayModel();
                SqlCommand cmd = new SqlCommand(@" select s1.sale_name,
		                                            sum(case when s1.day <= 7 then 1 else 0 end) as day_0,
		                                            sum(case when s1.day > 7 and s1.day < 14 then 1 else 0 end) as day_7,
		                                            sum(case when s1.day >= 14 and s1.day < 30 then 1 else 0 end) as day_14,
		                                            sum(case when s1.day >= 30 and s1.day < 60 then 1 else 0 end) as day_30,
		                                            sum(case when s1.day >= 60 and s1.day < 90 then 1 else 0 end) as day_60,
		                                            sum(case when s1.day > 90 then 1 else 0 end) as day_90
		                                            from (
	                                            select sale_name,
		                                            stages,
		                                            DATEDIFF(Day, stages_update_date,getDate()) as day
	                                            from Quotation 
	                                            where sale_name = '" + name + "' and stages <> '' ) as s1 " +
                                               "group by s1.sale_name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        day.sale_name = dr["sale_name"].ToString();
                        day.day_0 = dr["day_0"].ToString();
                        day.day_7 = dr["day_7"].ToString();
                        day.day_14 = dr["day_14"].ToString();
                        day.day_30 = dr["day_30"].ToString();
                        day.day_60 = dr["day_60"].ToString();
                        day.day_90 = dr["day_90"].ToString();


                    }
                    dr.Close();
                }
                return day;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<Home_StagesModel> getDataStages(string name)
        {
            try
            {
                List<Home_StagesModel> stages = new List<Home_StagesModel>();
                SqlCommand cmd = new SqlCommand(@"select sale_name,
                                                         stages,
                                                         format(sum(cast(replace(quoted_price,',','') as float))/1000000,'N2') as mb
                                                  from Quotation 
                                                  where sale_name = '" + name + "' " +
                                                 "group by sale_name,stages having stages <> ''", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Home_StagesModel p = new Home_StagesModel()
                        {
                            sale_name = dr["sale_name"].ToString(),
                            stages = dr["stages"].ToString(),
                            mb = dr["mb"].ToString()
                        };
                        stages.Add(p);
                    }
                    dr.Close();
                }
                return stages;
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
