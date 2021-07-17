using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Entities;


namespace DanteBot.CustomMessages{
    public class GameSessionMessages{
        public DiscordEmbedBuilder CreateSessionMessage(
            DiscordUser host, 
            string juego, 
            string description,
            int numjugadores){
            return new DiscordEmbedBuilder{
                Title = $"{host.Username} ha creado una sesion para: {juego}!",
                Description = $"{host.Username} quiere jugar {juego} con {numjugadores} más\n\n '{description}'",
                Color = DiscordColor.Rose,
                Author = new DiscordEmbedBuilder.EmbedAuthor{
                    IconUrl = host.AvatarUrl
                },
                Footer = new DiscordEmbedBuilder.EmbedFooter{
                    IconUrl = "https://cdn.discordapp.com/attachments/858779821002063886/865789293360316466/madoka_01.png",
                    Text = host.Username
                }
            };
        }
    }
}