using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NPoco.T4.Tests.Common
{
	public static class Random<T> where T : class, new()
	{
		private static readonly Random _random = new Random(DateTime.Now.Millisecond);

		public static T Create()
		{
			return Create(null);
		}

		public static T Create(List<string> protectedPropertyNames)
		{
			T result = new T();
			return Update(result, protectedPropertyNames);
		}

		public static T Update(T obj)
		{
			return Update(obj, null);
		}

		public static T UpdateSpecifiedProperties(T obj, List<string> propertiesToUpdate)
		{
			var protectedNames = (typeof(T)).GetProperties().Select(x => x.Name).Except(propertiesToUpdate).ToList();
			return Update(obj, protectedNames);
		}


		public static T Update(T obj, List<string> protectedPropertyNames)
		{
			Type type = typeof(T);
			int stringLength;
			string s;

			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				PropertyInfo info = propertyInfo;
				bool isFound = (protectedPropertyNames == null) ? false : protectedPropertyNames.Any(name => name.Equals(info.Name, StringComparison.InvariantCultureIgnoreCase));

				// Looking for StringLength attribute
				stringLength = 50;
				object[] attributes = info.GetCustomAttributes(typeof(StringLengthAttribute), true);
				if ((attributes != null) && (attributes.Length > 0))
				{
					stringLength = (attributes[0] as StringLengthAttribute).MaximumLength;
				}

				if (propertyInfo.CanWrite && !isFound)
				{
					if (propertyInfo.PropertyType == typeof(DateTime))
					{	propertyInfo.SetValue(obj, DateTime.Now, null);	}

					else if (propertyInfo.PropertyType == typeof(DateTime?))
					{ propertyInfo.SetValue(obj, ((_random.Next(0, 10) < 7) ? (DateTime?)DateTime.Now : null), null); }


					else if (propertyInfo.PropertyType == typeof(int))
					{ propertyInfo.SetValue(obj, _random.Next(), null); }

					else if (propertyInfo.PropertyType == typeof(int?))
					{ propertyInfo.SetValue(obj, ((_random.Next(0, 10) < 7) ? (int?)_random.Next() : null), null); }


					else if (propertyInfo.PropertyType == typeof(bool))
					{ propertyInfo.SetValue(obj, _random.Next(0, 2) == 0 ? false : true, null); }

					else if (propertyInfo.PropertyType == typeof(bool?))
					{ propertyInfo.SetValue(obj, ((_random.Next(0, 10) < 7) ? (bool?)(_random.Next(0, 2) == 0 ? false : true) : null), null); }


					else if (propertyInfo.PropertyType == typeof(double))
					{ propertyInfo.SetValue(obj, _random.NextDouble(), null); }

					else if (propertyInfo.PropertyType == typeof(double?))
					{ propertyInfo.SetValue(obj, ((_random.Next(0, 10) < 7) ? (double?)_random.NextDouble() : null), null); }

					else if (propertyInfo.PropertyType == typeof(float))
					{ propertyInfo.SetValue(obj, (float)_random.Next() / 10000, null); }

					else if (propertyInfo.PropertyType == typeof(float?))
					{ propertyInfo.SetValue(obj, ((_random.Next(0, 10) < 7) ? (float?)((float)_random.Next() / 10000) : null), null); }


					else if (propertyInfo.PropertyType == typeof(byte))
					{ propertyInfo.SetValue(obj, byte.Parse(_random.Next(byte.MinValue, byte.MaxValue).ToString()), null); }

					else if (propertyInfo.PropertyType == typeof(byte?))
					{ propertyInfo.SetValue(obj, ((_random.Next(0, 10) < 7) ? (byte?)byte.Parse(_random.Next(byte.MinValue, byte.MaxValue).ToString()) : null), null); }


					else if (propertyInfo.PropertyType == typeof(Guid))
					{ propertyInfo.SetValue(obj, Guid.NewGuid(), null); }

					else if (propertyInfo.PropertyType == typeof(Guid?))
					{ propertyInfo.SetValue(obj, ((_random.Next(0, 10) < 7) ? (Guid?)Guid.NewGuid() : null), null); }


					else if (propertyInfo.PropertyType == typeof(string))
					{
						s = Guid.NewGuid().ToString();
						if (s.Length > stringLength)
						{
							s = s.Substring(s.Length - stringLength, stringLength);
						}
						propertyInfo.SetValue(obj, s, null);
					}
				}
			}

			return obj;
		}
	}

}
