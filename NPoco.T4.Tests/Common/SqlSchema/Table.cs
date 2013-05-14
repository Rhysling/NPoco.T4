using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPoco.T4.Tests.Common.SqlSchema
{
	public class Table
	{
		public List<Column> Columns;
		public string Name;
		public string Schema;
		public string PrimaryKeyNamesFromAttribute;
		public string CleanName;
		public string ClassName;
		public string EnumName;
		public string RepoName;
		public bool IsEnum;
		public string SequenceName;
		public bool IsAutoIncrement = false;
		public bool Ignore;

		public string PrimaryKeyNames
		{
			get
			{
				return String.Join(", ", this.Columns.Where(x => x.IsPK).Select(a => a.Name));
			}
		}

		public Column[] PK
		{
			get
			{
				return this.Columns.Where(x => x.IsPK).ToArray();
			}
		}

		public Column GetColumn(string columnName)
		{
			return Columns.Single(x => string.Compare(x.Name, columnName, true) == 0);
		}

		public Column this[string columnName]
		{
			get
			{
				return GetColumn(columnName);
			}
		}

	}


	public class Tables : List<Table>
	{
		public Tables()
		{
		}

		public Table GetTable(string tableName)
		{
			return this.Single(x => string.Compare(x.Name, tableName, true) == 0);
		}

		public Table this[string tableName]
		{
			get
			{
				return GetTable(tableName);
			}
		}

	}
}
