using Eventful.NPCs;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Utilities
{
    public class VanillaNPCHappiness : GlobalNPC
    {
        public override void SetStaticDefaults()
        {
            #region Vanilla NPCs
            var mechanicHappiness = NPCHappiness.Get(NPCID.Mechanic);
            var dryadHappiness = NPCHappiness.Get(NPCID.Dryad);
            var cyborgHappiness = NPCHappiness.Get(NPCID.Cyborg);
            #endregion

            #region Weatherman
            int weathermanType = ModContent.NPCType<Weatherman>();

            mechanicHappiness.SetNPCAffection(weathermanType, AffectionLevel.Love);
            dryadHappiness.SetNPCAffection(weathermanType, AffectionLevel.Like);
            cyborgHappiness.SetNPCAffection(weathermanType, AffectionLevel.Dislike);
            #endregion
        }
    }
}