﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;

namespace WebForecastReport.Service
{
    public class Quotation_ReportService : IQuotation_Report
    {
        public List<Quotation_Report_DepartmentModel> GetReportDepartment(string department, string month_first, string month_last)
        {
            try
            {
                List<Quotation_Report_DepartmentModel> reports = new List<Quotation_Report_DepartmentModel>();
                string command = "";
                string command_sub_type = "";
                string command_sub_stages = "";
                string command_sub_pending = "";

                if (department == "ALL")
                {
                    command = string.Format($@"with s1 as(select department,sale_name as sale,
                                                    cast(sum(sum(cast(replace(quoted_price,',','') as float))/1000000) over (partition by sale_name) as decimal(10,2)) as quo_mb,
                                                    count(quotation_no) as quo_cnt,
                                                    sum(case when product_type ='product' then 1 else 0 end) as product_cnt,
													sum(case when product_type ='product' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as product_mb,
                                                    sum(case when product_type ='project' then 1 else 0 end) as project_cnt,
													sum(case when product_type ='project' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as project_mb,
                                                    sum(case when product_type ='service' then 1 else 0 end) as service_cnt,
													sum(case when product_type ='service' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as service_mb,
                                                    sum(case when stages='Closed(Won)' then 1 else 0 end) as won_quo_cnt,
                                                    sum(case when stages='Closed(Won)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as won_mb,
                                                    sum(case when stages='Closed(Lost)' then 1 else 0 end) as loss_quo_cnt,
                                                    sum(case when stages='Closed(Lost)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as loss_mb,
                                                    sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt,
                                                    sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb,
                                                    sum(case when stages is null or stages not in('','Closed(Won)','Closed(Lost)','No go') then 1 else 0 end) as pending_quo_cnt,
                                                    sum(case when stages is null or stages not in('','Closed(Won)','Closed(Lost)','No go') then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as pending_mb
                                                    from Quotation where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' group by department,sale_name union all

                                                    select (department + ' Total') as department,
                                                    '' as sale,
                                                    cast(sum(sum(cast(replace(quoted_price,',','') as float))/1000000) over (partition by department) as decimal(10,2)) as quo_mb,
                                                    count(quotation_no) as quo_cnt,
                                                    sum(case when product_type ='product' then 1 else 0 end) as product_cnt,
                                                    sum(case when product_type ='product' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as product_mb,
                                                    sum(case when product_type ='project' then 1 else 0 end) as project_cnt,
                                                    sum(case when product_type ='project' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as project_mb,
                                                    sum(case when product_type ='service' then 1 else 0 end) as service_cnt,
                                                    sum(case when product_type ='service' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as service_mb,
                                                    sum(case when stages='Closed(Won)' then 1 else 0 end) as won_quo_cnt,
                                                    sum(case when stages='Closed(Won)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as won_mb,
                                                    sum(case when stages='Closed(Lost)' then 1 else 0 end) as loss_quo_cnt,
                                                    sum(case when stages='Closed(Lost)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as loss_mb,
                                                    sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt,
                                                    sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb,
                                                    sum(case when stages is null or stages not in ('','Closed(Won)', 'Closed(Lost)', 'No go') then 1 else 0 end) as pending_quo_cnt,
                                                    sum(case when stages is null or stages not in ('','Closed(Won)', 'Closed(Lost)', 'No go') then cast(cast(replace(quoted_price, ',', '') as float) / 1000000 as decimal(10, 2)) else 0 end) as pending_mb
                                                    from Quotation where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' group by department union all

                                                    select ('Total') as department,
                                                    '' as sale,
                                                    cast(sum(cast(replace(quoted_price,',','') as float))/1000000 as decimal(10,2)) as quo_mb,
                                                    count(quotation_no)as quo_cnt,
                                                    sum(case when product_type ='product' then 1 else 0 end) as product_cnt,
                                                    sum(case when product_type ='product' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as product_mb,
                                                    sum(case when product_type ='project' then 1 else 0 end) as project_cnt,
                                                    sum(case when product_type ='project' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as project_mb,
                                                    sum(case when product_type ='service' then 1 else 0 end) as service_cnt,
                                                    sum(case when product_type ='service' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as service_mb,
                                                    sum(case when stages='Closed(Won)' then 1 else 0 end) as won_quo_cnt,
                                                    sum(case when stages='Closed(Won)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as won_mb,
                                                    sum(case when stages='Closed(Lost)' then 1 else 0 end) as loss_quo_cnt,
                                                    sum(case when stages='Closed(Lost)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as loss_mb,
                                                    sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt,
                                                    sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb,
                                                    sum(case when stages is null or stages not in ('','Closed(Won)', 'Closed(Lost)', 'No go') then 1 else 0 end) as pending_quo_cnt,
                                                    sum(case when stages is null or stages not in ('','Closed(Won)', 'Closed(Lost)', 'No go') then cast(cast(replace(quoted_price, ',', '') as float) / 1000000 as decimal(10, 2)) else 0 end) as pending_mb
                                                    from Quotation where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}')
                                                    select * from s1 order by s1.department,s1.sale");

                    command_sub_type = string.Format($@"with x As(select department,
	                                                    sale_name,
	                                                    product_type,
	                                                    sum(case when stages ='Closed(Won)' then 1 else 0 end) as type_won_cnt,
	                                                    format(sum(case when stages ='Closed(Won)' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_won_mb,
	                                                    sum(case when stages ='Closed(Lost)' then 1 else 0 end) as type_lost_cnt,
	                                                    format(sum(case when stages ='Closed(Lost)' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_lost_mb,
	                                                    sum(case when stages ='No go' then 1 else 0 end) as type_nogo_cnt,
	                                                    format(sum(case when stages ='No go' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_nogo_mb,
	                                                    sum(case when stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go') then 1 else 0 end) as type_pending_cnt,
	                                                    format(sum(case when stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go') then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_pending_mb
                                                    from Quotation 
                                                    where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and product_type <> ''
                                                    group by department,sale_name,product_type
                                                    union all

                                                    select department,
	                                                    sale_name + ' Total' as sale_name,
	                                                    '' as product_type,
	                                                    sum(case when stages ='Closed(Won)' then 1 else 0 end) as type_won_cnt,
	                                                    format(sum(case when stages ='Closed(Won)' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_won_mb,
	                                                    sum(case when stages ='Closed(Lost)' then 1 else 0 end) as type_lost_cnt,
	                                                    format(sum(case when stages ='Closed(Lost)' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_lost_mb,
	                                                    sum(case when stages ='No go' then 1 else 0 end) as type_nogo_cnt,
	                                                    format(sum(case when stages ='No go' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_nogo_mb,
	                                                    sum(case when stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go') then 1 else 0 end) as type_pending_cnt,
	                                                    format(sum(case when stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go') then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_pending_mb
                                                    from Quotation 
                                                    where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and product_type <> ''
                                                    group by department,sale_name

                                                    union all

                                                    select department + ' Total' as department,
	                                                    '' as sale_name,
	                                                    '' as product_type,
	                                                    sum(case when stages ='Closed(Won)' then 1 else 0 end) as type_won_cnt,
	                                                    format(sum(case when stages ='Closed(Won)' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_won_mb,
	                                                    sum(case when stages ='Closed(Lost)' then 1 else 0 end) as type_lost_cnt,
	                                                    format(sum(case when stages ='Closed(Lost)' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_lost_mb,
	                                                    sum(case when stages ='No go' then 1 else 0 end) as type_nogo_cnt,
	                                                    format(sum(case when stages ='No go' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_nogo_mb,
	                                                    sum(case when stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go') then 1 else 0 end) as type_pending_cnt,
	                                                    format(sum(case when stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go') then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_pending_mb
                                                    from Quotation 
                                                    where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and product_type <> ''
                                                    group by department)

                                                    select * from x order by x.department,x.sale_name,x.product_type");
                    command_sub_stages = string.Format($@"with x As(select department,
                                                            sale_name,
                                                            stages,
                                                            sum(case when product_type ='Project' then 1 else 0 end) as stages_project_cnt,
                                                            format(sum(case when product_type ='Project' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_project_mb,
                                                            sum(case when product_type ='Product' then 1 else 0 end) as stages_product_cnt,
                                                            format(sum(case when product_type ='Product' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_product_mb,
                                                            sum(case when product_type ='Service' then 1 else 0 end) as stages_service_cnt,
                                                            format(sum(case when product_type ='Service' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_service_mb
                                                            from Quotation

                                                             where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and (stages <> '' and stages is not null) and stages in('Closed(Won)','Closed(Lost)','No go')
                                                             group by department,sale_name,stages

                                                             union all

                                                            select department,
                                                            sale_name + '_Total' as sale_name,
                                                            '' as stages,
                                                            sum(case when product_type ='Project' then 1 else 0 end) as stages_project_cnt,
                                                            format(sum(case when product_type ='Project' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_project_mb,
                                                            sum(case when product_type ='Product' then 1 else 0 end) as stages_product_cnt,
                                                            format(sum(case when product_type ='Product' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_product_mb,
                                                            sum(case when product_type ='Service' then 1 else 0 end) as stages_service_cnt,
                                                            format(sum(case when product_type ='Service' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_service_mb
                                                            from Quotation

                                                             where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and (stages <> '' and stages is not null) and stages in('Closed(Won)','Closed(Lost)','No go')
                                                             group by department,sale_name

                                                             union all

                                                             select department + '_Total' as department,
                                                            '' as sale_name,
                                                            '' as stages,
                                                            sum(case when product_type ='Project' then 1 else 0 end) as stages_project_cnt,
                                                            format(sum(case when product_type ='Project' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_project_mb,
                                                            sum(case when product_type ='Product' then 1 else 0 end) as stages_product_cnt,
                                                            format(sum(case when product_type ='Product' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_product_mb,
                                                            sum(case when product_type ='Service' then 1 else 0 end) as stages_service_cnt,
                                                            format(sum(case when product_type ='Service' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_service_mb
                                                            from Quotation

                                                             where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and (stages <> '' and stages is not null) and stages in('Closed(Won)','Closed(Lost)','No go')
                                                             group by department)

                                                             select * from x order by x.department,x.sale_name");
                    command_sub_pending = string.Format($@"with x As(select sub_stages.department,
	                                                        sub_stages.sale_name,
	                                                        'pending' as stages,
	                                                        sub_stages.product_type,
	                                                        sum(cast(sub_stages.stages_pending_cnt as float)) as stages_pending_cnt,
	                                                        format(sum(cast(sub_stages.stages_pending_mb as float)),'N2') as stages_pending_mb 
	                                                        from (
		                                                        select department,
		                                                        sale_name,
		                                                        'Pending' as stages,
		                                                        product_type,
		                                                        sum(case when product_type is not null or product_type not in ('','Project','Product','Service') then 1 else 0 end) as stages_pending_cnt,
		                                                        sum(case when product_type is not null or product_type not in ('','Project','Product','Service') then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end) as stages_pending_mb
		                                                        from Quotation 
		                                                        where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and stages <> '' 
		                                                        group by department,sale_name,product_type,stages having stages is not null and stages not in('','Closed(Won)','Closed(Lost)','No go')) as sub_stages

                                                        group by sub_stages.department,sub_stages.sale_name,sub_stages.product_type

                                                        union all

                                                        select sub_stages.department,
	                                                        sub_stages.sale_name,
	                                                        'pending Total' as stages,
	                                                        sub_stages.product_type,
	                                                        sum(cast(sub_stages.stages_pending_cnt as float)) as stages_pending_cnt,
	                                                        format(sum(cast(sub_stages.stages_pending_mb as float)),'N2') as stages_pending_mb 
	                                                        from (
		                                                        select department,
		                                                        sale_name,
		                                                        'Pending Total' as stages,
		                                                        '' as product_type,
		                                                        sum(case when product_type is not null or product_type not in ('','Project','Product','Service') then 1 else 0 end) as stages_pending_cnt,
		                                                        sum(case when product_type is not null or product_type not in ('','Project','Product','Service') then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end) as stages_pending_mb
		                                                        from Quotation 
		                                                        where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and stages <> '' 
		                                                        group by department,sale_name,stages having stages is not null and stages not in('','Closed(Won)','Closed(Lost)','No go')) as sub_stages

                                                        group by sub_stages.department,sub_stages.sale_name,sub_stages.product_type

                                                        union all

                                                        select sub_stages.department + ' Total' as department,
	                                                        '' as sale_name,
	                                                        '' as stages,
	                                                        sub_stages.product_type,
	                                                        sum(cast(sub_stages.stages_pending_cnt as float)) as stages_pending_cnt,
	                                                        format(sum(cast(sub_stages.stages_pending_mb as float)),'N2') as stages_pending_mb 
	                                                        from (
		                                                        select department,
		                                                        '' as sale_name,
		                                                        '' as stages,
		                                                        '' as product_type,
		                                                        sum(case when product_type is not null or product_type not in ('','Project','Product','Service') then 1 else 0 end) as stages_pending_cnt,
		                                                        sum(case when product_type is not null or product_type not in ('','Project','Product','Service') then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end) as stages_pending_mb
		                                                        from Quotation 
		                                                        where left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and stages <> '' 
		                                                        group by department,stages having stages is not null and stages not in('','Closed(Won)','Closed(Lost)','No go')) as sub_stages

                                                        group by sub_stages.department,sub_stages.sale_name,sub_stages.product_type)

                                                        select * from x order by x.department,x.sale_name,x.stages");
                }
                else
                {
                    command = string.Format($@"with s1 as (select department, sale_name as sale,
                                                    cast(sum(sum(cast(replace(quoted_price,',','') as float))/1000000) over (partition by sale_name) as decimal(10,2)) as quo_mb,
                                                    count(quotation_no) as quo_cnt,
                                                    sum(case when product_type ='product' then 1 else 0 end) as product_cnt,
													sum(case when product_type ='product' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as product_mb,
                                                    sum(case when product_type ='project' then 1 else 0 end) as project_cnt,
													sum(case when product_type ='project' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as project_mb,
                                                    sum(case when product_type ='service' then 1 else 0 end) as service_cnt,
													sum(case when product_type ='service' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as service_mb,
                                                    sum(case when stages='Closed(Won)' then 1 else 0 end) as won_quo_cnt,
                                                    sum(case when stages='Closed(Won)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as won_mb,
                                                    sum(case when stages='Closed(Lost)' then 1 else 0 end) as loss_quo_cnt,
                                                    sum(case when stages='Closed(Lost)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as loss_mb,
                                                    sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt,
                                                    sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb,
                                                    sum(case when stages is null or stages not in('','Closed(Won)','Closed(Lost)','No go') then 1 else 0 end) as pending_quo_cnt,
                                                    sum(case when stages is null or stages not in('','Closed(Won)','Closed(Lost)','No go') then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as pending_mb
                                                    from Quotation where department='{department} ' and left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' group by department,sale_name union all

                                                    select (department + ' Total') as department,
                                                    '' as sale,
                                                    cast(sum(sum(cast(replace(quoted_price,',','') as float))/1000000) over (partition by department) as decimal(10,2)) as quo_mb,
                                                    count(quotation_no) as quo_cnt,
                                                    sum(case when product_type ='product' then 1 else 0 end) as product_cnt,
                                                    sum(case when product_type ='product' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as product_mb,
                                                    sum(case when product_type ='project' then 1 else 0 end) as project_cnt,
                                                    sum(case when product_type ='project' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as project_mb,
                                                    sum(case when product_type ='service' then 1 else 0 end) as service_cnt,
                                                    sum(case when product_type ='service' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as service_mb,
                                                    sum(case when stages='Closed(Won)' then 1 else 0 end) as won_quo_cnt,
                                                    sum(case when stages='Closed(Won)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as won_mb,
                                                    sum(case when stages='Closed(Lost)' then 1 else 0 end) as loss_quo_cnt,
                                                    sum(case when stages='Closed(Lost)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as loss_mb,
                                                    sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt,
                                                    sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb,
                                                    sum(case when stages is null or stages not in('','Closed(Won)','Closed(Lost)','No go') then 1 else 0 end) as pending_quo_cnt,
                                                    sum(case when stages is null or stages not in ('','Closed(Won)', 'Closed(Lost)', 'No go') then cast(cast(replace(quoted_price, ',', '') as float) / 1000000 as decimal(10, 2)) else 0 end) as pending_mb
                                                    from Quotation where department='{department}' and left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' group by department)
                                                    select * from s1 order by s1.department,s1.sale");

                    command_sub_type = string.Format($@"with x As(select department,
	                                                    sale_name,
	                                                    product_type,
	                                                    sum(case when stages ='Closed(Won)' then 1 else 0 end) as type_won_cnt,
	                                                    format(sum(case when stages ='Closed(Won)' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_won_mb,
	                                                    sum(case when stages ='Closed(Lost)' then 1 else 0 end) as type_lost_cnt,
	                                                    format(sum(case when stages ='Closed(Lost)' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_lost_mb,
	                                                    sum(case when stages ='No go' then 1 else 0 end) as type_nogo_cnt,
	                                                    format(sum(case when stages ='No go' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_nogo_mb,
	                                                    sum(case when stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go') then 1 else 0 end) as type_pending_cnt,
	                                                    format(sum(case when stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go') then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_pending_mb
                                                    from Quotation 
                                                    where department='{department}' and left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and product_type <> ''
                                                    group by department,sale_name,product_type
                                                    union all

                                                    select department,
	                                                    sale_name + ' Total' as sale_name,
	                                                    '' as product_type,
	                                                    sum(case when stages ='Closed(Won)' then 1 else 0 end) as type_won_cnt,
	                                                    format(sum(case when stages ='Closed(Won)' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_won_mb,
	                                                    sum(case when stages ='Closed(Lost)' then 1 else 0 end) as type_lost_cnt,
	                                                    format(sum(case when stages ='Closed(Lost)' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_lost_mb,
	                                                    sum(case when stages ='No go' then 1 else 0 end) as type_nogo_cnt,
	                                                    format(sum(case when stages ='No go' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_nogo_mb,
	                                                    sum(case when stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go') then 1 else 0 end) as type_pending_cnt,
	                                                    format(sum(case when stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go') then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as type_pending_mb
                                                    from Quotation 
                                                    where department='{department}' and left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and product_type <> ''
                                                    group by department,sale_name)

                                                    select * from x order by x.department,x.sale_name,x.product_type");

                    command_sub_stages = string.Format($@"with x As(select department,
                                                            sale_name,
                                                            stages,
                                                            sum(case when product_type ='Project' then 1 else 0 end) as stages_project_cnt,
                                                            format(sum(case when product_type ='Project' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_project_mb,
                                                            sum(case when product_type ='Product' then 1 else 0 end) as stages_product_cnt,
                                                            format(sum(case when product_type ='Product' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_product_mb,
                                                            sum(case when product_type ='Service' then 1 else 0 end) as stages_service_cnt,
                                                            format(sum(case when product_type ='Service' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_service_mb
                                                            from Quotation

                                                             where department='{department}' and left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and (stages <> '' and stages is not null) and stages in('Closed(Won)','Closed(Lost)','No go')
                                                             group by department,sale_name,stages

                                                             union all

                                                            select department,
                                                            sale_name + '_Total' as sale_name,
                                                            '' as stages,
                                                            sum(case when product_type ='Project' then 1 else 0 end) as stages_project_cnt,
                                                            format(sum(case when product_type ='Project' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_project_mb,
                                                            sum(case when product_type ='Product' then 1 else 0 end) as stages_product_cnt,
                                                            format(sum(case when product_type ='Product' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_product_mb,
                                                            sum(case when product_type ='Service' then 1 else 0 end) as stages_service_cnt,
                                                            format(sum(case when product_type ='Service' then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end),'N2') as stages_service_mb
                                                            from Quotation

                                                             where department='{department}' and left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and (stages <> '' and stages is not null) and stages in('Closed(Won)','Closed(Lost)','No go')
                                                             group by department,sale_name)
                                                          select* from x order by x.department,x.sale_name");
                    command_sub_pending = string.Format($@"with x As(select sub_stages.department,
	                                                        sub_stages.sale_name,
	                                                        'pending' as stages,
	                                                        sub_stages.product_type,
	                                                        sum(cast(sub_stages.stages_pending_cnt as float)) as stages_pending_cnt,
	                                                        format(sum(cast(sub_stages.stages_pending_mb as float)),'N2') as stages_pending_mb 
	                                                        from (
		                                                        select department,
		                                                        sale_name,
		                                                        'Pending' as stages,
		                                                        product_type,
		                                                        sum(case when product_type is not null or product_type not in ('','Project','Product','Service') then 1 else 0 end) as stages_pending_cnt,
		                                                        sum(case when product_type is not null or product_type not in ('','Project','Product','Service') then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end) as stages_pending_mb
		                                                        from Quotation 
		                                                        where department='{department}' and left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and stages <> '' 
		                                                        group by department,sale_name,product_type,stages having stages is not null and stages not in('','Closed(Won)','Closed(Lost)','No go')) as sub_stages

                                                        group by sub_stages.department,sub_stages.sale_name,sub_stages.product_type

                                                        union all

                                                        select sub_stages.department,
	                                                        sub_stages.sale_name,
	                                                        'pending Total' as stages,
	                                                        sub_stages.product_type,
	                                                        sum(cast(sub_stages.stages_pending_cnt as float)) as stages_pending_cnt,
	                                                        format(sum(cast(sub_stages.stages_pending_mb as float)),'N2') as stages_pending_mb 
	                                                        from (
		                                                        select department,
		                                                        sale_name,
		                                                        'Pending Total' as stages,
		                                                        '' as product_type,
		                                                        sum(case when product_type is not null or product_type not in ('','Project','Product','Service') then 1 else 0 end) as stages_pending_cnt,
		                                                        sum(case when product_type is not null or product_type not in ('','Project','Product','Service') then cast(replace(quoted_price,',','') as float ) / 1000000 else 0 end) as stages_pending_mb
		                                                        from Quotation 
		                                                        where department='{department}' and left(Convert(varchar,date,23),7) between '{month_first}' and '{month_last}' and stages <> '' 
		                                                        group by department,sale_name,stages having stages is not null and stages not in('','Closed(Won)','Closed(Lost)','No go')) as sub_stages

                                                        group by sub_stages.department,sub_stages.sale_name,sub_stages.product_type)

                                                        select * from x order by x.department,x.sale_name");
                }

                List<Quotation_Report_Sub_TypeModel> sub_types = new List<Quotation_Report_Sub_TypeModel>();
                SqlCommand cmd_sub_type = new SqlCommand(command_sub_type, ConnectSQL.OpenConnect());
                SqlDataReader dr_sub_type = cmd_sub_type.ExecuteReader();
                if (dr_sub_type.HasRows)
                {
                    while (dr_sub_type.Read())
                    {
                        Quotation_Report_Sub_TypeModel r = new Quotation_Report_Sub_TypeModel()
                        {
                            department = dr_sub_type["department"].ToString(),
                            sale_name = dr_sub_type["sale_name"].ToString(),
                            product_type = dr_sub_type["product_type"].ToString(),
                            type_won_cnt = dr_sub_type["type_won_cnt"].ToString(),
                            type_won_mb = dr_sub_type["type_won_mb"].ToString(),
                            type_lost_cnt = dr_sub_type["type_lost_cnt"].ToString(),
                            type_lost_mb = dr_sub_type["type_lost_mb"].ToString(),
                            type_nogo_cnt = dr_sub_type["type_nogo_cnt"].ToString(),
                            type_nogo_mb = dr_sub_type["type_nogo_mb"].ToString(),
                            type_pending_cnt = dr_sub_type["type_pending_cnt"].ToString(),
                            type_pending_mb = dr_sub_type["type_pending_mb"].ToString()
                        };
                        sub_types.Add(r);
                    }
                    dr_sub_type.Close();
                }
                ConnectSQL.CloseConnect();

                List<Quotation_Report_Sub_StagesModel> sub_stages = new List<Quotation_Report_Sub_StagesModel>();
                SqlCommand cmd_sub_stages = new SqlCommand(command_sub_stages, ConnectSQL.OpenConnect());
                SqlDataReader dr_sub_stages = cmd_sub_stages.ExecuteReader();
                if (dr_sub_stages.HasRows)
                {
                    while (dr_sub_stages.Read())
                    {
                        Quotation_Report_Sub_StagesModel r = new Quotation_Report_Sub_StagesModel()
                        {
                            department = dr_sub_stages["department"].ToString(),
                            sale_name = dr_sub_stages["sale_name"].ToString(),
                            stages = dr_sub_stages["stages"].ToString(),
                            stages_project_cnt = dr_sub_stages["stages_project_cnt"].ToString(),
                            stages_project_mb = dr_sub_stages["stages_project_mb"].ToString(),
                            stages_product_cnt = dr_sub_stages["stages_product_cnt"].ToString(),
                            stages_product_mb = dr_sub_stages["stages_product_mb"].ToString(),
                            stages_service_cnt = dr_sub_stages["stages_service_cnt"].ToString(),
                            stages_service_mb = dr_sub_stages["stages_service_mb"].ToString()
                        };
                        sub_stages.Add(r);
                    }
                    dr_sub_stages.Close();
                }
                ConnectSQL.CloseConnect();

                List<Quotation_Report_Sub_PendingModel> sub_pending = new List<Quotation_Report_Sub_PendingModel>();
                SqlCommand cmd_sub_pending = new SqlCommand(command_sub_pending, ConnectSQL.OpenConnect());
                SqlDataReader dr_sub_pending = cmd_sub_pending.ExecuteReader();
                if (dr_sub_pending.HasRows)
                {
                    while (dr_sub_pending.Read())
                    {
                        Quotation_Report_Sub_PendingModel r = new Quotation_Report_Sub_PendingModel()
                        {
                            department = dr_sub_pending["department"].ToString(),
                            sale_name = dr_sub_pending["sale_name"].ToString(),
                            stages = dr_sub_pending["stages"].ToString(),
                            product_type = dr_sub_pending["product_type"].ToString(),
                            stages_pending_cnt = dr_sub_pending["stages_pending_cnt"].ToString(),
                            stages_pending_mb = dr_sub_pending["stages_pending_mb"].ToString()
                        };
                        sub_pending.Add(r);
                    }
                    dr_sub_pending.Close();
                }

                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Quotation_Report_DepartmentModel r = new Quotation_Report_DepartmentModel()
                        {
                            department = dr["department"].ToString(),
                            sale = dr["sale"].ToString(),
                            quo_mb = dr["quo_mb"].ToString(),
                            quo_cnt = dr["quo_cnt"].ToString(),
                            product_cnt = dr["product_cnt"].ToString(),
                            product_mb = dr["product_mb"].ToString(),
                            project_cnt = dr["project_cnt"].ToString(),
                            project_mb = dr["project_mb"].ToString(),
                            service_cnt = dr["service_cnt"].ToString(),
                            service_mb = dr["service_mb"].ToString(),
                            won_quo_cnt = dr["won_quo_cnt"].ToString(),
                            won_mb = dr["won_mb"].ToString(),
                            loss_quo_cnt = dr["loss_quo_cnt"].ToString(),
                            loss_mb = dr["loss_mb"].ToString(),
                            nogo_quo_cnt = dr["nogo_quo_cnt"].ToString(),
                            nogo_mb = dr["nogo_mb"].ToString(),
                            pending_quo_cnt = dr["pending_quo_cnt"].ToString(),
                            pending_mb = dr["pending_mb"].ToString(),
                            quotation_sub_type = sub_types.FirstOrDefault(),
                            quotation_sub_stages = sub_stages.FirstOrDefault(),
                            quotation_sub_pending = sub_pending.FirstOrDefault()
                        };
                        reports.Add(r);
                    }
                    dr.Close();
                }

                ConnectSQL.CloseConnect();

                return reports;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<Quotation_Report_QuarterModel> GetReportQuarter(string department, string year)
        {
            try
            {
                List<Quotation_Report_QuarterModel> reports = new List<Quotation_Report_QuarterModel>();
                string command = "";
                if (department == "ALL")
                {
                    command = @"with s1 as(select department,sale_name as sale, " +
                            "sum(case when expected_order_date like '" + year + "-01%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jan_in, " +
                            "sum(case when expected_order_date like '" + year + "-01%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jan_out, " +
                            "sum(case when expected_order_date like '" + year + "-02%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as feb_in, " +
                            "sum(case when expected_order_date like '" + year + "-02%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as feb_out, " +
                            "sum(case when expected_order_date like '" + year + "-03%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as mar_in, " +
                            "sum(case when expected_order_date like '" + year + "-03%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as mar_out, " +
                            "sum(case when expected_order_date like '" + year + "-04%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as apr_in, " +
                            "sum(case when expected_order_date like '" + year + "-04%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as apr_out, " +
                            "sum(case when expected_order_date like '" + year + "-05%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as may_in, " +
                            "sum(case when expected_order_date like '" + year + "-05%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as may_out, " +
                            "sum(case when expected_order_date like '" + year + "-06%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jun_in, " +
                            "sum(case when expected_order_date like '" + year + "-06%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jun_out, " +
                            "sum(case when expected_order_date like '" + year + "-07%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jul_in, " +
                            "sum(case when expected_order_date like '" + year + "-07%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jul_out, " +
                            "sum(case when expected_order_date like '" + year + "-08%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as aug_in, " +
                            "sum(case when expected_order_date like '" + year + "-08%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as aug_out, " +
                            "sum(case when expected_order_date like '" + year + "-09%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as sep_in, " +
                            "sum(case when expected_order_date like '" + year + "-09%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as sep_out, " +
                            "sum(case when expected_order_date like '" + year + "-10%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as oct_in, " +
                            "sum(case when expected_order_date like '" + year + "-10%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as oct_out, " +
                            "sum(case when expected_order_date like '" + year + "-11%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as nov_in, " +
                            "sum(case when expected_order_date like '" + year + "-11%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as nov_out, " +
                            "sum(case when expected_order_date like '" + year + "-12%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as dec_in, " +
                            "sum(case when expected_order_date like '" + year + "-12%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as dec_out " +
                            "from Quotation group by department,sale_name union all " +

                            "select(department + ' Total') as department,'' as sale, " +
                            "sum(case when expected_order_date like '" + year + "-01%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jan_in, " +
                            "sum(case when expected_order_date like '" + year + "-01%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jan_out, " +
                            "sum(case when expected_order_date like '" + year + "-02%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as feb_in, " +
                            "sum(case when expected_order_date like '" + year + "-02%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as feb_out, " +
                            "sum(case when expected_order_date like '" + year + "-03%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as mar_in, " +
                            "sum(case when expected_order_date like '" + year + "-03%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as mar_out, " +
                            "sum(case when expected_order_date like '" + year + "-04%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as apr_in, " +
                            "sum(case when expected_order_date like '" + year + "-04%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as apr_out, " +
                            "sum(case when expected_order_date like '" + year + "-05%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as may_in, " +
                            "sum(case when expected_order_date like '" + year + "-05%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as may_out, " +
                            "sum(case when expected_order_date like '" + year + "-06%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jun_in, " +
                            "sum(case when expected_order_date like '" + year + "-06%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jun_out, " +
                            "sum(case when expected_order_date like '" + year + "-07%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jul_in, " +
                            "sum(case when expected_order_date like '" + year + "-07%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jul_out, " +
                            "sum(case when expected_order_date like '" + year + "-08%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as aug_in, " +
                            "sum(case when expected_order_date like '" + year + "-08%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as aug_out, " +
                            "sum(case when expected_order_date like '" + year + "-09%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as sep_in, " +
                            "sum(case when expected_order_date like '" + year + "-09%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as sep_out, " +
                            "sum(case when expected_order_date like '" + year + "-10%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as oct_in, " +
                            "sum(case when expected_order_date like '" + year + "-10%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as oct_out, " +
                            "sum(case when expected_order_date like '" + year + "-11%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as nov_in, " +
                            "sum(case when expected_order_date like '" + year + "-11%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as nov_out, " +
                            "sum(case when expected_order_date like '" + year + "-12%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as dec_in, " +
                            "sum(case when expected_order_date like '" + year + "-12%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as dec_out " +
                            "from Quotation group by department union all " +

                            "select 'Total' as department,'' as sale, " +
                            "sum(case when expected_order_date like '" + year + "-01%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jan_in, " +
                            "sum(case when expected_order_date like '" + year + "-01%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jan_out, " +
                            "sum(case when expected_order_date like '" + year + "-02%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as feb_in, " +
                            "sum(case when expected_order_date like '" + year + "-02%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as feb_out, " +
                            "sum(case when expected_order_date like '" + year + "-03%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as mar_in, " +
                            "sum(case when expected_order_date like '" + year + "-03%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as mar_out, " +
                            "sum(case when expected_order_date like '" + year + "-04%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as apr_in, " +
                            "sum(case when expected_order_date like '" + year + "-04%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as apr_out, " +
                            "sum(case when expected_order_date like '" + year + "-05%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as may_in, " +
                            "sum(case when expected_order_date like '" + year + "-05%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as may_out, " +
                            "sum(case when expected_order_date like '" + year + "-06%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jun_in, " +
                            "sum(case when expected_order_date like '" + year + "-06%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jun_out, " +
                            "sum(case when expected_order_date like '" + year + "-07%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jul_in, " +
                            "sum(case when expected_order_date like '" + year + "-07%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jul_out, " +
                            "sum(case when expected_order_date like '" + year + "-08%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as aug_in, " +
                            "sum(case when expected_order_date like '" + year + "-08%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as aug_out, " +
                            "sum(case when expected_order_date like '" + year + "-09%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as sep_in, " +
                            "sum(case when expected_order_date like '" + year + "-09%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as sep_out, " +
                            "sum(case when expected_order_date like '" + year + "-10%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as oct_in, " +
                            "sum(case when expected_order_date like '" + year + "-10%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as oct_out, " +
                            "sum(case when expected_order_date like '" + year + "-11%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as nov_in, " +
                            "sum(case when expected_order_date like '" + year + "-11%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as nov_out, " +
                            "sum(case when expected_order_date like '" + year + "-12%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as dec_in, " +
                            "sum(case when expected_order_date like '" + year + "-12%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as dec_out " +
                            "from Quotation) " +
                            "select* from s1 order by s1.department,s1.sale ";
                }
                else
                {
                    command = @"with s1 as(select department,sale_name as sale, " +
                            "sum(case when expected_order_date like '" + year + "-01%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jan_in, " +
                            "sum(case when expected_order_date like '" + year + "-01%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jan_out, " +
                            "sum(case when expected_order_date like '" + year + "-02%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as feb_in, " +
                            "sum(case when expected_order_date like '" + year + "-02%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as feb_out, " +
                            "sum(case when expected_order_date like '" + year + "-03%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as mar_in, " +
                            "sum(case when expected_order_date like '" + year + "-03%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as mar_out, " +
                            "sum(case when expected_order_date like '" + year + "-04%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as apr_in, " +
                            "sum(case when expected_order_date like '" + year + "-04%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as apr_out, " +
                            "sum(case when expected_order_date like '" + year + "-05%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as may_in, " +
                            "sum(case when expected_order_date like '" + year + "-05%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as may_out, " +
                            "sum(case when expected_order_date like '" + year + "-06%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jun_in, " +
                            "sum(case when expected_order_date like '" + year + "-06%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jun_out, " +
                            "sum(case when expected_order_date like '" + year + "-07%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jul_in, " +
                            "sum(case when expected_order_date like '" + year + "-07%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jul_out, " +
                            "sum(case when expected_order_date like '" + year + "-08%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as aug_in, " +
                            "sum(case when expected_order_date like '" + year + "-08%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as aug_out, " +
                            "sum(case when expected_order_date like '" + year + "-09%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as sep_in, " +
                            "sum(case when expected_order_date like '" + year + "-09%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as sep_out, " +
                            "sum(case when expected_order_date like '" + year + "-10%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as oct_in, " +
                            "sum(case when expected_order_date like '" + year + "-10%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as oct_out, " +
                            "sum(case when expected_order_date like '" + year + "-11%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as nov_in, " +
                            "sum(case when expected_order_date like '" + year + "-11%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as nov_out, " +
                            "sum(case when expected_order_date like '" + year + "-12%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as dec_in, " +
                            "sum(case when expected_order_date like '" + year + "-12%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as dec_out " +
                            "from Quotation where department='" + department + "' group by department,sale_name union all " +

                            "select(department + ' Total') as department,'' as sale, " +
                            "sum(case when expected_order_date like '" + year + "-01%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jan_in, " +
                            "sum(case when expected_order_date like '" + year + "-01%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jan_out, " +
                            "sum(case when expected_order_date like '" + year + "-02%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as feb_in, " +
                            "sum(case when expected_order_date like '" + year + "-02%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as feb_out, " +
                            "sum(case when expected_order_date like '" + year + "-03%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as mar_in, " +
                            "sum(case when expected_order_date like '" + year + "-03%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as mar_out, " +
                            "sum(case when expected_order_date like '" + year + "-04%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as apr_in, " +
                            "sum(case when expected_order_date like '" + year + "-04%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as apr_out, " +
                            "sum(case when expected_order_date like '" + year + "-05%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as may_in, " +
                            "sum(case when expected_order_date like '" + year + "-05%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as may_out, " +
                            "sum(case when expected_order_date like '" + year + "-06%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jun_in, " +
                            "sum(case when expected_order_date like '" + year + "-06%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jun_out, " +
                            "sum(case when expected_order_date like '" + year + "-07%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jul_in, " +
                            "sum(case when expected_order_date like '" + year + "-07%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as jul_out, " +
                            "sum(case when expected_order_date like '" + year + "-08%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as aug_in, " +
                            "sum(case when expected_order_date like '" + year + "-08%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as aug_out, " +
                            "sum(case when expected_order_date like '" + year + "-09%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as sep_in, " +
                            "sum(case when expected_order_date like '" + year + "-09%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as sep_out, " +
                            "sum(case when expected_order_date like '" + year + "-10%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as oct_in, " +
                            "sum(case when expected_order_date like '" + year + "-10%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as oct_out, " +
                            "sum(case when expected_order_date like '" + year + "-11%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as nov_in, " +
                            "sum(case when expected_order_date like '" + year + "-11%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as nov_out, " +
                            "sum(case when expected_order_date like '" + year + "-12%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as dec_in, " +
                            "sum(case when expected_order_date like '" + year + "-12%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) /1000000 as decimal(10,2)) else 0 end end) as dec_out " +
                            "from Quotation where department='" + department + "' group by department) " +
                            "select* from s1 order by s1.department,s1.sale ";
                }
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Quotation_Report_QuarterModel r = new Quotation_Report_QuarterModel()
                        {
                            department = dr["department"].ToString(),
                            sale = dr["sale"].ToString(),
                            jan_in = dr["jan_in"].ToString(),
                            jan_out = dr["jan_out"].ToString(),
                            feb_in = dr["feb_in"].ToString(),
                            feb_out = dr["feb_out"].ToString(),
                            mar_in = dr["mar_in"].ToString(),
                            mar_out = dr["mar_out"].ToString(),
                            sum_in_q1 =
                            ((dr["jan_in"].ToString() != "" ? float.Parse(dr["jan_in"].ToString()) : 0) +
                             (dr["feb_in"].ToString() != "" ? float.Parse(dr["feb_in"].ToString()) : 0) +
                             (dr["mar_in"].ToString() != "" ? float.Parse(dr["mar_in"].ToString()) : 0)).ToString(),
                            sum_out_q1 =
                            ((dr["jan_out"].ToString() != "" ? float.Parse(dr["jan_out"].ToString()) : 0) +
                             (dr["feb_out"].ToString() != "" ? float.Parse(dr["feb_out"].ToString()) : 0) +
                             (dr["mar_out"].ToString() != "" ? float.Parse(dr["mar_out"].ToString()) : 0)).ToString(),
                            apr_in = dr["apr_in"].ToString(),
                            apr_out = dr["apr_out"].ToString(),
                            may_in = dr["may_in"].ToString(),
                            may_out = dr["may_out"].ToString(),
                            jun_in = dr["jun_in"].ToString(),
                            jun_out = dr["jun_out"].ToString(),
                            sum_in_q2 =
                            ((dr["apr_in"].ToString() != "" ? float.Parse(dr["apr_in"].ToString()) : 0) +
                             (dr["may_in"].ToString() != "" ? float.Parse(dr["may_in"].ToString()) : 0) +
                             (dr["jun_in"].ToString() != "" ? float.Parse(dr["jun_in"].ToString()) : 0)).ToString(),
                            sum_out_q2 =
                             ((dr["apr_out"].ToString() != "" ? float.Parse(dr["apr_out"].ToString()) : 0) +
                             (dr["may_out"].ToString() != "" ? float.Parse(dr["may_out"].ToString()) : 0) +
                             (dr["jun_out"].ToString() != "" ? float.Parse(dr["jun_out"].ToString()) : 0)).ToString(),
                            jul_in = dr["jul_in"].ToString(),
                            jul_out = dr["jul_out"].ToString(),
                            aug_in = dr["aug_in"].ToString(),
                            aug_out = dr["aug_out"].ToString(),
                            sep_in = dr["sep_in"].ToString(),
                            sep_out = dr["sep_out"].ToString(),
                            sum_in_q3 =
                            ((dr["jul_in"].ToString() != "" ? float.Parse(dr["jul_in"].ToString()) : 0) +
                             (dr["aug_in"].ToString() != "" ? float.Parse(dr["aug_in"].ToString()) : 0) +
                             (dr["sep_in"].ToString() != "" ? float.Parse(dr["sep_in"].ToString()) : 0)).ToString(),
                            sum_out_q3 =
                            ((dr["jul_out"].ToString() != "" ? float.Parse(dr["jul_out"].ToString()) : 0) +
                             (dr["aug_out"].ToString() != "" ? float.Parse(dr["aug_out"].ToString()) : 0) +
                             (dr["sep_out"].ToString() != "" ? float.Parse(dr["sep_out"].ToString()) : 0)).ToString(),
                            oct_in = dr["oct_in"].ToString(),
                            oct_out = dr["oct_out"].ToString(),
                            nov_in = dr["nov_in"].ToString(),
                            nov_out = dr["nov_out"].ToString(),
                            dec_in = dr["dec_in"].ToString(),
                            dec_out = dr["dec_out"].ToString(),
                            sum_in_q4 =
                            ((dr["oct_in"].ToString() != "" ? float.Parse(dr["oct_in"].ToString()) : 0) +
                             (dr["nov_in"].ToString() != "" ? float.Parse(dr["nov_in"].ToString()) : 0) +
                             (dr["dec_in"].ToString() != "" ? float.Parse(dr["dec_in"].ToString()) : 0)).ToString(),
                            sum_out_q4 =
                            ((dr["oct_out"].ToString() != "" ? float.Parse(dr["oct_out"].ToString()) : 0) +
                             (dr["nov_out"].ToString() != "" ? float.Parse(dr["nov_out"].ToString()) : 0) +
                             (dr["dec_out"].ToString() != "" ? float.Parse(dr["dec_out"].ToString()) : 0)).ToString(),
                        };
                        reports.Add(r);
                    }
                    dr.Close();
                }
                return reports;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<Quotation_Report_StatusModel> GetReportStatus(string year, string department, string sale)
        {
            try
            {
                List<Quotation_Report_StatusModel> reports = new List<Quotation_Report_StatusModel>();

                string command = "";

                if (department == "ALL" && sale == "ALL")
                {
                    command = string.Format($@"with s1 As(
                                 select  cast(ROW_NUMBER ( ) over (partition by sale_name order by sale_name) as varchar) as no,
                                 sale_name,
                                 enduser,
                                 project_name,
                                 format(sum(case when [status] ='IN' then cast(replace(quoted_price,',','') as float) / 1000000 else 0 end),'N2') as status_in,
                                 format(sum(case when [status] ='OUT' then cast(replace(quoted_price,',','') as float) /1000000 else 0 end),'N2') as status_out
                                 from Quotation where status <>'' and date like '{year}%' group by sale_name,enduser,project_name
                                 union all
                                 select  '' as no,
                                 sale_name + '_Total',
                                 '',
                                 '',
                                 format(sum(case when [status] ='IN' then cast(replace(quoted_price,',','') as float) / 1000000 else 0 end),'N2') as status_in,
                                 format(sum(case when [status] ='OUT' then cast(replace(quoted_price,',','') as float) /1000000 else 0 end),'N2') as status_out
                                 from Quotation where status <>'' and date like '{year}%' group by sale_name

                                 union all

                                 select  '' as no,
                                 'รวม',
                                 '',
                                 '',
                                 format(sum(case when [status] ='IN' then cast(replace(quoted_price,',','') as float) / 1000000 else 0 end),'N2') as status_in,
                                 format(sum(case when [status] ='OUT' then cast(replace(quoted_price,',','') as float) /1000000 else 0 end),'N2') as status_out
                                 from Quotation where status <>'' and date like '{year}%')

                                 select * from s1 order by s1.sale_name");
                }
                else if (department != "ALL" && sale == "ALL")
                {
                    command = string.Format($@"with s1 As(
                                 select  cast(ROW_NUMBER ( ) over (partition by sale_name order by sale_name) as varchar) as no,
                                 sale_name,
                                 enduser,
                                 project_name,
                                 format(sum(case when [status] ='IN' then cast(replace(quoted_price,',','') as float) / 1000000 else 0 end),'N2') as status_in,
                                 format(sum(case when [status] ='OUT' then cast(replace(quoted_price,',','') as float) /1000000 else 0 end),'N2') as status_out
                                 from Quotation where status <>'' and department='{department}' and date like '{year}%' group by sale_name,enduser,project_name
                                 union all
                                 select  '' as no,
                                 sale_name + '_Total',
                                 '',
                                 '',
                                 format(sum(case when [status] ='IN' then cast(replace(quoted_price,',','') as float) / 1000000 else 0 end),'N2') as status_in,
                                 format(sum(case when [status] ='OUT' then cast(replace(quoted_price,',','') as float) /1000000 else 0 end),'N2') as status_out
                                 from Quotation where status <>'' and department='{department}' and date like '{year}%' group by sale_name

                                 union all

                                 select  '' as no,
                                 'รวม',
                                 '',
                                 '',
                                 format(sum(case when [status] ='IN' then cast(replace(quoted_price,',','') as float) / 1000000 else 0 end),'N2') as status_in,
                                 format(sum(case when [status] ='OUT' then cast(replace(quoted_price,',','') as float) /1000000 else 0 end),'N2') as status_out
                                 from Quotation where status <>'' and department='{department}' and date like '{year}%')

                                 select * from s1 order by s1.sale_name");
                }
                else if (department != "ALL" && sale != "ALL")
                {
                    command = string.Format($@"with s1 As(
                                 select  cast(ROW_NUMBER ( ) over (partition by sale_name order by sale_name) as varchar) as no,
                                 sale_name,
                                 enduser,
                                 project_name,
                                 format(sum(case when [status] ='IN' then cast(replace(quoted_price,',','') as float) / 1000000 else 0 end),'N2') as status_in,
                                 format(sum(case when [status] ='OUT' then cast(replace(quoted_price,',','') as float) /1000000 else 0 end),'N2') as status_out
                                 from Quotation where status <>'' and department='{department}' and sale_name='{sale}' and date like '{year}%' group by sale_name,enduser,project_name
                                 union all
                                 select  '' as no,
                                 sale_name + '_Total',
                                 '',
                                 '',
                                 format(sum(case when [status] ='IN' then cast(replace(quoted_price,',','') as float) / 1000000 else 0 end),'N2') as status_in,
                                 format(sum(case when [status] ='OUT' then cast(replace(quoted_price,',','') as float) /1000000 else 0 end),'N2') as status_out
                                 from Quotation where status <>'' and department='{department}' and  sale_name='{sale}' and date like '{year}%' group by sale_name)

                                 select * from s1 order by s1.sale_name");
                }

                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Quotation_Report_StatusModel r = new Quotation_Report_StatusModel()
                        {
                            no = dr["no"].ToString(),
                            sale_name = dr["sale_name"].ToString(),
                            enduser = dr["enduser"].ToString(),
                            project_name = dr["project_name"].ToString(),
                            status_in = dr["status_in"].ToString(),
                            status_out = dr["status_out"].ToString(),
                        };
                        reports.Add(r);
                    }
                    dr.Close();
                }
                return reports;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<Quotation_Report_YearModel> GetReportYear(string department, string year)
        {
            try
            {
                List<Quotation_Report_YearModel> reports = new List<Quotation_Report_YearModel>();

                string command = @"with s1 as(
                                select department, CAST(YEAR(date) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(date) AS VARCHAR(2)), 2) as month,
                                format(sum(cast(replace(quoted_price,',','') as float))/1000000,'N2') as quo_mb,
                                count(quotation_no) as quo_cnt,
                                sum(case when product_type ='product' then 1 else 0 end) as product_cnt,
                                sum(case when product_type ='product' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as product_mb,
                                sum(case when product_type ='project' then 1 else 0 end) as project_cnt,
                                sum(case when product_type ='project' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as project_mb,
                                sum(case when product_type ='service' then 1 else 0 end) as service_cnt,
                                sum(case when product_type ='service' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as service_mb,
                                sum(case when stages='Closed(Won)' then 1 else 0 end) as won_quo_cnt,
                                sum(case when stages='Closed(Won)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as won_mb,
                                sum(case when stages='Closed(Lost)' then 1 else 0 end) as loss_quo_cnt,
                                sum(case when stages='Closed(Lost)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as loss_mb,
                                sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt,
                                sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb,
                                sum(case when stages is null or stages not in('','Closed(Won)','Closed(Lost)','No go') then 1 else 0 end) as pending_quo_cnt,
                                sum(case when stages is null or stages not in('','Closed(Won)','Closed(Lost)','No go') then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as pending_mb
                                from Quotation where department='" + department + "' and date like '" + year + "%' group by department,DATEPART(YEAR,date) ,DATEPART(MONTH,date) " +
                                "union all " +

                                "select 'Total' as department, 'Total' as month, " +
                                "format(sum(cast(replace(quoted_price,',','') as float))/1000000,'N2') as quo_mb, " +
                                "count(quotation_no) as quo_cnt, " +
                                "sum(case when product_type ='product' then 1 else 0 end) as product_cnt, " +
                                "sum(case when product_type ='product' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as product_mb, " +
                                "sum(case when product_type ='project' then 1 else 0 end) as project_cnt, " +
                                "sum(case when product_type ='project' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as project_mb, " +
                                "sum(case when product_type ='service' then 1 else 0 end) as service_cnt, " +
                                "sum(case when product_type ='service' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as service_mb, " +
                                "sum(case when stages='Closed(Won)' then 1 else 0 end) as won_quo_cnt, " +
                                "sum(case when stages='Closed(Won)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as won_mb, " +
                                "sum(case when stages='Closed(Lost)' then 1 else 0 end) as loss_quo_cnt, " +
                                "sum(case when stages='Closed(Lost)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as loss_mb, " +
                                "sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt, " +
                                "sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb, " +
                                "sum(case when stages is null or stages not in('','Closed(Won)','Closed(Lost)','No go') then 1 else 0 end) as pending_quo_cnt, " +
                                "sum(case when stages is null or stages not in('','Closed(Won)','Closed(Lost)','No go') then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as pending_mb " +
                                "from Quotation where department='" + department + "' and date like '" + year + "%'  group by department,DATEPART(YEAR,date)) " +
                                "select * from s1 order by s1.department";

                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Quotation_Report_YearModel r = new Quotation_Report_YearModel()
                        {
                            department = dr["department"].ToString(),
                            month = dr["month"].ToString(),
                            quo_mb = dr["quo_mb"].ToString(),
                            quo_cnt = dr["quo_cnt"].ToString(),
                            product_cnt = dr["product_cnt"].ToString(),
                            product_mb = dr["product_mb"].ToString(),
                            project_cnt = dr["project_cnt"].ToString(),
                            project_mb = dr["project_mb"].ToString(),
                            service_cnt = dr["service_cnt"].ToString(),
                            service_mb = dr["service_mb"].ToString(),
                            won_quo_cnt = dr["won_quo_cnt"].ToString(),
                            won_mb = dr["won_mb"].ToString(),
                            loss_quo_cnt = dr["loss_quo_cnt"].ToString(),
                            loss_mb = dr["loss_mb"].ToString(),
                            nogo_quo_cnt = dr["nogo_quo_cnt"].ToString(),
                            nogo_mb = dr["nogo_mb"].ToString(),
                            pending_quo_cnt = dr["pending_quo_cnt"].ToString(),
                            pending_mb = dr["pending_mb"].ToString(),
                        };
                        reports.Add(r);
                    }
                    dr.Close();
                }
                return reports;
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


//                                                  select Quotation.department, Quotation.sale_name as sale,
//                                                  cast(sum(sum(cast(replace(quoted_price,',','') as float))/ 1000000) over(partition by Quotation.sale_name) as decimal(10, 2)) as quo_mb,
//                                                  count(quotation_no) as quo_cnt,
//													sum(sub_sub_type.product_won_cnt) / count(sub_sub_type.product_won_cnt) as product_won_cnt ,
//													sum(sub_sub_type.product_won_mb) / count(sub_sub_type.product_won_mb) as product_won_mb,
//													sum(sub_sub_type.product_lost_cnt) / count(sub_sub_type.product_lost_cnt) as product_lost_cnt ,
//													sum(sub_sub_type.product_lost_mb) / count(sub_sub_type.product_lost_mb) as product_lost_mb,
//													sum(sub_sub_type.product_nogo_cnt) / count(sub_sub_type.product_nogo_cnt) as product_nogo_cnt ,
//													sum(sub_sub_type.product_nogo_mb) / count(sub_sub_type.product_nogo_mb) as product_nogo_mb,
//													sum(sub_sub_type.product_pending_cnt) / count(sub_sub_type.product_pending_cnt) as product_pending_cnt ,
//													sum(sub_sub_type.product_pending_mb) / count(sub_sub_type.product_pending_mb) as product_pending_mb,
//                                                    sum(case when Quotation.product_type = 'product' then 1 else 0 end) as product_cnt,
//													sum(case when Quotation.product_type = 'product' then cast(cast(replace(quoted_price, ',', '') as float) / 1000000 as decimal(10, 2)) else 0 end) as product_mb,

//													sum(sub_sub_type.project_won_cnt) / count(sub_sub_type.project_won_cnt) as project_won_cnt ,
//													sum(sub_sub_type.project_won_mb) / count(sub_sub_type.project_won_mb) as project_won_mb,
//													sum(sub_sub_type.project_lost_cnt) / count(sub_sub_type.project_lost_cnt) as project_lost_cnt ,
//													sum(sub_sub_type.project_lost_mb) / count(sub_sub_type.project_lost_mb) as project_lost_mb,
//													sum(sub_sub_type.project_nogo_cnt) / count(sub_sub_type.project_nogo_cnt) as project_nogo_cnt ,
//													sum(sub_sub_type.project_nogo_mb) / count(sub_sub_type.project_nogo_mb) as project_nogo_mb,
//													sum(sub_sub_type.project_pending_cnt) / count(sub_sub_type.project_pending_cnt) as project_pending_cnt ,
//													sum(sub_sub_type.project_pending_mb) / count(sub_sub_type.project_pending_mb) as project_pending_mb,
//                                                    sum(case when Quotation.product_type = 'project' then 1 else 0 end) as project_cnt,
//													sum(case when Quotation.product_type = 'project' then cast(cast(replace(quoted_price, ',', '') as float) / 1000000 as decimal(10, 2)) else 0 end) as project_mb,

//													sum(sub_sub_type.service_won_cnt) / count(sub_sub_type.service_won_cnt) as service_won_cnt ,
//													sum(sub_sub_type.service_won_mb) / count(sub_sub_type.service_won_mb) as service_won_mb,
//													sum(sub_sub_type.service_lost_cnt) / count(sub_sub_type.service_lost_cnt) as service_lost_cnt ,
//													sum(sub_sub_type.service_lost_mb) / count(sub_sub_type.service_lost_mb) as service_lost_mb,
//													sum(sub_sub_type.service_nogo_cnt) / count(sub_sub_type.service_nogo_cnt) as service_nogo_cnt ,
//													sum(sub_sub_type.service_nogo_mb) / count(sub_sub_type.service_nogo_mb) as service_nogo_mb,
//													sum(sub_sub_type.service_pending_cnt) / count(sub_sub_type.service_pending_cnt) as service_pending_cnt ,
//													sum(sub_sub_type.service_pending_mb) / count(sub_sub_type.service_pending_mb) as service_pending_mb,
//                                                    sum(case when Quotation.product_type = 'service' then 1 else 0 end) as service_cnt,
//													sum(case when Quotation.product_type = 'service' then cast(cast(replace(quoted_price, ',', '') as float) / 1000000 as decimal(10, 2)) else 0 end) as service_mb,
                                                    
//													sum(sub_sub_stages.won_product_cnt) / count(sub_sub_stages.won_product_cnt) as won_product_cnt,
//													sum(sub_sub_stages.won_product_mb) / count(sub_sub_stages.won_product_mb) as won_product_mb,
//													sum(sub_sub_stages.won_project_cnt) / count(sub_sub_stages.won_project_cnt) as won_project_cnt,
//													sum(sub_sub_stages.won_project_mb) / count(sub_sub_stages.won_project_mb) as won_project_mb,
//													sum(sub_sub_stages.won_service_cnt) / count(sub_sub_stages.won_service_cnt) as won_service_cnt,
//													sum(sub_sub_stages.won_service_mb) / count(sub_sub_stages.won_service_mb) as won_service_mb,
//													sum(case when stages = 'Closed(Won)' then 1 else 0 end) as won_quo_cnt,
//                                                    sum(case when stages = 'Closed(Won)' then cast(cast(replace(quoted_price, ',', '') as float) / 1000000 as decimal(10, 2)) else 0 end) as won_mb,
                                                    
//													sum(sub_sub_stages.lost_product_cnt) / count(sub_sub_stages.lost_product_cnt) as lost_product_cnt,
//													sum(sub_sub_stages.lost_product_mb) / count(sub_sub_stages.lost_product_mb) as lost_product_mb,
//													sum(sub_sub_stages.lost_project_cnt) / count(sub_sub_stages.lost_project_cnt) as lost_project_cnt,
//													sum(sub_sub_stages.lost_project_mb) / count(sub_sub_stages.lost_project_mb) as lost_project_mb,
//													sum(sub_sub_stages.lost_service_cnt) / count(sub_sub_stages.lost_service_cnt) as lost_service_cnt,
//													sum(sub_sub_stages.lost_service_mb) / count(sub_sub_stages.lost_service_mb) as lost_service_mb,
//													sum(case when stages = 'Closed(Lost)' then 1 else 0 end) as loss_quo_cnt,
//                                                    sum(case when stages = 'Closed(Lost)' then cast(cast(replace(quoted_price, ',', '') as float) / 1000000 as decimal(10, 2)) else 0 end) as loss_mb,
                                                    
//													sum(sub_sub_stages.nogo_product_cnt) / count(sub_sub_stages.nogo_product_cnt) as nogo_product_cnt,
//													sum(sub_sub_stages.nogo_product_mb) / count(sub_sub_stages.nogo_product_mb) as nogo_product_mb,
//													sum(sub_sub_stages.nogo_project_cnt) / count(sub_sub_stages.nogo_project_cnt) as nogo_project_cnt,
//													sum(sub_sub_stages.nogo_project_mb) / count(sub_sub_stages.nogo_project_mb) as nogo_project_mb,
//													sum(sub_sub_stages.nogo_service_cnt) / count(sub_sub_stages.nogo_service_cnt) as nogo_service_cnt,
//													sum(sub_sub_stages.nogo_service_mb) / count(sub_sub_stages.nogo_service_mb) as nogo_service_mb,
//													sum(case when stages = 'No go' then 1 else 0 end) as nogo_quo_cnt,
//                                                    sum(case when stages = 'No go' then cast(cast(replace(quoted_price, ',', '') as float) / 1000000 as decimal(10, 2)) else 0 end) as nogo_mb,
                                                    
//													sum(sub_sub_pending.product_pending_cnt) / count(sub_sub_pending.product_pending_cnt) as product_pending_cnt,
//													sum(sub_sub_pending.product_pending_mb) / count(sub_sub_pending.product_pending_mb) as product_pending_mb,
//													sum(sub_sub_pending.project_pending_cnt) / count(sub_sub_pending.project_pending_cnt) as project_pending_cnt,
//													sum(sub_sub_pending.project_pending_mb) / count(sub_sub_pending.project_pending_mb) as project_pending_mb,
//													sum(sub_sub_pending.service_pending_cnt) / count(sub_sub_pending.service_pending_cnt) as service_pending_cnt,
//													sum(sub_sub_pending.service_pending_mb) / count(sub_sub_pending.service_pending_mb) as service_pending_mb,
//													sum(case when stages is null or stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then 1 else 0 end) as pending_quo_cnt,
//                                                    sum(case when stages is null or stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then cast(cast(replace(quoted_price, ',', '') as float) / 1000000 as decimal(10, 2)) else 0 end) as pending_mb
//                                                    from Quotation
//                                                    left join (

//                                                        select sub_type.department,sub_type.sale_name,
//															sum(case when sub_type.product_type = 'Product' then type_won_cnt else 0 end) as product_won_cnt,
//															sum(case when sub_type.product_type = 'Product' then cast(type_won_mb as float) else 0 end) as product_won_mb,
//															sum(case when sub_type.product_type = 'Product' then type_lost_cnt else 0 end) as product_lost_cnt,
//															sum(case when sub_type.product_type = 'Product' then cast(type_lost_mb as float) else 0 end) as product_lost_mb,
//															sum(case when sub_type.product_type = 'Product' then type_nogo_cnt else 0 end) as product_nogo_cnt,
//															sum(case when sub_type.product_type = 'Product' then cast(type_nogo_mb as float) else 0 end) as product_nogo_mb,
//															sum(case when sub_type.product_type = 'Product' then type_pending_cnt else 0 end) as product_pending_cnt,
//															sum(case when sub_type.product_type = 'Product' then cast(type_pending_mb as float) else 0 end) as product_pending_mb,

//															sum(case when sub_type.product_type = 'Project' then type_won_cnt else 0 end) as project_won_cnt,
//															sum(case when sub_type.product_type = 'Project' then cast(type_won_mb as float) else 0 end) as project_won_mb,
//															sum(case when sub_type.product_type = 'Project' then type_lost_cnt else 0 end) as project_lost_cnt,
//															sum(case when sub_type.product_type = 'Project' then cast(type_lost_mb as float) else 0 end) as project_lost_mb,
//															sum(case when sub_type.product_type = 'Project' then type_nogo_cnt else 0 end) as project_nogo_cnt,
//															sum(case when sub_type.product_type = 'Project' then cast(type_nogo_mb as float) else 0 end) as project_nogo_mb,
//															sum(case when sub_type.product_type = 'Project' then type_pending_cnt else 0 end) as project_pending_cnt,
//															sum(case when sub_type.product_type = 'Project' then cast(type_pending_mb as float) else 0 end) as project_pending_mb,

//															sum(case when sub_type.product_type = 'Service' then type_won_cnt else 0 end) as service_won_cnt,
//															sum(case when sub_type.product_type = 'Service' then cast(type_won_mb as float) else 0 end) as service_won_mb,
//															sum(case when sub_type.product_type = 'Service' then type_lost_cnt else 0 end) as service_lost_cnt,
//															sum(case when sub_type.product_type = 'Service' then cast(type_lost_mb as float) else 0 end) as service_lost_mb,
//															sum(case when sub_type.product_type = 'Service' then type_nogo_cnt else 0 end) as service_nogo_cnt,
//															sum(case when sub_type.product_type = 'Service' then cast(type_nogo_mb as float) else 0 end) as service_nogo_mb,
//															sum(case when sub_type.product_type = 'Service' then type_pending_cnt else 0 end) as service_pending_cnt,
//															sum(case when sub_type.product_type = 'Service' then cast(type_pending_mb as float) else 0 end) as service_pending_mb

//                                                        from(
//                                                            select department,
//                                                                sale_name,
//                                                                product_type,
//                                                                sum(case when stages = 'Closed(Won)' then 1 else 0 end) as type_won_cnt,
//																format(sum(case when stages = 'Closed(Won)' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end),'N2') as type_won_mb,
//																sum(case when stages = 'Closed(Lost)' then 1 else 0 end) as type_lost_cnt,
//																format(sum(case when stages = 'Closed(Lost)' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end),'N2') as type_lost_mb,
//																sum(case when stages = 'No go' then 1 else 0 end) as type_nogo_cnt,
//																format(sum(case when stages = 'No go' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end),'N2') as type_nogo_mb,
//																sum(case when stages is not null and stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then 1 else 0 end) as type_pending_cnt,
//																format(sum(case when stages is not null and stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end),'N2') as type_pending_mb

//                                                            from Quotation

//                                                            where department = 'ENG' and left(Convert(varchar, date,23),7) between '2022-02' and '2022-02' and product_type<> ''
//															group by department, sale_name, product_type) as sub_type

//                                                        group by sub_type.department, sub_type.sale_name
//													) as sub_sub_type

//                                                     ON sub_sub_type.sale_name = Quotation.sale_name

//                                                    left join (
//                                                        select sub_stages.department, sub_stages.sale_name,
//                                                            sum(case when stages = 'Closed(Won)' then sub_stages.stages_project_cnt else 0 end) as won_project_cnt,
//															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_project_mb as float) else 0 end) as won_project_mb,
//															sum(case when stages = 'Closed(Won)' then sub_stages.stages_product_cnt else 0 end) as won_product_cnt,
//															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_product_mb as float) else 0 end) as won_product_mb,
//															sum(case when stages = 'Closed(Won)' then sub_stages.stages_service_cnt else 0 end) as won_service_cnt,
//															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_service_mb as float) else 0 end) as won_service_mb,

//															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_project_cnt else 0 end) as lost_project_cnt,
//															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_project_mb as float) else 0 end) as lost_project_mb,
//															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_product_cnt else 0 end) as lost_product_cnt,
//															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_product_mb as float) else 0 end) as lost_product_mb,
//															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_service_cnt else 0 end) as lost_service_cnt,
//															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_service_mb as float) else 0 end) as lost_service_mb,

//															sum(case when stages = 'No go' then sub_stages.stages_project_cnt else 0 end) as nogo_project_cnt,
//															sum(case when stages = 'No go' then cast(sub_stages.stages_project_mb as float) else 0 end) as nogo_project_mb,
//															sum(case when stages = 'No go' then sub_stages.stages_product_cnt else 0 end) as nogo_product_cnt,
//															sum(case when stages = 'No go' then cast(sub_stages.stages_product_mb as float) else 0 end) as nogo_product_mb,
//															sum(case when stages = 'No go' then sub_stages.stages_service_cnt else 0 end) as nogo_service_cnt,
//															sum(case when stages = 'No go' then cast(sub_stages.stages_service_mb as float) else 0 end) as nogo_service_mb

//                                                        from(
//                                                            select department,
//                                                                sale_name,
//                                                                stages,
//                                                                sum(case when product_type = 'Project' then 1 else 0 end) as stages_project_cnt,
//																format(sum(case when product_type = 'Project' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end),'N2') as stages_project_mb,
//																sum(case when product_type = 'Product' then 1 else 0 end) as stages_product_cnt,
//																format(sum(case when product_type = 'Product' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end),'N2') as stages_product_mb,
//																sum(case when product_type = 'Service' then 1 else 0 end) as stages_service_cnt,
//																format(sum(case when product_type = 'Service' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end),'N2') as stages_service_mb
//                                                             from Quotation
//                                                             where department = 'ENG' and left(Convert(varchar, date,23),7) between '2022-02' and '2022-02' and stages in('Closed(Won)','Closed(Lost)','No go')
//                                                             group by department, sale_name, stages ) as sub_stages


//                                                        group by sub_stages.department, sub_stages.sale_name 
//													) as sub_sub_stages

//                                                    ON Quotation.sale_name = sub_sub_stages.sale_name

//                                                    left join (
//                                                    select sub_pending.department, sub_pending.sale_name,
//                                                            sum(case when product_type = 'Product' then stages_pending_cnt else 0 end ) as product_pending_cnt,
//															sum(case when product_type = 'Product' then cast(stages_pending_mb as float) else 0 end ) as product_pending_mb,
//															sum(case when product_type = 'Project' then stages_pending_cnt else 0 end ) as project_pending_cnt,
//															sum(case when product_type = 'Project' then cast(stages_pending_mb as float) else 0 end ) as project_pending_mb,
//															sum(case when product_type = 'Service' then stages_pending_cnt else 0 end ) as service_pending_cnt,
//															sum(case when product_type = 'Service' then cast(stages_pending_mb as float) else 0 end ) as service_pending_mb

//                                                             from(
//                                                            select sub_stages.department,
//                                                            sub_stages.sale_name,
//                                                            'pending' as stages,
//                                                            sub_stages.product_type,
//                                                            sum(cast(sub_stages.stages_pending_cnt as float)) as stages_pending_cnt,
//                                                            format(sum(cast(sub_stages.stages_pending_mb as float)), 'N2') as stages_pending_mb

//                                                            from(
//                                                                select department,
//                                                                sale_name,
//                                                                'Pending' as stages,
//                                                                product_type,
//                                                                sum(case when product_type is not null or product_type not in ('', 'Project', 'Product', 'Service') then 1 else 0 end) as stages_pending_cnt,
//		                                                        sum(case when product_type is not null or product_type not in ('', 'Project', 'Product', 'Service') then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end) as stages_pending_mb

//                                                                from Quotation

//                                                                where department = 'ENG' and left(Convert(varchar, date,23),7) between '2022-02' and '2022-02' and stages<> '' 
//		                                                        group by department, sale_name, product_type, stages having stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go')) as sub_stages
//                                                        group by sub_stages.department, sub_stages.sale_name, sub_stages.product_type) as sub_pending


//                                                        group by sub_pending.department, sub_pending.sale_name
//													) as sub_sub_pending


//                                                    ON Quotation.sale_name = sub_sub_pending.sale_name

//                                                    where Quotation.department= 'ENG' and left(Convert(varchar, Quotation.date,23),7) between '2022-02' and '2022-02' group by Quotation.department, Quotation.sale_name


