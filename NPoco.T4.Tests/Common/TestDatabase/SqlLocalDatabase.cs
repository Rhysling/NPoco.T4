using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace NPoco.T4.Tests.Common.TestDatabase
{
	public class SqlLocalDatabase : BaseDatabase
	{
		protected string DBName = "NPoco_Tests";

		public SqlLocalDatabase(string connectionString)
		{
				ConnectionString = connectionString;
				ProviderName = "System.Data.SqlClient";

				RecreateDataBase();
				EnsureSharedConnectionConfigured();

				//Console.WriteLine("Tables (Constructor): " + Environment.NewLine);
				//var dt = ((SqlConnection)Connection).GetSchema("Tables");
				//foreach (DataRow row in dt.Rows)
				//{
				//		Console.WriteLine((string)row[2]);
				//}
		}

		public override void EnsureSharedConnectionConfigured()
		{
			if (Connection != null) return;

			lock (_syncRoot)
			{
				Connection = new SqlConnection(ConnectionString);
				Connection.Open();
			}
		}

		public override void RecreateDataBase()
		{
			Console.WriteLine("----------------------------");
			Console.WriteLine("Using SQL Server Local DB   ");
			Console.WriteLine("----------------------------");

			base.RecreateDataBase();

			/* 
				* Using new connection so that when a transaction is bound to Connection if it rolls back 
				* it doesn't blow away the tables
				*/
			var conn = new SqlConnection("Data Source=localhost;Initial Catalog=master;Integrated Security=True;");
			conn.Open();
			var cmd = conn.CreateCommand();

			// Try to detach the DB in case the clean up code wasn't called (usually aborted debugging)
			cmd.CommandText = String.Format(@"
					IF (EXISTS(SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = '{0}' OR name = '{0}')))
					BEGIN
							ALTER DATABASE {0} SET single_user WITH rollback immediate
							DROP DATABASE {0}
					END
			", DBName);
			cmd.ExecuteNonQuery();

			// Create the new DB
			cmd.CommandText = String.Format("CREATE DATABASE {0}", DBName);
			cmd.ExecuteNonQuery();

			cmd.Connection.ChangeDatabase(DBName);

			// Create the Schema
			cmd.CommandText = @"
					CREATE TABLE [dbo].[IdentityObjects](
						[Id] [int] IDENTITY(1,1) NOT NULL,
						[Name] [nvarchar](200) NULL,
						[Age] [int] NULL,
						[DateOfBirth] [date] NULL,
						[Savings] [decimal](12, 3) NULL,
						[DependentCount] [tinyint] NULL,
						Gender char(1) null,
						IsActive bit not null
					PRIMARY KEY CLUSTERED 
					(
						[Id] ASC
					) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
					) ON [PRIMARY];
			";
			cmd.ExecuteNonQuery();

			cmd.CommandText = @"
					CREATE TABLE [dbo].[KeyedIntObjects](
						[Id] [int] NOT NULL,
						[Name] [nvarchar](200) NULL,
						[Age] [int] NULL,
						[DateOfBirth] [date] NULL,
						[Savings] [decimal](12, 3) NULL,
						[DependentCount] [tinyint] NULL,
						Gender char(1) null,
						IsActive bit not null
					PRIMARY KEY CLUSTERED 
					(
						[Id] ASC
					) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
					) ON [PRIMARY]
			";
			cmd.ExecuteNonQuery();

			cmd.CommandText = @"
					CREATE TABLE [dbo].[KeyedGuidObjects](
						[Id] [uniqueidentifier] NOT NULL,
						[Name] [nvarchar](200) NULL,
						[Age] [int] NULL,
						[DateOfBirth] [date] NULL,
						[Savings] [decimal](12, 3) NULL,
						[DependentCount] [tinyint] NULL,
						Gender char(1) null,
						IsActive bit not null
					PRIMARY KEY CLUSTERED 
					(
						[Id] ASC
					)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
					) ON [PRIMARY]
			";
			cmd.ExecuteNonQuery();

			cmd.CommandText = @"
					CREATE TABLE [dbo].[CompositeKeyObjects](
						[Key1ID] [int] NOT NULL,
						[Key2ID] [int] NOT NULL,
						[Key3ID] [int] NOT NULL,
						[TextData] [nvarchar](512) NULL,
						[DateEntered] [datetime] NOT NULL,
						[DateUpdated] [datetime] NULL,
						CONSTRAINT [PK__Composit__8885DE3707020F21] PRIMARY KEY CLUSTERED 
					(
						[Key1ID] ASC,
						[Key2ID] ASC,
						[Key3ID] ASC
					)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
					) ON [PRIMARY];
			";
			cmd.ExecuteNonQuery();

			cmd.CommandText = @"
					CREATE TABLE [dbo].[NoKeyNonDistinctObjects](
						[FullName] [varchar](50) NOT NULL,
						[ItemInt] [int] NOT NULL,
						[OptionalInt] [int] NULL,
						[Color] [varchar](50) NULL
					) ON [PRIMARY];
			";
			cmd.ExecuteNonQuery();
			
			cmd.CommandText = @"
			CREATE TABLE [dbo].[ListObjects](
				[Id] [int] NOT NULL,
				[ShortName] [varchar](50) NOT NULL,
				[Description] [varchar](255) NOT NULL,
				[StatusKey] [char](1) NOT NULL,
				[SortOrder] [int] NOT NULL,
			 CONSTRAINT [PK_ListObjects] PRIMARY KEY CLUSTERED 
			(
				[Id] ASC
			)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
			) ON [PRIMARY]
			";
			cmd.ExecuteNonQuery();

			cmd.Dispose();
			conn.Close();
			conn.Dispose();
		}

		public override void CleanupDataBase()
		{
			/* 
				* Trying to do any clean up here fails until the Database object gets disposed.
				* The create deletes and recreates the files so this isn't really necessary
			*/
		}
	}
}
