using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Entities;
using Extension.DiscordMemberExtension;

using DanteBot.Attributes;
using DanteBot.Handlers;

namespace DanteBot{
    public class BananaCommandModule : BaseCommandModule{

        public MapacheGuild mapaService {private get; set;}

        [Command("banana?")]
        [MustBeBanana]
        public async Task EresBanana(CommandContext ctx){
            await ctx.RespondAsync(ctx.Member.IsBanana() ? "Si" : "No");
        }

        [Command("BecomeAsBanana")]
        [RequireRoles(RoleCheckMode.All, "Administrador")]
        [Cooldown(1, 40000f, CooldownBucketType.Global)]
        public async Task BecomeAsBanana(CommandContext ctx){
            await ctx.Message.DeleteAsync();
            var BananaRole = mapaService.BananaRole;
            //var BananaEMoji = DiscordEmoji.FromName(":banana:");
            var interactiviy = ctx.Client.GetInteractivity();
            var MessageEmbed = new DiscordEmbedBuilder{
                Title = "BECOME AS BANANA",
                Color = DiscordColor.Yellow,
                Description = "BECOME AS BANANA????",
                Footer = new DiscordEmbedBuilder.EmbedFooter{
                    Text = "Reacciona para obtener el rol"
                },
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail{
                    Url = "https://cdn.discordapp.com/attachments/840450708850868244/864722468165124136/platano.png"
                }
            };

            var SendedMessage = await ctx.Guild.GetChannel(840450708850868244).SendMessageAsync(MessageEmbed);
            var result = await interactiviy.WaitForReactionAsync(
                x =>{
                    var member = x.Guild.GetMemberAsync(x.User.Id).Result;
                    return (x.Message == SendedMessage && !member.Roles.Contains(BananaRole));
                }, timeoutoverride : TimeSpan.FromSeconds(2));
            if(result.Result == null){
                await SendedMessage.DeleteAsync();
                await ctx.RespondAsync("Nadie quizo");
                return;
            }
            await SendedMessage.DeleteAsync();
            var Miembro = await ctx.Guild.GetMemberAsync(result.Result.User.Id);
            await Miembro.GrantRoleAsync(BananaRole);

            var EmbedBanana = new DiscordEmbedBuilder{
                Title = "SHHHHHHHH",
                Description = $"Bienvenido {Miembro.Mention} a la sociedad secreta",
                Footer = new DiscordEmbedBuilder.EmbedFooter{
                    Text = "🍌"
                },
                Author = new DiscordEmbedBuilder.EmbedAuthor{
                    IconUrl = "https://cdn.discordapp.com/attachments/840450708850868244/864722468165124136/platano.png"
                },
                Color = DiscordColor.Yellow          
            };

            var lastMessage = await ctx.Guild.GetChannel(840450708850868244).SendMessageAsync(EmbedBanana);
            Thread.Sleep(3000);
            await lastMessage.DeleteAsync();
        }
    }
}