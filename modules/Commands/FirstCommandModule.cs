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

namespace DanteBot{
    public class FirstCommandModule : BaseCommandModule{
        public bool stoped = false;

        private Dictionary<string, string> EmotesMessages = new Dictionary<string, string>{
            {":Pichuter_BlowJob:", "El pichuter es todo un loco "},
            {":Axoli_Mood:", "Eternal mood de axoli"},
            {":perro_asco:", "A pagliacci no le ha gustado eso"}
        };

        public Random rng {private get; set;}

        [Command("Hola")]
        [Description("Le dices hola a lo que digas :D")]
        [Cooldown(1,1f, CooldownBucketType.Global)]
        public async Task HolaCommand(
        CommandContext ctx, 
        [Description("Nombre de lo que quieras saludar")]
        [RemainingText] string name){            
            await ctx.RespondAsync($"{ctx.User.Mention} acaba de saludar A: {name}");
        }

        [Command("Random")]
        [Description("Elige un número aleatorio entre 2 números")]
        public async Task RandomCommand(CommandContext ctx, 
        [Description("Número mínimo del número aleatorio")] int min, 
        [Description("Número máximo del número aleatorio")] int max){            
            await ctx.RespondAsync($"Tu numero aleatorio es: {rng.Next(min, max+1)}");
        }

        [Command("interact")]
        public async Task Response(CommandContext ctx){
            var interactivity = ctx.Client.GetInteractivity();            

            await ctx.Channel.SendMessageAsync("Responde con algo y te mandaré lo mismo");
            var message = await interactivity.WaitForMessageAsync(
                x => x.Channel == ctx.Channel && x.Author == ctx.User, TimeSpan.FromSeconds(2)
            );

            if(message.Result == null){
                await ctx.Channel.SendMessageAsync("No respondiste con nada");
            }else{
                await ctx.Channel.SendMessageAsync(message.Result.Content);
            }
        }

        [Command("interactReact")]
        public async Task ResponseReact(CommandContext ctx){
            var interactivity = ctx.Client.GetInteractivity();

            var sendmessage = await ctx.Channel.SendMessageAsync("Reacciona a este mensaje y te responderé con la reacción");
            var react = await interactivity.WaitForReactionAsync(
                x => x.Message == sendmessage && x.User == ctx.User
            );

            var emoji = react.Result.Emoji;

            Console.WriteLine(emoji.Id.ToString());
            
            if(EmotesMessages.ContainsKey(emoji.GetDiscordName())){
                await ctx.Channel.SendMessageAsync($"{EmotesMessages[emoji.GetDiscordName()]} {emoji}");
            }                
            else{
                await ctx.Channel.SendMessageAsync(emoji);
            }
        }

        [Command("Poll")]
        [RequireRoles(RoleCheckMode.Any, "Administrador")]
        [Description("Crea una encuesta a partir de emojis")]
        public async Task Poll(CommandContext context, TimeSpan duration, DiscordChannel canal, params DiscordEmoji[] emojiOptions){
            var interactivity = context.Client.GetInteractivity();
            var options = emojiOptions.Select(x => x.ToString());

            var pollEmbed = new DiscordEmbedBuilder{
                Title = "Nueva Encuesta!",
                Description = string.Join(" ", options),
                Color = DiscordColor.Azure
            };

            var pollMessage = await canal.SendMessageAsync(pollEmbed);

            var pollResult = await interactivity.DoPollAsync(pollMessage, emojiOptions, timeout : duration);
            var reactions = pollResult.Where(x => x.Total > 0).Select(x => $"{x.Emoji}: {x.Total}");                  
            await context.Channel.SendMessageAsync(string.Join("\n", reactions));
        }

        [Command("Timer")]
        public async Task Timer(CommandContext ctx, double miliSeconds){
            Timer timer = new Timer(miliSeconds);
            int i = 0;
            timer.Elapsed += async (sender, e) => {
                if(stoped){
                    timer.Stop();
                    timer.Dispose();
                    stoped = false;
                }else{
                    await ctx.RespondAsync($"Es la vez numero {++i} que se usa el timer, SignalTime :{e.SignalTime}");
                }
            };
            await ctx.RespondAsync("Inicia el timer");
            timer.Start();
        }

        [Command("Stop")]
        public async Task Stop(CommandContext context){
            stoped = true;
            await context.RespondAsync("Se paró el Timer");
        }
    }
}