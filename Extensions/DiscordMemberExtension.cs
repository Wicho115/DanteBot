using System.Linq;
using System.Collections.Generic;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;

namespace Extension.DiscordMemberExtension{
    public static class Extensions{
        public static bool IsBanana(this DiscordUser usuario){
            return usuario.Id.ToString() == "804194414934491168";
        }

        public static bool IsAdmin(this DiscordUser usuario, CommandContext ctx){
            var AdminRole = ctx.Guild.GetRole(804595988471611402);
            var ModRole = ctx.Guild.GetRole(804593782673637416);
            var roles = ctx.Member.Roles;
            return roles.Contains(ModRole) || roles.Contains(AdminRole) ;
        }
    }
}