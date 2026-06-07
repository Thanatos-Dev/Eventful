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
            if (BuriedBarrageInvasion.isActive && (player.ZoneNormalCaverns || player.ZoneMarble || player.ZoneGranite || player.ZoneGemCave))
            {
                return true;
            }

            return false;
        }

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Music/BuriedBarrageMusic");
    }
}