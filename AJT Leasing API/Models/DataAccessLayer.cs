using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using static AJT_Leasing_API.Models.ConfirmAddDriverResponse;

namespace AJT_Leasing_API.Models
{
    public class DataAccessLayer
    {
        //Changes start by 27-04-2021 New Parameter
        string SaveErrorResp = string.Empty;
        //Changes start by 27-04-2021 New Parameter

        public DataTable GetTransformData(GetTransformationDetails objGetTransformDetails)
        {
            try
            {
                OracleConnection connection = new OracleConnection(Constant.OracleDB);
                OracleCommand _cmd = new OracleCommand();
                _cmd.Connection = connection;
                _cmd.CommandText = Constant.GetTransformationDetails;
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = objGetTransformDetails.Flag;
                _cmd.Parameters.Add("P_SOURCE", OracleDbType.Varchar2).Value = objGetTransformDetails.Source;
                _cmd.Parameters.Add("P_SOURCE_CODE", OracleDbType.Varchar2).Value = objGetTransformDetails.SourceCode;
                _cmd.Parameters.Add("P_SRC_TYPE", OracleDbType.Varchar2).Value = objGetTransformDetails.SourceType;
                _cmd.Parameters.Add("P_REFERENCE_NO", OracleDbType.Varchar2).Value = objGetTransformDetails.ReferenceNo;
                OracleParameter param = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                _cmd.Parameters.Add(param);
                return Execute(ref _cmd);
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, string.Empty, string.Empty, "GetTransformData");
                SaveErrorResp = SaveErrorDetails("1", string.Empty, string.Empty, ex.ToString(), "GetTransformData", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
        }
        public DataTable GetQuotePremDetails(TrfProcedureDetails trfProcedureDetails)
        {
            OracleConnection conn = new OracleConnection();
            DataTable dtquotePremDetails = new DataTable();
            try
            {

                ErrHandler.WriteLog("PremDetails-Input", JsonConvert.SerializeObject(trfProcedureDetails), trfProcedureDetails.Reqt_Ref_No);
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.prc_prem_calc_tameeni))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("P_REQT_REF_NO", OracleDbType.Varchar2).Value = trfProcedureDetails.Reqt_Ref_No;
                        cmd.Parameters.Add("P_REQT_REF_NO", OracleDbType.Varchar2).Value = "LEASING";
                        cmd.Parameters.Add("P_MAKE_CODE", OracleDbType.Int32).Value = trfProcedureDetails.Make_Code;
                        cmd.Parameters.Add("P_MODEL_CODE", OracleDbType.Int64).Value = trfProcedureDetails.Model_Code;
                        cmd.Parameters.Add("P_BODY_TYPE_CODE ", OracleDbType.Int32).Value = trfProcedureDetails.Body_Type_Code;
                        cmd.Parameters.Add("P_PRODUCTION_YEAR", OracleDbType.Int32).Value = trfProcedureDetails.Production_Year;
                        cmd.Parameters.Add("P_DRIVER_AGE", OracleDbType.Int32).Value = trfProcedureDetails.Driver_Age;
                        cmd.Parameters.Add("P_GENDER ", OracleDbType.Int32).Value = trfProcedureDetails.Gender;
                        cmd.Parameters.Add("P_NCD_LEVEL", OracleDbType.Int32).Value = trfProcedureDetails.NCD_Level;
                        cmd.Parameters.Add("P_NCD_DRIVER_LEVEL", OracleDbType.Int32).Value = trfProcedureDetails.Ncd_Driver_Level;
                        cmd.Parameters.Add("P_TAT", OracleDbType.Int32).Value = trfProcedureDetails.Tat;
                        cmd.Parameters.Add("P_CITY", OracleDbType.Varchar2).Value = trfProcedureDetails.City;
                        cmd.Parameters.Add("P_DRIVER_NATIONALITY", OracleDbType.Int64).Value = trfProcedureDetails.Driver_Nationality;
                        cmd.Parameters.Add("P_ZIPCODE", OracleDbType.Int32).Value = trfProcedureDetails.ZipCode;
                        cmd.Parameters.Add("P_VEHICLEMAKETEXTNIC", OracleDbType.Varchar2).Value = trfProcedureDetails.VehicleMakeTextNIC;
                        cmd.Parameters.Add("P_VEHICLEMODELTEXTNIC", OracleDbType.Varchar2).Value = trfProcedureDetails.VehicleModelTextNIC;
                        cmd.Parameters.Add("P_POLICY_EFFECTIVE_DATE", OracleDbType.Varchar2).Value = DateTime.Now.ToString("dd-MM-yyyy");
                        cmd.Parameters.Add("P_POLICYHOLDER_ID", OracleDbType.Int64).Value = trfProcedureDetails.PolicyHolder_Id;
                        cmd.Parameters.Add("P_VEHICLE_SEQUENCE_NUMBER", OracleDbType.Int64).Value = trfProcedureDetails.Vehicle_Sequence_Number;
                        cmd.Parameters.Add("P_VEHICLECUSTOM_ID", OracleDbType.Int64).Value = trfProcedureDetails.VehicleCustome_Id;
                        cmd.Parameters.Add("P_SUMINSURED", OracleDbType.Int64).Value = trfProcedureDetails.SumInsured != null ? trfProcedureDetails.SumInsured : (object)DBNull.Value;
                        cmd.Parameters.Add("P_REPAIRTYPE", OracleDbType.Varchar2).Value = trfProcedureDetails.REPAIRTYPE;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        OracleDataReader dr = cmd.ExecuteReader();
                        dtquotePremDetails.Load(dr);
                        //Changes Begin by Kunal on 14-12-2021 for new parameter P_reson
                        //if (dtquotePremDetails.Rows.Count <= 0)
                        //{
                        //    var ReasonWithError = (String)cmd.Parameters["P_REASON"].Value.ToString();
                        //    dtquotePremDetails.Columns.Add("P_REASON");
                        //    dtquotePremDetails.Rows.Add(ReasonWithError);
                        //    return dtquotePremDetails;
                        //}
                        //var ReasonWithoutError = (String)cmd.Parameters["P_REASON"].Value.ToString();
                        //dtquotePremDetails.Columns.Add("P_REASON");
                        //dtquotePremDetails.Rows.Add(ReasonWithoutError);
                        //Changes End by Kunal on 14-12-2021 for new parameter P_reson

                        ErrHandler.WriteLog("PremDetails-Output", JsonConvert.SerializeObject(dtquotePremDetails), trfProcedureDetails.Reqt_Ref_No);
                        return dtquotePremDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, trfProcedureDetails.Reqt_Ref_No, string.Empty, "GetQuotePremDetails");
                SaveErrorResp = SaveErrorDetails("1", trfProcedureDetails.Reqt_Ref_No, JsonConvert.SerializeObject(trfProcedureDetails), ex.ToString(), "GetQuotePremDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        //Changes Start By Sagar 14092021

        public string SaveQuoteDetails(QuoteDetails quoteDetails, QuoteResponse quoteResponse, int flag, DataTable quoteOutput, long QuoteReferenceNo, ErrorDetails errorDetails, TrfProcedureDetails TariffInput)
        {
            string Resp;
            DataTable dataTable = new DataTable();
            OracleConnection conn = new OracleConnection();
            try
            {
                OracleConnection connection = new OracleConnection(Constant.OracleDB);
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connection;
                cmd.CommandText = Constant.QuoteDetails;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = flag;
                cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = quoteDetails.RequestReferenceNo;
                cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Varchar2).Value = quoteResponse.CompQuotes[0].QuoteReferenceNo.ToString();
                cmd.Parameters.Add("P_QUOTEREQTJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(quoteDetails);
                cmd.Parameters.Add("P_QUOTERESPJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(quoteResponse == null ? errorDetails : (object)quoteResponse);
                cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(TariffInput);
                cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(quoteOutput);
                cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = quoteResponse == null ? "Failed" : "Created";
                cmd.Parameters.Add("P_INSURANCETYPEID", OracleDbType.Int32).Value = Convert.ToInt32(ConfigurationManager.AppSettings["InsuranceTypeID"]);
                cmd.Parameters.Add("P_POLICYREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                cmd.Parameters.Add("P_POLICYRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = DBNull.Value;
                cmd.Parameters.Add("P_VEHIMGREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                cmd.Parameters.Add("P_VEHIMGRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                cmd.Parameters.Add("P_PNREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                cmd.Parameters.Add("P_PNRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                cmd.Parameters.Add("P_LESSORID", OracleDbType.Long).Value = quoteDetails.Details.LessorID;
                cmd.Parameters.Add("P_LESSEEID", OracleDbType.Long).Value = quoteDetails.Details.LesseeID;
                cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
                cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                dataTable = Execute(ref cmd);
                if (dataTable.Rows.Count > 0 && (dataTable.Rows[0]["QUOTEREFERENCENO"] != null || !string.IsNullOrEmpty(dataTable.Rows[0]["QUOTEREFERENCENO"].ToString())))
                {
                    Resp = dataTable.Rows[0]["QUOTEREFERENCENO"].ToString();
                }
                else
                {
                    Resp = "request_failed";
                }
            }
            catch (OracleException oex)
            {
                ErrHandler.WriteError(oex, quoteDetails.RequestReferenceNo, QuoteReferenceNo.ToString(), "SaveQuoteDetails(OracleException)");
                SaveErrorResp = SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), oex.ToString(), "SaveQuoteDetails(OracleException)", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                Resp = oex.Number == 1 ? "duplicate_error" : "unexpected_error";
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, quoteDetails.RequestReferenceNo, QuoteReferenceNo.ToString(), "SaveQuoteDetails(Exception)");
                SaveErrorResp = SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), ex.ToString(), "SaveQuoteDetails(Exception)", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                Resp = "unexpected_error";
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return Resp;
        }

        public DataTable GetQuoteDetails(int flag,string RequestReferenceNo, string QuoteReferenceNo, long? PolicyRefNo)
        {
            ErrHandler.WriteLog("GetQuoteDetails : ", "log1", $"RequestReferenceNo:{RequestReferenceNo.ToString()};QuoteReferenceNo:{QuoteReferenceNo.ToString()}");
            DataTable dtquoteDetails = new DataTable();
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.QuoteDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = flag == 2 ? RequestReferenceNo : (object)DBNull.Value;
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Varchar2).Value = flag == 2 || flag == 8 ? QuoteReferenceNo : (object)DBNull.Value;
                        cmd.Parameters.Add("P_QUOTEREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUOTERESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_INSURANCETYPEID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLICYREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLICYRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = flag == 6 ? PolicyRefNo : (object)DBNull.Value;
                        cmd.Parameters.Add("P_VEHIMGREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHIMGRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PNREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PNRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Long).Value = DBNull.Value;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Long).Value = DBNull.Value;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        ErrHandler.WriteLog("GetQuoteDetails : ", "log2", $"RequestReferenceNo:{ RequestReferenceNo.ToString()};QuoteReferenceNo:{QuoteReferenceNo.ToString()}");
                        OracleDataReader dr = cmd.ExecuteReader();
                        dtquoteDetails.Load(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, RequestReferenceNo, QuoteReferenceNo, "GetQuoteDetails");
                SaveErrorResp = SaveErrorDetails("1", RequestReferenceNo, string.Empty, ex.ToString(), "GetQuoteDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return dtquoteDetails;
        }

        public DataTable GetQuotationDetails(int flag,string QuoteReferenceNo)
        {
            ErrHandler.WriteLog("GetQuoteDetails : ", "log1", $"QuoteReferenceNo:{QuoteReferenceNo.ToString()}");
            DataTable dtquoteDetails = new DataTable();
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.QuoteDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Varchar2).Value = QuoteReferenceNo;
                        cmd.Parameters.Add("P_QUOTEREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUOTERESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_INSURANCETYPEID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLICYREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLICYRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHIMGREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHIMGRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PNREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PNRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Long).Value = DBNull.Value;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Long).Value = DBNull.Value;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        ErrHandler.WriteLog("GetQuoteDetails : ", "log2", $"QuoteReferenceNo:{QuoteReferenceNo.ToString()}");
                        OracleDataReader dr = cmd.ExecuteReader();
                        dtquoteDetails.Load(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, string.Empty, QuoteReferenceNo, "GetQuoteDetails");
                SaveErrorResp = SaveErrorDetails("1", string.Empty, string.Empty, ex.ToString(), "GetQuoteDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return dtquoteDetails;
        }

        public string UpdateQuoteDetails(int flag, PolicyRequest policyRequest, PolicyResponse policyResponse, ErrorDetails errorDetails)
        {
            Int32 Resp;
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.QuoteDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = flag == 3 ? policyRequest.RequestReferenceNo : (object)DBNull.Value;
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Varchar2).Value = flag == 3 ? policyRequest.QuoteReferenceNo : (object)DBNull.Value;
                        cmd.Parameters.Add("P_QUOTEREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUOTERESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = flag == 3 ? ((policyResponse != null && policyResponse.Status) ? "SELECTED" : (object)DBNull.Value) : (object)DBNull.Value;
                        cmd.Parameters.Add("P_INSURANCETYPEID", OracleDbType.Int32).Value = Convert.ToInt32(ConfigurationManager.AppSettings["InsuranceTypeID"]);
                        cmd.Parameters.Add("P_POLICYREQTJSON", OracleDbType.Clob).Value = flag == 3 ? JsonConvert.SerializeObject(policyRequest) : (object)DBNull.Value;
                        cmd.Parameters.Add("P_POLICYRESPJSON", OracleDbType.Clob).Value = flag == 3 ? JsonConvert.SerializeObject(policyResponse == null ? errorDetails : (object)policyResponse) : (object)DBNull.Value;
                        //cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = flag == 3 && policyResponse != null ? policyResponse.PolicyReferenceNo : flag == 4 ? vehImgUploadEntitycs.PolicyReferenceNo : (object)DBNull.Value;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = (object)DBNull.Value;
                        cmd.Parameters.Add("P_VEHIMGREQTJSON", OracleDbType.Clob).Value = (object)DBNull.Value;
                        cmd.Parameters.Add("P_VEHIMGRESPJSON", OracleDbType.Clob).Value = (object)DBNull.Value;
                        cmd.Parameters.Add("P_PNREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PNRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Long).Value = DBNull.Value;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Long).Value = DBNull.Value;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        Resp = cmd.ExecuteNonQuery();
                        return Resp == -1 ? "Success" : "request_failed";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, policyRequest.RequestReferenceNo, policyRequest.QuoteReferenceNo.ToString(), "UpdateQuoteDetails");
                SaveErrorResp = SaveErrorDetails("1", policyRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyRequest), ex.ToString(), "UpdateQuoteDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                return "unexpected_error";
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        #region 'PN Start'

        public string UpdateQuoteDetails(int flag, PolicyIssueRequest policyIssueRequest, PolicyIssueResponse policyIssueResponse)
        {
            Int32 Resp;
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.QuoteDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = policyIssueRequest.RequestReferenceNo;
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Varchar2).Value = policyIssueRequest.QuoteReferenceNo;
                        cmd.Parameters.Add("P_QUOTEREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUOTERESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["PolicyIssueStatus"].ToString();
                        cmd.Parameters.Add("P_INSURANCETYPEID", OracleDbType.Int32).Value = Convert.ToInt32(ConfigurationManager.AppSettings["InsuranceTypeID"]);
                        cmd.Parameters.Add("P_POLICYREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLICYRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHIMGREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHIMGRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PNREQTJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(policyIssueRequest);
                        cmd.Parameters.Add("P_PNRESPJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(policyIssueResponse);
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Long).Value = DBNull.Value;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Long).Value = DBNull.Value;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        Resp = cmd.ExecuteNonQuery();
                        return Resp == -1 ? "Success" : "request_failed";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, policyIssueRequest.RequestReferenceNo, policyIssueRequest.QuoteReferenceNo.ToString(), "UpdateQuoteDetails");
                SaveErrorResp = SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), ex.ToString(), "UpdateQuoteDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                return "unexpected_error";
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        #endregion 'PN End' 

        public string SavePolDetails(int flag, SavePolicyDetails policyDetails)
        {
            Int32 Resp;
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyNumber))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = policyDetails.ReqtReferenceNo;
                        cmd.Parameters.Add("P_INSURANCECOMPANYCD", OracleDbType.Int32).Value = policyDetails.InsuranceCompanyCode;
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Int64).Value = policyDetails.QuoteReferenceNo;
                        cmd.Parameters.Add("P_INSURANCETYPEID", OracleDbType.Int32).Value = policyDetails.InsuranceTypeId;
                        cmd.Parameters.Add("P_OLDPOLICYNUMBER", OracleDbType.Varchar2).Value = policyDetails.OldPolicyNumber;
                        cmd.Parameters.Add("P_NEWPOLICYNUMBER", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CUSTOMIZEDPARAMETER", OracleDbType.Clob).Value = JsonConvert.SerializeObject(policyDetails.customizedParameters);
                        cmd.Parameters.Add("P_POLICYREQUESTREFERENCENO", OracleDbType.Varchar2).Value = policyDetails.PolicyReqtRefNo;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Int64).Value = policyDetails.PolicyRefNo;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = policyDetails.Status;
                        cmd.Parameters.Add("P_ISERROR", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ERRORDESC", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        Resp = cmd.ExecuteNonQuery();
                        return Resp == -1 ? "Success" : "request_failed";
                    }
                }
            }
            catch (OracleException oex)
            {
                ErrHandler.WriteError(oex, policyDetails.ReqtReferenceNo, policyDetails.QuoteReferenceNo.ToString(), "SavePolDetails(OracleException)");
                SaveErrorResp = SaveErrorDetails("1", policyDetails.ReqtReferenceNo, JsonConvert.SerializeObject(policyDetails), oex.ToString(), "SavePolDetails(OracleException)", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                return oex.Number == 1 ? "duplicate_error" : "unexpected_error";
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, policyDetails.ReqtReferenceNo, policyDetails.QuoteReferenceNo.ToString(), "SavePolDetails(Exception)");
                SaveErrorResp = SaveErrorDetails("1", policyDetails.ReqtReferenceNo, JsonConvert.SerializeObject(policyDetails), ex.ToString(), "SavePolDetails(Exception)", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                return "unexpected_error";
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        internal static bool IsAuthorizedUser(string Username, string Password)
        {
            DataTable dtResp = new DataTable();
            try
            {
                OracleConnection connection = new OracleConnection(Constant.OracleDB);
                OracleCommand _cmd = new OracleCommand();
                _cmd.Connection = connection;
                _cmd.CommandText = Constant.AuthenticationDetaileGet;
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Parameters.Add("P_AUTHFOR", OracleDbType.Varchar2).Value = "AJTLEASING";
                _cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2).Value = Username;
                _cmd.Parameters.Add("P_PASSWORD", OracleDbType.Varchar2).Value = Password;
                OracleParameter param = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                _cmd.Parameters.Add(param);
                dtResp = Execute(ref _cmd);
                if (dtResp != null && dtResp.Rows.Count > 0)
                {
                    return (Username == dtResp.Rows[0]["USERNAME"].ToString() && Password == dtResp.Rows[0]["PASSWORD"].ToString());
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, string.Empty, string.Empty, "IsAuthorizedUser");
                return false;
            }
        }

        //public DataTable GetScheme(int flag, string aggregator, long insuredId)
        //{
        //    DataTable dtSchemeDetails = new DataTable();
        //    OracleConnection conn = new OracleConnection();
        //    try
        //    {
        //        using (conn = new OracleConnection(Constant.OracleDB))
        //        {
        //            using (OracleCommand cmd = new OracleCommand(Constant.GetSchemeCode))
        //            {
        //                conn.Open();
        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = flag;
        //                cmd.Parameters.Add("P_AGGREGATORNAME", OracleDbType.Varchar2).Value = aggregator;
        //                cmd.Parameters.Add("P_INSUREDID", OracleDbType.Int64).Value = insuredId;
        //                cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        //                OracleDataReader dr = cmd.ExecuteReader();
        //                dtSchemeDetails.Load(dr);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, string.Empty, string.Empty, "GetScheme");
        //        throw;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //            conn.Dispose();
        //        }
        //    }
        //    return dtSchemeDetails;
        //}

        public static DataTable Execute(ref OracleCommand _cmd)
        {
            try
            {
                _cmd.Connection.Open();
                OracleDataReader dr = _cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
            catch (Exception exx)
            {
                throw exx;
            }
            finally
            {
                _cmd.Connection.Close();
                _cmd.Connection.Dispose();
            }
        }

        //public string SaveUpdtPolicyDtails(int Flag, List<Error> ErrorMsg, QueryFeatureRequest queryFeatureRequest, TrfProcedureDetails trfProcedureDetails, DataTable dtPremDetails, int UpdateReason)
        //{
        //    Int32 Resp;
        //    OracleConnection conn = new OracleConnection();
        //    DataTable dtDocumentNo = new DataTable();
        //    try
        //    {
        //        using (conn = new OracleConnection(Constant.OracleDB))
        //        {
        //            using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
        //            {
        //                conn.Open();
        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = Flag;
        //                cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = queryFeatureRequest.QueryRequestReferenceNo;
        //                cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Long).Value = queryFeatureRequest.PolicyReferenceNo;
        //                cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = queryFeatureRequest.PolicyNumber;
        //                cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_LESSORID", OracleDbType.Long).Value = queryFeatureRequest.LessorID;
        //                cmd.Parameters.Add("P_LESSEEID", OracleDbType.Long).Value = queryFeatureRequest.LesseeID;
        //                cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = UpdateReason;
        //                cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "Failed" : "QueryFeature";
        //                cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
        //                cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = queryFeatureRequest != null ? JsonConvert.SerializeObject(queryFeatureRequest) : string.Empty;
        //                cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = trfProcedureDetails != null ? JsonConvert.SerializeObject(trfProcedureDetails) : string.Empty;
        //                cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = dtPremDetails != null && dtPremDetails.Rows.Count > 0 ? JsonConvert.SerializeObject(dtPremDetails) : string.Empty;
        //                cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = queryFeatureRequest.VehicleSequenceNumber;
        //                cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = queryFeatureRequest.VehicleCustomID;
        //                cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_MOBILENO", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        //                Resp = cmd.ExecuteNonQuery();
        //                return Resp == -1 ? "Success" : "request_failed";

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, queryFeatureRequest.PolicyReferenceNo.ToString(), string.Empty, "SaveUpdtPolicyDtails");
        //        SaveErrorResp = SaveErrorDetails("1", queryFeatureRequest.PolicyReferenceNo.ToString(), JsonConvert.SerializeObject(queryFeatureRequest), ex.ToString(), "SaveUpdtPolicyDtails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
        //        throw ex;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //}

        //public string UpdtPolicyDtails(int Flag, List<Error> ErrorMsg, PurchaseFeatureRequest purchaseFeatureRequest, int UpdateReason)
        //{
        //    Int32 Resp;
        //    OracleConnection conn = new OracleConnection();
        //    DataTable dtDocumentNo = new DataTable();
        //    try
        //    {
        //        using (conn = new OracleConnection(Constant.OracleDB))
        //        {
        //            using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
        //            {
        //                conn.Open();
        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = Flag;
        //                cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = purchaseFeatureRequest.QueryRequestReferenceNo;
        //                cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Long).Value = purchaseFeatureRequest.PolicyReferenceNo;
        //                cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_LESSORID", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_LESSEEID", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = UpdateReason;
        //                cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "Failed" : "Pending";
        //                cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
        //                cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_MOBILENO", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = purchaseFeatureRequest != null ? JsonConvert.SerializeObject(purchaseFeatureRequest) : string.Empty;
        //                cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        //                Resp = cmd.ExecuteNonQuery();
        //                return Resp == -1 ? "Success" : "request_failed";

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, purchaseFeatureRequest.PolicyReferenceNo.ToString(), string.Empty, "UpdtPolicyDtails");
        //        SaveErrorResp = SaveErrorDetails("1", purchaseFeatureRequest.PolicyReferenceNo.ToString(), JsonConvert.SerializeObject(purchaseFeatureRequest), ex.ToString(), "UpdtPolicyDtails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
        //        throw ex;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //}

        // Start date: 16-12-2020 by rahul for making IcUpdatePolicy api

        //public string SaveUpdtPolicyDetails(int flag, IcUpdatePolicyRequest icUpdatePolicyRequest, List<Error> ErrorMsg, int UpdateReason, string PlateNoA, string PlateNoB, string PlateNoC)
        //{
        //    string Resp;
        //    OracleConnection conn = new OracleConnection();
        //    try
        //    {
        //        using (conn = new OracleConnection(Constant.OracleDB))
        //        {
        //            using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
        //            {
        //                conn.Open();
        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = flag;
        //                cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.RequestReferenceNo;
        //                cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Long).Value = icUpdatePolicyRequest.QuoteReferenceNo;
        //                cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.PolicyNumber;
        //                cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_LESSORID", OracleDbType.Long).Value = icUpdatePolicyRequest.LessorID;
        //                cmd.Parameters.Add("P_LESSEEID", OracleDbType.Long).Value = icUpdatePolicyRequest.LesseeID;
        //                cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = UpdateReason;
        //                cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "Failed" : "Pending";
        //                cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
        //                cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(icUpdatePolicyRequest);
        //                cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.VehicleSequenceNumber;
        //                cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.VehicleCustomID;
        //                cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.VehiclePlateTypeID.ToString();
        //                cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = icUpdatePolicyRequest.VehiclePlateNumber;
        //                cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = PlateNoA;
        //                cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = PlateNoB;
        //                cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = PlateNoC;
        //                cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.Email;
        //                cmd.Parameters.Add("P_MOBILENO", OracleDbType.Long).Value = icUpdatePolicyRequest.MobileNo;
        //                cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        //                Resp = cmd.ExecuteNonQuery().ToString();
        //                return Resp == "-1" ? "Success" : "request_failed";

        //            }
        //        }
        //    }
        //    catch (OracleException oex)
        //    {
        //        ErrHandler.WriteError(oex, icUpdatePolicyRequest.RequestReferenceNo, icUpdatePolicyRequest.QuoteReferenceNo.ToString(), "SaveUpdtPolicyDetails");
        //        SaveErrorResp = this.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), oex.ToString(), "SaveUpdtPolicyDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
        //        Resp = oex.Number == 1 ? "duplicate_error" : (oex.Message.Contains("no data found") ? "Data_Not_Found" : "unexpected_error");
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, icUpdatePolicyRequest.RequestReferenceNo, icUpdatePolicyRequest.QuoteReferenceNo.ToString(), "SaveUpdtPolicyDetails");
        //        SaveErrorResp = SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), ex.ToString(), "SaveUpdtPolicyDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
        //        return "unexpected_error";
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //    return Resp;
        //}
        // end date: 16-12-2020 by rahul for making IcUpdatePolicy api


        //public string CancelPolicy(int flag, IcCancelPolicyRequest icCancelPolicyRequest, List<Error> ErrorMsg, int UpdateReason)
        //{
        //    Int32 Resp;
        //    OracleConnection conn = new OracleConnection();
        //    try
        //    {
        //        using (conn = new OracleConnection(Constant.OracleDB))
        //        {
        //            using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
        //            {
        //                conn.Open();
        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = flag;
        //                cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = icCancelPolicyRequest.RequestReferenceNo;
        //                cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Long).Value = icCancelPolicyRequest.QuoteReferenceNo;
        //                cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = icCancelPolicyRequest.PolicyNumber;
        //                cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = icCancelPolicyRequest.PolicyReferenceNo;
        //                cmd.Parameters.Add("P_LESSORID", OracleDbType.Long).Value = icCancelPolicyRequest.LessorID;
        //                cmd.Parameters.Add("P_LESSEEID", OracleDbType.Long).Value = icCancelPolicyRequest.LesseeID;
        //                cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = UpdateReason;
        //                cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "Failed" : "Pending";
        //                cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
        //                cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(icCancelPolicyRequest);
        //                cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = icCancelPolicyRequest.VehicleSequenceNumber;
        //                cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = icCancelPolicyRequest.VehicleCustomID;
        //                cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_MOBILENO", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = icCancelPolicyRequest.CancellationRequestTime;
        //                cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        //                Resp = cmd.ExecuteNonQuery();
        //                return Resp == -1 ? "Success" : "request_failed";

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, icCancelPolicyRequest.RequestReferenceNo, icCancelPolicyRequest.QuoteReferenceNo.ToString(), "CancelPolicy");
        //        SaveErrorResp = SaveErrorDetails("1", icCancelPolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icCancelPolicyRequest), ex.ToString(), "CancelPolicy", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
        //        return "unexpected_error";
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //}

