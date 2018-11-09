using FindCoin.core;
using FindCoin.Mysql;
using FindCoin.thinneo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FindCoin.Block
{
	class SaveHashlist : ISave
	{
		private static SaveHashlist instance = null;
		public static SaveHashlist getInstance()
		{
			if (instance == null)
			{
				return new SaveHashlist();
			}
			return instance;
		}

		public override void Save(JToken jObject, string path)
		{
			JObject hashresult = new JObject();
		
			hashresult["result"] = jObject["result"];

			List<string> slist = new List<string>();
		
			slist.Add(jObject["hashlist"].ToString());
			MysqlConn.ExecuteDataInsert("hashlist", slist);			
		}
	}
}
