using System;
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

                if (department == "ALL")
                {
                    command = string.Format($@"													
													DECLARE @million as float
													DECLARE @month_first as VARCHAR(10)
													DECLARE @month_last as VARCHAR(10)
													SET @million = 1000000		
													SET @month_first = '{month_first}';
													SET @month_last = '{month_last}';
													with main As(
													
													select Quotation.department, Quotation.sale_name as sale,
													count(quotation_no) as quo_cnt,
													cast(sum(cast(replace(quoted_price,',','') as float))/ @million as decimal(10, 2)) as quo_mb,                                                
													sum(sub_sub_type.product_won_cnt) / count(sub_sub_type.product_won_cnt) as product_won_cnt ,
													cast(sum(sub_sub_type.product_won_mb) / count(sub_sub_type.product_won_mb) as decimal(10, 2)) as product_won_mb,
													sum(sub_sub_type.product_lost_cnt) / count(sub_sub_type.product_lost_cnt) as product_lost_cnt ,
													cast(sum(sub_sub_type.product_lost_mb) / count(sub_sub_type.product_lost_mb) as decimal(10, 2)) as product_lost_mb,
													sum(sub_sub_type.product_nogo_cnt) / count(sub_sub_type.product_nogo_cnt) as product_nogo_cnt ,
													cast(sum(sub_sub_type.product_nogo_mb) / count(sub_sub_type.product_nogo_mb) as decimal(10, 2)) as product_nogo_mb,
													sum(sub_sub_type.product_pending_cnt) / count(sub_sub_type.product_pending_cnt) as product_pending_cnt ,
													cast(sum(sub_sub_type.product_pending_mb) / count(sub_sub_type.product_pending_mb) as decimal(10, 2)) as product_pending_mb,
                                                    sum(case when Quotation.product_type = 'product' and (stages is not null and stages <> '') then 1 else 0 end) as product_cnt,
													sum(case when Quotation.product_type = 'product' and (stages is not null and stages <> '') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as product_mb,

													sum(sub_sub_type.project_won_cnt) / count(sub_sub_type.project_won_cnt) as project_won_cnt ,
													cast(sum(sub_sub_type.project_won_mb) / count(sub_sub_type.project_won_mb) as decimal(10, 2)) as project_won_mb,
													sum(sub_sub_type.project_lost_cnt) / count(sub_sub_type.project_lost_cnt) as project_lost_cnt ,
													cast(sum(sub_sub_type.project_lost_mb) / count(sub_sub_type.project_lost_mb) as decimal(10, 2)) as project_lost_mb,
													sum(sub_sub_type.project_nogo_cnt) / count(sub_sub_type.project_nogo_cnt) as project_nogo_cnt ,
													cast(sum(sub_sub_type.project_nogo_mb) / count(sub_sub_type.project_nogo_mb) as decimal(10, 2)) as project_nogo_mb,
													sum(sub_sub_type.project_pending_cnt) / count(sub_sub_type.project_pending_cnt) as project_pending_cnt ,
													cast(sum(sub_sub_type.project_pending_mb) / count(sub_sub_type.project_pending_mb) as decimal(10, 2)) as project_pending_mb,
                                                    sum(case when Quotation.product_type = 'project' and (stages is not null and stages <> '') then 1 else 0 end) as project_cnt,
													sum(case when Quotation.product_type = 'project' and (stages is not null and stages <> '') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as project_mb,

													sum(sub_sub_type.service_won_cnt) / count(sub_sub_type.service_won_cnt) as service_won_cnt ,
													cast(sum(sub_sub_type.service_won_mb) / count(sub_sub_type.service_won_mb) as decimal(10, 2)) as service_won_mb,
													sum(sub_sub_type.service_lost_cnt) / count(sub_sub_type.service_lost_cnt) as service_lost_cnt ,
													cast(sum(sub_sub_type.service_lost_mb) / count(sub_sub_type.service_lost_mb) as decimal(10, 2)) as service_lost_mb,
													sum(sub_sub_type.service_nogo_cnt) / count(sub_sub_type.service_nogo_cnt) as service_nogo_cnt ,
													cast(sum(sub_sub_type.service_nogo_mb) / count(sub_sub_type.service_nogo_mb) as decimal(10, 2)) as service_nogo_mb,
													sum(sub_sub_type.service_pending_cnt) / count(sub_sub_type.service_pending_cnt) as service_pending_cnt ,
													cast(sum(sub_sub_type.service_pending_mb) / count(sub_sub_type.service_pending_mb) as decimal(10, 2)) as service_pending_mb,
                                                    sum(case when Quotation.product_type = 'service' and (stages is not null and stages <> '') then 1 else 0 end) as service_cnt,
													sum(case when Quotation.product_type = 'service' and (stages is not null and stages <> '') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as service_mb,
                                                    
													sum(sub_sub_stages.won_product_cnt) / count(sub_sub_stages.won_product_cnt) as won_product_cnt,
													cast(sum(sub_sub_stages.won_product_mb) / count(sub_sub_stages.won_product_mb) as decimal(10, 2)) as won_product_mb,
													sum(sub_sub_stages.won_project_cnt) / count(sub_sub_stages.won_project_cnt) as won_project_cnt,
													cast(sum(sub_sub_stages.won_project_mb) / count(sub_sub_stages.won_project_mb) as decimal(10, 2)) as won_project_mb,
													sum(sub_sub_stages.won_service_cnt) / count(sub_sub_stages.won_service_cnt) as won_service_cnt,
													cast(sum(sub_sub_stages.won_service_mb) / count(sub_sub_stages.won_service_mb) as decimal(10, 2)) as won_service_mb,
													sum(case when stages = 'Closed(Won)' then 1 else 0 end) as won_quo_cnt,
                                                    sum(case when stages = 'Closed(Won)' then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as won_mb,
                                                    
													sum(sub_sub_stages.lost_product_cnt) / count(sub_sub_stages.lost_product_cnt) as lost_product_cnt,
													cast(sum(sub_sub_stages.lost_product_mb) / count(sub_sub_stages.lost_product_mb) as decimal(10, 2)) as lost_product_mb,
													sum(sub_sub_stages.lost_project_cnt) / count(sub_sub_stages.lost_project_cnt) as lost_project_cnt,
													cast(sum(sub_sub_stages.lost_project_mb) / count(sub_sub_stages.lost_project_mb) as decimal(10, 2)) as lost_project_mb,
													sum(sub_sub_stages.lost_service_cnt) / count(sub_sub_stages.lost_service_cnt) as lost_service_cnt,
													cast(sum(sub_sub_stages.lost_service_mb) / count(sub_sub_stages.lost_service_mb) as decimal(10, 2)) as lost_service_mb,
													sum(case when stages = 'Closed(Lost)' then 1 else 0 end) as loss_quo_cnt,
                                                    sum(case when stages = 'Closed(Lost)' then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as loss_mb,
                                                    
													sum(sub_sub_stages.nogo_product_cnt) / count(sub_sub_stages.nogo_product_cnt) as nogo_product_cnt,
													cast(sum(sub_sub_stages.nogo_product_mb) / count(sub_sub_stages.nogo_product_mb) as decimal(10, 2)) as nogo_product_mb,
													sum(sub_sub_stages.nogo_project_cnt) / count(sub_sub_stages.nogo_project_cnt) as nogo_project_cnt,
													cast(sum(sub_sub_stages.nogo_project_mb) / count(sub_sub_stages.nogo_project_mb) as decimal(10, 2)) as nogo_project_mb,
													sum(sub_sub_stages.nogo_service_cnt) / count(sub_sub_stages.nogo_service_cnt) as nogo_service_cnt,
													cast(sum(sub_sub_stages.nogo_service_mb) / count(sub_sub_stages.nogo_service_mb) as decimal(10, 2)) as nogo_service_mb,
													sum(case when stages = 'No go' then 1 else 0 end) as nogo_quo_cnt,
                                                    sum(case when stages = 'No go' then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as nogo_mb,
                                                    
													sum(sub_sub_pending.pending_product_cnt) / count(sub_sub_pending.pending_product_cnt) as pending_product_cnt,
													cast(sum(sub_sub_pending.pending_product_mb) / count(sub_sub_pending.pending_product_mb) as decimal(10, 2)) as pending_product_mb,
													sum(sub_sub_pending.pending_project_cnt) / count(sub_sub_pending.pending_project_cnt) as pending_project_cnt,
													cast(sum(sub_sub_pending.pending_project_mb) / count(sub_sub_pending.pending_project_mb) as decimal(10, 2)) as pending_project_mb,
													sum(sub_sub_pending.pending_service_cnt) / count(sub_sub_pending.pending_service_cnt) as pending_service_cnt,
													cast(sum(sub_sub_pending.pending_service_mb) / count(sub_sub_pending.pending_service_mb) as decimal(10, 2)) as pending_service_mb,
													sum(case when stages is null or stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then 1 else 0 end) as pending_quo_cnt,
                                                    sum(case when stages is null or stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as pending_mb
                                                    from Quotation
                                                    left join (

                                                        select sub_type.department,sub_type.sale_name,
															sum(case when sub_type.product_type = 'Product' then type_won_cnt else 0 end) as product_won_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_won_mb as float) else 0 end) as product_won_mb,
															sum(case when sub_type.product_type = 'Product' then type_lost_cnt else 0 end) as product_lost_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_lost_mb as float) else 0 end) as product_lost_mb,
															sum(case when sub_type.product_type = 'Product' then type_nogo_cnt else 0 end) as product_nogo_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_nogo_mb as float) else 0 end) as product_nogo_mb,
															sum(case when sub_type.product_type = 'Product' then type_pending_cnt else 0 end) as product_pending_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_pending_mb as float) else 0 end) as product_pending_mb,

															sum(case when sub_type.product_type = 'Project' then type_won_cnt else 0 end) as project_won_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_won_mb as float) else 0 end) as project_won_mb,
															sum(case when sub_type.product_type = 'Project' then type_lost_cnt else 0 end) as project_lost_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_lost_mb as float) else 0 end) as project_lost_mb,
															sum(case when sub_type.product_type = 'Project' then type_nogo_cnt else 0 end) as project_nogo_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_nogo_mb as float) else 0 end) as project_nogo_mb,
															sum(case when sub_type.product_type = 'Project' then type_pending_cnt else 0 end) as project_pending_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_pending_mb as float) else 0 end) as project_pending_mb,

															sum(case when sub_type.product_type = 'Service' then type_won_cnt else 0 end) as service_won_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_won_mb as float) else 0 end) as service_won_mb,
															sum(case when sub_type.product_type = 'Service' then type_lost_cnt else 0 end) as service_lost_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_lost_mb as float) else 0 end) as service_lost_mb,
															sum(case when sub_type.product_type = 'Service' then type_nogo_cnt else 0 end) as service_nogo_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_nogo_mb as float) else 0 end) as service_nogo_mb,
															sum(case when sub_type.product_type = 'Service' then type_pending_cnt else 0 end) as service_pending_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_pending_mb as float) else 0 end) as service_pending_mb

                                                        from(
                                                            select department,
                                                                sale_name,
                                                                product_type,
                                                                sum(case when stages = 'Closed(Won)' then 1 else 0 end) as type_won_cnt,
																format(sum(case when stages = 'Closed(Won)' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as type_won_mb,
																sum(case when stages = 'Closed(Lost)' then 1 else 0 end) as type_lost_cnt,
																format(sum(case when stages = 'Closed(Lost)' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as type_lost_mb,
																sum(case when stages = 'No go' then 1 else 0 end) as type_nogo_cnt,
																format(sum(case when stages = 'No go' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as type_nogo_mb,
																sum(case when stages is not null and stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then 1 else 0 end) as type_pending_cnt,
																format(sum(case when stages is not null and stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as type_pending_mb

                                                            from Quotation

                                                            where left(Convert(varchar, date,23),7) between @month_first and @month_last and product_type<> ''
															group by department, sale_name, product_type) as sub_type

                                                        group by sub_type.department, sub_type.sale_name
													) as sub_sub_type

                                                     ON sub_sub_type.sale_name = Quotation.sale_name

                                                    left join (
                                                        select sub_stages.department, sub_stages.sale_name,
                                                            sum(case when stages = 'Closed(Won)' then sub_stages.stages_project_cnt else 0 end) as won_project_cnt,
															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_project_mb as float) else 0 end) as won_project_mb,
															sum(case when stages = 'Closed(Won)' then sub_stages.stages_product_cnt else 0 end) as won_product_cnt,
															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_product_mb as float) else 0 end) as won_product_mb,
															sum(case when stages = 'Closed(Won)' then sub_stages.stages_service_cnt else 0 end) as won_service_cnt,
															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_service_mb as float) else 0 end) as won_service_mb,

															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_project_cnt else 0 end) as lost_project_cnt,
															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_project_mb as float) else 0 end) as lost_project_mb,
															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_product_cnt else 0 end) as lost_product_cnt,
															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_product_mb as float) else 0 end) as lost_product_mb,
															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_service_cnt else 0 end) as lost_service_cnt,
															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_service_mb as float) else 0 end) as lost_service_mb,

															sum(case when stages = 'No go' then sub_stages.stages_project_cnt else 0 end) as nogo_project_cnt,
															sum(case when stages = 'No go' then cast(sub_stages.stages_project_mb as float) else 0 end) as nogo_project_mb,
															sum(case when stages = 'No go' then sub_stages.stages_product_cnt else 0 end) as nogo_product_cnt,
															sum(case when stages = 'No go' then cast(sub_stages.stages_product_mb as float) else 0 end) as nogo_product_mb,
															sum(case when stages = 'No go' then sub_stages.stages_service_cnt else 0 end) as nogo_service_cnt,
															sum(case when stages = 'No go' then cast(sub_stages.stages_service_mb as float) else 0 end) as nogo_service_mb

                                                        from(
                                                            select department,
                                                                sale_name,
                                                                stages,
                                                                sum(case when product_type = 'Project' then 1 else 0 end) as stages_project_cnt,
																format(sum(case when product_type = 'Project' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as stages_project_mb,
																sum(case when product_type = 'Product' then 1 else 0 end) as stages_product_cnt,
																format(sum(case when product_type = 'Product' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as stages_product_mb,
																sum(case when product_type = 'Service' then 1 else 0 end) as stages_service_cnt,
																format(sum(case when product_type = 'Service' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as stages_service_mb
                                                             from Quotation
                                                             where left(Convert(varchar, date,23),7) between @month_first and @month_last and stages in('Closed(Won)','Closed(Lost)','No go')
                                                             group by department, sale_name, stages ) as sub_stages


                                                        group by sub_stages.department, sub_stages.sale_name 
													) as sub_sub_stages

                                                    ON Quotation.sale_name = sub_sub_stages.sale_name

                                                    left join (
                                                    select sub_pending.department, sub_pending.sale_name,
                                                            sum(case when product_type = 'Product' then stages_pending_cnt else 0 end ) as pending_product_cnt,
															sum(case when product_type = 'Product' then cast(stages_pending_mb as float) else 0 end ) as pending_product_mb,
															sum(case when product_type = 'Project' then stages_pending_cnt else 0 end ) as pending_project_cnt,
															sum(case when product_type = 'Project' then cast(stages_pending_mb as float) else 0 end ) as pending_project_mb,
															sum(case when product_type = 'Service' then stages_pending_cnt else 0 end ) as pending_service_cnt,
															sum(case when product_type = 'Service' then cast(stages_pending_mb as float) else 0 end ) as pending_service_mb

                                                             from(
                                                            select sub_stages.department,
                                                            sub_stages.sale_name,
                                                            'pending' as stages,
                                                            sub_stages.product_type,
                                                            sum(cast(sub_stages.stages_pending_cnt as float)) as stages_pending_cnt,
                                                            format(sum(cast(sub_stages.stages_pending_mb as float)), 'N2') as stages_pending_mb

                                                            from(
                                                                select department,
                                                                sale_name,
                                                                'Pending' as stages,
                                                                product_type,
                                                                sum(case when product_type in ('Project', 'Product', 'Service') then 1 else 0 end) as stages_pending_cnt,
		                                                        sum(case when product_type in ('Project', 'Product', 'Service') then cast(replace(quoted_price, ',', '') as float) / @million else 0 end) as stages_pending_mb

                                                                from Quotation

                                                                where left(Convert(varchar, date,23),7) between @month_first and @month_last and stages<> '' 
		                                                        group by department, sale_name, product_type, stages having stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go')) as sub_stages
                                                        group by sub_stages.department, sub_stages.sale_name, sub_stages.product_type) as sub_pending


                                                        group by sub_pending.department, sub_pending.sale_name
													) as sub_sub_pending


                                                    ON Quotation.sale_name = sub_sub_pending.sale_name

                                                    where left(Convert(varchar, Quotation.date,23),7) between @month_first and @month_last 
													group by Quotation.department, Quotation.sale_name)
                                                    -- Name
													select * from main
													
													union all
                                                    -- Department
													select main.department + '_Total' as department,
															'' as sale,
															sum(main.quo_cnt) as quo_cnt,
															sum(main.quo_mb) as quo_mb,
															sum(main.product_won_cnt) as product_won_cnt ,
															sum(main.product_won_mb) as product_won_mb,
															sum(main.product_lost_cnt) as product_lost_cnt ,
															sum(main.product_lost_mb) as product_lost_mb,
															sum(main.product_nogo_cnt)  as product_nogo_cnt ,
															sum(main.product_nogo_mb)  as product_nogo_mb,
															sum(main.product_pending_cnt)  as product_pending_cnt ,
															sum(main.product_pending_mb) as product_pending_mb,
															sum(main.product_cnt) as product_cnt,
															sum(main.product_mb) as product_mb,

															sum(main.project_won_cnt) as project_won_cnt ,
															sum(main.project_won_mb) as project_won_mb,
															sum(main.project_lost_cnt) as project_lost_cnt ,
															sum(main.project_lost_mb) as project_lost_mb,
															sum(main.project_nogo_cnt) as project_nogo_cnt ,
															sum(main.project_nogo_mb) as project_nogo_mb,
															sum(main.project_pending_cnt) as project_pending_cnt ,
															sum(main.project_pending_mb) as project_pending_mb,
															sum(main.project_cnt) as project_cnt,
															sum(main.project_mb) as project_mb,

															sum(main.service_won_cnt) as service_won_cnt ,
															sum(main.service_won_mb) as service_won_mb,
															sum(main.service_lost_cnt) as service_lost_cnt ,
															sum(main.service_lost_mb) as service_lost_mb,
															sum(main.service_nogo_cnt) as service_nogo_cnt ,
															sum(main.service_nogo_mb)  as service_nogo_mb,
															sum(main.service_pending_cnt) as service_pending_cnt ,
															sum(main.service_pending_mb) as service_pending_mb,
															sum(main.service_cnt) as service_cnt,
															sum(main.service_mb) as service_mb,
                                                    
															sum(main.won_product_cnt) as won_product_cnt,
															sum(main.won_product_mb) as won_product_mb,
															sum(main.won_project_cnt) as won_project_cnt,
															sum(main.won_project_mb) as won_project_mb,
															sum(main.won_service_cnt) as won_service_cnt,
															sum(main.won_service_mb) as won_service_mb,
															sum(main.won_quo_cnt) as won_quo_cnt,
															sum(main.won_mb) as won_mb,
                                                    
															sum(main.lost_product_cnt) as lost_product_cnt,
															sum(main.lost_product_mb) as lost_product_mb,
															sum(main.lost_project_cnt) as lost_project_cnt,
															sum(main.lost_project_mb) as lost_project_mb,
															sum(main.lost_service_cnt) as lost_service_cnt,
															sum(main.lost_service_mb) as lost_service_mb,
															sum(main.loss_quo_cnt) as loss_quo_cnt,
															sum(main.loss_mb) as loss_mb,
                                                    
															sum(main.nogo_product_cnt) as nogo_product_cnt,
															sum(main.nogo_product_mb) as nogo_product_mb,
															sum(main.nogo_project_cnt) as nogo_project_cnt,
															sum(main.nogo_project_mb) as nogo_project_mb,
															sum(main.nogo_service_cnt) as nogo_service_cnt,
															sum(main.nogo_service_mb) as nogo_service_mb,
															sum(main.nogo_quo_cnt) as nogo_quo_cnt,
															sum(main.nogo_mb) as nogo_mb,
                                                    
															sum(main.pending_product_cnt) as pending_product_cnt,
															sum(main.pending_product_mb) as pending_product_mb,
															sum(main.pending_project_cnt) as pending_project_cnt,
															sum(main.pending_project_mb) as pending_project_mb,
															sum(main.pending_service_cnt) as pending_service_cnt,
															sum(main.pending_service_mb) as pending_service_mb,
															sum(main.pending_quo_cnt) as pending_quo_cnt,
															sum(main.pending_mb) as pending_mb
		
                                                    from main group by main.department
													 
													union all
                                                    --Total
													select 'Total' as department,
															'' as sale,
															sum(main.quo_cnt) as quo_cnt,
															sum(main.quo_mb) as quo_mb,
															sum(main.product_won_cnt) as product_won_cnt ,
															sum(main.product_won_mb) as product_won_mb,
															sum(main.product_lost_cnt) as product_lost_cnt ,
															sum(main.product_lost_mb) as product_lost_mb,
															sum(main.product_nogo_cnt)  as product_nogo_cnt ,
															sum(main.product_nogo_mb)  as product_nogo_mb,
															sum(main.product_pending_cnt)  as product_pending_cnt ,
															sum(main.product_pending_mb) as product_pending_mb,
															sum(main.product_cnt) as product_cnt,
															sum(main.product_mb) as product_mb,

															sum(main.project_won_cnt) as project_won_cnt ,
															sum(main.project_won_mb) as project_won_mb,
															sum(main.project_lost_cnt) as project_lost_cnt ,
															sum(main.project_lost_mb) as project_lost_mb,
															sum(main.project_nogo_cnt) as project_nogo_cnt ,
															sum(main.project_nogo_mb) as project_nogo_mb,
															sum(main.project_pending_cnt) as project_pending_cnt ,
															sum(main.project_pending_mb) as project_pending_mb,
															sum(main.project_cnt) as project_cnt,
															sum(main.project_mb) as project_mb,

															sum(main.service_won_cnt) as service_won_cnt ,
															sum(main.service_won_mb) as service_won_mb,
															sum(main.service_lost_cnt) as service_lost_cnt ,
															sum(main.service_lost_mb) as service_lost_mb,
															sum(main.service_nogo_cnt) as service_nogo_cnt ,
															sum(main.service_nogo_mb)  as service_nogo_mb,
															sum(main.service_pending_cnt) as service_pending_cnt ,
															sum(main.service_pending_mb) as service_pending_mb,
															sum(main.service_cnt) as service_cnt,
															sum(main.service_mb) as service_mb,
                                                    
															sum(main.won_product_cnt) as won_product_cnt,
															sum(main.won_product_mb) as won_product_mb,
															sum(main.won_project_cnt) as won_project_cnt,
															sum(main.won_project_mb) as won_project_mb,
															sum(main.won_service_cnt) as won_service_cnt,
															sum(main.won_service_mb) as won_service_mb,
															sum(main.won_quo_cnt) as won_quo_cnt,
															sum(main.won_mb) as won_mb,
                                                    
															sum(main.lost_product_cnt) as lost_product_cnt,
															sum(main.lost_product_mb) as lost_product_mb,
															sum(main.lost_project_cnt) as lost_project_cnt,
															sum(main.lost_project_mb) as lost_project_mb,
															sum(main.lost_service_cnt) as lost_service_cnt,
															sum(main.lost_service_mb) as lost_service_mb,
															sum(main.loss_quo_cnt) as loss_quo_cnt,
															sum(main.loss_mb) as loss_mb,
                                                    
															sum(main.nogo_product_cnt) as nogo_product_cnt,
															sum(main.nogo_product_mb) as nogo_product_mb,
															sum(main.nogo_project_cnt) as nogo_project_cnt,
															sum(main.nogo_project_mb) as nogo_project_mb,
															sum(main.nogo_service_cnt) as nogo_service_cnt,
															sum(main.nogo_service_mb) as nogo_service_mb,
															sum(main.nogo_quo_cnt) as nogo_quo_cnt,
															sum(main.nogo_mb) as nogo_mb,
                                                    
															sum(main.pending_product_cnt) as pending_product_cnt,
															sum(main.pending_product_mb) as pending_product_mb,
															sum(main.pending_project_cnt) as pending_project_cnt,
															sum(main.pending_project_mb) as pending_project_mb,
															sum(main.pending_service_cnt) as pending_service_cnt,
															sum(main.pending_service_mb) as pending_service_mb,
															sum(main.pending_quo_cnt) as pending_quo_cnt,
															sum(main.pending_mb) as pending_mb
		
                                                    from main 
													order by  main.department");

                }
                else
                {
                    command = string.Format($@"		DECLARE @department as VARCHAR(10)
													DECLARE @million as float
													DECLARE @month_first as VARCHAR(10)
													DECLARE @month_last as VARCHAR(10)
													SET @department = '{department}';
													SET @million = 1000000		
													SET @month_first = '{month_first}';
													SET @month_last = '{month_last}';
													with main As(
													
													select Quotation.department, Quotation.sale_name as sale,
													count(quotation_no) as quo_cnt,
													cast(sum(cast(replace(quoted_price,',','') as float))/ @million as decimal(10, 2)) as quo_mb,                                                
													sum(sub_sub_type.product_won_cnt) / count(sub_sub_type.product_won_cnt) as product_won_cnt ,
													cast(sum(sub_sub_type.product_won_mb) / count(sub_sub_type.product_won_mb) as decimal(10, 2)) as product_won_mb,
													sum(sub_sub_type.product_lost_cnt) / count(sub_sub_type.product_lost_cnt) as product_lost_cnt ,
													cast(sum(sub_sub_type.product_lost_mb) / count(sub_sub_type.product_lost_mb) as decimal(10, 2)) as product_lost_mb,
													sum(sub_sub_type.product_nogo_cnt) / count(sub_sub_type.product_nogo_cnt) as product_nogo_cnt ,
													cast(sum(sub_sub_type.product_nogo_mb) / count(sub_sub_type.product_nogo_mb) as decimal(10, 2)) as product_nogo_mb,
													sum(sub_sub_type.product_pending_cnt) / count(sub_sub_type.product_pending_cnt) as product_pending_cnt ,
													cast(sum(sub_sub_type.product_pending_mb) / count(sub_sub_type.product_pending_mb) as decimal(10, 2)) as product_pending_mb,
                                                    sum(case when Quotation.product_type = 'product' and (stages is not null and stages <> '') then 1 else 0 end) as product_cnt,
													sum(case when Quotation.product_type = 'product' and (stages is not null and stages <> '') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as product_mb,

													sum(sub_sub_type.project_won_cnt) / count(sub_sub_type.project_won_cnt) as project_won_cnt ,
													cast(sum(sub_sub_type.project_won_mb) / count(sub_sub_type.project_won_mb) as decimal(10, 2)) as project_won_mb,
													sum(sub_sub_type.project_lost_cnt) / count(sub_sub_type.project_lost_cnt) as project_lost_cnt ,
													cast(sum(sub_sub_type.project_lost_mb) / count(sub_sub_type.project_lost_mb) as decimal(10, 2)) as project_lost_mb,
													sum(sub_sub_type.project_nogo_cnt) / count(sub_sub_type.project_nogo_cnt) as project_nogo_cnt ,
													cast(sum(sub_sub_type.project_nogo_mb) / count(sub_sub_type.project_nogo_mb) as decimal(10, 2)) as project_nogo_mb,
													sum(sub_sub_type.project_pending_cnt) / count(sub_sub_type.project_pending_cnt) as project_pending_cnt ,
													cast(sum(sub_sub_type.project_pending_mb) / count(sub_sub_type.project_pending_mb) as decimal(10, 2)) as project_pending_mb,
                                                    sum(case when Quotation.product_type = 'project' and (stages is not null and stages <> '') then 1 else 0 end) as project_cnt,
													sum(case when Quotation.product_type = 'project' and (stages is not null and stages <> '') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as project_mb,

													sum(sub_sub_type.service_won_cnt) / count(sub_sub_type.service_won_cnt) as service_won_cnt ,
													cast(sum(sub_sub_type.service_won_mb) / count(sub_sub_type.service_won_mb) as decimal(10, 2)) as service_won_mb,
													sum(sub_sub_type.service_lost_cnt) / count(sub_sub_type.service_lost_cnt) as service_lost_cnt ,
													cast(sum(sub_sub_type.service_lost_mb) / count(sub_sub_type.service_lost_mb) as decimal(10, 2)) as service_lost_mb,
													sum(sub_sub_type.service_nogo_cnt) / count(sub_sub_type.service_nogo_cnt) as service_nogo_cnt ,
													cast(sum(sub_sub_type.service_nogo_mb) / count(sub_sub_type.service_nogo_mb) as decimal(10, 2)) as service_nogo_mb,
													sum(sub_sub_type.service_pending_cnt) / count(sub_sub_type.service_pending_cnt) as service_pending_cnt ,
													cast(sum(sub_sub_type.service_pending_mb) / count(sub_sub_type.service_pending_mb) as decimal(10, 2)) as service_pending_mb,
                                                    sum(case when Quotation.product_type = 'service' and (stages is not null and stages <> '') then 1 else 0 end) as service_cnt,
													sum(case when Quotation.product_type = 'service' and (stages is not null and stages <> '') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as service_mb,
                                                    
													sum(sub_sub_stages.won_product_cnt) / count(sub_sub_stages.won_product_cnt) as won_product_cnt,
													cast(sum(sub_sub_stages.won_product_mb) / count(sub_sub_stages.won_product_mb) as decimal(10, 2)) as won_product_mb,
													sum(sub_sub_stages.won_project_cnt) / count(sub_sub_stages.won_project_cnt) as won_project_cnt,
													cast(sum(sub_sub_stages.won_project_mb) / count(sub_sub_stages.won_project_mb) as decimal(10, 2)) as won_project_mb,
													sum(sub_sub_stages.won_service_cnt) / count(sub_sub_stages.won_service_cnt) as won_service_cnt,
													cast(sum(sub_sub_stages.won_service_mb) / count(sub_sub_stages.won_service_mb) as decimal(10, 2)) as won_service_mb,
													sum(case when stages = 'Closed(Won)' then 1 else 0 end) as won_quo_cnt,
                                                    sum(case when stages = 'Closed(Won)' then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as won_mb,
                                                    
													sum(sub_sub_stages.lost_product_cnt) / count(sub_sub_stages.lost_product_cnt) as lost_product_cnt,
													cast(sum(sub_sub_stages.lost_product_mb) / count(sub_sub_stages.lost_product_mb) as decimal(10, 2)) as lost_product_mb,
													sum(sub_sub_stages.lost_project_cnt) / count(sub_sub_stages.lost_project_cnt) as lost_project_cnt,
													cast(sum(sub_sub_stages.lost_project_mb) / count(sub_sub_stages.lost_project_mb) as decimal(10, 2)) as lost_project_mb,
													sum(sub_sub_stages.lost_service_cnt) / count(sub_sub_stages.lost_service_cnt) as lost_service_cnt,
													cast(sum(sub_sub_stages.lost_service_mb) / count(sub_sub_stages.lost_service_mb) as decimal(10, 2)) as lost_service_mb,
													sum(case when stages = 'Closed(Lost)' then 1 else 0 end) as loss_quo_cnt,
                                                    sum(case when stages = 'Closed(Lost)' then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as loss_mb,
                                                    
													sum(sub_sub_stages.nogo_product_cnt) / count(sub_sub_stages.nogo_product_cnt) as nogo_product_cnt,
													cast(sum(sub_sub_stages.nogo_product_mb) / count(sub_sub_stages.nogo_product_mb) as decimal(10, 2)) as nogo_product_mb,
													sum(sub_sub_stages.nogo_project_cnt) / count(sub_sub_stages.nogo_project_cnt) as nogo_project_cnt,
													cast(sum(sub_sub_stages.nogo_project_mb) / count(sub_sub_stages.nogo_project_mb) as decimal(10, 2)) as nogo_project_mb,
													sum(sub_sub_stages.nogo_service_cnt) / count(sub_sub_stages.nogo_service_cnt) as nogo_service_cnt,
													cast(sum(sub_sub_stages.nogo_service_mb) / count(sub_sub_stages.nogo_service_mb) as decimal(10, 2)) as nogo_service_mb,
													sum(case when stages = 'No go' then 1 else 0 end) as nogo_quo_cnt,
                                                    sum(case when stages = 'No go' then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as nogo_mb,
                                                    
													sum(sub_sub_pending.pending_product_cnt) / count(sub_sub_pending.pending_product_cnt) as pending_product_cnt,
													cast(sum(sub_sub_pending.pending_product_mb) / count(sub_sub_pending.pending_product_mb) as decimal(10, 2)) as pending_product_mb,
													sum(sub_sub_pending.pending_project_cnt) / count(sub_sub_pending.pending_project_cnt) as pending_project_cnt,
													cast(sum(sub_sub_pending.pending_project_mb) / count(sub_sub_pending.pending_project_mb) as decimal(10, 2)) as pending_project_mb,
													sum(sub_sub_pending.pending_service_cnt) / count(sub_sub_pending.pending_service_cnt) as pending_service_cnt,
													cast(sum(sub_sub_pending.pending_service_mb) / count(sub_sub_pending.pending_service_mb) as decimal(10, 2)) as pending_service_mb,
													sum(case when stages is null or stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then 1 else 0 end) as pending_quo_cnt,
                                                    sum(case when stages is null or stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as pending_mb
                                                    from Quotation
                                                    left join (

                                                        select sub_type.department,sub_type.sale_name,
															sum(case when sub_type.product_type = 'Product' then type_won_cnt else 0 end) as product_won_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_won_mb as float) else 0 end) as product_won_mb,
															sum(case when sub_type.product_type = 'Product' then type_lost_cnt else 0 end) as product_lost_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_lost_mb as float) else 0 end) as product_lost_mb,
															sum(case when sub_type.product_type = 'Product' then type_nogo_cnt else 0 end) as product_nogo_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_nogo_mb as float) else 0 end) as product_nogo_mb,
															sum(case when sub_type.product_type = 'Product' then type_pending_cnt else 0 end) as product_pending_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_pending_mb as float) else 0 end) as product_pending_mb,

															sum(case when sub_type.product_type = 'Project' then type_won_cnt else 0 end) as project_won_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_won_mb as float) else 0 end) as project_won_mb,
															sum(case when sub_type.product_type = 'Project' then type_lost_cnt else 0 end) as project_lost_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_lost_mb as float) else 0 end) as project_lost_mb,
															sum(case when sub_type.product_type = 'Project' then type_nogo_cnt else 0 end) as project_nogo_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_nogo_mb as float) else 0 end) as project_nogo_mb,
															sum(case when sub_type.product_type = 'Project' then type_pending_cnt else 0 end) as project_pending_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_pending_mb as float) else 0 end) as project_pending_mb,

															sum(case when sub_type.product_type = 'Service' then type_won_cnt else 0 end) as service_won_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_won_mb as float) else 0 end) as service_won_mb,
															sum(case when sub_type.product_type = 'Service' then type_lost_cnt else 0 end) as service_lost_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_lost_mb as float) else 0 end) as service_lost_mb,
															sum(case when sub_type.product_type = 'Service' then type_nogo_cnt else 0 end) as service_nogo_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_nogo_mb as float) else 0 end) as service_nogo_mb,
															sum(case when sub_type.product_type = 'Service' then type_pending_cnt else 0 end) as service_pending_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_pending_mb as float) else 0 end) as service_pending_mb

                                                        from(
                                                            select department,
                                                                sale_name,
                                                                product_type,
                                                                sum(case when stages = 'Closed(Won)' then 1 else 0 end) as type_won_cnt,
																format(sum(case when stages = 'Closed(Won)' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as type_won_mb,
																sum(case when stages = 'Closed(Lost)' then 1 else 0 end) as type_lost_cnt,
																format(sum(case when stages = 'Closed(Lost)' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as type_lost_mb,
																sum(case when stages = 'No go' then 1 else 0 end) as type_nogo_cnt,
																format(sum(case when stages = 'No go' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as type_nogo_mb,
																sum(case when stages is not null and stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then 1 else 0 end) as type_pending_cnt,
																format(sum(case when stages is not null and stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as type_pending_mb

                                                            from Quotation

                                                            where department = @department and left(Convert(varchar, date,23),7) between @month_first and @month_last and product_type<> ''
															group by department, sale_name, product_type) as sub_type

                                                        group by sub_type.department, sub_type.sale_name
													) as sub_sub_type

                                                     ON sub_sub_type.sale_name = Quotation.sale_name

                                                    left join (
                                                        select sub_stages.department, sub_stages.sale_name,
                                                            sum(case when stages = 'Closed(Won)' then sub_stages.stages_project_cnt else 0 end) as won_project_cnt,
															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_project_mb as float) else 0 end) as won_project_mb,
															sum(case when stages = 'Closed(Won)' then sub_stages.stages_product_cnt else 0 end) as won_product_cnt,
															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_product_mb as float) else 0 end) as won_product_mb,
															sum(case when stages = 'Closed(Won)' then sub_stages.stages_service_cnt else 0 end) as won_service_cnt,
															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_service_mb as float) else 0 end) as won_service_mb,

															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_project_cnt else 0 end) as lost_project_cnt,
															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_project_mb as float) else 0 end) as lost_project_mb,
															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_product_cnt else 0 end) as lost_product_cnt,
															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_product_mb as float) else 0 end) as lost_product_mb,
															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_service_cnt else 0 end) as lost_service_cnt,
															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_service_mb as float) else 0 end) as lost_service_mb,

															sum(case when stages = 'No go' then sub_stages.stages_project_cnt else 0 end) as nogo_project_cnt,
															sum(case when stages = 'No go' then cast(sub_stages.stages_project_mb as float) else 0 end) as nogo_project_mb,
															sum(case when stages = 'No go' then sub_stages.stages_product_cnt else 0 end) as nogo_product_cnt,
															sum(case when stages = 'No go' then cast(sub_stages.stages_product_mb as float) else 0 end) as nogo_product_mb,
															sum(case when stages = 'No go' then sub_stages.stages_service_cnt else 0 end) as nogo_service_cnt,
															sum(case when stages = 'No go' then cast(sub_stages.stages_service_mb as float) else 0 end) as nogo_service_mb

                                                        from(
                                                            select department,
                                                                sale_name,
                                                                stages,
                                                                sum(case when product_type = 'Project' then 1 else 0 end) as stages_project_cnt,
																format(sum(case when product_type = 'Project' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as stages_project_mb,
																sum(case when product_type = 'Product' then 1 else 0 end) as stages_product_cnt,
																format(sum(case when product_type = 'Product' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as stages_product_mb,
																sum(case when product_type = 'Service' then 1 else 0 end) as stages_service_cnt,
																format(sum(case when product_type = 'Service' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end),'N2') as stages_service_mb
                                                             from Quotation
                                                             where department = @department and left(Convert(varchar, date,23),7) between @month_first and @month_last and stages in('Closed(Won)','Closed(Lost)','No go')
                                                             group by department, sale_name, stages ) as sub_stages


                                                        group by sub_stages.department, sub_stages.sale_name 
													) as sub_sub_stages

                                                    ON Quotation.sale_name = sub_sub_stages.sale_name

                                                    left join (
                                                    select sub_pending.department, sub_pending.sale_name,
                                                            sum(case when product_type = 'Product' then stages_pending_cnt else 0 end ) as pending_product_cnt,
															sum(case when product_type = 'Product' then cast(stages_pending_mb as float) else 0 end ) as pending_product_mb,
															sum(case when product_type = 'Project' then stages_pending_cnt else 0 end ) as pending_project_cnt,
															sum(case when product_type = 'Project' then cast(stages_pending_mb as float) else 0 end ) as pending_project_mb,
															sum(case when product_type = 'Service' then stages_pending_cnt else 0 end ) as pending_service_cnt,
															sum(case when product_type = 'Service' then cast(stages_pending_mb as float) else 0 end ) as pending_service_mb

                                                             from(
                                                            select sub_stages.department,
                                                            sub_stages.sale_name,
                                                            'pending' as stages,
                                                            sub_stages.product_type,
                                                            sum(cast(sub_stages.stages_pending_cnt as float)) as stages_pending_cnt,
                                                            format(sum(cast(sub_stages.stages_pending_mb as float)), 'N2') as stages_pending_mb

                                                            from(
                                                                select department,
                                                                sale_name,
                                                                'Pending' as stages,
                                                                product_type,
                                                                sum(case when product_type in ('Project', 'Product', 'Service') then 1 else 0 end) as stages_pending_cnt,
		                                                        sum(case when product_type in ('Project', 'Product', 'Service') then cast(replace(quoted_price, ',', '') as float) / @million else 0 end) as stages_pending_mb

                                                                from Quotation

                                                                where department = @department and left(Convert(varchar, date,23),7) between @month_first and @month_last and stages<> '' 
		                                                        group by department, sale_name, product_type, stages having stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go')) as sub_stages
                                                        group by sub_stages.department, sub_stages.sale_name, sub_stages.product_type) as sub_pending


                                                        group by sub_pending.department, sub_pending.sale_name
													) as sub_sub_pending


                                                    ON Quotation.sale_name = sub_sub_pending.sale_name

                                                    where Quotation.department = @department and left(Convert(varchar, Quotation.date,23),7) between @month_first and @month_last 
													group by Quotation.department, Quotation.sale_name)
                                                    -- Name
													select * from main
													
													union all
                                                    -- Department
													select main.department + '_Total' as department,
															'' as sale,
															sum(main.quo_cnt) as quo_cnt,
															sum(main.quo_mb) as quo_mb,
															sum(main.product_won_cnt) as product_won_cnt ,
															sum(main.product_won_mb) as product_won_mb,
															sum(main.product_lost_cnt) as product_lost_cnt ,
															sum(main.product_lost_mb) as product_lost_mb,
															sum(main.product_nogo_cnt)  as product_nogo_cnt ,
															sum(main.product_nogo_mb)  as product_nogo_mb,
															sum(main.product_pending_cnt)  as product_pending_cnt ,
															sum(main.product_pending_mb) as product_pending_mb,
															sum(main.product_cnt) as product_cnt,
															sum(main.product_mb) as product_mb,

															sum(main.project_won_cnt) as project_won_cnt ,
															sum(main.project_won_mb) as project_won_mb,
															sum(main.project_lost_cnt) as project_lost_cnt ,
															sum(main.project_lost_mb) as project_lost_mb,
															sum(main.project_nogo_cnt) as project_nogo_cnt ,
															sum(main.project_nogo_mb) as project_nogo_mb,
															sum(main.project_pending_cnt) as project_pending_cnt ,
															sum(main.project_pending_mb) as project_pending_mb,
															sum(main.project_cnt) as project_cnt,
															sum(main.project_mb) as project_mb,

															sum(main.service_won_cnt) as service_won_cnt ,
															sum(main.service_won_mb) as service_won_mb,
															sum(main.service_lost_cnt) as service_lost_cnt ,
															sum(main.service_lost_mb) as service_lost_mb,
															sum(main.service_nogo_cnt) as service_nogo_cnt ,
															sum(main.service_nogo_mb)  as service_nogo_mb,
															sum(main.service_pending_cnt) as service_pending_cnt ,
															sum(main.service_pending_mb) as service_pending_mb,
															sum(main.service_cnt) as service_cnt,
															sum(main.service_mb) as service_mb,
                                                    
															sum(main.won_product_cnt) as won_product_cnt,
															sum(main.won_product_mb) as won_product_mb,
															sum(main.won_project_cnt) as won_project_cnt,
															sum(main.won_project_mb) as won_project_mb,
															sum(main.won_service_cnt) as won_service_cnt,
															sum(main.won_service_mb) as won_service_mb,
															sum(main.won_quo_cnt) as won_quo_cnt,
															sum(main.won_mb) as won_mb,
                                                    
															sum(main.lost_product_cnt) as lost_product_cnt,
															sum(main.lost_product_mb) as lost_product_mb,
															sum(main.lost_project_cnt) as lost_project_cnt,
															sum(main.lost_project_mb) as lost_project_mb,
															sum(main.lost_service_cnt) as lost_service_cnt,
															sum(main.lost_service_mb) as lost_service_mb,
															sum(main.loss_quo_cnt) as loss_quo_cnt,
															sum(main.loss_mb) as loss_mb,
                                                    
															sum(main.nogo_product_cnt) as nogo_product_cnt,
															sum(main.nogo_product_mb) as nogo_product_mb,
															sum(main.nogo_project_cnt) as nogo_project_cnt,
															sum(main.nogo_project_mb) as nogo_project_mb,
															sum(main.nogo_service_cnt) as nogo_service_cnt,
															sum(main.nogo_service_mb) as nogo_service_mb,
															sum(main.nogo_quo_cnt) as nogo_quo_cnt,
															sum(main.nogo_mb) as nogo_mb,
                                                    
															sum(main.pending_product_cnt) as pending_product_cnt,
															sum(main.pending_product_mb) as pending_product_mb,
															sum(main.pending_project_cnt) as pending_project_cnt,
															sum(main.pending_project_mb) as pending_project_mb,
															sum(main.pending_service_cnt) as pending_service_cnt,
															sum(main.pending_service_mb) as pending_service_mb,
															sum(main.pending_quo_cnt) as pending_quo_cnt,
															sum(main.pending_mb) as pending_mb
                                                    
													from main group by main.department
													order by  main.department");

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
                            product_won_cnt = dr["product_won_cnt"].ToString(),
                            product_won_mb = dr["product_won_mb"].ToString(),
                            product_lost_cnt = dr["product_lost_cnt"].ToString(),
                            product_lost_mb = dr["product_lost_mb"].ToString(),
                            product_nogo_cnt = dr["product_nogo_cnt"].ToString(),
                            product_nogo_mb = dr["product_nogo_mb"].ToString(),
                            product_pending_cnt = dr["product_pending_cnt"].ToString(),
                            product_pending_mb = dr["product_pending_mb"].ToString(),
                            product_cnt = dr["product_cnt"].ToString(),
                            product_mb = dr["product_mb"].ToString(),
                            project_won_cnt = dr["project_won_cnt"].ToString(),
                            project_won_mb = dr["project_won_mb"].ToString(),
                            project_lost_cnt = dr["project_lost_cnt"].ToString(),
                            project_lost_mb = dr["project_lost_mb"].ToString(),
                            project_nogo_cnt = dr["project_nogo_cnt"].ToString(),
                            project_nogo_mb = dr["project_nogo_mb"].ToString(),
                            project_pending_cnt = dr["project_pending_cnt"].ToString(),
                            project_pending_mb = dr["project_pending_mb"].ToString(),
                            project_cnt = dr["project_cnt"].ToString(),
                            project_mb = dr["project_mb"].ToString(),
                            service_won_cnt = dr["service_won_cnt"].ToString(),
                            service_won_mb = dr["service_won_mb"].ToString(),
                            service_lost_cnt = dr["service_lost_cnt"].ToString(),
                            service_lost_mb = dr["service_lost_mb"].ToString(),
                            service_nogo_cnt = dr["service_nogo_cnt"].ToString(),
                            service_nogo_mb = dr["service_nogo_mb"].ToString(),
                            service_pending_cnt = dr["service_pending_cnt"].ToString(),
                            service_pending_mb = dr["service_pending_mb"].ToString(),
                            service_cnt = dr["service_cnt"].ToString(),
                            service_mb = dr["service_mb"].ToString(),
                            won_product_cnt = dr["won_product_cnt"].ToString(),
                            won_product_mb = dr["won_product_mb"].ToString(),
                            won_project_cnt = dr["won_project_cnt"].ToString(),
                            won_project_mb = dr["won_project_mb"].ToString(),
                            won_service_cnt = dr["won_service_cnt"].ToString(),
                            won_service_mb = dr["won_service_mb"].ToString(),
                            won_quo_cnt = dr["won_quo_cnt"].ToString(),
                            won_mb = dr["won_mb"].ToString(),
                            lost_product_cnt = dr["lost_product_cnt"].ToString(),
                            lost_product_mb = dr["lost_product_mb"].ToString(),
                            lost_project_cnt = dr["lost_project_cnt"].ToString(),
                            lost_project_mb = dr["lost_project_mb"].ToString(),
                            lost_service_cnt = dr["lost_service_cnt"].ToString(),
                            lost_service_mb = dr["lost_service_mb"].ToString(),
                            loss_quo_cnt = dr["loss_quo_cnt"].ToString(),
                            loss_mb = dr["loss_mb"].ToString(),
                            nogo_product_cnt = dr["nogo_product_cnt"].ToString(),
                            nogo_product_mb = dr["nogo_product_mb"].ToString(),
                            nogo_project_cnt = dr["nogo_project_cnt"].ToString(),
                            nogo_project_mb = dr["nogo_project_mb"].ToString(),
                            nogo_service_cnt = dr["nogo_service_cnt"].ToString(),
                            nogo_service_mb = dr["nogo_service_mb"].ToString(),
                            nogo_quo_cnt = dr["nogo_quo_cnt"].ToString(),
                            nogo_mb = dr["nogo_mb"].ToString(),
                            pending_product_cnt = dr["pending_product_cnt"].ToString(),
                            pending_product_mb = dr["pending_product_mb"].ToString(),
                            pending_project_cnt = dr["pending_project_cnt"].ToString(),
                            pending_project_mb = dr["pending_project_mb"].ToString(),
                            pending_service_cnt = dr["pending_service_cnt"].ToString(),
                            pending_service_mb = dr["pending_service_mb"].ToString(),
                            pending_quo_cnt = dr["pending_quo_cnt"].ToString(),
                            pending_mb = dr["pending_mb"].ToString()
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

        public List<Quotation_Report_PendingInOutModel> GetReportPendingInOutByDepSale(string department, string month_first, string month_last)
        {
			try
			{
				List<Quotation_Report_PendingInOutModel> pendings = new List<Quotation_Report_PendingInOutModel>();
				string command = "";
				if (department == "ALL")
				{
					command = string.Format($@"  DECLARE @million as float
													DECLARE @month_first as VARCHAR(10)
													DECLARE @month_last as VARCHAR(10)
													SET @million = 1000000
													SET @month_first = '{month_first}';
													SET @month_last = '{month_last}';

													with main as (
														select department,
															sale_name,
															product_type,
															type,
															brand,
															cast(sum(case when status ='IN' 
																AND stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting')
																then cast(replace(quoted_price,',','') as float) / @million
																else 0 end ) as decimal(10, 2)) as pending_in,
															cast(sum(case when status in ('OUT','') 
																AND stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting') 
																then cast(replace(quoted_price,',','') as float) / @million 
																else 0 end ) as decimal(10, 2)) as pending_out,
															cast(sum(case when stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting') 
																then cast(replace(quoted_price,',','') as float) / @million 
																else 0 end ) as decimal(10, 2)) as pending
														from Quotation 
														where left(Convert(varchar, date,23),7) between @month_first and @month_last AND product_type IS NOT NULL and product_type <>''
														group by department,sale_name,product_type,type,brand	
													)

													select * from main  union all
													select department,
														(sale_name + '_Total') as sale_name,
														'' product_type, 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,sum(main.pending) as pending
													from main  
													group by main.department,main.sale_name union all
													select department,
														'รวม' as sale_name,
														'' product_type, 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,sum(main.pending) as pending
													from main  
													group by main.department union all
													select department,
														sale_name,
														product_type + '_Total' as product_type , 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,sum(main.pending) as pending
													from main  
													group by main.department,main.sale_name,main.product_type union all
													select 'รวม' as department,
														'' as sale_name,
														'' product_type, 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,sum(main.pending) as pending
													from main
													order by main.department,main.sale_name,main.product_type");
				}
				else
				{
					command = string.Format($@"  DECLARE @million as float
													DECLARE @month_first as VARCHAR(10)
													DECLARE @month_last as VARCHAR(10)
													DECLARE @department as VARCHAR(10)
													SET @million = 1000000
													SET @month_first = '{month_first}';
													SET @month_last = '{month_last}';
													SET @department = '{department}';

													with main as (
														select department,
															sale_name,
															product_type,
															type,
															brand,
															cast(sum(case when status ='IN' 
																AND stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting')
																then cast(replace(quoted_price,',','') as float) / @million
																else 0 end ) as decimal(10, 2)) as pending_in,
															cast(sum(case when status in ('OUT','') 
																AND stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting') 
																then cast(replace(quoted_price,',','') as float) / @million 
																else 0 end ) as decimal(10, 2)) as pending_out,
															cast(sum(case when stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting') 
																then cast(replace(quoted_price,',','') as float) / @million 
																else 0 end ) as decimal(10, 2)) as pending
														from Quotation 
														where left(Convert(varchar, date,23),7) between @month_first and @month_last and department = @department AND product_type IS NOT NULL and product_type <>''
														group by department,sale_name,product_type,type,brand	
													)

													select * from main  union all
													select department,
														(sale_name + '_Total') as sale_name,
														'' product_type, 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,sum(main.pending) as pending
													from main  
													group by main.department,main.sale_name union all
													select 'รวม' as department,
														'' as sale_name,
														'' product_type, 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,sum(main.pending) as pending
													from main  
													group by main.department
													order by main.department,main.sale_name");
				}
				SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					while (dr.Read())
					{
						Quotation_Report_PendingInOutModel p = new Quotation_Report_PendingInOutModel();

						p.department = dr["department"].ToString();
						p.sale_name = dr["sale_name"].ToString();
						p.product_type = dr["product_type"].ToString();
						if (dr["type"].ToString().Count(x => x == '|') > 0)
                        {
							p.type = dr["type"].ToString().Split('|')[0];
						}
                        else
                        {
							p.type = dr["type"].ToString();
						}
						if (dr["brand"].ToString().Count(x => x == '|') > 0)
						{
							p.brand = dr["brand"].ToString().Split('|')[0];
						}
						else
						{
							p.brand = dr["brand"].ToString();
						}
						p.pending_in = dr["pending_in"].ToString();
						p.pending_out = dr["pending_out"].ToString();
						p.pending = dr["pending"].ToString();
						
						pendings.Add(p);
					}
					dr.Close();
				}
				return pendings;
			}
			finally
			{
				if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
				{
					ConnectSQL.CloseConnect();
				}
			}
		}

        public List<Quotation_Report_PendingInOutModel> GetReportPendingInOutByDepartment(string department, string month_first, string month_last)
        {
			try
			{
				List<Quotation_Report_PendingInOutModel> pendings = new List<Quotation_Report_PendingInOutModel>();
				string command = "";
				if (department == "ALL")
				{
					command = string.Format($@"     DECLARE @million as float
													DECLARE @month_first as VARCHAR(10)
													DECLARE @month_last as VARCHAR(10)
													SET @million = 1000000
													SET @month_first = '{month_first}';
													SET @month_last = '{month_last}';
													

													with main as (
														select department,
															product_type,
															type,
															brand,
															cast(sum(case when status ='IN' 
																AND stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting')
																then cast(replace(quoted_price,',','') as float) / @million
																else 0 end ) as decimal(10, 2)) as pending_in,
															cast(sum(case when status in ('OUT','') 
																AND stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting') 
																then cast(replace(quoted_price,',','') as float) / @million 
																else 0 end ) as decimal(10, 2)) as pending_out,
															cast(sum(case when stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting') 
																then cast(replace(quoted_price,',','') as float) / @million 
																else 0 end ) as decimal(10, 2)) as pending
														from Quotation 
														where left(Convert(varchar, date,23),7) between @month_first and @month_last AND product_type IS NOT NULL and product_type <>''
														group by department,product_type,type,brand	
													)

													select * from main  union all
													select department,
														(product_type + '_Total') as product_type, 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,sum(main.pending) as pending
													from main  
													group by main.department,main.product_type union all
													select department,
														'Total' product_type, 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,sum(main.pending) as pending
													from main  
													group by main.department union all

													select 'รวม' as department,
														product_type, 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,
														sum(main.pending) as pending
													from main
													group by main.product_type union all

													select 'รวมทั้งหมด' as department,
														'' as product_type, 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,
														sum(main.pending) as pending
													from main
													order by main.department,main.product_type");
				}
				else
				{
					command = string.Format($@"  DECLARE @million as float
													DECLARE @month_first as VARCHAR(10)
													DECLARE @month_last as VARCHAR(10)
													DECLARE @department as VARCHAR(10)
													SET @million = 1000000
													SET @month_first = '{month_first}';
													SET @month_last = '{month_last}';
													SET @department = '{department}';

													with main as (
														select department,
															product_type,
															type,
															brand,
															cast(sum(case when status ='IN' 
																AND stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting')
																then cast(replace(quoted_price,',','') as float) / @million
																else 0 end ) as decimal(10, 2)) as pending_in,
															cast(sum(case when status in ('OUT','') 
																AND stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting') 
																then cast(replace(quoted_price,',','') as float) / @million 
																else 0 end ) as decimal(10, 2)) as pending_out,
															cast(sum(case when stages in ('','Quote for Budget','Negotiation/Review','Proposal/Quote for Order','Prospecting') 
																then cast(replace(quoted_price,',','') as float) / @million 
																else 0 end ) as decimal(10, 2)) as pending
														from Quotation 
														where left(Convert(varchar, date,23),7) between @month_first and @month_last and department = @department AND product_type IS NOT NULL and product_type <>''
														group by department,product_type,type,brand	
													)

													select * from main  union all
													select department,
														(product_type + '_Total') as product_type, 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,sum(main.pending) as pending
													from main  
													group by main.department,main.product_type union all
													select 'รวม' as department,
														'' as product_type, 
														'' as type, 
														'' as brand, 
														sum(main.pending_in) as pending_in,
														sum(main.pending_out) as pending_out,sum(main.pending) as pending
													from main
													group by main.department
													order by main.department,main.product_type");
				}
				SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					while (dr.Read())
					{
						Quotation_Report_PendingInOutModel p = new Quotation_Report_PendingInOutModel();

						p.department = dr["department"].ToString();
						p.product_type = dr["product_type"].ToString();
						if (dr["type"].ToString().Count(x => x == '|') > 0)
						{
							p.type = dr["type"].ToString().Split('|')[0];
						}
						else
						{
							p.type = dr["type"].ToString();
						}
						if (dr["brand"].ToString().Count(x => x == '|') > 0)
						{
							p.brand = dr["brand"].ToString().Split('|')[0];
						}
						else
						{
							p.brand = dr["brand"].ToString();
						}
						p.pending_in = dr["pending_in"].ToString();
						p.pending_out = dr["pending_out"].ToString();
						p.pending = dr["pending"].ToString();

						pendings.Add(p);
					}
					dr.Close();
				}
				return pendings;
			}
			finally
			{
				if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
				{
					ConnectSQL.CloseConnect();
				}
			}
		}
        public List<Quotation_Report_QuarterModel> GetReportQuarter(string department, string year ,string stages)
        {
            try
            {
                List<Quotation_Report_QuarterModel> reports = new List<Quotation_Report_QuarterModel>();
                string command = "";
				string stage = "";
				if (stages == "Pending")
                {
					stage = " stages in ('', 'Quote for Budget', 'Negotiation/Review', 'Proposal/Quote for Order', 'Prospecting')";

				}
				else if(stages == "Win")
                {
					stage = " stages = 'Closed(Won)'";
				}
                else if(stages == "Lose")
                {
					stage = " stages ='Closed(Lost)'";
				}
                else if (stages == "No go")
                {
					stage = " stages ='No go'";
                }
                else
                {
					stages = "";
                }

                if (department == "ALL")
                {
                    command = string.Format($@" DECLARE @million as float
											SET @million = 1000000;
											with s1 as(select department,sale_name as sale,
                            sum(case when expected_order_date like '{year}-01%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jan_in,
                            sum(case when expected_order_date like '{year}-01%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jan_out,
                            sum(case when expected_order_date like '{year}-02%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as feb_in,
                            sum(case when expected_order_date like '{year}-02%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as feb_out,
                            sum(case when expected_order_date like '{year}-03%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as mar_in,
                            sum(case when expected_order_date like '{year}-03%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as mar_out,
                            sum(case when expected_order_date like '{year}-04%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as apr_in,
                            sum(case when expected_order_date like '{year}-04%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as apr_out,
                            sum(case when expected_order_date like '{year}-05%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as may_in,
                            sum(case when expected_order_date like '{year}-05%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as may_out,
                            sum(case when expected_order_date like '{year}-06%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jun_in,
                            sum(case when expected_order_date like '{year}-06%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jun_out,
                            sum(case when expected_order_date like '{year}-07%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jul_in,
                            sum(case when expected_order_date like '{year}-07%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jul_out,
                            sum(case when expected_order_date like '{year}-08%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as aug_in,
                            sum(case when expected_order_date like '{year}-08%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as aug_out,
                            sum(case when expected_order_date like '{year}-09%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as sep_in,
                            sum(case when expected_order_date like '{year}-09%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as sep_out,
                            sum(case when expected_order_date like '{year}-10%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as oct_in,
                            sum(case when expected_order_date like '{year}-10%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as oct_out,
                            sum(case when expected_order_date like '{year}-11%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as nov_in,
                            sum(case when expected_order_date like '{year}-11%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as nov_out,
                            sum(case when expected_order_date like '{year}-12%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as dec_in,
                            sum(case when expected_order_date like '{year}-12%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as dec_out
							from Quotation where {stage}
							group by department,sale_name union all

                            select(department + ' Total') as department,'' as sale,
                            sum(case when expected_order_date like '{year}-01%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jan_in,
                            sum(case when expected_order_date like '{year}-01%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jan_out,
                            sum(case when expected_order_date like '{year}-02%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as feb_in,
                            sum(case when expected_order_date like '{year}-02%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as feb_out,
                            sum(case when expected_order_date like '{year}-03%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as mar_in,
                            sum(case when expected_order_date like '{year}-03%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as mar_out,
                            sum(case when expected_order_date like '{year}-04%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as apr_in,
                            sum(case when expected_order_date like '{year}-04%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as apr_out,
                            sum(case when expected_order_date like '{year}-05%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as may_in,
                            sum(case when expected_order_date like '{year}-05%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as may_out,
                            sum(case when expected_order_date like '{year}-06%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jun_in,
                            sum(case when expected_order_date like '{year}-06%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jun_out,
                            sum(case when expected_order_date like '{year}-07%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jul_in,
                            sum(case when expected_order_date like '{year}-07%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jul_out,
                            sum(case when expected_order_date like '{year}-08%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as aug_in,
                            sum(case when expected_order_date like '{year}-08%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as aug_out,
                            sum(case when expected_order_date like '{year}-09%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as sep_in,
                            sum(case when expected_order_date like '{year}-09%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as sep_out,
                            sum(case when expected_order_date like '{year}-10%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as oct_in,
                            sum(case when expected_order_date like '{year}-10%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as oct_out,
                            sum(case when expected_order_date like '{year}-11%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as nov_in,
                            sum(case when expected_order_date like '{year}-11%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as nov_out,
                            sum(case when expected_order_date like '{year}-12%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as dec_in,
                            sum(case when expected_order_date like '{year}-12%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as dec_out
							from Quotation where {stage}
							group by department union all

                            select 'Total' as department,'' as sale,
                            sum(case when expected_order_date like '{year}-01%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jan_in,
                            sum(case when expected_order_date like '{year}-01%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jan_out,
                            sum(case when expected_order_date like '{year}-02%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as feb_in,
                            sum(case when expected_order_date like '{year}-02%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as feb_out,
                            sum(case when expected_order_date like '{year}-03%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as mar_in,
                            sum(case when expected_order_date like '{year}-03%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as mar_out,
                            sum(case when expected_order_date like '{year}-04%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as apr_in,
                            sum(case when expected_order_date like '{year}-04%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as apr_out,
                            sum(case when expected_order_date like '{year}-05%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as may_in,
                            sum(case when expected_order_date like '{year}-05%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as may_out,
                            sum(case when expected_order_date like '{year}-06%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jun_in,
                            sum(case when expected_order_date like '{year}-06%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jun_out,
                            sum(case when expected_order_date like '{year}-07%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jul_in,
                            sum(case when expected_order_date like '{year}-07%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jul_out,
                            sum(case when expected_order_date like '{year}-08%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as aug_in,
                            sum(case when expected_order_date like '{year}-08%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as aug_out,
                            sum(case when expected_order_date like '{year}-09%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as sep_in,
                            sum(case when expected_order_date like '{year}-09%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as sep_out,
                            sum(case when expected_order_date like '{year}-10%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as oct_in,
                            sum(case when expected_order_date like '{year}-10%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as oct_out,
                            sum(case when expected_order_date like '{year}-11%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as nov_in,
                            sum(case when expected_order_date like '{year}-11%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as nov_out,
                            sum(case when expected_order_date like '{year}-12%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as dec_in,
                            sum(case when expected_order_date like '{year}-12%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as dec_out
							from Quotation where {stage} )
                            select* from s1 order by s1.department,s1.sale ");
                }
                else
                {
					command = string.Format($@" DECLARE @million as float
											SET @million = 1000000;
							with s1 as(select department,sale_name as sale,
							sum(case when expected_order_date like '{year}-01%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jan_in,
							sum(case when expected_order_date like '{year}-01%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jan_out,
							sum(case when expected_order_date like '{year}-02%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as feb_in,
							sum(case when expected_order_date like '{year}-02%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as feb_out,
							sum(case when expected_order_date like '{year}-03%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as mar_in,
							sum(case when expected_order_date like '{year}-03%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as mar_out,
							sum(case when expected_order_date like '{year}-04%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as apr_in,
							sum(case when expected_order_date like '{year}-04%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as apr_out,
							sum(case when expected_order_date like '{year}-05%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as may_in,
							sum(case when expected_order_date like '{year}-05%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as may_out,
							sum(case when expected_order_date like '{year}-06%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jun_in,
							sum(case when expected_order_date like '{year}-06%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jun_out,
							sum(case when expected_order_date like '{year}-07%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jul_in,
							sum(case when expected_order_date like '{year}-07%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jul_out,
							sum(case when expected_order_date like '{year}-08%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as aug_in,
							sum(case when expected_order_date like '{year}-08%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as aug_out,
							sum(case when expected_order_date like '{year}-09%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as sep_in,
							sum(case when expected_order_date like '{year}-09%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as sep_out,
							sum(case when expected_order_date like '{year}-10%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as oct_in,
							sum(case when expected_order_date like '{year}-10%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as oct_out,
							sum(case when expected_order_date like '{year}-11%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as nov_in,
							sum(case when expected_order_date like '{year}-11%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as nov_out,
							sum(case when expected_order_date like '{year}-12%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as dec_in,
							sum(case when expected_order_date like '{year}-12%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as dec_out
							from Quotation where department='{department}' and {stage}
							group by department,sale_name union all

							select(department + ' Total') as department,'' as sale,
							sum(case when expected_order_date like '{year}-01%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jan_in,
							sum(case when expected_order_date like '{year}-01%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jan_out,
							sum(case when expected_order_date like '{year}-02%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as feb_in,
							sum(case when expected_order_date like '{year}-02%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as feb_out,
							sum(case when expected_order_date like '{year}-03%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as mar_in,
							sum(case when expected_order_date like '{year}-03%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as mar_out,
							sum(case when expected_order_date like '{year}-04%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as apr_in,
							sum(case when expected_order_date like '{year}-04%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as apr_out,
							sum(case when expected_order_date like '{year}-05%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as may_in,
							sum(case when expected_order_date like '{year}-05%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as may_out,
							sum(case when expected_order_date like '{year}-06%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jun_in,
							sum(case when expected_order_date like '{year}-06%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jun_out,
							sum(case when expected_order_date like '{year}-07%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jul_in,
							sum(case when expected_order_date like '{year}-07%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as jul_out,
							sum(case when expected_order_date like '{year}-08%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as aug_in,
							sum(case when expected_order_date like '{year}-08%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as aug_out,
							sum(case when expected_order_date like '{year}-09%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as sep_in,
							sum(case when expected_order_date like '{year}-09%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as sep_out,
							sum(case when expected_order_date like '{year}-10%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as oct_in,
							sum(case when expected_order_date like '{year}-10%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as oct_out,
							sum(case when expected_order_date like '{year}-11%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as nov_in,
							sum(case when expected_order_date like '{year}-11%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as nov_out,
							sum(case when expected_order_date like '{year}-12%' then case when status='IN' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as dec_in,
							sum(case when expected_order_date like '{year}-12%' then case when status='OUT' then cast(cast(replace(quoted_price,',','') as float) / @million as decimal(10,2)) else 0 end end) as dec_out
							from Quotation where department='{department}' and {stage}
							group by department)
							select* from s1 order by s1.department,s1.sale ");
                }
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
					while (dr.Read())
					{
						Quotation_Report_QuarterModel r = new Quotation_Report_QuarterModel();

						r.department = dr["department"].ToString();
						r.sale = dr["sale"].ToString();
						r.jan_in = dr["jan_in"].ToString();
						r.jan_out = dr["jan_out"].ToString();
						r.feb_in = dr["feb_in"].ToString();
						r.feb_out = dr["feb_out"].ToString();
						r.mar_in = dr["mar_in"].ToString();
						r.mar_out = dr["mar_out"].ToString();
						var sum_in_q1 = string.Format("{0:0.00}",
							((dr["jan_in"].ToString() != "" ? float.Parse(dr["jan_in"].ToString()) : 0) +
							 (dr["feb_in"].ToString() != "" ? float.Parse(dr["feb_in"].ToString()) : 0) +
							 (dr["mar_in"].ToString() != "" ? float.Parse(dr["mar_in"].ToString()) : 0)));
						r.sum_in_q1 = sum_in_q1;
						var sum_out_q1 = string.Format("{0:0.00}",
						((dr["jan_out"].ToString() != "" ? float.Parse(dr["jan_out"].ToString()) : 0) +
						 (dr["feb_out"].ToString() != "" ? float.Parse(dr["feb_out"].ToString()) : 0) +
						 (dr["mar_out"].ToString() != "" ? float.Parse(dr["mar_out"].ToString()) : 0)));
						r.sum_out_q1 = sum_out_q1;
						r.apr_in = dr["apr_in"].ToString();
						r.apr_out = dr["apr_out"].ToString();
						r.may_in = dr["may_in"].ToString();
						r.may_out = dr["may_out"].ToString();
						r.jun_in = dr["jun_in"].ToString();
						r.jun_out = dr["jun_out"].ToString();
						var sum_in_q2 = string.Format("{0:0.00}",
						((dr["apr_in"].ToString() != "" ? float.Parse(dr["apr_in"].ToString()) : 0) +
						 (dr["may_in"].ToString() != "" ? float.Parse(dr["may_in"].ToString()) : 0) +
						 (dr["jun_in"].ToString() != "" ? float.Parse(dr["jun_in"].ToString()) : 0)));
						r.sum_in_q2 = sum_in_q2;
						var sum_out_q2 = string.Format("{0:0.00}",
						 ((dr["apr_out"].ToString() != "" ? float.Parse(dr["apr_out"].ToString()) : 0) +
						 (dr["may_out"].ToString() != "" ? float.Parse(dr["may_out"].ToString()) : 0) +
						 (dr["jun_out"].ToString() != "" ? float.Parse(dr["jun_out"].ToString()) : 0)));
						r.sum_out_q2 = sum_out_q2;
						r.jul_in = dr["jul_in"].ToString();
						r.jul_out = dr["jul_out"].ToString();
						r.aug_in = dr["aug_in"].ToString();
						r.aug_out = dr["aug_out"].ToString();
						r.sep_in = dr["sep_in"].ToString();
						r.sep_out = dr["sep_out"].ToString();
						var sum_in_q3 = string.Format("{0:0.00}",
						((dr["jul_in"].ToString() != "" ? float.Parse(dr["jul_in"].ToString()) : 0) +
						 (dr["aug_in"].ToString() != "" ? float.Parse(dr["aug_in"].ToString()) : 0) +
						 (dr["sep_in"].ToString() != "" ? float.Parse(dr["sep_in"].ToString()) : 0)));
						r.sum_in_q3 = sum_in_q3;
						var sum_out_q3 = string.Format("{0:0.00}",
						((dr["jul_out"].ToString() != "" ? float.Parse(dr["jul_out"].ToString()) : 0) +
						 (dr["aug_out"].ToString() != "" ? float.Parse(dr["aug_out"].ToString()) : 0) +
						 (dr["sep_out"].ToString() != "" ? float.Parse(dr["sep_out"].ToString()) : 0)));
						r.sum_out_q3 = sum_out_q3;
						r.oct_in = dr["oct_in"].ToString();
						r.oct_out = dr["oct_out"].ToString();
						r.nov_in = dr["nov_in"].ToString();
						r.nov_out = dr["nov_out"].ToString();
						r.dec_in = dr["dec_in"].ToString();
						r.dec_out = dr["dec_out"].ToString();
						var sum_in_q4 = string.Format("{0:0.00}",
						((dr["oct_in"].ToString() != "" ? float.Parse(dr["oct_in"].ToString()) : 0) +
						 (dr["nov_in"].ToString() != "" ? float.Parse(dr["nov_in"].ToString()) : 0) +
						 (dr["dec_in"].ToString() != "" ? float.Parse(dr["dec_in"].ToString()) : 0)));
						r.sum_in_q4 = sum_in_q4;
						var sum_out_q4 = string.Format("{0:0.00}",
						((dr["oct_out"].ToString() != "" ? float.Parse(dr["oct_out"].ToString()) : 0) +
						 (dr["nov_out"].ToString() != "" ? float.Parse(dr["nov_out"].ToString()) : 0) +
						 (dr["dec_out"].ToString() != "" ? float.Parse(dr["dec_out"].ToString()) : 0)));
						r.sum_out_q4 = sum_out_q4;
						r.sum_in = string.Format("{0:0.00}", (float.Parse(sum_in_q1) + float.Parse(sum_in_q2) + float.Parse(sum_in_q3) + float.Parse(sum_in_q4)));
						r.sum_out = string.Format("{0:0.00}", (float.Parse(sum_out_q1) + float.Parse(sum_out_q2) + float.Parse(sum_out_q3) + float.Parse(sum_out_q4)));

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

                string command = string.Format($@"	DECLARE @department as VARCHAR(10)
													DECLARE @million as float
													DECLARE @year as VARCHAR(10)
													SET @department = '{department}';
													SET @million = 1000000;	
													SET @year = '{year}';
								
													with main As(
													select sub_main.month,
														sum(sub_main.quo_cnt) as quo_cnt,
														sum(sub_main.quo_mb) as quo_mb,
														sum(sub_main.product_won_cnt ) / count(sub_main.product_won_cnt ) as product_won_cnt,
														cast(sum(sub_main.product_won_mb) / count(sub_main.product_won_mb)as decimal(10,2)) as product_won_mb,
														sum(sub_main.product_lost_cnt ) / count(sub_main.product_lost_cnt ) as product_lost_cnt,
														cast(sum(sub_main.product_lost_mb) / count(sub_main.product_lost_mb) as decimal(10,2)) as product_lost_mb,
														sum(sub_main.product_nogo_cnt ) / count(sub_main.product_nogo_cnt ) as product_nogo_cnt,
														cast(sum(sub_main.product_nogo_mb) / count(sub_main.product_nogo_mb) as decimal(10,2)) as product_nogo_mb,
														sum(sub_main.product_pending_cnt ) / count(sub_main.product_pending_cnt ) as product_pending_cnt,
														cast(sum(sub_main.product_pending_mb) / count(sub_main.product_pending_mb) as decimal(10,2)) as product_pending_mb,
														sum(sub_main.product_cnt) as product_cnt,
														cast(sum(sub_main.product_mb) as decimal(10,2)) as product_mb,

														sum(sub_main.project_won_cnt ) / count(sub_main.project_won_cnt ) as project_won_cnt,
														cast(sum(sub_main.project_won_mb) / count(sub_main.project_won_mb) as decimal(10,2)) as project_won_mb,
														sum(sub_main.project_lost_cnt ) / count(sub_main.project_lost_cnt ) as project_lost_cnt,
														cast(sum(sub_main.project_lost_mb) / count(sub_main.project_lost_mb) as decimal(10,2)) as project_lost_mb,
														sum(sub_main.project_nogo_cnt ) / count(sub_main.project_nogo_cnt ) as project_nogo_cnt,
														cast(sum(sub_main.project_nogo_mb) / count(sub_main.project_nogo_mb) as decimal(10,2)) as project_nogo_mb,
														sum(sub_main.project_pending_cnt ) / count(sub_main.project_pending_cnt ) as project_pending_cnt,
														cast(sum(sub_main.project_pending_mb) / count(sub_main.project_pending_mb) as decimal(10,2)) as project_pending_mb,
														sum(sub_main.project_cnt) as project_cnt,
														cast(sum(sub_main.project_mb) as decimal(10,2)) as project_mb,

														sum(sub_main.service_won_cnt ) / count(sub_main.service_won_cnt ) as service_won_cnt,
														cast(sum(sub_main.service_won_mb) / count(sub_main.service_won_mb) as decimal(10,2)) as service_won_mb,
														sum(sub_main.service_lost_cnt ) / count(sub_main.service_lost_cnt )as service_lost_cnt,
														cast(sum(sub_main.service_lost_mb) / count(sub_main.service_lost_mb) as decimal(10,2)) as service_lost_mb,
														sum(sub_main.service_nogo_cnt ) / count(sub_main.service_nogo_cnt ) as service_nogo_cnt,
														cast(sum(sub_main.service_nogo_mb) / count(sub_main.service_nogo_mb) as decimal(10,2)) as service_nogo_mb,
														sum(sub_main.service_pending_cnt ) / count(sub_main.service_pending_cnt ) as service_pending_cnt,
														cast(sum(sub_main.service_pending_mb) / count(sub_main.service_pending_mb) as decimal(10,2)) as service_pending_mb,
														sum(sub_main.service_cnt) as service_cnt,
														cast(sum(sub_main.service_mb) as decimal(10,2)) as service_mb,
                                                    
														sum(sub_main.won_product_cnt) / count(sub_main.won_product_cnt) as won_product_cnt,
														cast(sum(sub_main.won_product_mb) / count(sub_main.won_product_mb) as decimal(10,2)) as won_product_mb,
														sum(sub_main.won_project_cnt) / count(sub_main.won_project_cnt) as won_project_cnt,
														cast(sum(sub_main.won_project_mb) / count(sub_main.won_project_mb) as decimal(10,2)) as won_project_mb,
														sum(sub_main.won_service_cnt) / count(sub_main.won_service_cnt) as won_service_cnt,
														cast(sum(sub_main.won_service_mb) / count(sub_main.won_service_mb) as decimal(10,2)) as won_service_mb,
														sum(sub_main.won_quo_cnt) as won_quo_cnt,
														cast(sum(sub_main.won_mb) as decimal(10,2)) as won_mb,
                                                    
														sum(sub_main.lost_product_cnt) / count(sub_main.lost_product_cnt) as lost_product_cnt,
														cast(sum(sub_main.lost_product_mb) / count(sub_main.lost_product_mb) as decimal(10,2)) as lost_product_mb,
														sum(sub_main.lost_project_cnt) / count(sub_main.lost_project_cnt) as lost_project_cnt,
														cast(sum(sub_main.lost_project_mb) / count(sub_main.lost_project_mb) as decimal(10,2)) as lost_project_mb,
														sum(sub_main.lost_service_cnt) / count(sub_main.lost_service_cnt) as lost_service_cnt,
														cast(sum(sub_main.lost_service_mb) / count(sub_main.lost_service_mb) as decimal(10,2)) as lost_service_mb,
														sum(sub_main.loss_quo_cnt) as loss_quo_cnt,
														cast(sum(sub_main.loss_mb) as decimal(10,2)) as loss_mb,
                                                    
														sum(sub_main.nogo_product_cnt) / count(sub_main.nogo_product_cnt)as nogo_product_cnt,
														cast(sum(sub_main.nogo_product_mb) / count(sub_main.nogo_product_mb)as decimal(10,2)) as nogo_product_mb,
														sum(sub_main.nogo_project_cnt) / count(sub_main.nogo_project_cnt)as nogo_project_cnt,
														cast(sum(sub_main.nogo_project_mb) / count(sub_main.nogo_project_mb) as decimal(10,2)) as nogo_project_mb,
														sum(sub_main.nogo_service_cnt) / count(sub_main.nogo_service_cnt) as nogo_service_cnt,
														cast(sum(sub_main.nogo_service_mb) / count(sub_main.nogo_service_mb) as decimal(10,2)) as nogo_service_mb,
														sum(sub_main.nogo_quo_cnt) as nogo_quo_cnt,
														cast(sum(sub_main.nogo_mb) as decimal(10,2)) as nogo_mb,
                                                    
														sum(sub_main.pending_product_cnt) / count(sub_main.pending_product_cnt) as pending_product_cnt,
														cast(sum(sub_main.pending_product_mb) / count(sub_main.pending_product_mb) as decimal(10,2)) as pending_product_mb,
														sum(sub_main.pending_project_cnt) / count(sub_main.pending_project_cnt) as pending_project_cnt,
														cast(sum(sub_main.pending_project_mb) / count(sub_main.pending_project_mb) as decimal(10,2)) as pending_project_mb,
														sum(sub_main.pending_service_cnt) / count(sub_main.pending_service_cnt) as pending_service_cnt,
														cast(sum(sub_main.pending_service_mb) / count(sub_main.pending_service_mb) as decimal(10,2)) as pending_service_mb,
														sum(sub_main.pending_quo_cnt) as pending_quo_cnt,
														cast(sum(sub_main.pending_mb) as decimal(10,2)) as pending_mb
													 from (
														select
														CAST(YEAR(date) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(date) AS VARCHAR(2)), 2) as month,
														count(quotation_no) as quo_cnt,													
														cast(sum(cast(replace(quoted_price,',','') as float))/ @million as decimal(10, 2)) as quo_mb,                                                
														sum(sub_sub_type.product_won_cnt) / count(sub_sub_type.product_won_cnt) as product_won_cnt ,
														cast(sum(sub_sub_type.product_won_mb) / count(sub_sub_type.product_won_mb) as decimal(10, 2)) as product_won_mb,
														sum(sub_sub_type.product_lost_cnt) / count(sub_sub_type.product_lost_cnt) as product_lost_cnt ,
														cast(sum(sub_sub_type.product_lost_mb) / count(sub_sub_type.product_lost_mb) as decimal(10, 2)) as product_lost_mb,
														sum(sub_sub_type.product_nogo_cnt) / count(sub_sub_type.product_nogo_cnt) as product_nogo_cnt ,
														cast(sum(sub_sub_type.product_nogo_mb) / count(sub_sub_type.product_nogo_mb) as decimal(10, 2)) as product_nogo_mb,
														sum(sub_sub_type.product_pending_cnt) / count(sub_sub_type.product_pending_cnt) as product_pending_cnt ,
														cast(sum(sub_sub_type.product_pending_mb) / count(sub_sub_type.product_pending_mb) as decimal(10, 2)) as product_pending_mb,
														sum(case when Quotation.product_type = 'product' and (stages is not null and stages <> '') then 1 else 0 end) as product_cnt,
														sum(case when Quotation.product_type = 'product' and (stages is not null and stages <> '') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as product_mb,

														sum(sub_sub_type.project_won_cnt) / count(sub_sub_type.project_won_cnt) as project_won_cnt ,
														cast(sum(sub_sub_type.project_won_mb) / count(sub_sub_type.project_won_mb) as decimal(10, 2)) as project_won_mb,
														sum(sub_sub_type.project_lost_cnt) / count(sub_sub_type.project_lost_cnt) as project_lost_cnt ,
														cast(sum(sub_sub_type.project_lost_mb) / count(sub_sub_type.project_lost_mb) as decimal(10, 2)) as project_lost_mb,
														sum(sub_sub_type.project_nogo_cnt) / count(sub_sub_type.project_nogo_cnt) as project_nogo_cnt ,
														cast(sum(sub_sub_type.project_nogo_mb) / count(sub_sub_type.project_nogo_mb) as decimal(10, 2)) as project_nogo_mb,
														sum(sub_sub_type.project_pending_cnt) / count(sub_sub_type.project_pending_cnt) as project_pending_cnt ,
														cast(sum(sub_sub_type.project_pending_mb) / count(sub_sub_type.project_pending_mb) as decimal(10, 2)) as project_pending_mb,
														sum(case when Quotation.product_type = 'project' and (stages is not null and stages <> '') then 1 else 0 end) as project_cnt,
														sum(case when Quotation.product_type = 'project' and (stages is not null and stages <> '') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as project_mb,

														sum(sub_sub_type.service_won_cnt) / count(sub_sub_type.service_won_cnt) as service_won_cnt ,
														cast(sum(sub_sub_type.service_won_mb) / count(sub_sub_type.service_won_mb) as decimal(10, 2)) as service_won_mb,
														sum(sub_sub_type.service_lost_cnt) / count(sub_sub_type.service_lost_cnt) as service_lost_cnt ,
														cast(sum(sub_sub_type.service_lost_mb) / count(sub_sub_type.service_lost_mb) as decimal(10, 2))as service_lost_mb,
														sum(sub_sub_type.service_nogo_cnt) / count(sub_sub_type.service_nogo_cnt) as service_nogo_cnt ,
														cast(sum(sub_sub_type.service_nogo_mb) / count(sub_sub_type.service_nogo_mb) as decimal(10, 2))as service_nogo_mb,
														sum(sub_sub_type.service_pending_cnt) / count(sub_sub_type.service_pending_cnt) as service_pending_cnt ,
														cast(sum(sub_sub_type.service_pending_mb) / count(sub_sub_type.service_pending_mb)as decimal(10, 2)) as service_pending_mb,
														sum(case when Quotation.product_type = 'service' and (stages is not null and stages <> '') then 1 else 0 end) as service_cnt,
														sum(case when Quotation.product_type = 'service' and (stages is not null and stages <> '') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as service_mb,
                                                    
														sum(sub_sub_stages.won_product_cnt) / count(sub_sub_stages.won_product_cnt) as won_product_cnt,
														cast(sum(sub_sub_stages.won_product_mb) / count(sub_sub_stages.won_product_mb) as decimal(10, 2)) as won_product_mb,
														sum(sub_sub_stages.won_project_cnt) / count(sub_sub_stages.won_project_cnt) as won_project_cnt,
														cast(sum(sub_sub_stages.won_project_mb) / count(sub_sub_stages.won_project_mb) as decimal(10, 2)) as won_project_mb,
														sum(sub_sub_stages.won_service_cnt) / count(sub_sub_stages.won_service_cnt) as won_service_cnt,
														cast(sum(sub_sub_stages.won_service_mb) / count(sub_sub_stages.won_service_mb) as decimal(10, 2)) as won_service_mb,
														sum(case when stages = 'Closed(Won)' then 1 else 0 end) as won_quo_cnt,
														sum(case when stages = 'Closed(Won)' then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as won_mb,
                                                    
														sum(sub_sub_stages.lost_product_cnt) / count(sub_sub_stages.lost_product_cnt) as lost_product_cnt,
														cast(sum(sub_sub_stages.lost_product_mb) / count(sub_sub_stages.lost_product_mb) as decimal(10, 2)) as lost_product_mb,
														sum(sub_sub_stages.lost_project_cnt) / count(sub_sub_stages.lost_project_cnt) as lost_project_cnt,
														cast(sum(sub_sub_stages.lost_project_mb) / count(sub_sub_stages.lost_project_mb) as decimal(10, 2)) as lost_project_mb,
														sum(sub_sub_stages.lost_service_cnt) / count(sub_sub_stages.lost_service_cnt) as lost_service_cnt,
														cast(sum(sub_sub_stages.lost_service_mb) / count(sub_sub_stages.lost_service_mb) as decimal(10, 2)) as lost_service_mb,
														sum(case when stages = 'Closed(Lost)' then 1 else 0 end) as loss_quo_cnt,
														sum(case when stages = 'Closed(Lost)' then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as loss_mb,
                                                    
														sum(sub_sub_stages.nogo_product_cnt) / count(sub_sub_stages.nogo_product_cnt) as nogo_product_cnt,
														cast(sum(sub_sub_stages.nogo_product_mb) / count(sub_sub_stages.nogo_product_mb) as decimal(10, 2)) as nogo_product_mb,
														sum(sub_sub_stages.nogo_project_cnt) / count(sub_sub_stages.nogo_project_cnt) as nogo_project_cnt,
														cast(sum(sub_sub_stages.nogo_project_mb) / count(sub_sub_stages.nogo_project_mb) as decimal(10, 2)) as nogo_project_mb,
														sum(sub_sub_stages.nogo_service_cnt) / count(sub_sub_stages.nogo_service_cnt) as nogo_service_cnt,
														cast(sum(sub_sub_stages.nogo_service_mb) / count(sub_sub_stages.nogo_service_mb) as decimal(10, 2)) as nogo_service_mb,
														sum(case when stages = 'No go' then 1 else 0 end) as nogo_quo_cnt,
														sum(case when stages = 'No go' then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as nogo_mb,
                                                    
														sum(sub_sub_pending.pending_product_cnt) / count(sub_sub_pending.pending_product_cnt) as pending_product_cnt,
														cast(sum(sub_sub_pending.pending_product_mb) / count(sub_sub_pending.pending_product_mb) as decimal(10, 2)) as pending_product_mb,
														sum(sub_sub_pending.pending_project_cnt) / count(sub_sub_pending.pending_project_cnt) as pending_project_cnt,
														cast(sum(sub_sub_pending.pending_project_mb) / count(sub_sub_pending.pending_project_mb) as decimal(10, 2)) as pending_project_mb,
														sum(sub_sub_pending.pending_service_cnt) / count(sub_sub_pending.pending_service_cnt) as pending_service_cnt,
														cast(sum(sub_sub_pending.pending_service_mb) / count(sub_sub_pending.pending_service_mb) as decimal(10, 2)) as pending_service_mb,
														sum(case when stages is not null and stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then 1 else 0 end) as pending_quo_cnt,
														sum(case when stages is not null and stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then cast(cast(replace(quoted_price, ',', '') as float) / @million as decimal(10, 2)) else 0 end) as pending_mb
                                                    from Quotation
                                                    left join (
														select
															sub_type.month,
															sum(case when sub_type.product_type = 'Product' then type_won_cnt else 0 end) as product_won_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_won_mb as float) else 0 end) as product_won_mb,
															sum(case when sub_type.product_type = 'Product' then type_lost_cnt else 0 end) as product_lost_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_lost_mb as float) else 0 end) as product_lost_mb,
															sum(case when sub_type.product_type = 'Product' then type_nogo_cnt else 0 end) as product_nogo_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_nogo_mb as float) else 0 end) as product_nogo_mb,
															sum(case when sub_type.product_type = 'Product' then type_pending_cnt else 0 end) as product_pending_cnt,
															sum(case when sub_type.product_type = 'Product' then cast(type_pending_mb as float) else 0 end) as product_pending_mb,

															sum(case when sub_type.product_type = 'Project' then type_won_cnt else 0 end) as project_won_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_won_mb as float) else 0 end) as project_won_mb,
															sum(case when sub_type.product_type = 'Project' then type_lost_cnt else 0 end) as project_lost_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_lost_mb as float) else 0 end) as project_lost_mb,
															sum(case when sub_type.product_type = 'Project' then type_nogo_cnt else 0 end) as project_nogo_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_nogo_mb as float) else 0 end) as project_nogo_mb,
															sum(case when sub_type.product_type = 'Project' then type_pending_cnt else 0 end) as project_pending_cnt,
															sum(case when sub_type.product_type = 'Project' then cast(type_pending_mb as float) else 0 end) as project_pending_mb,

															sum(case when sub_type.product_type = 'Service' then type_won_cnt else 0 end) as service_won_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_won_mb as float) else 0 end) as service_won_mb,
															sum(case when sub_type.product_type = 'Service' then type_lost_cnt else 0 end) as service_lost_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_lost_mb as float) else 0 end) as service_lost_mb,
															sum(case when sub_type.product_type = 'Service' then type_nogo_cnt else 0 end) as service_nogo_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_nogo_mb as float) else 0 end) as service_nogo_mb,
															sum(case when sub_type.product_type = 'Service' then type_pending_cnt else 0 end) as service_pending_cnt,
															sum(case when sub_type.product_type = 'Service' then cast(type_pending_mb as float) else 0 end) as service_pending_mb

                                                        from(
                                                            select
																CAST(YEAR(date) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(date) AS VARCHAR(2)), 2) as month,
                                                                product_type,
                                                                sum(case when stages = 'Closed(Won)' then 1 else 0 end) as type_won_cnt,
																cast(sum(case when stages = 'Closed(Won)' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end) as decimal(10, 2)) as type_won_mb,
																sum(case when stages = 'Closed(Lost)' then 1 else 0 end) as type_lost_cnt,
																cast(sum(case when stages = 'Closed(Lost)' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end) as decimal(10, 2)) as type_lost_mb,
																sum(case when stages = 'No go' then 1 else 0 end) as type_nogo_cnt,
																cast(sum(case when stages = 'No go' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end) as decimal(10, 2)) as type_nogo_mb,
																sum(case when stages is not null and stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then 1 else 0 end) as type_pending_cnt,
																cast(sum(case when stages is not null and stages not in ('', 'Closed(Won)', 'Closed(Lost)', 'No go') then cast(replace(quoted_price, ',', '') as float) / @million else 0 end) as decimal(10, 2)) as type_pending_mb

                                                            from Quotation
                                                            where department = @department and CAST(YEAR(date) AS VARCHAR(4)) = @year
															group by date,product_type having date like '{year}%' ) as sub_type

                                                        group by sub_type.month
                                                        
													) as sub_sub_type

                                                     ON CAST(YEAR(date) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(date) AS VARCHAR(2)), 2) = sub_sub_type.month

                                                    left join (
														select
															sub_stages.month,
                                                            sum(case when stages = 'Closed(Won)' then sub_stages.stages_project_cnt else 0 end) as won_project_cnt,
															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_project_mb as float) else 0 end) as won_project_mb,
															sum(case when stages = 'Closed(Won)' then sub_stages.stages_product_cnt else 0 end) as won_product_cnt,
															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_product_mb as float) else 0 end) as won_product_mb,
															sum(case when stages = 'Closed(Won)' then sub_stages.stages_service_cnt else 0 end) as won_service_cnt,
															sum(case when stages = 'Closed(Won)' then cast(sub_stages.stages_service_mb as float) else 0 end) as won_service_mb,

															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_project_cnt else 0 end) as lost_project_cnt,
															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_project_mb as float) else 0 end) as lost_project_mb,
															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_product_cnt else 0 end) as lost_product_cnt,
															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_product_mb as float) else 0 end) as lost_product_mb,
															sum(case when stages = 'Closed(Lost)' then sub_stages.stages_service_cnt else 0 end) as lost_service_cnt,
															sum(case when stages = 'Closed(Lost)' then cast(sub_stages.stages_service_mb as float) else 0 end) as lost_service_mb,

															sum(case when stages = 'No go' then sub_stages.stages_project_cnt else 0 end) as nogo_project_cnt,
															sum(case when stages = 'No go' then cast(sub_stages.stages_project_mb as float) else 0 end) as nogo_project_mb,
															sum(case when stages = 'No go' then sub_stages.stages_product_cnt else 0 end) as nogo_product_cnt,
															sum(case when stages = 'No go' then cast(sub_stages.stages_product_mb as float) else 0 end) as nogo_product_mb,
															sum(case when stages = 'No go' then sub_stages.stages_service_cnt else 0 end) as nogo_service_cnt,
															sum(case when stages = 'No go' then cast(sub_stages.stages_service_mb as float) else 0 end) as nogo_service_mb

                                                        from(
                                                            select
																CAST(YEAR(date) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(date) AS VARCHAR(2)), 2) as month,
                                                                stages,
                                                                sum(case when product_type = 'Project' then 1 else 0 end) as stages_project_cnt,
																cast(sum(case when product_type = 'Project' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end) as decimal(10, 2)) as stages_project_mb,
																sum(case when product_type = 'Product' then 1 else 0 end) as stages_product_cnt,
																cast(sum(case when product_type = 'Product' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end) as decimal(10, 2)) as stages_product_mb,
																sum(case when product_type = 'Service' then 1 else 0 end) as stages_service_cnt,
																cast(sum(case when product_type = 'Service' then cast(replace(quoted_price, ',', '') as float) / @million else 0 end) as decimal(10, 2)) as stages_service_mb
                                                             from Quotation
                                                             where department = @department and CAST(YEAR(date) AS VARCHAR(4)) = @year and stages in('Closed(Won)','Closed(Lost)','No go')
                                                             group by date,stages having date like '{year}%') as sub_stages

                                                        group by sub_stages.month
													) as sub_sub_stages

                                                    ON CAST(YEAR(date) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(date) AS VARCHAR(2)), 2) = sub_sub_stages.month

                                                    left join (
														select
															sub_pending.month,
                                                            sum(case when product_type = 'Product' then stages_pending_cnt else 0 end ) as pending_product_cnt,
															sum(case when product_type = 'Product' then cast(stages_pending_mb as float) else 0 end ) as pending_product_mb,
															sum(case when product_type = 'Project' then stages_pending_cnt else 0 end ) as pending_project_cnt,
															sum(case when product_type = 'Project' then cast(stages_pending_mb as float) else 0 end ) as pending_project_mb,
															sum(case when product_type = 'Service' then stages_pending_cnt else 0 end ) as pending_service_cnt,
															sum(case when product_type = 'Service' then cast(stages_pending_mb as float) else 0 end ) as pending_service_mb

                                                            from(
																select 
																sub_stages.month,                                                         
																'pending' as stages,
																sub_stages.product_type,
																sum(cast(sub_stages.stages_pending_cnt as float)) as stages_pending_cnt,
																cast(sum(cast(sub_stages.stages_pending_mb as float)) as decimal(10, 2)) as stages_pending_mb

																from(
																	select
																		CAST(YEAR(date) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(date) AS VARCHAR(2)), 2) as month,
																		'Pending' as stages,
																		product_type,
																		sum(case when product_type in ('Project', 'Product', 'Service') then 1 else 0 end) as stages_pending_cnt,
																		sum(case when product_type in ('Project', 'Product', 'Service') then cast(replace(quoted_price, ',', '') as float) / @million else 0 end) as stages_pending_mb

																	from Quotation

																	where department = @department and CAST(YEAR(date) AS VARCHAR(4)) = @year and stages<> '' 
																	group by date, product_type, stages having date like '{year}%' and stages is not null and stages not in ('','Closed(Won)','Closed(Lost)','No go')) as sub_stages
																group by sub_stages.month, sub_stages.product_type) as sub_pending
                                                        group by sub_pending.month
													) as sub_sub_pending

                                                    ON CAST(YEAR(date) AS VARCHAR(4)) + '-' + right('00' + CAST(MONTH(date) AS VARCHAR(2)), 2) = sub_sub_pending.month
													
                                                    where Quotation.department = @department and CAST(YEAR(date) AS VARCHAR(4)) = @year
													group by date having date like '{year}%'
													)as sub_main
													  
													group by sub_main.month )
                                                    
                                                    --name
													select * from main
													                                           
													union all
                                                    --department
													select 
															'Total' as month,
															sum(main.quo_cnt) as quo_cnt,
															sum(main.quo_mb) as quo_mb,
															sum(main.product_won_cnt) as product_won_cnt ,
															sum(main.product_won_mb) as product_won_mb,
															sum(main.product_lost_cnt) as product_lost_cnt ,
															sum(main.product_lost_mb) as product_lost_mb,
															sum(main.product_nogo_cnt)  as product_nogo_cnt ,
															sum(main.product_nogo_mb)  as product_nogo_mb,
															sum(main.product_pending_cnt)  as product_pending_cnt ,
															sum(main.product_pending_mb) as product_pending_mb,
															sum(main.product_cnt) as product_cnt,
															sum(main.product_mb) as product_mb,

															sum(main.project_won_cnt) as project_won_cnt ,
															sum(main.project_won_mb) as project_won_mb,
															sum(main.project_lost_cnt) as project_lost_cnt ,
															sum(main.project_lost_mb) as project_lost_mb,
															sum(main.project_nogo_cnt) as project_nogo_cnt ,
															sum(main.project_nogo_mb) as project_nogo_mb,
															sum(main.project_pending_cnt) as project_pending_cnt ,
															sum(main.project_pending_mb) as project_pending_mb,
															sum(main.project_cnt) as project_cnt,
															sum(main.project_mb) as project_mb,

															sum(main.service_won_cnt) as service_won_cnt ,
															sum(main.service_won_mb) as service_won_mb,
															sum(main.service_lost_cnt) as service_lost_cnt ,
															sum(main.service_lost_mb) as service_lost_mb,
															sum(main.service_nogo_cnt) as service_nogo_cnt ,
															sum(main.service_nogo_mb)  as service_nogo_mb,
															sum(main.service_pending_cnt) as service_pending_cnt ,
															sum(main.service_pending_mb) as service_pending_mb,
															sum(main.service_cnt) as service_cnt,
															sum(main.service_mb) as service_mb,
                                                    
															sum(main.won_product_cnt) as won_product_cnt,
															sum(main.won_product_mb) as won_product_mb,
															sum(main.won_project_cnt) as won_project_cnt,
															sum(main.won_project_mb) as won_project_mb,
															sum(main.won_service_cnt) as won_service_cnt,
															sum(main.won_service_mb) as won_service_mb,
															sum(main.won_quo_cnt) as won_quo_cnt,
															sum(main.won_mb) as won_mb,
                                                    
															sum(main.lost_product_cnt) as lost_product_cnt,
															sum(main.lost_product_mb) as lost_product_mb,
															sum(main.lost_project_cnt) as lost_project_cnt,
															sum(main.lost_project_mb) as lost_project_mb,
															sum(main.lost_service_cnt) as lost_service_cnt,
															sum(main.lost_service_mb) as lost_service_mb,
															sum(main.loss_quo_cnt) as loss_quo_cnt,
															sum(main.loss_mb) as loss_mb,
                                                    
															sum(main.nogo_product_cnt) as nogo_product_cnt,
															sum(main.nogo_product_mb) as nogo_product_mb,
															sum(main.nogo_project_cnt) as nogo_project_cnt,
															sum(main.nogo_project_mb) as nogo_project_mb,
															sum(main.nogo_service_cnt) as nogo_service_cnt,
															sum(main.nogo_service_mb) as nogo_service_mb,
															sum(main.nogo_quo_cnt) as nogo_quo_cnt,
															sum(main.nogo_mb) as nogo_mb,
                                                    
															sum(main.pending_product_cnt) as pending_product_cnt,
															sum(main.pending_product_mb) as pending_product_mb,
															sum(main.pending_project_cnt) as pending_project_cnt,
															sum(main.pending_project_mb) as pending_project_mb,
															sum(main.pending_service_cnt) as pending_service_cnt,
															sum(main.pending_service_mb) as pending_service_mb,
															sum(main.pending_quo_cnt) as pending_quo_cnt,
															sum(main.pending_mb) as pending_mb
		
                                                    from main 																							
													order by  main.month");

                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Quotation_Report_YearModel r = new Quotation_Report_YearModel()
                        {
                            month = dr["month"].ToString(),
                            quo_mb = dr["quo_mb"].ToString(),
                            quo_cnt = dr["quo_cnt"].ToString(),
                            product_won_cnt = dr["product_won_cnt"].ToString(),
                            product_won_mb = dr["product_won_mb"].ToString(),
                            product_lost_cnt = dr["product_lost_cnt"].ToString(),
                            product_lost_mb = dr["product_lost_mb"].ToString(),
                            product_nogo_cnt = dr["product_nogo_cnt"].ToString(),
                            product_nogo_mb = dr["product_nogo_mb"].ToString(),
                            product_pending_cnt = dr["product_pending_cnt"].ToString(),
                            product_pending_mb = dr["product_pending_mb"].ToString(),
                            product_cnt = dr["product_cnt"].ToString(),
                            product_mb = dr["product_mb"].ToString(),
                            project_won_cnt = dr["project_won_cnt"].ToString(),
                            project_won_mb = dr["project_won_mb"].ToString(),
                            project_lost_cnt = dr["project_lost_cnt"].ToString(),
                            project_lost_mb = dr["project_lost_mb"].ToString(),
                            project_nogo_cnt = dr["project_nogo_cnt"].ToString(),
                            project_nogo_mb = dr["project_nogo_mb"].ToString(),
                            project_pending_cnt = dr["project_pending_cnt"].ToString(),
                            project_pending_mb = dr["project_pending_mb"].ToString(),
                            project_cnt = dr["project_cnt"].ToString(),
                            project_mb = dr["project_mb"].ToString(),
                            service_won_cnt = dr["service_won_cnt"].ToString(),
                            service_won_mb = dr["service_won_mb"].ToString(),
                            service_lost_cnt = dr["service_lost_cnt"].ToString(),
                            service_lost_mb = dr["service_lost_mb"].ToString(),
                            service_nogo_cnt = dr["service_nogo_cnt"].ToString(),
                            service_nogo_mb = dr["service_nogo_mb"].ToString(),
                            service_pending_cnt = dr["service_pending_cnt"].ToString(),
                            service_pending_mb = dr["service_pending_mb"].ToString(),
                            service_cnt = dr["service_cnt"].ToString(),
                            service_mb = dr["service_mb"].ToString(),
                            won_product_cnt = dr["won_product_cnt"].ToString(),
                            won_product_mb = dr["won_product_mb"].ToString(),
                            won_project_cnt = dr["won_project_cnt"].ToString(),
                            won_project_mb = dr["won_project_mb"].ToString(),
                            won_service_cnt = dr["won_service_cnt"].ToString(),
                            won_service_mb = dr["won_service_mb"].ToString(),
                            won_quo_cnt = dr["won_quo_cnt"].ToString(),
                            won_mb = dr["won_mb"].ToString(),
                            lost_product_cnt = dr["lost_product_cnt"].ToString(),
                            lost_product_mb = dr["lost_product_mb"].ToString(),
                            lost_project_cnt = dr["lost_project_cnt"].ToString(),
                            lost_project_mb = dr["lost_project_mb"].ToString(),
                            lost_service_cnt = dr["lost_service_cnt"].ToString(),
                            lost_service_mb = dr["lost_service_mb"].ToString(),
                            loss_quo_cnt = dr["loss_quo_cnt"].ToString(),
                            loss_mb = dr["loss_mb"].ToString(),
                            nogo_product_cnt = dr["nogo_product_cnt"].ToString(),
                            nogo_product_mb = dr["nogo_product_mb"].ToString(),
                            nogo_project_cnt = dr["nogo_project_cnt"].ToString(),
                            nogo_project_mb = dr["nogo_project_mb"].ToString(),
                            nogo_service_cnt = dr["nogo_service_cnt"].ToString(),
                            nogo_service_mb = dr["nogo_service_mb"].ToString(),
                            nogo_quo_cnt = dr["nogo_quo_cnt"].ToString(),
                            nogo_mb = dr["nogo_mb"].ToString(),
                            pending_product_cnt = dr["pending_product_cnt"].ToString(),
                            pending_product_mb = dr["pending_product_mb"].ToString(),
                            pending_project_cnt = dr["pending_project_cnt"].ToString(),
                            pending_project_mb = dr["pending_project_mb"].ToString(),
                            pending_service_cnt = dr["pending_service_cnt"].ToString(),
                            pending_service_mb = dr["pending_service_mb"].ToString(),
                            pending_quo_cnt = dr["pending_quo_cnt"].ToString(),
                            pending_mb = dr["pending_mb"].ToString()
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