        // Start date: 22-12-2020 by rahul for making SaveDriverDetails api

        //public string SaveDriverDetails(int flag, AddDriverRequest addDriverRequest, List<Error> ErrorMsg, int UpdateReason)
        //{
        //    Int32 Resp;
        //    OracleConnection conn = new OracleConnection();
        //    try
        //    {
        //        using (conn = new OracleConnection(Constant.OracleDB))
        //        {
        //            using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
        //            {
        //                conn.Open();
        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = flag;
        //                cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = addDriverRequest.AddDriverRequestReferenceNo;
        //                cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Long).Value = addDriverRequest.PolicyReferenceNo;
        //                cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = addDriverRequest.PolicyNumber;
        //                cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_LESSORID", OracleDbType.Long).Value = addDriverRequest.LessorID;
        //                cmd.Parameters.Add("P_LESSEEID", OracleDbType.Long).Value = addDriverRequest.LesseeID;
        //                cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = UpdateReason;
        //                cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "Failed" : "AddDriver";
        //                cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
        //                cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(addDriverRequest);
        //                cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = addDriverRequest.VehicleSequenceNumber;
        //                cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = addDriverRequest.VehicleCustomID;
        //                cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_MOBILENO", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        //                Resp = cmd.ExecuteNonQuery();
        //                return Resp == -1 ? "Success" : "request_failed";

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, addDriverRequest.AddDriverRequestReferenceNo.ToString(), addDriverRequest.PolicyNumber.ToString(), "SaveDriverDetails");
        //        SaveErrorResp = SaveErrorDetails("1", addDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(addDriverRequest), ex.ToString(), "SaveDriverDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
        //        return "unexpected_error";
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //}

