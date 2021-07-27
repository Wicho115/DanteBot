using System.Threading.Tasks;
using System.Timers;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;

namespace Extension.DiscordMessageExtension{
    public static class Extensions{
        public static Task DeleteAsync(this DiscordMessage mensaje, double miliseconds){
            Timer timer = new Timer(miliseconds);
            timer.AutoReset = false;
            timer.Elapsed += async (sender, e) =>{
                await mensaje.DeleteAsync();
            };
            timer.Start();
            return Task.CompletedTask;
        }
    }
}