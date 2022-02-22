using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;

namespace WebForecastReport.Service
{
    public class TargetService : ITarget
    {
        public string Delete(string year, string name)
        {
            try
            {
                string command = "";

                command = "DELETE FROM Target WHERE year='" + year + "' and sale_name='" + name + "'";

                SqlCommand com = new SqlCommand(command, ConnectSQL.OpenConnect());
                com.ExecuteNonQuery();
                return "Delete Success";
            }
            catch
            {
                return "Delete Failed";
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<TargetModel> getData()
        {
            try
            {
                List<TargetModel> targets = new List<TargetModel>();
                SqlCommand cmd = new SqlCommand("select * from Target order by department,sale_name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TargetModel p = new TargetModel()
                        {
                            year = dr["year"].ToString(),
                            department = dr["department"].ToString(),
                            sale_name = dr["sale_name"].ToString(),
                            product = dr["product"].ToString(),
                            project = dr["project"].ToString(),
                            service = dr["service"].ToString()
                        };
                        targets.Add(p);
                    }
                    dr.Close();
                }
                return targets;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public string Insert(string year, string department, string name)
        {
            try
            {
                bool b = false;
                string commandchk = "";
                string command = "";

                commandchk = "select * from Target where year = '" + year + "' and sale_name = '" + name + "'";
                command = @"INSERT INTO Target(year,department,sale_name,product,project,service) VALUES (@year,@department,@sale_name,@product,@project,@service)";

                SqlCommand cmd1 = new SqlCommand(commandchk, ConnectSQL.OpenConnect());
                SqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.HasRows)
                {
                    b = true;
                }
                if (!b)
                {
                    using (SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect()))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = ConnectSQL.OpenConnect();
                        cmd.Parameters.AddWithValue("@year", year);
                        cmd.Parameters.AddWithValue("@department", department);
                        cmd.Parameters.AddWithValue("@sale_name", name);
                        cmd.Parameters.AddWithValue("@product", "0");
                        cmd.Parameters.AddWithValue("@project", "0");
                        cmd.Parameters.AddWithValue("@service", "0");
                        cmd.ExecuteNonQuery();
                    }
                }
                return "Insert Success";
            }
            catch
            {
                return "Insert Failed";
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }
        public string Update(TargetModel model)
        {
            try
            {
                string command = "";

                command = @"UPDATE Target SET product = '" + model.product + "'," +
                                             "project = '" + model.project + "'," +
                                             "service = '" + model.service + "' " +
                                             "WHERE year='" + model.year + "' and sale_name='" + model.sale_name + "'";

                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(command);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = ConnectSQL.OpenConnect();
                reader = cmd.ExecuteReader();
                reader.Close();

                return "Update Success";
            }
            catch
            {
                return "Update Failed";
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
