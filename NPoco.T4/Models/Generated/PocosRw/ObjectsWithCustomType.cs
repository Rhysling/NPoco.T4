using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NPoco;

namespace MyApp.Models
{
	[TableName("ObjectsWithCustomType")]
	[PrimaryKey("Id", false)]
	[ExplicitColumns]
	public partial class ObjectsWithCustomType : MyAppDB.Record<ObjectsWithCustomType>, Repositories.Core.IKeyed<string>
	{
		[NPoco.Column] 
		[StringLength(20)]
		[Required()]
		public string Id { get; set; }
		
		[NPoco.Column] 
		[StringLength(200)]
		public string Name { get; set; }
		
		[NPoco.Column] 
		public DateTime? MySpecialTypeField { get; set; }
		
	}
}
