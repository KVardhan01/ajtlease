using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AJT_Leasing_API.Models
{
    public class PolicyIssueEntity
    {
    }

    #region 'Policy Issue Request'
    public class PolicyIssueRequest
    {
        [Required, RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "PolicyRequestReferenceNo Invalid Character"), StringLength(13, ErrorMessage = "PolicyRequestReferenceNo Invalid Length")]
        public string PolicyRequestReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Invalid InsuranceCompanyCode")]
        public int InsuranceCompanyCode { get; set; }

        [Required, RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "RequestReferenceNo Invalid Character"), StringLength(13, ErrorMessage = "RequestReferenceNo Invalid Length")]
        public string RequestReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "QuoteReferenceNo Invalid Character")]
        public long QuoteReferenceNo { get; set; }

        public PolicyIssueDetails Details { get; set; }
    }

    public class PolicyIssueDetails
    {
        [StringLength(256, ErrorMessage = "Email Invalid Length")]
        public string Email { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Mobile Invalid Character"), StringLength(10, ErrorMessage = "Mobile Invalid Length")]
        public string MobileNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid LesseeID")]
        public long LesseeID { get; set; }

        [Required, RegularExpression("^[0-9-/]*$", ErrorMessage = "PolicyEffectiveDate Invalid Character"), StringLength(10, ErrorMessage = "PolicyEffectiveDate Invalid Length")]
        public string PolicyEffectiveDate { get; set; }

        //[RegularExpression("^[ a-zA-Z0-9-/]*$", ErrorMessage = "PolicyNumber Invalid Character"), StringLength(100, ErrorMessage = "PolicyNumber Invalid Length")]
        //public string PolicyNumber { get; set; }

        [RegularExpression("^[ a-zA-Z0-9]*$", ErrorMessage = "DeductibleReferenceNo Invalid Character"), StringLength(20, ErrorMessage = "DeductibleReferenceNo Invalid Length")]
        public string DeductibleReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,5}$", ErrorMessage = "Invalid DeductibleAmount")]
        public int DeductibleAmount { get; set; }

        [Required, RegularExpression(@"^(\d{1,8})(\.\d{1,2})?$", ErrorMessage = "PaidAmount Invalid Character")]
        public decimal PaidAmount { get; set; }

        [Required, RegularExpression(@"^(\d{1,8})(\.\d{1,2})?$", ErrorMessage = "BasePremium Invalid Character")]
        public decimal BasePremium { get; set; }
        public PremiumFeatures[] PolicyPremiumFeatures { get; set; }
        public PremiumFeatures[] DynamicPremiumFeatures { get; set; }

        [Required, RegularExpression("^[1-2]{1}$", ErrorMessage = "Invalid VehicleUniqueTypeID")]
        public int VehicleUniqueTypeID { get; set; }

        [RegularExpression("^(?:[0-9]{1,12})$", ErrorMessage = "Invalid VehicleSequenceNumber")]
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

        [Required, RegularExpression("^[ a-zA-Z0-9-]*$", ErrorMessage = "VehicleVIN Invalid Character"), StringLength(17, ErrorMessage = "VehicleVIN Invalid Length")]
        public string VehicleVIN { get; set; }

        [RegularExpression("^[0-9-/]*$", ErrorMessage = "VehicleRegistrationExpiryDate Invalid Character"), StringLength(10, ErrorMessage = "VehicleRegistrationExpiryDate Invalid Length")]
        public string VehicleRegistrationExpiryDate { get; set; }

        [Required]
        public bool IsVehicleOwnerTransfer { get; set; }

        [RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Invalid VehicleOwnerIdentityNo")]
        public long VehicleOwnerIdentityNo { get; set; }

        [RegularExpression("^(?:[0-9]{1,5})$", ErrorMessage = "Invalid VehicleWeight")]
        public int? VehicleWeight { get; set; }

        [RegularExpression("^(?:[0-9]{1,3})$", ErrorMessage = "Invalid VehicleBodyCode")]
        public int? VehicleBodyCode { get; set; }

        public CustomizedParameter[] CustomizedParameter { get; set; }

    }

    #endregion 'Policy Issue Request'

    #region 'Policy Issue Response'
    public class PolicyIssueResponse
    {
        public long PolicyReferenceNo { get; set; }
        //public string PolicyNumber { get; set; }
        public string PolicyEffectiveDate { get; set; }
        public string PolicyExpiryDate { get; set; }
        public string PolicyFileUrl { get; set; }
        public bool Status { get; set; }
        public List<Error> errors { get; set; }
    }

    #endregion 'Policy Issue Response'
}