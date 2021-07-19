using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.Net;

namespace DanteBot.Handlers{
    public class MapacheGuild{
        public DiscordGuild MapaGuild {get; private set;}

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
        #endregion
        public MapacheGuild(DiscordGuild mapaGuild){
            MapaGuild = mapaGuild;
        }        
    }
}