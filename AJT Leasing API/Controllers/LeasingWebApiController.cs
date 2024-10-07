using AJT_Leasing_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using System.Web.Http.Cors;
using System.IO;
using System.Configuration;
using System.Web.Globalization;
using static AJT_Leasing_API.Models.ConfirmAddDriverResponse;

namespace AJT_Leasing_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LeasingWebApiController : ApiController
    {
        AddDriverResponse addDriverResponse = new AddDriverResponse();
        QuoteResponse quoteResponse = new QuoteResponse();
        DataAccessLayer dataAccessLayer = new DataAccessLayer();
        CommonValidation validation = new CommonValidation();
        DataTable dtquoteDetails = new DataTable();
        string PNUpdatedResp = string.Empty;
        string SavePolDetails = string.Empty;
        ErrorDetails ErrorDetails = new ErrorDetails();
        Quotes quotes = new Quotes();
        TrfProcedureDetails tariffInput = new TrfProcedureDetails();
        Error error = new Error();
        //PremiumBreakdown[] premiumBreakdownsList;
        PolicyResponse policyResponse = new PolicyResponse();
        //IcUpdatePolicyResponse icUpdatePolicyResponse = new IcUpdatePolicyResponse();
        //IcCancelPolicyRequest icCancelPolicy = new IcCancelPolicyRequest();
        //IcCancelPolicyResponse icCancelPolicyResp = new IcCancelPolicyResponse();
        //DataTable dtTransformVal = new DataTable();
        List<Error> ErrorMsg = new List<Error>();
        //DriverBrtDetails[] DriverBrtDetails;
        DataTable dtPremDetails = new DataTable();
        //int BodyType = 0;
        //decimal PolAmt = 0, PolTaxableAmt = 0, DiscountAmt, BasePremium = 0;
        string quoteSaveResp = string.Empty;
        //string SaveUpdtPolDetails = string.Empty;
        //bool IsPremCalculated = false, IsQuotationStop = false;
        DateTime? drvrDob;
        //DataTable dtDiscountPerct = new DataTable();
        //DataTable dtDiscountAmt = new DataTable();
        ////DataTable dtScheme = new DataTable();
        //string EskaNationality = string.Empty;
        //string Source = "01";
        //string PlateNoA, PlateNoB, PlateNoC;
        //Changes start by 27-04-2021 New Parameter
        string SaveErrorResp = string.Empty;
        //Changes start by 27-04-2021 New Parameter
        StaticValues staticValues = new StaticValues()  ;
        //QueryFeatureRequest queryfeaturerequest = new QueryFeatureRequest();
        //QuoteDetails quoteDetails = new QuoteDetails();
        ////[Route("TameeniApi/TameeniWebApi/Quote")]
        //PolicyIssueRequest policyIssueRequest = new PolicyIssueRequest();
        //QueryFeatureResponse queryFeatureResponse = new QueryFeatureResponse();
        //ConfirmAddDriverResponse confirmAddDriverResponse = new ConfirmAddDriverResponse();

        string Resp;
        string SaveDriver = string.Empty;


        [Route("LeasingApi/LeasingWebApi/Quote")]
        [HttpPost]
        public IHttpActionResult Quote([FromBody] QuoteDetails quoteDetails)
        {
            try
            {
                if (quoteDetails != null && quoteDetails.Details != null)
                {
                    ErrHandler.WriteLog("Execution Start Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), quoteDetails.RequestReferenceNo);
                    string file = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), ConfigurationManager.AppSettings["StaticFile"].ToString()));
                    staticValues = JsonConvert.DeserializeObject<StaticValues>(file);
                    staticValues.Aggregator = "AJTLEASE";
                    ErrorMsg = validation.Validation(quoteDetails);
                    if (ErrorMsg.Count <= 0)
                    {
                        dtPremDetails = dataAccessLayer.GetQuotePremDetails(quotes.PreptrfDetails(quoteDetails, quoteDetails.Details.VehicleSumInsured, drvrDob, ref tariffInput));
                        if (dtPremDetails != null)
                        {
                            //quoteResponse = quote.PrepQuoteResp(dtPremDetails, quoteDetails, ErrorMsg, dtDiscountPerct, dtDiscountAmt, staticValues);
                            quoteResponse = quotes.PrepQuoteResp(dtPremDetails, quoteDetails, ErrorMsg, staticValues);
                            // quoteSaveResp = dataAccessLayer.SaveQuoteDetails(quoteDetails, quoteResponse, 1, dtPremDetails, quoteResponse == null ? quote.GenerateUniqueRefeNo() : quoteResponse.CompQuotes[0].QuoteReferenceNo, ErrorDetails, tariffInput);
                            quoteSaveResp = dataAccessLayer.SaveQuoteDetails(quoteDetails, quoteResponse, 1, dtPremDetails, 0, ErrorDetails, tariffInput);
                            if (quoteSaveResp == "request_failed" || quoteSaveResp == "duplicate_error" || quoteSaveResp == "unexpected_error")
                            {
                                switch (quoteSaveResp)
                                {
                                    case "request_failed":
                                        error.message = "Request failed. Contact Tameeni and share request details to investigate further.";
                                        break;
                                    case "duplicate_error":
                                        error.field = "RequestReferenceNo";
                                        error.message = "A field is expecting unique value between requests which founds duplicates.";
                                        break;
                                    case "unexpected_error":
                                        error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                                        break;
                                    default:
                                        break;
                                }
                                error.code = quoteSaveResp;
                                ErrorMsg.Add(error);
                                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, quoteDetails.RequestReferenceNo);
                                quoteResponse = null;
                                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), error.message, "Quote", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");

                            }
                            /*else
                            {

                              for(int i=0; i<quoteResponse.CompQuotes.Length; i++)
                                {
                                    quoteResponse.CompQuotes[i].QuoteReferenceNo += Convert.ToInt64(quoteSaveResp);
                                }
                                quoteResponse.CompQuotes = quoteResponse.CompQuotes;
                            }*/
                        }
                        else
                        {
                            error.code = "";
                            error.field = "Premiumdatacalling";
                            error.message = "premium data not found";
                            ErrorMsg.Add(error);
                            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, quoteDetails.RequestReferenceNo);
                            quoteResponse = null;
                        }

                    }
                    else
                    {
                        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, quoteDetails.RequestReferenceNo);
                        quoteResponse = null;
                        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), JsonConvert.SerializeObject(ErrorMsg), "Quote", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
                    }



                }
                else
                {
                    ErrorMsg.Add(new Error
                    {
                        code = "invalid_json",
                        field = "Client or Validation error",
                        message = "Incorrect Request Structure"
                    });
                }


            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, quoteDetails.RequestReferenceNo, string.Empty, "Quote");
                error.field = "";
                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                error.code = "unexpected_error";
                ErrorMsg.Add(error);
                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, quoteDetails.RequestReferenceNo.ToString());
                quoteResponse = null;
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), ex.ToString(), "Quote", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
                return Json(quoteResponse == null ? ErrorDetails : (object)quoteResponse);

            }
            return Json(quoteResponse == null ? ErrorDetails : (object)quoteResponse);
        }


        //[Route("TameeniApi/TameeniWebApi/Policy")]
        //[Route("TameeniApi/TameeniWebApi/QuoteSelected")]
        [Route("LeasingApi/LeasingWebApi/Policy")]
        [HttpPost]
        public IHttpActionResult Policy([FromBody] PolicyRequest policyRequest)
        {
            try
            {
                if (policyRequest != null)
                {
                    ErrHandler.WriteLog("Execution Start Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), policyRequest.RequestReferenceNo);
                    ErrorMsg = validation.ValidatePolicy(policyRequest);

                    if (ErrorMsg.Count > 0)
                    {
                        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, policyRequest.RequestReferenceNo.ToString());
                        policyResponse = null;
                    }
                    else
                    {
                        dtPremDetails = dataAccessLayer.GetQuoteDetails(2, policyRequest.RequestReferenceNo, policyRequest.QuoteReferenceNo.ToString(), null);
                        if (dtPremDetails != null && dtPremDetails.Rows.Count > 0)
                        {
                            if (dtPremDetails.Rows[0]["STATUS"].ToString() == "Created" || dtPremDetails.Rows[0]["STATUS"].ToString() == "SELECTED")
                            {
                                policyResponse = quotes.PrepPolicyResposne(policyRequest, ErrorMsg, true);
                            }
                            else if (dtPremDetails.Rows[0]["STATUS"].ToString() == "FAILED")
                            {
                                error.field = "";
                                error.message = "Quotation not created successfully.";
                                error.code = "QUOTESELECTED";
                                ErrorMsg.Add(error);
                                policyResponse = quotes.PrepPolicyResposne(policyRequest, ErrorMsg, false);
                            }
                            else if (dtPremDetails.Rows[0]["STATUS"].ToString() == "PolPurchased")
                            {
                                ErrorDetails.Status = true;
                                error.field = "";
                                error.message = "Pol already purchased.Contact Tameeni and share request details to investigate further";
                                error.code = "POLPURCHASED";
                                ErrorMsg.Add(error);
                                policyResponse = quotes.PrepPolicyResposne(policyRequest, ErrorMsg, true);
                            }
                            else
                            {
                                error.field = "";
                                error.message = "Quote Details not found.";
                                error.code = "policy";
                                ErrorMsg.Add(error);
                                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, policyRequest.RequestReferenceNo.ToString());
                                policyResponse = null;
                                SaveErrorResp = this.dataAccessLayer.SaveErrorDetails("1", policyRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyRequest), error.message, "Policy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
                            }
                        }
                        else
                        {
                            error.field = "";
                            error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                            error.code = "unexpected_error";
                            ErrorMsg.Add(error);
                            ErrHandler.WriteLog("Quote Details not found", "Quote details not found against Policy Request Reference No: " + policyRequest.PolicyRequestReferenceNo, policyRequest.PolicyRequestReferenceNo);
                            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, policyRequest.RequestReferenceNo.ToString());
                            policyResponse = null;
                            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyRequest), JsonConvert.SerializeObject(ErrorDetails), "Policy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
                        }
                        quoteSaveResp = dataAccessLayer.UpdateQuoteDetails(3, policyRequest, policyResponse, ErrorDetails);
                        if (quoteSaveResp != "Success")
                        {
                            switch (quoteSaveResp)
                            {
                                case "request_failed":
                                    error.message = "Request failed. Contact Tameeni and share request details to investigate further.";
                                    break;
                                case "unexpected_error":
                                    error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                                    break;
                                default:
                                    break;
                            }
                            error.code = quoteSaveResp;
                            ErrorMsg.Add(error);
                            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, policyRequest.RequestReferenceNo.ToString());
                            policyResponse = null;
                            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyRequest), error.message, "Policy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
                        }
                        ErrHandler.WriteLog("Execution Stop Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), policyRequest.RequestReferenceNo);

                    }

                }
                else
                {
                    error.field = "";
                    error.message = "JSON request is not well formed.";
                    error.code = "invalid_json";
                    ErrorMsg.Add(error);
                    ErrorDetails = quotes.PrepFailureResp(ErrorMsg, "");
                    policyResponse = null;
                    SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyRequest), error.message, "Policy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, policyRequest.PolicyRequestReferenceNo, policyRequest.QuoteReferenceNo.ToString(), "Policy");
                error.field = "";
                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                error.code = "unexpected_error";
                ErrorMsg.Add(error);
                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, policyRequest.RequestReferenceNo.ToString());
                policyResponse = null;
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyRequest), error.message, "Policy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
            }
            return Json(policyResponse == null ? ErrorDetails : (object)policyResponse);

        }

        [Route("LeasingApi/LeasingWebApi/PolicyIssue")]
        [HttpPost]
        public IHttpActionResult PolicyIssue([FromBody] PolicyIssueRequest policyIssueRequest)
        {
            List<Error> ErrorMsg = new List<Error>();
            PolicyIssueResponse policyIssueResponse = new PolicyIssueResponse();
            try
            {
                if (policyIssueRequest != null)
                {
                    ErrHandler.WriteLog("Execution Start Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), policyIssueRequest.RequestReferenceNo);
                    ErrorMsg = validation.ValidatePolicyIssue(policyIssueRequest);
                    if (ErrorMsg.Count > 0)
                    {
                        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, policyIssueRequest.RequestReferenceNo.ToString());
                        policyIssueResponse = null;
                    }
                    else
                    {
                        dtquoteDetails = dataAccessLayer.GetQuoteDetails(2, policyIssueRequest.RequestReferenceNo, policyIssueRequest.QuoteReferenceNo.ToString(), null);
                        if (dtquoteDetails.Rows.Count <= 0)
                        {
                            error.field = "";
                            error.message = "Records not found against Request Reference Number & Quote Reference Number";
                            error.code = "invalid_input";
                            ErrorMsg.Add(error);
                            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, policyIssueRequest.QuoteReferenceNo.ToString());
                            policyIssueResponse = null;
                            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), error.message, "PolicyIssue", "PolicyIssuanceController", "TameeniLeasing", "TameeniLeasing");
                        }
                        else if (dtquoteDetails.Rows[0]["STATUS"].ToString() == "Created" || dtquoteDetails.Rows[0]["STATUS"].ToString() == "SELECTED")
                        {
                            policyIssueResponse = quotes.PrepPolicyIssueResp(ErrorMsg, policyIssueRequest);
                            PNUpdatedResp = dataAccessLayer.UpdateQuoteDetails(5, policyIssueRequest, policyIssueResponse);
                            if (PNUpdatedResp != "Success")
                            {
                                switch (PNUpdatedResp)
                                {
                                    case "request_failed":
                                        error.message = "Request failed. Contact Tameeni and share request details to investigate further.";
                                        break;
                                    case "unexpected_error":
                                        error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                                        break;
                                    default:
                                        break;
                                }
                                error.field = "";
                                error.code = PNUpdatedResp;
                                ErrorMsg.Add(error);
                                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, policyIssueRequest.QuoteReferenceNo.ToString());
                                policyIssueResponse = null;
                                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), error.message, "PolicyIssue", "PolicyIssuanceController", "TameeniLeasing", "TameeniLeasing");
                                
                            }
                            else
                            {
                                quoteResponse = JsonConvert.DeserializeObject<QuoteResponse>(dtquoteDetails.Rows[0]["QUOTERESPJSON"].ToString());
                                policyResponse = JsonConvert.DeserializeObject<PolicyResponse>(dtquoteDetails.Rows[0]["POLICYRESPJSON"].ToString());
                                SavePolDetails = dataAccessLayer.SavePolDetails(1, quotes.PrepPolDetails(policyIssueRequest, quoteResponse, policyResponse));
                                if (SavePolDetails != "Success")
                                {
                                    switch (SavePolDetails)
                                    {
                                        case "request_failed":
                                            error.message = "Request failed. Contact Tameeni and share request details to investigate further.";
                                            break;
                                        case "duplicate_error":
                                            error.field = "RequestReferenceNo";
                                            error.message = "A field is expecting unique value between requests which founds duplicates.";
                                            break;
                                        case "unexpected_error":
                                            error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                                            break;
                                        default:
                                            break;
                                    }
                                    error.code = SavePolDetails;
                                    ErrorMsg.Add(error);
                                    ErrorDetails = quotes.PrepFailureResp(ErrorMsg, policyIssueRequest.QuoteReferenceNo.ToString());
                                    policyIssueResponse = null;
                                    SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), error.message, "PolicyIssue", "PolicyIssuanceController", "TameeniLeasing", "TameeniLeasing");
                                }
                            }
                        }
                        else
                        {
                            error.field = "";
                            error.message = "Already processed purchase request against the request reference number & quote reference number";
                            error.code = "invalid_input";
                            ErrorMsg.Add(error);
                            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, policyIssueRequest.QuoteReferenceNo.ToString());
                            policyIssueResponse = null;
                            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), error.message, "PolicyIssue", "PolicyIssuanceController", "TameeniLeasing", "TameeniLeasing");
                        }
                    }
                    ErrHandler.WriteLog("Execution Stop Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), policyIssueRequest.RequestReferenceNo);
                }
                else
                {
                    error.field = "";
                    error.message = "JSON request is not well formed.";
                    error.code = "invalid_json";
                    ErrorMsg.Add(error);
                    ErrorDetails = quotes.PrepFailureResp(ErrorMsg, "");
                    policyIssueResponse = null;
                    SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), error.message, "PolicyIssue", "PolicyIssuanceController", "TameeniLeasing", "TameeniLeasing");
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, string.Empty, policyIssueRequest.QuoteReferenceNo.ToString(), "PolicyIssue");
                error.field = "";
                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                error.code = "unexpected_error";
                ErrorMsg.Add(error);
                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, policyIssueRequest.QuoteReferenceNo.ToString());
                policyIssueResponse = null;
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), ex.ToString(), "PolicyIssue", "PolicyIssuanceController", "TameeniLeasing", "TameeniLeasing");
            }
            return Json(policyIssueResponse == null ? ErrorDetails : (object)policyIssueResponse);
        }


        //#region Addfeature
        //[Route("AJT_Leasing_API/IHCWebApi/QueryFeature")]
        //[HttpPost]
        //public IHttpActionResult QueryFeature([FromBody] QueryFeatureRequest queryFeatureRequest)
        //{
        //    try
        //    {
        //        if (queryFeatureRequest != null)
        //        {
        //            ErrHandler.WriteLog("Execution Start Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), "QueryFeature Meyhod");
        //            string file = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), ConfigurationManager.AppSettings["StaticFile"].ToString()));
        //            staticValues = JsonConvert.DeserializeObject<StaticValues>(file);
        //            ErrorMsg = validation.Validation(queryFeatureRequest);
        //            if (ErrorMsg.Count <= 0)
        //            {
        //                dtquoteDetails = dataAccessLayer.GetQuotationDetails(10, queryFeatureRequest.PolicyReferenceNo.ToString());
        //                if (dtquoteDetails != null && dtquoteDetails.Rows.Count > 0)
        //                {
        //                    quoteDetails = JsonConvert.DeserializeObject<QuoteDetails>(dtquoteDetails.Rows[0]["QuoteDetails"].ToString());
        //                    policyIssueRequest = JsonConvert.DeserializeObject<PolicyIssueRequest>(dtquoteDetails.Rows[0]["PNREQTJSON"].ToString());

        //                    queryFeatureResponse = quotes.PrepQueryFeatureResp(queryFeatureRequest, quoteDetails, policyIssueRequest, ErrorMsg, staticValues);
        //                    if (queryFeatureResponse.PolicyPremiumFeatures.Length <= 0)
        //                    {
        //                        error.field = "";
        //                        error.message = "There are no covers available for provided Request";
        //                        error.code = "";
        //                        ErrorMsg.Add(error);
        //                        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, queryFeatureRequest.QueryRequestReferenceNo.ToString());
        //                        queryFeatureResponse = null;
        //                        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", queryFeatureRequest.QueryRequestReferenceNo.ToString(), JsonConvert.SerializeObject(queryFeatureRequest), error.message, "QueryFeature", "AddFeatureController", "TameeniLeasing", "TameeniLeasing");
        //                    }
        //                    else
        //                    {
        //                        Resp = dataAccessLayer.SaveUpdtPolicyDtails(1, ErrorMsg, queryFeatureRequest, tariffInput, dtPremDetails, 5, queryFeatureResponse, dtquoteDetails);
        //                        ErrHandler.WriteLog("Execution Stop Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), queryFeatureRequest.QueryRequestReferenceNo.ToString());
        //                    }
                            
        //                }
        //                else
        //                {
        //                    error.field = "";
        //                    //error.message = "Error from Quote SP: " + dtPremDetails.Rows[0]["P_REASON"].ToString();
        //                    error.message = "Error from Quote SP:Quotation Details Not Found";
        //                    error.code = "";
        //                    ErrorMsg.Add(error);
        //                    ErrorDetails = quotes.PrepFailureResp(ErrorMsg, queryFeatureRequest.QueryRequestReferenceNo.ToString());
        //                    queryFeatureResponse = null;
        //                }

                       
                        

        //            }
        //            else
        //            {
        //                ErrHandler.WriteLog("Quote Details not found", "Quote details not found against Policy Reference No: " + queryFeatureRequest.PolicyReferenceNo, queryFeatureRequest.QueryRequestReferenceNo.ToString());
        //                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, queryFeatureRequest.QueryRequestReferenceNo.ToString());
        //                queryFeatureResponse = null;
        //                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", queryFeatureRequest.QueryRequestReferenceNo.ToString(), JsonConvert.SerializeObject(queryFeatureRequest), error.message, "QueryFeature", "AddFeatureController", "TameeniLeasing", "TameeniLeasing");
        //            }
        //        }
        //        else
        //        {
        //            error.field = "";
        //            error.message = "Json Request is not well formed";
        //            error.code = "invalid_json";
        //            ErrorMsg.Add(error);
        //            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, queryFeatureRequest.QueryRequestReferenceNo.ToString());
        //            quoteResponse = null;
        //            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", queryFeatureRequest.QueryRequestReferenceNo.ToString(), JsonConvert.SerializeObject(queryFeatureRequest), JsonConvert.SerializeObject(ErrorMsg), "Quote", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        ErrHandler.WriteError(ex, queryFeatureRequest.QueryRequestReferenceNo.ToString(), string.Empty, "QueryFeature");
        //        error.field = "";
        //        error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        error.code = "unexpected_error";
        //        ErrorMsg.Add(error);
        //        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, queryFeatureRequest.QueryRequestReferenceNo.ToString());
        //        queryFeatureResponse = null;
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", queryFeatureRequest.QueryRequestReferenceNo.ToString(), JsonConvert.SerializeObject(queryFeatureRequest), ex.ToString(), "QueryFeature", "AddFeatureController", "TameeniLeasing", "TameeniLeasing");

        //    }
        //    return Json(queryFeatureResponse == null ? ErrorDetails : (object)queryFeatureResponse);
        //}

        //[Route("AJT_Leasing_API/IHCWebApi/PurchaseFeature")]
        //[HttpPost]
        //public IHttpActionResult PurchaseFeature([FromBody] PurchaseFeatureRequest purchaseFeatureRequest)
        //{
        //    PurchaseFeatureResponse purchaseFeatureResponse = new PurchaseFeatureResponse();
        //    DataTable queryfeaturedata = new DataTable();
        //    try
        //    {
        //        if (purchaseFeatureRequest != null)
        //        {
        //            ErrHandler.WriteLog("PurchaseFeature method", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), purchaseFeatureRequest.QueryRequestReferenceNo.ToString());
        //            ErrorMsg = validation.ValidatePurchaseFeature(purchaseFeatureRequest);
        //            if (ErrorMsg.Count > 0)
        //            {
        //                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, purchaseFeatureRequest.QueryRequestReferenceNo.ToString());
        //                purchaseFeatureResponse = null;
        //            }
        //            else
        //            {
        //                purchaseFeatureResponse = quotes.PrepPurchaseFeatureResp(purchaseFeatureRequest, ErrorMsg);

        //                queryfeaturedata = dataAccessLayer.Existingdatachecking(2,purchaseFeatureRequest.QueryRequestReferenceNo.ToString(), purchaseFeatureRequest.QueryResponseReferenceNo);
        //                if(queryfeaturedata != null && queryfeaturedata.Rows.Count> 0)
        //                {
        //                    Resp = dataAccessLayer.UpdtPolicyDtails(3, ErrorMsg, purchaseFeatureRequest, 5, purchaseFeatureResponse);
        //                    if (Resp != "Success")
        //                    {
        //                        switch (Resp)
        //                        {
        //                            case "request_failed":
        //                                error.message = "Request failed. Contact Tameeni and share request details to investigate further.";
        //                                break;
        //                            case "unexpected_error":
        //                                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //                                break;
        //                            default:
        //                                break;
        //                        }
        //                        error.code = Resp;
        //                        ErrorMsg.Add(error);
        //                        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, purchaseFeatureRequest.QueryRequestReferenceNo.ToString());
        //                        purchaseFeatureResponse = null;
        //                        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", purchaseFeatureRequest.QueryRequestReferenceNo.ToString(), JsonConvert.SerializeObject(purchaseFeatureRequest), error.message, "PurchaseFeature", "AddFeatureController", "TameeniLeasing", "TameeniLeasing");
        //                    }
        //                }
        //                else
        //                {

        //                    error.code = "";
        //                    error.message = "quation not existed please check";
        //                    error.field = "";
        //                    ErrorMsg.Add(error);
        //                    ErrorDetails = quotes.PrepFailureResp(ErrorMsg, purchaseFeatureRequest.QueryRequestReferenceNo.ToString());
        //                    purchaseFeatureResponse = null;
        //                    SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", purchaseFeatureRequest.QueryRequestReferenceNo.ToString(), JsonConvert.SerializeObject(purchaseFeatureRequest), error.message, "PurchaseFeature", "AddFeatureController", "TameeniLeasing", "TameeniLeasing");
        //                }
                       

                        
        //                ErrHandler.WriteLog("Execution Stop Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), purchaseFeatureRequest.QueryRequestReferenceNo.ToString());
        //            }
        //        }
        //        else
        //        {
        //            error.field = "";
        //            error.message = "JSON request is not well formed.";
        //            error.code = "invalid_json";
        //            ErrorMsg.Add(error);
        //            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, "");
        //            purchaseFeatureResponse = null;
        //            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", purchaseFeatureRequest.QueryRequestReferenceNo.ToString(), JsonConvert.SerializeObject(purchaseFeatureRequest), error.message, "PurchaseFeature", "AddFeatureController", "TameeniLeasing", "TameeniLeasing");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, purchaseFeatureRequest.QueryRequestReferenceNo.ToString(), string.Empty, "PurchaseFeature");
        //        error.field = "";
        //        error.message = "An Unexpected error occurred from server. Contact IHC and share request details to investigate further.";
        //        error.code = "unexpected_error";
        //        ErrorMsg.Add(error);
        //        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, purchaseFeatureRequest.QueryRequestReferenceNo.ToString());
        //        purchaseFeatureResponse = null;
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", purchaseFeatureRequest.QueryRequestReferenceNo.ToString(), JsonConvert.SerializeObject(purchaseFeatureRequest), ex.ToString(), "PurchaseFeature", "AddFeatureController", "TameeniLeasing", "TameeniLeasing");

        //    }
        //    return Json(purchaseFeatureResponse == null ? ErrorDetails : (object)purchaseFeatureResponse);
        //}

        //#endregion  Addfeature


        //[Route("AJT_Leasing_API/IHCWebApi/AddDriver")]
        //[HttpPost]
        //public IHttpActionResult AddDriver([FromBody] AddDriverRequest addDriverRequest)
        //{
        //    try
        //    {
        //        List<Error> err = new List<Error>();
        //        if (addDriverRequest != null)
        //        {
        //            ErrHandler.WriteLog("addDriverRequest : ", "", addDriverRequest.AddDriverRequestReferenceNo.ToString());
        //            ErrorMsg = validation.ValidateAddDriver(addDriverRequest);
        //            if (ErrorMsg.Count > 0)
        //            {
        //                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, addDriverRequest.AddDriverRequestReferenceNo.ToString());
        //                addDriverResponse = null;

        //            }
        //            else
        //            {
        //                dtquoteDetails = dataAccessLayer.GetQuotationDetails(10, addDriverRequest.PolicyReferenceNo.ToString());
        //                if (dtquoteDetails!=null && dtquoteDetails.Rows.Count>0)
        //                {
        //                    addDriverResponse = quotes.PrepAddDriverResp(err, addDriverRequest);
        //                    SaveDriver = dataAccessLayer.SaveDriverDetails(1, addDriverRequest, ErrorMsg, 6, addDriverResponse, dtquoteDetails);
        //                    if (SaveDriver != "Success")
        //                    {
        //                        switch (SaveDriver)
        //                        {
        //                            case "request_failed":
        //                                error.message = "Request failed. Contact Tameeni and share request details to investigate further.";
        //                                break;
        //                            case "unexpected_error":
        //                                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //                                break;
        //                            default:
        //                                break;
        //                        }
        //                        error.code = SaveDriver;
        //                        ErrorMsg.Add(error);
        //                        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, addDriverRequest.AddDriverRequestReferenceNo.ToString());
        //                        addDriverResponse = null;
        //                        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", addDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(addDriverRequest), error.message, "AddDriver", "AddDriverController", "TameeniLeasing", "TameeniLeasing");
        //                    }
        //                }
        //                else
        //                {
        //                    error.field = "";
        //                    //error.message = "Error from Quote SP: " + dtPremDetails.Rows[0]["P_REASON"].ToString();
        //                    error.message = "PolicyReferenceNo Based Quotation Not Found";
        //                    error.code = "";
        //                    ErrorMsg.Add(error);
        //                    ErrorDetails = quotes.PrepFailureResp(ErrorMsg, addDriverRequest.PolicyReferenceNo.ToString());
        //                    queryFeatureResponse = null;
        //                }

        //                ErrHandler.WriteLog("Execution Stop Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), addDriverRequest.AddDriverRequestReferenceNo.ToString());

        //            }
        //        }
        //        else
        //        {
        //            error.field = "";
        //            error.message = "JSON request is not well formed.";
        //            error.code = "invalid_json";
        //            ErrorMsg.Add(error);
        //            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, "");
        //            confirmAddDriverResponse = null;
        //            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", addDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(addDriverRequest), error.message, "AddDriver", "AddDriverController", "TameeniLeasing", "TameeniLeasing");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, addDriverRequest.AddDriverRequestReferenceNo.ToString(), string.Empty, "AddDriver");
        //        error.field = "";
        //        error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        error.code = "unexpected_error";
        //        ErrorMsg.Add(error);
        //        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, addDriverRequest.AddDriverRequestReferenceNo.ToString());
        //        addDriverResponse = null;
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", addDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(addDriverRequest), ex.ToString(), "AddDriver", "AddDriverController", "TameeniLeasing", "TameeniLeasing");
        //    }
        //    return Json(addDriverResponse == null ? ErrorDetails : (object)addDriverResponse);
        //}

        //[Route("AJT_Leasing_API/IHCWebApi/ConfirmAddDriver")]
        //[HttpPost]
        //public IHttpActionResult ConfirmAddDriver([FromBody] ConfirmAddDriverRequest confirmAddDriverRequest)
        //{
        //    List<Error> ErrMsg = new List<Error>();
        //    DataTable dataTable = new DataTable();
        //    try
        //    {
        //        if (confirmAddDriverRequest != null)
        //        {
        //            ErrHandler.WriteLog("Execution Start Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString());
        //            ErrorMsg = validation.ValidateConfirmAddDriver(confirmAddDriverRequest);
        //            if (ErrorMsg.Count > 0)
        //            {
        //                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString());
        //                confirmAddDriverResponse = null;
        //            }
        //            else
        //            {

        //                confirmAddDriverResponse = quotes.PrepConfirmAddDriverResp(ErrMsg, confirmAddDriverRequest);
        //                dataTable = dataAccessLayer.Existingdatachecking(2, confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), Convert.ToInt64(confirmAddDriverRequest.AddDriverResponseReferenceNo));
        //                if(dataTable != null && dataTable.Rows.Count > 0)
        //                {
        //                    SaveDriver = dataAccessLayer.SaveConfirmDriverDetails(4, confirmAddDriverRequest, ErrorMsg, 6, confirmAddDriverResponse);
        //                    if (SaveDriver != "Success")
        //                    {
        //                        switch (SaveDriver)
        //                        {
        //                            case "request_failed":
        //                                error.message = "Request failed. Contact Tameeni and share request details to investigate further.";
        //                                break;
        //                            case "unexpected_error":
        //                                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //                                break;
        //                            default:
        //                                break;
        //                        }
        //                        error.code = SaveDriver;
        //                        ErrorMsg.Add(error);
        //                        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString());
        //                        confirmAddDriverResponse = null;
        //                        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(confirmAddDriverRequest), error.message, "ConfirmAddDriver", "AddDriverController", "TameeniLeasing", "TameeniLeasing");
        //                    }
        //                }
        //                else
        //                {
        //                    error.code = "";
        //                    error.message = "driver not existed please check";
        //                    error.field = "";
        //                    ErrorMsg.Add(error);
        //                    ErrorDetails = quotes.PrepFailureResp(ErrorMsg, confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString());
        //                    confirmAddDriverResponse = null;
        //                    SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(confirmAddDriverRequest), error.message, "PurchaseFeature", "AddFeatureController", "TameeniLeasing", "TameeniLeasing");
        //                }

                        
                        
        //                ErrHandler.WriteLog("Execution Stop Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString());
        //            }
        //        }
        //        else
        //        {
        //            error.field = "";
        //            error.message = "JSON request is not well formed.";
        //            error.code = "invalid_json";
        //            ErrorMsg.Add(error);
        //            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, "");
        //            confirmAddDriverResponse = null;
        //            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(confirmAddDriverRequest), error.message, "ConfirmAddDriver", "AddDriverController", "TameeniLeasing", "TameeniLeasing");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), string.Empty, "AddDriver");
        //        error.field = "";
        //        error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        error.code = "unexpected_error";
        //        ErrorMsg.Add(error);
        //        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString());
        //        confirmAddDriverResponse = null;
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(confirmAddDriverRequest), ex.ToString(), "ConfirmAddDriver", "AddDriverController", "TameeniLeasing", "TameeniLeasing");

        //    }
        //    return Json(confirmAddDriverResponse == null ? ErrorDetails : (object)confirmAddDriverResponse);
        //}

        //[Route("AJT_Leasing_API/IHCWebApi/ICUpdatePolicy")]
        //[HttpPost]
        //public IHttpActionResult ICUpdatePolicy([FromBody] IcUpdatePolicyRequest icUpdatePolicyRequest)
        //{
        //    IcUpdatePolicyResponse icUpdatePolicyResponse = new IcUpdatePolicyResponse();
        //    List<Error> ErrMsg = new List<Error>();
        //    try
        //    {
        //        if (icUpdatePolicyRequest != null)
        //        {
        //            ErrHandler.WriteLog("Execution Start Time : ", "ICUpdatePolicy Method", icUpdatePolicyRequest.RequestReferenceNo);
        //            ErrorMsg = validation.ValidateIcUpdatePolicy(icUpdatePolicyRequest);
        //            if (ErrorMsg.Count > 0)
        //            {
        //                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, icUpdatePolicyRequest.RequestReferenceNo.ToString());
        //                icUpdatePolicyResponse = null;
        //            }
        //            else
        //            {
        //                dtquoteDetails = dataAccessLayer.GetQuotationDetails(10, icUpdatePolicyRequest.PolicyReferenceNo.ToString());
        //                if (dtquoteDetails!=null&& dtquoteDetails.Rows.Count>0)
        //                {
        //                    if (icUpdatePolicyRequest.UpdatePolicyReasonID == 2 || icUpdatePolicyRequest.UpdatePolicyReasonID == 3)
        //                    {
        //                        dtTransformVal = dataAccessLayer.GetTransformData(quotes.PrepTransformDetails(5, Source, icUpdatePolicyRequest.FirstPlateLetterID.ToString(), "T_PLATECHAR"));
        //                        try
        //                        {
        //                            PlateNoA = dtTransformVal.Rows[0]["MDM_CORE_CODE"].ToString();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            error.field = "T_PLATECHAR(PlateNoA)";
        //                            error.message = "An Unexpected error occurred from server. Contact Elite and share request details to investigate further.";
        //                            error.code = "unexpected_error";
        //                            ErrHandler.WriteLog("\nTransformation not found: ", error.field, icUpdatePolicyRequest.PolicyNumber);
        //                            //SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), ex.ToString(), "ICUpdatePolicy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
        //                            //throw;
        //                        }
        //                        dtTransformVal = dataAccessLayer.GetTransformData(quotes.PrepTransformDetails(5, Source, icUpdatePolicyRequest.SecondPlateLetterID.ToString(), "T_PLATECHAR"));
        //                        try
        //                        {
        //                            PlateNoB = dtTransformVal.Rows[0]["MDM_CORE_CODE"].ToString();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            error.field = "T_PLATECHAR(PlateNoB)";
        //                            error.message = "An Unexpected error occurred from server. Contact Elite and share request details to investigate further.";
        //                            error.code = "unexpected_error";
        //                            ErrHandler.WriteLog("\nTransformation not found: ", error.field, icUpdatePolicyRequest.PolicyNumber);
        //                           // SaveErrorResp = this.dataAccessLayer.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), ex.ToString(), "ICUpdatePolicy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
        //                           // throw;
        //                        }
        //                        dtTransformVal = dataAccessLayer.GetTransformData(quotes.PrepTransformDetails(5, Source, icUpdatePolicyRequest.ThirdPlateLetterID.ToString(), "T_PLATECHAR"));
        //                        try
        //                        {
        //                            PlateNoC = dtTransformVal.Rows[0]["MDM_CORE_CODE"].ToString();
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            error.field = "T_PLATECHAR(PlateNoC)";
        //                            error.message = "An Unexpected error occurred from server. Contact Elite and share request details to investigate further.";
        //                            error.code = "unexpected_error";
        //                            ErrHandler.WriteLog("\nTransformation not found: ", error.field, icUpdatePolicyRequest.PolicyNumber);
        //                            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), ex.ToString(), "ICUpdatePolicy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
        //                            //throw;
        //                        }

        //                        icUpdatePolicyResponse = quotes.PrepIcupdtPolicyResp(ErrMsg, icUpdatePolicyRequest);
        //                        SaveUpdtPolDetails = dataAccessLayer.SaveUpdtPolicyDetails(1, icUpdatePolicyRequest, ErrorMsg, 1, PlateNoA, PlateNoB, PlateNoC, icUpdatePolicyResponse, dtquoteDetails);
        //                        if (SaveUpdtPolDetails != "Success")
        //                        {
        //                            switch (SaveUpdtPolDetails)
        //                            {
        //                                case "request_failed":
        //                                    error.message = "Request failed. Contact Tameeni and share request details to investigate further.";
        //                                    break;
        //                                case "unexpected_error":
        //                                    error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //                                    break;
        //                                case "duplicate_error":
        //                                    error.message = "A field is expecting unique value between requests which founds duplicates.";
        //                                    break;
        //                                case "Data_Not_Found":
        //                                    error.message = "A data not found against Provided Request.";
        //                                    break;
        //                                default:
        //                                    break;
        //                            }
        //                            error.code = SaveUpdtPolDetails;
        //                            ErrorMsg.Add(error);
        //                            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, icUpdatePolicyRequest.RequestReferenceNo.ToString());
        //                            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), error.message, "ICUpdatePolicy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
        //                            icUpdatePolicyResponse = null;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        icUpdatePolicyResponse = quotes.PrepIcupdtPolicyResp(ErrMsg, icUpdatePolicyRequest);
        //                        SaveUpdtPolDetails = dataAccessLayer.SaveUpdtPolicyDetails(1, icUpdatePolicyRequest, ErrorMsg, 1, PlateNoA, PlateNoB, PlateNoC, icUpdatePolicyResponse, dtquoteDetails);
        //                        if (SaveUpdtPolDetails != "Success")
        //                        {
        //                            switch (SaveUpdtPolDetails)
        //                            {
        //                                case "request_failed":
        //                                    error.message = "Request failed. Contact Tameeni and share request details to investigate further.";
        //                                    break;
        //                                case "unexpected_error":
        //                                    error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //                                    break;
        //                                case "duplicate_error":
        //                                    error.message = "A field is expecting unique value between requests which founds duplicates.";
        //                                    break;
        //                                case "Data_Not_Found":
        //                                    error.message = "A data not found against Provided Request.";
        //                                    break;
        //                                default:
        //                                    break;
        //                            }
        //                            error.code = SaveUpdtPolDetails;
        //                            ErrorMsg.Add(error);
        //                            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, icUpdatePolicyRequest.RequestReferenceNo.ToString());
        //                            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), error.message, "ICUpdatePolicy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
        //                            icUpdatePolicyResponse = null;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    error.field = "";
        //                    //error.message = "Error from Quote SP: " + dtPremDetails.Rows[0]["P_REASON"].ToString();
        //                    error.message = "PolicyReferenceNo Based Quotation Not Found";
        //                    error.code = "";
        //                    ErrorMsg.Add(error);
        //                    ErrorDetails = quotes.PrepFailureResp(ErrorMsg, icUpdatePolicyRequest.PolicyReferenceNo.ToString());
        //                    queryFeatureResponse = null;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            error.field = "";
        //            error.message = "JSON request is not well formed.";
        //            error.code = "invalid_json";
        //            ErrorMsg.Add(error);
        //            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, "");
        //            icUpdatePolicyResponse = null;
        //            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), error.message, "ICUpdatePolicy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, icUpdatePolicyRequest.RequestReferenceNo, icUpdatePolicyRequest.QuoteReferenceNo.ToString(), "ICUpdatePolicy");
        //        error.field = "";
        //        error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        error.code = "unexpected_error";
        //        ErrorMsg.Add(error);
        //        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, icUpdatePolicyRequest.RequestReferenceNo.ToString());
        //        icUpdatePolicyResponse = null;
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), ex.ToString(), "ICUpdatePolicy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");

        //    }
        //    return Json(icUpdatePolicyResponse == null ? ErrorDetails : (object)icUpdatePolicyResponse);
        //}

        //[Route("AJT_Leasing_API/IHCWebApi/IcCancelPolicy")]
        //[HttpPost]
        //public IHttpActionResult IcCancelPolicy([FromBody] IcCancelPolicyRequest icCancelPolicyRequest)
        //{
        //    string CancelPolicyResp;
        //    List<Error> ErrMsg = new List<Error>();
        //    IcCancelPolicyResponse icCancelPolicyResp = new IcCancelPolicyResponse();
        //    try
        //    {
        //        if (icCancelPolicyRequest != null)
        //        {
        //            ErrHandler.WriteLog("Execution Start Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), icCancelPolicyRequest.RequestReferenceNo);
        //            ErrorMsg = validation.ValidateCancelPolicy(icCancelPolicyRequest);
        //            if (ErrorMsg.Count > 0)
        //            {
        //                ErrorDetails = quotes.PrepFailureResp(ErrorMsg, icCancelPolicyRequest.RequestReferenceNo.ToString());
        //                icCancelPolicyResp = null;
        //            }
        //            else
        //            {
        //                DataTable dt = new DataTable();
                        

        //                dtquoteDetails = dataAccessLayer.GetQuotationDetails(10, icCancelPolicyRequest.PolicyReferenceNo.ToString());
        //                if(dtquoteDetails != null && dtquoteDetails.Rows.Count > 0)
        //                {
        //                    icCancelPolicyResp = quotes.PrepIcCancelPolicyResp(ErrMsg, icCancelPolicyRequest);
        //                    CancelPolicyResp = dataAccessLayer.CancelPolicy(1, icCancelPolicyRequest, ErrorMsg, 3, icCancelPolicyResp, dtquoteDetails);
        //                    if (CancelPolicyResp == "request_failed" || CancelPolicyResp == "duplicate_error" || CancelPolicyResp == "unexpected_error")
        //                    {
        //                        switch (CancelPolicyResp)
        //                        {
        //                            case "request_failed":
        //                                error.message = "Request failed. Contact Elite and share request details to investigate further.";
        //                                break;
        //                            case "duplicate_error":
        //                                error.field = "RequestReferenceNo";
        //                                error.message = "A field is expecting unique value between requests which founds duplicates.";
        //                                break;
        //                            case "unexpected_error":
        //                                error.message = "An Unexpected error occurred from server. Contact Elite and share request details to investigate further.";
        //                                break;
        //                            default:
        //                                break;
        //                        }
        //                        error.code = CancelPolicyResp;
        //                        ErrorMsg.Add(error);
        //                        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, icCancelPolicyRequest.RequestReferenceNo);
        //                        icCancelPolicyResp = null;
        //                        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icCancelPolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icCancelPolicyRequest), JsonConvert.SerializeObject(ErrorDetails), "IcCancelPolicy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");

        //                    }
        //                    else
        //                    {
        //                        //icCancelPolicyResp = quote.PrepIcCancelPolicyResp(ErrMsg, icCancelPolicyRequest);
        //                    }
        //                    ErrHandler.WriteLog("Execution Stop Time : ", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss.fff tt"), icCancelPolicyRequest.RequestReferenceNo);

                            
        //                }
        //                else
        //                {
        //                    error.field = "";
        //                    //error.message = "Error from Quote SP: " + dtPremDetails.Rows[0]["P_REASON"].ToString();
        //                    error.message = "PolicyReferenceNo Based Quotation Not Found";
        //                    error.code = "";
        //                    ErrorMsg.Add(error);
        //                    ErrorDetails = quotes.PrepFailureResp(ErrorMsg, icCancelPolicyRequest.PolicyReferenceNo.ToString());
        //                    queryFeatureResponse = null;
        //                }


        //            }
        //        }
        //        else
        //        {
        //            error.field = "";
        //            error.message = "JSON request is not well formed.";
        //            error.code = "invalid_json";
        //            ErrorMsg.Add(error);
        //            ErrorDetails = quotes.PrepFailureResp(ErrorMsg, "");
        //            icCancelPolicyResp = null;
        //            SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icCancelPolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icCancelPolicyRequest), error.message, "IcCancelPolicy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        ErrHandler.WriteError(ex, icCancelPolicyRequest.RequestReferenceNo, icCancelPolicyRequest.QuoteReferenceNo.ToString(), "IcCancelPolicy");
        //        error.field = "";
        //        error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        error.code = "unexpected_error";
        //        ErrorMsg.Add(error);
        //        ErrorDetails = quotes.PrepFailureResp(ErrorMsg, icCancelPolicyRequest.RequestReferenceNo.ToString());
        //        icCancelPolicyResp = null;
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icCancelPolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icCancelPolicyRequest), ex.ToString(), "IcCancelPolicy", "IHCWebApiController", "TameeniLeasing", "TameeniLeasing");
        //        return Ok(icCancelPolicyResp == null ? ErrorDetails : (object)icCancelPolicyResp);
        //    }
        //    return Json(icCancelPolicyResp == null ? ErrorDetails : (object)icCancelPolicyResp);
        //}

    }
}