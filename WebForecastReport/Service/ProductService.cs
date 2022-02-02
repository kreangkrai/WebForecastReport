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
    public class ProductService : IProduct
    {
        public string Delete(string name, string type_brand)
        {
            try
            {
                string command = "";
                if (type_brand == "Type")
                {
                    command = "DELETE FROM type_product WHERE name='" + name + "'";
                }
                else
                {
                    command = "DELETE FROM Product WHERE name='" + name + "'";
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

        public List<TypeModel> GetProductBrand()
        {
            try
            {
                List<TypeModel> products = new List<TypeModel>();
                SqlCommand cmd = new SqlCommand("select * from Product order by name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TypeModel p = new TypeModel()
                        {
                            id = Int32.Parse(dr["Id"].ToString()),
                            name = dr["name"].ToString()
                        };
                        products.Add(p);
                    }
                    dr.Close();
                }
                return products;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<TypeModel> GetProducts(string type_brand)
        {
            try
            {
                string command = "";
                if (type_brand == "Type")
                {
                    command = "select * from type_product order by name";
                }
                else
                {
                    command = "select * from Product order by name";
                }
                List<TypeModel> products = new List<TypeModel>();
                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TypeModel p = new TypeModel()
                        {
                            id = Int32.Parse(dr["Id"].ToString()),
                            name = dr["name"].ToString()
                        };
                        products.Add(p);
                    }
                    dr.Close();
                }
                return products;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<TypeModel> GetProductType()
        {
            try
            {
                List<TypeModel> products = new List<TypeModel>();
                SqlCommand cmd = new SqlCommand("select * from type_product order by name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TypeModel p = new TypeModel()
                        {
                            id = Int32.Parse(dr["Id"].ToString()),
                            name = dr["name"].ToString()
                        };
                        products.Add(p);
                    }
                    dr.Close();
                }
                return products;
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
                    commandchk = "select* from type_product where name = '" + name + "'";
                    command = @"INSERT INTO type_product(name) VALUES (@name)";
                }
                else
                {
                    commandchk = "select* from Product where name = '" + name + "'";
                    command = @"INSERT INTO Product(name) VALUES (@name)";
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
                    command = @"UPDATE type_product SET name = '" + name + "'" +
                                                                      "WHERE Id='" + id + "'";
                }
                else
                {
                    command = @"UPDATE Product SET name = '" + name + "'" +
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
