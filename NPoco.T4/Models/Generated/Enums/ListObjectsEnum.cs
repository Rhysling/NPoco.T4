using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NPoco;

namespace MyApp.Models
{
	public enum ListObjectsEnum
	{
		[Description("Live with Letter of Intent Only")]
		LiveLetter = 1,

		[Description("Will Accept Most Patients")]
		TakesPatients = 2,

		[Description("Active")]
		Active = 3,

		[Description("Prospect")]
		Prospect = 4,

		[Description("Tracked but not expected to participate")]
		OnHold = 5

	}


	public static partial class ApLists
	{
		public static List<Tuple<int, string>> GetListObjectList(string zeroItemText, string negOneItemText)
		{
			var lst = new List<Tuple<int, string>>();
			if (!String.IsNullOrWhiteSpace(negOneItemText)) lst.Add(new Tuple<int, string>(-1, negOneItemText));
			if (!String.IsNullOrWhiteSpace(zeroItemText)) lst.Add(new Tuple<int, string>(0, zeroItemText));

			lst.Add(new Tuple<int, string>(1, "LiveLetter"));
			lst.Add(new Tuple<int, string>(2, "TakesPatients"));
			lst.Add(new Tuple<int, string>(3, "Active"));
			lst.Add(new Tuple<int, string>(4, "Prospect"));
			lst.Add(new Tuple<int, string>(5, "OnHold"));

			return lst;
		}

		public static List<SelectListItem> GetListObjectSL(string zeroItemText, string negOneItemText)
		{
			return GetListObjectList(zeroItemText, negOneItemText).Select(a => new System.Web.Mvc.SelectListItem { Value = a.Item1.ToString(), Text = a.Item2 }).ToList();
		}
	}
}
