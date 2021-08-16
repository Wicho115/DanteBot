using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Entities;

using Extension.DiscordMemberExtension;
using Extension.DiscordMessageExtension;

using DanteBot.Handlers;
using DanteBot.CustomMessages;

namespace DanteBot{
    
    public class FunCommandModule : BaseCommandModule{

        public DeadGameMessages deadGameMessages {private get; set;}
        public MapacheGuild mapaService {private get; set;}

        #region CONFESAR COMMAND
        [Command("Confesar")]
        [Description("Comando que sirve para confesar tus más oscuros pecados (SOLO DM)")]
        [Cooldown(1, 10f, CooldownBucketType.User)]        
        public async Task ConfesarCommand(CommandContext ctx, [RemainingText] 
        [Description("La confesión que deseas realizar")] string confesion){
            var interactivity = ctx.Client.GetInteractivity();
            if(!ctx.Channel.IsPrivate){
                await ctx.Message.DeleteAsync();
                await ctx.Channel.SendMessageAsync($"HEY! {ctx.User.Mention}, Las confesiones solo se reciben por DM");       
                await ctx.Member.SendMessageAsync(
                    "Hola! 👋 Por acá puedes confesarte usando el comando '!D confesar [confesion]'");
                return;        
            }
            //Los ids son de el canal y el server de familia mapache
            var GuildMapache = await ctx.Client.GetGuildAsync(804581949615505438, withCounts : true);
            var ChannelConfesiones = GuildMapache.GetChannel(858902825366192128);   

            try{
                await GuildMapache.GetMemberAsync(ctx.User.Id);      
            }
            catch (System.Exception){
                await ctx.RespondAsync("No eres miembro de Familia mapache");                
                return;            
            }

            var yesEmote = DiscordEmoji.FromName(ctx.Client, ":+1:");
            var noEmote = DiscordEmoji.FromName(ctx.Client, ":-1:");
            var confirmedMessage = await ctx.Channel.SendMessageAsync("¿Deseas que tu confesión sea Anónima?");
            await confirmedMessage.CreateReactionAsync(yesEmote);
            await confirmedMessage.CreateReactionAsync(noEmote);
            var result = await interactivity.WaitForReactionAsync(
                x => x.Message == confirmedMessage &&
                (x.Emoji == yesEmote || x.Emoji == noEmote) &&
                x.User == ctx.User
            );
            await confirmedMessage.DeleteAsync();
            if(result.Result == null){
                var badGateway = new DiscordEmbedBuilder{
                Title = "Tiempo Expirado",
                Color = DiscordColor.Red,
                Description = "La petición expiró :(  -  Intentalo de nuevo más tarde"
            };    
                await ctx.Channel.SendMessageAsync(badGateway);
                return;
            }else if(result.Result.Emoji == yesEmote){
                var confesionAnonEmbed = new DiscordEmbedBuilder{
                    Title = "Nueva Confesión!",
                    Color = DiscordColor.Gold,
                    Description = confesion,
                    Footer = new DiscordEmbedBuilder.EmbedFooter{
                        Text = "Confesión anónima", 
                        IconUrl = ctx.Client.CurrentUser.AvatarUrl}
                };            
                await ChannelConfesiones.SendMessageAsync(confesionAnonEmbed);
            }else if(result.Result.Emoji == noEmote){
                var confesionEmbed = new DiscordEmbedBuilder{
                    Title = $"Nueva Confesión De: {ctx.User.Username}",
                    Color = DiscordColor.Gold,
                    Description = confesion,
                    Footer = new DiscordEmbedBuilder.EmbedFooter{
                        Text = ctx.User.Username, 
                        IconUrl = ctx.User.AvatarUrl}
                };            
                await ChannelConfesiones.SendMessageAsync(confesionEmbed);
            }else return;
            
            await ctx.RespondAsync("Tu confesión ha sido enviada!");
        }
        #endregion
        
        #region PPT COMMAND
        [Command("ppt")]
        [Cooldown(1, 10f, CooldownBucketType.User)]
        public async Task PPT(CommandContext ctx, DiscordMember retado){
            if(await MessageHandler.AwaitResponse(ctx, deadGameMessages.Confirmation(ctx))){
                await PiedraPapelTijera(ctx, retado, true);                
            }else{
                await PiedraPapelTijera(ctx, retado, false);
            }
        }
        #endregion

        #region PPT
        
