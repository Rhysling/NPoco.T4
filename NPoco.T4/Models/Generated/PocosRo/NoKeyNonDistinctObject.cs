using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NPoco;

namespace MyApp.Models
{
	[TableName("NoKeyNonDistinctObjects")]
	[ExplicitColumns]
	public partial class NoKeyNonDistinctObject : MyAppDB.Record<NoKeyNonDistinctObject>
	{
		[NPoco.Column] 
		public string FullName { get; set; }

		[NPoco.Column] 
		public int ItemInt { get; set; }

		[NPoco.Column] 
		public int? OptionalInt { get; set; }

		[NPoco.Column] 
		public string Color { get; set; }

	}
}
