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
        public List<Home_DataModel> getDataByIndividual(string year, string name)
        {
            try
            {
                List<Home_DataModel> datas = new List<Home_DataModel>();
                string command = string.Format($@"DECLARE @million as float
                                                  SET @million = 1000000
												select 'Type' as [group],
													sale_name as name,
													product_type as [type],
													'' as stages,
													format(sum(cast(replace(quoted_price,',','') as float))/@million,'N2') as mb,
													count(*) as cnt
                                                  from Quotation 
                                                  where sale_name='{name}' and stages_update_date like '{year}%' AND (exclude_quote is null or exclude_quote = 0) 
												  group by sale_name,product_type having product_type <>'' union all

                                                  select 'Stages' as [group],
													  sale_name as name,
													  product_type,
													  stages,
													  format(sum(cast(replace(quoted_price,',','') as float))/@million,'N2') as mb,
													  count(*) as cnt
                                                  from Quotation
                                                  where sale_name='{name}' and
													   stages_update_date like '{year}%' AND
													  (exclude_quote is null or exclude_quote = 0) and
													  product_type <>'' and stages in('Closed(Won)','Closed(Lost)','No go')
                                                  group by sale_name,stages,product_type union all

                                                  select s1.[group],
												  s1.name,
												  s1.product_type,
												  'Pending' as stages, 
												  sum(cast(s1.mb as float)) as mb,
												  sum(s1.cnt) as cnt
                                                  from (select count(*) as cnt,'Stages' as [group],sale_name as name,product_type,stages,format(sum(cast(replace(quoted_price,',','') as float))/@million,'N2') as mb
                                                        from Quotation where sale_name='{name}' and stages_update_date like '{year}%' and (exclude_quote is null or exclude_quote = 0)
                                                        group by sale_name,stages,product_type
                                                        having product_type <>'' and stages not in('Closed(Won)','Closed(Lost)','No go','Quote for Budget')) as s1
                                                  group by s1.[group],s1.name,s1.product_type union all

												  select s1.[group],
												  s1.name,
												  s1.product_type,
												  'Pending In' as stages, 
												  sum(cast(s1.mb as float)) as mb,
												  sum(s1.cnt) as cnt
                                                  from (select count(*) as cnt,'Stages' as [group],sale_name as name,product_type,stages,format(sum(cast(replace(quoted_price,',','') as float))/@million,'N2') as mb
                                                        from Quotation 
                                                        where sale_name='{name}' and stages_update_date like '{year}%'  and status = 'IN' and
                                                            product_type <>'' and 
                                                            stages not in('Closed(Won)','Closed(Lost)','No go','Quote for Budget') and 
                                                            (exclude_quote is null or exclude_quote = 0)
                                                        group by sale_name,stages,product_type) as s1
                                                  group by s1.[group],s1.name,s1.product_type union all
                                                
                                                  select 'Stages' as [group],
												  sale_name as name,
												  product_type,
												  stages,
												  format(sum(cast(replace(quoted_price,',','') as float))/@million,'N2') as mb,
												  count(*) as cnt
                                                  from Quotation where sale_name='{name}' and stages_update_date like '{year}%' and (exclude_quote is null or exclude_quote = 0)
                                                  group by sale_name,stages,product_type
                                                  having product_type <>'' and stages in('Quote for Budget')");
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Home_DataModel p = new Home_DataModel()
                        {
                            group = dr["group"].ToString(),
                            name = dr["name"].ToString(),
                            type = dr["type"].ToString(),
                            stages = dr["stages"].ToString(),
                            mb = dr["mb"].ToString(),
                            cnt = dr["cnt"].ToString()
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

        public List<Home_DataModel> getDataByDepartment(string year, string department)
        {
            try
            {
                List<Home_DataModel> datas = new List<Home_DataModel>();
                string command = "";
                if (department == "ALL")
                {
                    command = string.Format($@"DECLARE @million as float
												  SET @million = 1000000
												  select 'Type' as [group],
												  product_type as [type],
												  '' as name, 
												  '' as stages, 
												  cast(sum(cast(replace(quoted_price,',','') as float))/@million as decimal(10,2)) as mb,
												  count(*) as cnt
                                                  from Quotation 
                                                  where stages_update_date like '{year}%' AND
												    (exclude_quote is null or exclude_quote = 0) 
												  group by product_type having product_type <>'' union all

                                                  select 'Stages' as [group],
												  product_type,
												  '' as name,
												  stages,
												  cast(sum(cast(replace(quoted_price,',','') as float))/@million as decimal(10,2)) as mb,
												  count(*) as cnt
                                                  from Quotation
                                                  where stages_update_date like '{year}%' AND (exclude_quote is null or exclude_quote = 0)
                                                  group by stages,product_type having product_type <>'' and stages in('Closed(Won)','Closed(Lost)','No go') union all

                                                  select s1.[group],
												  s1.product_type,
												  '' as name,
												  'Pending' as stages, 
												  sum(cast(s1.mb as float)) as mb,
												  sum(s1.cnt) as cnt
                                                  from (select count(*) as cnt,
														  'Stages' as [group],
														  product_type,
														  stages,
														  cast(sum(cast(replace(quoted_price,',','') as float))/@million as decimal(10,2)) as mb
                                                        from Quotation where stages_update_date like '{year}%' and (exclude_quote is null or exclude_quote = 0)
                                                        group by stages,product_type
                                                        having product_type <>'' and stages not in('Closed(Won)','Closed(Lost)','No go','Quote for Budget')) as s1
                                                  group by s1.[group],s1.product_type union all

												  select s1.[group],
												  s1.product_type,
												  '' as name,
												  'Pending In' as stages, 
												  sum(cast(s1.mb as float)) as mb,
												  sum(s1.cnt) as cnt
                                                  from (select count(*) as cnt,
														  'Stages' as [group],
														  product_type,
														  stages,
														  cast(sum(cast(replace(quoted_price,',','') as float))/@million as decimal(10,2)) as mb
                                                        from Quotation where stages_update_date like '{year}%' and status = 'IN' and (exclude_quote is null or exclude_quote = 0)
                                                        group by stages,product_type
                                                        having product_type <>'' and stages not in('Closed(Won)','Closed(Lost)','No go','Quote for Budget')) as s1
                                                  group by s1.[group],s1.product_type union all

                                                  select 'Stages' as [group],
														  product_type,
														  '' as name,
														  stages,
														  cast(sum(cast(replace(quoted_price,',','') as float))/@million as decimal(10,2)) as mb,
														  count(*) as cnt
                                                        from Quotation where stages_update_date like '{year}%' and (exclude_quote is null or exclude_quote = 0)
                                                        group by stages,product_type
                                                        having product_type <>'' and stages in('Quote for Budget')");
                }
                else
                {
                    command = string.Format($@"DECLARE @million as float
												  SET @million = 1000000
												  select 'Type' as [group],
												  product_type as [type],
												  department as name, 
												  '' as stages, 
												  cast(sum(cast(replace(quoted_price,',','') as float))/@million as decimal(10,2)) as mb,
												  count(*) as cnt
                                                  from Quotation 
                                                  where department = '{department}' and stages_update_date like '{year}%' AND
												    (exclude_quote is null or exclude_quote = 0) 
												  group by department,product_type having product_type <>'' union all

                                                  select 'Stages' as [group],
												  product_type,
												  department as name,
												  stages,
												  cast(sum(cast(replace(quoted_price,',','') as float))/@million as decimal(10,2)) as mb,
												  count(*) as cnt
                                                  from Quotation
                                                  where department = '{department}' and stages_update_date like '{year}%' AND (exclude_quote is null or exclude_quote = 0)
                                                  group by department,stages,product_type having product_type <>'' and stages in('Closed(Won)','Closed(Lost)','No go') union all

                                                  select s1.[group],
												  s1.product_type,
												  s1.name,
												  'Pending' as stages, 
												  sum(cast(s1.mb as float)) as mb,
												  sum(s1.cnt) as cnt
                                                  from (select count(*) as cnt,
														  'Stages' as [group],
														  department as name,
														  product_type,
														  stages,
														  cast(sum(cast(replace(quoted_price,',','') as float))/@million as decimal(10,2)) as mb
                                                        from Quotation where department = '{department}' and stages_update_date like '{year}%' and (exclude_quote is null or exclude_quote = 0)
                                                        group by department,stages,product_type
                                                        having product_type <>'' and stages not in('Closed(Won)','Closed(Lost)','No go','Quote for Budget')) as s1
                                                  group by s1.[group],s1.name,s1.product_type union all

												  select s1.[group],
												  s1.product_type,
												  s1.name,
												  'Pending In' as stages, 
												  sum(cast(s1.mb as float)) as mb,
												  sum(s1.cnt) as cnt
                                                  from (select count(*) as cnt,
														  'Stages' as [group],
														  department as name,
														  product_type,
														  stages,
														  cast(sum(cast(replace(quoted_price,',','') as float))/@million as decimal(10,2)) as mb
                                                        from Quotation where department = '{department}' and stages_update_date like '{year}%' and status = 'IN' and (exclude_quote is null or exclude_quote = 0)
                                                        group by department,stages,product_type
                                                        having product_type <>'' and stages not in('Closed(Won)','Closed(Lost)','No go','Quote for Budget')) as s1
                                                  group by s1.[group],s1.name,s1.product_type union all

                                                  select 'Stages' as [group],
														  product_type,
														  department as name,
														  stages,
														  cast(sum(cast(replace(quoted_price,',','') as float))/@million as decimal(10,2)) as mb,
														  count(*) as cnt
                                                        from Quotation where department = '{department}' and stages_update_date like '{year}%' and (exclude_quote is null or exclude_quote = 0)
                                                        group by department,stages,product_type
                                                        having product_type <>'' and stages in('Quote for Budget')");
                }
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Home_DataModel p = new Home_DataModel()
                        {
                            group = dr["group"].ToString(),
                            name = dr["name"].ToString(),
                            type = dr["type"].ToString(),
                            stages = dr["stages"].ToString(),
                            mb = dr["mb"].ToString(),
                            cnt = dr["cnt"].ToString()
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
	                                            where sale_name = '{name}' and stages_update_date like '{year}%' and stages not in ('Closed(Won)','Closed(Lost)','No go','Quote for Budget')) as s1 
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

        public List<Home_DayModel> getDataDayByDepartment(string year, string department)
        {
            try
            {
                List<Home_DayModel> days = new List<Home_DayModel>();
                string command = "";
                if (department == "ALL")
                {
                    command = string.Format($@"select s1.department,s1.sale_name,
                                                    sum(s1.no_data) / count(s1.no_data) as no_data,
	                                                sum(case when s1.day < 7 then 1 else 0 end) as day_0,
	                                                sum(case when s1.day >= 7 and s1.day < 14 then 1 else 0 end) as day_7,
	                                                sum(case when s1.day >= 14 and s1.day < 30 then 1 else 0 end) as day_14,
	                                                sum(case when s1.day >= 30 and s1.day < 60 then 1 else 0 end) as day_30,
	                                                sum(case when s1.day >= 60 then 1 else 0 end) as day_60
	                                                from (
		                                                select Quotation.department,
			                                                Quotation.sale_name,
			                                                Quotation.stages,
			                                                s2.no_data,
			                                                DATEDIFF(Day, stages_update_date,getDate()) as day
		                                                from Quotation
		                                                LEFT JOIN (
					                                                select department,sale_name,
						                                                   sum(case when (stages is null or stages='') then 1 else 0 end) as no_data
					                                                from Quotation 
					                                                where sale_name <>'' and date like '{year}%'
					                                                group by department,sale_name) as s2
					                                                ON s2.sale_name = Quotation.sale_name
		                                                where Quotation.sale_name <>'' and stages_update_date like '{year}%' and stages not in ('Closed(Won)','Closed(Lost)','No go','Quote for Budget')) as s1 
	                                                group by s1.department,s1.sale_name
	                                                order by s1.sale_name");
                }
                else
                {
                    command = string.Format($@" select s1.department,s1.sale_name,
                                                    sum(s1.no_data) / count(s1.no_data) as no_data,
	                                                sum(case when s1.day < 7 then 1 else 0 end) as day_0,
	                                                sum(case when s1.day >= 7 and s1.day < 14 then 1 else 0 end) as day_7,
	                                                sum(case when s1.day >= 14 and s1.day < 30 then 1 else 0 end) as day_14,
	                                                sum(case when s1.day >= 30 and s1.day < 60 then 1 else 0 end) as day_30,
	                                                sum(case when s1.day >= 60 then 1 else 0 end) as day_60
	                                                from (
		                                                select Quotation.department,
			                                                Quotation.sale_name,
			                                                Quotation.stages,
			                                                s2.no_data,
			                                                DATEDIFF(Day, stages_update_date,getDate()) as day
		                                                from Quotation
		                                                LEFT JOIN (
					                                                select department,sale_name,
						                                                   sum(case when (stages is null or stages='') then 1 else 0 end) as no_data
					                                                from Quotation 
					                                                where department = '{department}' and sale_name <>'' and date like '{year}%'
					                                                group by department,sale_name) as s2
					                                                ON s2.sale_name = Quotation.sale_name
		                                                where Quotation.department = '{department}' and Quotation.sale_name <>'' and stages_update_date like '{year}%' and stages not in ('Closed(Won)','Closed(Lost)','No go','Quote for Budget')) as s1 
	                                            group by s1.department,s1.sale_name");
                }
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Home_DayModel d = new Home_DayModel()
                        {
                            sale_name = dr["sale_name"].ToString(),
                            no_data = dr["no_data"].ToString(),
                            day_0 = dr["day_0"].ToString(),
                            day_7 = dr["day_7"].ToString(),
                            day_14 = dr["day_14"].ToString(),
                            day_30 = dr["day_30"].ToString(),
                            day_60 = dr["day_60"].ToString()
                        };
                        days.Add(d);
                    }
                    dr.Close();
                }
                return days;
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
                                                    where sale_name = '{sale_name}' and stages_update_date like '{year}%' and stages not in ('','Closed(Won)','Closed(Lost)','No go','Quote for Budget') and DATEDIFF(day,stages_update_date,getDate()) {day} order by quotation_no");
                }
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Home_Stages_DayModel p = new Home_Stages_DayModel();

                        p.quotation_no = dr["quotation_no"].ToString();
                        p.project_name = dr["project_name"].ToString();
                        p.stages = dr["stages"].ToString();
                        var date = dr["stages_update_date"] != DBNull.Value ? Convert.ToDateTime(dr["stages_update_date"].ToString()).ToString("yyyy-MM-dd") : "";
                        if (date != "")
                        {                           
                            double days = (DateTime.Now - Convert.ToDateTime(date)).Days;
                            p.stages_update_date = days.ToString();
                        }
                        else
                        {
                            p.stages_update_date = "";
                        }
                       
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

        public List<Home_StagesModel> getDataStagesByIndividual(string year, string name)
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
                                                select sale_name as name,
                                                stages,
                                                format(sum(cast(replace(quoted_price,',','') as float))/ @million,'N2') as mb,
                                                count(*) as cnt
                                                from Quotation 
                                                where sale_name = @sale and CAST(YEAR(stages_update_date) AS VARCHAR(4)) = @year and ( stages <> '' and stages is not null AND (exclude_quote is null or exclude_quote = 0))
                                                group by sale_name,stages");
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Home_StagesModel p = new Home_StagesModel()
                        {
                            name = dr["name"].ToString(),
                            stages = dr["stages"].ToString(),
                            mb = dr["mb"].ToString(),
                            cnt = dr["cnt"].ToString()
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

        public List<Home_StagesModel> getDataStagesByDepartment(string year, string department)
        {
            try
            {
                List<Home_StagesModel> stages = new List<Home_StagesModel>();
                string command = "";
                if (department == "ALL")
                {
                    command = string.Format($@"declare @million as float
                                                set @million = 1000000;
                                                select '' as name,
                                                stages,
                                                cast(sum(cast(replace(quoted_price,',','') as float))/ @million as decimal(10,2)) as mb,
                                                count(*) as cnt
                                                from Quotation 
                                                where CAST(YEAR(stages_update_date) AS VARCHAR(4)) = '{year}' 
													and ( stages <> '' and stages is not null AND (exclude_quote is null or exclude_quote = 0))
                                                group by stages");
                }
                else
                {
                    command = string.Format($@"declare @million as float
                                                set @million = 1000000;
                                                select department as 'name',
                                                stages,
                                                cast(sum(cast(replace(quoted_price,',','') as float))/ @million as decimal(10,2)) as mb,
                                                count(*) as cnt
                                                from Quotation 
                                                where department = '{department}' and CAST(YEAR(stages_update_date) AS VARCHAR(4)) = '{year}' 
													and ( stages <> '' and stages is not null AND (exclude_quote is null or exclude_quote = 0))
                                                group by department,stages");
                }
               
                 
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Home_StagesModel p = new Home_StagesModel()
                        {
                            name = dr["name"].ToString(),
                            stages = dr["stages"].ToString(),
                            mb = dr["mb"].ToString(),
                            cnt = dr["cnt"].ToString()
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

        public List<SubQuotationModel> GetDataSubQuotationDepartment(string year, string department, string type)
        {
            try
            {
                List<SubQuotationModel> datas = new List<SubQuotationModel>();

                string command = "";
                if (type == "Quo. Product")
                {
                    if (department == "ALL")
                    {
                        command = string.Format($@"select * from Quotation
                                            where product_type='Product' and
                                            stages_update_date like '{year}%' AND
                                            (exclude_quote is null or exclude_quote = 0)");
                    }
                    else
                    {
                        command = string.Format($@"select * from Quotation
                                            where department='{department}' and
                                            product_type='Product' and
                                            stages_update_date like '{year}%' AND
                                            (exclude_quote is null or exclude_quote = 0)");
                    }
                    
                }
                else if (type == "Quo. Project")
                {
                    if (department == "ALL")
                    {
                        command = string.Format($@"select * from Quotation
                                            where product_type='Project' and
                                            stages_update_date like '{year}%' AND
                                            (exclude_quote is null or exclude_quote = 0)");
                    }
                    else
                    {
                        command = string.Format($@"select * from Quotation
                                            where department='{department}' and
                                            product_type='Project' and
                                            stages_update_date like '{year}%' AND
                                            (exclude_quote is null or exclude_quote = 0)");
                    }
                }
                else if (type == "Quo. Service")
                {
                    if (department == "ALL")
                    {
                        command = string.Format($@"select * from Quotation
                                            where product_type='Service' and
                                            stages_update_date like '{year}%' AND
                                            (exclude_quote is null or exclude_quote = 0)");
                    }
                    else
                    {
                        command = string.Format($@"select * from Quotation
                                            where department='{department}' and
                                            product_type='Service' and
                                            stages_update_date like '{year}%' AND
                                            (exclude_quote is null or exclude_quote = 0)");
                    }
                }
                else if (type == "WON Product")
                {
                    if (department == "ALL")
                    {
                        command = string.Format($@"select * from Quotation
                                                where product_type='Product' and
                                                stages = 'Closed(Won)' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                    else
                    {
                        command = string.Format($@"select * from Quotation
                                                where department='{department}' and
                                                product_type='Product' and
                                                stages = 'Closed(Won)' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                }
                else if (type == "WON Project")
                {
                    if (department == "ALL")
                    {
                        command = string.Format($@"select * from Quotation
                                                where product_type='Project' and
                                                stages = 'Closed(Won)' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                    else
                    {
                        command = string.Format($@"select * from Quotation
                                                where department='{department}' and
                                                product_type='Project' and
                                                stages = 'Closed(Won)' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                }
                else if (type == "WON Service")
                {
                    if (department == "ALL")
                    {
                        command = string.Format($@"select * from Quotation
                                                where product_type='Service' and
                                                stages = 'Closed(Won)' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                    else
                    {
                        command = string.Format($@"select * from Quotation
                                                where department='{department}' and
                                                product_type='Service' and
                                                stages = 'Closed(Won)' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                }
                else if (type == "Product In")
                {
                    if (department == "ALL")
                    {
                        command = string.Format($@"select * from Quotation
                                                where product_type='Product' and
                                                status = 'IN' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                    else
                    {
                        command = string.Format($@"select * from Quotation
                                                where department='{department}' and
                                                product_type='Product' and
                                                status = 'IN' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                }
                else if (type == "Project In")
                {
                    if (department == "ALL")
                    {
                        command = string.Format($@"select * from Quotation
                                                where product_type='Project' and
                                                status = 'IN' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                    else
                    {
                        command = string.Format($@"select * from Quotation
                                                where department='{department}' and
                                                product_type='Project' and
                                                status = 'IN' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                }
                else if (type == "Service In")
                {
                    if (department == "ALL")
                    {
                        command = string.Format($@"select * from Quotation
                                                where product_type='Service' and
                                                status = 'IN' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                    else
                    {
                        command = string.Format($@"select * from Quotation
                                                where department='{department}' and
                                                product_type='Service' and
                                                status = 'IN' and
                                                stages_update_date like '{year}%' AND
                                                (exclude_quote is null or exclude_quote = 0)");
                    }
                }
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SubQuotationModel s = new SubQuotationModel()
                        {
                            sale_name = dr["sale_name"].ToString(),
                            department = dr["department"].ToString(),
                            quotation_no = dr["quotation_no"].ToString(),
                            //customer = dr["customer"].ToString(),
                            //project_name = dr["project_name"].ToString(),
                            product_type = dr["product_type"].ToString(),
                            quoted_price = dr["quoted_price"].ToString(),
                            status = dr["status"].ToString(),
                            stages = dr["stages"].ToString()
                        };
                        datas.Add(s);
                    }
                    dr.Close();
                }
                ConnectSQL.CloseConnect();
                return datas;
            }
            catch
            {
                return new List<SubQuotationModel>() { };
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<SubQuotationModel> GetDataSubQuotationIndividual(string year, string name, string type)
        {
            try
            {
                List<SubQuotationModel> datas = new List<SubQuotationModel>();

                string command = "";
                if (type == "Quo. Product")
                {
                    command = string.Format($@"select * from Quotation
                                            where sale_name='{name}' and
                                            product_type='Product' and
                                            stages_update_date like '{year}%' AND
                                            (exclude_quote is null or exclude_quote = 0)");
                }
                else if(type == "Quo. Project")
                {
                    command = string.Format($@"select * from Quotation
                                            where sale_name='{name}' and
                                            product_type='Project' and
                                            stages_update_date like '{year}%' AND
                                            (exclude_quote is null or exclude_quote = 0)");
                }
                else if (type == "Quo. Service")
                {
                    command = string.Format($@"select * from Quotation
                                            where sale_name='{name}' and
                                            product_type='Service' and
                                            stages_update_date like '{year}%' AND
                                            (exclude_quote is null or exclude_quote = 0)");
                }
                else if (type == "WON Product")
                {
                    command = string.Format($@"select * from Quotation 
                                            where sale_name='{name}' and
                                             product_type='Product' and
                                             stages = 'Closed(Won)' and
                                             stages_update_date like '{year}%' AND
                                             (exclude_quote is null or exclude_quote = 0)");
                }
                else if (type == "WON Project")
                {
                    command = string.Format($@"select * from Quotation 
                                            where sale_name='{name}' and
                                             product_type='Project' and
                                             stages = 'Closed(Won)' and
                                             stages_update_date like '{year}%' AND
                                             (exclude_quote is null or exclude_quote = 0)");
                }
                else if (type == "WON Service")
                {
                    command = string.Format($@"select * from Quotation 
                                            where sale_name='{name}' and
                                             product_type='Service' and
                                             stages = 'Closed(Won)' and
                                             stages_update_date like '{year}%' AND
                                             (exclude_quote is null or exclude_quote = 0)");
                }
                else if (type == "Product In")
                {
                    command = string.Format($@"select * from Quotation 
                                                where sale_name='{name}' and
                                                 product_type='Product' and
                                                 status = 'IN' and
                                                 stages_update_date like '{year}%' AND
                                                 (exclude_quote is null or exclude_quote = 0)");
                }
                else if (type == "Project In")
                {
                    command = string.Format($@"select * from Quotation 
                                                where sale_name='{name}' and
                                                 product_type='Project' and
                                                 status = 'IN' and
                                                 stages_update_date like '{year}%' AND
                                                 (exclude_quote is null or exclude_quote = 0)");
                }
                else if (type == "Service In")
                {
                    command = string.Format($@"select * from Quotation 
                                                where sale_name='{name}' and
                                                 product_type='Service' and
                                                 status = 'IN' and
                                                 stages_update_date like '{year}%' AND
                                                 (exclude_quote is null or exclude_quote = 0)");
                }
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SubQuotationModel s = new SubQuotationModel()
                        {
                            sale_name = dr["sale_name"].ToString(),
                            department = dr["department"].ToString(),
                            quotation_no = dr["quotation_no"].ToString(),
                            //customer = dr["customer"].ToString(),
                            //project_name = dr["project_name"].ToString(),
                            product_type = dr["product_type"].ToString(),
                            quoted_price = dr["quoted_price"].ToString(),
                            status = dr["status"].ToString(),
                            stages = dr["stages"].ToString()
                        };
                        datas.Add(s);
                    }
                    dr.Close();
                }
                ConnectSQL.CloseConnect();
                return datas;
            }
            catch
            {
                return new List<SubQuotationModel>() { };
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<HittingRateModel> GetHittingRateByDepartment(string year,string department)
        {
            try
            {
                List<HittingRateModel> hittingRates = new List<HittingRateModel>();
                string command = "";
                if (department == "ALL")
                {
                    command = string.Format($@"   DECLARE @million as float
                                                     SET @million = 1000000;
	                                                    with main as (select product_type,sum(case when stages = 'Closed(Won)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as win,
		                                                    sum(case when stages = 'Closed(Lost)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as lose,
		                                                    sum(case when stages = 'No go' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as nogo,
		                                                    sum(case when stages in ('Closed(Won)','Closed(Lost)','No go') then cast(replace(quoted_price,',','') as float) / @million else 0 end) as total
	                                                    from Quotation
	                                                    where product_type <>'' and stages_update_date like '{year}%' and product_type is not null and stages in ('Closed(Won)','Closed(Lost)','No go') AND (exclude_quote is null or exclude_quote = 0)
	                                                    group by product_type
	                                                    ), sub as (
		                                                    select 'all' as product_type,sum(case when stages = 'Closed(Won)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as win,
			                                                    sum(case when stages = 'Closed(Lost)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as lose,
			                                                    sum(case when stages = 'No go' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as nogo,
			                                                    sum(case when stages in ('Closed(Won)','Closed(Lost)','No go') then cast(replace(quoted_price,',','') as float) / @million else 0 end) as total
		                                                    from Quotation
		                                                    where product_type <>'' and stages_update_date like '{year}%' and product_type is not null and stages in ('Closed(Won)','Closed(Lost)','No go') AND (exclude_quote is null or exclude_quote = 0)
	                                                    )
	                                                    select main.product_type as type,
	                                                    cast(((main.win / main.total)*100)as decimal(10, 1)) as hitting_rate
	                                                    from main union all
	                                                    select 'Total' as type,
	                                                    cast(((sub.win / sub.total)*100)as decimal(10, 1)) as hitting_rate
	                                                    from sub");
                }
                else
                {
                    command = string.Format($@"   DECLARE @million as float
                                                     SET @million = 1000000;
	                                                    with main as (select product_type,sum(case when stages = 'Closed(Won)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as win,
		                                                    sum(case when stages = 'Closed(Lost)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as lose,
		                                                    sum(case when stages = 'No go' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as nogo,
		                                                    sum(case when stages in ('Closed(Won)','Closed(Lost)','No go') then cast(replace(quoted_price,',','') as float) / @million else 0 end) as total
	                                                    from Quotation
	                                                    where department= '{department}' and product_type <>'' and stages_update_date like '{year}%' and product_type is not null and stages in ('Closed(Won)','Closed(Lost)','No go') AND (exclude_quote is null or exclude_quote = 0)
	                                                    group by product_type
	                                                    ), sub as (
		                                                    select 'all' as product_type,sum(case when stages = 'Closed(Won)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as win,
			                                                    sum(case when stages = 'Closed(Lost)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as lose,
			                                                    sum(case when stages = 'No go' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as nogo,
			                                                    sum(case when stages in ('Closed(Won)','Closed(Lost)','No go') then cast(replace(quoted_price,',','') as float) / @million else 0 end) as total
		                                                    from Quotation
		                                                    where department= '{department}' and product_type <>'' and stages_update_date like '{year}%' and product_type is not null and stages in ('Closed(Won)','Closed(Lost)','No go') AND (exclude_quote is null or exclude_quote = 0)
	                                                    )
	                                                    select main.product_type as type,
	                                                    cast(((main.win / main.total)*100)as decimal(10, 1)) as hitting_rate
	                                                    from main union all
	                                                    select 'Total' as type,
	                                                    cast(((sub.win / sub.total)*100)as decimal(10, 1)) as hitting_rate
	                                                    from sub");
                }
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        HittingRateModel p = new HittingRateModel()
                        {
                            type = dr["type"].ToString(),
                            hitting_rate = dr["hitting_rate"] != DBNull.Value ? double.Parse(dr["hitting_rate"].ToString()) :0.0
                        };
                        hittingRates.Add(p);
                    }
                    dr.Close();
                }
                return hittingRates;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<HittingRateModel> GetHittingRateByName(string year,string name)
        {
            try
            {
                List<HittingRateModel> hittingRates = new List<HittingRateModel>();
                string command = string.Format($@"   DECLARE @million as float
                                                     SET @million = 1000000;
	                                                    with main as (select product_type,sum(case when stages = 'Closed(Won)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as win,
		                                                    sum(case when stages = 'Closed(Lost)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as lose,
		                                                    sum(case when stages = 'No go' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as nogo,
		                                                    sum(case when stages in ('Closed(Won)','Closed(Lost)','No go') then cast(replace(quoted_price,',','') as float) / @million else 0 end) as total
	                                                    from Quotation
	                                                    where sale_name= '{name}' and stages_update_date like '{year}%' and product_type <>'' and product_type is not null and stages in ('Closed(Won)','Closed(Lost)','No go') AND (exclude_quote is null or exclude_quote = 0)
	                                                    group by product_type
	                                                    ), sub as (
		                                                    select 'all' as product_type,sum(case when stages = 'Closed(Won)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as win,
			                                                    sum(case when stages = 'Closed(Lost)' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as lose,
			                                                    sum(case when stages = 'No go' then cast(replace(quoted_price,',','') as float) / @million else 0 end) as nogo,
			                                                    sum(case when stages in ('Closed(Won)','Closed(Lost)','No go') then cast(replace(quoted_price,',','') as float) / @million else 0 end) as total
		                                                    from Quotation
		                                                    where sale_name= '{name}' and stages_update_date like '{year}%' and product_type <>'' and product_type is not null and stages in ('Closed(Won)','Closed(Lost)','No go') AND (exclude_quote is null or exclude_quote = 0)
	                                                    )
	                                                    select main.product_type as type,
	                                                    cast((case when main.total <> 0 then (main.win / main.total)*100 else 0 end) as decimal(10, 1)) as hitting_rate
	                                                    from main union all
	                                                    select 'Total' as type,
	                                                    cast((case when sub.total <> 0 then (sub.win / sub.total)*100 else 0 end) as decimal(10, 1)) as hitting_rate
	                                                    from sub");
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        HittingRateModel p = new HittingRateModel()
                        {
                            type = dr["type"].ToString(),
                            hitting_rate = dr["hitting_rate"] != DBNull.Value ? double.Parse(dr["hitting_rate"].ToString()) : 0.0
                        };
                        hittingRates.Add(p);
                    }
                    dr.Close();
                }
                return hittingRates;
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
                                                 where department = '" + department + "' and stages_update_date like '" + year + "%' and stages ='Closed(Won)' AND (exclude_quote is null or exclude_quote = 0) " +
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
                                                 "where department = '" + department + "' and stages_update_date like '" + year + "%' and stages ='Closed(Won)' AND (exclude_quote is null or exclude_quote = 0) " +
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
                                                 where department = '" + department + "' and stages_update_date like '" + year + "%' and stages ='Closed(Won)' AND (exclude_quote is null or exclude_quote = 0) " +
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

        public TargetDepartment GetTargetDepartment(string year, string department)
        {
            try
            {
                TargetDepartment target = new TargetDepartment();

                string command = "";
                if (department == "ALL")
                {
                    command = string.Format($@"select 'ALL' as department,
                                                sum(cast(product as float)) as product,
                                                sum(cast(project as float)) as project,
                                                sum(cast(service as float)) as service
                                               from Target");
                }
                else
                {
                    command = string.Format($@"select department,
	                                                sum(cast(product as float)) as product,
	                                                sum(cast(project as float)) as project,
	                                                sum(cast(service as float)) as service
                                                from Target group by department,year having department='{department}' and year='{year}'");
                }
                
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        target.department = dr["department"].ToString();
                        target.product = dr["product"].ToString();
                        target.project = dr["project"].ToString();
                        target.service = dr["service"].ToString();
                    }
                    dr.Close();
                }
                ConnectSQL.CloseConnect();
                return target;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public TargetIndividual GetTargetIndividual(string year, string name)
        {
            try
            {
                TargetIndividual target = new TargetIndividual();
                string command = string.Format($@"select * from Target where sale_name='{name}' and year='{year}'");
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        target.sale_name = dr["sale_name"].ToString();
                        target.product = dr["product"].ToString();
                        target.project = dr["project"].ToString();
                        target.service = dr["service"].ToString();                                              
                    }
                    dr.Close();
                }
                ConnectSQL.CloseConnect();
                return target;
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
