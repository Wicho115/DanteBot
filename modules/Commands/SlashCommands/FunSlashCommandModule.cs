using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using DSharpPlus;
using DSharpPlus.Entities;

using DanteBot.Handlers;

namespace DanteBot{
    public class FunSlashCommand : ApplicationCommandModule{
        public MapacheGuild mapaService {private get; set;}

        [SlashCommand("Confesar", "confiesa tus pecados")]
        public async Task ConfesarCOmmand(InteractionContext ctx,
        [Option("Confesion", "Confesion la cual se publicará")] string confesion, 
        [Option("Anonimo", "¿Quieres que sea anonima? La confesion es anonima por default")] bool anonimo = true,
        [Option("Color", "Color de la confesion")] ColorEnum color = ColorEnum.Oro){
            await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource,
            new DiscordInteractionResponseBuilder().AsEphemeral(true));

            var confesionBuilder = new DiscordEmbedBuilder();
            confesionBuilder.WithColor(SelectColor(color));
            confesionBuilder.WithDescription(confesion);

            if(anonimo){
                confesionBuilder.WithTitle("Nueva Confesión!");
                confesionBuilder.WithFooter("Confesión anónima", ctx.Client.CurrentUser.AvatarUrl);
            }else{
                confesionBuilder.WithTitle($"Nueva Confesión De: {ctx.User.Username}");
                confesionBuilder.WithFooter(ctx.User.Username, ctx.User.AvatarUrl);
            }
            
            await mapaService.ConfesionChannel.SendMessageAsync(confesionBuilder);

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Se ha enviado tu confesion al canal de {mapaService.ConfesionChannel.Mention}"));
        }

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
    }
}