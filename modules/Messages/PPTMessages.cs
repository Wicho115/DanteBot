using System.Collections.Generic;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

namespace DanteBot.CustomMessages{
    public class DeadGameMessages{
        public DiscordEmbedBuilder Winner(DiscordMember ganador, DiscordMember perdedor){
            return new DiscordEmbedBuilder{
                Title = $"{ganador.Username} Es el ganador!",
                Color = DiscordColor.Purple,
                Description = $"{perdedor.Mention} se lleva Muted por 30 segundos",
                Thumbnail = 
                new DiscordEmbedBuilder.EmbedThumbnail{Url = ganador.AvatarUrl},
                Footer = new DiscordEmbedBuilder.EmbedFooter{IconUrl = perdedor.AvatarUrl, 
                Text = $"{perdedor.Username} es el perdedor"},
            };
        }

        public DiscordEmbedBuilder Tie(CommandContext ctx, string title = "Nadie gana! Suerte la proxima!"){
            return new DiscordEmbedBuilder{
                Title = title,
                Color = DiscordColor.Aquamarine,
                Description = "Hay Un Empate!!!",
                Author = new DiscordEmbedBuilder.EmbedAuthor{
                    IconUrl = ctx.Client.CurrentUser.AvatarUrl,
                    Name = "DanteBot"
                }
            };
        }

        public async Task<MensajesJugadoresPPT> PPTSendMessage(
            DiscordMember retado, DiscordMember actual){

            var gameEmbed = new DiscordEmbedBuilder{
                Color = DiscordColor.Blue,
                Title = "Selecciona Tu Ataque",
                Description = $"Reacciona a una de las siguientes opciones y elige con que arma lucharás!",
                Footer = new DiscordEmbedBuilder.EmbedFooter{Text = "💎  ||  📄  ||  ✂"}
            };

            var mensajeRetado = await retado.SendMessageAsync(gameEmbed);
            var mensajeJugador = await actual.SendMessageAsync(gameEmbed);
                        
            return new MensajesJugadoresPPT{
                MensajeJugador = mensajeJugador,
                MensajeRetado = mensajeRetado
            };
        }
    }

    public class MensajesJugadoresPPT{
        public DiscordMessage MensajeRetado{get; set;}
        public DiscordMessage MensajeJugador {get; set;}

        public void Deconstruct(out DiscordMessage mensajeRetado, out DiscordMessage mensajeJugador) => (mensajeRetado, mensajeJugador) = (MensajeRetado, MensajeJugador);
    }
}