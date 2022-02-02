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
    public class ServiceService : IService
    {
        public string Delete(string name, string type_brand)
        {
            try
            {
                string command = "";
                if (type_brand == "Type")
                {
                    command = "DELETE FROM type_service WHERE name='" + name + "'";
                }
                else
                {
                    command = "DELETE FROM Service WHERE name='" + name + "'";
                }
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

        public List<ServiceModel> GetService(string type_brand)
        {
            try
            {
                string command = "";
                if (type_brand == "Type")
                {
                    command = "select * from type_service order by name";
                }
                else
                {
                    command = "select * from Service order by name";
                }
                List<ServiceModel> services = new List<ServiceModel>();
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ServiceModel s = new ServiceModel()
                        {
                            id = Int32.Parse(dr["Id"].ToString()),
                            name = dr["name"].ToString()
                        };
                        services.Add(s);
                    }
                    dr.Close();
                }
                return services;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<ServiceModel> GetServiceBrand()
        {
            try
            {
                List<ServiceModel> services = new List<ServiceModel>();
                SqlCommand cmd = new SqlCommand("select * from Service order by name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ServiceModel p = new ServiceModel()
                        {
                            id = Int32.Parse(dr["Id"].ToString()),
                            name = dr["name"].ToString()
                        };
                        services.Add(p);
                    }
                    dr.Close();
                }
                return services;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<ServiceModel> GetServiceType()
        {
            try
            {
                List<ServiceModel> services = new List<ServiceModel>();
                SqlCommand cmd = new SqlCommand("select * from type_service order by name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ServiceModel p = new ServiceModel()
                        {
                            id = Int32.Parse(dr["Id"].ToString()),
                            name = dr["name"].ToString()
                        };
                        services.Add(p);
                    }
                    dr.Close();
                }
                return services;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public string Insert(string name, string type_brand)
        {
            try
            {
                bool b = false;
                string commandchk = "";
                string command = "";
                if (type_brand == "Type")
                {
                    commandchk = "select * from type_service where name = '" + name + "'";
                    command = @"INSERT INTO type_service(name) VALUES (@name)";
                }
                else
                {
                    commandchk = "select * from Service where name = '" + name + "'";
                    command = @"INSERT INTO Service(name) VALUES (@name)";
                }
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
                        cmd.Parameters.AddWithValue("@name", name);

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

        public string Update(int id, string name, string type_brand)
        {
            try
            {
                string command = "";
                if (type_brand == "Type")
                {
                    command = @"UPDATE type_service SET name='" + name + "'" +
                                                                      "WHERE Id='" + id + "'";
                }
                else
                {
                    command = @"UPDATE Service SET name='" + name + "'" +
                                                                      "WHERE Id='" + id + "'";
                }
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
