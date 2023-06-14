using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFP_AutoEvent.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    internal class PlayerShowRules : ICommand
    {
        public string Command { get; } = "правила";
        public string[] Aliases { get; } = new string[] { "rules" };
        public string Description { get; } = "Показывает правила для активного ивента";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = EventManager.LongRules;
            return true;
        }
    }
}
