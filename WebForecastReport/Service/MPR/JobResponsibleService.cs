﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Service.MPR
{
    public class JobResponsibleService : IJobResponsible
    {
        public List<JobResponsibleModel> GetJobResponsible(string user_name)
        {
            List<JobResponsibleModel> jrs = new List<JobResponsibleModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
	                    JobResponsible.job_id,
	                    Jobs.job_name,
                        Quotation.customer,
	                    JobResponsible.user_id,
	                    Sale_User.Name AS user_name,
	                    Sale_User.Department2 AS department,
	                    JobResponsible.role,
	                    JobResponsible.assign_by,
	                    JobResponsible.assign_date
                    FROM JobResponsible
                        LEFT JOIN Jobs ON JobResponsible.job_id = Jobs.job_id
                        LEFT JOIN [gps_sale_tracking].[dbo].Sale_User AS Sale_User ON JobResponsible.user_id = Sale_User.Login
                        LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                    WHERE LOWER(Sale_User.Name) = '{user_name}'
                    ORDER BY JobResponsible.job_id");
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
                        JobResponsibleModel jr = new JobResponsibleModel()
                        {
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["user_name"] != DBNull.Value ? dr["user_name"].ToString() : "",
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                            department = dr["department"] != DBNull.Value ? dr["department"].ToString() : "",
                            role = dr["role"] != DBNull.Value ? dr["role"].ToString() : "",
                            assign_by = dr["assign_by"] != DBNull.Value ? dr["assign_by"].ToString() : "",
                            assign_date = dr["assign_date"] != DBNull.Value ? Convert.ToDateTime(dr["assign_date"]) : default(DateTime),
                        };
                        jrs.Add(jr);
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
            return jrs;
        }

        public List<JobResponsibleModel> GetJobLists()
        {
            List<JobResponsibleModel> jrs = new List<JobResponsibleModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
	                    Jobs.job_id,
                        Jobs.job_name,
                        Quotation.customer,
	                    JobResponsible.user_id,
                        Sale_User.Name
                    FROM Jobs
                    LEFT JOIN JobResponsible ON Jobs.job_id = JobResponsible.job_id
                    LEFT JOIN [gps_sale_tracking].dbo.Sale_User ON JobResponsible.user_id = Sale_User.Login
                    LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                    ORDER BY Jobs.job_id, JobResponsible.user_id");
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
                        JobResponsibleModel jr = new JobResponsibleModel()
                        {
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "",
                        };
                        jrs.Add(jr);
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
            return jrs;
        }

        public List<QuotationResponsibleModel> GetQuotationResponsible(string user_name)
        {
            List<QuotationResponsibleModel> qrs = new List<QuotationResponsibleModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT 
	                    Proposal.quotation_no,
	                    Quotation.project_name,
                        Quotation.customer
                    FROM Proposal
	                    LEFT JOIN Quotation ON Proposal.quotation_no = Quotation.quotation_no
                    WHERE Proposal.engineer_in_charge LIKE '%{user_name}%'
                    ORDER BY Proposal.quotation_no");
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
                        QuotationResponsibleModel qr = new QuotationResponsibleModel()
                        {
                            quotation_no = dr["quotation_no"] != DBNull.Value ? dr["quotation_no"].ToString() : "",
                            project_name = dr["project_name"] != DBNull.Value ? dr["project_name"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                        };
                        qrs.Add(qr);
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
            return qrs;
        }

        public List<JobResponsibleModel> GetAssignEngineers(string job_id)
        {
            List<JobResponsibleModel> jrs = new List<JobResponsibleModel>();
            try
            {
                string string_command = string.Format($@"
                    SELECT
	                    JobResponsible.job_id,
	                    Jobs.job_name,
                        Quotation.customer,
	                    JobResponsible.user_id,
	                    Sale_User.Name AS user_name,
	                    Sale_User.Department2 AS department,
	                    JobResponsible.role,
	                    JobResponsible.assign_by,
                        JobResponsible.assign_date
                    FROM JobResponsible
                        LEFT JOIN Jobs ON JobResponsible.job_id = Jobs.job_id
                        LEFT JOIN [gps_sale_tracking].[dbo].Sale_User AS Sale_User ON JobResponsible.user_id = Sale_User.Login
                        LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                    WHERE  JobResponsible.job_id = '{job_id}'
                    ORDER BY JobResponsible.job_id");
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
                        JobResponsibleModel jr = new JobResponsibleModel()
                        {
                            user_id = dr["user_id"] != DBNull.Value ? dr["user_id"].ToString() : "",
                            user_name = dr["user_name"] != DBNull.Value ? dr["user_name"].ToString() : "",
                            job_id = dr["job_id"] != DBNull.Value ? dr["job_id"].ToString() : "",
                            job_name = dr["job_name"] != DBNull.Value ? dr["job_name"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                            department = dr["department"] != DBNull.Value ? dr["department"].ToString() : "",
                            role = dr["role"] != DBNull.Value ? dr["role"].ToString() : "",
                            assign_by = dr["assign_by"] != DBNull.Value ? dr["assign_by"].ToString() : "",
                            assign_date = dr["assign_date"] != DBNull.Value ? Convert.ToDateTime(dr["assign_date"].ToString()) : default(DateTime),
                        };
                        jrs.Add(jr);
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
            return jrs;
        }

        public string AddJobResponsible(JobResponsibleModel jr)
        {
            try
            {
                string string_command = string.Format($@"
                    BEGIN
                        IF NOT EXISTS (
                            SELECT job_id, user_id FROM JobResponsible WHERE user_id = @user_id AND job_id = @job_id
                        )
                        BEGIN
                            INSERT INTO JobResponsible(job_id, user_id, role, assign_by, assign_date)
                            VALUES(@job_id, @user_id, @role, @assign_by, @assign_date)
                        END
                    END
                ");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@job_id", jr.job_id.Replace("-", String.Empty));
                    cmd.Parameters.AddWithValue("@user_id", jr.user_id);
                    cmd.Parameters.AddWithValue("@role", jr.role);
                    cmd.Parameters.AddWithValue("@assign_by", jr.assign_by);
                    cmd.Parameters.AddWithValue("@assign_date", jr.assign_date);
                    if (ConnectSQL.con.State != System.Data.ConnectionState.Open)
                    {
                        ConnectSQL.CloseConnect();
                        ConnectSQL.OpenConnect();
                    }
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception exception)
            {
                return exception.Message;
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
    }
}
