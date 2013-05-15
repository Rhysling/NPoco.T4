using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Models.Core
{
	public interface ISelfValidating
	{
		bool IsValid { get; }
		string ErrorMessage { get; }
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class SelfValidationAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var obj = (ISelfValidating)value;

			if (!obj.IsValid) return new ValidationResult(obj.ErrorMessage);
			return ValidationResult.Success;
		}
	}
}
