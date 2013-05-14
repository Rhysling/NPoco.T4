using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NPoco.T4.Tests.Common.SqlSchema
{
	public class NPocoSchema
	{

		public NPocoSchema(string connectionString)
		{
			_connectionString = connectionString;
		}

		string _connectionString = "";
		string _classPrefix = "";
		string _classSuffix = "";
		string _schemaName = null;		


		static string ConstructTableSelectSql()
		{
			/*
			string Table_Sql = @"SELECT *
				FROM  INFORMATION_SCHEMA.TABLES
				WHERE (TABLE_NAME <> 'sysdiagrams') {0}
				ORDER BY TABLE_TYPE, TABLE_NAME";
	
			string w ="";
			if ((TableNames != null) && TableNames.Length > 0)
			{
				w = "AND (";
				foreach (string s in TableNames)
				{
					w += "(TABLE_NAME = '" + s +"') OR ";
				}
				w = w.Substring(0, w.Length - 4) + ")";
			}
			return string.Format(Table_Sql, w);
			*/

			return @"SELECT *
				FROM  INFORMATION_SCHEMA.TABLES
				WHERE (TABLE_NAME <> 'sysdiagrams')
				ORDER BY TABLE_TYPE, TABLE_NAME";
		}


		static Regex rxCleanUp = new Regex(@"[^\w\d_]", RegexOptions.Compiled);

		static Func<string, string> CleanUp = (str) =>
		{
			str = rxCleanUp.Replace(str, "_");
			if (char.IsDigit(str[0])) str = "_" + str;
	
			return str;
		};

		string CheckNullable(Column col)
		{
			string result="";
			if(col.IsNullable && 
				col.PropertyType !="byte[]" && 
				col.PropertyType !="string" &&
				col.PropertyType !="Microsoft.SqlServer.Types.SqlGeography" &&
				col.PropertyType !="Microsoft.SqlServer.Types.SqlGeometry"
				)
				result="?";
			return result;
		}



		public Tables GetTables(string classPrefix, string classSuffix, string schemaName)
		{
			_classPrefix = classPrefix;
			_classSuffix = classSuffix;
			_schemaName = schemaName;

			var _factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
		
			Tables result;
			using (var conn=_factory.CreateConnection())
			{
				conn.ConnectionString = _connectionString;         
				conn.Open();
		
				var reader= new SqlServerSchemaReader();
		
				result = reader.ReadSchema(conn, _factory);
		
				// Remove unrequired tables/views
				for (int i=result.Count-1; i>=0; i--)
				{
					if (_schemaName != null && string.Compare(result[i].Schema, _schemaName, true) != 0)
					{
						result.RemoveAt(i);
						continue;
					}
					//if (!IncludeViews && result[i].IsView)
					//{
					//	result.RemoveAt(i);
					//	continue;
					//}
				}

				conn.Close();

		
				var rxClean = new Regex("^(Equals|GetHashCode|GetType|ToString|repo|Save|IsNew|Insert|Update|Delete|Exists|SingleOrDefault|Single|First|FirstOrDefault|Fetch|Page|Query)$");
				foreach (var t in result)
				{
					t.ClassName = _classPrefix + t.ClassName + _classSuffix;
					foreach (var c in t.Columns)
					{
						c.PropertyName = rxClean.Replace(c.PropertyName, "_$1");

						// Make sure property name doesn't clash with class name
						if (c.PropertyName == t.ClassName)
							c.PropertyName = "_" + c.PropertyName;
					}
				}

				return result;
			}
		}
	

		
		class SqlServerSchemaReader
		{
			DbConnection _connection;
			DbProviderFactory _factory;

			// SchemaReader.ReadSchema
			public Tables ReadSchema(DbConnection connection, DbProviderFactory factory)
			{
				var result = new Tables();
		
				_connection=connection;
				_factory=factory;

				var cmd = _factory.CreateCommand();
				cmd.Connection = connection;
				cmd.CommandText = ConstructTableSelectSql();

				//pull the tables in a reader
				using(cmd)
				{
					using (var rdr = cmd.ExecuteReader())
					{
						while(rdr.Read())
						{
							Table tbl=new Table();
							tbl.Name=rdr["TABLE_NAME"].ToString();
							tbl.Schema=rdr["TABLE_SCHEMA"].ToString();
							//tbl.IsView=string.Compare(rdr["TABLE_TYPE"].ToString(), "View", true)==0;
							tbl.CleanName=CleanUp(tbl.Name);
							tbl.ClassName=Inflector.MakeSingular(tbl.CleanName);
							if (tbl.Name.EndsWith("Enum"))
							{
								tbl.EnumName = tbl.ClassName;
								tbl.IsEnum = true;
								tbl.ClassName += "Poco";
							}
							else
							{
								tbl.EnumName = null;
								tbl.IsEnum = false;
							}
							tbl.RepoName = tbl.ClassName + "Db";
							result.Add(tbl);
						}
					}
				}
		
				foreach (var tbl in result)
				{
					tbl.Columns = LoadColumns(tbl);

					// Mark the primary key(s)
					string[] pkNames = GetPK(tbl.Name);
					if (pkNames != null)
					{
						var pkCols = tbl.Columns.Where(a => pkNames.Contains(a.Name));
						foreach (var c in pkCols)
						{
							c.IsPK = true;
						}
						tbl.IsAutoIncrement = pkCols.Any(a => a.IsAutoIncrement);
					}
				}

				return result;
			}
	

			List<Column> LoadColumns(Table tbl)
			{
				using (var cmd = _factory.CreateCommand())
				{
					cmd.Connection = _connection;
					cmd.CommandText = COLUMN_SQL;

					var p = cmd.CreateParameter();
					p.ParameterName = "@tableName";
					p.Value = tbl.Name;
					cmd.Parameters.Add(p);

					p = cmd.CreateParameter();
					p.ParameterName = "@schemaName";
					p.Value = tbl.Schema;
					cmd.Parameters.Add(p);

					var result=new List<Column>();
					using (IDataReader rdr=cmd.ExecuteReader())
					{
						while(rdr.Read())
						{
							Column col=new Column();
							col.Name=rdr["ColumnName"].ToString();
							col.PropertyName = CleanUp(col.Name);
							col.IsNullable = rdr["IsNullable"].ToString() == "YES";
							col.PropertyType = GetPropertyType(rdr["DataType"].ToString(), col.IsNullable);
							int.TryParse(rdr["MaxLength"].ToString(), out col.StringLength);
							col.IsAutoIncrement = ((int)rdr["IsIdentity"]) == 1;
							result.Add(col);
						}
					}

					return result;
				}
			}

			string[] GetPK(string table)
			{

				string sql = @"SELECT c.name AS ColumnName
				FROM sys.indexes AS i 
				INNER JOIN sys.index_columns AS ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id 
				INNER JOIN sys.objects AS o ON i.object_id = o.object_id 
				LEFT OUTER JOIN sys.columns AS c ON ic.object_id = c.object_id AND c.column_id = ic.column_id
				WHERE (i.type = 1) AND (o.name = @tableName)";

				using (var cmd = _factory.CreateCommand())
				{
					cmd.Connection = _connection;
					cmd.CommandText = sql;

					var p = cmd.CreateParameter();
					p.ParameterName = "@tableName";
					p.Value = table;
					cmd.Parameters.Add(p);

					var pkColNames = new List<string>();
					using (IDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							pkColNames.Add(rdr["ColumnName"].ToString());
						}
					}

					if (!pkColNames.Any()) return null;

					return pkColNames.ToArray();
				}
			}
	
			string GetPropertyType(string sqlType, bool isNullable)
			{
				string sysType="string";
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
						sysType=  "DateTime";
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

				if (isNullable && (sysType != "object") && (sysType != "string"))
				{
					sysType += "?";
				}
				return sysType;
			}


			// *** Moved Table_Sql definition to top for insertion of custom select SQL ****
			//	const string TABLE_SQL = @"SELECT *
			//		FROM  INFORMATION_SCHEMA.TABLES
			//		WHERE TABLE_TYPE='BASE TABLE'";
			// *** 
	
			const string COLUMN_SQL = @"SELECT 
					TABLE_CATALOG AS [Database],
					TABLE_SCHEMA AS Owner, 
					TABLE_NAME AS TableName, 
					COLUMN_NAME AS ColumnName, 
					ORDINAL_POSITION AS OrdinalPosition, 
					COLUMN_DEFAULT AS DefaultSetting, 
					IS_NULLABLE AS IsNullable, DATA_TYPE AS DataType, 
					CHARACTER_MAXIMUM_LENGTH AS MaxLength, 
					DATETIME_PRECISION AS DatePrecision,
					COLUMNPROPERTY(object_id('[' + TABLE_SCHEMA + '].[' + TABLE_NAME + ']'), COLUMN_NAME, 'IsIdentity') AS IsIdentity,
					COLUMNPROPERTY(object_id('[' + TABLE_SCHEMA + '].[' + TABLE_NAME + ']'), COLUMN_NAME, 'IsComputed') as IsComputed
				FROM  INFORMATION_SCHEMA.COLUMNS
				WHERE TABLE_NAME=@tableName AND TABLE_SCHEMA=@schemaName
				ORDER BY OrdinalPosition ASC";
		}

	}
}
