using System.Collections.Generic;
using System.Linq;
using DanteBot.Entities;

namespace DanteBot.Handlers{
    public class GameSessionHandler{
        public List<GameSession> sesionesActivas;

        public void StartSesion(GameSession sesion){
            sesionesActivas.Add(sesion);
        }
        
        public void EndSession(GameSession sesion){
            sesionesActivas.Remove(sesion);
        }
    }
}
