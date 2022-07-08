using WebForecastReport.Interfaces.MPR;
using WebForecastReport.Models.MPR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Service;

namespace WebForecastReport.Services.MPR
{
    public class JobService : IJob
    {
        public List<JobModel> GetAllJobs()
        {
            List<JobModel> jobs = new List<JobModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        Jobs.job_id,
                        Jobs.job_name,
                        Jobs.cost,
                        Jobs.md_rate,
                        Jobs.pd_rate,
                        Jobs.status,
                        Jobs.quotation_no,
                        Quotation.customer,
	                    Quotation.enduser,
	                    Quotation.sale_name,
	                    Quotation.department
                    FROM Jobs
                    LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no");
                SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                    ConnectSQL.OpenConnect();
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        JobModel job = new JobModel()
                        {
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            cost = dr["cost"] != DBNull.Value ? Convert.ToInt32(dr["cost"]) : 0,
                            md_rate = dr["md_rate"] != DBNull.Value ? Convert.ToDouble(dr["md_rate"]) : 1,
                            pd_rate = dr["pd_rate"] != DBNull.Value ? Convert.ToDouble(dr["pd_rate"]) : 1,
                            factor = 0,
                            manpower = 0,
                            cost_per_manpower = 0,
                            ot_manpower = 0,
                            status = dr["status"] != DBNull.Value ? dr["status"].ToString() : "",
                            quotation_no = dr["quotation_no"] != DBNull.Value ? dr["quotation_no"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                            enduser = dr["enduser"] != DBNull.Value ? dr["enduser"].ToString() : "",
                            sale_name = dr["sale_name"] != DBNull.Value ? dr["sale_name"].ToString() : "",
                            department = dr["department"] != DBNull.Value ? dr["department"].ToString() : "",
                        };
                        job.factor = job.md_rate + job.pd_rate;
                        jobs.Add(job);
                    }
                    dr.Close();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return jobs;
        }

        public string CreateJob(JobModel job)
        {
            try
            {
                string string_command = string.Format($@"
                    INSERT INTO 
                        Jobs(job_id, job_name, quotation_no, cost, md_rate, pd_rate, status)
                        VALUES(@job_id, @job_name, @quotation_no, @cost, @md_rate, @pd_rate, @status)");
                using (SqlCommand cmd = new SqlCommand(string_command,ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@job_id", job.job_id.Replace("-", String.Empty));
                    cmd.Parameters.AddWithValue("@job_name", job.job_name);
                    cmd.Parameters.AddWithValue("@quotation_no", job.quotation_no);
                    cmd.Parameters.AddWithValue("@cost", job.cost);
                    cmd.Parameters.AddWithValue("@md_rate", job.md_rate);
                    cmd.Parameters.AddWithValue("@pd_rate", job.pd_rate);
                    cmd.Parameters.AddWithValue("@status", job.status);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return "Success";
        }

        public string UpdateJob(JobModel job)
        {
            try
            {
                string string_command = string.Format($@"
                    UPDATE Jobs 
                    SET
                        job_name = @job_name,
                        quotation_no = @quotation_no,
                        cost = @cost,
                        md_rate = @md_rate,
                        pd_rate = @pd_rate,
                        status = @status 
                    WHERE job_id = @job_id");
                using (SqlCommand cmd = new SqlCommand(string_command,ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@job_id", job.job_id.Replace("-", String.Empty));
                    cmd.Parameters.AddWithValue("@job_name", job.job_name);
                    cmd.Parameters.AddWithValue("@quotation_no", job.quotation_no);
                    cmd.Parameters.AddWithValue("@cost", job.cost);
                    cmd.Parameters.AddWithValue("@md_rate", job.md_rate);
                    cmd.Parameters.AddWithValue("@pd_rate", job.pd_rate);
                    cmd.Parameters.AddWithValue("@status", job.status);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return "Success";
        }

        public List<JobQuotationModel> GetJobQuotations(string year)
        {
            List<JobQuotationModel> quots = new List<JobQuotationModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
                        quotation_no,
                        customer
                    FROM Quotation
                    WHERE quotation_no Like 'Q{year}%' OR 
                    quotation_no Like 'Q{Convert.ToInt32(year) - 1}%'");
                SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect());
                if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                    ConnectSQL.OpenConnect();
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        JobQuotationModel quot = new JobQuotationModel()
                        {
                            quotation_no = dr["quotation_no"] != DBNull.Value ? dr["quotation_no"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                        };
                        quots.Add(quot);
                    }
                    dr.Close();
                }
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
            return quots;
        }
    }
}
