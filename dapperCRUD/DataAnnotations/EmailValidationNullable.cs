using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace dapperCRUD.DataAnnotations
{

    public class EmailRegex
    {
        public const string email =
            @"^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\ \\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))";

        public const string emailError = "Invalid Email!";
    }
    public class EmailValidationNullable : ValidationAttribute
    {
        private readonly string _isRequired;
        //private readonly int _allowedType;

        //public FileTypeValidation(AttachmentType allowedType)
        public EmailValidationNullable()
        {
            _isRequired = "";
        }

        public EmailValidationNullable(string isRequired)
        {
            _isRequired = isRequired;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //if (value == null) return ValidationResult.Success;

            //if (_isRequired.Length == 0)
            //{
            //    return ValidationResult.Success;
            //}

            //string valueString = value.ToString();
            //if (!Regex.IsMatch(valueString, EmailRegex.email))
            //{
            //    return new ValidationResult($"The email must be in a valid format");
            //}

            ////new ValidationResult($"The {validationContext.DisplayName} field must be a {_allowedType} file.");

            //return ValidationResult.Success;


            //////////

            //string valueString = value.ToString();

            //If User have not wrote Required in Parameter
            if (_isRequired != "Required")
            {
                //Allows an empty string 
                if (value == null) return ValidationResult.Success;
                if (value.ToString() == "") return ValidationResult.Success;

                ////string valueString = value.ToString();
                //if (!Regex.IsMatch(valueString, EmailRegex.email))
                //{
                //    return new ValidationResult($"The email must be in a valid format");
                //}
            }

            //if (value.ToString() == "") return ValidationResult.Success;
            if((value == null) || (value.ToString() == ""))
            {
                return new ValidationResult($"Please provide email");
            }
            string valueString = value.ToString();

            //If User passed Required then format must be valid

            if (!Regex.IsMatch(valueString, EmailRegex.email))
            {
                return new ValidationResult($"The email must be in a valid format");
                
            }

            ////If Required is mispelled then still validation does not applied
            //if (_isRequired != "Required")
            //{
            //    return ValidationResult.Success;
            //}

            return ValidationResult.Success;
        }

    }
}
