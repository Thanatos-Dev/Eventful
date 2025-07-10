using Eventful.Events;
using Eventful.Items.Miscellaneous;
using Eventful.Weapons;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Enemies.SunnyDay
{
    public class Snake : ModNPC
    {
        private enum Frame
        {
            Move1,
            Move2,
            Move3,
            Move4,
            Attack1,
            Attack2,
            Attack3,
            Attack4,
            Attack5,
            Attack6
        }

        public enum ActionState
        {
            Move,
            Attack
        };

        public float AI_Timer = 0;
        public float AI_State = 0;

        private float attackCooldown = 50f;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

                new FlavorTextBestiaryInfoElement("An overgrown snake that loves snapping at Terrarians")
            });
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Scale = 0.65f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }
        
        public override void SetDefaults()
        {
            NPC.width = 92 / 2;
            NPC.height = 28;
            NPC.damage = 11;
            NPC.lifeMax = 30;
            NPC.defense = 6;
            NPC.knockBackResist = 0.25f;
            NPC.value = 200;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            AIType = NPCID.ZombieMushroom;

            SpawnModBiomes = [ModContent.GetInstance<SunnyDayBiome>().Type];

            Banner = Type;
            BannerItem = Mod.Find<ModItem>("SnakeBanner").Type;

            #region Audio pitch variance
            NPC.HitSound = SoundID.NPCHit23 with
            {
                PitchVariance = 0.25f
            };
            NPC.DeathSound = SoundID.NPCDeath26 with
            {
                PitchVariance = 0.25f
            };
            #endregion
        }

        #region Animation
        // Move: 0-3
        // Attack: 4-9
        public override void FindFrame(int frameHeight)
        {
            int moveAnimationSpeed = 5;

            switch (AI_State)
            {
                case (float)ActionState.Move:

                    NPC.frameCounter++;

                    if (NPC.frameCounter >= moveAnimationSpeed)
                    {
                        NPC.frameCounter = 0;
                        NPC.frame.Y += frameHeight;

                        if (NPC.frame.Y >= 3 * frameHeight)
                        {
                            NPC.frame.Y = 0;
                        }
                    }

                    break;

                case (float)ActionState.Attack:

                    NPC.frameCounter++;

                    #region Frames
                    if (NPC.frameCounter < 5)
                    {
                        NPC.frame.Y = (int)Frame.Attack1 * frameHeight;
                    }
                    else if (NPC.frameCounter < 10)
                    {
                        NPC.frame.Y = (int)Frame.Attack2 * frameHeight;
                    }
                    else if (NPC.frameCounter < 15)
                    {
                        NPC.frame.Y = (int)Frame.Attack3 * frameHeight;
                    }
                    else if (NPC.frameCounter < 20)
                    {
                        NPC.frame.Y = (int)Frame.Attack4 * frameHeight;
                    }
                    else if (NPC.frameCounter < 25)
                    {
                        NPC.frame.Y = (int)Frame.Attack5 * frameHeight;
                    }
                    else if (NPC.frameCounter < 30)
                    {
                        NPC.frame.Y = (int)Frame.Attack6 * frameHeight;
                    }
                    else if (NPC.frameCounter < 40)
                    {
                        NPC.frame.Y = (int)Frame.Attack2 * frameHeight;
                    }
                    else if (NPC.frameCounter < 50)
                    {
                        NPC.frame.Y = (int)Frame.Attack1 * frameHeight;
                    }
                    else
                    {
                        NPC.frameCounter = 0;
                    }
                    #endregion

                    break;
            }
        }
        #endregion

        #region AI
        // AI
        public override void AI()
        {
            base.AI();
        }

        // PostAI
        public override void PostAI()
        {
            Player target = Main.player[NPC.target];

            if (AI_Timer <= 0)
            {
                AI_Timer = 0;
            }

            if (NPC.velocity.X > 1.5f)
            {
                NPC.velocity.X = 1.5f;
            }

            if (NPC.velocity.X < -1.5f)
            {
                NPC.velocity.X = -1.5f;
            }

            NPC.spriteDirection = NPC.direction;
            NPC.TargetClosest();

            NPC.GravityMultiplier *= 100;
            NPC.MaxFallSpeedMultiplier *= 0.5f;

            bool canAttack = NPC.HasValidTarget &&
                Main.netMode != NetmodeID.MultiplayerClient &&
                AI_Timer == 0 &&
                Vector2.Distance(NPC.Center, target.Center) <= 3.5f * 16f &&
                Collision.CanHitLine(NPC.position, NPC.width, NPC.height, target.position, target.width, target.height);

            switch (AI_State)
            {
                case (float)ActionState.Move:
                    AI_Move(canAttack);
                    break;

                case (float)ActionState.Attack:
                    AI_Attack();
                    break;
            }
        }

        // Move
        private void AI_Move(bool canAttack)
        {
            NPC.damage = 0;

            if (canAttack)
            {
                AI_State = (float)ActionState.Attack;
                AI_Timer = attackCooldown;
                NPC.netUpdate = true;
            }

            AI_Timer--;
        }

        // Attack
        private void AI_Attack()
        {
            AI_Timer--;

            NPC.velocity.X = 0;

            if (NPC.DirectionTo(Main.player[NPC.target].Center).X > 0)
            {
                NPC.spriteDirection = 1;
            }

            if (NPC.DirectionTo(Main.player[NPC.target].Center).X < 0)
            {
                NPC.spriteDirection = -1;
            }

            if (AI_Timer <= attackCooldown - 20f)
            {
                NPC.damage = 11;

                if (Main.expertMode)
                {
                    NPC.damage = 11 * 2;
                }

                if (Main.masterMode)
                {
                    NPC.damage = 11 * 3;
                }
            }

            if (AI_Timer <= 0)
            {
                AI_State = (float)ActionState.Move;
                NPC.netUpdate = true;
            }
        }
        #endregion

        public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
        {
            Rectangle damageHitbox = new Rectangle((int)NPC.position.X - NPC.width, (int)NPC.position.Y - NPC.height, NPC.width * 3, NPC.height);

            npcHitbox = damageHitbox;

            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            #region Blood
            for (int d = 0; d < 5; d++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
            }
            #endregion
        }

        public override void OnKill()
        {
            #region Gore
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SnakeGore1").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SnakeGore2").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SnakeGore3").Type, NPC.scale);
            #endregion

            #region Blood
            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
            }
            #endregion
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WaterBalloon>(), 2, 5, 10)); //50% drop rate, 5-10

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Serpentine>(), 10)); //10% drop rate
        }
    }
}