using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.RGB;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System;

namespace Eventful.Events
{
    class SunnyDayEvent : ModSystem
    {
        public static bool isActive = false;

        #region World Data
        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("EventActive", isActive);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey("EventActive"))
            {
                isActive = tag.GetBool("EventActive");
            }
        }
        #endregion

        public override void PreUpdateWorld()
        {
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

            #region Heat Distortion
            new Filter(new ScreenShaderData("HeatDistortion").UseImage("Images/Misc/Perlin"), EffectPriority.VeryHigh);

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
        }
    }
}