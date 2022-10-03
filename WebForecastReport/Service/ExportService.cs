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
        readonly IQuotation_Report Quotation_Report;
        readonly ILogStatus LogStatus;
        readonly ILogStages LogStages;
        public ExportService()
        {
            Quotation_Report = new Quotation_ReportService();
            LogStatus = new LogStatusService();
            LogStages = new LogStagesService();
        }

        public Stream ExportLogStages(FileInfo path, string year)
        {
            Stream stream = new MemoryStream();
            List<Log_StagesModel> reports = new List<Log_StagesModel>();
            if (path.Exists)
            {
                reports = LogStages.GetStagesByYear(year);
                using (ExcelPackage p = new ExcelPackage(path))
                {
                    ExcelWorksheet worksheet = p.Workbook.Worksheets["Sheet1"];

                    int startRows = 2;
                    for (int i = 0; i < reports.Count; i++)
                    {
                        worksheet.Cells["A" + (i + startRows)].Value = reports[i].quotation != null ? reports[i].quotation.ToString() : "";
                        worksheet.Cells["B" + (i + startRows)].Value = reports[i].project_name != null ? reports[i].project_name.ToString() : "";
                        worksheet.Cells["C" + (i + startRows)].Value = reports[i].date_edit != null ? reports[i].date_edit.ToString() : "";
                        worksheet.Cells["D" + (i + startRows)].Value = reports[i].stages_from != null ? reports[i].stages_from.ToString() : "";
                        worksheet.Cells["E" + (i + startRows)].Value = reports[i].stages_to != null ? reports[i].stages_to.ToString() : "";
                        worksheet.Cells["F" + (i + startRows)].Value = reports[i].reason != null ? reports[i].reason.ToString() : "";
                        worksheet.Cells["G" + (i + startRows)].Value = reports[i].name != null ? reports[i].name.ToString() : "";
                    }
                    p.SaveAs(stream);
                    stream.Position = 0;
                }
            }
            return stream;
        }

        public Stream ExportLogStatus(FileInfo path, string year)
        {
            Stream stream = new MemoryStream();
            List<Log_StatusModel> reports = new List<Log_StatusModel>();
            if (path.Exists)
            {
                reports = LogStatus.GetStatusByYear(year);
                using (ExcelPackage p = new ExcelPackage(path))
                {
                    ExcelWorksheet worksheet = p.Workbook.Worksheets["Sheet1"];

                    int startRows = 2;
                    for (int i = 0; i < reports.Count; i++)
                    {
                        worksheet.Cells["A" + (i + startRows)].Value = reports[i].quotation != null ? reports[i].quotation.ToString() : "";
                        worksheet.Cells["B" + (i + startRows)].Value = reports[i].project_name != null ? reports[i].project_name.ToString() : "";
                        worksheet.Cells["C" + (i + startRows)].Value = reports[i].date_edit != null ? reports[i].date_edit.ToString() : "";
                        worksheet.Cells["D" + (i + startRows)].Value = reports[i].status_from != null ? reports[i].status_from.ToString() : "";
                        worksheet.Cells["E" + (i + startRows)].Value = reports[i].status_to != null ? reports[i].status_to.ToString() : "";
                        worksheet.Cells["F" + (i + startRows)].Value = reports[i].reason != null ? reports[i].reason.ToString() : "";
                        worksheet.Cells["G" + (i + startRows)].Value = reports[i].name != null ? reports[i].name.ToString() : "";
                    }
                    p.SaveAs(stream);
                    stream.Position = 0;
                }
            }
            return stream;
        }

        public Stream ExportQuotation(FileInfo path, string role, string name, string department)
        {
            Stream stream = new MemoryStream();
            List<QuotationModel> quotations = new List<QuotationModel>();
            if (path.Exists)
            {
                string connectString = "";
                if (role == "Admin")
                {
                    connectString = "select * from Quotation where exclude_quote <>'True' order by quotation_no";
                }
                else if (role != "Admin" && role != "")
                {
                    connectString = "select * from Quotation where department='" + role + "' or sale_name='" + name + "' and exclude_quote <>'True' order by sale_name";
                }
                else
                {
                    connectString = "select * from Quotation where sale_name='" + name + "' and exclude_quote <>'True' order by quotation_no";
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
                            date = dr["date"] != DBNull.Value ? Convert.ToDateTime(dr["date"].ToString()).ToString("yyyy-MM-dd") : "",
                            customer = dr["customer"].ToString(),
                            enduser = dr["enduser"].ToString(),
                            project_name = dr["project_name"].ToString(),
                            site_location = dr["site_location"].ToString(),
                            product_type = dr["product_type"].ToString(),
                            type = dr["type"].ToString(),
                            brand = dr["brand"].ToString(),
                            part_no = dr["part_no"].ToString(),
                            spec = dr["spec"].ToString(),
                            quantity = dr["quantity"].ToString(),
                            supplier_quotation_no = dr["supplier_quotation_no"].ToString(),
                            total_value = dr["total_value"].ToString(),
                            unit = dr["unit"].ToString(),
                            quoted_price = dr["quoted_price"].ToString(),
                            expected_order_date = dr["expected_order_date"] != DBNull.Value ? Convert.ToDateTime(dr["expected_order_date"].ToString()).ToString("yyyy-MM-dd") : "",
                            required_onsite_date = dr["required_onsite_date"] != DBNull.Value ? Convert.ToDateTime(dr["required_onsite_date"].ToString()).ToString("yyyy-MM-dd") : "",
                            proposer = dr["proposer"].ToString(),
                            expected_date = dr["expected_date"] != DBNull.Value ? Convert.ToDateTime(dr["expected_date"].ToString()).ToString("yyyy-MM-dd") : "",
                            status = dr["status"].ToString(),
                            stages = dr["stages"].ToString(),
                            stages_update_date = dr["stages_update_date"] != DBNull.Value ? Convert.ToDateTime(dr["stages_update_date"].ToString()).ToString("yyyy-MM-dd") : "",
                            how_to_support = dr["how_to_support"].ToString(),
                            competitor = dr["competitor"].ToString(),
                            competitor_description = dr["competitor_description"].ToString(),
                            competitor_price = dr["competitor_price"].ToString(),
                            sale_name = dr["sale_name"].ToString(),
                            department = dr["department"].ToString(),
                            detail = dr["detail"].ToString(),
                            exclude_quote = dr["exclude_quote"] != DBNull.Value ? Convert.ToBoolean(dr["exclude_quote"].ToString()):false
                        };

                        int count = q.brand.Count(x => x == '|');
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
                                        brand = q.brand.Split("|")[i],
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
                                        stages_update_date = q.stages_update_date,
                                        how_to_support = q.how_to_support,
                                        competitor = q.competitor,
                                        competitor_description = q.competitor_description,
                                        competitor_price = q.competitor_price,
                                        sale_name = q.sale_name,
                                        department = q.department,
                                        detail = q.detail,
                                        exclude_quote = q.exclude_quote
                                    };
                                    quotations.Add(qu);
                                }
                                else
                                {
                                    QuotationModel qu = new QuotationModel()
                                    {
                                        type = q.type.Split("|")[i],
                                        brand = q.brand.Split("|")[i],
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
                        worksheet.Cells["J" + (i + startRows)].Value = quotations[i].brand != null ? quotations[i].brand.ToString() : "";
                        worksheet.Cells["K" + (i + startRows)].Value = quotations[i].part_no != null ? quotations[i].part_no.ToString() : "";
                        worksheet.Cells["L" + (i + startRows)].Value = quotations[i].spec != null ? quotations[i].spec.ToString() : "";
                        worksheet.Cells["M" + (i + startRows)].Value = quotations[i].quantity != null ? quotations[i].quantity.ToString() : "";
                        worksheet.Cells["N" + (i + startRows)].Value = quotations[i].supplier_quotation_no != null ? quotations[i].supplier_quotation_no.ToString() : "";
                        worksheet.Cells["O" + (i + startRows)].Value = quotations[i].total_value != null ? quotations[i].total_value.ToString() : "";
                        worksheet.Cells["P" + (i + startRows)].Value = quotations[i].unit != null ? quotations[i].unit.ToString() : "";
                        worksheet.Cells["Q" + (i + startRows)].Value = quotations[i].quoted_price != null ? quotations[i].quoted_price.ToString() : "";
                        worksheet.Cells["R" + (i + startRows)].Value = quotations[i].expected_order_date != null ? quotations[i].expected_order_date.ToString() : "";
                        worksheet.Cells["S" + (i + startRows)].Value = quotations[i].required_onsite_date != null ? quotations[i].required_onsite_date.ToString() : "";
                        worksheet.Cells["T" + (i + startRows)].Value = quotations[i].proposer != null ? quotations[i].proposer.ToString() : "";
                        worksheet.Cells["U" + (i + startRows)].Value = quotations[i].expected_date != null ? quotations[i].expected_date.ToString() : "";
                        worksheet.Cells["V" + (i + startRows)].Value = quotations[i].status != null ? quotations[i].status.ToString() : "";
                        worksheet.Cells["W" + (i + startRows)].Value = quotations[i].stages != null ? quotations[i].stages.ToString() : "";
                        worksheet.Cells["X" + (i + startRows)].Value = quotations[i].stages_update_date != null ? quotations[i].stages_update_date.ToString() : "";
                        worksheet.Cells["Y" + (i + startRows)].Value = quotations[i].how_to_support != null ? quotations[i].how_to_support.ToString() : "";
                        worksheet.Cells["Z" + (i + startRows)].Value = quotations[i].competitor != null ? quotations[i].competitor.ToString() : "";
                        worksheet.Cells["AA" + (i + startRows)].Value = quotations[i].competitor_description != null ? quotations[i].competitor_description.ToString() : "";
                        worksheet.Cells["AB" + (i + startRows)].Value = quotations[i].competitor_price != null ? quotations[i].competitor_price.ToString() : "";
                        worksheet.Cells["AC" + (i + startRows)].Value = quotations[i].sale_name != null ? quotations[i].sale_name.ToString() : "";
                        worksheet.Cells["AD" + (i + startRows)].Value = quotations[i].department != null ? quotations[i].department.ToString() : "";
                        worksheet.Cells["AE" + (i + startRows)].Value = quotations[i].detail != null ? quotations[i].detail.ToString() : "";
                        worksheet.Cells["AF" + (i + startRows)].Value = quotations[i].exclude_quote.ToString();
                    }
                    p.SaveAs(stream);
                    stream.Position = 0;
                }
            }
            return stream;
        }

        public Stream ExportQuotation_Report_Department(FileInfo path, string department, string month_first, string month_last)
        {
            Stream stream = new MemoryStream();
            List<Quotation_Report_DepartmentModel> reports = new List<Quotation_Report_DepartmentModel>();
            if (path.Exists)
            {

                reports = Quotation_Report.GetReportDepartment(department, month_first, month_last);
                using (ExcelPackage p = new ExcelPackage(path))
                {
                    ExcelWorksheet worksheet = p.Workbook.Worksheets["Report"];

                    int startRows = 6;
                    for (int i = 0; i < reports.Count; i++)
                    {
                        worksheet.Cells["A" + (i + startRows)].Value = reports[i].department != null ? reports[i].department.ToString() : "";
                        worksheet.Cells["B" + (i + startRows)].Value = reports[i].sale != null ? reports[i].sale.ToString() : "";
                        worksheet.Cells["C" + (i + startRows)].Value = reports[i].quo_cnt != null ? reports[i].quo_cnt.ToString() : "";
                        worksheet.Cells["D" + (i + startRows)].Value = reports[i].quo_mb != null ? reports[i].quo_mb.ToString() : "";
                        worksheet.Cells["E" + (i + startRows)].Value = reports[i].project_won_cnt != null ? reports[i].project_won_cnt.ToString() : "";
                        worksheet.Cells["F" + (i + startRows)].Value = reports[i].project_won_mb != null ? reports[i].project_won_mb.ToString() : "";
                        worksheet.Cells["G" + (i + startRows)].Value = reports[i].project_lost_cnt != null ? reports[i].project_lost_cnt.ToString() : "";
                        worksheet.Cells["H" + (i + startRows)].Value = reports[i].project_lost_mb != null ? reports[i].project_lost_mb.ToString() : "";
                        worksheet.Cells["I" + (i + startRows)].Value = reports[i].project_nogo_cnt != null ? reports[i].project_nogo_cnt.ToString() : "";
                        worksheet.Cells["J" + (i + startRows)].Value = reports[i].project_nogo_mb != null ? reports[i].project_nogo_mb.ToString() : "";
                        worksheet.Cells["K" + (i + startRows)].Value = reports[i].project_pending_cnt != null ? reports[i].project_pending_cnt.ToString() : "";
                        worksheet.Cells["L" + (i + startRows)].Value = reports[i].project_pending_mb != null ? reports[i].project_pending_mb.ToString() : "";
                        worksheet.Cells["M" + (i + startRows)].Value = reports[i].project_quote_cnt != null ? reports[i].project_quote_cnt.ToString() : "";
                        worksheet.Cells["N" + (i + startRows)].Value = reports[i].project_quote_mb != null ? reports[i].project_quote_mb.ToString() : "";
                        worksheet.Cells["O" + (i + startRows)].Value = reports[i].project_cnt != null ? reports[i].project_cnt.ToString() : "";
                        worksheet.Cells["P" + (i + startRows)].Value = reports[i].project_mb != null ? reports[i].project_mb.ToString() : "";
                        worksheet.Cells["Q" + (i + startRows)].Value = reports[i].product_won_cnt != null ? reports[i].product_won_cnt.ToString() : "";
                        worksheet.Cells["R" + (i + startRows)].Value = reports[i].product_won_mb != null ? reports[i].product_won_mb.ToString() : "";
                        worksheet.Cells["S" + (i + startRows)].Value = reports[i].product_lost_cnt != null ? reports[i].product_lost_cnt.ToString() : "";
                        worksheet.Cells["T" + (i + startRows)].Value = reports[i].product_lost_mb != null ? reports[i].product_lost_mb.ToString() : "";
                        worksheet.Cells["U" + (i + startRows)].Value = reports[i].product_nogo_cnt != null ? reports[i].product_nogo_cnt.ToString() : "";
                        worksheet.Cells["V" + (i + startRows)].Value = reports[i].product_nogo_mb != null ? reports[i].product_nogo_mb.ToString() : "";
                        worksheet.Cells["W" + (i + startRows)].Value = reports[i].product_pending_cnt != null ? reports[i].product_pending_cnt.ToString() : "";
                        worksheet.Cells["X" + (i + startRows)].Value = reports[i].product_pending_mb != null ? reports[i].product_pending_mb.ToString() : "";
                        worksheet.Cells["Y" + (i + startRows)].Value = reports[i].product_quote_cnt != null ? reports[i].product_quote_cnt.ToString() : "";
                        worksheet.Cells["Z" + (i + startRows)].Value = reports[i].product_quote_mb != null ? reports[i].product_quote_mb.ToString() : "";
                        worksheet.Cells["AA" + (i + startRows)].Value = reports[i].product_cnt != null ? reports[i].product_cnt.ToString() : "";
                        worksheet.Cells["AB" + (i + startRows)].Value = reports[i].product_mb != null ? reports[i].product_mb.ToString() : "";
                        worksheet.Cells["AC" + (i + startRows)].Value = reports[i].service_won_cnt != null ? reports[i].service_won_cnt.ToString() : "";
                        worksheet.Cells["AD" + (i + startRows)].Value = reports[i].service_won_mb != null ? reports[i].service_won_mb.ToString() : "";
                        worksheet.Cells["AE" + (i + startRows)].Value = reports[i].service_lost_cnt != null ? reports[i].service_lost_cnt.ToString() : "";
                        worksheet.Cells["AF" + (i + startRows)].Value = reports[i].service_lost_mb != null ? reports[i].service_lost_mb.ToString() : "";
                        worksheet.Cells["AG" + (i + startRows)].Value = reports[i].service_nogo_cnt != null ? reports[i].service_nogo_cnt.ToString() : "";
                        worksheet.Cells["AH" + (i + startRows)].Value = reports[i].service_nogo_mb != null ? reports[i].service_nogo_mb.ToString() : "";
                        worksheet.Cells["AI" + (i + startRows)].Value = reports[i].service_pending_cnt != null ? reports[i].service_pending_cnt.ToString() : "";
                        worksheet.Cells["AJ" + (i + startRows)].Value = reports[i].service_pending_mb != null ? reports[i].service_pending_mb.ToString() : "";
                        worksheet.Cells["AK" + (i + startRows)].Value = reports[i].service_quote_cnt != null ? reports[i].service_quote_cnt.ToString() : "";
                        worksheet.Cells["AL" + (i + startRows)].Value = reports[i].service_quote_mb != null ? reports[i].service_quote_mb.ToString() : "";
                        worksheet.Cells["AM" + (i + startRows)].Value = reports[i].service_cnt != null ? reports[i].service_cnt.ToString() : "";
                        worksheet.Cells["AN" + (i + startRows)].Value = reports[i].service_mb != null ? reports[i].service_mb.ToString() : "";
                        worksheet.Cells["AO" + (i + startRows)].Value = reports[i].won_project_cnt != null ? reports[i].won_project_cnt.ToString() : "";
                        worksheet.Cells["AP" + (i + startRows)].Value = reports[i].won_project_mb != null ? reports[i].won_project_mb.ToString() : "";
                        worksheet.Cells["AQ" + (i + startRows)].Value = reports[i].won_product_cnt != null ? reports[i].won_product_cnt.ToString() : "";
                        worksheet.Cells["AR" + (i + startRows)].Value = reports[i].won_product_mb != null ? reports[i].won_product_mb.ToString() : "";
                        worksheet.Cells["AS" + (i + startRows)].Value = reports[i].won_service_cnt != null ? reports[i].won_service_cnt.ToString() : "";
                        worksheet.Cells["AT" + (i + startRows)].Value = reports[i].won_service_mb != null ? reports[i].won_service_mb.ToString() : "";
                        worksheet.Cells["AU" + (i + startRows)].Value = reports[i].won_quo_cnt != null ? reports[i].won_quo_cnt.ToString() : "";
                        worksheet.Cells["AV" + (i + startRows)].Value = reports[i].won_mb != null ? reports[i].won_mb.ToString() : "";
                        worksheet.Cells["AW" + (i + startRows)].Value = reports[i].lost_project_cnt != null ? reports[i].lost_project_cnt.ToString() : "";
                        worksheet.Cells["AX" + (i + startRows)].Value = reports[i].lost_project_mb != null ? reports[i].lost_project_mb.ToString() : "";
                        worksheet.Cells["AY" + (i + startRows)].Value = reports[i].lost_product_cnt != null ? reports[i].lost_product_cnt.ToString() : "";
                        worksheet.Cells["AZ" + (i + startRows)].Value = reports[i].lost_product_mb != null ? reports[i].lost_product_mb.ToString() : "";
                        worksheet.Cells["BA" + (i + startRows)].Value = reports[i].lost_service_cnt != null ? reports[i].lost_service_cnt.ToString() : "";
                        worksheet.Cells["BB" + (i + startRows)].Value = reports[i].lost_service_mb != null ? reports[i].lost_service_mb.ToString() : "";
                        worksheet.Cells["BC" + (i + startRows)].Value = reports[i].loss_quo_cnt != null ? reports[i].loss_quo_cnt.ToString() : "";
                        worksheet.Cells["BD" + (i + startRows)].Value = reports[i].loss_mb != null ? reports[i].loss_mb.ToString() : "";
                        worksheet.Cells["BE" + (i + startRows)].Value = reports[i].nogo_project_cnt != null ? reports[i].nogo_project_cnt.ToString() : "";
                        worksheet.Cells["BF" + (i + startRows)].Value = reports[i].nogo_project_mb != null ? reports[i].nogo_project_mb.ToString() : "";
                        worksheet.Cells["BG" + (i + startRows)].Value = reports[i].nogo_product_cnt != null ? reports[i].nogo_product_cnt.ToString() : "";
                        worksheet.Cells["BH" + (i + startRows)].Value = reports[i].nogo_product_mb != null ? reports[i].nogo_product_mb.ToString() : "";
                        worksheet.Cells["BI" + (i + startRows)].Value = reports[i].nogo_service_cnt != null ? reports[i].nogo_service_cnt.ToString() : "";
                        worksheet.Cells["BJ" + (i + startRows)].Value = reports[i].nogo_service_mb != null ? reports[i].nogo_service_mb.ToString() : "";
                        worksheet.Cells["BK" + (i + startRows)].Value = reports[i].nogo_quo_cnt != null ? reports[i].nogo_quo_cnt.ToString() : "";
                        worksheet.Cells["BL" + (i + startRows)].Value = reports[i].nogo_mb != null ? reports[i].nogo_mb.ToString() : "";
                        worksheet.Cells["BM" + (i + startRows)].Value = reports[i].pending_project_cnt != null ? reports[i].pending_project_cnt.ToString() : "";
                        worksheet.Cells["BN" + (i + startRows)].Value = reports[i].pending_project_mb != null ? reports[i].pending_project_mb.ToString() : "";
                        worksheet.Cells["BO" + (i + startRows)].Value = reports[i].pending_product_cnt != null ? reports[i].pending_product_cnt.ToString() : "";
                        worksheet.Cells["BP" + (i + startRows)].Value = reports[i].pending_product_mb != null ? reports[i].pending_product_mb.ToString() : "";
                        worksheet.Cells["BQ" + (i + startRows)].Value = reports[i].pending_service_cnt != null ? reports[i].pending_service_cnt.ToString() : "";
                        worksheet.Cells["BR" + (i + startRows)].Value = reports[i].pending_service_mb != null ? reports[i].pending_service_mb.ToString() : "";
                        worksheet.Cells["BS" + (i + startRows)].Value = reports[i].pending_quo_cnt != null ? reports[i].pending_quo_cnt.ToString() : "";
                        worksheet.Cells["BT" + (i + startRows)].Value = reports[i].pending_mb != null ? reports[i].pending_mb.ToString() : "";
                        worksheet.Cells["BU" + (i + startRows)].Value = reports[i].quote_project_cnt != null ? reports[i].quote_project_cnt.ToString() : "";
                        worksheet.Cells["BV" + (i + startRows)].Value = reports[i].quote_project_mb != null ? reports[i].quote_project_mb.ToString() : "";
                        worksheet.Cells["BW" + (i + startRows)].Value = reports[i].quote_product_cnt != null ? reports[i].quote_product_cnt.ToString() : "";
                        worksheet.Cells["BX" + (i + startRows)].Value = reports[i].quote_product_mb != null ? reports[i].quote_product_mb.ToString() : "";
                        worksheet.Cells["BY" + (i + startRows)].Value = reports[i].quote_service_cnt != null ? reports[i].quote_service_cnt.ToString() : "";
                        worksheet.Cells["BZ" + (i + startRows)].Value = reports[i].quote_service_mb != null ? reports[i].quote_service_mb.ToString() : "";
                        worksheet.Cells["CA" + (i + startRows)].Value = reports[i].quote_quo_cnt != null ? reports[i].quote_quo_cnt.ToString() : "";
                        worksheet.Cells["CB" + (i + startRows)].Value = reports[i].quote_mb != null ? reports[i].quote_mb.ToString() : "";
                    }
                    worksheet.Cells["B1"].Value = month_first + "-" + month_last;
                    p.SaveAs(stream);
                    stream.Position = 0;
                }
            }
            return stream;
        }

        public Stream ExportQuotation_Report_PendingInOut(FileInfo path, string department, string month_first, string month_last)
        {
            Stream stream = new MemoryStream();
            List<Quotation_Report_PendingInOutModel> reports = new List<Quotation_Report_PendingInOutModel>();
            if (path.Exists)
            {     
                using (ExcelPackage p = new ExcelPackage(path))
                {
                    reports = Quotation_Report.GetReportPendingInOutByDepSale(department, month_first, month_last);
                    ExcelWorksheet worksheet = p.Workbook.Worksheets["Sale"];

                    int startRows = 3;
                    for (int i = 0; i < reports.Count; i++)
                    {
                        worksheet.Cells["A" + (i + startRows)].Value = reports[i].department != null ? reports[i].department.ToString() : "";
                        worksheet.Cells["B" + (i + startRows)].Value = reports[i].sale_name != null ? reports[i].sale_name.ToString() : "";
                        worksheet.Cells["C" + (i + startRows)].Value = reports[i].product_type != null ? reports[i].product_type.ToString() : "";
                        worksheet.Cells["D" + (i + startRows)].Value = reports[i].type != null ? reports[i].type.ToString() : "";
                        worksheet.Cells["E" + (i + startRows)].Value = reports[i].brand != null ? reports[i].brand.ToString() : "";
                        worksheet.Cells["F" + (i + startRows)].Value = reports[i].pending_in != null ? double.Parse(reports[i].pending_in.ToString()) : 0.00;
                        worksheet.Cells["G" + (i + startRows)].Value = reports[i].pending_out != null ? double.Parse(reports[i].pending_out.ToString()) : 0.00;
                        worksheet.Cells["H" + (i + startRows)].Value = reports[i].pending != null ? double.Parse(reports[i].pending.ToString()) : 0.00;
                    }
                    worksheet.Cells["B1"].Value = month_first + "-" + month_last;
                    worksheet.Cells["D1"].Value = department;

                    reports = Quotation_Report.GetReportPendingInOutByDepartment(department, month_first, month_last);
                    worksheet = p.Workbook.Worksheets["Department"];

                    startRows = 3;
                    for (int i = 0; i < reports.Count; i++)
                    {
                        worksheet.Cells["A" + (i + startRows)].Value = reports[i].department != null ? reports[i].department.ToString() : "";
                        worksheet.Cells["B" + (i + startRows)].Value = reports[i].product_type != null ? reports[i].product_type.ToString() : "";
                        worksheet.Cells["C" + (i + startRows)].Value = reports[i].type != null ? reports[i].type.ToString() : "";
                        worksheet.Cells["D" + (i + startRows)].Value = reports[i].brand != null ? reports[i].brand.ToString() : "";
                        worksheet.Cells["E" + (i + startRows)].Value = reports[i].pending_in != null ? double.Parse(reports[i].pending_in.ToString()) : 0.00;
                        worksheet.Cells["F" + (i + startRows)].Value = reports[i].pending_out != null ? double.Parse(reports[i].pending_out.ToString()) : 0.00;
                        worksheet.Cells["G" + (i + startRows)].Value = reports[i].pending != null ? double.Parse(reports[i].pending.ToString()) : 0.00;
                    }
                    worksheet.Cells["B1"].Value = month_first + "-" + month_last;
                    worksheet.Cells["D1"].Value = department;

                    p.SaveAs(stream);
                    stream.Position = 0;
                }               
            }
            return stream;
        }

        public Stream ExportQuotation_Report_Quarter(FileInfo path, string department, string year,string stages)
        {
            Stream stream = new MemoryStream();
            List<Quotation_Report_QuarterModel> reports = new List<Quotation_Report_QuarterModel>();
            if (path.Exists)
            {
                reports = Quotation_Report.GetReportQuarter(department, year, stages);
                using (ExcelPackage p = new ExcelPackage(path))
                {
                    ExcelWorksheet worksheet = p.Workbook.Worksheets["Report"];

                    int startRows = 5;
                    for (int i = 0; i < reports.Count; i++)
                    {
                        worksheet.Cells["A" + (i + startRows)].Value = reports[i].department != null ? reports[i].department.ToString() : "";
                        worksheet.Cells["B" + (i + startRows)].Value = reports[i].sale != null ? reports[i].sale.ToString() : "";
                        worksheet.Cells["C" + (i + startRows)].Value = reports[i].jan_in != null ? reports[i].jan_in.ToString() : "";
                        worksheet.Cells["D" + (i + startRows)].Value = reports[i].jan_out != null ? reports[i].jan_out.ToString() : "";
                        worksheet.Cells["E" + (i + startRows)].Value = reports[i].feb_in != null ? reports[i].feb_in.ToString() : "";
                        worksheet.Cells["F" + (i + startRows)].Value = reports[i].feb_out != null ? reports[i].feb_out.ToString() : "";
                        worksheet.Cells["G" + (i + startRows)].Value = reports[i].mar_in != null ? reports[i].mar_in.ToString() : "";
                        worksheet.Cells["H" + (i + startRows)].Value = reports[i].mar_out != null ? reports[i].mar_out.ToString() : "";
                        worksheet.Cells["I" + (i + startRows)].Value = reports[i].sum_in_q1 != null ? reports[i].sum_in_q1.ToString() : "";
                        worksheet.Cells["J" + (i + startRows)].Value = reports[i].sum_out_q1 != null ? reports[i].sum_out_q1.ToString() : "";
                        worksheet.Cells["K" + (i + startRows)].Value = reports[i].apr_in != null ? reports[i].apr_in.ToString() : "";
                        worksheet.Cells["L" + (i + startRows)].Value = reports[i].apr_out != null ? reports[i].apr_out.ToString() : "";
                        worksheet.Cells["M" + (i + startRows)].Value = reports[i].may_in != null ? reports[i].may_in.ToString() : "";
                        worksheet.Cells["N" + (i + startRows)].Value = reports[i].may_out != null ? reports[i].may_out.ToString() : "";
                        worksheet.Cells["O" + (i + startRows)].Value = reports[i].jun_in != null ? reports[i].jun_in.ToString() : "";
                        worksheet.Cells["P" + (i + startRows)].Value = reports[i].jun_out != null ? reports[i].jun_out.ToString() : "";
                        worksheet.Cells["Q" + (i + startRows)].Value = reports[i].sum_in_q2 != null ? reports[i].sum_in_q2.ToString() : "";
                        worksheet.Cells["R" + (i + startRows)].Value = reports[i].sum_out_q2 != null ? reports[i].sum_out_q2.ToString() : "";
                        worksheet.Cells["S" + (i + startRows)].Value = reports[i].jul_in != null ? reports[i].jul_in.ToString() : "";
                        worksheet.Cells["T" + (i + startRows)].Value = reports[i].jul_out != null ? reports[i].jul_out.ToString() : "";
                        worksheet.Cells["U" + (i + startRows)].Value = reports[i].aug_in != null ? reports[i].aug_in.ToString() : "";
                        worksheet.Cells["V" + (i + startRows)].Value = reports[i].aug_out != null ? reports[i].aug_out.ToString() : "";
                        worksheet.Cells["W" + (i + startRows)].Value = reports[i].sep_in != null ? reports[i].sep_in.ToString() : "";
                        worksheet.Cells["X" + (i + startRows)].Value = reports[i].sep_out != null ? reports[i].sep_out.ToString() : "";
                        worksheet.Cells["Y" + (i + startRows)].Value = reports[i].sum_in_q3 != null ? reports[i].sum_in_q3.ToString() : "";
                        worksheet.Cells["Z" + (i + startRows)].Value = reports[i].sum_out_q3 != null ? reports[i].sum_out_q3.ToString() : "";
                        worksheet.Cells["AA" + (i + startRows)].Value = reports[i].oct_in != null ? reports[i].oct_in.ToString() : "";
                        worksheet.Cells["AB" + (i + startRows)].Value = reports[i].oct_out != null ? reports[i].oct_out.ToString() : "";
                        worksheet.Cells["AC" + (i + startRows)].Value = reports[i].nov_in != null ? reports[i].nov_in.ToString() : "";
                        worksheet.Cells["AD" + (i + startRows)].Value = reports[i].nov_out != null ? reports[i].nov_out.ToString() : "";
                        worksheet.Cells["AE" + (i + startRows)].Value = reports[i].dec_in != null ? reports[i].dec_in.ToString() : "";
                        worksheet.Cells["AF" + (i + startRows)].Value = reports[i].dec_out != null ? reports[i].dec_out.ToString() : "";
                        worksheet.Cells["AG" + (i + startRows)].Value = reports[i].sum_in_q4 != null ? reports[i].sum_in_q4.ToString() : "";
                        worksheet.Cells["AH" + (i + startRows)].Value = reports[i].sum_out_q4 != null ? reports[i].sum_out_q4.ToString() : "";
                        worksheet.Cells["AI" + (i + startRows)].Value = reports[i].sum_in != null ? reports[i].sum_in.ToString() : "";
                        worksheet.Cells["AJ" + (i + startRows)].Value = reports[i].sum_out != null ? reports[i].sum_out.ToString() : "";
                    }
                    worksheet.Cells["B1"].Value = year;
                    worksheet.Cells["E1"].Value = stages;
                    p.SaveAs(stream);
                    stream.Position = 0;
                }
            }
            return stream;
        }

        public Stream ExportQuotation_Report_Year(FileInfo path, string department, string year)
        {
            Stream stream = new MemoryStream();
            List<Quotation_Report_YearModel> reports = new List<Quotation_Report_YearModel>();
            if (path.Exists)
            {

                reports = Quotation_Report.GetReportYear(department, year);
                using (ExcelPackage p = new ExcelPackage(path))
                {
                    ExcelWorksheet worksheet = p.Workbook.Worksheets["Report"];

                    int startRows = 6;
                    for (int i = 0; i < reports.Count; i++)
                    {
                        worksheet.Cells["A" + (i + startRows)].Value = reports[i].month != null ? reports[i].month.ToString() : "";
                        worksheet.Cells["B" + (i + startRows)].Value = reports[i].quo_cnt != null ? reports[i].quo_cnt.ToString() : "";
                        worksheet.Cells["C" + (i + startRows)].Value = reports[i].quo_mb != null ? reports[i].quo_mb.ToString() : "";
                        worksheet.Cells["D" + (i + startRows)].Value = reports[i].project_won_cnt != null ? reports[i].project_won_cnt.ToString() : "";
                        worksheet.Cells["E" + (i + startRows)].Value = reports[i].project_won_mb != null ? reports[i].project_won_mb.ToString() : "";
                        worksheet.Cells["F" + (i + startRows)].Value = reports[i].project_lost_cnt != null ? reports[i].project_lost_cnt.ToString() : "";
                        worksheet.Cells["G" + (i + startRows)].Value = reports[i].project_lost_mb != null ? reports[i].project_lost_mb.ToString() : "";
                        worksheet.Cells["H" + (i + startRows)].Value = reports[i].project_nogo_cnt != null ? reports[i].project_nogo_cnt.ToString() : "";
                        worksheet.Cells["I" + (i + startRows)].Value = reports[i].project_nogo_mb != null ? reports[i].project_nogo_mb.ToString() : "";
                        worksheet.Cells["J" + (i + startRows)].Value = reports[i].project_pending_cnt != null ? reports[i].project_pending_cnt.ToString() : "";
                        worksheet.Cells["K" + (i + startRows)].Value = reports[i].project_pending_mb != null ? reports[i].project_pending_mb.ToString() : "";
                        worksheet.Cells["L" + (i + startRows)].Value = reports[i].project_quote_cnt != null ? reports[i].project_quote_cnt.ToString() : "";
                        worksheet.Cells["M" + (i + startRows)].Value = reports[i].project_quote_mb != null ? reports[i].project_quote_mb.ToString() : "";
                        worksheet.Cells["N" + (i + startRows)].Value = reports[i].project_cnt != null ? reports[i].project_cnt.ToString() : "";
                        worksheet.Cells["O" + (i + startRows)].Value = reports[i].project_mb != null ? reports[i].project_mb.ToString() : "";
                        worksheet.Cells["P" + (i + startRows)].Value = reports[i].product_won_cnt != null ? reports[i].product_won_cnt.ToString() : "";
                        worksheet.Cells["Q" + (i + startRows)].Value = reports[i].product_won_mb != null ? reports[i].product_won_mb.ToString() : "";
                        worksheet.Cells["R" + (i + startRows)].Value = reports[i].product_lost_cnt != null ? reports[i].product_lost_cnt.ToString() : "";
                        worksheet.Cells["S" + (i + startRows)].Value = reports[i].product_lost_mb != null ? reports[i].product_lost_mb.ToString() : "";
                        worksheet.Cells["T" + (i + startRows)].Value = reports[i].product_nogo_cnt != null ? reports[i].product_nogo_cnt.ToString() : "";
                        worksheet.Cells["U" + (i + startRows)].Value = reports[i].product_nogo_mb != null ? reports[i].product_nogo_mb.ToString() : "";
                        worksheet.Cells["V" + (i + startRows)].Value = reports[i].product_pending_cnt != null ? reports[i].product_pending_cnt.ToString() : "";
                        worksheet.Cells["W" + (i + startRows)].Value = reports[i].product_pending_mb != null ? reports[i].product_pending_mb.ToString() : "";
                        worksheet.Cells["X" + (i + startRows)].Value = reports[i].product_quote_cnt != null ? reports[i].product_quote_cnt.ToString() : "";
                        worksheet.Cells["Y" + (i + startRows)].Value = reports[i].product_quote_mb != null ? reports[i].product_quote_mb.ToString() : "";
                        worksheet.Cells["Z" + (i + startRows)].Value = reports[i].product_cnt != null ? reports[i].product_cnt.ToString() : "";
                        worksheet.Cells["AA" + (i + startRows)].Value = reports[i].product_mb != null ? reports[i].product_mb.ToString() : "";
                        worksheet.Cells["AB" + (i + startRows)].Value = reports[i].service_won_cnt != null ? reports[i].service_won_cnt.ToString() : "";
                        worksheet.Cells["AC" + (i + startRows)].Value = reports[i].service_won_mb != null ? reports[i].service_won_mb.ToString() : "";
                        worksheet.Cells["AD" + (i + startRows)].Value = reports[i].service_lost_cnt != null ? reports[i].service_lost_cnt.ToString() : "";
                        worksheet.Cells["AE" + (i + startRows)].Value = reports[i].service_lost_mb != null ? reports[i].service_lost_mb.ToString() : "";
                        worksheet.Cells["AF" + (i + startRows)].Value = reports[i].service_nogo_cnt != null ? reports[i].service_nogo_cnt.ToString() : "";
                        worksheet.Cells["AG" + (i + startRows)].Value = reports[i].service_nogo_mb != null ? reports[i].service_nogo_mb.ToString() : "";
                        worksheet.Cells["AH" + (i + startRows)].Value = reports[i].service_pending_cnt != null ? reports[i].service_pending_cnt.ToString() : "";
                        worksheet.Cells["AI" + (i + startRows)].Value = reports[i].service_pending_mb != null ? reports[i].service_pending_mb.ToString() : "";
                        worksheet.Cells["AJ" + (i + startRows)].Value = reports[i].service_quote_cnt != null ? reports[i].service_quote_cnt.ToString() : "";
                        worksheet.Cells["AK" + (i + startRows)].Value = reports[i].service_quote_mb != null ? reports[i].service_quote_mb.ToString() : "";
                        worksheet.Cells["AL" + (i + startRows)].Value = reports[i].service_cnt != null ? reports[i].service_cnt.ToString() : "";
                        worksheet.Cells["AM" + (i + startRows)].Value = reports[i].service_mb != null ? reports[i].service_mb.ToString() : "";
                        worksheet.Cells["AN" + (i + startRows)].Value = reports[i].won_project_cnt != null ? reports[i].won_project_cnt.ToString() : "";
                        worksheet.Cells["AO" + (i + startRows)].Value = reports[i].won_project_mb != null ? reports[i].won_project_mb.ToString() : "";
                        worksheet.Cells["AP" + (i + startRows)].Value = reports[i].won_product_cnt != null ? reports[i].won_product_cnt.ToString() : "";
                        worksheet.Cells["AQ" + (i + startRows)].Value = reports[i].won_product_mb != null ? reports[i].won_product_mb.ToString() : "";
                        worksheet.Cells["AR" + (i + startRows)].Value = reports[i].won_service_cnt != null ? reports[i].won_service_cnt.ToString() : "";
                        worksheet.Cells["AS" + (i + startRows)].Value = reports[i].won_service_mb != null ? reports[i].won_service_mb.ToString() : "";
                        worksheet.Cells["AT" + (i + startRows)].Value = reports[i].won_quo_cnt != null ? reports[i].won_quo_cnt.ToString() : "";
                        worksheet.Cells["AU" + (i + startRows)].Value = reports[i].won_mb != null ? reports[i].won_mb.ToString() : "";
                        worksheet.Cells["AV" + (i + startRows)].Value = reports[i].lost_project_cnt != null ? reports[i].lost_project_cnt.ToString() : "";
                        worksheet.Cells["AW" + (i + startRows)].Value = reports[i].lost_project_mb != null ? reports[i].lost_project_mb.ToString() : "";
                        worksheet.Cells["AX" + (i + startRows)].Value = reports[i].lost_product_cnt != null ? reports[i].lost_product_cnt.ToString() : "";
                        worksheet.Cells["AY" + (i + startRows)].Value = reports[i].lost_product_mb != null ? reports[i].lost_product_mb.ToString() : "";
                        worksheet.Cells["AZ" + (i + startRows)].Value = reports[i].lost_service_cnt != null ? reports[i].lost_service_cnt.ToString() : "";
                        worksheet.Cells["BA" + (i + startRows)].Value = reports[i].lost_service_mb != null ? reports[i].lost_service_mb.ToString() : "";
                        worksheet.Cells["BB" + (i + startRows)].Value = reports[i].loss_quo_cnt != null ? reports[i].loss_quo_cnt.ToString() : "";
                        worksheet.Cells["BC" + (i + startRows)].Value = reports[i].loss_mb != null ? reports[i].loss_mb.ToString() : "";
                        worksheet.Cells["BD" + (i + startRows)].Value = reports[i].nogo_project_cnt != null ? reports[i].nogo_project_cnt.ToString() : "";
                        worksheet.Cells["BE" + (i + startRows)].Value = reports[i].nogo_project_mb != null ? reports[i].nogo_project_mb.ToString() : "";
                        worksheet.Cells["BF" + (i + startRows)].Value = reports[i].nogo_product_cnt != null ? reports[i].nogo_product_cnt.ToString() : "";
                        worksheet.Cells["BG" + (i + startRows)].Value = reports[i].nogo_product_mb != null ? reports[i].nogo_product_mb.ToString() : "";
                        worksheet.Cells["BH" + (i + startRows)].Value = reports[i].nogo_service_cnt != null ? reports[i].nogo_service_cnt.ToString() : "";
                        worksheet.Cells["BI" + (i + startRows)].Value = reports[i].nogo_service_mb != null ? reports[i].nogo_service_mb.ToString() : "";
                        worksheet.Cells["BJ" + (i + startRows)].Value = reports[i].nogo_quo_cnt != null ? reports[i].nogo_quo_cnt.ToString() : "";
                        worksheet.Cells["BK" + (i + startRows)].Value = reports[i].nogo_mb != null ? reports[i].nogo_mb.ToString() : "";
                        worksheet.Cells["BL" + (i + startRows)].Value = reports[i].pending_project_cnt != null ? reports[i].pending_project_cnt.ToString() : "";
                        worksheet.Cells["BM" + (i + startRows)].Value = reports[i].pending_project_mb != null ? reports[i].pending_project_mb.ToString() : "";
                        worksheet.Cells["BN" + (i + startRows)].Value = reports[i].pending_product_cnt != null ? reports[i].pending_product_cnt.ToString() : "";
                        worksheet.Cells["BO" + (i + startRows)].Value = reports[i].pending_product_mb != null ? reports[i].pending_product_mb.ToString() : "";
                        worksheet.Cells["BP" + (i + startRows)].Value = reports[i].pending_service_cnt != null ? reports[i].pending_service_cnt.ToString() : "";
                        worksheet.Cells["BQ" + (i + startRows)].Value = reports[i].pending_service_mb != null ? reports[i].pending_service_mb.ToString() : "";
                        worksheet.Cells["BR" + (i + startRows)].Value = reports[i].pending_quo_cnt != null ? reports[i].pending_quo_cnt.ToString() : "";
                        worksheet.Cells["BS" + (i + startRows)].Value = reports[i].pending_mb != null ? reports[i].pending_mb.ToString() : "";
                        worksheet.Cells["BT" + (i + startRows)].Value = reports[i].quote_project_cnt != null ? reports[i].quote_project_cnt.ToString() : "";
                        worksheet.Cells["BU" + (i + startRows)].Value = reports[i].quote_project_mb != null ? reports[i].quote_project_mb.ToString() : "";
                        worksheet.Cells["BV" + (i + startRows)].Value = reports[i].quote_product_cnt != null ? reports[i].quote_product_cnt.ToString() : "";
                        worksheet.Cells["BW" + (i + startRows)].Value = reports[i].quote_product_mb != null ? reports[i].quote_product_mb.ToString() : "";
                        worksheet.Cells["BX" + (i + startRows)].Value = reports[i].quote_service_cnt != null ? reports[i].quote_service_cnt.ToString() : "";
                        worksheet.Cells["BY" + (i + startRows)].Value = reports[i].quote_service_mb != null ? reports[i].quote_service_mb.ToString() : "";
                        worksheet.Cells["BZ" + (i + startRows)].Value = reports[i].quote_quo_cnt != null ? reports[i].quote_quo_cnt.ToString() : "";
                        worksheet.Cells["CA" + (i + startRows)].Value = reports[i].quote_mb != null ? reports[i].quote_mb.ToString() : "";
                    }
                    worksheet.Cells["B1"].Value = year;
                    worksheet.Cells["D1"].Value = department;
                    p.SaveAs(stream);
                    stream.Position = 0;
                }
            }
            return stream;
        }
    }
}
