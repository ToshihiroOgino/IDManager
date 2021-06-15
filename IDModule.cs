using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot
{
	[Group("id")]
	public class IDModule : ModuleBase
	{
		[Command("add")]
		[Summary("サービスとIDを記録しておくコマンド   例：!id add Steam username")]
		public async Task Add([Remainder] string text)
		{
			ulong id = Program.UserID;
			try
			{
				string service = text.Substring(0, text.IndexOf(" "));
				string servid = text.Substring(text.IndexOf(" ") + 1);
				try
				{
					JsonManager.AddUserData(id, service, servid);
					await ReplyAsync("情報を追加しました");
				}
				catch
				{
					await ReplyAsync(service + "に関する情報が既に登録されています。IDを更新したい場合は一旦既存の情報を削除してください。");
				}
			}
			catch
			{
				await ReplyAsync("コマンドが間違っています   例：!id add Steam username");
			}
		}

		[Command("del")]
		[Summary("指定したサービスの記録を削除するコマンド   例：!id del Steam")]
		public async Task Delete([Remainder] string service)
		{
			ulong id = Program.UserID;
			try
			{
				JsonManager.DeleteUserData(id, service);
				await ReplyAsync("情報を削除しました");
			}
			catch
			{
				await ReplyAsync("コマンドが間違っています   例：!id del Steam");
			}
		}

		[Command("show")]
		[Summary("メンションした相手が登録しているIDの一覧を表示するコマンド")]
		public async Task Show([Remainder] string target)
		{
			try
			{
				target = target.Replace("<", "").Replace("@", "").Replace("!", "").Replace(">", "");
				Dictionary<string, string> UserData = JsonManager.ReadByID(Convert.ToUInt64(target));
				var user = Program._client.GetUser(Convert.ToUInt64(target));

				if (UserData.Count != 0)
				{
					string result = "```Discord : " + user.Username.ToString() + "\n";
					foreach (var (key, value) in UserData)
					{
						if (key == "EmptyEmptyEmptyEmptyEmpty")
						{
							await ReplyAsync("このユーザーはまだ情報を登録していません");
							return;
						}
						result += key + " : " + value + "\n";
					}
					result += "```";
					await ReplyAsync(result);
				}
				else
					await ReplyAsync("このユーザーはまだ情報を登録していません");

				Console.WriteLine("show UserData (ID:{0})", target);
			}
			catch
			{
				await ReplyAsync("コマンドが間違っています   例：!id show @Someone");
			}
		}

		[Command("vc")]
		[Summary("コマンドを実行したユーザーが接続しているボイスチャンネル内のユーザーの登録情報を一括表示する")]
		public async Task VC()
		{
			var user = Program._client.GetUser(Program.UserID);

			foreach (SocketGuild guild in Program._client.Guilds)
				foreach (SocketGuildChannel guildChannel in guild.Channels)
					if (guildChannel.GetType().Name.ToString() == "SocketVoiceChannel")
					{
						bool exists = false;
						foreach (SocketGuildUser targetUser in guildChannel.Users)
						{
							ulong id = targetUser.Id;
							if (id == Program.UserID)
							{
								exists = true;
								break;
							}
						}
						if (exists)
						{
							foreach (SocketGuildUser targetUser in guildChannel.Users)
								await Show(targetUser.Id.ToString());
							return;
						}
					}
			await ReplyAsync("コマンドを実行したユーザーがボイスチャンネルに接続していなければ、このコマンドは機能しません");
		}

		[Command("help")]
		[Summary("説明の表示")]
		public async Task Help()
		{
			var commands = Program._commands.Commands;

			EmbedBuilder embedBuilder = new EmbedBuilder();
			string text = "```コマンド一覧\n";

			foreach (CommandInfo command in commands)
			{
				text += command.Name + " : " + command.Summary + "\n";
			}
			await ReplyAsync(text + "```");

			Console.WriteLine("Show help");
		}
	}
}
