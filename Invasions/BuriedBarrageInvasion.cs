using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.UI;
using Eventful.Enemies.BuriedBarrage;

namespace Eventful.Invasions
{
    class BuriedBarrageInvasion : ModSystem
    {
        #region Variables
        public static bool isActive = false;
        public static int killCount = 0;
        #endregion

        public override void OnWorldLoad()
        {
            isActive = false;
            killCount = 0;
        }

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
            if (killCount == 10)
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
                //Make all spawn chances equal 100

                pool.Add(ModContent.NPCType<MutantMosquito>(), 22);

                pool.Add(ModContent.NPCType<MutantCentipedeHead>(), 12);

                pool.Add(ModContent.NPCType<MutantMole>(), 22);

                pool.Add(ModContent.NPCType<MutantBeetle>(), 22);

                pool.Add(ModContent.NPCType<MutantRat>(), 22);
            }
        }
    }
}