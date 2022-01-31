using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebForecastReport.Interface;
using WebForecastReport.Models;

namespace WebForecastReport.Service
{
    public class ExportService : IExport
    {
        public Stream ExportQuotation(FileInfo path, string role, string name, string department)
        {
            Stream stream = new MemoryStream();
            List<QuotationModel> quotations = new List<QuotationModel>();
            if (path.Exists)
            {
                string connectString = "";
                if (role == "Admin")
                {
                    connectString = "select * from Quotation order by quotation_no";
                }
                else if (role != "Admin" && role != "")
                {
                    connectString = "select * from Quotation where department='" + department + "' order by sale_name";
                }
                else
                {
                    connectString = "select * from Quotation where sale_name='" + name + "' order by quotation_no";
                }

                SqlCommand cmd = new SqlCommand(connectString, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        QuotationModel q = new QuotationModel()
                        {
                            quotation_no = dr["quotation_no"].ToString(),
                            revision = dr["revision"].ToString(),
                            date = dr["date"].ToString() != "" ? Convert.ToDateTime(dr["date"].ToString()).ToString("yyyy-MM-dd") : "",
                            customer = dr["customer"].ToString(),
                            enduser = dr["enduser"].ToString(),
                            project_name = dr["project_name"].ToString(),
                            site_location = dr["site_location"].ToString(),
                            product_type = dr["product_type"].ToString(),
                            type = dr["type"].ToString(),
                            part_no = dr["part_no"].ToString(),
                            spec = dr["spec"].ToString(),
                            quantity = dr["quantity"].ToString(),
                            supplier_quotation_no = dr["supplier_quotation_no"].ToString(),
                            total_value = dr["total_value"].ToString(),
                            unit = dr["unit"].ToString(),
                            quoted_price = dr["quoted_price"].ToString(),
                            expected_order_date = dr["expected_order_date"].ToString() != "" ? Convert.ToDateTime(dr["expected_order_date"].ToString()).ToString("yyyy-MM-dd") : "",
                            required_onsite_date = dr["required_onsite_date"].ToString() != "" ? Convert.ToDateTime(dr["required_onsite_date"].ToString()).ToString("yyyy-MM-dd") : "",
                            proposer = dr["proposer"].ToString(),
                            expected_date = dr["expected_date"].ToString() != "" ? Convert.ToDateTime(dr["expected_date"].ToString()).ToString("yyyy-MM-dd") : "",
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

                        int count = q.type.Count(x => x == '|');
                        if (count > 0)
                        {
                            for (int i = 0; i <= count; i++)
                            {
                                if (i == 0)
                                {
                                    QuotationModel qu = new QuotationModel()
                                    {
                                        quotation_no = q.quotation_no,
                                        revision = q.revision,
                                        date = q.date,
                                        customer = q.customer,
                                        enduser = q.enduser,
                                        project_name = q.project_name,
                                        site_location = q.site_location,
                                        product_type = q.product_type,
                                        type = q.type.Split("|")[i],
                                        part_no = q.part_no.Split("|")[i],
                                        spec = q.spec.Split("|")[i],
                                        quantity = q.quantity.Split("|")[i],
                                        supplier_quotation_no = q.supplier_quotation_no.Split("|")[i],
                                        total_value = q.total_value.Split("|")[i],
                                        unit = q.unit.Split("|")[i],
                                        quoted_price = q.quoted_price,
                                        expected_order_date = q.expected_order_date,
                                        required_onsite_date = q.required_onsite_date,
                                        proposer = q.proposer,
                                        expected_date = q.expected_date,
                                        status = q.status,
                                        stages = q.stages,
                                        how_to_support = q.how_to_support,
                                        competitor = q.competitor,
                                        competitor_description = q.competitor_description,
                                        competitor_price = q.competitor_price,
                                        sale_name = q.sale_name,
                                        department = q.department,
                                        detail = q.detail
                                    };
                                    quotations.Add(qu);
                                }
                                else
                                {
                                    QuotationModel qu = new QuotationModel()
                                    {
                                        type = q.type.Split("|")[i],
                                        part_no = q.part_no.Split("|")[i],
                                        spec = q.spec.Split("|")[i],
                                        quantity = q.quantity.Split("|")[i],
                                        supplier_quotation_no = q.supplier_quotation_no.Split("|")[i],
                                        total_value = q.total_value.Split("|")[i],
                                        unit = q.unit.Split("|")[i],
                                    };
                                    quotations.Add(qu);
                                }
                            }
                        }
                        else
                        {
                            quotations.Add(q);
                        }
                    }
                    dr.Close();
                }

                using (ExcelPackage p = new ExcelPackage(path))
                {
                    ExcelWorksheet worksheet = p.Workbook.Worksheets["Quotations"];

                    int startRows = 2;
                    for (int i = 0; i < quotations.Count; i++)
                    {
                        worksheet.Cells["A" + (i + startRows)].Value = quotations[i].quotation_no != null ? quotations[i].quotation_no.ToString() : "";
                        worksheet.Cells["B" + (i + startRows)].Value = quotations[i].revision != null ? quotations[i].revision.ToString() : "";
                        worksheet.Cells["C" + (i + startRows)].Value = quotations[i].date != null ? quotations[i].date.ToString() : "";
                        worksheet.Cells["D" + (i + startRows)].Value = quotations[i].customer != null ? quotations[i].customer.ToString() : "";
                        worksheet.Cells["E" + (i + startRows)].Value = quotations[i].enduser != null ? quotations[i].enduser.ToString() : "";
                        worksheet.Cells["F" + (i + startRows)].Value = quotations[i].project_name != null ? quotations[i].project_name.ToString() : "";
                        worksheet.Cells["G" + (i + startRows)].Value = quotations[i].site_location != null ? quotations[i].site_location.ToString() : "";
                        worksheet.Cells["H" + (i + startRows)].Value = quotations[i].product_type != null ? quotations[i].product_type.ToString() : "";
                        worksheet.Cells["I" + (i + startRows)].Value = quotations[i].type != null ? quotations[i].type.ToString() : "";
                        worksheet.Cells["J" + (i + startRows)].Value = quotations[i].part_no != null ? quotations[i].part_no.ToString() : "";
                        worksheet.Cells["K" + (i + startRows)].Value = quotations[i].spec != null ? quotations[i].spec.ToString() : "";
                        worksheet.Cells["L" + (i + startRows)].Value = quotations[i].quantity != null ? quotations[i].quantity.ToString() : "";
                        worksheet.Cells["M" + (i + startRows)].Value = quotations[i].supplier_quotation_no != null ? quotations[i].supplier_quotation_no.ToString() : "";
                        worksheet.Cells["N" + (i + startRows)].Value = quotations[i].total_value != null ? quotations[i].total_value.ToString() : "";
                        worksheet.Cells["O" + (i + startRows)].Value = quotations[i].unit != null ? quotations[i].unit.ToString() : "";
                        worksheet.Cells["P" + (i + startRows)].Value = quotations[i].quoted_price != null ? quotations[i].quoted_price.ToString() : "";
                        worksheet.Cells["Q" + (i + startRows)].Value = quotations[i].expected_order_date != null ? quotations[i].expected_order_date.ToString() : "";
                        worksheet.Cells["R" + (i + startRows)].Value = quotations[i].required_onsite_date != null ? quotations[i].required_onsite_date.ToString() : "";
                        worksheet.Cells["S" + (i + startRows)].Value = quotations[i].proposer != null ? quotations[i].proposer.ToString() : "";
                        worksheet.Cells["T" + (i + startRows)].Value = quotations[i].expected_date != null ? quotations[i].expected_date.ToString() : "";
                        worksheet.Cells["U" + (i + startRows)].Value = quotations[i].status != null ? quotations[i].status.ToString() : "";
                        worksheet.Cells["V" + (i + startRows)].Value = quotations[i].stages != null ? quotations[i].stages.ToString() : "";
                        worksheet.Cells["W" + (i + startRows)].Value = quotations[i].how_to_support != null ? quotations[i].how_to_support.ToString() : "";
                        worksheet.Cells["X" + (i + startRows)].Value = quotations[i].competitor != null ? quotations[i].competitor.ToString() : "";
                        worksheet.Cells["Y" + (i + startRows)].Value = quotations[i].competitor_description != null ? quotations[i].competitor_description.ToString() : "";
                        worksheet.Cells["Z" + (i + startRows)].Value = quotations[i].competitor_price != null ? quotations[i].competitor_price.ToString() : "";
                        worksheet.Cells["AA" + (i + startRows)].Value = quotations[i].sale_name != null ? quotations[i].sale_name.ToString() : "";
                        worksheet.Cells["AB" + (i + startRows)].Value = quotations[i].department != null ? quotations[i].department.ToString() : "";
                        worksheet.Cells["AC" + (i + startRows)].Value = quotations[i].detail != null ? quotations[i].detail.ToString() : "";
                    }
                    p.SaveAs(stream);
                    stream.Position = 0;
                }
            }
            return stream;
        }
    }
}
