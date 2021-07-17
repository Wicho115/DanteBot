using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using Extension.DiscordMemberExtension;

namespace DanteBot.Attributes{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequiredChannelAttribute : CheckBaseAttribute{
        public ulong ChannelId {get; private set;}
        public RequiredChannelAttribute(ulong ID){
            ChannelId = ID;
        }
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help){
           return Task.FromResult(ctx.Channel.Id == ChannelId);
        }
    }
}