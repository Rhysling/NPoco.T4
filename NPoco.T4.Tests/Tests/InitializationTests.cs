using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPoco.T4.Tests.Common.TestDatabase;
using MyApp.Models;

namespace NPoco.T4.Tests.Tests
{
	[TestClass]
	public class InitializationTests
	{
		private static FileStream logFile;

		[AssemblyInitialize()]
		public static void Initialize(TestContext testContext)
		{
			TestDatabase.Initialize();

			string logFilePath = Environment.ExpandEnvironmentVariables("%userprofile%") + @"\Documents\Visual Studio 2012\Projects\NPoco.T4\NPoco.T4.Tests\TraceResult.log";
			logFile = new FileStream(logFilePath, FileMode.OpenOrCreate, FileAccess.Write);
			TextWriterTraceListener myListener = new TextWriterTraceListener(logFile);
			Trace.Listeners.Add(myListener);
			Trace.WriteLine("Test Run Started -- " + DateTime.Now.ToString("F"));
		}

		[AssemblyCleanup()]
		public static void Cleanup()
		{
			Trace.WriteLine("");
			Trace.WriteLine("Test Run Ended -- " + DateTime.Now.ToString("F"));
			Trace.Flush();
			logFile.Flush();
			logFile.Close();
			logFile.Dispose();
		}


		[TestMethod]
		public void TestDataIntegity()
		{
			string resKCO = TestData.VerifyRecordCountMatchForPocoType(typeof(CompositeKeyObject), TestDatabase.Db);
			Assert.IsTrue(resKCO.Length == 0, resKCO);

			string resIO = TestData.VerifyRecordCountMatchForPocoType(typeof(IdentityObject), TestDatabase.Db);
			Assert.IsTrue(resIO.Length == 0, resIO);

			string resKGO = TestData.VerifyRecordCountMatchForPocoType(typeof(KeyedGuidObject), TestDatabase.Db);
			Assert.IsTrue(resKGO.Length == 0, resKGO);

			string resKIO = TestData.VerifyRecordCountMatchForPocoType(typeof(KeyedIntObject), TestDatabase.Db);
			Assert.IsTrue(resKIO.Length == 0, resKIO);

			string resLO = TestData.VerifyRecordCountMatchForPocoType(typeof(ListObject), TestDatabase.Db);
			Assert.IsTrue(resLO.Length == 0, resLO);

			string resNKNDO = TestData.VerifyRecordCountMatchForPocoType(typeof(NoKeyNonDistinctObject), TestDatabase.Db);
			Assert.IsTrue(resNKNDO.Length == 0, resNKNDO);

		}
	}
}
