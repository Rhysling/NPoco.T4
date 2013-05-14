using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPoco.T4.Tests.Common
{
	public static class TypeMap
	{
		public static string CommonSystemTypeFromSqlType(string sqlType)
		{
			string sysType = "string";
			switch (sqlType)
			{
				case "bigint":
					sysType = "long";
					break;
				case "smallint":
					sysType = "short";
					break;
				case "int":
					sysType = "int";
					break;
				case "uniqueidentifier":
					sysType = "Guid";
					break;
				case "smalldatetime":
				case "datetime":
				case "date":
				case "time":
					sysType = "DateTime";
					break;
				case "float":
					sysType = "double";
					break;
				case "real":
					sysType = "float";
					break;
				case "numeric":
				case "smallmoney":
				case "decimal":
				case "money":
					sysType = "decimal";
					break;
				case "tinyint":
					sysType = "byte";
					break;
				case "bit":
					sysType = "bool";
					break;
				case "image":
				case "binary":
				case "varbinary":
				case "timestamp":
					sysType = "byte[]";
					break;
				case "geography":
					sysType = "Microsoft.SqlServer.Types.SqlGeography";
					break;
				case "geometry":
					sysType = "Microsoft.SqlServer.Types.SqlGeometry";
					break;
			}
			return sysType;
		}

		public static string CommonSystemTypeFromFrameworkType(string frameworkType)
		{	
			string sysType = "unknown";

			// frameworkType	"System.Nullable`1[System.Double]"
			// frameworkType = "System.DateTime"
			bool isNullable = frameworkType.StartsWith("System.Nullable");

			if (isNullable)
			{
				int st = frameworkType.IndexOf("[") + 1;
				int en = frameworkType.IndexOf("]");
				frameworkType = frameworkType.Substring(st, en - st);
			}

			switch (frameworkType)
			{
				case "System.Boolean": sysType = "bool"; break;
				case "System.Byte": sysType = "byte"; break;
				case "System.SByte": sysType = "sbyte"; break;
				case "System.Char": sysType = "char"; break;
				case "System.DateTime": sysType = "DateTime"; break;
				case "System.Decimal": sysType = "decimal"; break;
				case "System.Double": sysType = "double"; break;
				case "System.Single": sysType = "float"; break;
				case "System.Int32": sysType = "int"; break;
				case "System.UInt32": sysType = "uint"; break;
				case "System.Int64": sysType = "long"; break;
				case "System.UInt64": sysType = "ulong"; break;
				case "System.Object": sysType = "object"; break;
				case "System.Int16": sysType = "short"; break;
				case "System.UInt16": sysType = "ushort"; break;
				case "System.String": sysType = "string"; break;
				case "System.Guid": sysType = "Guid"; break;
			}

			if (isNullable && (sysType != "object") && (sysType != "string"))
			{
				sysType += "?";
			}
			return sysType;
		}
		
	}
}
