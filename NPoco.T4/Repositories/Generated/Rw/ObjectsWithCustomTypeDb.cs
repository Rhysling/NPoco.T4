using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using MyApp.Models;

namespace MyApp.Repositories
{ 
	public class ObjectsWithCustomTypeDb : Repositories.Core.IKeyedRepository<string, ObjectsWithCustomType>	{
		private NPoco.Database db = new NPoco.Database(Services.AppSettings.ConnectionString, DatabaseType.SqlServer2008) { Mapper = new Core.CustomTypeMapper() };

		public bool Insert(ObjectsWithCustomType entity)
		{
			db.Insert(entity);
			return true;
		}

		public bool Update(ObjectsWithCustomType entity)
		{
			db.Update(entity);
			return true;
		}

		public bool Delete(string id)
		{
			db.Delete<ObjectsWithCustomType>((object)id);
			return true;
		}

		public bool Delete(IEnumerable<string> ids)
		{
			foreach (string id in ids)
			{
				db.Delete<ObjectsWithCustomType>(id);
			}
			return true;
		}

		public bool Destroy(string id)
		{
			db.Delete<ObjectsWithCustomType>((object)id);
			return true;
		}


		public ObjectsWithCustomType FindBy(string id)
		{
			return db.SingleOrDefaultById<ObjectsWithCustomType>((object)id);
		}

		public IEnumerable<ObjectsWithCustomType> All()
		{
			return db.Fetch<ObjectsWithCustomType>(" ");
		}

		public string MaxId()
		{
			return db.Single<string>("SELECT MAX(Id) FROM [ObjectsWithCustomType]");
		}
	}
}	
	
