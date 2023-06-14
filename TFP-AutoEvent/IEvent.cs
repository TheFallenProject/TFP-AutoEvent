using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFP_AutoEvent
{
    public static class EventDefaults
    {
        public delegate void EndedDelegate();
    }

    public interface IEvent
    {
        /// <summary>
        /// Name of the event for admins and internal code
        /// </summary>
        string CommandName { get; set; }

        /// <summary>
        /// Name of the event for players and admins. Expected to be text-mesh-pro'ed
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// Is this event designed to be low-player friendly? Used by the external plugins to determine if they should use this event.
        /// </summary>
        bool LowPlayerEvent { get; set; }

        /// <summary>
        /// If picked randomly, what are the chances of this event being picked? Increase this for events, that players like. Decrease if it is experimental or not very well-received. 0 Disables it entirely from the queue.
        /// </summary>
        int EventWeighting { get; set; }

        /// <summary>
        /// The event in 30 or less words total. Displays the rules before launching.
        /// </summary>
        string ShortRulesDescription { get; set; }

        /// <summary>
        /// Rules for event in the (~) console. Expect most players to not read it though, because they have an attention span of a fucking goldfish.
        /// </summary>
        string ExpandedRules { get; set; }

        /// <summary>
        /// Fired before showing the short rules and enabling expanded rules.
        /// </summary>
        /// <param name="reason">The reason because the event cannot start. Set it to "NA" to provide no reason (not recommended)</param>
        /// <returns>bool, True if OK to start, False otherwise</returns>
        bool LaunchCheck(out string reason);

        /// <summary>
        /// Fired when the event was picked. Use it to spawn the players/prepare the map. DO NOT do anything hint/broadcast related during PreLaunch sequence unless you know what you are doing!
        /// </summary>
        void PreLaunch();

        /// <summary>
        /// Fired when the rules have been shown for certain time AND LaunchCkeck returned true
        /// </summary>
        void Engage();

        /// <summary>
        /// Fired when the event is ended or disengaged. Feel free to use it yourself, if you want to end the event;
        /// </summary>
        void DisEngage();

        /// <summary>
        /// Fire this when event has ended to let the Plugin know that everything is over.
        /// Not firing it will result in pretty bad stuff btw.
        /// </summary>
        event EventDefaults.EndedDelegate Ended;
    }
}
