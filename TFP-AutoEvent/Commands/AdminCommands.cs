using CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFP_AutoEvent.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class AdminListLoaded : ICommand
    {
        public string Command { get; } = "ev_list";
        public string[] Aliases { get; } = null;
        public string Description { get; } = "Lists all loaded events in the Event Manager";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = $"There are {EventManager.loadedEvents.Count} loaded events";

            if (EventManager.loadedEvents.Count > 0)
            {
                response += "\nList:";
                foreach (var ev in EventManager.loadedEvents)
                {
                    response += $"\n{(ev.LowPlayerEvent ? "<color=yellow>[LPE]</color>" : "[NRM]")} {ev.DisplayName} <b>[{ev.CommandName}]</b>";
                }
            }
            return true;
        }
    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class AdminStart : ICommand
    {
        public string Command { get; } = "ev_start";
        public string[] Aliases { get; } = null;
        public string Description { get; } = "Starts an event";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 1)
            {
                response = "Syntax: ev_start [event_command_name]";
                return false;
            }

            IEvent eventPick;
            try
            {
                eventPick = EventManager.loadedEvents.First(ev => ev.CommandName == arguments.At(0));
            }
            catch 
            {
                response = "This event does not exist or isn't loaded.";
                return false;
            }

            EventManager.LaunchEvent(eventPick);
            response = "Success! We have forced the event to begin via Event Manager.";
            return true;
        }
    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class AdminEnd : ICommand
    {
        public string Command { get; } = "ev_stop";
        public string[] Aliases { get; } = null;
        public string Description { get; } = "Stops an event";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            EventManager.ForciblyStopEvent();
            response = "Success! We have forced the event to stop via Event Manager.";
            return true;
        }
    }

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class AdminReload : ICommand
    {
        public string Command { get; } = "ev_reload";
        public string[] Aliases { get; } = null;
        public string Description { get; } = "Reloads external events";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            EventManager.ReloadEvents();
            response = "Success! We have reload the events via Event Manager.";
            return true;
        }
    }
}
