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
    public class QuotationService : IQuotation
    {
        public string Delete(QuotationModel model)
        {
            try
            {
                string sql_user = "DELETE FROM Quotation WHERE quotation_no='" + model.quotation_no + "'";
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

        public List<QuotationModel> GetAll(string name)
        {
            try
            {
                List<QuotationModel> quotations = new List<QuotationModel>();
                SqlCommand cmd = new SqlCommand("select * from Quotation where sale_name='" + name + "' order by date desc", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        QuotationModel q = new QuotationModel()
                        {
                            quotation_no = dr["quotation_no"].ToString(),
                            revision = dr["revision"].ToString(),
                            date = Convert.ToDateTime(dr["date"].ToString()).ToString("yyyy-MM-dd"),
                            customer = dr["customer"].ToString(),
                            enduser = dr["enduser"].ToString(),
                            project_name = dr["project_name"].ToString(),
                            site_location = dr["site_location"].ToString(),
                            product_type = dr["product_type"].ToString(),
                            part_no = dr["part_no"].ToString(),
                            spec = dr["spec"].ToString(),
                            quantity = dr["quantity"].ToString(),
                            supplier_quotation_no = dr["supplier_quotation_no"].ToString(),
                            total_value = dr["total_value"].ToString(),
                            unit = dr["unit"].ToString(),
                            quoted_price = dr["quoted_price"].ToString(),
                            expected_order_date = Convert.ToDateTime(dr["expected_order_date"].ToString()).ToString("yyyy-MM-dd"),
                            required_onsite_date = Convert.ToDateTime(dr["required_onsite_date"].ToString()).ToString("yyyy-MM-dd"),
                            proposer = dr["proposer"].ToString(),
                            expected_date = dr["expected_date"].ToString() != "" ? Convert.ToDateTime(dr["expected_date"].ToString()).ToString("yyyy-MM-dd"): "1900-01-01",
                            status = dr["status"].ToString(),
                            stages = dr["stages"].ToString(),
                            how_to_support = dr["how_to_support"].ToString(),
                            competitor = dr["competitor"].ToString(),
                            competitor_description = dr["competitor_description"].ToString(),
                            competitor_price = dr["competitor_price"].ToString(),
                            sale_name = dr["sale_name"].ToString(),
                            department = dr["department"].ToString(),
                            detail = dr["detail"].ToString()
                        };
                        quotations.Add(q);
                    }
                    dr.Close();
                }
                return quotations;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public string GetlastQuotation()
        {
            try
            {
                string year = DateTime.Now.ToString("yy");
                string str = "";
                SqlCommand cmd = new SqlCommand("select top 1 case when quotation_no like 'Q" + year + "%' then quotation_no else 'Q" + year + "0000' end as  quotation_no from Quotation order by quotation_no desc", ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        str = dr["quotation_no"].ToString();
                    }
                    dr.Close();
                }
                return str;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public string Insert(QuotationModel model)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Quotation(
                                                                            quotation_no,
                                                                            revision,
                                                                            date,
                                                                            customer,
                                                                            enduser,
                                                                            project_name,
                                                                            site_location,
                                                                            product_type,
                                                                            part_no,
                                                                            spec,
                                                                            quantity,
                                                                            supplier_quotation_no,
                                                                            total_value,
                                                                            unit,
                                                                            quoted_price,
                                                                            expected_order_date,
                                                                            required_onsite_date,
                                                                            proposer,
                                                                            expected_date,
                                                                            status,
                                                                            stages,
                                                                            how_to_support,
                                                                            competitor,
                                                                            competitor_description,
                                                                            competitor_price,
                                                                            sale_name,
                                                                            department,
                                                                            detail) VALUES (
                                                                            @quotation_no,
                                                                            @revision,
                                                                            @date,
                                                                            @customer,
                                                                            @enduser,
                                                                            @project_name,
                                                                            @site_location,
                                                                            @product_type,
                                                                            @part_no,
                                                                            @spec,
                                                                            @quantity,
                                                                            @supplier_quotation_no,
                                                                            @total_value,
                                                                            @unit,
                                                                            @quoted_price,
                                                                            @expected_order_date,
                                                                            @required_onsite_date,
                                                                            @proposer,
                                                                            @expected_date,
                                                                            @status, 
                                                                            @stages, 
                                                                            @how_to_support, 
                                                                            @competitor, 
                                                                            @competitor_description, 
                                                                            @competitor_price, 
                                                                            @sale_name,
                                                                            @department,
                                                                            @detail)", ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = ConnectSQL.OpenConnect();
                    cmd.Parameters.AddWithValue("@quotation_no", model.quotation_no);
                    cmd.Parameters.AddWithValue("@revision", model.revision);
                    cmd.Parameters.AddWithValue("@date", model.date);
                    cmd.Parameters.AddWithValue("@customer", model.customer);
                    cmd.Parameters.AddWithValue("@enduser", model.enduser);
                    cmd.Parameters.AddWithValue("@project_name", model.project_name);
                    cmd.Parameters.AddWithValue("@site_location", model.site_location);
                    cmd.Parameters.AddWithValue("@product_type", model.product_type);
                    cmd.Parameters.AddWithValue("@part_no", model.part_no);
                    cmd.Parameters.AddWithValue("@spec", model.spec);
                    cmd.Parameters.AddWithValue("@quantity", model.quantity);
                    cmd.Parameters.AddWithValue("@supplier_quotation_no", model.supplier_quotation_no);
                    cmd.Parameters.AddWithValue("@total_value", model.total_value);
                    cmd.Parameters.AddWithValue("@unit", model.unit);
                    cmd.Parameters.AddWithValue("@quoted_price", model.quoted_price);
                    cmd.Parameters.AddWithValue("@expected_order_date", model.expected_order_date);
                    cmd.Parameters.AddWithValue("@required_onsite_date", model.required_onsite_date);
                    cmd.Parameters.AddWithValue("@proposer", model.proposer);
                    cmd.Parameters.AddWithValue("@expected_date", model.expected_date);
                    cmd.Parameters.AddWithValue("@status", model.status);
                    cmd.Parameters.AddWithValue("@stages", model.stages);
                    cmd.Parameters.AddWithValue("@how_to_support", model.how_to_support);
                    cmd.Parameters.AddWithValue("@competitor", model.competitor);
                    cmd.Parameters.AddWithValue("@competitor_description", model.competitor_description);
                    cmd.Parameters.AddWithValue("@competitor_price", model.competitor_price);
                    cmd.Parameters.AddWithValue("@sale_name", model.sale_name);
                    cmd.Parameters.AddWithValue("@department", model.department);
                    cmd.Parameters.AddWithValue("@detail", model.detail);
                    cmd.ExecuteNonQuery();

                    return "Insert Success";
                }
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

        public string InsertQuotation(QuotationModel model)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Quotation(
                                                                            quotation_no,revision,
                                                                            date,expected_order_date,required_onsite_date,sale_name,department) VALUES (
                                                                            @quotation_no,@revision,@date,@expected_order_date,@required_onsite_date,@sale_name,@department)", ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = ConnectSQL.OpenConnect();
                    cmd.Parameters.AddWithValue("@quotation_no", model.quotation_no);
                    cmd.Parameters.AddWithValue("@revision", model.revision);
                    cmd.Parameters.AddWithValue("@date", model.date);
                    //cmd.Parameters.AddWithValue("@proposer", model.proposer);
                    cmd.Parameters.AddWithValue("@expected_order_date", model.expected_order_date);
                    cmd.Parameters.AddWithValue("@required_onsite_date", model.required_onsite_date);
                    //cmd.Parameters.AddWithValue("@expected_date", null);
                    cmd.Parameters.AddWithValue("@sale_name", model.sale_name);
                    cmd.Parameters.AddWithValue("@department", model.department);
                    cmd.ExecuteNonQuery();

                    return "Insert Success";
                }
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

        public string Update(QuotationModel model)
        {
            try
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(@"UPDATE Quotation SET revision='"+model.revision+"'," +
                                                                      "date='" + model.date + "'," +
                                                                      "customer='" + model.customer + "'," +
                                                                      "enduser='" + model.enduser + "'," +
                                                                      "project_name='" + model.project_name + "'," +
                                                                      "site_location='" + model.site_location + "'," +
                                                                      "product_type='" + model.product_type + "'," +
                                                                      "part_no='" + model.part_no + "'," +
                                                                      "spec='" + model.spec + "'," +
                                                                      "quantity='" + model.quantity + "'," +
                                                                      "supplier_quotation_no='" + model.supplier_quotation_no + "'," +
                                                                      "total_value='" + model.total_value + "'," +
                                                                      "unit='" + model.unit + "'," +
                                                                      "quoted_price='" + model.quoted_price + "'," +
                                                                      "expected_order_date='" + model.expected_order_date + "'," +
                                                                      "required_onsite_date='" + model.required_onsite_date + "'," +
                                                                      "proposer='" + model.proposer + "'," +
                                                                      "expected_date='" + model.expected_date + "'," +
                                                                      "status='" + model.status + "'," +
                                                                      "stages='" + model.stages + "'," +
                                                                      "how_to_support='" + model.how_to_support + "'," +
                                                                      "competitor='" + model.competitor + "'," +
                                                                      "competitor_description='" + model.competitor_description + "'," +
                                                                      "competitor_price='" + model.competitor_price + "'," +
                                                                      "sale_name='" + model.sale_name + "'," +
                                                                      "department='" + model.department + "'," +
                                                                      "detail='" + model.detail + "'" +
                                                                      "WHERE quotation_no='" + model.quotation_no + "'");
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