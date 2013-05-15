using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyApp.Models.Core;
using NPoco;

namespace MyApp.Models
{
	// ***
	// This POCO was initially scaffolded by the generator then
	// modified by hand to add the custom type and the EquivalentTypeForTesting
	// attribute (which plugs in an equivalent system type for testing).
	// ***
	
	[TableName("ObjectsWithCustomType")]
	[PrimaryKey("Id", false)]
	[ExplicitColumns]
	public partial class ObjectsWithCustomType : MyAppDB.Record<ObjectsWithCustomType>, Repositories.Core.IKeyed<int>
	{
		[NPoco.Column] 
		public int Id { get; set; }
		
		[NPoco.Column] 
		[StringLength(200)]
		public string Name { get; set; }
		
		[NPoco.Column]
		[EquivalentTypeForTesting("DateTime?")]
		public SampleCustomType? MySpecialTypeField { get; set; }
		
	}
}
