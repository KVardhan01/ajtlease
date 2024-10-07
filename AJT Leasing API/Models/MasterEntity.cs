using AJT_Leasing_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AJT_Leasing_API.Models
{
    public class MasterEntity
    {
    }

    #region 'Quote Request Class Properties' 
    public class QuoteDetails
    {
        [Required, RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "RequestReferenceNo Invalid Character"), StringLength(13, ErrorMessage = "RequestReferenceNo Invalid Length")]
        public string RequestReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,13}$", ErrorMessage = "Invalid InsuranceCompanyCode")]
        public int InsuranceCompanyCode { get; set; }

        public Details Details { get; set; }

    }

    public class Details
    {
        [Required, RegularExpression("^[ a-zA-Z.-]*$", ErrorMessage = "LessorNameEN Invalid Character"), StringLength(150, ErrorMessage = "LessorNameEN Invalid Length")]
        public string LessorNameEN { get; set; }

        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid LessorID")]
        public long LessorID { get; set; }

        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid LessorBranch")]
        public long LessorBranch { get; set; }


        [Required]
        public NationalAddress[] LessorNationalAddress { get; set; }

        [Required]
        public bool IsRenewal { get; set; }
        // change start by rahul on date : 13-08-2021
        //[RegularExpression("^[ a-zA-Z0-9/-]*$", ErrorMessage = "PolicyNumber Invalid Character"), StringLength(15, ErrorMessage = "PolicyNumber Invalid Length")]
        [RegularExpression("^[ a-zA-Z0-9/-]*$", ErrorMessage = "PolicyNumber Invalid Character"), StringLength(15, ErrorMessage = "PolicyNumber Invalid Length")]
        public string PolicyNumber { get; set; }
        // change end by rahul on date : 13-08-2021
        [Required, RegularExpression("^[0-9]{1,2}$", ErrorMessage = "Invalid PurposeofVehicleUseID")]
        public int PurposeofVehicleUseID { get; set; }

        [RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Invalid Cylinders")]
        public int Cylinders { get; set; }

        [RegularExpression("^[0-9]{1,6}$", ErrorMessage = "Invalid VehicleMileage")]
        public int VehicleMileage { get; set; }

        [RegularExpression("^[0-9]{1,6}$", ErrorMessage = "Invalid VehicleExpectedMileageYear")]
        public int VehicleExpectedMileageYear { get; set; }

        [RegularExpression(@"^(\d{1,5})(\.\d{1,3})?$", ErrorMessage = "VehicleEngineSizeCC Invalid Character")]
        public decimal? VehicleEngineSizeCC { get; set; }

        [Required, RegularExpression("^[1-2]{1}$", ErrorMessage = "VehicleTransmission Invalid Character")]
        public int VehicleTransmission { get; set; }

        [Required,RegularExpression("^[1-3]{1,2}$", ErrorMessage = "VehicleNightParking Invalid Character")]
        public int VehicleNightParking { get; set; }

        [RegularExpression("^[0-9]{1,2}$", ErrorMessage = "VehicleCapacity Invalid Character")]
        public int VehicleCapacity { get; set; }

        //public int VehicleBodyCode { get; set; }

        //Changes Start By Sagar Validation Changes 13012021
        //[Required, RegularExpression("^[0-9]{1,5}$", ErrorMessage = "VehicleMakeCodeNIC Invalid Character")]
        [Required, RegularExpression("^[-]1|[0-9]{1,5}$", ErrorMessage = "VehicleMakeCodeNIC Invalid Character")]
        public int VehicleMakeCodeNIC { get; set; }

        //[Required, RegularExpression("^[ a-zA-Z\u0600-\u06FF.-]*$", ErrorMessage = "VehicleMakeTextNIC Invalid Character"), StringLength(25, ErrorMessage = "VehicleMakeTextNIC Invalid Length")]
        [Required, RegularExpression("^[ a-zA-Z\u0600-\u06FF.-]*$", ErrorMessage = "VehicleMakeTextNIC Invalid Character"), StringLength(25, ErrorMessage = "VehicleMakeTextNIC Invalid Length")]
        public string VehicleMakeTextNIC { get; set; }

        //[Required, RegularExpression("^[0-9]{1,4}$", ErrorMessage = "VehicleMakeCodeIHC Invalid Character")]
        //public int VehicleMakeCodeIHC { get; set; }

        //Changes Start By Sagar Validation Changes 13012021
        //[Required, RegularExpression("^[0-9]{1,5}$", ErrorMessage = "VehicleModelCodeNIC Invalid Character")]
        [Required, RegularExpression("^[-]1|[0-9]{1,5}$", ErrorMessage = "VehicleModelCodeNIC Invalid Character")]
        public int VehicleModelCodeNIC { get; set; }

        //Changes done by Sagar on 05022021 Add * on Validation 
        [Required, RegularExpression("^[ a-zA-Z0-9*\u0600-\u06FF.-]*$", ErrorMessage = "VehicleModelTextNIC Invalid Character"), StringLength(25, ErrorMessage = "VehicleModelTextNIC Invalid Length")]
        public string VehicleModelTextNIC { get; set; }

        //[Required, RegularExpression("^[0-9]{1,5}$", ErrorMessage = "VehicleModelCodeIHC Invalid Character")]
        //public int VehicleModelCodeIHC { get; set; }

        [Required, RegularExpression("^[0-9]{1,4}$", ErrorMessage = "ManufactureYear Invalid Character")]
        public int ManufactureYear { get; set; }

        [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "VehicleColorCode Invalid Character")]
        public int VehicleColorCode { get; set; }

        [StringLength(150, ErrorMessage = "VehicleModifications Invalid Length")]
        public string VehicleModifications { get; set; }

        //Changes done by Sagar on 05022021 Changed length to 10
        [Required, RegularExpression("^[0-9]{1,6}$", ErrorMessage = "VehicleSumInsured Invalid Character")]
        public int VehicleSumInsured { get; set; }

       // public float DepreciationratePercentage { get; set; }

        [Required, RegularExpression("^[1-2]{1}$", ErrorMessage = "RepairMethod Invalid Character")]
        public int RepairMethod { get; set; }

        public CustomizedParameter[] CustomizedParameter { get; set; }

        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid LesseeID")]
        public long LesseeID { get; set; }

        [Required, RegularExpression("^[ a-zA-Z\u0600-\u06FF.-]*$", ErrorMessage = "FullName Invalid Character"), StringLength(150, ErrorMessage = "FullName Invalid Length")]
        public string FullName { get; set; }
        [RegularExpression("^[a-zA-Z0-9u0600-\u06FF-/() ]*$", ErrorMessage = "FullName Invalid Character"), StringLength(150, ErrorMessage = "FullName Invalid Length")]
        public string ArabicFirstName { get; set; }

        [RegularExpression("^[ a-zA-Z\u0600-\u06FF.-]*$", ErrorMessage = "ArabicMiddleName Invalid Character"), StringLength(150, ErrorMessage = "ArabicMiddleName Invalid Length")]
        public string ArabicMiddleName { get; set; }

        // [RegularExpression("^[ a-zA-Z\u0600-\u06FF.-]*$", ErrorMessage = "ArabicLastName Invalid Character"), StringLength(150, ErrorMessage = "ArabicLastName Invalid Length")]
        public string ArabicLastName { get; set; }


        [RegularExpression("[a-zA-Z0-9]*$", ErrorMessage = "EnglishFirstName Invalid Character"), StringLength(150, ErrorMessage = "EnglishFirstName Invalid Length")]
        public string EnglishFirstName { get; set; }

        [RegularExpression("[a-zA-Z0-9]*$", ErrorMessage = "EnglishMiddleName Invalid Character"), StringLength(150, ErrorMessage = "EnglishMiddleName Invalid Length")]
        public string EnglishMiddleName { get; set; }

        [RegularExpression("^[ a-zA-Z.-]*$", ErrorMessage = "EnglishLastName Invalid Character"), StringLength(150, ErrorMessage = "EnglishLastName Invalid Length")]
        public string EnglishLastName { get; set; }

        [RegularExpression("^(?:[0-9]{1,3})$", ErrorMessage = "LesseeNationalityID Invalid Character")]
        public int? LesseeNationalityID { get; set; }

        [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "VehicleUsagePercentage Invalid Character")]
        public int VehicleUsagePercentage { get; set; }

        //Changes done by Sagar on 05022021 Added () in Validation
        [Required, RegularExpression("^[ a-zA-Z0-9()/\u0600-\u06FF.-]*$", ErrorMessage = "LesseeOccupation Invalid Character"),StringLength(25, ErrorMessage = "LesseeOccupation Invalid Length")]
        public string LesseeOccupation { get; set; }

        [Required, RegularExpression("^[ a-zA-Z0-9\u0600-\u06FF.-]*$", ErrorMessage = "LesseeEducation Invalid Character"), StringLength(25, ErrorMessage = "LesseeEducation Invalid Length")]
        public string LesseeEducation { get; set; }

        [Required, RegularExpression("^[0-9]{1,2}$", ErrorMessage = "LesseeChildrenBelow16 Invalid Character")]
        public int LesseeChildrenBelow16 { get; set; }

        //[RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "LesseeWorkCompanyName Invalid Character"), StringLength(50, ErrorMessage = "LesseeWorkCompanyName Invalid Length")]
        public string LesseeWorkCompanyName { get; set; }

        [RegularExpression("^(?:[0-9]{1,3})$", ErrorMessage = "LesseeWorkCityID Invalid Character")]
        public int? LesseeWorkCityID { get; set; }

        public CountriesValidDrivingLicense[] CountriesValidDrivingLicense { get; set; }

        [RegularExpression("^(?:[0-9]{1,2})$", ErrorMessage = "LesseeNoOfClaims Invalid Character")]
        public int? LesseeNoOfClaims { get; set; }


        [RegularExpression("^[ a-zA-Z0-9,]*$", ErrorMessage = "LesseeTrafficViolationsCode Invalid Character"), StringLength(50, ErrorMessage = "LesseeTrafficViolationsCode Invalid Length")]
        public string LesseeTrafficViolationsCode { get; set; }


        [RegularExpression("^[ a-zA-Z0-9,]*$", ErrorMessage = "LesseeHealthConditionsCode Invalid Character"), StringLength(50, ErrorMessage = "LesseeHealthConditionsCode Invalid Length")]
        public string LesseeHealthConditionsCode { get; set; }

        [RegularExpression("^[0-9-/]*$", ErrorMessage = "LesseeDateOfBirthG Invalid Character"), StringLength(10, ErrorMessage = "LesseeDateOfBirthG Invalid Length")]
        public string LesseeDateOfBirthG { get; set; }

        [RegularExpression("^[0-9-/]*$", ErrorMessage = "LesseeDateOfBirthH Invalid Character"), StringLength(10, ErrorMessage = "LesseeDateOfBirthH Invalid Length")]
        public string LesseeDateOfBirthH { get; set; }

        [Required, RegularExpression("^[1-2]{1}$", ErrorMessage = "LesseeGender Invalid Character")]
        public int LesseeGender { get; set; }

        [Required, RegularExpression("^[1-6]{1}$", ErrorMessage = "LesseeMaritalStatus Invalid Character")]
        public int LesseeMaritalStatus { get; set; }

        [Required, RegularExpression("^([1-9]|1[01])$", ErrorMessage = "LesseeLicenseType Invalid Character")]
        public int LesseeLicenseType { get; set; }


        [Required, RegularExpression("^[0-9]{1,2}$", ErrorMessage = "LesseeLicenseOwnYears Invalid Character")]
        public int LesseeLicenseOwnYears { get; set; }


        [RegularExpression("^[ a-zA-Z0-9-]*$", ErrorMessage = "LesseeNCDCode Invalid Character"), StringLength(10, ErrorMessage = "LesseeNCDCode Invalid Length")]
        public string LesseeNCDCode { get; set; }

        [RegularExpression("^[ a-zA-Z0-9-]*$", ErrorMessage = "LesseeNCDReference Invalid Character"), StringLength(50, ErrorMessage = "LesseeNCDReference Invalid Length")]
        public string LesseeNCDReference { get; set; }

        [RegularExpression("^(?:[0-9]{1})$", ErrorMessage = "LesseeNoOfAccidents Invalid Character")]
        public int? LesseeNoOfAccidents { get; set; }

        public NajmCaseDetails[] NajmCaseDetails { get; set; }

        public NationalAddress[] LesseeNationalAddress { get; set; }

        public DriverDetails[] DriverDetails { get; set; }

    }

    public class NationalAddress
    {
        [Required, RegularExpression("^[0-9]{1,4}$", ErrorMessage = "BuildingNumber Invalid Character")]
        public int BuildingNumber { get; set; }

        [StringLength(200, ErrorMessage = "Street Invalid Length")]
        public string Street { get; set; }

        [StringLength(100, ErrorMessage = "District Invalid Length")]
        public string District { get; set; }

        //[Required, RegularExpression("^[ a-zA-Z0-9\u0600-\u06FF.-]*$", ErrorMessage = "City Invalid Character"), StringLength(50, ErrorMessage = "City Invalid Length")]
        [Required, StringLength(50, ErrorMessage = "City Invalid Length")]
        public string City { get; set; }

        [Required, RegularExpression("^[0-9]{1,5}$", ErrorMessage = "ZipCode Invalid Character")]
        public int ZipCode { get; set; }

        [Required, RegularExpression("^[0-9]{1,4}$", ErrorMessage = "AdditionalNumber Invalid Character")]
        public int AdditionalNumber { get; set; }

    }

    //public class PolicyDocument
    //{
    //    [Required]
    //    public string DocumentMedia { get; set; }

    //    [Required]
    //    public string DocumentType { get; set; }
    //}

    public class CustomizedParameter
    {
        [Required, RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "Key Invalid Character"), StringLength(50, ErrorMessage = "Key Invalid Length")]
        public string Key { get; set; }

        [Required, RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "Value1 Invalid Character"), StringLength(50, ErrorMessage = "Value1 Invalid Length")]
        public string Value1 { get; set; }

        [StringLength(50, ErrorMessage = "Value2 Invalid Length")]
        public string Value2 { get; set; }

        [StringLength(50, ErrorMessage = "Value3 Invalid Length")]
        public string Value3 { get; set; }

        [StringLength(50, ErrorMessage = "Value4 Invalid Length")]
        public string Value4 { get; set; }

    }

    public class CountriesValidDrivingLicense
    {
        [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "DrivingLicenseCountryID Invalid Character")]
        public int DrivingLicenseCountryID { get; set; }

        [Required, RegularExpression("^[0-9]{1,2}$", ErrorMessage = "DriverLicenseYears Invalid Character")]
        public int DriverLicenseYears { get; set; }

    }

    public class NajmCaseDetails
    {
        [Required, RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "CaseNumber Invalid Character"), StringLength(20, ErrorMessage = "CaseNumber Invalid Length")]
        public string CaseNumber { get; set; }
        [Required, RegularExpression("^[a-zA-Z0-9:-]*$", ErrorMessage = "AccidentDate Invalid Character"), StringLength(20, ErrorMessage = "AccidentDate Invalid Length")]
        public string AccidentDate { get; set; }
        [Required, RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Liability Invalid Character"), StringLength(100, ErrorMessage = "Liability Invalid Length")]
        public string Liability { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "DriverAge Invalid Character"), StringLength(3, ErrorMessage = "DriverAge Invalid Length")]
        public string DriverAge { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "CarModel Invalid Character"), StringLength(100, ErrorMessage = "CarModel Invalid Length")]
        public string CarModel { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "CarType Invalid Character"), StringLength(100, ErrorMessage = "CarType Invalid Length")]
        public string CarType { get; set; }
        [Required, RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "DriverID Invalid Character"), StringLength(10, ErrorMessage = "DriverID Invalid Length")]
        public string DriverID { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "SequenceNumber Invalid Character"), StringLength(12, ErrorMessage = "SequenceNumber Invalid Length")]
        public string SequenceNumber { get; set; }
        [RegularExpression("^[a-zA-Z0-9.-]*$", ErrorMessage = "OwnerID Invalid Character"), StringLength(12, ErrorMessage = "OwnerID Invalid Length")]
        public string OwnerID { get; set; }
        [RegularExpression("^[a-zA-Z0-9.-]*$", ErrorMessage = "EstimatedAmount Invalid Character"), StringLength(20, ErrorMessage = "EstimatedAmount Invalid Length")]
        public string EstimatedAmount { get; set; }
        [RegularExpression("^[a-zA-Z0-9.-]*$", ErrorMessage = "DamageParts Invalid Character"), StringLength(500, ErrorMessage = "DamageParts Invalid Length")]
        public string DamageParts { get; set; }
        //Changes starts by Sagar 05012020
        [StringLength(500, ErrorMessage = "CauseOfAccident Invalid Length")]
        public string CauseOfAccident { get; set; }
        //Changes End by Sagar 05012020
    }

    public class DriverDetails
    {
        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid DriverID")]
        public long DriverID { get; set; }

        [Required, RegularExpression("^[ a-zA-Z\u0600-\u06FF.-]*$", ErrorMessage = "DriverFullName Invalid Character"), StringLength(150, ErrorMessage = "DriverFullName Invalid Length")]
        public string DriverFullName { get; set; }
        [Required,RegularExpression(@"^[ a-zA-Z\u0600-\u06FF.\-\s]*$", ErrorMessage = "ArabicFirstName Invalid Character"), StringLength(150, ErrorMessage = "ArabicFirstName Invalid Length")]
        public string ArabicFirstName { get; set; }

        [RegularExpression("^[ a-zA-Z\u0600-\u06FF.-]*$", ErrorMessage = "ArabicMiddleName Invalid Character"), StringLength(150, ErrorMessage = "ArabicMiddleName Invalid Length")]
        public string ArabicMiddleName { get; set; }
        [RegularExpression("^[ a-zA-Z\u0600-\u06FF.-]*$", ErrorMessage = "ArabicLastName Invalid Character"), StringLength(150, ErrorMessage = "ArabicLastName Invalid Length")]
        public string ArabicLastName { get; set; }
        [RegularExpression("[a-zA-Z0-9 ]*$", ErrorMessage = "EnglishFirstName Invalid Character"), StringLength(150, ErrorMessage = "EnglishFirstName Invalid Length")]
        public string EnglishFirstName { get; set; }

        [RegularExpression("^[ a-zA-Z.-]*$", ErrorMessage = "EnglishMiddleName Invalid Character"), StringLength(150, ErrorMessage = "EnglishMiddleName Invalid Length")]
        public string EnglishMiddleName { get; set; }
        [RegularExpression("[ a-zA-Z0-9]*$", ErrorMessage = "EnglishLastName Invalid Character"), StringLength(150, ErrorMessage = "EnglishLastName Invalid Length")]
        public string EnglishLastName { get; set; }

        [RegularExpression("^(?:[1-9]|1[0-1])$", ErrorMessage = "Invalid DriverRelation")]
        public int? DriverRelation { get; set; }

        [RegularExpression("^(?:[0-9]{1,3})$", ErrorMessage = "DriverNationalityID Invalid Character")]
        public int? DriverNationalityID { get; set; }

        [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "VehicleUsagePercentage Invalid Character")]
        public int VehicleUsagePercentage { get; set; }

        [Required, RegularExpression("^[ a-zA-Z0-9\u0600-\u06FF.-]*$", ErrorMessage = "DriverOccupation Invalid Character"), StringLength(25, ErrorMessage = "DriverOccupation Invalid Length")]
        public string DriverOccupation { get; set; }

        [Required, RegularExpression("^[ a-zA-Z\u0600-\u06FF.-]*$", ErrorMessage = "DriverEducation Invalid Character"), StringLength(25, ErrorMessage = "DriverEducation Invalid Length")]
        public string DriverEducation { get; set; }

        [Required, RegularExpression("^[0-9]{1,2}$", ErrorMessage = "DriverChildrenBelow16 Invalid Character")]
        public int DriverChildrenBelow16 { get; set; }

        [RegularExpression("^[ a-zA-Z0-9\u0600-\u06FF.-]*$", ErrorMessage = "DriverWorkCompanyName Invalid Character"), StringLength(50, ErrorMessage = "DriverWorkCompanyName Invalid Length")]
        public string DriverWorkCompanyName { get; set; }

        [RegularExpression("^(?:[0-9]{1,3})$", ErrorMessage = "DriverWorkCityID Invalid Character")]
        public int? DriverWorkCityID { get; set; }

        public CountriesValidDrivingLicense[] CountriesValidDrivingLicense { get; set; }

        [RegularExpression("^(?:[0-9]{1})$", ErrorMessage = "DriverNoOfClaims Invalid Character")]
        public int? DriverNoOfClaims { get; set; }


        [RegularExpression("^[ a-zA-Z0-9,]*$", ErrorMessage = "DriverTrafficViolationsCode Invalid Character"), StringLength(50, ErrorMessage = "DriverTrafficViolationsCode Invalid Length")]
        public string DriverTrafficViolationsCode { get; set; }


        [RegularExpression("^[ a-zA-Z0-9,]*$", ErrorMessage = "DriverHealthConditionsCode Invalid Character"), StringLength(50, ErrorMessage = "DriverHealthConditionsCode Invalid Length")]
        public string DriverHealthConditionsCode { get; set; }

        [RegularExpression("^[0-9-/]*$", ErrorMessage = "DriverDateOfBirthG Invalid Character"), StringLength(10, ErrorMessage = "DriverDateOfBirthG Invalid Length")]
        public string DriverDateOfBirthG { get; set; }

        [RegularExpression("^[0-9-/]*$", ErrorMessage = "DriverDateOfBirthH Invalid Character"), StringLength(10, ErrorMessage = "DriverDateOfBirthH Invalid Length")]
        public string DriverDateOfBirthH { get; set; }

        [Required, RegularExpression("^[1-2]{1}$", ErrorMessage = "DriverGender Invalid Character")]
        public int DriverGender { get; set; }

        [Required, RegularExpression("^[1-6]{1}$", ErrorMessage = "DriverMaritalStatus Invalid Character")]
        public int DriverMaritalStatus { get; set; }

        //[RegularExpression("^[a-zA-Z0-9\u0600-\u06FF.-]*$/", ErrorMessage = "DriverHomeAddressCity Invalid Character"), StringLength(50, ErrorMessage = "DriverHomeAddressCity Invalid Length")]
        [StringLength(50, ErrorMessage = "DriverHomeAddressCity Invalid Length")]
        public string DriverHomeAddressCity { get; set; }
        ///^[-@.\/#&+\w\s]*$/

        [StringLength(100, ErrorMessage = "DriverHomeAddress Invalid Length")]
        public string DriverHomeAddress { get; set; }

        [Required, RegularExpression("^[0-9]{1,2}$", ErrorMessage = "DriverLicenseType Invalid Character")]
        public int DriverLicenseType { get; set; }

        [Required, RegularExpression("^[0-9]{1,2}$", ErrorMessage = "DriverLicenseOwnYears Invalid Character")]
        public int DriverLicenseOwnYears { get; set; }

        [RegularExpression("^[0-9]{1}$", ErrorMessage = "DriverNoOfAccidents Invalid Character")]
        public int DriverNoOfAccidents { get; set; }

    }

    #endregion 'Quote Request Class Properties' 

    #region 'Quote Response'
    public class QuoteResponse
    {
        public string RequestReferenceNo { get; set; }
        public int InsuranceCompanyCode { get; set; }
        public DateTime QuotationExpiryDate { get; set; }
        public RespDriverDetails[] DriverDetails { get; set; }
        public long LesseeID { get; set; }
        public int VehicleUsagePercentage { get; set; }
        public string LesseeDateOfBirthG { get; set; }
        public string LesseeDateOfBirthH { get; set; }
        public int LesseeGender { get; set; }
        public int NCDEligibility { get; set; }
        public NajmCaseDetails[] NajmCaseDetails { get; set; }
        public CompQuotes[] CompQuotes { get; set; }


        public bool Status { get; set; }
        public List<Error> errors { get; set; }
    }

    public class CompQuotes
    {
        public long QuoteReferenceNo { get; set; }
        public int PolicyTitleID { get; set; }
        public Deductibles[] Deductibles { get; set; }
        public PolicyPremiumFeatures[] PolicyPremiumFeatures { get; set; }
        public CustomizedParameter[] CustomizedParameter { get; set; }
    }

    public class RespDriverDetails
    {
        public long DriverID { get; set; }
        public string DriverName { get; set; }
        public int VehicleUsagePercentage { get; set; }
        public string DriverDateOfBirthG { get; set; }
        public string DriverDateOfBirthH { get; set; }
        public int DriverGender { get; set; }
        public int NCDEligibility { get; set; }
    }

    public class Deductibles
    {
        public int DeductibleAmount { get; set; }
        public decimal PolicyPremium { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal BasePremium { get; set; }
        public decimal TechRate { get; set; }
        public PremiumBreakdown[] PremiumBreakdown { get; set; }
        public DynamicPremiumFeatures[] DynamicPremiumFeatures { get; set; }
        public Discounts[] Discounts { get; set; }
        public string DeductibleReferenceNo { get; set; }
    }

    public class PremiumBreakdown
    {
        public int BreakdownTypeID { get; set; }
        public decimal BreakdownAmount { get; set; }
        public double BreakdownPercentage { get; set; }
    }

    public class DynamicPremiumFeatures
    {
        public int FeatureID { get; set; }
        public int FeatureTypeID { get; set; }
        public decimal FeatureAmount { get; set; }
        public decimal FeatureTaxableAmount { get; set; }
    }
    public class PolicyPremiumFeatures
    {
        public int FeatureID { get; set; }
        public int FeatureTypeID { get; set; }
        public decimal FeatureAmount { get; set; }
        public decimal FeatureTaxableAmount { get; set; }
    }

    public class Discounts
    {
        public int DiscountTypeID { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
    }

    public class Error
    {
        public string field { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }

    #endregion 'Quote Response'

    public class ErrorDetails
    {
        public bool Status { get; set; }
        public List<Error> Errors { get; set; }
    }

    public class GetTransformationDetails
    {
        public int Flag { get; set; }
        public string Source { get; set; }
        public string SourceCode { get; set; }
        public string SourceType { get; set; }
        public string ReferenceNo { get; set; }
    }

    public class StopQuoteEntity
    {
        public int BodyType { get; set; }
        public string VehNationality { get; set; }
        public string City { get; set; }
        public string SchemeCode { get; set; }
        public int VehMgfYear { get; set; }
        public DateTime? DOB { get; set; }
        public int VehUse { get; set; }
    }
    #region  tarriff
    //public class TrfProcedureDetails
    //{
    //    public string ProductCode { get; set; }
    //    public int? SumInsured { get; set; }
    //    public DateTime EffectiveDate { get; set; }
    //    public string SchemeCode { get; set; }
    //    public string City { get; set; }

    //    //Changes Start By Sagar 14092021
    //    //public int DrvrGender { get; set; }
    //    //public DateTime? DrvrDOB { get; set; }
    //    //Changes End By Sagar 14092021
    //    public int Manufacturer { get; set; }
    //    public int MfgYear { get; set; }
    //    public int BodyType { get; set; }
    //    public int VehicleColor { get; set; }

    //    public string VehicleUse { get; set; }

    //    public int? RepairCondition { get; set; }
    //    public int LoyaltyFlag { get; set; }
    //    public string UniqueIdentifier { get; set; }
    //    public string PolicyholderNCDCode { get; set; }

    //    //Changes Start By Sagar 14092021
    //    //public string[] DriverNCDCode { get; set; }
    //    //Changes End By Sagar 14092021
    //    public string SplDst { get; set; }

    //    //Changes Start By Sagar 14092021
    //    //public long InsuredId { get; set; }
    //    //Changes End By Sagar 14092021
    //    public string VehicleChassisNumber { get; set; }
    //    public int? MaritalStatus { get; set; }
    //    public int VehicleModel { get; set; }

    //    //Changes Start By Sagar 14092021
    //    public long? PolicyholderNationalID { get; set; }
    //    public string PolicyholderDOB { get; set; }
    //    public int? PolicyholderGender { get; set; }
    //    public TariffDriverDetails[] DriverDetails { get; set; }

    //    //Changes End By Sagar 14092021

    //}
    #endregion  tarriff
    public class TrfProcedureDetails
    {
        public string Reqt_Ref_No { get; set; }
        public int Make_Code { get; set; }
        public int? Model_Code { get; set; }
        public int? Body_Type_Code { get; set; }

        //public string VEHICLE_MAKE_CODE { get; set; }
        public int Production_Year { get; set; }
        public int Driver_Age { get; set; }

        //public string REGION_ID { get; set; }
        public int? Gender { get; set; }
        public int NCD_Level { get; set; }
        public int Ncd_Driver_Level { get; set; }
        public int Tat { get; set; }

        //public int LOYALTY { get; set; }
        public string City { get; set; }


        public long Driver_Nationality { get; set; }
        public int? ZipCode { get; set; }
        public string VehicleMakeTextNIC { get; set; }
        public string VehicleModelTextNIC { get; set; }
        public DateTime Policy_Effective_Date { get; set; }
        public long PolicyHolder_Id { get; set; }
        public long? Vehicle_Sequence_Number { get; set; }
        public long? VehicleCustome_Id { get; set; }
        public long? SumInsured { get; set; }
        public long? Deductible { get; set; }
        public string REPAIRTYPE { get; set; }
    }

    #region 'QueryFeature Request'

    public class QueryFeatureRequest
    {
        [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "Invalid PolicyReferenceNo")]
        public long PolicyReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid QueryRequestReferenceNo")]
        public long QueryRequestReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Invalid InsuranceCompanyCode")]
        public int InsuranceCompanyCode { get; set; }

        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid LesseeID")]
        public long LesseeID { get; set; }

        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid LessorID")]
        public long LessorID { get; set; }

        [Required, RegularExpression("^[a-zA-Z0-9/-]*$", ErrorMessage = "PolicyNumber Invalid Character"), StringLength(100, ErrorMessage = "PolicyNumber Invalid Length")]
        public string PolicyNumber { get; set; }

        [RegularExpression("^(?:[0-9]{1,12})$", ErrorMessage = "Invalid VehicleSequenceNumber")]
        public long? VehicleSequenceNumber { get; set; }

        [RegularExpression("^(?:[0-9]{1,12})$", ErrorMessage = "Invalid VehicleCustomID")]
        public long? VehicleCustomID { get; set; }

        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "VehicleVIN Invalid Character"), StringLength(17, ErrorMessage = "VehicleVIN Invalid Length")]
        public string VehicleVIN { get; set; }

        public CustomizedParameter[] CustomizedParameter { get; set; }

    }

    #endregion 'QueryFeature Request'

    #region 'QueryFeature Response'

    public class QueryFeatureResponse
    {
        public long PolicyReferenceNo { get; set; }
        public long QueryRequestReferenceNo { get; set; }
        public long QueryResponseReferenceNo { get; set; }
        public int InsuranceCompanyCode { get; set; }
        public DynamicPremiumFeatures[] PolicyPremiumFeatures { get; set; }
        public bool Status { get; set; }
        public List<Error> errors { get; set; }
        public CustomizedParameter[] CustomizedParameter { get; set; }
    }

    #endregion 'QueryFeature Response'
    #region purchase feature
    public class PurchaseFeatureRequest
    {
        [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "Invalid PolicyReferenceNo")]
        public long PolicyReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "Invalid QueryRequestReferenceNo")]
        public long QueryRequestReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "Invalid QueryResponseReferenceNo")]
        public long QueryResponseReferenceNo { get; set; }

        [Required, RegularExpression(@"^(\d{1,8})(\.\d{1,2})?$", ErrorMessage = "AdditionalPremium Invalid Character")]
        public decimal? AdditionalPremium { get; set; }

        public AddedFeatures[] AddedFeatures { get; set; }
        public CustomizedParameter[] CustomizedParameter { get; set; }
    }
    public class AddedFeatures
    {
        [Required, RegularExpression("^[0-9]*$", ErrorMessage = "FeatureID Invalid Character")]
        public int FeatureID { get; set; }

        [Required, RegularExpression(@"^(\d{1,6})(\.\d{1,2})?$", ErrorMessage = "FeatureAmount Invalid Character")]
        public decimal FeatureAmount { get; set; }

        [Required, RegularExpression(@"^(\d{1,6})(\.\d{1,2})?$", ErrorMessage = "FeatureTaxableAmount Invalid Character")]
        public decimal FeatureTaxableAmount { get; set; }
    }
    #region 'PurchaseFeature Response'

    public class PurchaseFeatureResponse
    {
        public bool Status { get; set; }
        public List<Error> errors { get; set; }

        //Change Start By Sagar 01042021
        public long EndorsementReferenceNo { get; set; }
        //Change End By Sagar 01042021
    }

    #endregion
    #endregion

    #region 'Add Driver Request'
    public class AddDriverRequest
    {
        [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "Invalid PolicyReferenceNo")]
        public long PolicyReferenceNo { get; set; }

        //[Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid AddDriverRequestReferenceNo")]
        [Required, RegularExpression("^[a-zA-Z0-9/-]*$", ErrorMessage = "AddDriverRequestReferenceNo Invalid Character"), StringLength(13, ErrorMessage = "AddDriverRequestReferenceNo Invalid Length")]
        public string AddDriverRequestReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Invalid InsuranceCompanyCode")]
        public int InsuranceCompanyCode { get; set; }

        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid LesseeID")]
        public long LesseeID { get; set; }

        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid LessorID")]
        public long LessorID { get; set; }

        [Required, RegularExpression("^[a-zA-Z0-9/-]*$", ErrorMessage = "PolicyNumber Invalid Character"), StringLength(100, ErrorMessage = "PolicyNumber Invalid Length")]
        public string PolicyNumber { get; set; }

        [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "VehicleUsagePercentage Invalid Character")]
        public int VehicleUsagePercentage { get; set; }

        [RegularExpression("^(?:[0-9]{1,12})$", ErrorMessage = "Invalid VehicleSequenceNumber")]
        public long? VehicleSequenceNumber { get; set; }

        [RegularExpression("^(?:[0-9]{1,12})$", ErrorMessage = "Invalid VehicleCustomID")]
        public long? VehicleCustomID { get; set; }

        public DriverDetails[] DriverDetails { get; set; }
        public CustomizedParameter[] CustomizedParameter { get; set; }
    }
    public class AddDriverResponse
    {
        public long PolicyReferenceNo { get; set; }
        //public long AddDriverRequestReferenceNo { get; set; }
        public string AddDriverRequestReferenceNo { get; set; }//changes made by raju on 06-09-2024
        //public long AddDriverResponseReferenceNo { get; set; }
        public string AddDriverResponseReferenceNo { get; set; }
        public int InsuranceCompanyCode { get; set; }
        public DriversBreakdown[] DriversBreakdown { get; set; }
        public decimal AdditionalPremium { get; set; }
        public bool Status { get; set; }
        public List<Error> errors { get; set; }

        public CustomizedParameter[] CustomizedParameter { get; set; }
    }
    public class DriversBreakdown
    {
        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid DriverID")]
        public long DriverID { get; set; }
        public bool IsNew { get; set; }

        [Required, RegularExpression("^[ a-zA-Z\u0600-\u06FF.-]*$", ErrorMessage = "DriverName Invalid Character"), StringLength(150, ErrorMessage = "DriverName Invalid Length")]
        public string DriverName { get; set; }

        [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Invalid VehicleUsagePercentage")]
        public int VehicleUsagePercentage { get; set; }

        [RegularExpression("^[0-9-/]*$", ErrorMessage = "DriverDateOfBirthG Invalid Character"), StringLength(10, ErrorMessage = "DriverDateOfBirthG Invalid Length")]
        public string DriverDateOfBirthG { get; set; }

        [RegularExpression("^[0-9-/]*$", ErrorMessage = "DriverDateOfBirthH Invalid Character"), StringLength(10, ErrorMessage = "DriverDateOfBirthH Invalid Length")]
        public string DriverDateOfBirthH { get; set; }

        [Required, RegularExpression("^[0-9]{1}$", ErrorMessage = "Invalid DriverGender")]
        public int DriverGender { get; set; }

        //[Required, RegularExpression("^[0-9]{1}$", ErrorMessage = "Invalid NCDEligibility")]
        //public int NCDEligibility { get; set; }

        [Required, RegularExpression(@"^(\d{1,6})(\.\d{1,2})?$", ErrorMessage = "DriverAmount Invalid Character")]
        public decimal DriverAmount { get; set; }
    }
    public class ConfirmAddDriverRequest
    {
        [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Invalid InsuranceCompanyCode")]
        public int InsuranceCompanyCode { get; set; }
        [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "Invalid PolicyReferenceNo")]
        public long PolicyReferenceNo { get; set; }
        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid LesseeID")]
        public long LesseeID { get; set; }

        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid LessorID")]
        public long LessorID { get; set; }

        [Required, RegularExpression("^[a-zA-Z0-9/-]*$", ErrorMessage = "PolicyNumber Invalid Character"), StringLength(100, ErrorMessage = "PolicyNumber Invalid Length")]
        public string PolicyNumber { get; set; }

        //[Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid AddDriverRequestReferenceNo")]
        [Required, RegularExpression("^[a-zA-Z0-9/-]*$", ErrorMessage = "AddDriverRequestReferenceNo Invalid Character"), StringLength(13, ErrorMessage = "AddDriverRequestReferenceNo Invalid Length")]
        public string AddDriverRequestReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "Invalid AddDriverResponseReferenceNo")]
        public string AddDriverResponseReferenceNo { get; set; }

        public DriversBreakdown[] DriversBreakdown { get; set; }

        [Required, RegularExpression(@"^(\d{1,8})(\.\d{1,2})?$", ErrorMessage = "AdditionalPremium Invalid Character")]
        public decimal AdditionalPremium { get; set; }
        public CustomizedParameter[] CustomizedParameter { get; set; }
    }

    public class ConfirmAddDriverResponse
    {
        public bool Status { get; set; }
        public List<Error> errors { get; set; }
        public string EndorsementReferenceNo { get; set; }
        #endregion

        #region IcUpdatePolicy
        public class IcUpdatePolicyRequest
        {
            [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "InsuranceCompanyCode Invalid Character")]
            public int InsuranceCompanyCode { get; set; }
            [Required, RegularExpression("^[ a-zA-Z0-9-]*$", ErrorMessage = "PolicyRequestReferenceNo Invalid Character"), StringLength(20, ErrorMessage = "RequestReferenceNo Invalid Length")]
            public string PolicyRequestReferenceNo { get; set; }

            [Required, RegularExpression("^[ a-zA-Z0-9-]*$", ErrorMessage = "RequestReferenceNo Invalid Character"), StringLength(20, ErrorMessage = "RequestReferenceNo Invalid Length")]
            public string RequestReferenceNo { get; set; }

            [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "QuoteReferenceNo Invalid Character")]
            public long QuoteReferenceNo { get; set; }

            [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "PolicyReferenceNo Invalid Character")]
            public long PolicyReferenceNo { get; set; }

            [Required, RegularExpression("^[a-zA-Z0-9/-]*$", ErrorMessage = "PolicyNumber Invalid Character"), StringLength(100, ErrorMessage = "PolicyNumber Invalid Length")]
            public string PolicyNumber { get; set; }

            [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "LessorID Invalid Character")]
            public long LessorID { get; set; }

            [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "LesseeID Invalid Character")]
            public long LesseeID { get; set; }

            [RegularExpression("^[0-9]{1,12}$", ErrorMessage = "Invalid VehicleSequenceNumber")]
            public long? VehicleSequenceNumber { get; set; }

            [RegularExpression("^(?:[0-9]{1,12})$", ErrorMessage = "Invalid VehicleCustomID")]
            public long? VehicleCustomID { get; set; }

            [RegularExpression("^(?:[0-9]{1,2})$", ErrorMessage = "Invalid VehiclePlateTypeID")]
            public int? VehiclePlateTypeID { get; set; }

            [RegularExpression("^(?:[0-9]{1,4})$", ErrorMessage = "Invalid VehiclePlateNumber")]
            public int? VehiclePlateNumber { get; set; }

            [RegularExpression("^(?:[0-9]{1,2})$", ErrorMessage = "Invalid FirstPlateLetterID")]
            public int? FirstPlateLetterID { get; set; }

            [RegularExpression("^(?:[0-9]{1,2})$", ErrorMessage = "Invalid SecondPlateLetterID")]
            public int? SecondPlateLetterID { get; set; }

            [RegularExpression("^(?:[0-9]{1,2})$", ErrorMessage = "Invalid ThirdPlateLetterID")]
            public int? ThirdPlateLetterID { get; set; }

            [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email Invalid Character"), StringLength(256, ErrorMessage = "Email Invalid Length")]
            public string Email { get; set; }

            [RegularExpression("^[0-9]*$", ErrorMessage = "MobileNo Invalid Character"), StringLength(10, ErrorMessage = "MobileNo Invalid Length")]
            public string MobileNo { get; set; }

            public NationalAddress[] NationalAddress { get; set; }

            [Required, RegularExpression("^[0-9]{1,2}$", ErrorMessage = "Invalid UpdatePolicyReasonID")]
            public int UpdatePolicyReasonID { get; set; }

        }

        public class IcUpdatePolicyResponse
        {
            public bool Status { get; set; }
            public List<Error> errors { get; set; }
        }
        #endregion

        public class IcCancelPolicyRequest
        {
            [Required, RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "PolicyRequestReferenceNo Invalid Character"), StringLength(13, ErrorMessage = "PolicyRequestReferenceNo Invalid Length")]
            public string PolicyRequestReferenceNo { get; set; }

            [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "InsuranceCompanyCode Invalid Character")]
            public int InsuranceCompanyCode { get; set; }

            [Required, RegularExpression("^[ a-zA-Z0-9-]*$", ErrorMessage = "RequestReferenceNo Invalid Character"), StringLength(20, ErrorMessage = "RequestReferenceNo Invalid Length")]
            public string RequestReferenceNo { get; set; }

            [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "QuoteReferenceNo Invalid Character")]
            public long QuoteReferenceNo { get; set; }

            [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "PolicyReferenceNo Invalid Character")]
            public long PolicyReferenceNo { get; set; }

            [Required, RegularExpression("^[a-zA-Z0-9/-]*$", ErrorMessage = "PolicyNumber Invalid Character"), StringLength(100, ErrorMessage = "PolicyNumber Invalid Length")]
            public string PolicyNumber { get; set; }

            [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "LessorID Invalid Character")]
            public long LessorID { get; set; }

            [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "LesseeID Invalid Character")]
            public long LesseeID { get; set; }

            [Required, RegularExpression("^(?:[0-9]{1,12})$", ErrorMessage = "Invalid VehicleSequenceNumber")]
            public long? VehicleSequenceNumber { get; set; }

            [RegularExpression("^(?:[0-9]{1,12})$", ErrorMessage = "Invalid VehicleCustomID")]
            public long? VehicleCustomID { get; set; }

            // [Required, RegularExpression("^[a-zA-Z0-9/-]*$", ErrorMessage = "PolicyNumber Invalid Character"), StringLength(100, ErrorMessage = "PolicyNumber Invalid Length")]
            public string CancellationRequestTime { get; set; }

            [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "CancellationReason Invalid Character")]
            public int CancellationReason { get; set; }
            [Required]
            //public PolicyDocument[] PolicyDocument { get; set; }
            public CustomizedParameter[] CustomizedParameter { get; set; }
        }
        public class IcCancelPolicyResponse
        {
            public bool Status { get; set; }
            public List<Error> errors { get; set; }
        }
    }


    public class SavePolicyDetails
    {
        public long QuoteReferenceNo { get; set; }
        public string ReqtReferenceNo { get; set; }
        public int InsuranceCompanyCode { get; set; }
        public int InsuranceTypeId { get; set; }
        public string OldPolicyNumber { get; set; }
        public string NewPolicyNumber { get; set; }
        public CustomizedParameter[] customizedParameters { get; set; }
        public string PolicyReqtRefNo { get; set; }
        public long? PolicyRefNo { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string updatedBy { get; set; }
    }

    public class DriverBrtDetails
    {
        public DateTime? DriverBirthDate { get; set; }
    }

    //Changes Start By Sagar 14092021
    public class TariffDriverDetails
    {
        public int Serial { get; set; }
        public long DriverID { get; set; }
        public string DriverNCDCode { get; set; }
        public string DriverDateOfBirth { get; set; }
        public int DriverGender { get; set; }
        public int DriverMaritalStatus { get; set; }
    }
    //Changes End By Sagar 14092021

    public class StaticValues
    {
        public int MinimumDrivingAge { get; set; }
        public string CreatedBy { get; set; }
        public long MaxLiabilityTPL { get; set; }
        public string Aggregator { get; set; }
        public string CustomizedParamereList { get; set; }
        public long MaxLiabilityCOMP { get; set; }
        public int PolicyExpiryDays { get; set; }
        public int LoyaltyDiscountPer { get; set; }
        public int AddDiscountPer { get; set; }
        //changes start made by raju 09_22_2023
        public string AddDiscountEskaID { get; set; }
        public decimal SAMADiscountEskaID { get; set; }
        //changes start made by raju 09_22_2023
    }
}