        public async Task PiedraPapelTijera(CommandContext ctx, DiscordMember retado, bool muerte){
            if(ctx.Channel.IsPrivate){
                await ctx.RespondAsync("Debes estar en el servidor para usar este comando");
                return;
            }

            if(retado.IsBot){
                if(retado == ctx.Client.CurrentUser){
                    Console.WriteLine("Estás jugando con el bot");
                    await PiedraPapelTijera(ctx, muerte);
                    return;
                }
                if(ctx.User.IsBanana())
                await ctx.RespondAsync("BANANA NO PUEDES JUGAR PIEDRA PAPEL O TIJERA CON LOS BOTS!!!");
                else 
                await ctx.RespondAsync("No puedes jugar con bots");
                return;
            }

            if(ctx.User == retado){
                await ctx.RespondAsync("No puedes jugar contigo mismo");
                return;
            }

            #region PREPARATIVOS
            var intereactivy = ctx.Client.GetInteractivity();
            Random randoms = new Random();
            var arrayPlayers = new DiscordUser[]{ctx.User, retado};
            
            var AceeptanceEmbed = new DiscordEmbedBuilder{
                Color = (muerte) ? DiscordColor.Red : DiscordColor.Purple,
                Title = "Piedra 💎, Papel 📄, Tijera ✂" + $" {((muerte) ? " || ☠" : "")}",
                Description = $"Hey! {retado.Mention}, {ctx.User.Mention} te ha retado a 'Piedra, Papel, Tijera'"
                + $"{((muerte) ? "\nA MUERTEEEE" : "")}" + ", ¿Aceptas?"
            };
            var CheckedBoxEmoji = DiscordEmoji.FromName(ctx.Client, ":ballot_box_with_check:");
            var ExBoxEmoji = DiscordEmoji.FromName(ctx.Client, ":x:");

            var messageDuel = await ctx.Channel.SendMessageAsync(AceeptanceEmbed);

            await messageDuel.CreateReactionAsync(CheckedBoxEmoji);
            await messageDuel.CreateReactionAsync(ExBoxEmoji);

            var resultReact = await intereactivy.WaitForReactionAsync(
                x => x.User == retado && (x.Emoji == CheckedBoxEmoji || x.Emoji == ExBoxEmoji) && x.Message == messageDuel);

            if(resultReact.Result.Emoji == CheckedBoxEmoji){
                await ctx.Channel.SendMessageAsync($"{retado.Mention} Ha aceptado ser desafiado!");
            }else if(resultReact.Result.Emoji == ExBoxEmoji){
                await ctx.Channel.SendMessageAsync($"{retado.Mention} Ha declinado el desafio de {ctx.User.Mention}");
                return;
            }else{
                return;
            }
            #endregion

            #region THE GAME

            var PiedraEmoji = DiscordEmoji.FromName(ctx.Client, ":gem:");
            var PapelEmoji = DiscordEmoji.FromName(ctx.Client, ":page_facing_up:");
            var TijeraEmoji = DiscordEmoji.FromName(ctx.Client, ":scissors:");

            var (mensajeRetado, mensajeJugador) = await deadGameMessages.PPTSendMessage(retado, ctx.Member);

            await mensajeRetado.CreateReactionAsync(PiedraEmoji);
            await mensajeJugador.CreateReactionAsync(PiedraEmoji);
            await mensajeRetado.CreateReactionAsync(PapelEmoji);
            await mensajeJugador.CreateReactionAsync(PapelEmoji);
            await mensajeRetado.CreateReactionAsync(TijeraEmoji);
            await mensajeJugador.CreateReactionAsync(TijeraEmoji);

            var reaccionRetado = intereactivy.WaitForReactionAsync(
                x => x.User == retado 
                && (x.Emoji == TijeraEmoji || x.Emoji == PiedraEmoji || x.Emoji == PapelEmoji) 
                && x.Message == mensajeRetado
            );
            var reaccionJugador = intereactivy.WaitForReactionAsync(
                x => x.User == ctx.User
                && (x.Emoji == TijeraEmoji || x.Emoji == PiedraEmoji || x.Emoji == PapelEmoji) 
                && x.Message == mensajeJugador
            );
            var emojiJugador = (await reaccionJugador).Result.Emoji;
            var emojiRetado = (await reaccionRetado).Result.Emoji;
            Thread.Sleep(1500);
            #endregion

            #region WINNING LOGIC
            if(emojiJugador == PiedraEmoji){
                if(emojiRetado == PapelEmoji){
                    await FinDelJuego(ctx, retado, ctx.Member, muerte);                    
                }else if(emojiRetado == TijeraEmoji){
                    await FinDelJuego(ctx, ctx.Member, retado, muerte);
                }else{
                    await Empate(ctx);
                }
            }else if(emojiJugador == PapelEmoji){
                if(emojiRetado == TijeraEmoji){
                    await FinDelJuego(ctx, retado, ctx.Member, muerte);
                }else if(emojiRetado == PiedraEmoji){
                    await FinDelJuego(ctx, ctx.Member, retado, muerte);
                }else{
                    await Empate(ctx);
                }
            }else if(emojiJugador == TijeraEmoji){
                if(emojiRetado == PiedraEmoji){
                    await FinDelJuego(ctx, retado, ctx.Member, muerte);
                }else if(emojiRetado == PapelEmoji){
                    await FinDelJuego(ctx, ctx.Member, retado, muerte);
                }else{
                    await Empate(ctx);
                }
            }
            #endregion
        }   
        #endregion 

