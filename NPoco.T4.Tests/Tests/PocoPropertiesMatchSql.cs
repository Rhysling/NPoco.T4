using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NPoco.T4.Tests.Common;
using NPoco.T4.Tests.Common.SqlSchema;

using MyApp.Models;
using MyApp.Models.Core;
using MyApp.Repositories;
using MyApp.Repositories.Core;

namespace NPoco.T4.Tests.Tests
{

	[TestClass]
	public class PocoPropertiesMatchSql
	{
		private static Tables currentSqlTables;
		private static Tables currentPocos;


		[ClassInitialize()]
		public static void Initialize(TestContext testContext)
		{
			BuildCurrentSqlTables();
			BuildCurrentPocos();
		}


#region " Tests "

		[TestMethod]
		public void Compare_Poco_Properties_With_SQL_Schema()
		{
			// Loop through the POCOs and compare with SQL values

			foreach (var tblPoco in currentPocos)
			{
				var tblSql = currentSqlTables.SingleOrDefault(x => String.Compare(tblPoco.CleanName, x.CleanName) == 0);
				if (tblSql == null)
				{
					Assert.Fail("{0} poco missing from DB.", tblPoco.CleanName);
				}
				else
				{
					// Compare table level properties
					if (tblPoco.ClassName != tblSql.ClassName)
					{
						Assert.Fail(tblPoco.CleanName + ": ClassName-> Poco: " + tblPoco.ClassName + " / Sql: " + tblSql.ClassName);
					}
					if ((tblPoco.PrimaryKeyNames != tblSql.PrimaryKeyNames) && !String.IsNullOrEmpty(tblPoco.PrimaryKeyNames))
					{
						Assert.Fail(tblPoco.CleanName + ": PkName-> Poco: " + tblPoco.PrimaryKeyNames + " / Sql: " + tblSql.PrimaryKeyNames);
					}
					if (tblPoco.IsAutoIncrement != tblSql.IsAutoIncrement)
					{
						Assert.Fail(tblPoco.CleanName + ": IsAutoIncrement-> Poco: " + tblPoco.IsAutoIncrement + " / Sql: " + tblSql.IsAutoIncrement);
					}

					// Compare column level properties
					foreach (var colPoco in tblPoco.Columns)
					{
						var colSql = tblSql.Columns.Single(x => colPoco.PropertyName == x.PropertyName);
						if (colSql == null)
						{
							Assert.Fail(tblPoco.ClassName + "." + colPoco.PropertyName + " missing from DB");
						}
						else
						{
							// Compare column level properties
							string fqn = tblPoco.ClassName + "." + colPoco.PropertyName;

							if (colPoco.PropertyType != colSql.PropertyType)
							{
								Assert.Fail(fqn + ": PropertyType-> Poco: " + colPoco.PropertyType + " / Sql: " + colSql.PropertyType);
							}
							if (colPoco.StringLength != colSql.StringLength && !String.IsNullOrEmpty(tblPoco.PrimaryKeyNames))
							{
								Assert.Fail(fqn + ": StringLength-> Poco: " + colPoco.StringLength + " / Sql: " + colSql.StringLength);
							}
							if (colPoco.IsPK != colSql.IsPK)
							{
								Assert.Fail(fqn + ": IsPK-> Poco: " + colPoco.IsPK + " / Sql: " + colSql.IsPK);
							}
							if (colPoco.IsAutoIncrement != colSql.IsAutoIncrement)
							{
								Assert.Fail(fqn + ": IsAutoIncrement-> Poco: " + colPoco.IsAutoIncrement + " / Sql: " + colSql.IsAutoIncrement);
							}
							if (colPoco.IsNullable != colSql.IsNullable && colPoco.PropertyType != "string")
							{
								Assert.Fail(fqn + ": IsNullable-> Poco: " + colPoco.IsNullable + " / Sql: " + colSql.IsNullable);
							}

						}
					}

				}

			}

		}

		#endregion


		#region " Private Functions "

		public static void BuildCurrentSqlTables()
		{
			string connectionString = ConfigurationManager.ConnectionStrings["ConStr_Primary"].ConnectionString;

			var pps = new NPocoSchema(connectionString);
			currentSqlTables = pps.GetTables("", "", null);
		}


		public static void BuildCurrentPocos()
		{
			var tbls = new Tables();

			var iobj = new MyApp.Models.IdentityObject();
			var asm = Assembly.GetAssembly(iobj.GetType());

			Type[] types = asm.GetTypes();

			foreach (Type t in types)
			{
				var tbl = new Table();
				tbl.Columns = new List<Column>();
				Attribute[] attrs = Attribute.GetCustomAttributes(t);

				foreach (Attribute atr in attrs)
				{
					if (atr is NPoco.PrimaryKeyAttribute)
					{
						var pka = (NPoco.PrimaryKeyAttribute)atr;
						tbl.PrimaryKeyNamesFromAttribute =pka.Value;
						tbl.IsAutoIncrement = tbl.IsAutoIncrement || pka.AutoIncrement;
					}

					if (atr is NPoco.TableNameAttribute)
					{
						var a = (NPoco.TableNameAttribute)atr;
						tbl.ClassName = t.Name;
						tbl.CleanName = a.Value;

						var props = t.GetProperties();

						foreach (var p in props)
						{
							var col = new Column();

							Attribute[] propAttributes = Attribute.GetCustomAttributes(p);
							foreach (Attribute patr in propAttributes)
							{
								if (patr is StringLengthAttribute)
								{
									col.StringLength = ((StringLengthAttribute)patr).MaximumLength;
								}

								if (patr is NPoco.ColumnAttribute)
								{
									var ca = patr as NPoco.ColumnAttribute;
									col.Name = String.IsNullOrEmpty(ca.Name) ? p.Name : ca.Name;
									col.PropertyName = p.Name;
									col.PropertyType = TypeMap.CommonSystemTypeFromFrameworkType(p.PropertyType.ToString());
									col.IsNullable = col.PropertyType.EndsWith("?");
									col.IsAutoIncrement = col.IsPK && tbl.IsAutoIncrement;
								}

								if (patr is EquivalentTypeForTestingAttribute)
								{
									var ca = patr as EquivalentTypeForTestingAttribute;
									col.PropertyType = ca.Name;
									col.IsNullable = col.PropertyType.EndsWith("?");
								}
							}

							if (!String.IsNullOrEmpty(col.PropertyName))
							{
								tbl.Columns.Add(col);
							}
						}
					}
				}

				if (!String.IsNullOrEmpty(tbl.ClassName))
				{
					// Assign Primary Key(s) if found
					if (!String.IsNullOrEmpty(tbl.PrimaryKeyNamesFromAttribute))
					{
						string[] pkNames = tbl.PrimaryKeyNamesFromAttribute.Split(',').Select(a => a.Trim()).ToArray();
						var cols = tbl.Columns.Where(a => pkNames.Contains(a.PropertyName));
						foreach (var col in cols)
						{
							col.IsPK = true;
							col.IsAutoIncrement = tbl.IsAutoIncrement;
						}
					}

					tbls.Add(tbl);
				}

			}

			currentPocos = tbls;
		}

		#endregion
	}
}
