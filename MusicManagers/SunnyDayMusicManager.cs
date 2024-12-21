using Eventful.Events;
using Eventful.Invasions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.MusicManagers
{
    public class SunnyDayMusicManager : ModSceneEffect
    {
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
        public override bool IsSceneEffectActive(Player player)
        {
            if (SunnyDayEvent.isActive == true && player.ZoneOverworldHeight == true)
            {
                return true;
            }

            return false;
        }

        public override int Music => MusicID.ConsoleMenu;
    }
}