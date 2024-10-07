using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static AJT_Leasing_API.Models.ConfirmAddDriverResponse;


namespace AJT_Leasing_API.Models
{
    public class EntityValidationResult
    {
        public IList<ValidationResult> ValidationErrors { get; private set; }
        public bool HasError
        {
            get { return ValidationErrors.Count > 0; }
        }

        public EntityValidationResult(IList<ValidationResult> violations = null)
        {
            ValidationErrors = violations ?? new List<ValidationResult>();
        }
    }

    public class DataAnnotationQuoteDetails
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : QuoteDetails
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

    public class DataAnnotationDetails
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : Details
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

    public class DataAnnotationNationalAddressDetails
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : NationalAddress
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

    public class DataAnnotationCustomizedParameter
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : CustomizedParameter
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

    public class DataAnnotationCountriesValidDrivingLicense
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : CountriesValidDrivingLicense
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

    public class DataAnnotationNajmCaseDetails
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : NajmCaseDetails
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

    public class DataAnnotationNationalDriverDetails
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : DriverDetails
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

    public class DataAnnotationPolicy
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : PolicyRequest
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

    public class DataAnnotationPolicyDetails
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : PolicyDetails
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

    public class DataAnnotationPremiumFeaturesDetails
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : PremiumFeatures
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

    public class DataAnnotationPolicyIssue
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : PolicyIssueRequest
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

    public class DataAnnotationPolicyIssueDetails
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : PolicyIssueDetails
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }
    public class DataAnnotationQueryFeatureRequest
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : QueryFeatureRequest
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }
    public class DataAnnotationPurchaseFeature
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : PurchaseFeatureRequest
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }
    public class DataAnnotationAddDriver
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : AddDriverRequest
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }
    public class DataAnnotationConfirmAddDriver
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : ConfirmAddDriverRequest
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }
    public class DataAnnotationUpdatePolicy
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : IcUpdatePolicyRequest
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }
    public class DataAnnotationCancelPolicy
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : IcCancelPolicyRequest
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
    }

}