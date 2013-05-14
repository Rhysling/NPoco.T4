using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using MyApp.Models;

namespace MyApp.Repositories
{ 
	public class KeyedIntObjectDb : Repositories.Core.IKeyedRepository<int, KeyedIntObject>	{
		private NPoco.Database db = new NPoco.Database(Services.AppSettings.ConnectionString, DatabaseType.SqlServer2008) { Mapper = new Core.CustomTypeMapper() };

		public bool Insert(KeyedIntObject entity)
		{
			db.Insert(entity);
			return true;
		}

		public bool Update(KeyedIntObject entity)
		{
			db.Update(entity);
			return true;
		}

		public bool Delete(int id)
		{
			db.Delete<KeyedIntObject>(id);
			return true;
		}

		public bool Delete(IEnumerable<int> ids)
		{
			foreach (int id in ids)
			{
				db.Delete<KeyedIntObject>(id);
			}
			return true;
		}

		public bool Destroy(int id)
		{
			db.Delete<KeyedIntObject>(id);
			return true;
		}


		public KeyedIntObject FindBy(int id)
		{
			return db.SingleOrDefaultById<KeyedIntObject>(id);
		}

		public IEnumerable<KeyedIntObject> All()
		{
			return db.Fetch<KeyedIntObject>(" ");
		}

		public int MaxId()
		{
			return db.Single<int>("SELECT MAX(Id) FROM [KeyedIntObjects]");
		}
	}
}	
	
