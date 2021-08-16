using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

using DanteBot.Attributes;
using DanteBot.Handlers;

namespace DanteBot
{
    public class UtilSlashCommands : ApplicationCommandModule
    {
        public MapacheGuild mapaService {private get; set;}

        #region EMBED COMMAND
        [SlashCommand("embed", "comando para crear un embed")]
        [SlashRequireRole(804593782673637416)]
        public async Task EmbedCommand(InteractionContext ctx,
        [Option("Canal", "Canal en donde se enviará el embed")] DiscordChannel canal,
        [Option("Titulo", "Titulo del embed que se desea enviar")] string titulo,
        [Option("Descripcion", "Una descripcion para el embed")] string descripcion,
        [Option("Color", "Color en el que deseas el embed")] ColorEnum color = ColorEnum.Azul,
        [Option("Footer", "Texto que vaya en el footer del embed")] string footer = null,
        [Option("Imagen", "¿Quieres tener tu imagen en le embed?")] bool hasImage = false,
        [Option("Autor", "¿Quieres que salga el autor del embed?")] bool hasAuthor = false,
        [Option("ImageURL", "URL de la imagen que deseas usar en el embed")] string imageURL = null){
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource, 
                new DiscordInteractionResponseBuilder().AsEphemeral(true));
             
            var builder = new DiscordEmbedBuilder();

            builder.WithDescription(descripcion);
            builder.WithTitle(titulo);
            builder.WithColor(SelectColor(color));
            if(footer != null){
                builder.WithFooter(footer);
            }if(hasImage){
                builder.WithThumbnail(ctx.User.AvatarUrl);
            }if(hasAuthor){
                builder.WithAuthor($"{ctx.User.Username} dice:", iconUrl : ctx.User.AvatarUrl);
            }if(imageURL != null){
                builder.WithImageUrl(imageURL);
            }

            await canal.SendMessageAsync(builder);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder()
                .WithContent($"Se ha enviado el embed a: {canal.Mention}"));
        }
        #endregion

        #region POLL COMMAND
        [SlashCommand("Poll", "Comando que crea una encuesta")]
        public async Task PollCommand(InteractionContext ctx,
        [Option("Canal", "Canal en donde se desea enviar la poll")] DiscordChannel canal,
        [Option("Titulo", "Titulo de la encuesta")] string titulo,
        [Option("Opcion1", "Primera Opcion")] string opcion1,
        [Option("Opcion2", "Segunda Opcion")] string opcion2,
        [Option("Opcion3", "Tercera Opcion")] string opcion3 = null,
        [Option("Opcion4", "Cuarta Opcion")] string opcion4 = null,
        [Option("Mensaje", "Mensaje que quierasa colocar")] string mensaje = null,
        [Option("Color", "Color del embed")] ColorEnum color = ColorEnum.Azul,
        [Option("ImagenURL", "URL de imagen que se desea usar")] string imageURL = null){
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AsEphemeral(true));

            var interactiviy = ctx.Client.GetInteractivity();

            var builder = new DiscordEmbedBuilder();
            var descripcion = $"{DiscordEmoji.FromName(ctx.Client,":one:")}"
            + $" {opcion1}" + $"\n\n{DiscordEmoji.FromName(ctx.Client, ":two:")} {opcion2}"
            + $"{(opcion3 != null ? $"\n\n{DiscordEmoji.FromName(ctx.Client, ":three:")} {opcion3}" : "")}"  
            + $"{(opcion4 != null ? $"\n\n{DiscordEmoji.FromName(ctx.Client, ":four:")} {opcion4}" : "")}";

            builder.WithAuthor($"¡{ctx.User.Username} ha iniciado una encuesta!", iconUrl : ctx.User.AvatarUrl);
            builder.WithTitle(titulo);
            builder.WithDescription(descripcion);
            builder.WithColor(SelectColor(color));       
            if(imageURL != null) builder.WithImageUrl(imageURL);

            var message = await canal.SendMessageAsync(mensaje, builder);

            await message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client,":one:"));
            await message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client,":two:"));
            if(opcion3 != null)
                await message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client,":three:"));
            if(opcion4 != null)
                await message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client,":four:"));

            await ctx.EditResponseAsync( 
            new DiscordWebhookBuilder()
                .WithContent($"Se he enviado la poll a {canal.Mention}"));
        }
        #endregion

        

        #region SELECCIONAR COLOR
        private DiscordColor SelectColor(ColorEnum color){
            switch(color){
                case ColorEnum.Rojo:
                    return DiscordColor.Red;
                case ColorEnum.Verde:
                    return DiscordColor.Green;
                case ColorEnum.Naranja:
                    return DiscordColor.Orange;
                case ColorEnum.Purpura:
                    return DiscordColor.Purple;
                case ColorEnum.Violeta:
                    return DiscordColor.Violet;
                case ColorEnum.Oro:
                    return DiscordColor.Gold;
                default :
                    return DiscordColor.Blue;
            }
        }
        #endregion
    }

    public enum ColorEnum{
        [ChoiceName("Rojo")]
        Rojo,
        [ChoiceName("Azul")]
        Azul,
        [ChoiceName("Verde")]
        Verde,
        [ChoiceName("Naranja")]
        Naranja,
        [ChoiceName("Purpura")]
        Purpura,
        [ChoiceName("Violeta")]
        Violeta,
        [ChoiceName("Oro")]
        Oro
    }
}
