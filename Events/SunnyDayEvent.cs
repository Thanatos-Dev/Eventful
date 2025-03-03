using Eventful.Buffs;
using Eventful.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Eventful.Events
{
    class SunnyDayEvent : ModSystem
    {
        public static bool isActive = false;

        #region World Data
        public override void ClearWorld()
        {
            isActive = false;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            isActive = tag.GetBool("EventActive");
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag["EventActive"] = isActive;
        }
        #endregion

        #region Net
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(isActive);

            NetMessage.SendData(MessageID.WorldData);
        }

        public override void NetReceive(BinaryReader reader)
        {
            isActive = reader.ReadBoolean();
        }
        #endregion

        public override void PreUpdateWorld()
        {
            #region Random Spawning
            if (isActive == false && Main.dayTime == true && Main.time == 0)
            {
                isActive = Main.rand.NextBool(1, 10);

                if (isActive == true)
                {
                    #region Chat Message
                    if (Main.netMode == NetmodeID.Server)
                        NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
                    string key = "It's a sunny day!";
                    Color messageColor = new Color(50, 255, 130);
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
            else if (Main.dayTime == false)
            {
                isActive = false;
            }
            #endregion
            
            #region Heat Distortion
            if (Main.netMode != NetmodeID.Server && Main.LocalPlayer.ZoneOverworldHeight == true)
            {
                if (isActive == true)
                {
                    Filters.Scene["HeatDistortion"].GetShader().UseIntensity(2);
                    Filters.Scene.Activate("HeatDistortion");
                }
                else if (isActive == false)
                {
                    Filters.Scene["HeatDistortion"].GetShader().UseIntensity(1);
                    Filters.Scene.Deactivate("HeatDistortion");
                }
            }
            #endregion

            #region Sun Texture
            if (isActive == true)
            {
                TextureAssets.Sun = TextureAssets.Sun2;
            }
            else if (isActive == false)
            {
                TextureAssets.Sun = Main.Assets.Request<Texture2D>("Images/Sun");
            }
            #endregion

            #region Sweaty Debuff
            if (isActive == true)
            {
                Main.LocalPlayer.AddBuff(ModContent.BuffType<Sweaty>(), 10);
            }
            else if (isActive == false)
            {
                Main.LocalPlayer.ClearBuff(ModContent.BuffType<Sweaty>());
            }
            #endregion
        }

        public override void Unload()
        {
            TextureAssets.Sun = Main.Assets.Request<Texture2D>("Images/Sun");
        }
    }
}