        #region EndGame Methods
        public async Task FinDelJuego(CommandContext ctx, DiscordMember ganador, DiscordMember perdedor, bool muerte){
            if(muerte){
                await mapaService.Mutear(perdedor, 30000);
                await ctx.Channel.SendMessageAsync(deadGameMessages.WinnerMuerte(ganador, perdedor));
            }else
                await ctx.Channel.SendMessageAsync(deadGameMessages.Winner(ganador, perdedor));
            
        }
        public async Task Empate(CommandContext ctx){
            await ctx.Channel.SendMessageAsync(deadGameMessages.Tie(ctx));
        }
        #endregion

        #region PPTBOT
        public async Task PiedraPapelTijera(CommandContext ctx, bool muerte){
            var intereactivy = ctx.Client.GetInteractivity();
            Console.WriteLine("conseguiste interactividad");

            var retado = (await ctx.Guild.GetAllMembersAsync())
                        .Single(x => x == ctx.Client.CurrentUser);

            Console.WriteLine("se hizo un discordmember");
            Random randoms = new Random();
            
            var AceeptanceEmbed = new DiscordEmbedBuilder{
                Color = DiscordColor.Purple,
                Title = "Piedra 💎, Papel 📄, Tijera ✂",
                Description = $"Hey! estas retando a {ctx.Client.CurrentUser.Mention} a 'Piedra, Papel, Tijera.., ¿Estás seguro?'"
            };
            var CheckedBoxEmoji = DiscordEmoji.FromName(ctx.Client, ":ballot_box_with_check:");
            var ExBoxEmoji = DiscordEmoji.FromName(ctx.Client, ":x:");

            var messageDuel = await ctx.Channel.SendMessageAsync(AceeptanceEmbed);

            await messageDuel.CreateReactionAsync(CheckedBoxEmoji);
            await messageDuel.CreateReactionAsync(ExBoxEmoji);

            var resultReact = await intereactivy.WaitForReactionAsync(
                x => x.User == ctx.User && (x.Emoji == CheckedBoxEmoji || x.Emoji == ExBoxEmoji) && x.Message == messageDuel);
            await messageDuel.DeleteAsync(1000);

            if(resultReact.Result.Emoji == CheckedBoxEmoji){
                await ctx.Channel.SendMessageAsync($"Has desafiado a {ctx.Client.CurrentUser.Mention} el poderoso");
            }else if(resultReact.Result.Emoji == ExBoxEmoji){
                await ctx.Channel.SendMessageAsync($"Has declinado la oferta de desafiar a {ctx.Client.CurrentUser.Mention}");
                return;
            }else{
                return;
            }

            var PiedraEmoji = DiscordEmoji.FromName(ctx.Client, ":gem:");
            var PapelEmoji = DiscordEmoji.FromName(ctx.Client, ":page_facing_up:");
            var TijeraEmoji = DiscordEmoji.FromName(ctx.Client, ":scissors:");

            DiscordEmoji[] emojisPPT = {PiedraEmoji, PapelEmoji, TijeraEmoji};

            var emojiRetado = emojisPPT[randoms.Next(0,3)];

            var gameEmbed = new DiscordEmbedBuilder{
                Color = DiscordColor.Blue,
                Title = "Selecciona Tu Ataque",
                Description = $"Reacciona a una de las siguientes opciones y elige con que arma lucharás!",
                Footer = new DiscordEmbedBuilder.EmbedFooter{Text = "💎  ||  📄  ||  ✂"}
            };

            var message = await ctx.Member.SendMessageAsync(gameEmbed);

            await message.CreateReactionAsync(PiedraEmoji);
            await message.CreateReactionAsync(PapelEmoji);
            await message.CreateReactionAsync(TijeraEmoji);

            var reaccion = await intereactivy.WaitForReactionAsync(x => x.Message == message && 
                    (x.Emoji == PiedraEmoji || x.Emoji == PapelEmoji || x.Emoji == TijeraEmoji) &&
                    x.User == ctx.User);
            var emojiJugador = reaccion.Result.Emoji;

            Thread.Sleep(1750);
            
            #region Winning logic
            if(emojiJugador == PiedraEmoji){
                if(emojiRetado == PapelEmoji){
                    await FinDelJuego(ctx, retado, ctx.Member, muerte);                    
                }else if(emojiRetado == TijeraEmoji){
                    await FinDelJuego(ctx, ctx.Member, retado, muerte);
                }else{
                    await Empate(ctx);
                }
            }else if(emojiJugador == PapelEmoji){
                if(emojiRetado == TijeraEmoji){
                    await FinDelJuego(ctx, retado, ctx.Member, muerte);
                }else if(emojiRetado == PiedraEmoji){
                    await FinDelJuego(ctx, ctx.Member, retado, muerte);
                }else{
                    await Empate(ctx);
                }
            }else if(emojiJugador == TijeraEmoji){
                if(emojiRetado == PiedraEmoji){
                    await FinDelJuego(ctx, retado, ctx.Member, muerte);
                }else if(emojiRetado == PapelEmoji){
                    await FinDelJuego(ctx, ctx.Member, retado, muerte);
                }else{
                    await Empate(ctx);
                }
            }
            #endregion
        }
        #endregion
    }
}