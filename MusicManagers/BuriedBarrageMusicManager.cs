using Eventful.Invasions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.MusicManagers
{
    public class BuriedBarrageMusicManager : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override bool IsSceneEffectActive(Player player)
        {
            if (BuriedBarrageInvasion.isActive == true && player.ZoneNormalCaverns)
            {
                return true;
            }

            return false;
        }

        public override int Music => MusicID.GoblinInvasion;
    }
}