using Eventful.Events;
using Eventful.Items.WeatherToggles;
using Eventful.Miscellaneous;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Eventful.NPCs
{
    [AutoloadHead]
    public class Weatherman : ModNPC
    {
        public const string ShopName = "Shop";

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
            NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 0; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
            NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
            NPCID.Sets.AttackAverageChance[Type] = 30; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
            NPCID.Sets.HatOffsetY[Type] = 0; // For when a party is active, the party hat spawns at a Y offset.
            NPCID.Sets.ShimmerTownTransform[NPC.type] = false; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

            NPCID.Sets.ShimmerTownTransform[Type] = false; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            // Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
            // NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
            NPC.Happiness
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Like)
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate)
                .SetNPCAffection(NPCID.Mechanic, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Like)
                .SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Dislike)
            ; // < Mind the semicolon!
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true; // Sets NPC to be a Town NPC
            NPC.friendly = true; // NPC Will not attack player
            AnimationType = NPCID.Guide;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.knockBackResist = 0.5f;
            NPC.lifeMax = 250;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("The Weatherman used to be just a typical meteorologist and news anchor, but after working with the mechanic, he's learned to control the weather itself!"),
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            int num = NPC.life > 0 ? 1 : 5;

            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                // Spawn the gores. The positions of the arms and legs are lowered for a more natural look.
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("WeathermanGoreHead").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, Mod.Find<ModGore>("WeathermanGoreArm").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, Mod.Find<ModGore>("WeathermanGoreArm").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, Mod.Find<ModGore>("WeathermanGoreLeg").Type);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, Mod.Find<ModGore>("WeathermanGoreLeg").Type);
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            if (NPC.downedBoss1 || NPC.downedSlimeKing)
            {
                return true;
            }

            return false;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Ferrel",
                "Davis",
                "Wegener",
                "Dalton",
                "Abbe",
                "Shaw",
                "Stewart",
                "Celsius",
                "Wilson",
                "Emden",
                "Charney",
                "Rossby",
                "Espy",
                "Dines",
                "Symons"
            };
        }

        public override void SetChatButtons(ref string button, ref string button2)
        { // What the chat buttons are when you open up the chat UI
            button = Language.GetTextValue("LegacyInterface.28");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = ShopName; // Name of the shop tab we want to open.
            }
        }

        #region Chat
        public override string GetChat()
        {
            WeightedRandom<string> chat = new();

            // Normal
            chat.Add("I'm loving the weather today! Well, I do most days.");
            chat.Add("The mechanic was a big help when making my forecast devices. I couldn't have done it without her!");
            chat.Add("Have you seen me on the news? I got to keep the microphone!");

            // During rain
            if (Main.raining)
            {
                chat.Add("Make sure to grab an umbrella on your way out.");
                chat.Add("Oh crap, where's my umbrella? I thought I left it right here!");
            }

            // During a windy day
            if (Main.IsItAHappyWindyDay)
            {
                chat.Add("Today would be a great day to fly my kite!");
                chat.Add("Watch out for slimes today, I saw one being carried by a balloon!");
            }

            // During a thunderstorm
            if (Main.IsItStorming)
            {
                chat.Add("Ah, I love the sounds of a thunderstorm.");
                chat.Add("Get in here! It's storming hard!");
            }

            // During a sunny day
            if (SunnyDayEvent.isActive)
            {
                chat.Add("It's so sunny today! Don't forget to use sunscreen.");
                chat.Add("I wouldn't wanna be a vampire today!");
            }

            // During a slime rain
            if (Main.slimeRain)
            {
                chat.Add("Never seen weather like this before.");
                chat.Add("How does this weather even make sense?");
            }

            // When underground
            if (Main.LocalPlayer.ZoneNormalUnderground)
            {
                chat.Add("I can't experience the weather down here!");
                chat.Add("Is there another house for me on the surface? I don't wanna miss out on the weather.");
            }

            // When in the underworld
            if (Main.LocalPlayer.ZoneUnderworldHeight)
            {
                chat.Add("Why am I all the way down here? Do you just hate me or something?");
                chat.Add("The temperatures down here are crazy!");
            }

            return chat;
        }
        #endregion

        #region Shop
        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName)
                .Add<RainForecast>()
                .Add<WindForecast>()
                .Add<ThunderstormForecast>()
                .Add<SunnyForecast>()
                .Add<ClearSkiesForecast>()
                .Add<CalmWindsForecast>()
                .Add<CloudyForecast>()
                .Add(ItemID.Umbrella, Condition.InRain)
                .Add(ItemID.Sunglasses, CustomConditions.IsSunnyDay)
                .Add(ItemID.EskimoHood, Condition.InSnow)
                .Add(ItemID.EskimoCoat, Condition.InSnow)
                .Add(ItemID.EskimoPants, Condition.InSnow)
                .Add(ItemID.PinkEskimoHood, Condition.InSnow)
                .Add(ItemID.PinkEskimoCoat, Condition.InSnow)
                .Add(ItemID.PinkEskimoPants, Condition.InSnow);

            npcShop.Register(); // Name of this shop tab
        }
        #endregion

        #region Attacking
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 5;
            knockback = 5;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.WeatherPainShot;
            attackDelay = 1;
        }
        #endregion

        public override bool CanGoToStatue(bool toKingStatue) => true;
    }
}