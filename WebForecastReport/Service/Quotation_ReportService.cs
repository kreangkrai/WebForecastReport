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
                SqlCommand cmd = new SqlCommand(@"  select sale_name as sale,
                                                    sum(sum(cast(replace(quoted_price,',','') as float))/1000000) over (partition by sale_name) as quo_mb,
                                                    count(quotation_no) as quo_cnt,
                                                    sum(case when product_type ='product' then 1 else 0 end) as product_cnt,
                                                    sum(case when product_type ='project' then 1 else 0 end) as project_cnt,
                                                    sum(case when product_type ='service' then 1 else 0 end) as service_cnt,
                                                    sum(case when stages='Close(Won)' then 1 else 0 end) as won_quo_cnt,
                                                    sum(case when stages='Close(Won)' then cast(replace(quoted_price,',','') as float)/1000000 else 0 end) as won_mb,
                                                    sum(case when stages='Close(Loss)' then 1 else 0 end) as loss_quo_cnt,
                                                    sum(case when stages='Close(Loss)' then cast(replace(quoted_price,',','') as float)/1000000 else 0 end) as loss_mb,
                                                    sum(case when stages='No go' then 1 else 0 end) as nogo_quo_cnt,
                                                    sum(case when stages='No go' then cast(replace(quoted_price,',','') as float)/1000000 else 0 end) as nogo_mb
                                                    from Quotation where department='" + department + "' and date like '" + month + "%' group by sale_name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Quotation_Report_DepartmentModel r = new Quotation_Report_DepartmentModel()
                        {
                            sale = dr["sale"].ToString(),
                            quo_mb = dr["quo_mb"].ToString(),
                            quo_cnt = dr["quo_cnt"].ToString(),
                            product_cnt = dr["product_cnt"].ToString(),
                            project_cnt = dr["project_cnt"].ToString(),
                            service_cnt = dr["service_cnt"].ToString(),
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
    }
}
