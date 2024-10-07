using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using static AJT_Leasing_API.Models.ConfirmAddDriverResponse;

namespace AJT_Leasing_API.Models
{
    public class CommonValidation
    {
        string Mandotaryfield = "field_missing", invalidVal = "invalid_input", invalid_PremVal = "invalid_premium", stop_Quotation = "invalidPremium", VehSumInsured = "invalidSumInsured";
        Error[] errors;
        List<Error> Error = new List<Error>();

        //Changes start by sagar 27-04-2021
        string SaveErrorResp = string.Empty;
        DataAccessLayer dataAccessLayer = new DataAccessLayer();
        //Changes start by sagar 27-04-2021


        #region 'Quote Request Validation'
        public List<Error> Validation(QuoteDetails quoteDetails)
        {
            try
            {
                var validationResultQuoteDetails = DataAnnotationQuoteDetails.ValidateEntity<QuoteDetails>(quoteDetails);
                if (validationResultQuoteDetails.HasError)
                {
                    errors = new Error[validationResultQuoteDetails.ValidationErrors.Count];
                    for (int i = 0; i < validationResultQuoteDetails.ValidationErrors.Count; i++)
                    {
                        errors[i] = new Error();
                        errors[i].field = validationResultQuoteDetails.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
                        errors[i].message = validationResultQuoteDetails.ValidationErrors[i].ToString();
                        errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                        Error.Add(errors[i]);
                    }
                }

                var validationResultDetails = DataAnnotationDetails.ValidateEntity<Details>(quoteDetails.Details);
                if (validationResultDetails.HasError)
                {
                    errors = new Error[validationResultDetails.ValidationErrors.Count];
                    for (int i = 0; i < validationResultDetails.ValidationErrors.Count; i++)
                    {
                        errors[i] = new Error();
                        errors[i].field = validationResultDetails.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
                        errors[i].message = validationResultDetails.ValidationErrors[i].ErrorMessage.ToString();
                        errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                        Error.Add(errors[i]);
                    }
                }


                if (quoteDetails.Details.LessorNationalAddress != null)
                {
                    for (int i = 0; i < (quoteDetails.Details.LessorNationalAddress.Length); i++)
                    {
                        var validationResultNationalAddressDetails = DataAnnotationNationalAddressDetails.ValidateEntity<NationalAddress>(quoteDetails.Details.LessorNationalAddress[i]);

                        if (validationResultNationalAddressDetails.HasError)
                        {
                            errors = new Error[validationResultNationalAddressDetails.ValidationErrors.Count];
                            for (int j = 0; j < validationResultNationalAddressDetails.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultNationalAddressDetails.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultNationalAddressDetails.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
                if (quoteDetails.Details.CustomizedParameter != null)
                {
                    for (int i = 0; i < (quoteDetails.Details.CustomizedParameter.Length); i++)
                    {
                        var validationResultCustomizedParameter = DataAnnotationCustomizedParameter.ValidateEntity<CustomizedParameter>(quoteDetails.Details.CustomizedParameter[i]);

                        if (validationResultCustomizedParameter.HasError)
                        {
                            errors = new Error[validationResultCustomizedParameter.ValidationErrors.Count];
                            for (int j = 0; j < validationResultCustomizedParameter.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultCustomizedParameter.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultCustomizedParameter.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
                if (quoteDetails.Details.CountriesValidDrivingLicense != null)
                {
                    for (int i = 0; i < (quoteDetails.Details.CountriesValidDrivingLicense.Length); i++)
                    {
                        var validationResultCountriesValidDrivingLicense = DataAnnotationCountriesValidDrivingLicense.ValidateEntity<CountriesValidDrivingLicense>(quoteDetails.Details.CountriesValidDrivingLicense[i]);

                        if (validationResultCountriesValidDrivingLicense.HasError)
                        {
                            errors = new Error[validationResultCountriesValidDrivingLicense.ValidationErrors.Count];
                            for (int j = 0; j < validationResultCountriesValidDrivingLicense.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultCountriesValidDrivingLicense.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultCountriesValidDrivingLicense.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
                if (quoteDetails.Details.NajmCaseDetails != null)
                {
                    for (int i = 0; i < (quoteDetails.Details.NajmCaseDetails.Length); i++)
                    {
                        var validationResultNajmCaseDetails = DataAnnotationNajmCaseDetails.ValidateEntity<NajmCaseDetails>(quoteDetails.Details.NajmCaseDetails[i]);

                        if (validationResultNajmCaseDetails.HasError)
                        {
                            errors = new Error[validationResultNajmCaseDetails.ValidationErrors.Count];
                            for (int j = 0; j < validationResultNajmCaseDetails.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultNajmCaseDetails.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultNajmCaseDetails.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
                if (quoteDetails.Details.LesseeNationalAddress != null)
                {
                    for (int i = 0; i < (quoteDetails.Details.LesseeNationalAddress.Length); i++)
                    {
                        var validationResultNationalAddress = DataAnnotationNationalAddressDetails.ValidateEntity<NationalAddress>(quoteDetails.Details.LesseeNationalAddress[i]);

                        if (validationResultNationalAddress.HasError)
                        {
                            errors = new Error[validationResultNationalAddress.ValidationErrors.Count];
                            for (int j = 0; j < validationResultNationalAddress.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultNationalAddress.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultNationalAddress.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
                if (quoteDetails.Details.DriverDetails != null)
                {
                    for (int i = 0; i < (quoteDetails.Details.DriverDetails.Length); i++)
                    {
                        var validationResultDriverDetails = DataAnnotationNationalDriverDetails.ValidateEntity<DriverDetails>(quoteDetails.Details.DriverDetails[i]);

                        if (validationResultDriverDetails.HasError)
                        {
                            errors = new Error[validationResultDriverDetails.ValidationErrors.Count];
                            for (int j = 0; j < validationResultDriverDetails.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultDriverDetails.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultDriverDetails.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }

                Error.AddRange(OtherValidation(quoteDetails));
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, quoteDetails.RequestReferenceNo, string.Empty, "Validation");
                errors = new Error[1];
                errors[1].field = "";
                errors[1].message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                errors[1].code = "unexpected_error";
                Error.Add(errors[1]);
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), ex.ToString(), "Validation", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
            }
            return Error;
        }

        public List<Error> OtherValidation(QuoteDetails quoteDetails)
        {
            Error error = new Error();
            List<Error> ErrorList = new List<Error>();

            try
            {
                if (quoteDetails.Details.LessorNationalAddress == null)
                {
                    ErrorList.Add(ErrorDetails(invalidVal, "LessorNationalAddress", quoteDetails.RequestReferenceNo, string.Empty));
                }

                if (quoteDetails.Details.LesseeNationalAddress == null)
                {
                    ErrorList.Add(ErrorDetails(invalidVal, "LesseeNationalAddress", quoteDetails.RequestReferenceNo, string.Empty));
                }
                if (quoteDetails.Details.IsRenewal && string.IsNullOrEmpty(quoteDetails.Details.PolicyNumber))
                {
                    ErrorList.Add(ErrorDetails(invalidVal, "PolicyNumber", quoteDetails.RequestReferenceNo, string.Empty));
                }
                if (string.IsNullOrEmpty(quoteDetails.Details.EnglishFirstName) && string.IsNullOrEmpty(quoteDetails.Details.ArabicFirstName))
                {
                    ErrorList.Add(ErrorDetails(invalidVal, "ArabicFirstName", quoteDetails.RequestReferenceNo, string.Empty));
                    ErrorList.Add(ErrorDetails(invalidVal, "EnglishFirstName", quoteDetails.RequestReferenceNo, string.Empty));
                }
                if (quoteDetails.Details.LesseeID.ToString().StartsWith("2"))
                {
                    if (string.IsNullOrEmpty(quoteDetails.Details.LesseeDateOfBirthG))
                    {
                        ErrorList.Add(ErrorDetails(invalidVal, "LesseeDateOfBirthG", quoteDetails.RequestReferenceNo, string.Empty));
                    }
                    if (quoteDetails.Details.LesseeNationalityID == null)
                    {
                        ErrorList.Add(ErrorDetails(invalidVal, "LesseeNationalityID", quoteDetails.RequestReferenceNo, string.Empty));
                    }
                }
                if (quoteDetails.Details.LesseeID.ToString().StartsWith("1") && string.IsNullOrEmpty(quoteDetails.Details.LesseeDateOfBirthH))
                {
                    ErrorList.Add(ErrorDetails(invalidVal, "LesseeDateOfBirthH", quoteDetails.RequestReferenceNo, string.Empty));
                }
                if (quoteDetails.Details.DriverDetails != null)
                {
                    for (int i = 0; i < quoteDetails.Details.DriverDetails.Length; i++)
                    {

                        if (string.IsNullOrEmpty(quoteDetails.Details.DriverDetails[i].EnglishFirstName) && string.IsNullOrEmpty(quoteDetails.Details.DriverDetails[i].ArabicFirstName))
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].ArabicFirstName", quoteDetails.RequestReferenceNo, string.Empty));
                            ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].EnglishFirstName", quoteDetails.RequestReferenceNo, string.Empty));
                        }
                        if (string.IsNullOrEmpty(quoteDetails.Details.DriverDetails[i].EnglishMiddleName) && string.IsNullOrEmpty(quoteDetails.Details.DriverDetails[i].ArabicMiddleName))
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].ArabicMiddleName", quoteDetails.RequestReferenceNo, string.Empty));
                            ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].EnglishMiddleName", quoteDetails.RequestReferenceNo, string.Empty));
                        }
                        if (string.IsNullOrEmpty(quoteDetails.Details.DriverDetails[i].EnglishLastName) && string.IsNullOrEmpty(quoteDetails.Details.DriverDetails[i].ArabicLastName))
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].ArabicLastName", quoteDetails.RequestReferenceNo, string.Empty));
                            ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].EnglishLastName", quoteDetails.RequestReferenceNo, string.Empty));
                        }


                        if (quoteDetails.Details.DriverDetails[i].DriverID.ToString().StartsWith("2"))
                        {
                            if (string.IsNullOrEmpty(quoteDetails.Details.DriverDetails[i].DriverDateOfBirthG))
                            {
                                ErrorList.Add(ErrorDetails(invalidVal, "DriverDateOfBirthG", quoteDetails.RequestReferenceNo, string.Empty));
                            }
                            if (quoteDetails.Details.DriverDetails[i].DriverNationalityID == null)
                            {
                                ErrorList.Add(ErrorDetails(invalidVal, "DriverNationalityID", quoteDetails.RequestReferenceNo, string.Empty));
                            }
                        }
                        if (quoteDetails.Details.DriverDetails[i].DriverID.ToString().StartsWith("1") && string.IsNullOrEmpty(quoteDetails.Details.DriverDetails[i].DriverDateOfBirthH))
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "DriverDateOfBirthH", quoteDetails.RequestReferenceNo, string.Empty));
                        }
                    }
                }
                if (quoteDetails.Details.PurposeofVehicleUseID != 1 && Convert.ToBoolean(ConfigurationManager.AppSettings["ValidateVehPurpose"]))
                {
                    ErrorList.Add(ErrorDetails("invalid_VehUsageId", "PurposeofVehicleUseID", quoteDetails.RequestReferenceNo, string.Empty));
                }
                //17-02-2021 Change Start By Sagar Set Validation To value > 300000
                //if (quoteDetails.Details.VehicleSumInsured > Convert.ToInt32(ConfigurationManager.AppSettings["MaxVehVal"]))
                //{
                //    ErrorList.Add(ErrorDetails(VehSumInsured, "VehicleSumInsured", quoteDetails.RequestReferenceNo, string.Empty));
                //}
                //17-02-2021 Change End By Sagar Set Validation To value > 300000

            }
            catch (Exception Ex)
            {
                ErrHandler.WriteError(Ex, quoteDetails.RequestReferenceNo, null, "OtherValidation");
                error.field = "";
                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                error.code = "unexpected_error";
                Error.Add(error);
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", quoteDetails.RequestReferenceNo, JsonConvert.SerializeObject(quoteDetails), Ex.ToString(), "OtherValidation", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
            }
            return ErrorList;
        }

        #endregion 'Quote Request Validation'

        #region 'Policy Request Validation'
        public List<Error> ValidatePolicy(PolicyRequest policyRequest)
        {
            try
            {
                var validationResultPolicy = DataAnnotationPolicy.ValidateEntity<PolicyRequest>(policyRequest);
                if (validationResultPolicy.HasError)
                {
                    errors = new Error[validationResultPolicy.ValidationErrors.Count];
                    for (int i = 0; i < validationResultPolicy.ValidationErrors.Count; i++)
                    {
                        errors[i] = new Error();
                        errors[i].field = validationResultPolicy.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
                        errors[i].message = validationResultPolicy.ValidationErrors[i].ToString();
                        errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                        Error.Add(errors[i]);
                    }

                }

                var validationPolicyDetailsResultDetails = DataAnnotationPolicyDetails.ValidateEntity<PolicyDetails>(policyRequest.Details);
                if (validationPolicyDetailsResultDetails.HasError)
                {
                    errors = new Error[validationPolicyDetailsResultDetails.ValidationErrors.Count];
                    for (int i = 0; i < validationPolicyDetailsResultDetails.ValidationErrors.Count; i++)
                    {
                        errors[i] = new Error();
                        errors[i].field = validationPolicyDetailsResultDetails.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
                        errors[i].message = validationPolicyDetailsResultDetails.ValidationErrors[i].ErrorMessage.ToString();
                        errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                        Error.Add(errors[i]);
                    }
                }

                if (policyRequest.Details.PolicyPremiumFeatures != null)
                {
                    for (int i = 0; i < (policyRequest.Details.PolicyPremiumFeatures.Length); i++)
                    {
                        var validationResultPremiumFeaturesDetails = DataAnnotationPremiumFeaturesDetails.ValidateEntity<PremiumFeatures>(policyRequest.Details.PolicyPremiumFeatures[i]);

                        if (validationResultPremiumFeaturesDetails.HasError)
                        {
                            errors = new Error[validationResultPremiumFeaturesDetails.ValidationErrors.Count];
                            for (int j = 0; j < validationResultPremiumFeaturesDetails.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultPremiumFeaturesDetails.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultPremiumFeaturesDetails.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
                if (policyRequest.Details.DynamicPremiumFeatures != null)
                {
                    for (int i = 0; i < (policyRequest.Details.DynamicPremiumFeatures.Length); i++)
                    {
                        var validationResultDynamicPremiumFeaturesDetails = DataAnnotationPremiumFeaturesDetails.ValidateEntity<PremiumFeatures>(policyRequest.Details.DynamicPremiumFeatures[i]);

                        if (validationResultDynamicPremiumFeaturesDetails.HasError)
                        {
                            errors = new Error[validationResultDynamicPremiumFeaturesDetails.ValidationErrors.Count];
                            for (int j = 0; j < validationResultDynamicPremiumFeaturesDetails.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultDynamicPremiumFeaturesDetails.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultDynamicPremiumFeaturesDetails.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
                if (policyRequest.Details.CustomizedParameter != null)
                {
                    for (int i = 0; i < (policyRequest.Details.CustomizedParameter.Length); i++)
                    {
                        var validationResultCustomizedParameterDetails = DataAnnotationCustomizedParameter.ValidateEntity<CustomizedParameter>(policyRequest.Details.CustomizedParameter[i]);

                        if (validationResultCustomizedParameterDetails.HasError)
                        {
                            errors = new Error[validationResultCustomizedParameterDetails.ValidationErrors.Count];
                            for (int j = 0; j < validationResultCustomizedParameterDetails.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultCustomizedParameterDetails.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultCustomizedParameterDetails.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
                Error.AddRange(ValidatePolicyDetails(policyRequest));
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, policyRequest.RequestReferenceNo, policyRequest.QuoteReferenceNo.ToString(), "ValidatePolicy");
                errors = new Error[1];
                errors[1].field = "";
                errors[1].message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                errors[1].code = "unexpected_error";
                Error.Add(errors[1]);
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyRequest), ex.ToString(), "ValidatePolicy", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
            }
            return Error;
        }

        public List<Error> ValidatePolicyDetails(PolicyRequest policyRequest)
        {

            Error error = new Error();
            List<Error> ErrorList = new List<Error>();

            try
            {
                //if (policyRequest.Details.PolicyPremiumFeatures == null)
                //{
                //    ErrorList.Add(ErrorDetails(invalidVal, "PolicyRequest PolicyPremiumFeatures", policyRequest.RequestReferenceNo, string.Empty));
                //}
                if (policyRequest.Details.PolicyPremiumFeatures != null)
                {
                    for (int i = 0; i < policyRequest.Details.PolicyPremiumFeatures.Length; i++)
                    {
                        if (policyRequest.Details.PolicyPremiumFeatures[i].FeatureTypeID == 1)
                        {
                            if (policyRequest.Details.PolicyPremiumFeatures[i].FeatureAmount == null)
                            {
                                ErrorList.Add(ErrorDetails(invalidVal, "PolicyPremiumFeatures[" + i + "].FeatureAmount", policyRequest.RequestReferenceNo, string.Empty));
                            }
                            if (policyRequest.Details.PolicyPremiumFeatures[i].FeatureTaxableAmount == null)
                            {
                                ErrorList.Add(ErrorDetails(invalidVal, "PolicyPremiumFeatures[" + i + "].FeatureTaxableAmount", policyRequest.RequestReferenceNo, string.Empty));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, policyRequest.RequestReferenceNo, null, "ValidatePolicyDetails");
                error.field = "";
                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                error.code = "unexpected_error";
                Error.Add(error);
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyRequest), ex.ToString(), "ValidatePolicyDetails", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
            }
            return ErrorList;
        }

        #endregion 'Policy Request Validation'

        #region 'PolicyIssue Request Validation'

        public List<Error> ValidatePolicyIssue(PolicyIssueRequest policyIssueRequest)
        {
            try
            {
                var validationPolicyIssueDetails = DataAnnotationPolicyIssue.ValidateEntity<PolicyIssueRequest>(policyIssueRequest);
                if (validationPolicyIssueDetails.HasError)
                {
                    errors = new Error[validationPolicyIssueDetails.ValidationErrors.Count];
                    for (int i = 0; i < validationPolicyIssueDetails.ValidationErrors.Count; i++)
                    {
                        errors[i] = new Error();
                        errors[i].field = validationPolicyIssueDetails.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
                        errors[i].message = validationPolicyIssueDetails.ValidationErrors[i].ToString();
                        errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                        Error.Add(errors[i]);
                    }

                }

                var validationPolicyIssueResultDetails = DataAnnotationPolicyIssueDetails.ValidateEntity<PolicyIssueDetails>(policyIssueRequest.Details);
                if (validationPolicyIssueResultDetails.HasError)
                {
                    errors = new Error[validationPolicyIssueResultDetails.ValidationErrors.Count];
                    for (int i = 0; i < validationPolicyIssueResultDetails.ValidationErrors.Count; i++)
                    {
                        errors[i] = new Error();
                        errors[i].field = validationPolicyIssueResultDetails.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
                        errors[i].message = validationPolicyIssueResultDetails.ValidationErrors[i].ErrorMessage.ToString();
                        errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                        Error.Add(errors[i]);
                    }
                }

                if (policyIssueRequest.Details.PolicyPremiumFeatures != null)
                {
                    for (int i = 0; i < (policyIssueRequest.Details.PolicyPremiumFeatures.Length); i++)
                    {
                        var validationResultPremiumFeaturesDetails = DataAnnotationPremiumFeaturesDetails.ValidateEntity<PremiumFeatures>(policyIssueRequest.Details.PolicyPremiumFeatures[i]);

                        if (validationResultPremiumFeaturesDetails.HasError)
                        {
                            errors = new Error[validationResultPremiumFeaturesDetails.ValidationErrors.Count];
                            for (int j = 0; j < validationResultPremiumFeaturesDetails.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultPremiumFeaturesDetails.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultPremiumFeaturesDetails.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
                if (policyIssueRequest.Details.DynamicPremiumFeatures != null)
                {
                    for (int i = 0; i < (policyIssueRequest.Details.DynamicPremiumFeatures.Length); i++)
                    {
                        var validationResultDynamicPremiumFeaturesDetails = DataAnnotationPremiumFeaturesDetails.ValidateEntity<PremiumFeatures>(policyIssueRequest.Details.DynamicPremiumFeatures[i]);

                        if (validationResultDynamicPremiumFeaturesDetails.HasError)
                        {
                            errors = new Error[validationResultDynamicPremiumFeaturesDetails.ValidationErrors.Count];
                            for (int j = 0; j < validationResultDynamicPremiumFeaturesDetails.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultDynamicPremiumFeaturesDetails.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultDynamicPremiumFeaturesDetails.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
                if (policyIssueRequest.Details.CustomizedParameter != null)
                {
                    for (int i = 0; i < (policyIssueRequest.Details.CustomizedParameter.Length); i++)
                    {
                        var validationResultCustomizedParameterDetails = DataAnnotationCustomizedParameter.ValidateEntity<CustomizedParameter>(policyIssueRequest.Details.CustomizedParameter[i]);

                        if (validationResultCustomizedParameterDetails.HasError)
                        {
                            errors = new Error[validationResultCustomizedParameterDetails.ValidationErrors.Count];
                            for (int j = 0; j < validationResultCustomizedParameterDetails.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultCustomizedParameterDetails.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultCustomizedParameterDetails.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
                Error.AddRange(ValidatePolicyIssueDetails(policyIssueRequest));
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, policyIssueRequest.RequestReferenceNo, policyIssueRequest.QuoteReferenceNo.ToString(), "ValidatePolicyIssue");
                errors = new Error[1];
                errors[1].field = "";
                errors[1].message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                errors[1].code = "unexpected_error";
                Error.Add(errors[1]);
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), ex.ToString(), "ValidatePolicyIssue", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
            }
            return Error;
        }

        public List<Error> ValidatePolicyIssueDetails(PolicyIssueRequest policyIssueRequest)
        {

            Error error = new Error();
            List<Error> ErrorList = new List<Error>();

            try
            {
                //if (policyIssueRequest.Details.PolicyPremiumFeatures == null)
                //{
                //    ErrorList.Add(ErrorDetails(invalidVal, "PolicyIssueRequest PolicyPremiumFeatures", policyIssueRequest.RequestReferenceNo, string.Empty));
                //}
                switch (policyIssueRequest.Details.VehicleUniqueTypeID)
                {
                    case 1:
                        if (policyIssueRequest.Details.VehicleSequenceNumber == null)
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "VehicleSequenceNumber", policyIssueRequest.RequestReferenceNo, string.Empty));
                        }
                        if (policyIssueRequest.Details.VehiclePlateTypeID == null)
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "VehiclePlateTypeID", policyIssueRequest.RequestReferenceNo, string.Empty));
                        }
                        if (policyIssueRequest.Details.VehiclePlateNumber == null)
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "VehiclePlateNumber", policyIssueRequest.RequestReferenceNo, string.Empty));
                        }
                        if (policyIssueRequest.Details.FirstPlateLetterID == null)
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "FirstPlateLetterID", policyIssueRequest.RequestReferenceNo, string.Empty));
                        }
                        if (policyIssueRequest.Details.SecondPlateLetterID == null)
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "SecondPlateLetterID", policyIssueRequest.RequestReferenceNo, string.Empty));
                        }
                        if (Convert.ToInt32(policyIssueRequest.Details.VehiclePlateTypeID) != 10 && policyIssueRequest.Details.ThirdPlateLetterID == null)
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "ThirdPlateLetterID", policyIssueRequest.RequestReferenceNo, string.Empty));
                        }
                        if (policyIssueRequest.Details.VehicleRegistrationExpiryDate == null)
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "VehicleRegistrationExpiryDate", policyIssueRequest.RequestReferenceNo, string.Empty));
                        }
                        break;
                    case 2:
                        if (policyIssueRequest.Details.VehicleCustomID == null)
                        {
                            ErrorList.Add(ErrorDetails(invalidVal, "VehicleCustomID", policyIssueRequest.RequestReferenceNo, string.Empty));
                        }
                        break;
                    default:
                        break;
                }
                if(policyIssueRequest.Details.IsVehicleOwnerTransfer==true)
                {
                    if(policyIssueRequest.Details.VehicleOwnerIdentityNo==null)
                    {
                        ErrorList.Add(ErrorDetails(invalidVal, "VehicleOwnerIdentityNo", policyIssueRequest.RequestReferenceNo, string.Empty));
                    }

                }
                if (policyIssueRequest.Details.PolicyPremiumFeatures != null)
                {
                    for (int i = 0; i < policyIssueRequest.Details.PolicyPremiumFeatures.Length; i++)
                    {
                        if (policyIssueRequest.Details.PolicyPremiumFeatures[i].FeatureTypeID == 1)
                        {
                            if (policyIssueRequest.Details.PolicyPremiumFeatures[i].FeatureAmount == null)
                            {
                                ErrorList.Add(ErrorDetails(invalidVal, "PolicyPremiumFeatures[" + i + "].FeatureAmount", policyIssueRequest.RequestReferenceNo, string.Empty));
                            }
                            if (policyIssueRequest.Details.PolicyPremiumFeatures[i].FeatureTaxableAmount == null)
                            {
                                ErrorList.Add(ErrorDetails(invalidVal, "PolicyPremiumFeatures[" + i + "].FeatureTaxableAmount", policyIssueRequest.RequestReferenceNo, string.Empty));
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, policyIssueRequest.RequestReferenceNo, null, "ValidatePolicyIssueDetails");
                error.field = "";
                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                error.code = "unexpected_error";
                Error.Add(error);
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", policyIssueRequest.RequestReferenceNo, JsonConvert.SerializeObject(policyIssueRequest), ex.ToString(), "ValidatePolicyIssueDetails", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
            }
            return ErrorList;
        }


        #endregion 'PolicyIssue Request Validation'

        public List<Error> ValidatePremDetails(decimal PolAmt, decimal PolTaxableAmt, bool IsPremCalculated, string reqstReferenceNo, int BodyType, bool IsQuotationStop)
        {
            Error error = new Error();

            try
            {
                if (BodyType == 0)
                {
                    Error.Add(ErrorDetails("Make_Model", "Make_Model", reqstReferenceNo, string.Empty));
                }
                else if (IsQuotationStop)
                {
                    Error.Add(ErrorDetails(stop_Quotation, "Premium", reqstReferenceNo, string.Empty));
                }
                else if (!IsPremCalculated)
                {
                    Error.Add(ErrorDetails(invalid_PremVal, "Premium", reqstReferenceNo, string.Empty));
                }
                else
                {
                    if (Math.Round(PolAmt) < 100)
                    {
                        error.field = "PolicyAmount";
                        error.message = "Policy Amount should be greater than or equal to 100";
                        error.code = invalidVal;
                        Error.Add(error);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, reqstReferenceNo, null, "ValidatePremDetails");
                error.field = "";
                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                error.code = "unexpected_error";
                Error.Add(error);
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", reqstReferenceNo, string.Empty, ex.ToString(), "ValidatePremDetails", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
            }
            return Error;
        }

        #region queryfeature
        public List<Error> Validation(QueryFeatureRequest queryFeature)
        {
            Error error = new Error();
            List<Error> ErrorList = new List<Error>();

            try
            {
                var validationResultQueryDetails = DataAnnotationQueryFeatureRequest.ValidateEntity<QueryFeatureRequest>(queryFeature);
                if (validationResultQueryDetails.HasError)
                {
                    errors = new Error[validationResultQueryDetails.ValidationErrors.Count];
                    for (int i = 0; i < validationResultQueryDetails.ValidationErrors.Count; i++)
                    {
                        errors[i] = new Error();
                        errors[i].field = validationResultQueryDetails.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
                        errors[i].message = validationResultQueryDetails.ValidationErrors[i].ToString();
                        errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                        Error.Add(errors[i]);
                    }
                }
                if (queryFeature.CustomizedParameter != null)
                {
                    for (int i = 0; i < (queryFeature.CustomizedParameter.Length); i++)
                    {
                        var validationResultCustomizedParameter = DataAnnotationCustomizedParameter.ValidateEntity<CustomizedParameter>(queryFeature.CustomizedParameter[i]);

                        if (validationResultCustomizedParameter.HasError)
                        {
                            errors = new Error[validationResultCustomizedParameter.ValidationErrors.Count];
                            for (int j = 0; j < validationResultCustomizedParameter.ValidationErrors.Count; j++)
                            {
                                errors[j] = new Error();
                                errors[j].field = validationResultCustomizedParameter.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
                                errors[j].message = validationResultCustomizedParameter.ValidationErrors[j].ErrorMessage.ToString();
                                errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
                                Error.Add(errors[j]);
                            }
                        }
                    }
                }
               
            }
            catch (Exception)
            {

                throw;
            }
            return ErrorList;
        }
        #endregion
        //public List<Error> ValidatePurchaseFeature(PurchaseFeatureRequest purchaseFeatureRequest)
        //{
        //    try
        //    {
        //        var validationPurchaseFeatureDetails = DataAnnotationPurchaseFeature.ValidateEntity<PurchaseFeatureRequest>(purchaseFeatureRequest);
        //        if (validationPurchaseFeatureDetails.HasError)
        //        {
        //            errors = new Error[validationPurchaseFeatureDetails.ValidationErrors.Count];
        //            for (int i = 0; i < validationPurchaseFeatureDetails.ValidationErrors.Count; i++)
        //            {
        //                errors[i] = new Error();
        //                errors[i].field = validationPurchaseFeatureDetails.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
        //                errors[i].message = validationPurchaseFeatureDetails.ValidationErrors[i].ToString();
        //                errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
        //                Error.Add(errors[i]);
        //            }
        //        }
        //        if (purchaseFeatureRequest.AddedFeatures == null)
        //        {
        //            Error.Add(ErrorDetails(invalidVal, "AddedFeatures", purchaseFeatureRequest.QueryRequestReferenceNo.ToString(), string.Empty));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, purchaseFeatureRequest.QueryRequestReferenceNo.ToString(), string.Empty, "ValidatePurchaseFeature");
        //        errors = new Error[1];
        //        errors[1].field = "";
        //        errors[1].message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        errors[1].code = "unexpected_error";
        //        Error.Add(errors[1]);
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", purchaseFeatureRequest.QueryRequestReferenceNo.ToString(), JsonConvert.SerializeObject(purchaseFeatureRequest), ex.ToString(), "ValidatePurchaseFeature", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
        //    }
        //    return Error;

        //}
        //public List<Error> ValidateAddDriver(AddDriverRequest addDriverRequest)
        //{
        //    try
        //    {
        //        var validationAddDriver = DataAnnotationAddDriver.ValidateEntity<AddDriverRequest>(addDriverRequest);
        //        if (validationAddDriver.HasError)
        //        {
        //            errors = new Error[validationAddDriver.ValidationErrors.Count];
        //            for (int i = 0; i < validationAddDriver.ValidationErrors.Count; i++)
        //            {
        //                errors[i] = new Error();
        //                errors[i].field = validationAddDriver.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
        //                errors[i].message = validationAddDriver.ValidationErrors[i].ToString();
        //                errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
        //                Error.Add(errors[i]);
        //            }

        //        }

        //        if (addDriverRequest.CustomizedParameter != null)
        //        {
        //            for (int i = 0; i < (addDriverRequest.CustomizedParameter.Length); i++)
        //            {
        //                var validationAddDriverCustomizedParameter = DataAnnotationCustomizedParameter.ValidateEntity<CustomizedParameter>(addDriverRequest.CustomizedParameter[i]);

        //                if (validationAddDriverCustomizedParameter.HasError)
        //                {
        //                    errors = new Error[validationAddDriverCustomizedParameter.ValidationErrors.Count];
        //                    for (int j = 0; j < validationAddDriverCustomizedParameter.ValidationErrors.Count; j++)
        //                    {
        //                        errors[j] = new Error();
        //                        errors[j].field = validationAddDriverCustomizedParameter.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
        //                        errors[j].message = validationAddDriverCustomizedParameter.ValidationErrors[j].ErrorMessage.ToString();
        //                        errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
        //                        Error.Add(errors[j]);
        //                    }
        //                }
        //            }
        //        }

        //        Error.AddRange(SpclValidateAddDriver(addDriverRequest));
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, addDriverRequest.AddDriverRequestReferenceNo.ToString(), addDriverRequest.PolicyNumber.ToString(), "ValidateAddDriver");
        //        errors = new Error[1];
        //        errors[1].field = "";
        //        errors[1].message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        errors[1].code = "unexpected_error";
        //        Error.Add(errors[1]);
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", addDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(addDriverRequest), ex.ToString(), "ValidateAddDriver", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
        //    }
        //    return Error;
        //}
        //public List<Error> ValidateConfirmAddDriver(ConfirmAddDriverRequest confirmAddDriverRequest)
        //{
        //    try
        //    {
        //        var validationConfirmAddDriver = DataAnnotationConfirmAddDriver.ValidateEntity<ConfirmAddDriverRequest>(confirmAddDriverRequest);
        //        if (validationConfirmAddDriver.HasError)
        //        {
        //            errors = new Error[validationConfirmAddDriver.ValidationErrors.Count];
        //            for (int i = 0; i < validationConfirmAddDriver.ValidationErrors.Count; i++)
        //            {
        //                errors[i] = new Error();
        //                errors[i].field = validationConfirmAddDriver.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
        //                errors[i].message = validationConfirmAddDriver.ValidationErrors[i].ToString();
        //                errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
        //                Error.Add(errors[i]);
        //            }

        //        }
        //        if (confirmAddDriverRequest.CustomizedParameter != null)
        //        {
        //            for (int i = 0; i < confirmAddDriverRequest.CustomizedParameter.Length; i++)
        //            {
        //                var validationAddDriverCustomizedParameter = DataAnnotationCustomizedParameter.ValidateEntity<CustomizedParameter>(confirmAddDriverRequest.CustomizedParameter[i]);

        //                if (validationAddDriverCustomizedParameter.HasError)
        //                {
        //                    errors = new Error[validationAddDriverCustomizedParameter.ValidationErrors.Count];
        //                    for (int j = 0; j < validationAddDriverCustomizedParameter.ValidationErrors.Count; j++)
        //                    {
        //                        errors[j] = new Error();
        //                        errors[j].field = validationAddDriverCustomizedParameter.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
        //                        errors[j].message = validationAddDriverCustomizedParameter.ValidationErrors[j].ErrorMessage.ToString();
        //                        errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
        //                        Error.Add(errors[j]);
        //                    }
        //                }
        //            }
        //        }
        //        Error.AddRange(SpclValidateConfirmAddDriver(confirmAddDriverRequest.DriversBreakdown, confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString()));
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), confirmAddDriverRequest.AddDriverResponseReferenceNo.ToString(), "ValidateConfirmAddDriver");
        //        errors = new Error[1];
        //        errors[1].field = "";
        //        errors[1].message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        errors[1].code = "unexpected_error";
        //        Error.Add(errors[1]);
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", confirmAddDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(confirmAddDriverRequest), ex.ToString(), "ValidateConfirmAddDriver", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
        //    }
        //    return Error;
        //}

        //public List<Error> SpclValidateAddDriver(AddDriverRequest addDriverRequest)
        //{
        //    Error error = new Error();
        //    List<Error> ErrorList = new List<Error>();

        //    try
        //    {

        //        for (int i = 0; i < addDriverRequest.DriverDetails.Length; i++)
        //        {
        //            if (string.IsNullOrEmpty(addDriverRequest.DriverDetails[i].EnglishFirstName) && string.IsNullOrEmpty(addDriverRequest.DriverDetails[i].ArabicFirstName))
        //            {
        //                ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].ArabicFirstName", addDriverRequest.AddDriverRequestReferenceNo.ToString(), string.Empty));
        //                ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].EnglishFirstName", addDriverRequest.AddDriverRequestReferenceNo.ToString(), string.Empty));
        //            }
        //            if (string.IsNullOrEmpty(addDriverRequest.DriverDetails[i].EnglishMiddleName) && string.IsNullOrEmpty(addDriverRequest.DriverDetails[i].ArabicMiddleName))
        //            {
        //                ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].ArabicMiddleName", addDriverRequest.AddDriverRequestReferenceNo.ToString(), string.Empty));
        //                ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].EnglishMiddleName", addDriverRequest.AddDriverRequestReferenceNo.ToString(), string.Empty));
        //            }
        //            if (string.IsNullOrEmpty(addDriverRequest.DriverDetails[i].EnglishLastName) && string.IsNullOrEmpty(addDriverRequest.DriverDetails[i].ArabicLastName))
        //            {
        //                ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].ArabicLastName", addDriverRequest.AddDriverRequestReferenceNo.ToString(), string.Empty));
        //                ErrorList.Add(ErrorDetails(invalidVal, "DriverDetails[" + i + "].EnglishLastName", addDriverRequest.AddDriverRequestReferenceNo.ToString(), string.Empty));
        //            }
        //            if (addDriverRequest.DriverDetails[i].DriverID.ToString().StartsWith("2"))
        //            {
        //                if (string.IsNullOrEmpty(addDriverRequest.DriverDetails[i].DriverDateOfBirthG))
        //                {
        //                    ErrorList.Add(ErrorDetails(invalidVal, "DriverDateOfBirthG", addDriverRequest.AddDriverRequestReferenceNo.ToString(), string.Empty));
        //                }
        //                if (addDriverRequest.DriverDetails[i].DriverNationalityID == null)
        //                {
        //                    ErrorList.Add(ErrorDetails(invalidVal, "DriverNationalityID", addDriverRequest.AddDriverRequestReferenceNo.ToString(), string.Empty));
        //                }
        //            }
        //            if (addDriverRequest.DriverDetails[i].DriverID.ToString().StartsWith("1") && string.IsNullOrEmpty(addDriverRequest.DriverDetails[i].DriverDateOfBirthH))
        //            {
        //                ErrorList.Add(ErrorDetails(invalidVal, "DriverDateOfBirthH", addDriverRequest.AddDriverRequestReferenceNo.ToString(), string.Empty));
        //            }
        //        }


        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrHandler.WriteError(Ex, addDriverRequest.AddDriverRequestReferenceNo.ToString(), null, "SpclValidateAddDriver");
        //        error.field = "";
        //        error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        error.code = "unexpected_error";
        //        Error.Add(error);
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", addDriverRequest.AddDriverRequestReferenceNo.ToString(), JsonConvert.SerializeObject(addDriverRequest), Ex.ToString(), "SpclValidateAddDriver", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
        //    }
        //    return ErrorList;
        //}

        //public List<Error> SpclValidateConfirmAddDriver(DriversBreakdown[] DriversBreakdown, string AddDriverRequestReferenceNo)
        //{
        //    Error error = new Error();
        //    List<Error> ErrorList = new List<Error>();

        //    try
        //    {
        //        for (int i = 0; i < DriversBreakdown.Length; i++)
        //        {
        //            if (DriversBreakdown[i].DriverID.ToString().StartsWith("2"))
        //            {
        //                if (string.IsNullOrEmpty(DriversBreakdown[i].DriverDateOfBirthG))
        //                {
        //                    ErrorList.Add(ErrorDetails(invalidVal, "DriverDateOfBirthG", AddDriverRequestReferenceNo, string.Empty));
        //                }
        //            }
        //            if (DriversBreakdown[i].DriverID.ToString().StartsWith("1") && string.IsNullOrEmpty(DriversBreakdown[i].DriverDateOfBirthH))
        //            {
        //                ErrorList.Add(ErrorDetails(invalidVal, "DriverDateOfBirthH", AddDriverRequestReferenceNo, string.Empty));
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ErrHandler.WriteError(Ex, AddDriverRequestReferenceNo, null, "SpclValidateConfirmAddDriver");
        //        error.field = "";
        //        error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        error.code = "unexpected_error";
        //        Error.Add(error);
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", AddDriverRequestReferenceNo, string.Empty, Ex.ToString(), "SpclValidateConfirmAddDriver", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
        //    }
        //    return ErrorList;
        //}
        //#region IcUpdatePolicy
        //public List<Error> ValidateIcUpdatePolicy(IcUpdatePolicyRequest icUpdatePolicyRequest)
        //{
        //    try
        //    {
        //        var validationIcUpdatePolicy = DataAnnotationUpdatePolicy.ValidateEntity<IcUpdatePolicyRequest>(icUpdatePolicyRequest);
        //        if (validationIcUpdatePolicy.HasError)
        //        {
        //            errors = new Error[validationIcUpdatePolicy.ValidationErrors.Count];
        //            for (int i = 0; i < validationIcUpdatePolicy.ValidationErrors.Count; i++)
        //            {
        //                errors[i] = new Error();
        //                errors[i].field = validationIcUpdatePolicy.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
        //                errors[i].message = validationIcUpdatePolicy.ValidationErrors[i].ToString();
        //                errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
        //                Error.Add(errors[i]);
        //            }

        //        }

        //        if (icUpdatePolicyRequest.NationalAddress != null)
        //        {
        //            for (int i = 0; i < (icUpdatePolicyRequest.NationalAddress.Length); i++)
        //            {
        //                var validationUpdatePolicyNationalAddress = DataAnnotationNationalAddressDetails.ValidateEntity<NationalAddress>(icUpdatePolicyRequest.NationalAddress[i]);

        //                if (validationUpdatePolicyNationalAddress.HasError)
        //                {
        //                    errors = new Error[validationUpdatePolicyNationalAddress.ValidationErrors.Count];
        //                    for (int j = 0; j < validationUpdatePolicyNationalAddress.ValidationErrors.Count; j++)
        //                    {
        //                        errors[j] = new Error();
        //                        errors[j].field = validationUpdatePolicyNationalAddress.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
        //                        errors[j].message = validationUpdatePolicyNationalAddress.ValidationErrors[j].ErrorMessage.ToString();
        //                        errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
        //                        Error.Add(errors[j]);
        //                    }
        //                }
        //            }
        //        }
        //        Error.AddRange(ValidateIcUpdatePolicyDetails(icUpdatePolicyRequest));
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, icUpdatePolicyRequest.RequestReferenceNo, icUpdatePolicyRequest.QuoteReferenceNo.ToString(), "ValidateIcUpdatePolicy");
        //        errors = new Error[1];
        //        errors[1].field = "";
        //        errors[1].message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        errors[1].code = "unexpected_error";
        //        Error.Add(errors[1]);
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), ex.ToString(), "ValidateIcUpdatePolicy", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
        //    }
        //    return Error;

        //}
        //public List<Error> ValidateIcUpdatePolicyDetails(IcUpdatePolicyRequest icUpdatePolicyRequest)
        //{

        //    Error error = new Error();
        //    List<Error> ErrorList = new List<Error>();

        //    try
        //    {
        //        if (icUpdatePolicyRequest.UpdatePolicyReasonID == 2 && icUpdatePolicyRequest.VehicleCustomID == null && icUpdatePolicyRequest.VehicleSequenceNumber == null)
        //        {
        //            ErrorList.Add(ErrorDetails(invalidVal, "VehicleCustomID", icUpdatePolicyRequest.RequestReferenceNo, string.Empty));
        //            ErrorList.Add(ErrorDetails(invalidVal, "VehicleSequenceNumber", icUpdatePolicyRequest.RequestReferenceNo, string.Empty));
        //        }
        //        if (icUpdatePolicyRequest.UpdatePolicyReasonID == 2 || icUpdatePolicyRequest.UpdatePolicyReasonID == 3)
        //        {
        //            if (icUpdatePolicyRequest.VehiclePlateTypeID == null && icUpdatePolicyRequest.VehicleSequenceNumber == null)
        //            {
        //                ErrorList.Add(ErrorDetails(invalidVal, "VehiclePlateTypeID", icUpdatePolicyRequest.RequestReferenceNo, string.Empty));
        //            }
        //            if (icUpdatePolicyRequest.VehiclePlateNumber == null)
        //            {
        //                ErrorList.Add(ErrorDetails(invalidVal, "VehiclePlateNumber", icUpdatePolicyRequest.RequestReferenceNo, string.Empty));
        //            }
        //            if (icUpdatePolicyRequest.FirstPlateLetterID == null)
        //            {
        //                ErrorList.Add(ErrorDetails(invalidVal, "FirstPlateLetterID", icUpdatePolicyRequest.RequestReferenceNo, string.Empty));
        //            }
        //            if (icUpdatePolicyRequest.SecondPlateLetterID == null)
        //            {
        //                ErrorList.Add(ErrorDetails(invalidVal, "SecondPlateLetterID", icUpdatePolicyRequest.RequestReferenceNo, string.Empty));
        //            }
        //            if (icUpdatePolicyRequest.VehiclePlateTypeID != 10 && icUpdatePolicyRequest.ThirdPlateLetterID == null)
        //            {
        //                ErrorList.Add(ErrorDetails(invalidVal, "ThirdPlateLetterID", icUpdatePolicyRequest.RequestReferenceNo, string.Empty));
        //            }

        //        }
        //        if (icUpdatePolicyRequest.UpdatePolicyReasonID == 1 && icUpdatePolicyRequest.Email == null && icUpdatePolicyRequest.MobileNo == null)
        //        {
        //            ErrorList.Add(ErrorDetails(invalidVal, "Email", icUpdatePolicyRequest.RequestReferenceNo, string.Empty));
        //            ErrorList.Add(ErrorDetails(invalidVal, "MobileNo", icUpdatePolicyRequest.RequestReferenceNo, string.Empty));
        //        }
        //        if (icUpdatePolicyRequest.UpdatePolicyReasonID == 4 && icUpdatePolicyRequest.NationalAddress == null)
        //        {
        //            ErrorList.Add(ErrorDetails(invalidVal, "NationalAddress", icUpdatePolicyRequest.RequestReferenceNo, string.Empty));
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, icUpdatePolicyRequest.RequestReferenceNo, null, "ValidateIcUpdatePolicyDetails");
        //        error.field = "";
        //        error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        error.code = "unexpected_error";
        //        Error.Add(error);
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icUpdatePolicyRequest.RequestReferenceNo, JsonConvert.SerializeObject(icUpdatePolicyRequest), ex.ToString(), "ValidateIcUpdatePolicyDetails", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
        //    }
        //    return ErrorList;
        //}
        //#endregion
        //public List<Error> ValidateCancelPolicy(IcCancelPolicyRequest icCancelPolicyRequest)
        //{
        //    try
        //    {
        //        var validationIcCancelPolicy = DataAnnotationCancelPolicy.ValidateEntity<IcCancelPolicyRequest>(icCancelPolicyRequest);
        //        if (validationIcCancelPolicy.HasError)
        //        {
        //            errors = new Error[validationIcCancelPolicy.ValidationErrors.Count];
        //            for (int i = 0; i < validationIcCancelPolicy.ValidationErrors.Count; i++)
        //            {
        //                errors[i] = new Error();
        //                errors[i].field = validationIcCancelPolicy.ValidationErrors[i].MemberNames.ElementAt(0).ToString();
        //                errors[i].message = validationIcCancelPolicy.ValidationErrors[i].ToString();
        //                errors[i].code = errors[i].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
        //                Error.Add(errors[i]);
        //            }

        //        }

        //        if (icCancelPolicyRequest.CustomizedParameter != null)
        //        {
        //            for (int i = 0; i < (icCancelPolicyRequest.CustomizedParameter.Length); i++)
        //            {
        //                var validationCancelPolicyCustomizedParameter = DataAnnotationCustomizedParameter.ValidateEntity<CustomizedParameter>(icCancelPolicyRequest.CustomizedParameter[i]);

        //                if (validationCancelPolicyCustomizedParameter.HasError)
        //                {
        //                    errors = new Error[validationCancelPolicyCustomizedParameter.ValidationErrors.Count];
        //                    for (int j = 0; j < validationCancelPolicyCustomizedParameter.ValidationErrors.Count; j++)
        //                    {
        //                        errors[j] = new Error();
        //                        errors[j].field = validationCancelPolicyCustomizedParameter.ValidationErrors[j].MemberNames.ElementAt(0).ToString();
        //                        errors[j].message = validationCancelPolicyCustomizedParameter.ValidationErrors[j].ErrorMessage.ToString();
        //                        errors[j].code = errors[j].message.Contains("Invalid") ? invalidVal : Mandotaryfield;
        //                        Error.Add(errors[j]);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrHandler.WriteError(ex, icCancelPolicyRequest.RequestReferenceNo, icCancelPolicyRequest.QuoteReferenceNo.ToString(), "ValidateCancelPolicy");
        //        errors = new Error[1];
        //        errors[1].field = "";
        //        errors[1].message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
        //        errors[1].code = "unexpected_error";
        //        Error.Add(errors[1]);
        //        SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", icCancelPolicyRequest.RequestReferenceNo.ToString(), JsonConvert.SerializeObject(icCancelPolicyRequest), ex.ToString(), "ValidateCancelPolicy", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
        //    }
        //    return Error;

        //}

        public Error ErrorDetails(string errCode, string errField, string reqtRefNo, string quoteRefNo)
        {
            Error error = new Error();
            try
            {
                error.field = errField;
                error.code = errCode;
                switch (errCode)
                {
                    case "field_missing":
                        error.message = "A Mandatory field is not found in the request.";
                        break;
                    case "invalid_input":
                        error.message = "A value specified in the field is not valid value or not in proper format.";
                        break;
                    case "duplicate_error":
                        error.message = "A field is expecting unique value between requests which founds duplicates.";
                        break;
                    case "invalid_field":
                        error.message = "An unknown or additional field(s) specified in the request which was not expected.";
                        break;
                    case "invalid_json":
                        error.message = "JSON request is not well formed.";
                        break;
                    case "unexpected_error":
                        error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                        break;
                    case "authentication_failed":
                        error.message = "Authentication with the service failed. Invalid user name or password.";
                        break;
                    case "membership_expired":
                        error.message = "Authentication with the service failed. Invalid user name or password.";
                        break;
                    case "transformation_notfound":
                        error.message = "Authentication with the service failed. Invalid user name or password.";
                        break;
                    case "invalid_premium":
                        error.message = "Premium not calculated for the parameters provided.";
                        break;
                    case "invalid_VehUsageId":
                        error.message = "No premium for vehicle usage.";
                        break;
                    case "Manual_MakeModel":
                        error.message = "Manual entry of make & model.";
                        break;
                    case "Corporate_Cust":
                        error.message = "Corporate policy owner.";
                        break;
                    case "Make_Model":
                        error.message = "Make Model mapping not found.";
                        break;
                    case "invalid_PlateType":
                        error.message = "No premium for vehicle plate type.";
                        break;
                    case "invalidPremium":
                        error.message = "Premium not calculated for the parameters provided(Trigger).";
                        break;
                    case "PremiumNotCalculated":
                        error.message = "premium not calculated for the parameters provided(Manual Entered).";
                        break;
                    case "invalidSumInsured":
                        error.message = "Premium not calculated for the parameters provided(Sum Insured).";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex, reqtRefNo, quoteRefNo, "ErrorDetails");
                error.field = "";
                error.message = "An Unexpected error occurred from server. Contact Tameeni and share request details to investigate further.";
                error.code = "unexpected_error";
                SaveErrorResp = dataAccessLayer.SaveErrorDetails("1", reqtRefNo, string.Empty, ex.ToString(), "ErrorDetails", "CommonValidation", "TameeniLeasing", "TameeniLeasing");
            }
            return error;
        }
    }
}