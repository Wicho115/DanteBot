using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using DSharpPlus;
using DSharpPlus.Entities;

namespace DanteBot.Entities{
    public class GameSession{
        private DiscordUser _host;
        private DiscordChannel _channelSesion;
        private int _maxPlayers;
        private string _name;
        private string _game;
        private string _description;
        public GameSession(
            DiscordUser host, 
            DiscordChannel channelSesion,
            int maxPlayers, 
            string name, 
            string game, 
            string description){
                _host = host;
                _channelSesion = channelSesion;
                _name = name;
                _maxPlayers = maxPlayers;
                _description = description;
                _game = game;
        }

        /*public async Task M(){
            while(_channelSesion.Users.Any()){

            }
        }*/
    }
}