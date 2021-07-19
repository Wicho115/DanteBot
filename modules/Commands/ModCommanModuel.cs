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
public class ModCommandModule : BaseCommandModule{
    public MapacheGuild mapaService {private get; set;}

    [Command("mute")]
    [RequireRoles(RoleCheckMode.Any, "Administrador")]
    public async Task Mute(CommandContext ctx, DiscordMember persona, double tiempo){
        if(persona.Roles.Contains(mapaService.MutedRole)){
            await ctx.RespondAsync("Esta persona ya tiene este rol");
            return;
        }
        if(tiempo < 5){
            await ctx.RespondAsync("No se puede colocar un muted de menos de 5 segundos");
            return;
        }

        await persona.GrantRoleAsync(mapaService.MutedRole);
        await ctx.RespondAsync($"Se ha silenciado a {persona.Username} por {tiempo} segundos");

        Timer timer = new Timer(tiempo * 1000);        
        timer.AutoReset = false;
        timer.Elapsed += async (_ , e) =>{
            await persona.RevokeRoleAsync(mapaService.MutedRole);
        };
        timer.Start();
    }    
}