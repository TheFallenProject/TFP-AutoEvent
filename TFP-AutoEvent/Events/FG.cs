using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFP_AutoEvent.Events
{
    public class Plugin : IEvent //moving to internal events to test the class assignment bug being fixed (spoiler alert: it didn't work =()
    {
        public string CommandName { get; set; } = "funnyguns";
        public string DisplayName { get; set; } = "Funny Guns";
        public bool LowPlayerEvent { get; set; } = false;
        public int EventWeighting { get; set; } = 10;
        public string ShortRulesDescription { get; set; } = "Уничтожьте вражескую команду, пока не уничтожили вас!";
        public string ExpandedRules { get; set; } = "Вы спавнитесь за хаосита или моговца. Ваша задача - уничтожить всю противоположную команду. Во время игры будут добавляться мутаторы, которые могут как и помогать вам," +
            " так и мешать. Также зоны будут закрываться одна за одной, так что не стойте на одном месте! Удачи, вам она понадобится.";

        public event EventDefaults.EndedDelegate Ended;

        // used to balance CI spawn in case of unfair NTF spawn
        // private static bool CISpawnedUnfairly;

        public void DisEngage()
        {
            Exiled.API.Features.Log.Info("disengaged");
            Ended.Invoke();
        }

        public void Engage()
        {
            DisEngage();
        }

        public bool LaunchCheck(out string reason)
        {
            reason = "NA";
            return true;
        }

        public void PreLaunch()
        {
            //This is currently scrapped, since for somewhat reason the game crashes upon changing player's class. However, every other system (EventManager.cs), that is in place, works perfectly.
            foreach (var pl in Exiled.API.Features.Player.List)
            {
                pl.Role.Set(PlayerRoles.RoleTypeId.ChaosRifleman, PlayerRoles.RoleSpawnFlags.All); //debug
            }
        }
    }
}
