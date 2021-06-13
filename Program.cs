using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot
{
	class Program
	{
		public static ulong UserID = 0;

		public static DiscordSocketClient _client;
		public static CommandService _commands;
		public static IServiceProvider _services;

		static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();
			_client.Log += Log;

			_commands = new CommandService();
			_services = new ServiceCollection().BuildServiceProvider();
			_client.MessageReceived += CommandRecieved;

			var token = "--------------------Token--------------------";
			await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			await Task.Delay(-1);
		}

		private async Task CommandRecieved(SocketMessage messageParam)
		{
			var message = messageParam as SocketUserMessage;
			if (message == null) return;

			int argPos = 0;
			if (!(message.HasCharPrefix('!', ref argPos) ||
				message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
				message.Author.IsBot)
				return;

			UserID = message.Author.Id;
			var context = new SocketCommandContext(_client, message);

			await _commands.ExecuteAsync(
				context: context,
				argPos: argPos,
				services: null);
		}

		public Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
