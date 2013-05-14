using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPoco.T4.Tests.Common
{
	public static class Comparison
	{
		public static bool PublicInstancePropertiesEqual<T>(T self, T to) where T : class
		{
			return PublicInstancePropertiesEqual(self, to, new List<string>());
		}

		public static bool PublicInstancePropertiesEqual<T>(T self, T to, List<string> ignoreList) where T : class
		{
			if (self != null && to != null)
			{
				Type type = typeof(T);
				//List<string> ignoreList = new List<string>(ignore);
				foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
				{
					if (!ignoreList.Contains(pi.Name))
					{
						object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
						object toValue = type.GetProperty(pi.Name).GetValue(to, null);

						if (!((selfValue == null) && (toValue == null)))
						{
							if (selfValue.GetType() == typeof(DateTime))
							{
								var dv1 = DateTime.MinValue;
								var dv2 = DateTime.MinValue;
								if (!DateTime.TryParse(selfValue.ToString(), out dv1) || !DateTime.TryParse(toValue.ToString(), out dv2))
								{
									return false;
								}
								if (Math.Abs(dv1.Subtract(dv2).Milliseconds) > 5000)  //Same if within 5 seconds
								{
									return false;
								}
							}
							else if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
							{
								return false;
							}
						}
					}
				}
				return true;
			}
			return self == to;
		}

	}
}
