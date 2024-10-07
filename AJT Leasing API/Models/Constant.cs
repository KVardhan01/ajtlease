using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AJT_Leasing_API.Models
{
    public class Constant
    {
        public static string OracleDB = ConfigurationManager.ConnectionStrings["OracleDB"].ToString();
        public static string EjadaEskaMig = ConfigurationManager.ConnectionStrings["EjadaEskaMig"].ToString();
        public static string GetTransformationDetails = "USP_GET_TRANSFORM_DETAILS";
        internal static string AuthenticationDetaileGet = "USP_AUTHENTICATIONDETAILS_GET";
        //Changes Start By Sagar 14092021
        //public static string prc_prem_calc_tameeni = "EJADA_ESKAMIG.PKG_CALC_PREM.PRC_PREM_CALC_AGGR";
        //public static string prc_prem_calc_tameeni = "EJADA_ESKAMIG.PKG_CALC_PREM.PRC_PREM_CALC_AGGR_NEW";
        //Changes Start By Sagar 14092021

        //Changes Start By Shyam patil 29042022
        //public static string prc_prem_calc_tameeni = "EJADA_ESKAMIG.PKG_CALC_PREM.PRC_PREM_CALC_AGGR_NEW";
        public static string prc_prem_calc_tameeni = "USP_PREM_CALC_TAMEEENI_COMP";
        public static string ValidateSA = "USP_GET_ValidateSA";
        //Changes End By Shyam patil 29042022

        public static string QuoteDetails = "USP_QUOTEDETAILS_AJTL";//USP_QUOTEDETAILS_TL
        public static string UpdatePolicyNumber = "USP_UPDATEPOLICYNUMBER";
        // public static string IsStopQuotation = "MLAYER_ESKAMIG.USP_ISSTOPQUOTATION";
        //public static string GetSchemeCode = "GET_SCHEMECODE";
        public static string UpdatePolicyDetails = "USP_UPDATEPOLICYDETAILS";
        public static string SaveErrorLogs = "USP_SAVEERRORLOGS_AJTL";//USP_SAVEERRORLOGS

        // Changes start by rahul on Date:20-12-2021 for adding IcDocumentDownload
        //public static string LocalDB = ConfigurationManager.ConnectionStrings["LocalDB"].ToString();
        public static string GetDocumentDownload = "USP_DSP_GETInvoiceDetails";
        // Changes end by rahul on Date:20-12-2021 for adding IcDocumentDownload
        public static string GetCoverDetails = "USP_COVERDETAILS_MOTOR";
        public static string GetAdditionalDiscount = "USP_GET_ADDITIONAL_DISCOUNT";
        public static string GetVehicaldetails = "Usp_GetVehicaldetails";
        public static string GetAGGREGATORDetails = "Usp_GetAGGREGATORDetails";

    }
}