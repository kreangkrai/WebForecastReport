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
        public string Delete(string name)
        {
            try
            {
                string sql_user = "DELETE FROM Product WHERE name='" + name + "'";
                SqlCommand com = new SqlCommand(sql_user, ConnectSQL.OpenConnect());
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

        public  List<TypeModel> GetProducts()
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

        public string Insert(string name)
        {
            try
            {
                bool b = false;
                SqlCommand cmd1 = new SqlCommand("select * from Product where name = '" + name + "'", ConnectSQL.OpenConnect());
                SqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.HasRows)
                {
                    b = true;
                }
                if (!b)
                {
                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Product(name) VALUES (@name)", ConnectSQL.OpenConnect()))
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

        public string Update(int id,string name)
        {
            try
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(@"UPDATE Product SET name='" + name + "'" +
                                                                      "WHERE Id='" + id + "'");
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
