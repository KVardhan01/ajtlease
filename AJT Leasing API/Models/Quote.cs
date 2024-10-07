using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using static AJT_Leasing_API.Models.ConfirmAddDriverResponse;

namespace AJT_Leasing_API.Models
{
    public class Quotes
    {
        Error error = new Error();
        List<Error> errorsList = new List<Error>();
        string quoteRefeNo = string.Empty;
        DataAccessLayer dataAccessLayer = new DataAccessLayer();
        //Changes start by 27-04-2021 New Parameter
        string SaveErrorResp = string.Empty;
        //Changes start by 27-04-2021 New Parameter
        Date date = new Date();
        decimal totaldiscountamount = 0;
        StaticValues staticValues = new StaticValues();
        internal ErrorDetails PrepFailureResp(List<Error> ErrorList, string RequestReferenceNo)
        {
            ErrorDetails errorDetails = new ErrorDetails();
            try
            {
                errorDetails.Status = false;
                errorDetails.Errors = ErrorList;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, RequestReferenceNo, string.Empty, "PrepFailureResp");
                error.field = "";
                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                error.code = "unexpected_error";
                errorsList.Add(error);
                errorDetails.Status = false;
                errorDetails.Errors = errorsList;
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", RequestReferenceNo, string.Empty, ex.ToString(), "PrepFailureResp", "Quote", "TameeniLeasing", "TameeniLeasing");
            }
            return errorDetails;
        }

        #region Get Transformation Value
        internal GetTransformationDetails PrepTransformDetails(int Flag, string Source, string SourceCode, string SourceType)
        {
            GetTransformationDetails objTransformDetl = new GetTransformationDetails();
            try
            {
                objTransformDetl.Flag = Flag;
                objTransformDetl.Source = Source;
                objTransformDetl.SourceCode = SourceCode;
                objTransformDetl.SourceType = SourceType;
                //objTransformDetl.ReferenceNo = ReferenceNo;
            }
            catch (Exception ex)
            {
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", string.Empty, string.Empty, ex.ToString(), "PrepTransformDetails", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            return objTransformDetl;
        }
        #endregion

        #region Stop Quotation
        internal StopQuoteEntity PrepStopQuoteDetails(int EskaBodyTyp, string EskaNationality, string City, int VehMgfYear, DateTime? DOB, int VehicleUsage)
        {
            StopQuoteEntity stopQuoteEntity = new StopQuoteEntity();
            try
            {
                stopQuoteEntity.BodyType = EskaBodyTyp;
                stopQuoteEntity.VehNationality = EskaNationality;
                stopQuoteEntity.City = City;
                stopQuoteEntity.SchemeCode = ConfigurationManager.AppSettings["SchemeCode"].ToString();
                stopQuoteEntity.VehMgfYear = VehMgfYear;
                stopQuoteEntity.DOB = DOB;
                stopQuoteEntity.VehUse = VehicleUsage;
            }
            catch (Exception ex)
            {
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", string.Empty, string.Empty, ex.ToString(), "PrepStopQuoteDetails", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            return stopQuoteEntity;
        }
        #endregion

        internal DateTime ConvertToGregorian(string date)
        {
            try
            {
                string[] splitdate = date.Split('-');
                if ((Convert.ToInt32(splitdate[1]) == 2 || Convert.ToInt32(splitdate[1]) == 4 || Convert.ToInt32(splitdate[1]) == 6 ||
                     Convert.ToInt32(splitdate[1]) == 8 || Convert.ToInt32(splitdate[1]) == 10 || Convert.ToInt32(splitdate[1]) == 12) && Convert.ToInt32(splitdate[0]) > 29)
                {
                    splitdate[0] = "29";
                }
                DateTime dt = new DateTime(Convert.ToInt32(splitdate[2]), Convert.ToInt32(splitdate[1]), Convert.ToInt32(splitdate[0]), new HijriCalendar());
                return dt;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, string.Empty, string.Empty, "ConvertToGregorian");
                throw ex;
            }
        }


        #region 'Prepare tarif Procedure Inputs'

        internal TrfProcedureDetails PreptrfDetails(QuoteDetails quoteDetails, int VehicleSumInsured, DateTime? drvrDob, ref TrfProcedureDetails tariffInput)

        {
            TrfProcedureDetails trfProcedureDetails = new TrfProcedureDetails();
            TariffDriverDetails[] TariffDriverDetails = new TariffDriverDetails[quoteDetails.Details.DriverDetails.Length];
            try
            {
                trfProcedureDetails.Reqt_Ref_No = quoteDetails.RequestReferenceNo;
                trfProcedureDetails.Make_Code = quoteDetails.Details.VehicleMakeCodeNIC;
                trfProcedureDetails.Model_Code = quoteDetails.Details.VehicleModelCodeNIC;
                trfProcedureDetails.Body_Type_Code = 0;
                trfProcedureDetails.Production_Year = quoteDetails.Details.ManufactureYear;
                trfProcedureDetails.Gender = quoteDetails.Details.LesseeGender;
                trfProcedureDetails.NCD_Level = Convert.ToInt32(quoteDetails.Details.LesseeNCDCode);
                trfProcedureDetails.Ncd_Driver_Level = 0;
                trfProcedureDetails.Tat = 0;
                trfProcedureDetails.City = quoteDetails.Details.LesseeNationalAddress[0].City;
                trfProcedureDetails.Driver_Nationality = Convert.ToInt32(quoteDetails.Details.LesseeNationalityID);
                trfProcedureDetails.ZipCode = quoteDetails.Details.LesseeNationalAddress[0].ZipCode;
                trfProcedureDetails.VehicleMakeTextNIC = quoteDetails.Details.VehicleMakeTextNIC;
                trfProcedureDetails.VehicleModelTextNIC = quoteDetails.Details.VehicleModelTextNIC;
                trfProcedureDetails.Policy_Effective_Date = DateTime.Now;
                trfProcedureDetails.PolicyHolder_Id = quoteDetails.Details.LesseeID;
                trfProcedureDetails.Vehicle_Sequence_Number = 0;
                trfProcedureDetails.VehicleCustome_Id = 0;
                trfProcedureDetails.SumInsured = quoteDetails.Details.VehicleSumInsured;
                trfProcedureDetails.REPAIRTYPE = quoteDetails.Details.RepairMethod.ToString();

                string DOB = "";
                if (!string.IsNullOrEmpty(quoteDetails.Details.LesseeDateOfBirthG))
                {
                    DOB = quoteDetails.Details.LesseeDateOfBirthG;
                }

                else
                {
                    string[] tempDob = quoteDetails.Details.LesseeDateOfBirthH.Split('-');
                    if (Convert.ToInt32(tempDob[0]) > 28)
                    {
                        tempDob[0] = "28";
                    }
                    DOB = date.HijriToGreg(tempDob[0] + "-" + tempDob[1] + "-" + tempDob[2]).ToString();
                }
                trfProcedureDetails.Driver_Age = CalculateAge(DOB, DateTime.Now.ToString());

                ErrHandler.WriteLog("Tariff Request Details: ", JsonConvert.SerializeObject(trfProcedureDetails).ToString(), trfProcedureDetails.Reqt_Ref_No);
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, quoteDetails.RequestReferenceNo, string.Empty, "PreptrfDetails");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), ex.ToString(), "PreptrfDetails", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            tariffInput = trfProcedureDetails;
            return trfProcedureDetails;
        }
        #endregion 'Prepare tarif Procedure Inputs'

