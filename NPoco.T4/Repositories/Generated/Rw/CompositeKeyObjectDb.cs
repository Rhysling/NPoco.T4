using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using MyApp.Models;

namespace MyApp.Repositories
{ 
	public class CompositeKeyObjectDb	{
		private NPoco.Database db = new NPoco.Database(Services.AppSettings.ConnectionString, DatabaseType.SqlServer2008) { Mapper = new Core.CustomTypeMapper() };

		public bool Insert(CompositeKeyObject entity)
		{
			db.Insert(entity);
			return true;
		}

		public bool Update(CompositeKeyObject entity)
		{
			db.Update(entity);
			return true;
		}

		public bool Delete(int id)
		{
			db.Delete<CompositeKeyObject>(id);
			return true;
		}

		public bool Delete(IEnumerable<int> ids)
		{
			foreach (int id in ids)
			{
				db.Delete<CompositeKeyObject>(id);
			}
			return true;
		}

		public bool Destroy(int id)
		{
			db.Delete<CompositeKeyObject>(id);
			return true;
		}


		public CompositeKeyObject FindBy(int id)
		{
			return db.SingleOrDefaultById<CompositeKeyObject>(id);
		}

		public IEnumerable<CompositeKeyObject> All()
		{
			return db.Fetch<CompositeKeyObject>(" ");
		}

		public int MaxId()
		{
			return db.Single<int>("SELECT MAX(Id) FROM [CompositeKeyObjects]");
		}
	}
}	
	
