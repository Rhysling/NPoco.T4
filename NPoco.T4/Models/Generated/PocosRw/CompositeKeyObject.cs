

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NPoco;

namespace MyApp.Models
{
	[TableName("CompositeKeyObjects")]
	[PrimaryKey("Key1ID, Key2ID, Key3ID", false)]
	[ExplicitColumns]
	public partial class CompositeKeyObject : MyAppDB.Record<CompositeKeyObject>
	{
		[NPoco.Column] 
		public int Key1ID { get; set; }
		
		[NPoco.Column] 
		public int Key2ID { get; set; }
		
		[NPoco.Column] 
		public int Key3ID { get; set; }
		
		[NPoco.Column] 
		[StringLength(512)]
		public string TextData { get; set; }
		
		[NPoco.Column] 
		public DateTime DateEntered { get; set; }
		
		[NPoco.Column] 
		public DateTime? DateUpdated { get; set; }
		
	}
}
