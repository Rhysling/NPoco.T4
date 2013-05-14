using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using MyApp.Models;

namespace MyApp.Repositories
{ 
	public class IdentityObjectDb : Repositories.Core.IIdentityRepository<IdentityObject>
	{
		private NPoco.Database db = new NPoco.Database(Services.AppSettings.ConnectionString, DatabaseType.SqlServer2008) { Mapper = new Core.CustomTypeMapper() };

		public int Save(IdentityObject entity)
		{
			db.Save<IdentityObject>(entity);
			return entity.Id;
		}

		public bool Save(IEnumerable<IdentityObject> items)
		{
			foreach (IdentityObject item in items)
			{
				db.Save<IdentityObject>(item);
			}
			return true;
		}

		public bool Delete(int id)
		{
			db.Delete<IdentityObject>(id);
			return true;
		}

		public bool Delete(IEnumerable<int> ids)
		{
			foreach (int id in ids)
			{
				db.Delete<IdentityObject>(id);
			}
			return true;
		}

		public bool Destroy(int id)
		{
			db.Delete<IdentityObject>(id);
			return true;
		}


		public IdentityObject FindBy(int id)
		{
			return db.SingleOrDefaultById<IdentityObject>(id);
		}

		public IEnumerable<IdentityObject> All()
		{
			return db.Fetch<IdentityObject>(" ");
		}


		public void ReseedKey()
		{
			string sql ="DECLARE @@MaxId int; ";
			sql += "SELECT @@MaxId = MAX(Id) FROM IdentityObjects; ";
			sql += "DBCC CHECKIDENT ('IdentityObjects', RESEED, @@MaxId) WITH NO_INFOMSGS;";
			
			db.Execute(sql);
		}

	}
}	

