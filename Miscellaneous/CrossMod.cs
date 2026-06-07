using Eventful.Enemies.BuriedBarrage;
using Eventful.Invasions;
using Eventful.Items.Accessories;
using Eventful.Items.Miscellaneous;
using Eventful.Items.MusicBoxes;
using Eventful.Items.Summons;
using Eventful.Items.Tools;
using Eventful.Utilities;
using Eventful.Vanity;
using Eventful.Weapons;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Eventful.Miscellaneous
{
    public partial class CrossMod : ModSystem
    {
        public static ModEntry Wikithis { get; } = new("Wikithis");
        public static ModEntry BossChecklist { get; } = new("BossChecklist");
        public static ModEntry MusicDisplay { get; } = new("MusicDisplay");

        /// <summary> The names and instances of loaded crossmod mods per <see cref="ModEntry"/>. </summary>
        public static Dictionary<string, Mod> LoadedMods { get; } = [];

        public override void PostSetupContent()
        {
            Call_Wikithis();
            Call_BossChecklist();
            Call_MusicDisplay();
        }

        private void Call_Wikithis()
        {
            if (Wikithis.Enabled && !Main.dedServ)
            {
                Wikithis.Instance.Call("AddModURL", Mod, "https://terrariamods.wiki.gg/wiki/Eventful/{}");
            }
        }

        private void Call_BossChecklist()
        {
            #region Vanilla value reference
            /*
            KingSlime = 1f;
            EyeOfCthulhu = 2f;
            EaterOfWorlds = 3f;  
            BrainOfCthulhu = 3f;  
            QueenBee = 4f;
            Skeletron = 5f;
            DeerClops = 6f;
            WallOfFlesh = 7f;
            QueenSlime = 8f;
            TheTwins = 9f;
            TheDestroyer = 10f;
            SkeletronPrime = 11f;
            Plantera = 12f;
            Golem = 13f;
            Betsy = 14f;
            EmpressOfLight = 15f;
            DukeFishron = 16f;
            LunaticCultist = 17f;
            Moonlord = 18f;
            */
            #endregion

            List<int> BuriedBarrageEnemies = new List<int>()
            {
                ModContent.NPCType<MutantMosquito>(),
                ModContent.NPCType<MutantCentipedeHead>(),
                ModContent.NPCType<MutantMole>(),
                ModContent.NPCType<MutantBeetle>(),
                ModContent.NPCType<MutantRat>()
            };

            if (BossChecklist.Enabled)
            {
                #region Buried Barrage
                BossChecklist.Instance.Call("LogEvent", Mod, nameof(BuriedBarrageInvasion), 0.5f, () => DownedSystem.downedBuriedBarrage, BuriedBarrageEnemies, new Dictionary<string, object>()
                {
                    ["spawnItems"] = ModContent.ItemType<MutatedHeart>(),
                    ["collectibles"] = new List<int>
                    {
                        ModContent.ItemType<BuriedBarrageMusicBox>(),
                        ModContent.ItemType<MutatedFlesh>(),
                        ModContent.ItemType<MoleDiggingClaw>(),
                        ModContent.ItemType<MosquitoSack>(),
                        ModContent.ItemType<BeetleStaff>(),
                        ModContent.ItemType<CentipedeSnapper>(),
                        ModContent.ItemType<RatStaff>(),
                        ModContent.ItemType<MouseEars>()
                    },
                    ["overrideHeadTextures"] = "Eventful/Invasions/BuriedBarrageBiome_Icon",
                    ["spawnInfo"] = "Summoned with a Mutated Heart in the caverns. Mutated Hearts can be found in gold chests underground.",
                    ["customPortrait"] = (SpriteBatch spriteBatch, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("Eventful/Enemies/BuriedBarrage/BuriedBarrageBossChecklist").Value;
                        Vector2 centered = new(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        spriteBatch.Draw(texture, centered, color);
                    }
                });
                #endregion
            }
        }

        private void Call_MusicDisplay()
        {
            if (MusicDisplay.Enabled)
            {
                void AddMusic(string file, string name, string author)
                {
                    string credit = string.IsNullOrEmpty(author) ? "" : "by " + author;
                    MusicDisplay.Instance.Call("AddMusic", (short)MusicLoader.GetMusicSlot(Mod, "Music/" + file), name, credit, nameof(Eventful));
                }

                AddMusic("BuriedBarrageMusic", "\"Mutation Rising\" - Theme of the Buried Barrage", "Moonburn");
                AddMusic("SunnyDayMusic", "\"Daybreak\" - Theme of the Sunny Day", "Doc");
            }
        }
    }
}