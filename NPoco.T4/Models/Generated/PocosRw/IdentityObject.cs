using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NPoco;

namespace MyApp.Models
{
	[TableName("IdentityObjects")]
	[PrimaryKey("Id", true)]
	[ExplicitColumns]
	public partial class IdentityObject : MyAppDB.Record<IdentityObject>, Repositories.Core.IIdentity
	{
		[NPoco.Column] 
		public int Id { get; set; }
		
		[NPoco.Column] 
		[StringLength(200)]
		public string Name { get; set; }
		
		[NPoco.Column] 
		public int? Age { get; set; }
		
		[NPoco.Column] 
		public DateTime? DateOfBirth { get; set; }
		
		[NPoco.Column] 
		public decimal? Savings { get; set; }
		
		[NPoco.Column] 
		public byte? DependentCount { get; set; }
		
		[NPoco.Column] 
		[StringLength(1)]
		public string Gender { get; set; }
		
		[NPoco.Column] 
		public bool IsActive { get; set; }
		
	}
}
