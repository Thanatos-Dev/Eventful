using Eventful.Invasions;
using Eventful.Items.Miscellaneous;
using Eventful.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Enemies.BuriedBarrage
{
    internal class MutantCentipedeHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<MutantCentipedeBody>();
        public override int TailType => ModContent.NPCType<MutantCentipedeTail>();

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,

                                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Mutated from evolving underground, this creature is aggressive and will attack anyone in sight")
            });
        }

        public override void SetStaticDefaults()
        {
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = "Eventful/Enemies/BuriedBarrage/MutantCentipedeBestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
                Scale = 0.5f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerHead);
            NPC.aiStyle = -1;

            NPC.width = 42 / 3;
            NPC.height = 26;
            NPC.scale = 1.05f;

            NPC.damage = 6;
            NPC.lifeMax = 35;
            NPC.defense = 0;
            NPC.value = 400;

            #region Audio pitch variance
            NPC.HitSound = SoundID.NPCHit31 with
            {
                PitchVariance = 0.25f
            };
            NPC.DeathSound = SoundID.NPCDeath35 with
            {
                PitchVariance = 0.25f
            };
            #endregion
        }

        public override void AI()
        {
            #region Lighting
            Lighting.AddLight(NPC.Center, 0.15f, 0.15f, 0.15f);
            #endregion
        }

        public override void OnKill()
        {
            #region Gore
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MutantCentipedeHeadGore").Type, NPC.scale);
            #endregion

            #region Dust
            for (int d = 0; d < 15; d++)
            {
                Dust.NewDust(NPC.position, 0, 0, DustID.Smoke, 0, 0, 235, Color.White, Main.rand.NextFloat(1.5f, 2));
            }
            #endregion

            #region Invasion
            if (BuriedBarrageInvasion.isActive == true)
            {
                BuriedBarrageInvasion.killCount++; //Counts up the invasion's kill count

                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData); //Immediately inform clients of new world state.
                }
            }
            #endregion
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MutatedFlesh>(), 1, 1, 2)); //100% drop rate, 1-2

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CentipedeSnapper>(), 16)); //6.25% drop rate
        }

        public override void Init()
        {
            MinSegmentLength = 8;
            MaxSegmentLength = 12;

            CommonWormInit(this);
        }

        internal static void CommonWormInit(Worm worm)
        {
            worm.MoveSpeed = 4.5f;
            worm.Acceleration = 0.045f;
        }
    }

    internal class MutantCentipedeBody : WormBody
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true //Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            Main.npcFrameCount[Type] = 3;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerBody);
            NPC.aiStyle = -1;

            NPC.width = 42 / 3;
            NPC.height = 16;
            NPC.scale = 1.05f;

            NPC.damage = 3;
            NPC.lifeMax = 35;
            NPC.defense = 3;
            NPC.value = 50;

            #region Audio pitch variance
            NPC.HitSound = SoundID.NPCHit31 with
            {
                PitchVariance = 0.25f
            };
            #endregion
        }

        public override void AI()
        {
            #region Lighting
            Lighting.AddLight(NPC.Center, 0.15f, 0.15f, 0.15f);
            #endregion
        }

        #region Random Body Sprite
        public override void OnSpawn(IEntitySource source)
        {
            NPC.frameCounter = Main.rand.Next(3);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
        }
        #endregion

        public override void HitEffect(NPC.HitInfo hit)
        {
            #region Gore
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MutantCentipedeBodyGore").Type, NPC.scale);
            }
            #endregion
        }

        public override void Init()
        {
            MutantCentipedeHead.CommonWormInit(this);
        }
    }

    internal class MutantCentipedeTail : WormTail
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true //Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerTail);
            NPC.aiStyle = -1;

            NPC.width = 42 / 3;
            NPC.height = 26;
            NPC.scale = 1.05f;

            NPC.damage = 3;
            NPC.lifeMax = 35;
            NPC.defense = 6;
            NPC.value = 50;

            #region Audio pitch variance
            NPC.HitSound = SoundID.NPCHit31 with
            {
                PitchVariance = 0.25f
            };
            #endregion
        }

        public override void AI()
        {
            #region Lighting
            Lighting.AddLight(NPC.Center, 0.15f, 0.15f, 0.15f);
            #endregion
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            #region Gore
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MutantCentipedeTailGore").Type, NPC.scale);
            }
            #endregion
        }

        public override void Init()
        {
            MutantCentipedeHead.CommonWormInit(this);
        }
    }
}