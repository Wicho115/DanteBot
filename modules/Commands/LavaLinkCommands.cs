using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Lavalink;
using System.Linq;

namespace DanteBot{
    public class LavalinkCommands : BaseCommandModule{

        [Command]
        public async Task Join(CommandContext ctx){            
            var lava = ctx.Client.GetLavalink();
            if(!lava.ConnectedNodes.Any()){
                await ctx.RespondAsync($"The lavalink connection is not established... {ctx.Channel.Name}");
                return;
            }
            if(ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null){
                await ctx.RespondAsync("No estás en un canal de voz");
                return;
            }
            var actualChannel = ctx.Member.VoiceState.Channel;
            var node = lava.ConnectedNodes.Values.First();

            await node.ConnectAsync(actualChannel);
            await ctx.RespondAsync($"Joined {actualChannel.Name}!");
        }

        [Command]
        public async Task Leave(CommandContext ctx){            
            var lava = ctx.Client.GetLavalink();   
            if(!lava.ConnectedNodes.Any()){
                await ctx.RespondAsync($"La conexion a lavaLink no se ha concretado... {ctx.Channel.Name}");
                return;
            }
            if(ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null){
                await ctx.RespondAsync("No estás en un canal de voz");
                return;
            }
            var actualChannel = ctx.Member.VoiceState.Channel;
            var node = lava.ConnectedNodes.Values.First();

            var conn = node.GetGuildConnection(actualChannel.Guild);

            if(conn == null){
                await ctx.RespondAsync("Lavalink no está conectado");
                return;
            }

            await conn.DisconnectAsync();
            await ctx.RespondAsync($"Dejé el canal: {actualChannel.Name}");
        }

        [Command]
        public async Task Play(CommandContext context, [RemainingText] string search){            
            if(context.Member.VoiceState == null || context.Member.VoiceState.Channel == null){
                await context.RespondAsync("No estás en un canal de voz");
                return;
            }
            var actualChannel = context.Member.VoiceState.Channel;
            var lava = context.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(context.Member.Guild);
            
            if(conn == null){
                await node.ConnectAsync(actualChannel);
                conn = node.GetGuildConnection(context.Member.Guild);
            }

            if(conn.Channel.Id != context.Member.VoiceState.Channel.Id){
                    await context.RespondAsync("Alguien ya está escuchando en otro canal");
                    return;
            }

            var loadResult = await node.Rest.GetTracksAsync(search);

            if(loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed 
                || 
            !loadResult.Tracks.Any()){
                await context.RespondAsync($"La busqueda falló para la busqueda: {search}.");
                return;
            }

            var track = loadResult.Tracks.First();
            await conn.PlayAsync(track);

            await context.RespondAsync($"Ahora estoy tocando: {track.Title}");
        }

        [Command]
        public async Task Play(CommandContext context, Uri url){
            if(context.Member.VoiceState == null || context.Member.VoiceState.Channel == null){
                await context.RespondAsync("No estás en un canal de voz");
                return;
            }

            var lava = context.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(context.Member.Guild);

            if(conn == null){
                await context.RespondAsync("Lavalink no está conectado");
                return;
            }

            var loadResult = await node.Rest.GetTracksAsync(url);

            if(loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed 
                || 
            !loadResult.Tracks.Any()){
                await context.RespondAsync($"La busqueda falló para la busqueda: {url}.");
                return;
            }

            var track = loadResult.Tracks.First();
            await conn.PlayAsync(track);

            await context.RespondAsync($"Ahora estoy tocando: {track.Title}");
        }

        [Command]
        public async Task Pause(CommandContext ctx){
            if(ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null){
                await ctx.RespondAsync("No estas conectado");
                return;
            }

            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if(conn == null){
                await ctx.RespondAsync("Lavalink no está conectado");
                return;
            }

            if(conn.CurrentState.CurrentTrack == null){
                await ctx.RespondAsync("No hay rola aún");
                return;
            }
            await conn.PauseAsync();
        }
        
    }
}