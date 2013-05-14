using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPoco;

namespace NPoco.T4.Tests.Common.TestDatabase
{
	public static class TestDatabase
	{
		private static BaseDatabase _dbDirect;
		private static  NPoco.Database _db ;

		public static NPoco.Database Db
		{
			get
			{
				return _db;
			}
		}


		public static void Initialize()
		{
			var testDBType = Convert.ToInt32(ConfigurationManager.AppSettings["TestDBType"]);
			switch (testDBType)
			{
				case 1: // SQLite In-Memory
					throw new NotSupportedException("SQLite not supported DB type.");
					//return;

				case 2: // SQL Local DB
					_dbDirect = new SqlLocalDatabase(ConfigurationManager.ConnectionStrings["ConStr_Primary"].ConnectionString);
					_db = new Database(_dbDirect.Connection, new NPoco.DatabaseTypes.SqlServer2008DatabaseType(), IsolationLevel.ReadUncommitted); // Need read uncommitted for the transaction tests
					break;

				case 3: // SQL Server
				case 4: // SQL CE
				case 5: // MySQL
				case 6: // Oracle
				case 7: // Postgres

				default:
					throw new NotSupportedException("Unknown database platform specified.");
					//return;
			}

			// Insert test data
			TestData.RecreateData(_db);
		}


		public static void Cleanup()
		{
			if (_dbDirect == null) return;

			_dbDirect.CleanupDataBase();
			_dbDirect.Dispose();
		}

	}
}
