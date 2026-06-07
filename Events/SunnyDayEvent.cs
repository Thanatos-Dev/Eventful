using Eventful.Buffs;
using Eventful.Enemies.BuriedBarrage;
using Eventful.Enemies.SunnyDay;
using Eventful.Invasions;
using Eventful.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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
        }

        public override void NetReceive(BinaryReader reader)
        {
            isActive = reader.ReadBoolean();
        }
        #endregion

        public override void PreUpdateWorld()
        {
            #region Enable/Disable Event
            if (!isActive && Main.dayTime && Main.time == 0 && !Main.raining)
            {
                if (Main.rand.NextBool(1, 5))
                {
                    isActive = true;

                    string key = "It's a sunny day!";
                    Color messageColor = new Color(175, 75, 255);
                    Main.NewText(Language.GetTextValue(key), messageColor);
                }
            }
            else if (!Main.dayTime)
            {
                isActive = false;
            }

            if (isActive && Main.raining)
            {
                isActive = false;

                string key = "The sun has hidden behind the clouds!";
                Color messageColor = new Color(175, 75, 255);
                Main.NewText(Language.GetTextValue(key), messageColor);
            }
            #endregion
            
            #region Heat Distortion
            if (Main.LocalPlayer.ZoneOverworldHeight && !Main.LocalPlayer.ZoneSnow && Main.UseHeatDistortion)
            {
                if (isActive)
                {
                    Filters.Scene["HeatDistortion"].GetShader().UseIntensity(2.5f);
                    Filters.Scene.Activate("HeatDistortion");
                }
                else
                {
                    Filters.Scene["HeatDistortion"].GetShader().UseIntensity(1);
                    Filters.Scene.Deactivate("HeatDistortion");
                }
            }
            #endregion

            #region Sun Texture
            if (isActive)
            {
                TextureAssets.Sun = TextureAssets.Sun2;
            }
            else
            {
                TextureAssets.Sun = Main.Assets.Request<Texture2D>("Images/Sun");
            }
            #endregion

            #region Sweaty Debuff
            if (Main.LocalPlayer.ZoneOverworldHeight && !Main.LocalPlayer.ZoneSnow)
            {
                if (isActive)
                {
                    Main.LocalPlayer.AddBuff(ModContent.BuffType<Sweaty>(), 10);
                }
                else
                {
                    Main.LocalPlayer.ClearBuff(ModContent.BuffType<Sweaty>());
                }
            }
            else
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

    public class SunnyDaySpawns : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (SunnyDayEvent.isActive && spawnInfo.Player.ZoneForest)
            {
                #region Spawn Pool
                pool.Add(ModContent.NPCType<LivingSunflower>(), 0.1f);
                pool.Add(ModContent.NPCType<Snake>(), 0.1f);
                pool.Add(ModContent.NPCType<AngrySun>(), 0.1f);
                #endregion
            }
        }
    }

    public class SunnyDayBiome : ModBiome
    {
        public override string BestiaryIcon => base.BestiaryIcon;
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
    }
}