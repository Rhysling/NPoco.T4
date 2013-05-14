using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApp.Models.Core;

namespace MyApp.Repositories.Core
{
	public class CustomTypeMapper : NPoco.DefaultMapper
	{
		// Mapper exmple

		public override Func<object, object> GetFromDbConverter(Type DestType, Type SourceType)
		{
			if (DestType == typeof(SampleCustomModel))
			{
				return x => new SampleCustomModel((DateTime)x);
			}
			return base.GetFromDbConverter(DestType, SourceType);
		}

		public override Func<object, object> GetToDbConverter(Type DestType, Type SourceType)
		{
			if (SourceType == typeof(SampleCustomModel))
			{
				return x => ((SampleCustomModel)x).Value;
			}
			return base.GetToDbConverter(DestType, SourceType);
		}
	}
}
