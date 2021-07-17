using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using Extension.DiscordMemberExtension;
using System;


namespace DanteBot.Attributes{
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class MustBeBananaAttribute : CheckBaseAttribute{
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help){
            return Task.FromResult(ctx.Member.IsBanana() || ctx.Member.IsAdmin(ctx));
        }
    }
}
