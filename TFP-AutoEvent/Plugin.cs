using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFP_AutoEvent
{
    public class Plugin : Exiled.API.Features.Plugin<Config>
    {
        public override string Author => "Treeshold";

        public override string Name => "TFP-AutoEvents";

        public static void ExperimentalRoleChangeFix(Exiled.API.Features.Player pl, RoleTypeId newRole)
        {
            pl.Role.Set(newRole);
        }

        public override void OnEnabled()
        {
            EventManager.Init();
        }

        public override void OnDisabled()
        {
            EventManager.DeInit();
        }

        public override void OnReloaded()
        {
            EventManager.ReloadEvents();
        }
    }
}
