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
        public List<Quotation_Report_DepartmentModel> GetReportDepartment(string department, string month)
        {
            try
            {
                List<Quotation_Report_DepartmentModel> reports = new List<Quotation_Report_DepartmentModel>();
                string command = "";
                if (department == "ALL")
                {
                    command = @"with s1 as(select department,sale_name as sale,
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
                                                    sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb
                                                    from Quotation where date like '" + month + "%' group by department,sale_name union all " +

                                                    "select (department + ' Total') as department, " +
                                                    "'' as sale," +
                                                    "cast(sum(sum(cast(replace(quoted_price,',','') as float))/1000000) over (partition by department) as decimal(10,2)) as quo_mb," +
                                                    "count(quotation_no) as quo_cnt," +
                                                    "sum(case when product_type ='product' then 1 else 0 end) as product_cnt," +
                                                    "sum(case when product_type ='product' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as product_mb," +
                                                    "sum(case when product_type ='project' then 1 else 0 end) as project_cnt," +
                                                    "sum(case when product_type ='project' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as project_mb," +
                                                    "sum(case when product_type ='service' then 1 else 0 end) as service_cnt," +
                                                    "sum(case when product_type ='service' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as service_mb," +
                                                    "sum(case when stages='Closed(Won)' then 1 else 0 end) as won_quo_cnt," +
                                                    "sum(case when stages='Closed(Won)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as won_mb," +
                                                    "sum(case when stages='Closed(Lost)' then 1 else 0 end) as loss_quo_cnt," +
                                                    "sum(case when stages='Closed(Lost)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as loss_mb," +
                                                    "sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt," +
                                                    "sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb " +
                                                    "from Quotation where date like '" + month + "%' group by department union all " +

                                                    "select ('Total') as department," +
                                                    "'' as sale," +
                                                    "cast(sum(cast(replace(quoted_price,',','') as float))/1000000 as decimal(10,2)) as quo_mb," +
                                                    "count(quotation_no)as quo_cnt," +
                                                    "sum(case when product_type ='product' then 1 else 0 end) as product_cnt," +
                                                    "sum(case when product_type ='product' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as product_mb," +
                                                    "sum(case when product_type ='project' then 1 else 0 end) as project_cnt," +
                                                    "sum(case when product_type ='project' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as project_mb," +
                                                    "sum(case when product_type ='service' then 1 else 0 end) as service_cnt," +
                                                    "sum(case when product_type ='service' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as service_mb," +
                                                    "sum(case when stages='Closed(Won)' then 1 else 0 end) as won_quo_cnt," +
                                                    "sum(case when stages='Closed(Won)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as won_mb," +
                                                    "sum(case when stages='Closed(Lost)' then 1 else 0 end) as loss_quo_cnt," +
                                                    "sum(case when stages='Closed(Lost)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as loss_mb," +
                                                    "sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt," +
                                                    "sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb " +
                                                    "from Quotation where date like '" + month + "%') " +
                                                    "select * from s1 order by s1.department";
                }
                else
                {
                    command = @"with s1 as (select department, sale_name as sale,
                                                    cast(sum(sum(cast(replace(quoted_price, ',', '') as float)) / 1000000) over(partition by sale_name) as decimal(10,2)) as quo_mb,
                                                    count(quotation_no) as quo_cnt,
                                                    sum(case when product_type = 'product' then 1 else 0 end) as product_cnt,
													sum(case when product_type = 'product' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end) as product_mb,
                                                    sum(case when product_type = 'project' then 1 else 0 end) as project_cnt,
													sum(case when product_type = 'project' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end) as project_mb,
                                                    sum(case when product_type = 'service' then 1 else 0 end) as service_cnt,
													sum(case when product_type = 'service' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end) as service_mb,
                                                    sum(case when stages = 'Closed(Won)' then 1 else 0 end) as won_quo_cnt,
                                                    sum(case when stages = 'Closed(Won)' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end) as won_mb,
                                                    sum(case when stages = 'Closed(Lost)' then 1 else 0 end) as loss_quo_cnt,
                                                    sum(case when stages = 'Closed(Lost)' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end) as loss_mb,
                                                    sum(case when stages = 'No go' then 1 else 0 end) as nogo_quo_cnt,
                                                    sum(case when stages = 'No go' then cast(replace(quoted_price, ',', '') as float) / 1000000 else 0 end) as nogo_mb
                                                    from Quotation where department='" + department + "' and date like '" + month + "%' group by department,sale_name union all " +

                                                    "select (department + ' Total') as department, " +
                                                    "'' as sale," +
                                                    "cast(sum(sum(cast(replace(quoted_price,',','') as float))/1000000) over (partition by department) as decimal(10,2)) as quo_mb," +
                                                    "count(quotation_no) as quo_cnt," +
                                                    "sum(case when product_type ='product' then 1 else 0 end) as product_cnt," +
                                                    "sum(case when product_type ='product' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as product_mb," +
                                                    "sum(case when product_type ='project' then 1 else 0 end) as project_cnt," +
                                                    "sum(case when product_type ='project' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as project_mb," +
                                                    "sum(case when product_type ='service' then 1 else 0 end) as service_cnt," +
                                                    "sum(case when product_type ='service' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as service_mb," +
                                                    "sum(case when stages='Closed(Won)' then 1 else 0 end) as won_quo_cnt," +
                                                    "sum(case when stages='Closed(Won)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as won_mb," +
                                                    "sum(case when stages='Closed(Lost)' then 1 else 0 end) as loss_quo_cnt," +
                                                    "sum(case when stages='Closed(Lost)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as loss_mb," +
                                                    "sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt," +
                                                    "sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb " +
                                                    "from Quotation where department='" + department + "' and date like '" + month + "%' group by department union all " +

                                                    "select ('Total') as department," +
                                                    "'' as sale," +
                                                    "cast(sum(cast(replace(quoted_price,',','') as float))/1000000 as decimal(10,2)) as quo_mb," +
                                                    "count(quotation_no)as quo_cnt," +
                                                    "sum(case when product_type ='product' then 1 else 0 end) as product_cnt," +
                                                    "sum(case when product_type ='product' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as product_mb," +
                                                    "sum(case when product_type ='project' then 1 else 0 end) as project_cnt," +
                                                    "sum(case when product_type ='project' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as project_mb," +
                                                    "sum(case when product_type ='service' then 1 else 0 end) as service_cnt," +
                                                    "sum(case when product_type ='service' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as service_mb," +
                                                    "sum(case when stages='Closed(Won)' then 1 else 0 end) as won_quo_cnt," +
                                                    "sum(case when stages='Closed(Won)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as won_mb," +
                                                    "sum(case when stages='Closed(Lost)' then 1 else 0 end) as loss_quo_cnt," +
                                                    "sum(case when stages='Closed(Lost)' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as loss_mb," +
                                                    "sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt," +
                                                    "sum(case when stages='No go' then cast(cast(replace(quoted_price,',','') as float)/1000000 as decimal(10,2)) else 0 end) as nogo_mb " +
                                                    "from Quotation where department='" + department + "' and date like '" + month + "%') " +
                                                    "select * from s1 order by s1.department";
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
                            nogo_mb = dr["nogo_mb"].ToString()
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
                            "select* from s1 order by s1.department ";
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
                            "from Quotation where department='" + department + "' group by department union all " +

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
                            "from Quotation where department='" + department + "') " +
                            "select* from s1 order by s1.department ";
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
                            sum_q1 =
                            ((dr["jan_in"].ToString() != "" ? float.Parse(dr["jan_in"].ToString()) : 0) +
                             (dr["jan_out"].ToString() != "" ? float.Parse(dr["jan_out"].ToString()) : 0) +
                             (dr["feb_in"].ToString() != "" ? float.Parse(dr["feb_in"].ToString()) : 0) +
                             (dr["feb_out"].ToString() != "" ? float.Parse(dr["feb_out"].ToString()) : 0) +
                             (dr["mar_in"].ToString() != "" ? float.Parse(dr["mar_in"].ToString()) : 0) +
                             (dr["mar_out"].ToString() != "" ? float.Parse(dr["mar_out"].ToString()) : 0)).ToString(),
                            apr_in = dr["apr_in"].ToString(),
                            apr_out = dr["apr_out"].ToString(),
                            may_in = dr["may_in"].ToString(),
                            may_out = dr["may_out"].ToString(),
                            jun_in = dr["jun_in"].ToString(),
                            jun_out = dr["jun_out"].ToString(),
                            sum_q2 =
                            ((dr["apr_in"].ToString() != "" ? float.Parse(dr["apr_in"].ToString()) : 0) +
                             (dr["apr_out"].ToString() != "" ? float.Parse(dr["apr_out"].ToString()) : 0) +
                             (dr["may_in"].ToString() != "" ? float.Parse(dr["may_in"].ToString()) : 0) +
                             (dr["may_out"].ToString() != "" ? float.Parse(dr["may_out"].ToString()) : 0) +
                             (dr["jun_in"].ToString() != "" ? float.Parse(dr["jun_in"].ToString()) : 0) +
                             (dr["jun_out"].ToString() != "" ? float.Parse(dr["jun_out"].ToString()) : 0)).ToString(),
                            jul_in = dr["jul_in"].ToString(),
                            jul_out = dr["jul_out"].ToString(),
                            aug_in = dr["aug_in"].ToString(),
                            aug_out = dr["aug_out"].ToString(),
                            sep_in = dr["sep_in"].ToString(),
                            sep_out = dr["sep_out"].ToString(),
                            sum_q3 =
                            ((dr["jul_in"].ToString() != "" ? float.Parse(dr["jul_in"].ToString()) : 0) +
                             (dr["jul_out"].ToString() != "" ? float.Parse(dr["jul_out"].ToString()) : 0) +
                             (dr["aug_in"].ToString() != "" ? float.Parse(dr["aug_in"].ToString()) : 0) +
                             (dr["aug_out"].ToString() != "" ? float.Parse(dr["aug_out"].ToString()) : 0) +
                             (dr["sep_in"].ToString() != "" ? float.Parse(dr["sep_in"].ToString()) : 0) +
                             (dr["sep_out"].ToString() != "" ? float.Parse(dr["sep_out"].ToString()) : 0)).ToString(),
                            oct_in = dr["oct_in"].ToString(),
                            oct_out = dr["oct_out"].ToString(),
                            nov_in = dr["nov_in"].ToString(),
                            nov_out = dr["nov_out"].ToString(),
                            dec_in = dr["dec_in"].ToString(),
                            dec_out = dr["dec_out"].ToString(),
                            sum_q4 =
                            ((dr["oct_in"].ToString() != "" ? float.Parse(dr["oct_in"].ToString()) : 0) +
                             (dr["oct_out"].ToString() != "" ? float.Parse(dr["oct_out"].ToString()) : 0) +
                             (dr["nov_in"].ToString() != "" ? float.Parse(dr["nov_in"].ToString()) : 0) +
                             (dr["nov_out"].ToString() != "" ? float.Parse(dr["nov_out"].ToString()) : 0) +
                             (dr["dec_in"].ToString() != "" ? float.Parse(dr["dec_in"].ToString()) : 0) +
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
    }
}
