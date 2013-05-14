using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using MyApp.Models;

namespace MyApp.Repositories
{ 
	public class KeyedGuidObjectDb : Repositories.Core.IKeyedRepository<Guid, KeyedGuidObject>	{
		private NPoco.Database db = new NPoco.Database(Services.AppSettings.ConnectionString, DatabaseType.SqlServer2008) { Mapper = new Core.CustomTypeMapper() };

		public bool Insert(KeyedGuidObject entity)
		{
			db.Insert(entity);
			return true;
		}

		public bool Update(KeyedGuidObject entity)
		{
			db.Update(entity);
			return true;
		}

		public bool Delete(Guid id)
		{
			db.Delete<KeyedGuidObject>(id);
			return true;
		}

		public bool Delete(IEnumerable<Guid> ids)
		{
			foreach (Guid id in ids)
			{
				db.Delete<KeyedGuidObject>(id);
			}
			return true;
		}

		public bool Destroy(Guid id)
		{
			db.Delete<KeyedGuidObject>(id);
			return true;
		}


		public KeyedGuidObject FindBy(Guid id)
		{
			return db.SingleOrDefaultById<KeyedGuidObject>(id);
		}

		public IEnumerable<KeyedGuidObject> All()
		{
			return db.Fetch<KeyedGuidObject>(" ");
		}

		public Guid MaxId()
		{
			return db.Single<Guid>("SELECT MAX(Id) FROM [KeyedGuidObjects]");
		}
	}
}	
	
