using System;
using System.Timers;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Entities;

using DanteBot.Attributes;
using DanteBot.Handlers;
using Extension.DiscordMessageExtension;
public class ModCommandModule : BaseCommandModule{
    public MapacheGuild mapaService {private get; set;}

    [Command("mute")]
    [RequireRoles(RoleCheckMode.Any, "Moderador", "Administrador")]
    public async Task Mute(CommandContext ctx, DiscordMember persona, double tiempo){
        if(persona.Roles.Contains(mapaService.MutedRole)){
            await ctx.RespondAsync("Esta persona ya tiene este rol");
            return;
        }
        if(tiempo < 5){
            await ctx.RespondAsync("No se puede colocar un muted de menos de 5 segundos");
            return;
        }
        await ctx.Message.DeleteAsync();
        var message = await ctx.RespondAsync($"Se ha silenciado a {persona.Username} por {tiempo} segundos");
        await message.DeleteAsync(3000);
        await mapaService.Mutear(persona, tiempo * 1000, async () => {
            var message = await ctx.RespondAsync($"Se ha desilenciado a {persona.Username}");
            await message.DeleteAsync(3000);
        });
    }    
}