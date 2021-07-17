using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Entities;

using DanteBot.Attributes;
using DanteBot.Handlers;
using DanteBot.Entities;
using DanteBot.CustomMessages;

namespace DanteBot{
    public class GameCommandModule : BaseCommandModule{

        public GameSessionHandler service {private get; set;}
        public MapacheGuild mapaService {private get; set;}
        public GameSessionMessages gameMessages {private get; set;}

        [Command("CreateSession")]
        [RequiredChannel(865295974585860097)]
        public async Task CrearSesion(
            CommandContext ctx,
            string game, 
            int totalJugadores,
            [RemainingText] string description){      

                var rolJuegos = mapaService.JuegosRole;
                var EmbedSesioncreada = gameMessages
                .CreateSessionMessage(ctx.User, game, description, totalJugadores-1);

                var CanalVoz = await ctx.Guild
                .CreateChannelAsync($"{ctx.User.Username}-Sesion",
                ChannelType.Voice,
                mapaService.GameCategoryChannel,
                userLimit : totalJugadores);

                
                var sesion = new GameSession(
                    ctx.User, 
                    CanalVoz, 
                    totalJugadores, 
                    $"{ctx.User.Username}-Sesion", 
                    game,
                    description);
                service.StartSesion(sesion);

                await mapaService.SesionChannel.SendMessageAsync(rolJuegos.Mention, EmbedSesioncreada);
        }

        #region firstcomman
        [Command("CreateChannel")]
        [RequireRoles(RoleCheckMode.All, "Administrador")]
        public async Task CreateChannel(CommandContext ctx, string name){
            var Channel = ctx.Guild.GetChannel(804950042841448500);
            var resultChannel = await ctx.Guild.CreateChannelAsync(name, ChannelType.Text, parent : Channel);
            await ctx.RespondAsync($"Se ha creado el canal: {name}");
            Thread.Sleep(10000);
            await resultChannel.DeleteAsync();
        }
        #endregion
    }
}