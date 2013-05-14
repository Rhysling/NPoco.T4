using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using MyApp.Models;

namespace MyApp.Repositories
{ 
	public class NoKeyNonDistinctObjectDb
	{
		private NPoco.Database db = new NPoco.Database(Services.AppSettings.ConnectionString, DatabaseType.SqlServer2008) { Mapper = new Core.CustomTypeMapper() };

		public IEnumerable<NoKeyNonDistinctObject> All()
		{
			return db.Fetch<NoKeyNonDistinctObject>(" ");
		}

		//Example - filtered list:
		public IEnumerable<NoKeyNonDistinctObject> FilteredList(string str1, string str2)
		{
			return db.Fetch<NoKeyNonDistinctObject>("WHERE (v1=@p1) AND (v2=@p2)", new {p1 = str1, p2 = str2});
		}
		
		//Example - paged & filtered list:
		public Page<NoKeyNonDistinctObject> PagedFilteredList(string str1, string str2, long page, long itemsPerPage)
		{
			return db.Page<NoKeyNonDistinctObject>(page, itemsPerPage, "WHERE (v1=@p1) AND (v2=@p2)", new {p1 = str1, p2 = str2});
		}

		// More methods here ***
	}
}	
