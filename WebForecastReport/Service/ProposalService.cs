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
    public class ProposalService : IProposal
    {
        public List<string> chkQuotation(string name, string role)
        {
            List<string> quotations = new List<string>();
            if (role != "Admin")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(@"select quotation_no,proposer from Quotation where proposer ='" + name + "' except " +
                                                     "select quotation_no,proposal_created_by from Proposal where proposal_created_by='" + name + "'", ConnectSQL.OpenConnect());
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            quotations.Add(dr["quotation_no"].ToString());
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
            else
            {
                return quotations;
            }
        }

        public List<ProposalModel> getProposals(string name, string role)
        {
            try
            {
                List<ProposalModel> proposals = new List<ProposalModel>();
                string command = "";
                if (role == "Admin")
                {
                    command = "select Proposal.*,Quotation.* from Proposal LEFT JOIN Quotation ON Proposal.quotation_no = Quotation.quotation_no order by Proposal.quotation_no";
                }
                else if (role != "Admin" && role != "" && role != null)
                {
                    command = "select Proposal.*,Quotation.* from Proposal LEFT JOIN Quotation ON Proposal.quotation_no = Quotation.quotation_no where Proposal.proposal_department = '" + role + "' order by Proposal.quotation_no";
                }
                else
                {
                    command = "select Proposal.*,Quotation.* from Proposal LEFT JOIN Quotation ON Proposal.quotation_no = Quotation.quotation_no where Proposal.proposal_created_by = '" + name + "' order by Proposal.quotation_no";
                }

                SqlCommand cmd = new SqlCommand(command, ConnectSQL.OpenConnect());
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ProposalModel p = new ProposalModel()
                        {
                            proposal_created_by = dr["proposal_created_by"].ToString(),
                            proposal_department = dr["proposal_department"].ToString(),
                            request_date = dr["request_date"] != DBNull.Value ? Convert.ToDateTime(dr["request_date"].ToString()).ToString("yyyy-MM-dd") : "",
                            proposal_status = dr["proposal_status"].ToString(),
                            proposal_revision = dr["proposal_revision"].ToString(),
                            proposal_cost = dr["proposal_cost"].ToString(),
                            proposal_quoted_price = dr["proposal_quoted_price"].ToString(),
                            gp = dr["gp"].ToString(),
                            finish_date = dr["finish_date"] != DBNull.Value ? Convert.ToDateTime(dr["finish_date"].ToString()).ToString("yyyy-MM-dd") : "",
                            engineering_request = dr["engineering_request"].ToString(),
                            ppc_request = dr["ppc_request"].ToString(),
                            person_in_charge = dr["person_in_charge"].ToString(),
                            quotation = new QuotationModel()
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
                                expected_date = dr["expected_date"] != DBNull.Value ? Convert.ToDateTime(dr["expected_date"].ToString()).ToString("yyyy-MM-dd") : null,
                                status = dr["status"].ToString(),
                                stages = dr["stages"].ToString(),
                                how_to_support = dr["how_to_support"].ToString(),
                                competitor = dr["competitor"].ToString(),
                                competitor_description = dr["competitor_description"].ToString(),
                                competitor_price = dr["competitor_price"].ToString(),
                                sale_name = dr["sale_name"].ToString(),
                                department = dr["department"].ToString(),
                                detail = dr["detail"].ToString()
                            }
                        };
                        proposals.Add(p);
                    }
                    dr.Close();
                }
                return proposals;
            }
            finally
            {
                if (ConnectSQL.con.State == System.Data.ConnectionState.Open)
                {
                    ConnectSQL.CloseConnect();
                }
            }
        }

        public string Insert(List<QuotationModel> model, List<string> quotations)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO Proposal(
                                                                            quotation_no,
                                                                            proposal_created_by,
                                                                            proposal_department,
                                                                            request_date,proposal_revision,proposal_quoted_price) VALUES (
                                                                            @quotation_no,
                                                                            @proposal_created_by,
                                                                            @proposal_department,
                                                                            @request_date,
                                                                            @proposal_revision,
                                                                            @proposal_quoted_price)", ConnectSQL.OpenConnect()))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = ConnectSQL.OpenConnect();
                    cmd.Parameters.Add("@quotation_no", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@proposal_created_by", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@proposal_department", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@request_date", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@proposal_revision", SqlDbType.NVarChar);
                    cmd.Parameters.Add("@proposal_quoted_price", SqlDbType.NVarChar);
                    for (int i = 0; i < model.Count; i++)
                    {
                        bool b = quotations.Any(a => a == model[i].quotation_no); //check match quotation
                        if (b)
                        {
                            cmd.Parameters[0].Value = model[i].quotation_no;
                            cmd.Parameters[1].Value = model[i].proposer;
                            cmd.Parameters[2].Value = model[i].department;
                            cmd.Parameters[3].Value = DateTime.Now.ToString("yyyy-MM-dd");
                            cmd.Parameters[4].Value = model[i].revision;
                            cmd.Parameters[5].Value = model[i].quoted_price;

                            cmd.ExecuteNonQuery();
                        }
                    }



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

        public string Update(ProposalModel model)
        {
            try
            {
                SqlDataReader reader;
                SqlCommand cmd = new SqlCommand(@"UPDATE Proposal SET proposal_created_by='" + model.proposal_created_by + "'," +
                                                                      "proposal_department='" + model.proposal_department + "'," +
                                                                      "request_date='" + model.request_date + "'," +
                                                                      "proposal_status='" + model.proposal_status + "'," +
                                                                      "proposal_revision='" + model.proposal_revision + "'," +
                                                                      "proposal_cost='" + model.proposal_cost + "'," +
                                                                      "proposal_quoted_price='" + model.proposal_quoted_price + "'," +
                                                                      "gp='" + model.gp + "'," +
                                                                      "finish_date='" + model.finish_date + "'," +
                                                                      "engineering_request='" + model.engineering_request + "'," +
                                                                      "ppc_request='" + model.ppc_request + "'," +
                                                                      "person_in_charge='" + model.person_in_charge + "'" +
                                                                      "WHERE quotation_no='" + model.quotation.quotation_no + "'");
                cmd.CommandType = CommandType.Text;
                cmd.Connection = ConnectSQL.OpenConnect();
                reader = cmd.ExecuteReader();
                reader.Close();

                //update quotation {revision,quoted price} 
                SqlDataReader readerquptation;
                SqlCommand cmdquotation = new SqlCommand(@"UPDATE Quotation SET revision='" + model.proposal_revision + "'," +
                                                                      "quoted_price='" + model.proposal_quoted_price + "'" +
                                                                      "WHERE quotation_no='" + model.quotation.quotation_no + "'");
                cmdquotation.CommandType = CommandType.Text;
                cmdquotation.Connection = ConnectSQL.OpenConnect();
                readerquptation = cmd.ExecuteReader();
                readerquptation.Close();

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
