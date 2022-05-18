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
        public List<Home_DataModel> getData(string year, string name)
        {
            try
            {
                List<Home_DataModel> datas = new List<Home_DataModel>();
                SqlCommand cmd = new SqlCommand(@"select 'Type' as [group],sale_name,product_type as [type], '' as stages, format(sum(cast(replace(quoted_price,',','') as float))/1000000,'N2') as mb
                                                  from Quotation where sale_name='" + name + "' and stages_update_date like '" + year + "%' group by sale_name,product_type having product_type <>'' union all " +

                                                  "select 'Stages' as [group],sale_name,product_type,stages,format(sum(cast(replace(quoted_price,',','') as float))/1000000,'N2') as mb " +
                                                  "from Quotation " +
                                                  "where sale_name='" + name + "' and stages_update_date like '" + year + "%' " +
                                                  "group by sale_name,stages,product_type having product_type <>'' and stages in('Closed(Won)','Closed(Lost)','No go') union all " +

                                                  "select s1.[group],s1.sale_name,s1.product_type,'Pending' as stages, sum(cast(s1.mb as float)) as mb " +
                                                  "from (select 'Stages' as [group],sale_name,product_type,stages,format(sum(cast(replace(quoted_price,',','') as float))/1000000,'N2') as mb " +
                                                        "from Quotation where sale_name='" + name + "' and stages_update_date like '" + year + "%' " +
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

        public Home_DayModel getDataDay(string year, string name)
        {
            try
            {
                Home_DayModel day = new Home_DayModel();
                SqlCommand cmd = new SqlCommand(string.Format($@" select s1.sale_name,
                                                    (select sum(case when (stages is null or stages='') then 1 else 0 end) as no_data from Quotation where sale_name = '{name}') as no_data,
		                                            sum(case when s1.day < 7 then 1 else 0 end) as day_0,
		                                            sum(case when s1.day >= 7 and s1.day < 14 then 1 else 0 end) as day_7,
		                                            sum(case when s1.day >= 14 and s1.day < 30 then 1 else 0 end) as day_14,
		                                            sum(case when s1.day >= 30 and s1.day < 60 then 1 else 0 end) as day_30,
		                                            sum(case when s1.day >= 60 then 1 else 0 end) as day_60
		                                            from (
	                                            select sale_name,
		                                            stages,
		                                            DATEDIFF(Day, stages_update_date,getDate()) as day
	                                            from Quotation 
	                                            where sale_name = '{name}' and stages_update_date like '{year}%' and stages not in ('Closed(Won)','Closed(Lost)','No go')) as s1 
                                                group by s1.sale_name"), ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        day.sale_name = dr["sale_name"].ToString();
                        day.no_data = dr["no_data"].ToString();
                        day.day_0 = dr["day_0"].ToString();
                        day.day_7 = dr["day_7"].ToString();
                        day.day_14 = dr["day_14"].ToString();
                        day.day_30 = dr["day_30"].ToString();
                        day.day_60 = dr["day_60"].ToString();
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

        public List<Home_Stages_DayModel> getDataQuotationMoreDay(string year, string sale_name, string day)
        {
            try
            {
                List<Home_Stages_DayModel> stages = new List<Home_Stages_DayModel>();
                string command = "";
                if (day == "no data")
                {
                    command = string.Format($@"select quotation_no, project_name,
                                               stages,
                                               stages_update_date
                                               from Quotation 
                                               where sale_name = '{sale_name}' and (stages is null or stages = '')");
                }
                else
                {
                    command = string.Format($@"select quotation_no,
                                                    project_name,
                                                    stages,
                                                    stages_update_date 
                                                    from Quotation 
                                                    where sale_name = '{sale_name}' and stages_update_date like '{year}%' and stages not in ('','Closed(Won)','Closed(Lost)','No go') and DATEDIFF(day,stages_update_date,getDate()) {day} order by quotation_no");
                }
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Home_Stages_DayModel p = new Home_Stages_DayModel()
                        {
                            quotation_no = dr["quotation_no"].ToString(),
                            project_name = dr["project_name"].ToString(),
                            stages = dr["stages"].ToString(),
                            stages_update_date = dr["stages_update_date"] != DBNull.Value ? Convert.ToDateTime(dr["stages_update_date"].ToString()).ToString("yyyy-MM-dd") : "",
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

        public List<Home_StagesModel> getDataStages(string year, string name)
        {
            try
            {
                List<Home_StagesModel> stages = new List<Home_StagesModel>();
                string command = string.Format($@"declare @sale nvarchar(30)
                                                declare @year nvarchar(10)
                                                declare @million as float
                                                set @sale = '{name}';
                                                set @year = '{year}';
                                                set @million = 1000000;
                                                select sale_name,
                                                stages,
                                                format(sum(cast(replace(quoted_price,',','') as float))/ @million,'N2') as mb
                                                from Quotation 
                                                where sale_name = @sale and CAST(YEAR(stages_update_date) AS VARCHAR(4)) = @year and ( stages <> '' and stages is not null)
                                                group by sale_name,stages");
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
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

        public List<PerformanceModel> getPerformance(string year, string department)
        {
            try
            {
                List<PerformanceModel> targetPerformances = new List<PerformanceModel>();
                SqlCommand cmd = new SqlCommand(@"with s As(select s1.department,s1.sale_name,
                                                 s2.product as product_target,
                                                 s1.product_actual,
                                                 s2.project as project_target,
                                                 s1.project_actual,
                                                 s2.service as service_target,
                                                 s1.service_actual
                                                 from (select department,sale_name,
                                                 format(sum(case when product_type='product' then case when quoted_price is not null then cast(replace(quoted_price,',','') as float) else 0 end else 0 end) / 1000000 ,'N3') as product_actual,
                                                 format(sum(case when product_type='prject' then case when quoted_price is not null then cast(replace(quoted_price,',','') as float) else 0 end else 0 end) / 1000000,'N3') as project_actual,
                                                 format(sum(case when product_type='service' then case when quoted_price is not null then cast(replace(quoted_price,',','') as float) else 0 end else 0 end) / 1000000,'N3') as service_actual
                                                 from Quotation 
                                                 where department = '" + department + "' and stages_update_date like '" + year + "%' and stages ='Closed(Won)' " +
                                                 "group by department, sale_name) as s1 " +
                                                 "left join (select department,sale_name, " +
                                                 "sum(cast(product as float)) as product, " +
                                                 "sum(cast(project as float)) as project, " +
                                                 "sum(cast(service as float)) as service " +
                                                 "from Target where [year] = '" + year + "' group by department, sale_name) as s2 ON s1.sale_name = s2.sale_name union all " +

                                                 "select 'Total' as department,'' as sale_name, " +
                                                 "s2.product as product_target, " +
                                                 "s1.product_actual, " +
                                                 "s2.project as project_target, " +
                                                 "s1.project_actual, " +
                                                 "s2.service as service_target, " +
                                                 "s1.service_actual " +
                                                 "from (select department,'' as sale_name, " +
                                                 "format(sum(case when product_type = 'product' then case when quoted_price is not null then cast(replace(quoted_price, ',', '') as float) else 0 end else 0 end) / 1000000 ,'N3') as product_actual, " +
                                                 "format(sum(case when product_type = 'prject' then case when quoted_price is not null then cast(replace(quoted_price, ',', '') as float) else 0 end else 0 end) / 1000000 ,'N3') as project_actual, " +
                                                 "format(sum(case when product_type = 'service' then case when quoted_price is not null then cast(replace(quoted_price, ',', '') as float) else 0 end else 0 end) / 1000000 ,'N3') as service_actual " +
                                                 "from Quotation " +
                                                 "where department = '" + department + "' and stages_update_date like '" + year + "%' and stages ='Closed(Won)' " +
                                                 "group by department) as s1 " +
                                                 "left join  " +
                                                 "(select department, " +
                                                 "sum(cast(product as float)) as product, " +
                                                 "sum(cast(project as float)) as project, " +
                                                 "sum(cast(service as float)) as service " +
                                                 "from [Target] where [year]='" + year + "' group by department) as s2 ON s1.department = s2.department) " +
                                                 "select * from s order by s.department,s.sale_name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PerformanceModel p = new PerformanceModel()
                        {
                            department = dr["department"].ToString(),
                            sale_name = dr["sale_name"].ToString(),
                            product_target = dr["product_target"].ToString(),
                            product_actual = dr["product_actual"].ToString(),
                            project_target = dr["project_target"].ToString(),
                            project_actual = dr["project_actual"].ToString(),
                            service_target = dr["service_target"].ToString(),
                            service_actual = dr["service_actual"].ToString()
                        };
                        targetPerformances.Add(p);
                    }
                    dr.Close();
                }
                return targetPerformances;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<PerformanceModel> getPerformanceStack(string year, string department)
        {
            try
            {
                List<PerformanceModel> targetPerformances = new List<PerformanceModel>();
                SqlCommand cmd = new SqlCommand(@"select s1.department,s1.sale_name,
                                                 s2.product as product_target,
                                                 s1.product_actual,
                                                 s2.project as project_target,
                                                 s1.project_actual,
                                                 s2.service as service_target,
                                                 s1.service_actual
                                                 from (select department,sale_name,
                                                 format(sum(case when product_type='product' then case when quoted_price is not null then cast(replace(quoted_price,',','') as float) else 0 end else 0 end) / 1000000 ,'N3') as product_actual,
                                                 format(sum(case when product_type='prject' then case when quoted_price is not null then cast(replace(quoted_price,',','') as float) else 0 end else 0 end) / 1000000,'N3') as project_actual,
                                                 format(sum(case when product_type='service' then case when quoted_price is not null then cast(replace(quoted_price,',','') as float) else 0 end else 0 end) / 1000000,'N3') as service_actual
                                                 from Quotation 
                                                 where department = '" + department + "' and stages_update_date like '" + year + "%' and stages ='Closed(Won)' " +
                                                 "group by department, sale_name) as s1 " +
                                                 "left join (select department,sale_name, " +
                                                 "sum(cast(product as float)) as product, " +
                                                 "sum(cast(project as float)) as project, " +
                                                 "sum(cast(service as float)) as service " +
                                                 "from Target where [year] = '" + year + "' group by department, sale_name) as s2 ON s1.sale_name = s2.sale_name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PerformanceModel p = new PerformanceModel()
                        {
                            department = dr["department"].ToString(),
                            sale_name = dr["sale_name"].ToString(),
                            product_target = dr["product_target"].ToString(),
                            product_actual = dr["product_actual"].ToString(),
                            project_target = dr["project_target"].ToString(),
                            project_actual = dr["project_actual"].ToString(),
                            service_target = dr["service_target"].ToString(),
                            service_actual = dr["service_actual"].ToString()
                        };
                        targetPerformances.Add(p);
                    }
                    dr.Close();
                }
                return targetPerformances;
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
