using System;
using System.Threading.Tasks;
using System.Timers;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.Net;
using DSharpPlus.CommandsNext;

namespace DanteBot.Handlers{
    public class MapacheGuild{
        public DiscordGuild MapaGuild {get; private set;}
        //public DiscordEm

        #region CANALES FAMILIA MAPACHE
        public DiscordChannel GameCategoryChannel {get{
                return MapaGuild.GetChannel(865085611145035856);
            }}
        public DiscordChannel DanteBotChannel {get{
                return MapaGuild.GetChannel(840450708850868244);
            }}
        public DiscordChannel SesionChannel {get{
                return MapaGuild.GetChannel(865295974585860097);
            }}
        public DiscordChannel ConfesionChannel {get{
                return MapaGuild.GetChannel(858902825366192128);
            }}
        public DiscordChannel GeneralChannel {get{
                return MapaGuild.GetChannel(804581949615505440);
            }}
        #endregion

        #region ROLES FAMILIA MAPACHE
        public DiscordRole JuegosRole {get{
                return MapaGuild.GetRole(847980372239319052);
            }}
        public DiscordRole AvisosRole{get{
            return MapaGuild.GetRole(810749372203139132);
        }}
        public DiscordRole AdminRole {get{
                return MapaGuild.GetRole(804595988471611402);
            }}
        public DiscordRole ModRole {get{
                return MapaGuild.GetRole(804593782673637416);
            }}
        public DiscordRole BananaRole{get{
                return MapaGuild.GetRole(846600497737891872);
            }}
        public DiscordRole MutedRole{get{
                return MapaGuild.GetRole(858933298196512789);
            }}
        public DiscordRole IntegranteRole{get{
                return MapaGuild.GetRole(804593854959058984);
            }}
        #endregion
        
        #region EMOJIS
            public static DiscordEmoji YesEmoji(CommandContext ctx){
                return DiscordEmoji.FromName(ctx.Client, ":white_check_mark:");
            }

            public static DiscordEmoji NoEmoji(CommandContext ctx){
                return DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:");
            }
        #endregion
        public MapacheGuild(DiscordGuild mapaGuild){
            MapaGuild = mapaGuild;
        }      

        public async Task Mutear(DiscordMember miembro, double miliseconds, Action callback = null){
            if(!miembro.IsBot) await miembro.RevokeRoleAsync(IntegranteRole);
            await miembro.GrantRoleAsync(MutedRole);
            
            Timer timer = new Timer(miliseconds);
            timer.AutoReset = false;
            timer.Elapsed += async (sender, e) =>{      
                    if(!miembro.IsBot) await miembro.GrantRoleAsync(IntegranteRole);
                    await miembro.RevokeRoleAsync(MutedRole);
                    callback?.Invoke();            
            };

            timer.Start();
        }
    }
}