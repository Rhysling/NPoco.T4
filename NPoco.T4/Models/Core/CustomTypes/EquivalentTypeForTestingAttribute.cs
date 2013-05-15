using System;

namespace MyApp.Models.Core
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class EquivalentTypeForTestingAttribute : System.Attribute
	{
		public EquivalentTypeForTestingAttribute(string name) {
			this.Name = name;
		}

		public string Name { get; set; }
	}
}
