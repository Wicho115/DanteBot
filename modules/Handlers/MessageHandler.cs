using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.CommandsNext;

using Extension.DiscordMessageExtension;
//using DSharpPlus;
//using DSharpPlus;

namespace DanteBot.Handlers{
    public class MessageHandler{
        public static async Task<bool> AwaitResponse(CommandContext ctx, DiscordEmbed embed, DiscordChannel canal = null){
            var interactivity = ctx.Client.GetInteractivity();
            if(canal == null){
                canal = ctx.Channel;
            }

            var message = await canal.SendMessageAsync(embed);

            await message.CreateReactionAsync(MapacheGuild.YesEmoji(ctx));
            await message.CreateReactionAsync(MapacheGuild.NoEmoji(ctx));

            var result = await interactivity.WaitForReactionAsync(m => 
                (m.Emoji == MapacheGuild.YesEmoji(ctx) || m.Emoji == MapacheGuild.NoEmoji(ctx))
                && m.Message == message && m.User == ctx.User);
            
            var emoji = result.Result.Emoji;

            await message.DeleteAsync(500);

            return (emoji == MapacheGuild.YesEmoji(ctx));
        }
    }
}