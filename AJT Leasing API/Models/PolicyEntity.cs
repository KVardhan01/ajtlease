using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AJT_Leasing_API.Models
{
    public class PolicyEntity
    {
    }

    public class PolicyRequest
    {
        [Required, RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "PolicyRequestReferenceNo Invalid Character"), StringLength(13, ErrorMessage = "PolicyRequestReferenceNo Invalid Length")]
        public string PolicyRequestReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,3}$", ErrorMessage = "Invalid InsuranceCompanyCode")]
        public int InsuranceCompanyCode { get; set; }

        [Required, RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "RequestReferenceNo Invalid Character"), StringLength(13, ErrorMessage = "RequestReferenceNo Invalid Length")]
        public string RequestReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,15}$", ErrorMessage = "QuoteReferenceNo Invalid Character")]
        public long QuoteReferenceNo { get; set; }

        public PolicyDetails Details { get; set; }



    }

    public class PolicyDetails
    {
        [RegularExpression("^[a-zA-Z0-9-]*$", ErrorMessage = "DeductibleReferenceNo Invalid Character"), StringLength(20, ErrorMessage = "DeductibleReferenceNo Invalid Length")]
        public string DeductibleReferenceNo { get; set; }

        [Required, RegularExpression("^[0-9]{1,5}$", ErrorMessage = "DeductibleAmount Invalid Character")]
        public long DeductibleAmount { get; set; }

        public PremiumFeatures[] PolicyPremiumFeatures { get; set; }
        public PremiumFeatures[] DynamicPremiumFeatures { get; set; }
        public CustomizedParameter[] CustomizedParameter { get; set; }
    }

    public class PremiumFeatures
    {
        [Required, RegularExpression("^[0-9]*$", ErrorMessage = "FeatureID Invalid Character")]
        public int FeatureID { get; set; }

        [Required, RegularExpression("^[1-3]{1}$", ErrorMessage = "FeatureTypeID Invalid Character")]
        public int FeatureTypeID { get; set; }

        [RegularExpression(@"^(\d{1,6})(\.\d{1,2})?$", ErrorMessage = "FeatureAmount Invalid Character")]
        public decimal? FeatureAmount { get; set; }

        [RegularExpression(@"^(\d{1,6})(\.\d{1,2})?$", ErrorMessage = "FeatureTaxableAmount Invalid Character")]
        public decimal? FeatureTaxableAmount { get; set; }
    }

    public class PolicyResponse
    {
        public bool Status { get; set; }
        public List<Error> errors { get; set; }
    }
}