        // end date: 22-12-2020 by rahul for making SaveDriverDetails api

        // Start date: 22-12-2020 by rahul for making SaveDriverDetails api

        //public string SaveConfirmDriverDetails(int flag, ConfirmAddDriverRequest confirmAddDriverRequest, List<Error> ErrorMsg, int UpdateReason)
        //{
        //    Int32 Resp;
        //    OracleConnection conn = new OracleConnection();
        //    try
        //    {
        //        using (conn = new OracleConnection(Constant.OracleDB))
        //        {
        //            using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
        //            {
        //                conn.Open();
        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = flag;
        //                cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = confirmAddDriverRequest.AddDriverRequestReferenceNo;
        //                cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Long).Value = confirmAddDriverRequest.PolicyReferenceNo;
        //                cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_LESSORID", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_LESSEEID", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = UpdateReason;
        //                cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "Failed" : "Pending";
        //                cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
        //                cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_MOBILENO", OracleDbType.Long).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
        //                cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(confirmAddDriverRequest);
        //                cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = DBNull.Value;
        //                cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        //                Resp = cmd.ExecuteNonQuery();
        //                return Resp == -1 ? "Success" : "request_failed";

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), confirmAddDriverRequest.PolicyReferenceNo.ToString(), "SaveConfirmDriverDetails");
        //        SaveErrorResp = SaveErrorDetails("1", confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(confirmAddDriverRequest), ex.ToString(), "SaveConfirmDriverDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
        //        return "unexpected_error";
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //}
        // end date: 22-12-2020 by rahul for making SaveDriverDetails api

        public string SaveErrorDetails(string Flag, string RequestReferenceNumber, string InputJson, string ErrorDesc, string MethodName, string ClassName, string AggregaterName, string CreatedBy)
        {
            Int32 Resp;
            OracleConnection connection = new OracleConnection();
            try
            {
                using (connection = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.SaveErrorLogs))
                    {
                        connection.Open();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = Flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = RequestReferenceNumber;
                        cmd.Parameters.Add("P_INPUTJSON", OracleDbType.Clob).Value = InputJson;
                        cmd.Parameters.Add("P_ERRORDESC", OracleDbType.Clob).Value = ErrorDesc;
                        cmd.Parameters.Add("P_METHODNAME", OracleDbType.Varchar2).Value = MethodName;
                        cmd.Parameters.Add("P_CLASSNAME", OracleDbType.Varchar2).Value = ClassName;
                        cmd.Parameters.Add("P_AGGREGATORNAME", OracleDbType.Varchar2).Value = AggregaterName;
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = CreatedBy;
                        Resp = cmd.ExecuteNonQuery();
                        return Resp == -1 ? "Success" : "request_failed";
                    }
                }
            }
            catch (Exception exception1)
            {
                ErrHandler.WriteError(exception1, RequestReferenceNumber, string.Empty, "SaveErrorDetails");
                throw;
            }
        }

        // changes start by rahul on Date: 20-12-2021 add IcDocumentDownload
        #region 'IcDocumentDownload'
        //public DataTable GetIcDocumentDownload(IcDocumentDownloadRequest icDocumentDownloadRequest)
        //{
        //    string Resp = string.Empty;
        //    DataTable dt = new DataTable();
        //    OracleConnection conn = new OracleConnection();

        //    try
        //    {
        //        using (conn = new OracleConnection(Constant.OracleDB))
        //        {
        //            using (OracleCommand cmd = new OracleCommand(Constant.GetDocumentDownload))
        //            {
        //                conn.Open();
        //                cmd.Connection = conn;
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                //Resp = (String)cmd.Parameters["P_InvoicePDFData"].Value.ToString();
        //                cmd.Parameters.Add("P_POLICY_NO", OracleDbType.NVarchar2).Value = icDocumentDownloadRequest.PolicyNumber.ToString();
        //                //cmd.Parameters.Add("P_INVOICE_NO", OracleDbType.NVarchar2).Value = "";

        //                cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
        //                OracleDataReader dr = cmd.ExecuteReader();
        //                dt.Load(dr);

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, icDocumentDownloadRequest.DDRequestReferenceNo, icDocumentDownloadRequest.PolicyNumber, "GetIcDocumentDownload");
        //        SaveErrorResp = SaveErrorDetails("1", icDocumentDownloadRequest.DDRequestReferenceNo, string.Empty, ex.ToString(), "GetIcDocumentDownload", "DataAccessLayer", "TameeniLeasing", "AJT_Leasing_API");
        //        throw;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //    return dt;
        //}
        #endregion 'IcDocumentDownload'
        // changes end by rahul on Date: 16-11-2021 add IcDocumentDownload


        // Changes Start by Shyam Patil on Date:29-04-2022 for VALIDATION
        public bool ValidateSA(int FLAG, string PRODUCTCODE, decimal suminsured, string PolicyNumber)
        {
            string SCHEME_CODE = ConfigurationManager.AppSettings["SchemeCode"].ToString();
            //DataTable DocumentDownload = new DataTable();
            string Resp = string.Empty;
            DataTable dt = new DataTable();
            OracleConnection conn = new OracleConnection();

            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.ValidateSA))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = FLAG;
                        cmd.Parameters.Add("P_SCHEME_CODE", OracleDbType.NVarchar2).Value = SCHEME_CODE;
                        cmd.Parameters.Add("P_PRODUCTCODE", OracleDbType.NVarchar2).Value = PRODUCTCODE;
                        cmd.Parameters.Add("p_suminsured", OracleDbType.Decimal).Value = suminsured;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        OracleDataReader dr = cmd.ExecuteReader();
                        dt.Load(dr);

                        if (dt.Rows.Count > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, PRODUCTCODE, PolicyNumber, "ValidateSA");
                SaveErrorResp = SaveErrorDetails("1", PRODUCTCODE, string.Empty, ex.ToString(), "ValidateSA", "DataAccessLayer", "Tameeni", "TAMEENI");
                throw;
            }
            finally
            {
                conn.Clone();
                conn.Dispose();
            }
            return false;
        }

        public string SaveConfirmDriverDetails(int flag, ConfirmAddDriverRequest confirmAddDriverRequest, List<Error> ErrorMsg, int UpdateReason,ConfirmAddDriverResponse confirmAddDriverResponse)
        {
            Int32 Resp;
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = Convert.ToString(confirmAddDriverRequest.AddDriverRequestReferenceNo);
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Int64).Value = confirmAddDriverRequest.AddDriverResponseReferenceNo;
                        cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = confirmAddDriverRequest.PolicyNumber;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Int64).Value = confirmAddDriverRequest.PolicyReferenceNo;
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Int64).Value = confirmAddDriverRequest.LessorID;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Int64).Value = confirmAddDriverRequest.LesseeID;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = DBNull.Value; 
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "ConfirmAddDriverFailed" : "ConfirmAddDriver";
                        cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_MOBILENO", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = confirmAddDriverRequest!=null? (object)JsonConvert.SerializeObject(confirmAddDriverRequest):DBNull.Value;
                        cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASONID", OracleDbType.Int32).Value = UpdateReason;
                        cmd.Parameters.Add("P_QUERYFEATUREREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUERYFEATURERESPONSEJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASEREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASERESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTRESPJSON", OracleDbType.Clob).Value = confirmAddDriverResponse != null ? (object)JsonConvert.SerializeObject(confirmAddDriverResponse) : DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYRESJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        Resp = cmd.ExecuteNonQuery();
                        return Resp == -1 ? "Success" : "request_failed";

                    }
                }  
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), confirmAddDriverRequest.PolicyReferenceNo.ToString(), "SaveConfirmDriverDetails");
                SaveErrorResp = SaveErrorDetails("1", confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(confirmAddDriverRequest), ex.ToString(), "SaveConfirmDriverDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                return "unexpected_error";
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

       
        public DataTable GetCoverDetails(int flag, string ReqtReferenceNo, StaticValues staticValues)
        {
            DataTable dtCoverDetails = new DataTable();
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.GetCoverDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = flag;
                        cmd.Parameters.Add("P_AGGREGATORNAME", OracleDbType.Varchar2).Value = staticValues.Aggregator;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        OracleDataReader dr = cmd.ExecuteReader();
                        dtCoverDetails.Load(dr);
                    }
                }
            }

            catch (Exception ex)
            {
                SaveErrorResp = SaveErrorDetails("1", ReqtReferenceNo, ex.ToString(), "GetCoverDetails", ConfigurationManager.AppSettings["AggregatorName"].ToString(), ConfigurationManager.AppSettings["CreatedBy"].ToString(), "E", "");

                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return dtCoverDetails;
        }

        public DataTable GetVehicaldetails(int flag, string vehicleMake, string vEHICLE_TYPE)
        {
            StringBuilder parameters = new StringBuilder();
            try
            {
                OracleConnection connection = new OracleConnection(Constant.OracleDB);
                OracleCommand _cmd = new OracleCommand();
                _cmd.Connection = connection;
                _cmd.CommandText = Constant.GetVehicaldetails;
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = flag;
                _cmd.Parameters.Add("P_SOURCE_CODE", OracleDbType.Varchar2).Value = vehicleMake;
                _cmd.Parameters.Add("P_SRC_TYPE", OracleDbType.Varchar2).Value = vEHICLE_TYPE;
                OracleParameter param = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                param.Direction = ParameterDirection.Output;
                _cmd.Parameters.Add(param);
                foreach (OracleParameter p in _cmd.Parameters)
                {
                    parameters.Append(p.ParameterName + ": " + p.Value + Environment.NewLine);
                }
                return Execute(ref _cmd);
            }
            catch (Exception ex)
            {
                //ErrHandler.WriteError(ex, string.Empty, string.Empty, "getTransformData", 0);
                //SaveErrorDetails("", string.Empty, string.Empty, string.Empty, ex.ToString() + "   \nInput Parameters: " + parameters.ToString(), "GetVehicaldetails", "DataAccessLayer", "", "", "E");
                throw ex;
            }
        }

        public DataTable GetAGGREGATORDetails(int FLAG, string AGGREGATORNAME)
        {
            DataTable dtCheckActivePolicy = new DataTable();
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.GetAGGREGATORDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = FLAG;
                        cmd.Parameters.Add("P_AGGREGATORNAME", OracleDbType.Varchar2).Value = AGGREGATORNAME;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        OracleDataReader dr = cmd.ExecuteReader();
                        dtCheckActivePolicy.Load(dr);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return dtCheckActivePolicy;
        }

        public string SaveUpdtPolicyDtails(int Flag, List<Error> ErrorMsg, QueryFeatureRequest queryFeatureRequest, TrfProcedureDetails trfProcedureDetails, DataTable dtPremDetails, int UpdateReason,QueryFeatureResponse queryFeatureResponse,DataTable dt)
        {
            Int32 Resp;
            OracleConnection conn = new OracleConnection();
            DataTable dtDocumentNo = new DataTable();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = Flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = Convert.ToString(queryFeatureRequest.QueryRequestReferenceNo);
                        ErrHandler.WriteLog("SaveUpdtPolicyDtails", queryFeatureResponse == null ? "null" : JsonConvert.SerializeObject(queryFeatureResponse), "log_1");
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Int64).Value = queryFeatureResponse.QueryResponseReferenceNo;
                        ErrHandler.WriteLog("SaveUpdtPolicyDtails", queryFeatureResponse == null ? "null" : JsonConvert.SerializeObject(queryFeatureResponse), "log_2");
                        cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = queryFeatureRequest.PolicyNumber;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Int64).Value = queryFeatureRequest.PolicyReferenceNo;
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Int64).Value = queryFeatureRequest.LessorID;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Int64).Value = queryFeatureRequest.LesseeID;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = UpdateReason;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "QueryFeatureFailed" : "QueryFeature";
                        cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
                        cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = queryFeatureRequest != null ? JsonConvert.SerializeObject(queryFeatureRequest) : string.Empty;
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = trfProcedureDetails != null ? JsonConvert.SerializeObject(trfProcedureDetails) : string.Empty;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = dtPremDetails != null && dtPremDetails.Rows.Count > 0 ? JsonConvert.SerializeObject(dtPremDetails) : string.Empty;
                        cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = queryFeatureRequest.VehicleSequenceNumber;
                        cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = queryFeatureRequest.VehicleCustomID;
                        cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_MOBILENO", OracleDbType.Long).Value = DBNull.Value;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = Convert.ToInt32(dt.Rows[0]["DOCUMENTID"]);
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASONID", OracleDbType.Int32).Value = 5;
                        cmd.Parameters.Add("P_QUERYFEATUREREQJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(queryFeatureRequest);
                        cmd.Parameters.Add("P_QUERYFEATURERESPONSEJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(queryFeatureResponse);
                        cmd.Parameters.Add("P_PURCHASEREQJSON", OracleDbType.Clob).Value= DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASERESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERREQJSON", OracleDbType.Clob).Value= DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERRESPJSON", OracleDbType.Clob).Value= DBNull.Value;
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTRESPJSON", OracleDbType.Clob).Value= DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYREQJSON", OracleDbType.Clob).Value= DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYRESJSON", OracleDbType.Clob).Value= DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYREQJSON", OracleDbType.Clob).Value= DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYRESPJSON", OracleDbType.Clob).Value= DBNull.Value;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        Resp = cmd.ExecuteNonQuery();
                        return Resp == -1 ? "Success" : "request_failed";

                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, queryFeatureRequest.PolicyReferenceNo.ToString(), string.Empty, "SaveUpdtPolicyDtails");
                SaveErrorResp = SaveErrorDetails("1", queryFeatureRequest.PolicyReferenceNo.ToString(), JsonConvert.SerializeObject(queryFeatureRequest), ex.ToString(), "SaveUpdtPolicyDtails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public string SaveDriverDetails(int flag, AddDriverRequest addDriverRequest, List<Error> ErrorMsg, int UpdateReason,AddDriverResponse addDriverResponse,DataTable dt)
        {
            Int32 Resp;
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = Convert.ToString(addDriverRequest.AddDriverRequestReferenceNo);
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Int64).Value = addDriverResponse.AddDriverResponseReferenceNo;
                        cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = addDriverRequest.PolicyNumber;
                        //changes start made by raju on 06-09-2024
                       // cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Int64).Value = addDriverRequest.PolicyReferenceNo;
                        //changes start made by raju on 06-09-2024
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Int64).Value = addDriverRequest.LessorID;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Int64).Value = addDriverRequest.LesseeID;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = UpdateReason;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "AddDriverFailed" : "AddDriver";
                        cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
                        cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(addDriverRequest);
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = addDriverRequest.VehicleSequenceNumber;
                        cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = addDriverRequest.VehicleCustomID;
                        cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_MOBILENO", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = dt.Rows[0]["DOCUMENTID"];
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASONID", OracleDbType.Int32).Value = 6;
                        cmd.Parameters.Add("P_QUERYFEATUREREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUERYFEATURERESPONSEJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASEREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASERESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERREQJSON", OracleDbType.Clob).Value = addDriverRequest == null ? DBNull.Value : (object)JsonConvert.SerializeObject(addDriverRequest);
                        cmd.Parameters.Add("P_ADDDRIVERRESPJSON", OracleDbType.Clob).Value = addDriverResponse == null ? DBNull.Value : (object)JsonConvert.SerializeObject(addDriverResponse);
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYRESJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        Resp = cmd.ExecuteNonQuery();
                        return Resp == -1 ? "Success" : "request_failed";

                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, addDriverRequest.AddDriverRequestReferenceNo.ToString(), addDriverRequest.PolicyNumber.ToString(), "SaveDriverDetails");
                SaveErrorResp = SaveErrorDetails("1", addDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(addDriverRequest), ex.ToString(), "SaveDriverDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                return "unexpected_error";
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public string UpdtPolicyDtails(int Flag, List<Error> ErrorMsg, PurchaseFeatureRequest purchaseFeatureRequest, int UpdateReason,PurchaseFeatureResponse purchaseFeatureResponse)
        {
            Int32 Resp;
            OracleConnection conn = new OracleConnection();
            DataTable dtDocumentNo = new DataTable();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = Flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = Convert.ToString(purchaseFeatureRequest.QueryRequestReferenceNo);
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Int64).Value = purchaseFeatureRequest.QueryResponseReferenceNo;
                        cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = UpdateReason;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "FeaturePurchasedFailed" : "FeaturePurchased";
                        cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
                        cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_MOBILENO", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASONID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUERYFEATUREREQJSON", OracleDbType.Clob).Value = 5;
                        cmd.Parameters.Add("P_QUERYFEATURERESPONSEJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASEREQJSON", OracleDbType.Clob).Value = purchaseFeatureRequest == null ? DBNull.Value : (object)JsonConvert.SerializeObject(purchaseFeatureRequest);
                        cmd.Parameters.Add("P_PURCHASERESPJSON", OracleDbType.Clob).Value = purchaseFeatureResponse==null? DBNull.Value: (object)JsonConvert.SerializeObject(purchaseFeatureResponse);
                        cmd.Parameters.Add("P_ADDDRIVERREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYRESJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        Resp = cmd.ExecuteNonQuery();
                        return Resp == -1 ? "Success" : "request_failed";

                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, purchaseFeatureRequest.PolicyReferenceNo.ToString(), string.Empty, "UpdtPolicyDtails");
                SaveErrorResp = SaveErrorDetails("1", purchaseFeatureRequest.PolicyReferenceNo.ToString(), JsonConvert.SerializeObject(purchaseFeatureRequest), ex.ToString(), "UpdtPolicyDtails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public string SaveUpdtPolicyDetails(int flag, IcUpdatePolicyRequest icUpdatePolicyRequest, List<Error> ErrorMsg, int UpdateReason, string PlateNoA, string PlateNoB, string PlateNoC,IcUpdatePolicyResponse icUpdatePolicyResponse,DataTable dt)
        {
            string Resp;
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.RequestReferenceNo;
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Int64).Value = icUpdatePolicyRequest.QuoteReferenceNo;
                        cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.PolicyNumber;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Int64).Value = icUpdatePolicyRequest.PolicyReferenceNo;
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Int64).Value = icUpdatePolicyRequest.LessorID;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Int64).Value = icUpdatePolicyRequest.LesseeID;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = icUpdatePolicyRequest.UpdatePolicyReasonID;//UpdateReason;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "ICUpdatePolicyFailed" : "ICUpdatePolicy";
                        cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
                        cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(icUpdatePolicyRequest);
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.VehicleSequenceNumber;
                        cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.VehicleCustomID;
                        cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.VehiclePlateTypeID.ToString();
                        cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = icUpdatePolicyRequest.VehiclePlateNumber;
                        cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = PlateNoA == null? icUpdatePolicyRequest.FirstPlateLetterID.ToString(): PlateNoA;
                        cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = PlateNoB == null? icUpdatePolicyRequest.SecondPlateLetterID.ToString(): PlateNoB;
                        cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = PlateNoC== null? icUpdatePolicyRequest.ThirdPlateLetterID.ToString(): PlateNoC;
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = icUpdatePolicyRequest.Email;
                        cmd.Parameters.Add("P_MOBILENO", OracleDbType.Int64).Value = icUpdatePolicyRequest.MobileNo;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = dt.Rows[0]["DOCUMENTID"];
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASONID", OracleDbType.Int32).Value = icUpdatePolicyRequest.UpdatePolicyReasonID;
                        cmd.Parameters.Add("P_QUERYFEATUREREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUERYFEATURERESPONSEJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASEREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASERESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYREQJSON", OracleDbType.Clob).Value =  DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYRESJSON", OracleDbType.Clob).Value =  DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYREQJSON", OracleDbType.Clob).Value = icUpdatePolicyRequest != null ? (object)JsonConvert.SerializeObject(icUpdatePolicyRequest) : DBNull.Value; 
                        cmd.Parameters.Add("P_ICUPDATEPOLICYRESPJSON", OracleDbType.Clob).Value = icUpdatePolicyResponse != null ? (object)JsonConvert.SerializeObject(icUpdatePolicyResponse) : DBNull.Value;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        Resp = cmd.ExecuteNonQuery().ToString();
                        return Resp == "-1" ? "Success" : "request_failed";

                    }
                }
            }
            catch (OracleException oex)
            {
                ErrHandler.WriteError(oex, icUpdatePolicyRequest.RequestReferenceNo, icUpdatePolicyRequest.QuoteReferenceNo.ToString(), "SaveUpdtPolicyDetails");
                SaveErrorResp = this.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), oex.ToString(), "SaveUpdtPolicyDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                Resp = oex.Number == 1 ? "duplicate_error" : (oex.Message.Contains("no data found") ? "Data_Not_Found" : "unexpected_error");
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, icUpdatePolicyRequest.RequestReferenceNo, icUpdatePolicyRequest.QuoteReferenceNo.ToString(), "SaveUpdtPolicyDetails");
                SaveErrorResp = SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), ex.ToString(), "SaveUpdtPolicyDetails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                return "unexpected_error";
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return Resp;
        }
        public String CancelPolicyValidation(int flag, IcCancelPolicyRequest icCancelPolicyRequest, List<Error> ErrorMsg, int UpdateReason)
        {
            string Resp;
            DataTable dtquoteDetails = new DataTable();
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = icCancelPolicyRequest.RequestReferenceNo;
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Int64).Value = icCancelPolicyRequest.QuoteReferenceNo;
                        cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = icCancelPolicyRequest.PolicyNumber;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Int64).Value = icCancelPolicyRequest.PolicyReferenceNo;
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Int64).Value = icCancelPolicyRequest.LessorID;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Int64).Value = icCancelPolicyRequest.LesseeID;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = UpdateReason;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "Failed" : "Pending";
                        cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
                        cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(icCancelPolicyRequest);
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = icCancelPolicyRequest.VehicleSequenceNumber;
                        cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = icCancelPolicyRequest.VehicleCustomID;
                        cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_MOBILENO", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = icCancelPolicyRequest.CancellationRequestTime;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASONID", OracleDbType.Int32).Value = 7;
                        cmd.Parameters.Add("P_QUERYFEATUREREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUERYFEATURERESPONSEJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASEREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASERESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYRESJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        Resp = cmd.ExecuteNonQuery().ToString();
                        return Resp == "-1" ? "Success" : "request_failed";


                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, icCancelPolicyRequest.RequestReferenceNo, icCancelPolicyRequest.QuoteReferenceNo.ToString(), "CancelPolicy");
                SaveErrorResp = SaveErrorDetails("1", icCancelPolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icCancelPolicyRequest), ex.ToString(), "CancelPolicy", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public string CancelPolicy(int flag, IcCancelPolicyRequest icCancelPolicyRequest, List<Error> ErrorMsg, int UpdateReason,IcCancelPolicyResponse icCancelPolicyResponse,DataTable dtquote)
        {
            Int32 Resp;
            OracleConnection conn = new OracleConnection();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = icCancelPolicyRequest.RequestReferenceNo;
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Int64).Value = icCancelPolicyRequest.QuoteReferenceNo;
                        cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = icCancelPolicyRequest.PolicyNumber;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Int64).Value = icCancelPolicyRequest.PolicyReferenceNo;
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Int64).Value = icCancelPolicyRequest.LessorID;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Int64).Value = icCancelPolicyRequest.LesseeID;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = icCancelPolicyRequest.CancellationReason;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = ErrorMsg.Count > 0 ? "IcCancelPolicyFailed" : "IcCancelPolicy";
                        cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["AggregatorName"].ToString();
                        cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = JsonConvert.SerializeObject(icCancelPolicyRequest);
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = icCancelPolicyRequest.VehicleSequenceNumber;
                        cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = icCancelPolicyRequest.VehicleCustomID;
                        cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_MOBILENO", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = Convert.ToInt32(dtquote.Rows[0]["DOCUMENTID"]);
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = ConfigurationManager.AppSettings["UpdatedBy"].ToString();
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = icCancelPolicyRequest.CancellationRequestTime;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASONID", OracleDbType.Int32).Value = 7;
                        cmd.Parameters.Add("P_QUERYFEATUREREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUERYFEATURERESPONSEJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASEREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASERESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYREQJSON", OracleDbType.Clob).Value = icCancelPolicyRequest == null ? DBNull.Value : (object)JsonConvert.SerializeObject(icCancelPolicyRequest); 
                        cmd.Parameters.Add("P_ICCANCELPOLICYRESJSON", OracleDbType.Clob).Value = icCancelPolicyResponse == null ? DBNull.Value : (object)JsonConvert.SerializeObject(icCancelPolicyResponse);
                        cmd.Parameters.Add("P_ICUPDATEPOLICYREQJSON", OracleDbType.Clob).Value =  DBNull.Value ;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYRESPJSON", OracleDbType.Clob).Value =  DBNull.Value ;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        Resp = cmd.ExecuteNonQuery();
                        return Resp == -1 ? "Success" : "request_failed";

                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, icCancelPolicyRequest.RequestReferenceNo, icCancelPolicyRequest.QuoteReferenceNo.ToString(), "CancelPolicy");
                SaveErrorResp = SaveErrorDetails("1", icCancelPolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icCancelPolicyRequest), ex.ToString(), "CancelPolicy", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                return "unexpected_error";
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public DataTable Existingdatachecking(int Flag,string QueryRequestReferenceNo,long QueryResponseReferenceNo)
        {
            Int32 Resp;
            DataTable dataTable = new DataTable();
            OracleConnection conn = new OracleConnection();
            DataTable dtDocumentNo = new DataTable();
            try
            {
                using (conn = new OracleConnection(Constant.OracleDB))
                {
                    using (OracleCommand cmd = new OracleCommand(Constant.UpdatePolicyDetails))
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("P_FLAG", OracleDbType.Varchar2).Value = Flag;
                        cmd.Parameters.Add("P_REQTREFERENCENO", OracleDbType.Varchar2).Value = QueryRequestReferenceNo;
                        cmd.Parameters.Add("P_QUOTEREFERENCENO", OracleDbType.Int64).Value = QueryResponseReferenceNo;
                        cmd.Parameters.Add("P_POLICYNUMBER", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLICYREFERENCENO", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_LESSORID", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_LESSEEID", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASON", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_STATUS", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_AGGREGATOR", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLUPDATEREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PREMRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_NATIONALADDRESS", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLESEQUENCENUMBER", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLECUSTOMID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLEPLATETYPEID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_VEHICLEPLATENUMBER", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_FIRSTPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_SECONDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_THIRDPLATELETTERID", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_MOBILENO", OracleDbType.Int64).Value = DBNull.Value;
                        cmd.Parameters.Add("P_DOCUMENTID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CREATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEDBY", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTREQTJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_POLCANCELDATE", OracleDbType.Varchar2).Value = DBNull.Value;
                        cmd.Parameters.Add("P_UPDATEPOLICYREASONID", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("P_QUERYFEATUREREQJSON", OracleDbType.Clob).Value = 5;
                        cmd.Parameters.Add("P_QUERYFEATURERESPONSEJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASEREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_PURCHASERESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ADDDRIVERRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_CONFIRMPOLUPDTRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICCANCELPOLICYRESJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYREQJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_ICUPDATEPOLICYRESPJSON", OracleDbType.Clob).Value = DBNull.Value;
                        cmd.Parameters.Add("P_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        OracleDataReader dr = cmd.ExecuteReader();
                        dataTable.Load(dr);


                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, QueryRequestReferenceNo.ToString(), string.Empty, "UpdtPolicyDtails");
                SaveErrorResp = SaveErrorDetails("1", QueryRequestReferenceNo.ToString(), string.Empty, ex.ToString(), "UpdtPolicyDtails", "DataAccessLayer", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return dataTable;
        }
    }
}