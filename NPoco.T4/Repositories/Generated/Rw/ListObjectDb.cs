using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using MyApp.Models;

namespace MyApp.Repositories
{ 
	public class ListObjectDb : Repositories.Core.IKeyedRepository<int, ListObject>	{
		private NPoco.Database db = new NPoco.Database(Services.AppSettings.ConnectionString, DatabaseType.SqlServer2008) { Mapper = new Core.CustomTypeMapper() };

		public bool Insert(ListObject entity)
		{
			db.Insert(entity);
			return true;
		}

		public bool Update(ListObject entity)
		{
			db.Update(entity);
			return true;
		}

		public bool Delete(int id)
		{
			db.Delete<ListObject>(id);
			return true;
		}

		public bool Delete(IEnumerable<int> ids)
		{
			foreach (int id in ids)
			{
				db.Delete<ListObject>(id);
			}
			return true;
		}

		public bool Destroy(int id)
		{
			db.Delete<ListObject>(id);
			return true;
		}


		public ListObject FindBy(int id)
		{
			return db.SingleOrDefaultById<ListObject>(id);
		}

		public IEnumerable<ListObject> All()
		{
			return db.Fetch<ListObject>(" ");
		}

		public int MaxId()
		{
			return db.Single<int>("SELECT MAX(Id) FROM [ListObjects]");
		}
	}
}	
	
