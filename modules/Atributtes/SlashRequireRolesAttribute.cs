using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands.EventArgs;

namespace DanteBot.Attributes{
    public class SlashRequireRoleAttribute : SlashCheckBaseAttribute{
        public ulong roleId;

        public SlashRequireRoleAttribute(ulong roleID){
            this.roleId = roleID;
        }
        public override Task<bool> ExecuteChecksAsync(InteractionContext ctx)
        {
            var role = ctx.Member.Roles.Single(x => x.Id == roleId);
            return Task.FromResult(role.Id == roleId);
        }
    }
}