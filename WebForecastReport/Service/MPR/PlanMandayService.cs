using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Service.MPR
{
    public class PlanMandayService : IPlanManday
    {
        public List<PlanMandayModel> GetJobsPlans()
        {
            List<PlanMandayModel> plans = new List<PlanMandayModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                SELECT 
	                PlanManday.No,
	                PlanManday.Job_Milestone_ID,
	                Jobs.Job_ID,
	                Jobs.Job_Name,
	                Quotation.Customer,
	                Milestones.Milestone_ID,
	                Milestones.Milestone_Name,
	                PlanManday.User_ID,
	                EngineerUsers.Display_Name,
	                EngineerUsers.Department,
                    PlanManday.Date,
                    PlanManday.Hours
                FROM PlanManday
                LEFT JOIN JobMilestone ON PlanManday.Job_Milestone_ID = JobMilestone.Job_Milestone_ID
                LEFT JOIN Jobs ON JobMilestone.Job_ID = Jobs.Job_ID
                LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                LEFT JOIN Milestones ON JobMilestone.Milestone_ID = Milestones.Milestone_ID
                LEFT JOIN EngineerUsers ON PlanManday.User_ID = EngineerUsers.User_ID");
                SqlCommand command = new SqlCommand(string_command, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PlanMandayModel plan = new PlanMandayModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            job_milestone_id = dr["Job_Milestone_ID"] != DBNull.Value ? dr["Job_Milestone_ID"].ToString() : "",
                            job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                            job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                            customer = dr["Customer"] != DBNull.Value ? dr["Customer"].ToString() : "",
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : "",
                            user_id = dr["User_ID"] != DBNull.Value ? dr["User_ID"].ToString() : "",
                            user_name = dr["Display_Name"] != DBNull.Value ? dr["Display_Name"].ToString() : "",
                            department = dr["Department"] != DBNull.Value ? dr["Department"].ToString() : "",
                            date = dr["Date"] != DBNull.Value ? Convert.ToDateTime(dr["Date"]) : default(DateTime),
                            hours = dr["Hours"] != DBNull.Value ? float.Parse(dr["Hours"].ToString()) : 0,
                        };
                        plans.Add(plan);
                    }
                    dr.Close();
                }
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return plans;
        }

        public List<PlanMandayModel> GetJobPlans(string jobId)
        {
            List<PlanMandayModel> plans = new List<PlanMandayModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                SELECT 
	                PlanManday.No,
	                PlanManday.Job_Milestone_ID,
	                Jobs.Job_ID,
	                Jobs.Job_Name,
	                Quotation.Customer,
	                Milestones.Milestone_ID,
	                Milestones.Milestone_Name,
	                PlanManday.User_ID,
	                EngineerUsers.Display_Name,
	                EngineerUsers.Department,
	                PlanManday.Date,
	                PlanManday.Hours
                FROM PlanManday
                LEFT JOIN JobMilestone ON PlanManday.Job_Milestone_ID = JobMilestone.Job_Milestone_ID
                LEFT JOIN Jobs ON JobMilestone.Job_ID = Jobs.Job_ID
                LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                LEFT JOIN Milestones ON JobMilestone.Milestone_ID = Milestones.Milestone_ID
                LEFT JOIN EngineerUsers ON PlanManday.User_ID = EngineerUsers.User_ID
                WHERE Jobs.Job_ID = '{jobId}'");
                SqlCommand command = new SqlCommand(string_command, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PlanMandayModel plan = new PlanMandayModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            job_milestone_id = dr["Job_Milestone_ID"] != DBNull.Value ? dr["Job_Milestone_ID"].ToString() : "",
                            job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                            job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                            customer = dr["Customer"] != DBNull.Value ? dr["Customer"].ToString() : "",
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : "",
                            user_id = dr["User_ID"] != DBNull.Value ? dr["User_ID"].ToString() : "",
                            user_name = dr["Display_Name"] != DBNull.Value ? dr["Display_Name"].ToString() : "",
                            department = dr["Department"] != DBNull.Value ? dr["Department"].ToString() : "",
                            date = dr["Date"] != DBNull.Value ? Convert.ToDateTime(dr["Date"]) : default(DateTime),
                            hours = dr["Hours"] != DBNull.Value ? float.Parse(dr["Hours"].ToString()) : 0,
                        };
                        plans.Add(plan);
                    }
                    dr.Close();
                }
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return plans;
        }

        public List<PlanMandayModel> GetEngPlans(string engId)
        {
            List<PlanMandayModel> plans = new List<PlanMandayModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                SELECT 
	                PlanManday.No,
	                PlanManday.Job_Milestone_ID,
	                Jobs.Job_ID,
	                Jobs.Job_Name,
	                Quotation.Customer,
	                Milestones.Milestone_ID,
	                Milestones.Milestone_Name,
	                PlanManday.User_ID,
	                EngineerUsers.Display_Name,
	                EngineerUsers.Department,
	                PlanManday.Date,
	                PlanManday.Hours
                FROM PlanManday
                LEFT JOIN JobMilestone ON PlanManday.Job_Milestone_ID = JobMilestone.Job_Milestone_ID
                LEFT JOIN Jobs ON JobMilestone.Job_ID = Jobs.Job_ID
                LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                LEFT JOIN Milestones ON JobMilestone.Milestone_ID = Milestones.Milestone_ID
                LEFT JOIN EngineerUsers ON PlanManday.User_ID = EngineerUsers.User_ID
                WHERE PlanManday.User_ID = '{engId}'");
                SqlCommand command = new SqlCommand(string_command, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PlanMandayModel plan = new PlanMandayModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            job_milestone_id = dr["Job_Milestone_ID"] != DBNull.Value ? dr["Job_Milestone_ID"].ToString() : "",
                            job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                            job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                            customer = dr["Customer"] != DBNull.Value ? dr["Customer"].ToString() : "",
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : "",
                            user_id = dr["User_ID"] != DBNull.Value ? dr["User_ID"].ToString() : "",
                            user_name = dr["Display_Name"] != DBNull.Value ? dr["Display_Name"].ToString() : "",
                            department = dr["Department"] != DBNull.Value ? dr["Department"].ToString() : "",
                            date = dr["Date"] != DBNull.Value ? Convert.ToDateTime(dr["Date"]) : default(DateTime),
                            hours = dr["Hours"] != DBNull.Value ? float.Parse(dr["Hours"].ToString()) : 0
                        };
                        plans.Add(plan);
                    }
                    dr.Close();
                }
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return plans;
        }

        public List<PlanMandayModel> GetEngPlansByDate(string engId, DateTime date)
        {
            List<PlanMandayModel> plans = new List<PlanMandayModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                SELECT 
	                PlanManday.No,
	                PlanManday.Job_Milestone_ID,
	                Jobs.Job_ID,
	                Jobs.Job_Name,
	                Quotation.Customer,
	                Milestones.Milestone_ID,
	                Milestones.Milestone_Name,
	                PlanManday.User_ID,
	                EngineerUsers.Display_Name,
	                EngineerUsers.Department,
	                PlanManday.Date,
	                PlanManday.Hours
                FROM PlanManday
                LEFT JOIN JobMilestone ON PlanManday.Job_Milestone_ID = JobMilestone.Job_Milestone_ID
                LEFT JOIN Jobs ON JobMilestone.Job_ID = Jobs.Job_ID
                LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                LEFT JOIN Milestones ON JobMilestone.Milestone_ID = Milestones.Milestone_ID
                LEFT JOIN EngineerUsers ON PlanManday.User_ID = EngineerUsers.User_ID
                WHERE PlanManday.User_ID = '{engId}' AND PlanManday.Date LIKE '{date.ToString("yyyy-MM-dd")}'");
                SqlCommand command = new SqlCommand(string_command, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PlanMandayModel plan = new PlanMandayModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            job_milestone_id = dr["Job_Milestone_ID"] != DBNull.Value ? dr["Job_Milestone_ID"].ToString() : "",
                            job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                            job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                            customer = dr["Customer"] != DBNull.Value ? dr["Customer"].ToString() : "",
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : "",
                            user_id = dr["User_ID"] != DBNull.Value ? dr["User_ID"].ToString() : "",
                            user_name = dr["Display_Name"] != DBNull.Value ? dr["Display_Name"].ToString() : "",
                            department = dr["Department"] != DBNull.Value ? dr["Department"].ToString() : "",
                            date = dr["Date"] != DBNull.Value ? Convert.ToDateTime(dr["Date"]) : default(DateTime),
                            hours = dr["Hours"] != DBNull.Value ? float.Parse(dr["Hours"].ToString()) : 0
                        };
                        plans.Add(plan);
                    }
                    dr.Close();
                }
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return plans;
        }

        public List<PlanMandayModel> GetPlansBetweenDates(DateTime startDate, DateTime stopDate)
        {
            List<PlanMandayModel> plans = new List<PlanMandayModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                SELECT 
	                PlanManday.No,
	                PlanManday.Job_Milestone_ID,
	                Jobs.Job_ID,
	                Jobs.Job_Name,
	                Quotation.Customer,
	                Milestones.Milestone_ID,
	                Milestones.Milestone_Name,
	                PlanManday.User_ID,
	                EngineerUsers.Display_Name,
	                EngineerUsers.Department,
	                PlanManday.Date,
	                PlanManday.Hours
                FROM PlanManday
                LEFT JOIN JobMilestone ON PlanManday.Job_Milestone_ID = JobMilestone.Job_Milestone_ID
                LEFT JOIN Jobs ON JobMilestone.Job_ID = Jobs.Job_ID
                LEFT JOIN Quotation ON Jobs.quotation_no = Quotation.quotation_no
                LEFT JOIN Milestones ON JobMilestone.Milestone_ID = Milestones.Milestone_ID
                LEFT JOIN EngineerUsers ON PlanManday.User_ID = EngineerUsers.User_ID
                WHERE PlanManday.Date BETWEEN '{startDate.ToString("yyyy-MM-dd")}' AND '{stopDate.ToString("yyyy-MM-dd")}'");
                SqlCommand command = new SqlCommand(string_command, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PlanMandayModel plan = new PlanMandayModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            job_milestone_id = dr["Job_Milestone_ID"] != DBNull.Value ? dr["Job_Milestone_ID"].ToString() : "",
                            job_id = dr["Job_ID"] != DBNull.Value ? dr["Job_ID"].ToString() : "",
                            job_name = dr["Job_Name"] != DBNull.Value ? dr["Job_Name"].ToString() : "",
                            customer = dr["Customer"] != DBNull.Value ? dr["Customer"].ToString() : "",
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : "",
                            user_id = dr["User_ID"] != DBNull.Value ? dr["User_ID"].ToString() : "",
                            user_name = dr["Display_Name"] != DBNull.Value ? dr["Display_Name"].ToString() : "",
                            department = dr["Department"] != DBNull.Value ? dr["Department"].ToString() : "",
                            date = dr["Date"] != DBNull.Value ? Convert.ToDateTime(dr["Date"]) : default(DateTime),
                            hours = dr["Hours"] != DBNull.Value ? float.Parse(dr["Hours"].ToString()) : 0
                        };
                        plans.Add(plan);
                    }
                    dr.Close();
                }
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return plans;
        }

        public string CreatePlan(PlanMandayModel plan)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                IF EXISTS(
	                SELECT Job_Milestone_ID, User_ID, Date, Hours FROM PlanManday
	                WHERE Job_Milestone_ID = @Job_Milestone_ID AND User_ID = @User_ID AND Date = @Date
                )
                BEGIN
	                UPDATE PlanManday SET Hours = Hours + @Hours
	                WHERE Job_Milestone_ID = @Job_Milestone_ID AND User_ID = @User_ID AND Date = @Date
                END
                ELSE
                BEGIN
	                INSERT INTO PlanManday ( Job_Milestone_ID, Job_ID, Milestone_ID, User_ID, Date, Hours )
                    VALUES ( @Job_Milestone_ID, @Job_ID, @Milestone_ID, @User_ID, @Date, @Hours )
                END");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Job_Milestone_ID", plan.job_milestone_id);
                command.Parameters.AddWithValue("@Job_ID", plan.job_id);
                command.Parameters.AddWithValue("@Milestone_ID", plan.milestone_id);
                command.Parameters.AddWithValue("@User_ID", plan.user_id);
                command.Parameters.AddWithValue("@Date", plan.date);
                command.Parameters.AddWithValue("@Hours", plan.hours);
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

        public string EditPlan(PlanMandayModel plan)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                UPDATE PlanManday SET 
                    Date = @Date,
                    Hours = @Hours
                WHERE No = @No");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Date", plan.date);
                command.Parameters.AddWithValue("@Hours", plan.hours);
                command.Parameters.AddWithValue("@No", plan.no);
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

        public string DeletePlan(PlanMandayModel plan)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            connection.Open();
            try
            {
                string string_command = string.Format($@"
                DELETE FROM PlanManday 
                WHERE Job_Milestone_ID = @Job_Milestone_ID 
                AND User_ID = @User_ID 
                AND Date = @Date");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Job_Milestone_ID", plan.job_milestone_id);
                command.Parameters.AddWithValue("@User_ID", plan.user_id);
                command.Parameters.AddWithValue("@Date", plan.date);
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
