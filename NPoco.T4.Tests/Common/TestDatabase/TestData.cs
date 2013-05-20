using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NPoco;
using MyApp.Models;

namespace NPoco.T4.Tests.Common.TestDatabase
{
	public static class TestData
	{
		public static List<CompositeKeyObject> InMemoryCompositeKeyObjects { get; set; }
		public static List<IdentityObject> InMemoryIdentityObjects { get; set; }
		public static List<KeyedGuidObject> InMemoryKeyedGuidObjects { get; set; }
		public static List<KeyedIntObject> InMemoryKeyedIntObjects { get; set; }
		public static List<ListObject> InMemoryListObjects { get; set; }
		public static List<NoKeyNonDistinctObject> InMemoryNoKeyNonDistinctObjects { get; set; }
		public static List<ObjectsWithCustomType> InMemoryObjectsWithCustomType { get; set; }

		public static void RecreateData(NPoco.Database db)
		{
			InMemoryCompositeKeyObjects = new List<CompositeKeyObject>();
			InMemoryIdentityObjects = new List<IdentityObject>();
			InMemoryKeyedGuidObjects = new List<KeyedGuidObject>();
			InMemoryKeyedIntObjects = new List<KeyedIntObject>();
			InMemoryListObjects = new List<ListObject>();
			InMemoryNoKeyNonDistinctObjects = new List<NoKeyNonDistinctObject>();
			InMemoryObjectsWithCustomType = new List<ObjectsWithCustomType>();

			// Clear out any old items
			db.Execute("TRUNCATE TABLE CompositeKeyObjects;");
			db.Execute("TRUNCATE TABLE IdentityObjects;");
			db.Execute("TRUNCATE TABLE KeyedGuidObjects;");
			db.Execute("TRUNCATE TABLE KeyedIntObjects;");
			db.Execute("TRUNCATE TABLE ListObjects;");
			db.Execute("TRUNCATE TABLE NoKeyNonDistinctObjects;");
			db.Execute("DBCC CHECKIDENT ('IdentityObjects', RESEED, 0) WITH NO_INFOMSGS;");

			int pos;
			for (var i = 0; i < 15; i += 1 )
			{
				pos = i + 1;

				var cko = new CompositeKeyObject
				{
					Key1ID = (pos / 5) + 1,
					Key2ID = (pos / 3) + 1,
					Key3ID = (pos % 5),
					TextData = "Text" + pos,
					DateEntered = new DateTime(1970, 1, 1).AddYears(pos),
					DateUpdated = DateTime.Now
				};
				db.Insert(cko);
				InMemoryCompositeKeyObjects.Add(cko);

				var io = new IdentityObject
				{
						Name = "Name " + pos,
						Age = (pos % 2 == 0) ? (int?)pos * 3 : null,
						DateOfBirth = new DateTime(1970, 1, 1).AddYears(pos),
						Savings = (pos*100000) / 13,
						DependentCount = (pos % 2 == 1) ? (byte?)pos : null,
						Gender = (pos % 2 == 0) ? "M" : "F",
						IsActive = (pos % 2 == 1)
				};
				db.Insert(io);
				InMemoryIdentityObjects.Add(io);

				var kgo = new KeyedGuidObject
				{
					Id = System.Guid.NewGuid(),
					Name = "Name " + pos,
					Age = (pos % 2 == 0) ? (int?)pos * 3 : null,
					DateOfBirth = new DateTime(1970, 1, 1).AddYears(pos),
					Savings = (pos * 100000) / 13,
					DependentCount = (pos % 2 == 1) ? (byte?)pos : null,
					Gender = (pos % 2 == 0) ? "M" : "F",
					IsActive = (pos % 2 == 1)
				};
				db.Insert(kgo);
				InMemoryKeyedGuidObjects.Add(kgo);

				var kio = new KeyedIntObject
				{
					Id = pos,
					Name = "Name " + pos,
					Age = (pos % 2 == 0) ? (int?)pos * 3 : null,
					DateOfBirth = new DateTime(1970, 1, 1).AddYears(pos),
					Savings = (pos * 100000) / 13,
					DependentCount = (pos % 2 == 1) ? (byte?)pos : null,
					Gender = (pos % 2 == 0) ? "M" : "F",
					IsActive = (pos % 2 == 1)
				};
				db.Insert(kio);
				InMemoryKeyedIntObjects.Add(kio);

				var nkndo = new NoKeyNonDistinctObject
				{
					FullName = "Name " + pos % 2,
					ItemInt = pos % 2,
					OptionalInt = (pos % 2 == 0) ? (int?)42 : null,
					Color = (pos % 2 == 0) ? "Red" : "Blue"
				};
				db.Insert(nkndo);
				InMemoryNoKeyNonDistinctObjects.Add(nkndo);

				var owct = new ObjectsWithCustomType
				{
					Id = "StringId_" + pos.ToString(),
					Name = "Blah",
					MySpecialTypeField = new DateTime(1925 + pos, 2, 15)
				};
				db.Insert(owct);
				InMemoryObjectsWithCustomType.Add(owct);
			}


			InMemoryListObjects.Add(new ListObject { 
				Id = 1,
				ShortName = "LiveLetter",
				Description = "Live with Letter of Intent Only",
				StatusKey = "A",
				SortOrder =1
			});

			InMemoryListObjects.Add(new ListObject
			{
				Id = 2,
				ShortName = "TakesPatients",
				Description = "Will Accept Most Patients",
				StatusKey = "P",
				SortOrder = 10
			});

			InMemoryListObjects.Add(new ListObject
			{
				Id = 3,
				ShortName = "Active",
				Description = "Active",
				StatusKey = "I",
				SortOrder = 20
			});

			InMemoryListObjects.Add(new ListObject
			{
				Id = 4,
				ShortName = "Prospect",
				Description = "Prospect",
				StatusKey = "I",
				SortOrder = 30
			});

			InMemoryListObjects.Add(new ListObject
			{
				Id = 5,
				ShortName = "OnHold",
				Description = "Tracked but not expected to participate",
				StatusKey = "I",
				SortOrder = 90
			});

			foreach (var lo in InMemoryListObjects)
			{
				db.Insert(lo);
			}

		}

		public static string VerifyRecordCountMatchForPocoType(Type pocoType, NPoco.Database db)
		{
			if (db == null) return "No database. Run RecreateData";
			string tableName = pocoType.Name + (pocoType.Name.StartsWith("Objects") ? "" : "s");

			//InMemoryCompositeKeyObjects
			var imList = typeof(TestData).GetProperty("InMemory" + tableName).GetValue(null, null);

			int imCount = 0;
			if (imList is IEnumerable)
			{
				var enumerator = ((IEnumerable)imList).GetEnumerator();
				while (enumerator.MoveNext())
				{
					imCount += 1;
				}
			}

			int dbCount = db.ExecuteScalar<int>("SELECT COUNT(*) FROM " + tableName + ";");

			if (imCount != dbCount) return " For test of " + tableName + ": In Memory Count = " + imCount + "; but Db Count = " + dbCount;
			if (imCount == 0) return " For test of " + tableName + ": In Memory and Db have no items."; 
			return "";
		}

	}
}
