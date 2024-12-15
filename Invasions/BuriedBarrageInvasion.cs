using Eventful.Enemies.BuriedBarrage;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Eventful.Invasions
{
    class BuriedBarrageInvasion : ModSystem
    {
        #region Variables
        public static bool isActive = false;
        public static int killCount = 0;
        public static int killsNeeded = 10;

        public static List<int> invasionEnemies = new List<int>()
        {
            ModContent.NPCType<MutantMosquito>(),
            ModContent.NPCType<MutantCentipedeHead>(),
            ModContent.NPCType<MutantMole>(),
            ModContent.NPCType<MutantBeetle>(),
            ModContent.NPCType<MutantRat>()
        };
        #endregion

        #region World Data
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("InvasionActive", isActive);

            tag.Add("CurrentKillCount", killCount);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey("InvasionActive"))
            {
                isActive = tag.GetBool("InvasionActive");
            }

            if (tag.ContainsKey("CurrentKillCount"))
            {
                killCount = tag.GetInt("CurrentKillCount");
            }
        }
        #endregion

        #region Net
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(killCount);
        }

        public override void NetReceive(BinaryReader reader)
        {
            killCount = reader.ReadInt32();
        }
        #endregion

        public override void PreUpdateWorld()
        {
            if (killCount == killsNeeded)
            {
                isActive = false;

                #region Chat Message
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                string key = "The Buried Barrage has been defeated!";
                Color messageColor = new Color(175, 75, 255);
                if (Main.netMode == NetmodeID.Server) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == NetmodeID.SinglePlayer) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
                #endregion

                killCount = 0;
            }
        }

        public override void OnWorldLoad()
        {
            if (isActive == true)
            {
                #region Chat Message
                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                string key = "The Buried Barrage is invading the caverns!";
                Color messageColor = new Color(175, 75, 255);
                if (Main.netMode == NetmodeID.Server) // Server
                {
                    Terraria.Chat.ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key), messageColor);
                }
                else if (Main.netMode == NetmodeID.SinglePlayer) // Single Player
                {
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
                #endregion
            }
        }
    }

    public class BuriedBarrageSpawnRates : GlobalNPC
    {
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (BuriedBarrageInvasion.isActive == true && player.ZoneNormalCaverns == true)
            {
                spawnRate = 100;
                maxSpawns = 50;
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (BuriedBarrageInvasion.isActive == true && spawnInfo.Player.ZoneNormalCaverns == true)
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
}