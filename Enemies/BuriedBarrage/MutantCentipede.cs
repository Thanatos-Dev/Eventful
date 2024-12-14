using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Eventful.Items.Accessories;
using Eventful.Invasions;
using Terraria.DataStructures;
using System.IO;

namespace Eventful.Enemies.BuriedBarrage
{
    internal class MutantCentipedeHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<MutantCentipedeBody>();
        public override int TailType => ModContent.NPCType<MutantCentipedeTail>();

        public override void SetDefaults()
        {
            // Head is 10 defense, body 20, tail 30.
            NPC.CloneDefaults(NPCID.DiggerHead);
            NPC.aiStyle = -1;

            NPC.width = 42 / 3;
            NPC.height = 26;
            NPC.scale = 1.05f;

            NPC.damage = 6;
            NPC.lifeMax = 35;
            NPC.defense = 0;
            NPC.value = 50;

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
            Lighting.AddLight(NPC.Center, 0.25f, 0.25f, 0.25f);
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

        public override void Init()
        {
            // Set the segment variance
            // If you want the segment length to be constant, set these two properties to the same value
            MinSegmentLength = 8;
            MaxSegmentLength = 12;

            CommonWormInit(this);
        }

        internal static void CommonWormInit(Worm worm)
        {
            // These two properties handle the movement of the worm
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
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
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
            Lighting.AddLight(NPC.Center, 0.25f, 0.25f, 0.25f);
            #endregion
        }

        public override void OnSpawn(IEntitySource source)
        {
            NPC.frameCounter = Main.rand.Next(3);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = (int)NPC.frameCounter * frameHeight;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                #region Gore
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MutantCentipedeBodyGore").Type, NPC.scale);
                #endregion
            }
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
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
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
            Lighting.AddLight(NPC.Center, 0.25f, 0.25f, 0.25f);
            #endregion
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                #region Gore
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MutantCentipedeTailGore").Type, NPC.scale);
                #endregion
            }
        }

        public override void Init()
        {
            MutantCentipedeHead.CommonWormInit(this);
        }
    }
}