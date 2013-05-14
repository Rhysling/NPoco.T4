using System;
using System.Configuration;

namespace MyApp.Services
{
	public static class AppSettings
	{
		public static string ConnectionStringName
		{
			get
			{
				return "ConStr_Primary";
			}
		}

		private static string _connectionString = "";
		public static string ConnectionString
		{
			get
			{
				if (_connectionString == "")
				{
					_connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
				}
				return _connectionString;
			}
		}

	}
}