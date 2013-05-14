using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPoco.T4.Tests.Common.SqlSchema
{
	public class Column
	{
		public string Name;
		public string PropertyName;
		public string PropertyType;
		public int StringLength;
		public bool IsPK;
		public bool IsNullable;
		public bool IsAutoIncrement;
		public bool Ignore;
	}

}
