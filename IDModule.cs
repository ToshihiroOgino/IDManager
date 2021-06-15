using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot
{
	class JsonManager
	{
		public static void AddUserData(ulong id, string service, string value)
		{
			//既存のデータを取得し、新しいデータを追加
			var UserData = ReadByID(id);
			int kn = 0;
			foreach (string Key in UserData.Keys)
			{
				if (kn == 0 && Key == "EmptyEmptyEmptyEmptyEmpty")
				{
					Console.WriteLine("empty");
					AddUserData(id, service, value);
					return;
				}
				kn++;
			}

			UserData.Add(service, value);
			string json = JsonConvert.SerializeObject(UserData);

			//ファイル書き込み
			string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Json\" + id.ToString() + ".json";
			using (var sw = new StreamWriter(path, false))
			{
				sw.Write(json);
			}

			Console.WriteLine("Add (ID:{0} Key:{1} Value:{2})", id, service, value);
		}

		public static void DeleteUserData(ulong id, string service)
		{
			var UserData = ReadByID(id);
			int kn = 0;
			foreach (string Key in UserData.Keys)
			{
				if (kn == 0 && Key == "EmptyEmptyEmptyEmptyEmpty")
					return;
				kn++;
			}

			UserData.Remove(service);

			string json = JsonConvert.SerializeObject(UserData);

			//ファイル書き込み
			string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Json\" + id.ToString() + ".json";
			using (var sw = new StreamWriter(path, false))
			{
				sw.Write(json);
			}

			Console.WriteLine("Delete (ID:{0} Key:{1})", id, service);
		}

		public static Dictionary<string, string> ReadByID(ulong id)
		{
			string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Json\" + id.ToString() + ".json";
			string json = null;
			Dictionary<string, string> UserData = new Dictionary<string, string>();

			//該当IDのJSONファイルの存在を確認し、存在しない場合は生成する
			if (!File.Exists(path))
			{
				File.AppendAllText(path,"{}" +
					"");
				while (!File.Exists(path)) { int waiting = 1; }
				UserData.Add("EmptyEmptyEmptyEmptyEmpty", "empty");
			}
			else
			{
				using (var sr = new StreamReader(path))
				{
					json = sr.ReadToEnd();
				}
				UserData = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			}

			return UserData;
		}
	}
}
