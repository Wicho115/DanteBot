using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Net;
using DSharpPlus.Lavalink;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

using DanteBot.Attributes;
using DanteBot.Handlers;
using DanteBot.CustomMessages;

namespace DanteBot{
    public class Bot{        
        public DiscordClient client {get; private set;}
        public ServiceProvider services {get; private set;}
        public InteractivityExtension Interactivity {get; private set;}
        public CommandsNextExtension commands {get; private set;}

        public async Task RunAsync(){
            var json = string.Empty;
            using(var fs = File.OpenRead("config.json"))
            using(var sr = new StreamReader(fs, new UTF8Encoding(false)))
            json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration{
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            client = new DiscordClient(config);            

            #region LAVALINK CONNECTION
            var endpoint = new ConnectionEndpoint{
                Hostname = "127.0.0.1",
                Port = 2333,
            };

            var lavalinkConfig = new LavalinkConfiguration{
                Password = "youshallnotpass",
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint,
                SocketAutoReconnect = true
            };

            var lavalink = client.UseLavalink();            
            #endregion

            #region SERVICE PROVIDER                        
            var mapaGuild = await client.GetGuildAsync(804581949615505438);

            services = new ServiceCollection()
                .AddSingleton<Random>()
                .AddSingleton<GameSessionHandler>()
                .AddSingleton<GameSessionMessages>()
                .AddSingleton<DeadGameMessages>()
                .AddSingleton<TimerSession>()
                .AddSingleton<MapacheGuild>(_ =>{
                    return new MapacheGuild(mapaGuild);
                })
                .BuildServiceProvider();                     

            #endregion
            #region COMMAND CONFIG
            client.Ready += OnClientReady; 
                       

            client.UseInteractivity(new InteractivityConfiguration{
                Timeout = TimeSpan.FromMinutes(2),
                PollBehaviour = PollBehaviour.KeepEmojis
            });

            var commandsConfig = new CommandsNextConfiguration{
                StringPrefixes = new string[] {configJson.Prefix},
                EnableDms = true,
                EnableMentionPrefix = true,
                Services = services
            };            
            commands = client.UseCommandsNext(commandsConfig);            
            commands.CommandErrored += CmdHandleError;

            commands.RegisterCommands<FirstCommandModule>();
            //commands.RegisterCommands<LavalinkCommands>();            
            commands.RegisterCommands<FunCommandModule>();
            commands.RegisterCommands<BananaCommandModule>();
            commands.RegisterCommands<GameCommandModule>();
            commands.RegisterCommands<ModCommandModule>();            
            #endregion

            #region ASYNC CONNECTIONS
            await client.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);
            #endregion

            await Task.Delay(-1);
        }
        
        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e){            
            return Task.CompletedTask;
        }

        private async Task CmdHandleError(CommandsNextExtension _ , CommandErrorEventArgs e){    
            try{
                var failedChecks = ((ChecksFailedException)e.Exception).FailedChecks;
                foreach (var failedCheck in failedChecks)
                {
                    if(failedCheck is MustBeBananaAttribute){
                        await e.Context.RespondAsync("Solo banana para arriba pueden usar ese");
                    }
                    if(failedCheck is CooldownAttribute){
                        var cooldown = (CooldownAttribute)failedCheck;
                        await e.Context
                        .RespondAsync($"Cooldown for like {(int)cooldown.GetRemainingCooldown(e.Context).TotalSeconds} seconds");
                    }
                    return;
                }
            }catch(Exception ex){
                Console.WriteLine($"\n{ex.Message}");
            }                        
        }
    }
}