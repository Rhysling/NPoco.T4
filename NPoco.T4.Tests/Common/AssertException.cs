using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NPoco.T4.Tests.Common
{
	public static class AssertException
	{
		public static T Throws<T>(Action action) where T : Exception
		{
			string actual = "Did Not Fail";
			try
			{
				action();
			}
			catch (T ex)
			{
				return ex;
			}
			catch (Exception e)
			{
				actual = e.ToString();
			}

			Assert.Fail("Expected exception of type {0}. Actual: {1}", typeof(T), actual);

			return null;
		}
	}
}