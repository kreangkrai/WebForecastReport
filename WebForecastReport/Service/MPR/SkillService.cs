using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Service.MPR
{
    public class SkillService : ISkill
    {
        public List<EngSkillModel> GetSkills()
        {
            List<EngSkillModel> skills = new List<EngSkillModel>();
            try
            {
                string string_command = string.Format($@"SELECT [No], Skill_ID, Skill_Name, Skill_Description FROM Eng_Skill");
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
                        EngSkillModel skill = new EngSkillModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            skill_id = dr["Skill_ID"] != DBNull.Value ? dr["Skill_ID"].ToString() : "",
                            skill_name = dr["Skill_Name"] != DBNull.Value ? dr["Skill_Name"].ToString() : "",
                            skill_description = dr["Skill_Description"] != DBNull.Value ? dr["Skill_Description"].ToString() : "",
                        };
                        skills.Add(skill);
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
            return skills;
        }

        public string CreateSkill(EngSkillModel skill)
        {
            try
            {
                string string_command = string.Format($@"
                    INSERT INTO Eng_Skill(Skill_ID, Skill_Name, Skill_Description)
                    VALUES(@Skill_ID, @Skill_Name, @Skill_Description)");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Skill_ID", skill.skill_id);
                    cmd.Parameters.AddWithValue("@Skill_Name", skill.skill_name);
                    cmd.Parameters.AddWithValue("@Skill_Description", skill.skill_description);
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

        public string EditSkill(EngSkillModel skill)
        {
            try
            {
                string string_command = string.Format($@"
                    UPDATE Eng_Skill 
                    SET
                        Skill_Name = @Skill_Name,
                        Skill_Description = @Skill_Description
                    WHERE Skill_ID = @Skill_ID");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Skill_Name", skill.skill_name);
                    cmd.Parameters.AddWithValue("@Skill_Description", skill.skill_description);
                    cmd.Parameters.AddWithValue("@Skill_ID", skill.skill_id);
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

        public string DeleteSkill(EngSkillModel skill)
        {
            try
            {
                string string_command = string.Format($@"DELETE FROM Eng_Skill WHERE Skill_ID = @Skill_ID");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Skill_ID", skill.skill_id);
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
    }
}
