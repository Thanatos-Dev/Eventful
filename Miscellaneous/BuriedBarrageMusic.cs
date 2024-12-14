using Eventful.Invasions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Miscellaneous
{
    public class BuriedBarrageMusic : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override bool IsSceneEffectActive(Player player)
        {
            if (BuriedBarrageInvasion.isActive == true)
            {
                return true;
            }

            return false;
        }

        public override int Music => MusicID.GoblinInvasion;
    }
}