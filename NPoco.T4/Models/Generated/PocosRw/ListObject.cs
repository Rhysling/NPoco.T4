using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NPoco;

namespace MyApp.Models
{
	[TableName("ListObjects")]
	[PrimaryKey("Id", false)]
	[ExplicitColumns]
	public partial class ListObject : MyAppDB.Record<ListObject>, Repositories.Core.IKeyed<int>
	{
		[NPoco.Column] 
		public int Id { get; set; }
		
		[NPoco.Column] 
		[StringLength(50)]
		[Required()]
		public string ShortName { get; set; }
		
		[NPoco.Column] 
		[StringLength(255)]
		[Required()]
		public string Description { get; set; }
		
		[NPoco.Column] 
		[StringLength(1)]
		[Required()]
		public string StatusKey { get; set; }
		
		[NPoco.Column] 
		public int SortOrder { get; set; }
		
	}
}
