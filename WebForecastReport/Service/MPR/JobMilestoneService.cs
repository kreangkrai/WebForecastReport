using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Service.MPR
{
    public class JobMilestoneService : IJobMilestone
    {
        public List<JobMilestoneModel> GetJobsMilestones()
        {
            List<JobMilestoneModel> jms = new List<JobMilestoneModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                SELECT 
                    JobMilestone.No,
                    Job_Milestone_ID,
                    JobMilestone.Job_ID,
                    Jobs.Job_Name,
                    Quotation.customer,
                    JobMilestone.Milestone_ID,
                    Milestones.Milestone_Name,
                    JobMilestone.Start_Date,
                    JobMilestone.Stop_Date
                FROM JobMilestone
                LEFT JOIN Jobs ON JobMilestone.Job_ID = Jobs.Job_ID
                LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                LEFT JOIN Milestones ON JobMilestone.Milestone_ID = Milestones.Milestone_ID
                ORDER BY JobMilestone.Job_ID, JobMilestone.Start_Date");
                SqlCommand command = new SqlCommand(string_command, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        JobMilestoneModel jm = new JobMilestoneModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            job_milestone_id = dr["Job_Milestone_ID"] != DBNull.Value ? dr["Job_Milestone_ID"].ToString() : "",
                            job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                            job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : "",
                            start_date = dr["Start_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Start_Date"]) : default(DateTime),
                            stop_date = dr["Stop_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Stop_Date"]) : default(DateTime)
                        };
                        jms.Add(jm);
                    }
                    dr.Close();
                }
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return jms;
        }

        public List<JobMilestoneModel> GetJobMilestones(string jobId)
        {
            List<JobMilestoneModel> jobMilestones = new List<JobMilestoneModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                SELECT 
	                JobMilestone.No,
	                JobMilestone.Job_Milestone_ID,
	                Jobs.Job_ID,
	                Jobs.Job_Name,
                    Quotation.customer,
	                Milestones.Milestone_ID,
	                Milestones.Milestone_Name,
	                JobMilestone.Start_Date,
	                JobMilestone.Stop_Date
                FROM JobMilestone
                LEFT JOIN Jobs ON JobMilestone.Job_ID = Jobs.Job_ID
                LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                LEFT JOIN Milestones ON JobMilestone.Milestone_ID = Milestones.Milestone_ID
                WHERE Jobs.Job_ID = '{jobId}'
                ORDER BY JobMilestone.Start_Date");
                SqlCommand command = new SqlCommand(string_command, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        JobMilestoneModel jobMilestone = new JobMilestoneModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            job_milestone_id = dr["Job_Milestone_ID"] != DBNull.Value ? dr["Job_Milestone_ID"].ToString() : "",
                            job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                            job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : "",
                            start_date = dr["Start_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Start_Date"]) : default(DateTime),
                            stop_date = dr["Stop_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Stop_Date"]) : default(DateTime)
                        };
                        jobMilestones.Add(jobMilestone);
                    }
                    dr.Close();
                }
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return jobMilestones;
        }

        public List<JobMilestoneModel> GetJobsMilestonesAfterDate(DateTime date)
        {
            List<JobMilestoneModel> jobMilestones = new List<JobMilestoneModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                SELECT 
	                JobMilestone.No,
	                JobMilestone.Job_Milestone_ID,
	                Jobs.Job_ID,
	                Jobs.Job_Name,
	                Quotation.customer,
	                Milestones.Milestone_ID,
	                Milestones.Milestone_Name,
	                JobMilestone.Start_Date,
	                JobMilestone.Stop_Date
                FROM JobMilestone
                LEFT JOIN Jobs ON JobMilestone.Job_ID = Jobs.Job_ID
                LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                LEFT JOIN Milestones ON JobMilestone.Milestone_ID = Milestones.Milestone_ID
                WHERE JobMilestone.Stop_Date >= CAST('{date.ToString("yyyy-MM-dd")}' AS DATE)
                ORDER BY JobMilestone.Start_Date");
                SqlCommand command = new SqlCommand(string_command, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        JobMilestoneModel jobMilestone = new JobMilestoneModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            job_milestone_id = dr["Job_Milestone_ID"] != DBNull.Value ? dr["Job_Milestone_ID"].ToString() : "",
                            job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                            job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                            customer = dr["customer"] != DBNull.Value ? dr["customer"].ToString() : "",
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : "",
                            start_date = dr["Start_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Start_Date"]) : default(DateTime),
                            stop_date = dr["Stop_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Stop_Date"]) : default(DateTime)
                        };
                        jobMilestones.Add(jobMilestone);
                    }
                    dr.Close();
                }
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }            
            return jobMilestones;
        }

        public string CreateJobMilestone(JobMilestoneModel jm)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                INSERT INTO JobMilestone (
                    Job_Milestone_ID,
                    Job_ID,
                    Milestone_ID,
                    Start_Date,
                    Stop_Date ) 
                VALUES (
                    @Job_Milestone_ID, 
                    @Job_ID, 
                    @Milestone_ID,
                    @Start_Date,
                    @Stop_Date )");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Job_Milestone_ID", jm.job_milestone_id);
                command.Parameters.AddWithValue("@Job_ID", jm.job_id);
                command.Parameters.AddWithValue("@Milestone_ID", jm.milestone_id);
                command.Parameters.AddWithValue("@Start_Date", jm.start_date);
                command.Parameters.AddWithValue("@Stop_Date", jm.stop_date);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return "Success";
        }

        public string EditJobMilestone(JobMilestoneModel jm)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                UPDATE JobMilestone 
                SET Start_Date = @Start_Date, Stop_Date = @Stop_Date 
                WHERE Job_Milestone_ID = @Job_Milestone_ID");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Job_Milestone_ID", jm.job_milestone_id);
                command.Parameters.AddWithValue("@Start_Date", jm.start_date);
                command.Parameters.AddWithValue("@Stop_Date", jm.stop_date);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return "Success";
        }

        public string DeleteJobMilestone(JobMilestoneModel jm)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                DELETE FROM JobMilestone WHERE Job_Milestone_ID = @Job_Milestone_ID;
                DELETE FROM AssignEngineer WHERE Job_Milestone_ID = @Job_Milestone_ID;
                DELETE FROM PlanDay WHERE Job_Milestone_ID = @Job_Milestone_ID;");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Job_Milestone_ID", jm.job_milestone_id);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return "Success";
        }

        public string DeleteAllJobMilestones(JobMilestoneModel jm)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                DELETE FROM JobMilestone WHERE Job_ID = @Job_ID;
                DELETE FROM AssignEngineer WHERE Job_Milestone_ID LIKE '{jm.job_id}%';
                DELETE FROM PlanDay WHERE Job_Milestone_ID LIKE '{jm.job_id}%';");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Job_ID", jm.job_id);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return "Success";
        }
    }
}
