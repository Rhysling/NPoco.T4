using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MyApp.Models.Core
{
	// Just an example of a custom model type for binding.
	// This is a safe, non-null date with some magic year values.

	public struct SampleCustomType : ISelfValidating
	{
		// Note:
		// Year <= 1910 -> Empty
		// Year == 1915 -> Invalid
		// Year == 1920 -> Pending
		// Year == 1925 -> Not Required

		// Private
		private DateTime dt;
		private string inputString;

		// Constructors

		public SampleCustomType(DateTime inp)
		{
			dt = inp;
			inputString = "";
		}

		public SampleCustomType(DateTime? inp)
		{
			if (inp == null)
			{
				dt = new DateTime(1900, 1, 15);
			}
			else
			{
				dt = (DateTime)inp;
			}
			inputString = "";
		}

		public SampleCustomType(string inp)
		{
			dt = new DateTime(1900, 1, 15);
			inputString = "";
			ProcessInputString(inp);
		}


		// ToString()

		public override string ToString()
		{
			if (dt.Year <= 1910) return "Empty";
			if (dt.Year == 1915) return "Invalid";
			if (dt.Year == 1920) return "Pending";
			if (dt.Year == 1925) return "Not Required";
			return dt.ToShortDateString();
		}


		// Properties

		public DateTime Value
		{
			get
			{
				if (dt.Year <= 1910) return new DateTime(1900, 1, 15);
				if (dt.Year == 1915) return new DateTime(1915, 1, 15);
				if (dt.Year == 1920) return new DateTime(1920, 1, 15);
				if (dt.Year == 1925) return new DateTime(1925, 1, 15);
				return dt;
			}
			set
			{
				dt = value;
			}
		}

		public string StringValue
		{
			get
			{
				return this.ToString();
			}
			set
			{
				ProcessInputString(value);
			}
		}


		public bool IsEmpty
		{
			get
			{
				return (dt.Year <= 1910);
			}
			set
			{
				dt = new DateTime(1900, 1, 15);
			}
		}

		public bool IsPending
		{
			get
			{
				return (dt.Year == 1920);
			}
			set
			{
				dt = new DateTime(1920, 1, 15);
			}
		}

		public bool NotRequired
		{
			get
			{
				return (dt.Year == 1925);
			}
			set
			{
				dt = new DateTime(1925, 1, 15);
			}
		}

		public bool IsValid
		{
			get
			{
				return (dt.Year != 1915);
			}
		}

		public string ErrorMessage
		{
			get
			{
				return "'" + inputString + "' is not a valid date.";
			}
		}

		public DateDbState CurrentState
		{
			get
			{
				if (dt.Year <= 1910) return DateDbState.Empty;
				if (dt.Year == 1915) return DateDbState.Invalid;
				if (dt.Year == 1920) return DateDbState.Pending;
				if (dt.Year == 1925) return DateDbState.NotRequired;
				return DateDbState.ValidDate;
			}
		}


		// Private methods

		private void ProcessInputString(string inp)
		{
			inputString = inp;

			if (String.IsNullOrWhiteSpace(inp))
			{
				dt = new DateTime(1900, 1, 15);
				return;
			}

			inp = inp.ToLower().Trim().Replace(" ", "");
			if ((inp == "") || (inp == "empty") || (inp == "null"))
			{
				dt = new DateTime(1900, 1, 15);
			}
			else if (inp == "pending")
			{
				dt = new DateTime(1920, 1, 15);
			}
			else if (inp == "notrequired")
			{
				dt = new DateTime(1925, 1, 15);
			}
			else
			{
				if (!DateTime.TryParse(inp, out dt))
				{
					dt = new DateTime(1915, 1, 15);
				}
			}

		}
	}


	public enum DateDbState
	{
		ValidDate = 0,
		NotRequired = 1,
		Pending = 2,
		Invalid = 3,
		Empty = 4
	}

	

}
