using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NPoco.T4.Tests.Common;
using NPoco.T4.Tests.Common.TestDatabase;

using MyApp.Models;
using MyApp.Repositories;
using MyApp.Repositories.Core;

namespace NPoco.T4.Tests.Tests
{
	[TestClass]
	public class CompositeKeyObjectTests
	{
		[TestMethod]
		public void CompositeKeyObject_Insert()
		{
			const int dataKey1ID = 102;
			const int dataKey2ID = 200;
			const int dataKey3ID = 300;
			const string dataTextData = "This is some text data.";
			var dataDateEntered = DateTime.Now;

			var poco = new CompositeKeyObject
			{
				Key1ID = dataKey1ID,
				Key2ID = dataKey2ID,
				Key3ID = dataKey3ID,
				TextData = dataTextData,
				DateEntered = dataDateEntered
			};

			var db = TestDatabase.Db;
			db.Insert(poco);

			var verify = db.SingleOrDefault<CompositeKeyObject>(@"
				SELECT * 
				FROM CompositeKeyObjects
				WHERE Key1ID = @0 AND Key2ID = @1 AND Key3ID = @2
				", dataKey1ID, dataKey2ID, dataKey3ID);

			db.Execute(@"
				DELETE FROM CompositeKeyObjects
				WHERE Key1ID = @0 AND Key2ID = @1 AND Key3ID = @2
				", dataKey1ID, dataKey2ID, dataKey3ID);

			Assert.IsNotNull(verify);
			Assert.AreEqual(dataKey1ID, verify.Key1ID);
			Assert.AreEqual(dataKey2ID, verify.Key2ID);
			Assert.AreEqual(dataKey3ID, verify.Key3ID);
			Assert.AreEqual(dataTextData, verify.TextData);
			int tDiff = dataDateEntered.Subtract(verify.DateEntered).Milliseconds;
			//Trace.WriteLine("TDiff = " + tDiff);
			Assert.IsTrue(Math.Abs(tDiff) < 100, "DateEntered difference (ms): "+ tDiff);
		}
	}
}
