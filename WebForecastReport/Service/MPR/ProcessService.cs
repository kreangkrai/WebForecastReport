using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface.MPR;
using WebForecastReport.Models.MPR;

namespace WebForecastReport.Service.MPR
{
    public class ProcessService : IProcess
    {
        public List<EngProcessModel> GetProcesses()
        {
            List<EngProcessModel> processes = new List<EngProcessModel>();
            try
            {
                string string_command = string.Format($@"SELECT [No], Process_ID, Process_Name, Process_Description FROM Eng_Process");
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
                        EngProcessModel process = new EngProcessModel()
                        {
                            no = dr["No"] != DBNull.Value ? Convert.ToInt32(dr["No"]) : 0,
                            process_id = dr["Process_ID"] != DBNull.Value ? dr["Process_ID"].ToString() : "",
                            process_name = dr["Process_Name"] != DBNull.Value ? dr["Process_Name"].ToString() : "",
                            process_description = dr["Process_Description"] != DBNull.Value ? dr["Process_Description"].ToString() : "",
                        };
                        processes.Add(process);
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
            return processes;
        }

        public int GetLastProcessID()
        {
            int id = 0;
            try
            {
                string string_command = string.Format($@"SELECT TOP 1 Process_ID FROM Eng_Process ORDER BY Process_ID DESC");
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
                        id = dr["Process_ID"] != DBNull.Value ? Convert.ToInt32(dr["Process_ID"].ToString().Substring(3)) : 0;
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
            return id;
        }

        public string CreateProcess(EngProcessModel process)
        {
            try
            {
                string string_command = string.Format($@"
                    INSERT INTO Eng_Process(Process_ID, Process_Name, Process_Description)
                    VALUES(@Process_ID, @Process_Name, @Process_Description)");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Process_ID", process.process_id);
                    cmd.Parameters.AddWithValue("@Process_Name", process.process_name);
                    cmd.Parameters.AddWithValue("@Process_Description", process.process_description);
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

        public string EditProcess(EngProcessModel process)
        {
            try
            {
                string string_command = string.Format($@"
                    UPDATE Eng_Process 
                    SET
                        Process_Name = @Process_Name,
                        Process_Description = @Process_Description
                    WHERE Process_ID = @Process_ID");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Process_Name", process.process_name);
                    cmd.Parameters.AddWithValue("@Process_Description", process.process_description);
                    cmd.Parameters.AddWithValue("@Process_ID", process.process_id);
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

        public string DeleteProcess(EngProcessModel process)
        {
            try
            {
                string string_command = string.Format($@"DELETE FROM Eng_Process WHERE Process_ID = @Process_ID");
                using (SqlCommand cmd = new SqlCommand(string_command, ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Process_ID", process.process_id);
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
