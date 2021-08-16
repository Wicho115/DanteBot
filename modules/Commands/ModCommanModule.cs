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

[RequireRoles(RoleCheckMode.Any, "Moderador", "Administrador")]
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

    [Command("mute")]
    [RequireRoles(RoleCheckMode.Any, "Moderador", "Administrador")]
    public async Task Mute(CommandContext ctx, DiscordMember persona){
        await ctx.Message.DeleteAsync();
    }

    [Command("embed")]
    [RequireRoles(RoleCheckMode.Any, "Administrador", "Moderador")]
    public async Task EmbedBuild(CommandContext ctx, DiscordColor color, DiscordChannel canalDestino){
        await ctx.Message.DeleteAsync();
        var interactivity = ctx.Client.GetInteractivity();

        var embed = new DiscordEmbedBuilder();
        embed.Color = color;
        var messagetitle = await ctx.Channel.SendMessageAsync("Responda a continuacion con el titulo");
        var titulo = await interactivity.WaitForMessageAsync(x => x.Author == ctx.Member );
        await messagetitle.DeleteAsync();
        embed.Title = titulo.Result.Content;
        await titulo.Result.DeleteAsync();

        var messageContent = await ctx.Channel.SendMessageAsync("Responda a continuacion con el contenido");
        var contenido = await interactivity.WaitForMessageAsync(x => x.Author == ctx.Member);
        await messageContent.DeleteAsync();
        embed.Description = contenido.Result.Content;
        await contenido.Result.DeleteAsync();

        embed.Footer = new DiscordEmbedBuilder.EmbedFooter{
            Text = $"Autor: {ctx.User.Username}",
            IconUrl = ctx.User.AvatarUrl
        };
        await canalDestino.SendMessageAsync(embed);
    }

   /*  [Command("embed")]
    [RequireRoles(RoleCheckMode.Any, "Administrador", "Moderador")]
    public async Task EmbedBuild(CommandContext ctx, DiscordColor color, DiscordChannel canal){
        
    } */
}