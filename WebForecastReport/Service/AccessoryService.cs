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
    public class AccessoryService : IAccessory
    {
        public List<UserModel> getAllUser()
        {
            try
            {
                List<UserModel> users = new List<UserModel>();
                SqlCommand cmd = new SqlCommand(@"select DISTINCT t1.Login,t1.Department2,t1.[Group],t1.Name,case when t2.Role ='Admin' then 'Admin' else t2.Role end as Role from [gps_sale_tracking].[dbo].[Sale_User] as t1
                                                  left join[MES].[dbo].[User] as t2 ON t1.Login = t2.Name order by t1.Login", ConnectSQL.Open_db_gps_Connect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        UserModel u = new UserModel()
                        {
                            name = dr["Login"].ToString(),
                            fullname = dr["Name"].ToString(),
                            department = dr["Department2"].ToString(),
                            group = dr["Group"].ToString(),
                            role = dr["Role"].ToString()
                        };
                        users.Add(u);
                    }
                    dr.Close();
                }
                return users;
            }
            finally
            {
                if (ConnectSQL.con_db_gps.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.Close_db_gps_Connect();
                }
            }
        }

        public List<CustomerModel> getCustomers()
        {
            try
            {
                List<CustomerModel> customers = new List<CustomerModel>();
                SqlCommand cmd = new SqlCommand("select DISTINCT Name from Customer order by Name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        CustomerModel u = new CustomerModel()
                        {
                            name = dr["Name"].ToString(),
                        };
                        customers.Add(u);
                    }
                    dr.Close();
                }
                return customers;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<DepartmentModel> getDepartment()
        {
            try
            {
                List<DepartmentModel> departments = new List<DepartmentModel>();
                SqlCommand cmd = new SqlCommand(@"select distinct Department2 from Sale_User where Department2 is not null", ConnectSQL.Open_db_gps_Connect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        DepartmentModel d = new DepartmentModel()
                        {
                            department = dr["Department2"].ToString(),
                        };
                        departments.Add(d);
                    }
                    dr.Close();
                }
                return departments;
            }
            finally
            {
                if (ConnectSQL.con_db_gps.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.Close_db_gps_Connect();
                }
            }
        }

        public List<string> GetDepartmentOfQuotation()
        {
            try
            {
                List<string> departments = new List<string>();
                SqlCommand cmd = new SqlCommand("select distinct department from Quotation order by department", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        departments.Add(dr["department"].ToString());
                    }
                    dr.Close();
                }
                return departments;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<DepartmentModel> getDepartmentQuotation()
        {
            try
            {
                List<DepartmentModel> departments = new List<DepartmentModel>();
                SqlCommand cmd = new SqlCommand("select Distinct department from Quotation order by department", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        DepartmentModel d = new DepartmentModel()
                        {
                            department = dr["department"].ToString()
                        };
                        departments.Add(d);
                    }
                    dr.Close();
                }
                return departments;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<EndUserModel> getEndUsers()
        {
            try
            {
                List<EndUserModel> endUsers = new List<EndUserModel>();
                SqlCommand cmd = new SqlCommand("select DISTINCT Name from EndUser order by Name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        EndUserModel u = new EndUserModel()
                        {
                            name = dr["Name"].ToString(),
                        };
                        endUsers.Add(u);
                    }
                    dr.Close();
                }
                return endUsers;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public List<SaleModel> getSale()
        {
            try
            {
                List<SaleModel> sales = new List<SaleModel>();
                SqlCommand cmd = new SqlCommand("select DISTINCT Login,Department2 from Sale_User where [Group] ='Sale' order by Login", ConnectSQL.Open_db_gps_Connect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SaleModel s = new SaleModel()
                        {
                            name = dr["Login"].ToString(),
                            department = dr["Department2"].ToString()
                        };
                        sales.Add(s);
                    }
                    dr.Close();
                }
                return sales;
            }
            finally
            {
                if (ConnectSQL.con_db_gps.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.Close_db_gps_Connect();
                }
            }
        }

        public List<SaleModel> getUserQuotation()
        {
            try
            {
                List<SaleModel> sales = new List<SaleModel>();
                SqlCommand cmd = new SqlCommand("select Distinct sale_name,department from Quotation order by sale_name", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SaleModel s = new SaleModel()
                        {
                            name = dr["sale_name"].ToString(),
                            department = dr["department"].ToString()
                        };
                        sales.Add(s);
                    }
                    dr.Close();
                }
                return sales;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public string InsertCustomer(string customer)
        {
            try
            {
                bool b = false;
                SqlCommand cmd = new SqlCommand("select Name from Customer where Name='" + customer + "'", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    b = true;
                    dr.Close();
                }
                if (b)
                {
                    using (SqlCommand cmd1 = new SqlCommand("INSERT INTO Customer(Name) VALUES (@Name)", ConnectSQL.OpenConnect()))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = ConnectSQL.OpenConnect();
                        cmd1.Parameters.AddWithValue("@Name", customer);

                        cmd1.ExecuteNonQuery();


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

        public string InsertEndUser(string enduser)
        {
            try
            {
                bool b = false;
                SqlCommand cmd = new SqlCommand("select Name from EndUser where Name='" + enduser + "'", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    b = true;
                    dr.Close();
                }
                if (b)
                {
                    using (SqlCommand cmd1 = new SqlCommand("INSERT INTO EndUser(Name) VALUES (@Name)", ConnectSQL.OpenConnect()))
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.Connection = ConnectSQL.OpenConnect();
                        cmd1.Parameters.AddWithValue("@Name", enduser);

                        cmd1.ExecuteNonQuery();
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
    }
}
