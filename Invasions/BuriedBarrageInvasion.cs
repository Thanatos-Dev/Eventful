using Eventful.Enemies.BuriedBarrage;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Eventful.Utilities;

namespace Eventful.Invasions
{
    class BuriedBarrageInvasion : ModSystem
    {
        #region Variables
        public static bool isActive = false;
        public static int killCount = 0;
        public static int killsNeeded = 60;

        public static List<int> invasionEnemies = new List<int>()
        {
            ModContent.NPCType<MutantMosquito>(),
            ModContent.NPCType<MutantCentipedeHead>(),
            ModContent.NPCType<MutantMole>(),
            ModContent.NPCType<MutantBeetle>(),
            ModContent.NPCType<MutantRat>()
        };
        #endregion

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(killCount);
        }

        public override void NetReceive(BinaryReader reader)
        {
            killCount = reader.ReadInt32();
        }

        public override void PreUpdateWorld()
        {
            #region Complete Invasion
            if (killCount > killsNeeded - 1 && isActive)
            {
                string key = "The Buried Barrage has been defeated!";
                Color messageColor = new Color(175, 75, 255);
                Main.NewText(Language.GetTextValue(key), messageColor);

                isActive = false;
                killCount = 0;

                NPC.SetEventFlagCleared(ref DownedSystem.downedBuriedBarrage, -1);

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }
            #endregion
        }
    }

    public class BuriedBarrageSpawns : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (BuriedBarrageInvasion.isActive && (player.ZoneNormalCaverns || player.ZoneMarble || player.ZoneGranite || player.ZoneGemCave))
            {
                spawnRate = 75;
                maxSpawns = 100;
            }
        }
        
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (BuriedBarrageInvasion.isActive && (spawnInfo.Player.ZoneNormalCaverns || spawnInfo.Player.ZoneMarble || spawnInfo.Player.ZoneGranite || spawnInfo.Player.ZoneGemCave))
            {
                #region Spawn Pool
                //Make all spawn chances equal 100

                pool.Add(ModContent.NPCType<MutantMosquito>(), 23);
                pool.Add(ModContent.NPCType<MutantCentipedeHead>(), 8);
                pool.Add(ModContent.NPCType<MutantMole>(), 23);
                pool.Add(ModContent.NPCType<MutantBeetle>(), 23);
                pool.Add(ModContent.NPCType<MutantRat>(), 23);
                #endregion

                for (int type = 0; type < NPCLoader.NPCCount; type++)
                {
                    if (!BuriedBarrageInvasion.invasionEnemies.Contains(type))
                    {
                        pool.Remove(type); //Removes vanilla enemies from the spawn pool
                    }
                }
            }
        }
    }

    public class BuriedBarrageBiome : ModBiome
    {
        public override string BestiaryIcon => base.BestiaryIcon;
        public override SceneEffectPriority Priority => SceneEffectPriority.Event;
    }
}