        internal long GenerateUniqueRefeNo()
        {
            Random r = new Random();
            try
            {
                return Convert.ToInt64(DateTime.Now.ToString("yyMMddmmssfff"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal long GenerateUniqueQuoteRefeNo()
        {
            Random r = new Random();
            try
            {
                return Convert.ToInt64(DateTime.Now.ToString("yyMMddmmssfff"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region 'Prepare Quote Response'

        internal QuoteResponse PrepQuoteResp(DataTable dtPremdetails, QuoteDetails quoteDetails, List<Error> ErrMsg, StaticValues staticValues)
        {
            QuoteResponse quoteResponse = new QuoteResponse();
            try
            {
                quoteRefeNo = quoteResponse.RequestReferenceNo;
                quoteResponse.RequestReferenceNo = quoteDetails.RequestReferenceNo;
                quoteResponse.InsuranceCompanyCode = quoteDetails.InsuranceCompanyCode;
                int expiryDays = Convert.ToInt32(ConfigurationManager.AppSettings["QuoteExpiryDays"]);
                string ExpiryDate = DateTime.Now.AddDays(expiryDays).ToString("dd-MM-yyyy HH:mm:ss");
                quoteResponse.QuotationExpiryDate = Convert.ToDateTime(ExpiryDate);
                quoteResponse.DriverDetails = quoteDetails.Details.DriverDetails != null ? PrepDriverDetailsResp(quoteDetails) : null;
                quoteResponse.LesseeID = quoteDetails.Details.LesseeID;
                quoteResponse.VehicleUsagePercentage = quoteDetails.Details.VehicleUsagePercentage;
                quoteResponse.LesseeDateOfBirthG = quoteDetails.Details.LesseeDateOfBirthG;
                quoteResponse.LesseeDateOfBirthH = quoteDetails.Details.LesseeDateOfBirthH;
                //Changes starts by Sagar 05012020
                quoteResponse.LesseeGender = quoteDetails.Details.LesseeGender;
                //Changes Ends by Sagar 05012020
                quoteResponse.NCDEligibility = Convert.ToInt32(quoteDetails.Details.LesseeNCDCode);
                quoteResponse.NajmCaseDetails = quoteDetails.Details.NajmCaseDetails;
                quoteResponse.CompQuotes = PrepDetailResp(quoteDetails, dtPremdetails, ErrMsg, staticValues);
                quoteResponse.Status = ErrMsg.Count > 0 ? false : true;
                quoteResponse.errors = ErrMsg;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, quoteDetails.RequestReferenceNo, string.Empty, "PrepQuoteResp");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), ex.ToString(), "PrepQuoteResp", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            return quoteResponse;
        }

        CompQuotes[] PrepDetailResp(QuoteDetails quoteDetails, DataTable dtPremdetails, List<Error> errMsg, StaticValues staticValues) 
        {
            DataRow[] dtPremdetails_rows = dtPremdetails.Select();
            //CompQuotes[] CompQuotes = new CompQuotes[1];
            CompQuotes[] CompQuotes = new CompQuotes[dtPremdetails_rows.Length];
            DataTable dtCoverDetails = new DataTable();
            long quotereferenceno;
            List<CompQuotes> compquotelist = new List<CompQuotes>();
            Error error = new Error();
            try
            {
                if (errMsg.Count <= 0)
                {
                    int NoOfPass = 1;
                    DataTable dt = dataAccessLayer.GetVehicaldetails(1, quoteDetails.Details.VehicleMakeTextNIC, quoteDetails.Details.VehicleModelTextNIC);
                    if (dt.Rows.Count > 0)
                    {

                        NoOfPass = Convert.ToInt32(dt.Rows[0]["PASSENGERS"]);
                    }
                    else
                    {
                        NoOfPass = quoteDetails.Details.VehicleCapacity;
                    }
                    Random random = new Random();
                    quotereferenceno = Convert.ToInt64(Convert.ToString(random.Next(11, 99)) + Convert.ToString(GenerateUniqueQuoteRefeNo()));
                    foreach (DataRow item in dtPremdetails.Rows)
                    {
                        CompQuotes obj_CompQuotes = new CompQuotes();
                        obj_CompQuotes.QuoteReferenceNo = quotereferenceno;
                        //obj_CompQuotes.QuoteExpiryDate = DateTime.Today.AddDays(Convert.ToDouble(ConfigurationManager.AppSettings["QuoteExpiryDays"])).ToString("yyyy-MM-dd");
                        //Changes Ends by Sagar 05012020
                        obj_CompQuotes.PolicyTitleID = Convert.ToInt32(ConfigurationManager.AppSettings["PolicyTitleID"]);
                        // CompQuotes[0].Deductibles = GetDeductibles(dtPremdetails, quoteDetails.RequestReferenceNo, dtDiscPerct, dtDiscAmt, quoteDetails.Details.VehicleCapacity);
                        dtCoverDetails = dataAccessLayer.GetCoverDetails(1, quoteDetails.RequestReferenceNo, staticValues);
                        int DeductibleAmount1 = Convert.ToInt32(item["DEDUCTIBLE_RATE"]);

                        obj_CompQuotes.Deductibles = PrepareDeductbles(item, DeductibleAmount1, dtCoverDetails, quoteDetails, dtPremdetails, NoOfPass, staticValues);
                        // obj_CompQuotes.PolicyPremiumFeatures = Preparepremiumfeatures(NoOfPass, staticValues);
                        obj_CompQuotes.PolicyPremiumFeatures = null;
                        obj_CompQuotes.CustomizedParameter = quoteDetails.Details.CustomizedParameter;
                        compquotelist.Add(obj_CompQuotes);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, quoteDetails.RequestReferenceNo, string.Empty, "PrepDetailResp");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), ex.ToString(), "PrepDetailResp", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            return CompQuotes = compquotelist.ToArray();
        }

        public Deductibles[] PrepareDeductbles(DataRow row, int deductbleamount, DataTable dtcoverdetails, QuoteDetails quoteDetails, DataTable dtpremiumdata, int noofpassengers, StaticValues staticValues)
        {
            Deductibles[] deductibles = new Deductibles[0];
            List<Deductibles> list_deductibles = new List<Deductibles>();
            Random random = new Random();
            try
            {
                if (row != null)
                {
                    decimal totaldiscountamount = 0;
                    Deductibles obj_deductibles = new Deductibles();
                    obj_deductibles.DeductibleAmount = deductbleamount;
                    var discounts = GetDiscounts(quoteDetails, dtcoverdetails, row, dtpremiumdata);
                    obj_deductibles.Discounts = discounts;
                    totaldiscountamount = Convert.ToDecimal(totaldiscountamount + discounts.Sum(i => i.DiscountAmount));
                    //obj_deductibles.PolicyPremium = Convert.ToDecimal(Convert.ToDecimal(row["BASE_RATE_COMP"]) - totaldiscountamount);
                    obj_deductibles.PolicyPremium = Math.Round(Convert.ToDecimal(row["BASE_RATE_COMP"]) - totaldiscountamount, 2);
                    //obj_deductibles.TaxableAmount = Convert.ToDecimal(Convert.ToDecimal(row["BASE_RATE_COMP"]) - totaldiscountamount);
                    obj_deductibles.TaxableAmount = Math.Round(Convert.ToDecimal(row["BASE_RATE_COMP"]) - totaldiscountamount, 2);
                    //obj_deductibles.BasePremium = Convert.ToDecimal(row["BASE_RATE_COMP"]);
                    obj_deductibles.BasePremium = Math.Round(Convert.ToDecimal(row["BASE_RATE_COMP"]), 2);
                    obj_deductibles.PremiumBreakdown = Preparepremiumbreakdown(row);
                    obj_deductibles.DynamicPremiumFeatures = DynamicPreparepremiumfeatures(noofpassengers, staticValues);
                    obj_deductibles.DeductibleReferenceNo = Convert.ToInt64(Convert.ToString(random.Next(11, 99)) + Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year) + Convert.ToString(DateTime.Now.Hour) + Convert.ToString(DateTime.Now.Minute) + Convert.ToString(random.Next(11, 99))).ToString();
                    list_deductibles.Add(obj_deductibles);
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, quoteDetails.RequestReferenceNo, string.Empty, "PrepareDeductbles");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), ex.ToString(), "PrepareDeductbles", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            return deductibles = list_deductibles.ToArray();
        }

        internal RespDriverDetails[] PrepDriverDetailsResp(QuoteDetails quoteDetails)
        {
            RespDriverDetails[] RespdriverDetails = new RespDriverDetails[quoteDetails.Details.DriverDetails.Length];
            int DrvLength;
            try
            {
                for (DrvLength = 0; DrvLength < quoteDetails.Details.DriverDetails.Length; DrvLength++)
                {
                    RespdriverDetails[DrvLength] = new RespDriverDetails();
                    RespdriverDetails[DrvLength].DriverID = quoteDetails.Details.DriverDetails[DrvLength].DriverID;
                    RespdriverDetails[DrvLength].DriverName = quoteDetails.Details.DriverDetails[DrvLength].DriverFullName;
                    RespdriverDetails[DrvLength].VehicleUsagePercentage = quoteDetails.Details.DriverDetails[DrvLength].VehicleUsagePercentage;
                    RespdriverDetails[DrvLength].DriverDateOfBirthG = quoteDetails.Details.DriverDetails[DrvLength].DriverDateOfBirthG;
                    RespdriverDetails[DrvLength].DriverDateOfBirthH = quoteDetails.Details.DriverDetails[DrvLength].DriverDateOfBirthH;
                    RespdriverDetails[DrvLength].DriverGender = quoteDetails.Details.DriverDetails[DrvLength].DriverGender;
                    RespdriverDetails[DrvLength].NCDEligibility = 0;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, quoteDetails.RequestReferenceNo, string.Empty, "PrepDriverDetailsResp");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), ex.ToString(), "PrepDriverDetailsResp", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            return RespdriverDetails;
        }




        public Discounts[] GetDiscounts(QuoteDetails quoteDetails, DataTable dtCoverDetails, DataRow drprem, DataTable dtPremdetails)
        {
            Discounts[] discounts = new Discounts[0];
            List<Discounts> discountList = new List<Discounts>();
            try
            {
                //DataRow[] dr = dtCoverDetails.Select("BREAKDOWNTYPE = 'D' and EKSABREAKDOWNTYPEID = 1");
                DataRow[] dr = dtCoverDetails.Select("BREAKDOWNTYPE = 'D' and BREAKDOWNTYPEID IN (134,2)");
                Discounts discount = new Discounts();

                if (drprem != null)
                {

                    if (Convert.ToDecimal(drprem["NCD_RATE_COMP"]) > 0)
                    {
                        discount.DiscountTypeID = Convert.ToInt32(dr[0]["BREAKDOWNTYPEID"]);
                        discount.DiscountPercentage = GetNCDPercentage(Convert.ToDecimal(drprem["NCD_FACTOR_RATE_COMP"]));
                        discount.DiscountAmount = Math.Round(Convert.ToDecimal(drprem["NCD_RATE_COMP"]),2);
                        totaldiscountamount = totaldiscountamount + Convert.ToDecimal(dtPremdetails.Rows[0]["NCD_RATE_COMP"]);
                        discountList.Add(discount);
                    }

                    if (Convert.ToDecimal(drprem["LOYALTY_DISCOUNT_COMP"]) > 0)
                    {
                        dr = dtCoverDetails.Select("BREAKDOWNTYPE = 'D' and EKSABREAKDOWNTYPEID = 2");
                        discount = new Discounts();
                        discount.DiscountTypeID = Convert.ToInt32(dr[0]["BREAKDOWNTYPEID"]);
                        discount.DiscountPercentage = staticValues.LoyaltyDiscountPer;
                        discount.DiscountAmount = Convert.ToDecimal(drprem["LOYALTY_DISCOUNT_COMP"]);
                        discountList.Add(discount);
                    }
                    /*
                    if (Convert.ToDecimal(drprem["ADDITIONAL_DISCOUNT_RATE_COMP"]) > 0 && compQuoteRequest.Details.IsScheme)
                    {
                        string schemeRef = compQuoteRequest.Details.SchemeDetails[0].SchemeRef;
                        DataTable dtAddDiscount = dataAccessLayer.GetAdditionalDiscount(compQuoteRequest.Details.SchemeDetails[0].SchemeRef, compQuoteRequest.RequestReferenceNo, staticValues);
                        if (dtAddDiscount != null && dtAddDiscount.Rows.Count > 0)
                        {
                            discount = new Discounts();
                            discount.DiscountTypeID = !string.IsNullOrEmpty(dtAddDiscount.Rows[0]["DISCOUNT_ID"].ToString()) ? Convert.ToInt32(dtAddDiscount.Rows[0]["DISCOUNT_ID"]) : 0;
                            discount.DiscountPercentage = !string.IsNullOrEmpty(dtAddDiscount.Rows[0]["DISCOUNT_PERCENT"].ToString()) ? Convert.ToInt32(dtAddDiscount.Rows[0]["DISCOUNT_PERCENT"]) : 0;
                            discount.DiscountAmount = Convert.ToDecimal(drprem["ADDITIONAL_DISCOUNT_RATE_COMP"]);
                            discountList.Add(discount);
                        }
                    }*/

                    if (Convert.ToDecimal(drprem["SAMA_DISCOUNT"]) > 0)
                    {
                        dr = dtCoverDetails.Select("BREAKDOWNTYPE = 'D' and EKSABREAKDOWNTYPEID  IN ('" + staticValues.SAMADiscountEskaID + "') AND COVERPRICE IS NOT NULL");

                        foreach (DataRow row in dr)
                        {

                            discount = new Discounts();
                            discount.DiscountTypeID = Convert.ToInt32(row["BREAKDOWNTYPEID"]);
                            discount.DiscountPercentage = Convert.ToInt32(row["COVERPRICE"]);
                            discount.DiscountAmount = Convert.ToDecimal((Convert.ToDecimal(drprem["BASE_RATE_COMP"]) - Convert.ToDecimal(drprem["NJMFEE_COMP"]) - totaldiscountamount) * Convert.ToInt32(row["COVERPRICE"]) / 100);
                            totaldiscountamount = totaldiscountamount + (Convert.ToDecimal((Convert.ToDecimal(drprem["BASE_RATE_COMP"]) - Convert.ToDecimal(drprem["NJMFEE_COMP"]) - totaldiscountamount) * Convert.ToInt32(row["COVERPRICE"]) / 100));
                            discountList.Add(discount);
                        }
                    }
                    if (Convert.ToDecimal(drprem["ADDITIONAL_DISCOUNT_RATE_COMP"]) > 0)
                    {

                        dr = dtCoverDetails.Select("BREAKDOWNTYPE = 'D' and EKSABREAKDOWNTYPEID  IN ('" + staticValues.AddDiscountEskaID + "') AND COVERPRICE IS NOT NULL");
                        foreach (DataRow row in dr)
                        {

                            discount = new Discounts();
                            discount.DiscountTypeID = Convert.ToInt32(row["BREAKDOWNTYPEID"]);
                            discount.DiscountPercentage = Convert.ToInt32(row["COVERPRICE"]);
                            discount.DiscountAmount = Convert.ToDecimal(drprem["ADDITIONAL_DISCOUNT_RATE_COMP"]);
                            totaldiscountamount = totaldiscountamount + (Convert.ToDecimal((Convert.ToDecimal(drprem["BASE_RATE_COMP"]) - Convert.ToDecimal(drprem["NJMFEE_COMP"]) - totaldiscountamount) * Convert.ToInt32(row["COVERPRICE"]) / 100));
                            discountList.Add(discount);
                        }
                    }
                }

                return discounts = discountList.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PremiumBreakdown[] Preparepremiumbreakdown(DataRow row)
        {
            PremiumBreakdown[] premiumBreakdowns = new PremiumBreakdown[row.Table.Rows.IndexOf(row)];
            List<PremiumBreakdown> listpremiumBreakdowns = new List<PremiumBreakdown>();
            try
            {
                if (row.Table.Rows.Count > 0)
                {
                    PremiumBreakdown obj_premiumBreakdown = new PremiumBreakdown();
                    obj_premiumBreakdown.BreakdownTypeID = 2;
                    obj_premiumBreakdown.BreakdownPercentage = 0;
                    //obj_premiumBreakdown.BreakdownAmount = Convert.ToDecimal(row["BASE_RATE_COMP"]);
                    obj_premiumBreakdown.BreakdownAmount = Math.Round(Convert.ToDecimal(row["BASE_RATE_COMP"]),2);
                    listpremiumBreakdowns.Add(obj_premiumBreakdown);
                }
            }
            catch (Exception ex)
            {

            }
            return premiumBreakdowns = listpremiumBreakdowns.ToArray();
        }

        public PolicyPremiumFeatures[] Preparepremiumfeatures(int NoofPassanger, StaticValues staticValues)
        {
            PolicyPremiumFeatures[] PremiumFeatures = new PolicyPremiumFeatures[0];
            List<PolicyPremiumFeatures> featureList = new List<PolicyPremiumFeatures>();

            DataTable dt = dataAccessLayer.GetAGGREGATORDetails(1, staticValues.Aggregator);

            if (dt.Rows.Count > 0)
            {
                DataRow[] drList = dt.Select("BREAKDOWNTYPE = 'A'");

                if (drList.Length > 0)
                {
                    foreach (DataRow dr in drList)
                    {
                        int COVERPRICE = Convert.ToInt32(dr["COVERPRICE"]);
                        if (Convert.ToInt32(dr["EKSABREAKDOWNTYPEID"]) == Convert.ToInt32(ConfigurationManager.AppSettings["PACoverId"]))
                        {
                            COVERPRICE = COVERPRICE * NoofPassanger;
                        }

                        featureList.Add(new PolicyPremiumFeatures
                        {
                            FeatureID = Convert.ToInt32(dr["BREAKDOWNTYPEID"]),
                            FeatureTypeID = Convert.ToInt32(dr["FeatureTypeID"]),
                            FeatureAmount = COVERPRICE,
                            FeatureTaxableAmount = COVERPRICE
                        });
                    }
                }
            }
            else
            {
                featureList = null;
            }

            return PremiumFeatures = featureList.ToArray();
        }
        public DynamicPremiumFeatures[] DynamicPreparepremiumfeatures(int NoofPassanger, StaticValues staticValues)
        {
            DynamicPremiumFeatures[] dynamicPremiumFeatures = new DynamicPremiumFeatures[0];
            List<DynamicPremiumFeatures> featureList = new List<DynamicPremiumFeatures>();

            DataTable dt = dataAccessLayer.GetAGGREGATORDetails(1, staticValues.Aggregator);

            if (dt.Rows.Count > 0)
            {
                DataRow[] drList = dt.Select("BREAKDOWNTYPE = 'A'");

                if (drList.Length > 0)
                {
                    foreach (DataRow dr in drList)
                    {
                        int COVERPRICE = Convert.ToInt32(dr["COVERPRICE"]);
                        if (Convert.ToInt32(dr["EKSABREAKDOWNTYPEID"]) == Convert.ToInt32(ConfigurationManager.AppSettings["PACoverId"]))
                        {
                            COVERPRICE = COVERPRICE * NoofPassanger;
                        }

                        featureList.Add(new DynamicPremiumFeatures
                        {
                            FeatureID = Convert.ToInt32(dr["BREAKDOWNTYPEID"]),
                            FeatureTypeID = Convert.ToInt32(dr["FeatureTypeID"]),
                            FeatureAmount = COVERPRICE,
                            FeatureTaxableAmount = COVERPRICE
                        });
                    }
                }
            }
            else
            {
                featureList = null;
            }

            return dynamicPremiumFeatures = featureList.ToArray();
        }
        public decimal GetNCDPercentage(decimal ncdFactor)
        {
            if (ncdFactor < 1)
            {
                return (1 - ncdFactor) * 100;
            }
            else
            {
                return 0;
            }

        }

        // Changes Shyam by Shyam Patil on Date:22-04-2022 for VALIDATION
        internal DynamicPremiumFeatures[] GetdynamicPremiumFeatures(DataTable dtPremdetails, long? SeatCount)
        {
            DynamicPremiumFeatures[] premBreakdowns;
            try
            {
                premBreakdowns = new DynamicPremiumFeatures[dtPremdetails.Rows.Count];
                int SeatCapacity = Math.Max(Convert.ToInt32(SeatCount - 1), 1);

                for (int i = 0; i < dtPremdetails.Rows.Count; i++)
                {
                    DataRow dr = dtPremdetails.Rows[i];
                    premBreakdowns[i] = new DynamicPremiumFeatures();
                    premBreakdowns[i].FeatureID = Convert.ToInt32(dr["TAMEENI_ID"]);
                    //premBreakdowns[i].FeatureID = Convert.ToInt32(dr["TAMEENI_ID"]);
                    if (dr["MT_EXCESS_AMT"] != null && dr["MT_EXCESS_AMT"].ToString() != string.Empty)
                    {
                        premBreakdowns[i].FeatureTypeID = Convert.ToDecimal(dr["PREMIUM_AMOUNT"]) > 0 ? 1 : 2; ;
                        premBreakdowns[i].FeatureAmount = premBreakdowns[i].FeatureTaxableAmount = (premBreakdowns[i].FeatureID == 2064 && SeatCount != null) ? Math.Round(Convert.ToDecimal(dr["PREMIUM_AMOUNT"]) * SeatCapacity, 2) : Math.Round(Convert.ToDecimal(dr["PREMIUM_AMOUNT"]), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", string.Empty, string.Empty, ex.ToString(), "GetdynamicPremiumFeatures", "Quote", "Tameeni", "TAMEENI");
                throw ex;
            }
            return premBreakdowns;
        }
        // Changes Shyam by Shyam Patil on Date:22-04-2022 for VALIDATION

        #endregion 'Prepare Quote Response'

        internal PolicyResponse PrepPolicyResposne(PolicyRequest policyRequest, List<Error> ErrMsg, bool IsJsonValid)
        {
            PolicyResponse policyResponse = new PolicyResponse();
            Random random = new Random();
            try
            {
                policyResponse.Status = ErrMsg.Count > 0 ? false : true;
                policyResponse.errors = ErrMsg;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, policyRequest.RequestReferenceNo, policyRequest.QuoteReferenceNo.ToString(), "PrepPolicyResposne");
                error.field = "";
                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                error.code = "unexpected_error";
                errorsList.Add(error);
                policyResponse.Status = false;
                policyResponse.errors = errorsList;
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyRequest), ex.ToString(), "PrepPolicyResposne", "Quote", "TameeniLeasing", "TameeniLeasing");
            }
            return policyResponse;
        }

        internal SavePolicyDetails PrepPolDetails(PolicyIssueRequest policyIssueRequest, QuoteResponse quoteResponse, PolicyResponse PolicyResponse)
        {
            SavePolicyDetails policyDetails = new SavePolicyDetails();
            try
            {
                policyDetails.ReqtReferenceNo = quoteResponse.RequestReferenceNo;
                policyDetails.InsuranceCompanyCode = quoteResponse.InsuranceCompanyCode;
                policyDetails.QuoteReferenceNo = policyIssueRequest.QuoteReferenceNo;
                policyDetails.InsuranceTypeId = 2;
                policyDetails.OldPolicyNumber = DBNull.Value.ToString();
                policyDetails.customizedParameters = quoteResponse.CompQuotes[0].CustomizedParameter;
                policyDetails.Status = "0";
                policyDetails.PolicyReqtRefNo = policyIssueRequest.PolicyRequestReferenceNo;
                policyDetails.PolicyRefNo = policyIssueRequest.QuoteReferenceNo;

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, quoteResponse.RequestReferenceNo, string.Empty, "PrepPolDetails");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), ex.ToString(), "PrepPolDetails", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw;
            }
            return policyDetails;

        }

        internal PolicyIssueResponse PrepPolicyIssueResp(List<Error> ErrMsg, PolicyIssueRequest policyIssueRequest)
        {
            PolicyIssueResponse policyIssueResponse = new PolicyIssueResponse();
            try
            {
                policyIssueResponse.PolicyReferenceNo = policyIssueRequest.QuoteReferenceNo;
                //policyIssueResponse.PolicyNumber = null;
                policyIssueResponse.PolicyEffectiveDate = Convert.ToDateTime(policyIssueRequest.Details.PolicyEffectiveDate) <= Convert.ToDateTime(System.DateTime.Today.ToString("yyyy-MM-dd")) ? System.DateTime.Today.ToString("yyyy-MM-dd") : policyIssueRequest.Details.PolicyEffectiveDate;
                policyIssueResponse.PolicyExpiryDate = Convert.ToDateTime(policyIssueResponse.PolicyEffectiveDate).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
                policyIssueResponse.PolicyFileUrl =null;//need to verify 
                policyIssueResponse.Status = true;
                policyIssueResponse.errors = ErrMsg;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, policyIssueRequest.RequestReferenceNo, policyIssueRequest.QuoteReferenceNo.ToString(), "PrepPolicyIssueResp");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), ex.ToString(), "PrepPolicyIssueResp", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw;
            }
            return policyIssueResponse;

        }

        internal PolicyIssueResponse PrepUpdtPolicyResp(List<Error> ErrMsg, PolicyIssueRequest policyIssueRequest)
        {
            PolicyIssueResponse policyIssueResponse = new PolicyIssueResponse();
            try
            {
                policyIssueResponse.PolicyReferenceNo = policyIssueRequest.QuoteReferenceNo;
                //policyIssueResponse.PolicyNumber = null;
                policyIssueResponse.PolicyEffectiveDate = Convert.ToDateTime(policyIssueRequest.Details.PolicyEffectiveDate) <= Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd")) ? DateTime.Today.ToString("yyyy-MM-dd") : policyIssueRequest.Details.PolicyEffectiveDate;
                policyIssueResponse.PolicyExpiryDate = Convert.ToDateTime(policyIssueResponse.PolicyEffectiveDate).AddYears(1).AddDays(-1).ToString("yyyy-MM-dd");
                policyIssueResponse.Status = true;
                policyIssueResponse.errors = ErrMsg;

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, policyIssueRequest.RequestReferenceNo, policyIssueRequest.QuoteReferenceNo.ToString(), "PrepPolicyIssueResp");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), ex.ToString(), "PrepPolicyIssueResp", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw;
            }
            return policyIssueResponse;

        }

        internal QueryFeatureResponse PrepQueryFeatureResp(QueryFeatureRequest queryFeatureRequest,QuoteDetails quoteDetails, PolicyIssueRequest PolicyIssueRequest, List<Error> ErrMsg,StaticValues staticValues)
        {
            QueryFeatureResponse queryFeatureResponse = new QueryFeatureResponse();
            try
            {
                queryFeatureResponse.PolicyReferenceNo = queryFeatureRequest.PolicyReferenceNo;
                queryFeatureResponse.QueryRequestReferenceNo = queryFeatureRequest.QueryRequestReferenceNo;
                queryFeatureResponse.QueryResponseReferenceNo = GenerateUniqueRefeNo();
                queryFeatureResponse.InsuranceCompanyCode = queryFeatureRequest.InsuranceCompanyCode;
                int NoOfPass = 1;
                DataTable dt = dataAccessLayer.GetVehicaldetails(1, quoteDetails.Details.VehicleMakeTextNIC, quoteDetails.Details.VehicleModelTextNIC);
                if (dt.Rows.Count > 0)
                {

                    NoOfPass = Convert.ToInt32(dt.Rows[0]["PASSENGERS"]);
                }
                else
                {
                    NoOfPass = quoteDetails.Details.VehicleCapacity;
                }
                queryFeatureResponse.PolicyPremiumFeatures = DynamicPreparepremiumfeatures(NoOfPass, staticValues);
                queryFeatureResponse.PolicyPremiumFeatures = queryFeatureResponse.PolicyPremiumFeatures.Where(FAmt => FAmt.FeatureAmount > 0).ToArray();
                queryFeatureResponse.Status = ErrMsg.Count > 0 ? false : true;
                queryFeatureResponse.errors = ErrMsg;
                queryFeatureResponse.CustomizedParameter = queryFeatureRequest.CustomizedParameter;
                if (PolicyIssueRequest.Details.PolicyPremiumFeatures != null)
                {
                    for (int i = 0; i < PolicyIssueRequest.Details.PolicyPremiumFeatures.Count(); i++)
                    {
                        queryFeatureResponse.PolicyPremiumFeatures = queryFeatureResponse.PolicyPremiumFeatures.Where(Fid => Fid.FeatureID != PolicyIssueRequest.Details.PolicyPremiumFeatures[i].FeatureID).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, queryFeatureResponse.PolicyReferenceNo.ToString(), string.Empty, "PrepQueryFeatureResp");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", queryFeatureRequest.QueryRequestReferenceNo.ToString(), JsonConvert.SerializeObject(queryFeatureRequest), ex.ToString(), "PrepQueryFeatureResp", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            return queryFeatureResponse;
        }
        internal DynamicPremiumFeatures[] GetPolPremFeature(DataTable dtPremdetails, long? SeatCount)
        {
            int SeatCapacity = 0;
            int BenefitCode;
            DynamicPremiumFeatures[] policyPremiumFeatures;
            try
            {
                SeatCapacity = SeatCount > 0 ? Convert.ToInt32(SeatCount - 1) : 0;
                if (SeatCapacity <= 0)
                {
                    foreach (DataRow drow in dtPremdetails.Rows)
                    {
                        if (drow["CODE"].ToString() == "2064")
                        {
                            drow.Delete();
                        }
                    }
                    dtPremdetails.AcceptChanges();
                }
                DataRow[] dr = dtPremdetails.Select("CVTTYPE = 'C' and BASICCVT_YN = 0");
                policyPremiumFeatures = new DynamicPremiumFeatures[dr.Length > 0 ? dr.Length + 1 : dr.Length];
                if (dr.Length > 0)
                {
                    policyPremiumFeatures[0] = new DynamicPremiumFeatures();
                    policyPremiumFeatures[0].FeatureID = 239;
                    policyPremiumFeatures[0].FeatureTypeID = 2;
                    policyPremiumFeatures[0].FeatureAmount = 0;
                    policyPremiumFeatures[0].FeatureTaxableAmount = 0;

                    for (int i = 0; i < dr.Length; i++)
                    {
                        policyPremiumFeatures[i + 1] = new DynamicPremiumFeatures();
                        BenefitCode = Convert.ToInt32(dr[i]["CODE"]);
                        policyPremiumFeatures[i + 1].FeatureID = Convert.ToInt32(dr[i]["TAMEENI_ID"]);
                        if (dr[i]["PREMIUM_AMOUNT"] != null && dr[i]["PREMIUM_AMOUNT"].ToString() != string.Empty)
                        {
                            policyPremiumFeatures[i + 1].FeatureTypeID = Convert.ToDecimal(dr[i]["PREMIUM_AMOUNT"]) > 0 ? 1 : 2;
                            policyPremiumFeatures[i + 1].FeatureAmount = policyPremiumFeatures[i + 1].FeatureTaxableAmount = (BenefitCode == 2064 && SeatCount != null) ? Math.Round(Convert.ToDecimal(dr[i]["PREMIUM_AMOUNT"]) * SeatCapacity, 2) : Math.Round(Convert.ToDecimal(dr[i]["PREMIUM_AMOUNT"]), 2);
                        }
                        else
                        {
                            policyPremiumFeatures[i + 1].FeatureTypeID = 2;
                            policyPremiumFeatures[i + 1].FeatureAmount = 0;
                            policyPremiumFeatures[i + 1].FeatureTaxableAmount = 0;
                        }
                    }

                }
                else
                {
                    policyPremiumFeatures = null;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, quoteRefeNo, string.Empty, "GetPolPremFeature");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", string.Empty, string.Empty, ex.ToString(), "GetPolPremFeature", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            return policyPremiumFeatures;
        }
        internal PurchaseFeatureResponse PrepPurchaseFeatureResp(PurchaseFeatureRequest purchaseFeatureRequest, List<Error> ErrMsg)
        {
            PurchaseFeatureResponse purchaseFeatureResponse = new PurchaseFeatureResponse();
            try
            {
                purchaseFeatureResponse.Status = ErrMsg.Count > 0 ? false : true;
                purchaseFeatureResponse.errors = ErrMsg;
                //Change Start By Sagar 01042021
                purchaseFeatureResponse.EndorsementReferenceNo = purchaseFeatureRequest.QueryResponseReferenceNo;
                //Change End By Sagar 01042021
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, purchaseFeatureRequest.QueryRequestReferenceNo.ToString(), string.Empty, "PrepPurchaseFeatureResp");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", purchaseFeatureRequest.QueryRequestReferenceNo.ToString(), JsonConvert.SerializeObject(purchaseFeatureRequest), ex.ToString(), "PrepPurchaseFeatureResp", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw ex;
            }
            return purchaseFeatureResponse;
        }
        internal AddDriverResponse PrepAddDriverResp(List<Error> ErrMsg, AddDriverRequest addDriverRequest)
        {
            AddDriverResponse addDriverResponse = new AddDriverResponse();
            try
            {
                addDriverResponse.PolicyReferenceNo = addDriverRequest.PolicyReferenceNo;
                addDriverResponse.AddDriverRequestReferenceNo = addDriverRequest.AddDriverRequestReferenceNo;
                addDriverResponse.AddDriverResponseReferenceNo = Convert.ToString(GenerateUniqueRefeNo());
                addDriverResponse.InsuranceCompanyCode = addDriverRequest.InsuranceCompanyCode;
                addDriverResponse.DriversBreakdown = PrepDriversBreakdown(addDriverRequest.DriverDetails);
                addDriverResponse.AdditionalPremium = 0;
                addDriverResponse.Status = ErrMsg.Count > 0 ? false : true;
                addDriverResponse.errors = ErrMsg;
                addDriverResponse.CustomizedParameter = addDriverRequest.CustomizedParameter;

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, addDriverRequest.AddDriverRequestReferenceNo.ToString(), addDriverRequest.PolicyNumber.ToString(), "PrepAddDriverResp");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", addDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(addDriverRequest), ex.ToString(), "PrepAddDriverResp", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw;
            }
            return addDriverResponse;
        }
        internal DriversBreakdown[] PrepDriversBreakdown(DriverDetails[] DriverDetails)
        {
            DriversBreakdown[] DriversBreakdown = new DriversBreakdown[DriverDetails.Length];
            try
            {
                for (int i = 0; i < DriverDetails.Length; i++)
                {
                    DriversBreakdown[i] = new DriversBreakdown();
                    DriversBreakdown[i].DriverID = DriverDetails[i].DriverID;
                    DriversBreakdown[i].DriverName = DriverDetails[i].DriverFullName;
                    DriversBreakdown[i].VehicleUsagePercentage = DriverDetails[i].VehicleUsagePercentage;
                    DriversBreakdown[i].DriverDateOfBirthG = DriverDetails[i].DriverDateOfBirthG;
                    DriversBreakdown[i].DriverDateOfBirthH = DriverDetails[i].DriverDateOfBirthH;
                    string dob = !string.IsNullOrEmpty(DriversBreakdown[i].DriverDateOfBirthG) ? DriversBreakdown[i].DriverDateOfBirthG : Convert.ToString(ConvrtHijriToGregorian(DriversBreakdown[i].DriverDateOfBirthH));
                    int age = CalculateAge(dob, DateTime.Now.ToString());
                    DriversBreakdown[i].DriverGender = DriverDetails[i].DriverGender;
                   // DriversBreakdown[i].NCDEligibility = 0;
                    DriversBreakdown[i].DriverAmount = price(age);
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, string.Empty, string.Empty, "PrepDriversBreakdown");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", string.Empty, string.Empty, ex.ToString(), "PrepDriversBreakdown", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw;
            }
            return DriversBreakdown;
        }
        internal ConfirmAddDriverResponse PrepConfirmAddDriverResp(List<Error> ErrMsg, ConfirmAddDriverRequest confirmAddDriverRequest)
        {
            ConfirmAddDriverResponse confirmAddDriverResponse = new ConfirmAddDriverResponse();
            try
            {
                confirmAddDriverResponse.Status = ErrMsg.Count > 0 ? false : true;
                confirmAddDriverResponse.errors = ErrMsg;
                confirmAddDriverResponse.EndorsementReferenceNo = confirmAddDriverRequest.AddDriverResponseReferenceNo;
             
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), confirmAddDriverRequest.PolicyReferenceNo.ToString(), "PrepConfirmAddDriverResp");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(confirmAddDriverRequest), ex.ToString(), "PrepConfirmAddDriverResp", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw;
            }
            return confirmAddDriverResponse;

        }

        //changes start made by raju on 
        public int CalculateAge(string dob, string effectiveDate)
        {
            try
            {
                var age = Convert.ToDateTime(effectiveDate).Year - Convert.ToDateTime(dob).Year;
                if (Convert.ToDateTime(dob) > (Convert.ToDateTime(effectiveDate).AddYears(-age)))
                    age--;
                return age;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //changes start made by raju on 

        public DateTime ConvrtHijriToGregorian(string HijriDate)
        {
            try
            {
                string[] splitdate = HijriDate.Split('-');
                if ((Convert.ToInt32(splitdate[1]) == 2 || Convert.ToInt32(splitdate[1]) == 4 || Convert.ToInt32(splitdate[1]) == 6 ||
                     Convert.ToInt32(splitdate[1]) == 8 || Convert.ToInt32(splitdate[1]) == 10 || Convert.ToInt32(splitdate[1]) == 12) && Convert.ToInt32(splitdate[0]) > 29)
                {
                    splitdate[0] = "29";
                }
                DateTime dt = new DateTime(Convert.ToInt32(splitdate[2]), Convert.ToInt32(splitdate[1]), Convert.ToInt32(splitdate[0]), new HijriCalendar());
                return dt;
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, string.Empty, string.Empty, "ConvertToGregorian");
                throw ex;
            }


        }
        public decimal price(int age)
        {
            decimal price = 0;
            try
            {
                if (age < 25)
                {
                    price = 700;
                }
                else if (age >= 25 && age <= 30)
                {
                    price = 400;
                }
                else if (age >30)
                {
                    price = 300;
                }
                else
                {
                    price = 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return price;
        }
        internal IcUpdatePolicyResponse PrepIcupdtPolicyResp(List<Error> ErrMsg, IcUpdatePolicyRequest icUpdatePolicyRequest)
        {
            IcUpdatePolicyResponse icUpdatePolicyResponse = new IcUpdatePolicyResponse();
            try
            {

                icUpdatePolicyResponse.Status = ErrMsg.Count > 0 ? false : true;
                icUpdatePolicyResponse.errors = ErrMsg;

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, icUpdatePolicyRequest.RequestReferenceNo, icUpdatePolicyRequest.QuoteReferenceNo.ToString(), "PrepIcupdtPolicyResp");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), ex.ToString(), "PrepIcupdtPolicyResp", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw;
            }
            return icUpdatePolicyResponse;

        }
        internal IcCancelPolicyResponse PrepIcCancelPolicyResp(List<Error> ErrMsg, IcCancelPolicyRequest icCancelPolicyRequest)
        {
            IcCancelPolicyResponse icCancelPolicyResponse = new IcCancelPolicyResponse();
            try
            {
                icCancelPolicyResponse.Status = ErrMsg.Count > 0 ? false : true;
                //icCancelPolicyResponse.RequestedCommission = icCancelPolicyResponse.Status == true ? Convert.ToDecimal(0) : (decimal?)null;
                //icCancelPolicyResponse.RefundedAmount = icCancelPolicyResponse.Status == true ? Convert.ToDecimal(0) : (decimal?)null;
                icCancelPolicyResponse.errors = ErrMsg;

            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, icCancelPolicyRequest.RequestReferenceNo, icCancelPolicyRequest.QuoteReferenceNo.ToString(), "PrepIcCancelPolicyResp");
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icCancelPolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icCancelPolicyRequest), ex.ToString(), "PrepIcCancelPolicyResp", "Quote", "TameeniLeasing", "TameeniLeasing");
                throw;
            }
            return icCancelPolicyResponse;

        }
    }
}