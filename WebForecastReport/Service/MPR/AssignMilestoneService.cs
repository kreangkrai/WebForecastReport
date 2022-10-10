using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Service.MPR
{
    public class AssignMilestoneService : IAssignMilestone
    {
        public List<AssignMilestoneModel> GetAssignedEngineers()
        {
            List<AssignMilestoneModel> engs = new List<AssignMilestoneModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string strCmd = string.Format($@"
                SELECT 
	                AssignMilestone.No,
	                AssignMilestone.Job_Milestone_ID,
	                Jobs.Job_ID,
	                Jobs.Job_Name,
                    Quotation.Customer,
	                Milestones.Milestone_ID,
	                Milestones.Milestone_Name,
	                JobMilestone.Start_Date,
	                JobMilestone.Stop_Date,
	                AssignMilestone.User_ID,
	                EngineerUsers.Display_Name,
	                EngineerUsers.Department,
	                AssignMilestone.Days,
	                ISNULL(T1.Assigned_Days,0) as Assigned_Days
                FROM AssignMilestone
                LEFT JOIN JobMilestone ON AssignMilestone.Job_Milestone_ID = JobMilestone.Job_Milestone_ID
                LEFT JOIN Jobs ON JobMilestone.Job_ID = Jobs.Job_ID
                LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                LEFT JOIN Milestones ON JobMilestone.Milestone_ID = Milestones.Milestone_ID
                LEFT JOIN EngineerUsers ON AssignMilestone.User_ID = EngineerUsers.User_ID
                LEFT JOIN (
	                SELECT 
		                JobMilestone.Job_Milestone_ID,
		                PlanManday.User_ID,
		                SUM(PlanManday.Hours / 8) AS Assigned_Days
	                FROM JobMilestone
	                LEFT JOIN PlanManday ON JobMilestone.Job_Milestone_ID = PlanManday.Job_Milestone_ID
	                GROUP BY JobMilestone.Job_Milestone_ID, PlanManday.User_ID
	                ) AS T1 
                ON AssignMilestone.Job_Milestone_ID = T1.Job_Milestone_ID AND AssignMilestone.User_ID = T1.User_ID
                ORDER BY Jobs.Job_ID, Milestones.Milestone_ID ASC");
                SqlCommand command = new SqlCommand(strCmd, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        AssignMilestoneModel eng = new AssignMilestoneModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            job_milestone_id = dr["Job_Milestone_ID"] != DBNull.Value ? dr["Job_Milestone_ID"].ToString() : "",
                            start_date = dr["Start_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Start_Date"]) : default(DateTime),
                            stop_date = dr["Stop_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Stop_Date"]) : default(DateTime),
                            job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                            job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                            customer = dr["Customer"] != DBNull.Value ? dr["Customer"].ToString() : "",
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : "",
                            user_id = dr["User_ID"] != DBNull.Value ? dr["User_ID"].ToString() : "",
                            user_name = dr["Display_Name"] != DBNull.Value ? dr["Display_Name"].ToString() : "",
                            department = dr["Department"] != DBNull.Value ? dr["Department"].ToString() : "",
                            days = dr["Days"] != DBNull.Value ? float.Parse(dr["Days"].ToString()) : 0,
                            assigned_days = dr["Assigned_Days"] != DBNull.Value ? float.Parse(dr["Assigned_Days"].ToString()) : 0
                        };
                        engs.Add(eng);
                    }
                    dr.Close();
                }
            }
            finally
            {
                connection.Close();
            }
            return engs;
        }

        public List<AssignMilestoneModel> GetEngineerAssignedJobs(string engId)
        {
            List<AssignMilestoneModel> engs = new List<AssignMilestoneModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string strCmd = string.Format($@"
                SELECT 
	                AssignMilestone.No,
	                AssignMilestone.Job_Milestone_ID,
	                Jobs.Job_ID,
	                Jobs.Job_Name,
	                Quotation.Customer,
	                Milestones.Milestone_ID,
	                Milestones.Milestone_Name,
	                JobMilestone.Start_Date,
	                JobMilestone.Stop_Date,
	                AssignMilestone.User_ID,
	                EngineerUsers.Display_Name,
	                EngineerUsers.Department,
	                AssignMilestone.Days,
	                ISNULL(T1.Assigned_Days,0) as Assigned_Days
                FROM AssignMilestone
                LEFT JOIN JobMilestone ON AssignMilestone.Job_Milestone_ID = JobMilestone.Job_Milestone_ID
                LEFT JOIN Jobs ON JobMilestone.Job_ID = Jobs.Job_ID
                LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                LEFT JOIN Milestones ON JobMilestone.Milestone_ID = Milestones.Milestone_ID
                LEFT JOIN EngineerUsers ON AssignMilestone.User_ID = EngineerUsers.User_ID
                LEFT JOIN (
	                SELECT 
		                JobMilestone.Job_Milestone_ID,
		                PlanManday.User_ID,
		                SUM(PlanManday.Hours / 8) AS Assigned_Days
	                FROM JobMilestone
	                LEFT JOIN PlanManday ON JobMilestone.Job_Milestone_ID = PlanManday.Job_Milestone_ID
	                GROUP BY JobMilestone.Job_Milestone_ID, PlanManday.User_ID
	                ) AS T1 
                ON AssignMilestone.Job_Milestone_ID = T1.Job_Milestone_ID AND AssignMilestone.User_ID = T1.User_ID
                WHERE AssignMilestone.User_ID = '{engId}'
                ORDER BY Jobs.Job_ID, Milestones.Milestone_ID ASC");
                SqlCommand command = new SqlCommand(strCmd, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        AssignMilestoneModel eng = new AssignMilestoneModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            job_milestone_id = dr["Job_Milestone_ID"] != DBNull.Value ? dr["Job_Milestone_ID"].ToString() : "",
                            start_date = dr["Start_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Start_Date"]) : default(DateTime),
                            stop_date = dr["Stop_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Stop_Date"]) : default(DateTime),
                            job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                            job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                            customer = dr["Customer"] != DBNull.Value ? dr["Customer"].ToString() : "",
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : "",
                            user_id = dr["User_ID"] != DBNull.Value ? dr["User_ID"].ToString() : "",
                            user_name = dr["Display_Name"] != DBNull.Value ? dr["Display_Name"].ToString() : "",
                            department = dr["Department"] != DBNull.Value ? dr["Department"].ToString() : "",
                            days = dr["Days"] != DBNull.Value ? float.Parse(dr["Days"].ToString()) : 0,
                            assigned_days = dr["Assigned_Days"] != DBNull.Value ? float.Parse(dr["Assigned_Days"].ToString()) : 0
                        };
                        engs.Add(eng);
                    }
                    dr.Close();
                }
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return engs;
        }

        public List<AssignMilestoneModel> GetJobAssignedEngineers(string jobId)
        {
            List<AssignMilestoneModel> engs = new List<AssignMilestoneModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string strCmd = string.Format($@"
                SELECT 
	                AssignMilestone.No,
	                AssignMilestone.Job_Milestone_ID,
	                Jobs.Job_ID,
	                Jobs.Job_Name,
	                Quotation.Customer,
	                Milestones.Milestone_ID,
	                Milestones.Milestone_Name,
	                JobMilestone.Start_Date,
	                JobMilestone.Stop_Date,
	                AssignMilestone.User_ID,
	                EngineerUsers.Display_Name,
	                EngineerUsers.Department,
	                AssignMilestone.Days,
	                ISNULL(T1.Assigned_Days,0) as Assigned_Days
                FROM AssignMilestone
                LEFT JOIN JobMilestone ON AssignMilestone.Job_Milestone_ID = JobMilestone.Job_Milestone_ID
                LEFT JOIN Jobs ON JobMilestone.Job_ID = Jobs.Job_ID
                LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                LEFT JOIN Milestones ON JobMilestone.Milestone_ID = Milestones.Milestone_ID
                LEFT JOIN EngineerUsers ON AssignMilestone.User_ID = EngineerUsers.User_ID
                LEFT JOIN (
	                SELECT 
		                JobMilestone.Job_Milestone_ID,
		                PlanManday.User_ID,
		                SUM(PlanManday.Hours / 8) AS Assigned_Days
	                FROM JobMilestone
	                LEFT JOIN PlanManday ON JobMilestone.Job_Milestone_ID = PlanManday.Job_Milestone_ID
	                GROUP BY JobMilestone.Job_Milestone_ID, PlanManday.User_ID
	                ) AS T1 
                ON AssignMilestone.Job_Milestone_ID = T1.Job_Milestone_ID AND AssignMilestone.User_ID = T1.User_ID
                WHERE Jobs.Job_ID = '{jobId}'
                ORDER BY Jobs.Job_ID, Milestones.Milestone_ID ASC");
                SqlCommand command = new SqlCommand(strCmd, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        AssignMilestoneModel eng = new AssignMilestoneModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            job_milestone_id = dr["Job_Milestone_ID"] != DBNull.Value ? dr["Job_Milestone_ID"].ToString() : "",
                            start_date = dr["Start_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Start_Date"]) : default(DateTime),
                            stop_date = dr["Stop_Date"] != DBNull.Value ? Convert.ToDateTime(dr["Stop_Date"]) : default(DateTime),
                            job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                            job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                            customer = dr["Customer"] != DBNull.Value ? dr["Customer"].ToString() : "",
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : "",
                            user_id = dr["User_ID"] != DBNull.Value ? dr["User_ID"].ToString() : "",
                            user_name = dr["Display_Name"] != DBNull.Value ? dr["Display_Name"].ToString() : "",
                            department = dr["Department"] != DBNull.Value ? dr["Department"].ToString() : "",
                            days = dr["Days"] != DBNull.Value ? float.Parse(dr["Days"].ToString()) : 0,
                            assigned_days = dr["Assigned_Days"] != DBNull.Value ? float.Parse(dr["Assigned_Days"].ToString()) : 0
                        };
                        engs.Add(eng);
                    }
                    dr.Close();
                }
            }
            catch
            {
                connection.Close();
            }
            return engs;
        }

        public string AddEngineer(AssignMilestoneModel asgEng)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                INSERT INTO AssignMilestone (
                    Job_Milestone_ID,
                    User_ID,
                    Days ) 
                VALUES (
                    @Job_Milestone_ID, 
                    @User_ID,
                    @Days
                )");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Job_Milestone_ID", asgEng.job_milestone_id);
                command.Parameters.AddWithValue("@User_ID", asgEng.user_id);
                command.Parameters.AddWithValue("@Days", asgEng.days);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return "Success";
        }

        public string EditEngineer(AssignMilestoneModel asgEng)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                UPDATE AssignMilestone SET Days = @Days WHERE Job_Milestone_ID = @Job_Milestone_ID AND User_ID = @User_ID");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Job_Milestone_ID", asgEng.job_milestone_id);
                command.Parameters.AddWithValue("@User_ID", asgEng.user_id);
                command.Parameters.AddWithValue("@Days", asgEng.days);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return "Success";
        }

        public string DeleteEngineer(AssignMilestoneModel asgEng)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                DELETE FROM AssignMilestone WHERE Job_Milestone_ID = @Job_Milestone_ID AND User_ID = @User_ID;
                DELETE FROM PlanManday WHERE Job_Milestone_ID = @Job_Milestone_ID AND User_ID = @User_ID;");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Job_Milestone_ID", asgEng.job_milestone_id);
                command.Parameters.AddWithValue("@User_ID", asgEng.user_id);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return "Success";
        }
    }
}
