using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;

namespace Extension.DiscordEmojiExtension{
    public static class Extensions{
        public static DiscordEmoji Tijeras(this DiscordEmoji s, CommandContext ctx){
            return DiscordEmoji.FromName(ctx.Client, ":scissors:");
        }

        public static DiscordEmoji Papel(this DiscordEmoji s, CommandContext ctx){
            return DiscordEmoji.FromName(ctx.Client, ":page_facing_up:");
        }

        public static DiscordEmoji Piedra(this DiscordEmoji s, CommandContext ctx){
            return DiscordEmoji.FromName(ctx.Client, ":gem:");
        }
    }
}