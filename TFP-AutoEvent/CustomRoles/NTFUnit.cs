using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFP_AutoEvent.CustomRoles
{
    internal class NTFUnit : Exiled.CustomRoles.API.Features.CustomRole
    {
        public override uint Id { get; set; } = 0;
        public override int MaxHealth { get; set; } = 120;
        public override string Name { get; set; } = "MTF Operative";
        public override string Description { get; set; } = null;
        public override string CustomInfo { get; set; } = null;

        public override List<string> Inventory { get => base.Inventory; set => base.Inventory = value; }
    }
}
