using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using MyApp.Models;

namespace MyApp.Repositories
{ 
	public class ObjectsWithCustomTypeDb : Repositories.Core.IKeyedRepository<int, ObjectsWithCustomType>	{
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

		public bool Delete(int id)
		{
			db.Delete<ObjectsWithCustomType>(id);
			return true;
		}

		public bool Delete(IEnumerable<int> ids)
		{
			foreach (int id in ids)
			{
				db.Delete<ObjectsWithCustomType>(id);
			}
			return true;
		}

		public bool Destroy(int id)
		{
			db.Delete<ObjectsWithCustomType>(id);
			return true;
		}


		public ObjectsWithCustomType FindBy(int id)
		{
			return db.SingleOrDefaultById<ObjectsWithCustomType>(id);
		}

		public IEnumerable<ObjectsWithCustomType> All()
		{
			return db.Fetch<ObjectsWithCustomType>(" ");
		}

		public int MaxId()
		{
			return db.Single<int>("SELECT MAX(Id) FROM [ObjectsWithCustomType]");
		}
	}
}	
	
