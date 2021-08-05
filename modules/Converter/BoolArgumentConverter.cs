using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Attributes;

namespace DanteBot.Converters{
    public class BoolArgumentConverter : IArgumentConverter<bool>{
        public Task<Optional<bool>> ConvertAsync(string value, CommandContext ctx){
            if(bool.TryParse(value, out var boolean)){
                return Task.FromResult(Optional.FromValue(boolean));
            }

            switch(value.ToLower()){
                case "si":
                case "s":
                case "yes":
                case "t":
                case "y":
                    return Task.FromResult(Optional.FromValue(true));
                case "no":
                case "n":
                case "f":
                    return Task.FromResult(Optional.FromValue(false));
                default:
                    return Task.FromResult(Optional.FromNoValue<bool>());
            }
        }
    }
}