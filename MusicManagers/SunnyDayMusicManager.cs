using Eventful.Events;
using Eventful.Invasions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.MusicManagers
{
    public class SunnyDayMusicManager : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
        public override bool IsSceneEffectActive(Player player)
        {
            if (SunnyDayEvent.isActive && player.ZoneForest)
            {
                return true;
            }

            return false;
        }

        public override int Music => MusicLoader.GetMusicSlot(Mod, "Music/SunnyDayMusic");
    }
}