using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Service.MPR
{
    public class MilestoneService : IMilestone
    {
        public List<MilestoneModel> GetMilestones()
        {
            List<MilestoneModel> milestones = new List<MilestoneModel>();
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"SELECT [No], Milestone_ID, Milestone_Name FROM Milestones");
                SqlCommand command = new SqlCommand(string_command, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        MilestoneModel ms = new MilestoneModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["no"]) : 0,
                            milestone_id = dr["Milestone_ID"] != DBNull.Value ? dr["Milestone_ID"].ToString() : "",
                            milestone_name = dr["Milestone_Name"] != DBNull.Value ? dr["Milestone_Name"].ToString() : ""
                        };
                        milestones.Add(ms);
                    }
                    dr.Close();
                }
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return milestones;
        }

        public int GetLastMilestoneID()
        {
            int id = 0;
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"SELECT TOP 1 Milestone_ID FROM Milestones ORDER BY Milestone_ID DESC");
                SqlCommand command = new SqlCommand(string_command, connection);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        id = dr["Milestone_ID"] != DBNull.Value ? Convert.ToInt32(dr["Milestone_ID"].ToString().Substring(1)) : 0;
                    }
                    dr.Close();
                }
            }
            finally
            {
                ConnectSQL.CloseConnect();
            }
            return id;
        }

        public string CreateMilestone(MilestoneModel ms)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                INSERT INTO Milestones (
                    Milestone_ID,
                    Milestone_Name )
                VALUES (
                    @Milestone_ID,
                    @Milestone_Name
                )");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Milestone_ID", ms.milestone_id);
                command.Parameters.AddWithValue("@Milestone_Name", ms.milestone_name);
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

        public string EditMilestone(MilestoneModel ms)
        {
            SqlConnection connection = ConnectSQL.OpenConnect();
            try
            {
                string string_command = string.Format($@"
                UPDATE Milestones SET
                    Milestone_Name = @Milestone_Name,
                WHERE Milestone_ID = @Milestone_ID
                ");
                SqlCommand command = new SqlCommand(string_command, connection);
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@Milestone_ID", ms.milestone_id);
                command.Parameters.AddWithValue("@Milestone_Name", ms.milestone_name);
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
