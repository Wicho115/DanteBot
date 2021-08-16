using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands.EventArgs;
using DSharpPlus.CommandsNext.Builders;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.CommandsNext.Attributes;

using DanteBot.Attributes;

public class FirstSlash : ApplicationCommandModule{
    public override Task<bool> BeforeSlashExecutionAsync(InteractionContext ctx){
        return Task.FromResult(true);
    }

    [SlashCommand("test", "Un comando slash 2")]
    [SlashRequireOwner]
    public async Task TestCommand(InteractionContext ctx){
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        var builder = new DiscordWebhookBuilder();

        var Mapache = new DiscordComponentEmoji(DiscordEmoji.FromName(ctx.Client, ":raccoon:"));

        
        var btn1 = new DiscordButtonComponent(ButtonStyle.Success, "button 1", "Machape", emoji : Mapache);
        List<DiscordComponent> componentes = new List<DiscordComponent>(){
            btn1
        };
        var actionRow = new DiscordActionRowComponent(componentes);
        
        builder.WithContent("Ejemplo de mensaje").AddComponents(btn1);

        await ctx.EditResponseAsync(builder);
    }

    [SlashCommand("Saludo", "Un comando para mandar un saludo")]
    [SlashRequireRole(804593782673637416)]
    public async Task SaludarCommand(InteractionContext ctx,
        [Option("User", "El usuario al que desea saludar")] DiscordUser usuario){
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        
        await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Saludaste a {usuario.Mention}"));
    }

    [ContextMenu(ApplicationCommandType.UserContextMenu, "Abrazar")]
    public async Task AbrazarMenu(ContextMenuContext ctx){
        await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

        await ctx.EditResponseAsync(new DiscordWebhookBuilder()
            .WithContent($"{ctx.User.Mention} ha mandado un abrazo a {ctx.TargetUser.Mention}\nhttps://tenor.com/view/abrazo-un-te-mando-renatosx-gif-15711063")
            .WithTTS(true));
    }
}