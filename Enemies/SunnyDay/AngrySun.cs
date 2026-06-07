using Eventful.Events;
using Eventful.Items.Miscellaneous;
using Eventful.Projectiles.Enemies;
using Eventful.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Enemies.SunnyDay
{
    public class AngrySun : ModNPC
    {
        public static int shootTimerMax = 60 * 2;
        public static int shootTimer = shootTimerMax;

        private enum Frame
        {
            Move1,
            Move2,
            Move3,
            Move4,
            Move5,
            Attack1,
            Attack2
        }

        public enum ActionState
        {
            Move,
            Attack
        };

        public float AI_Timer = 0;
        public float AI_State = 0;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

                new FlavorTextBestiaryInfoElement("An embodiment of sunshine itself.")
            });
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 7;

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Scale = 0.75f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Wet] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 60;
            NPC.height = 60;
            NPC.damage = 12;
            NPC.lifeMax = 50;
            NPC.defense = 5;
            NPC.knockBackResist = 0.3f;
            NPC.value = 200;
            NPC.alpha = 1;

            SpawnModBiomes = [ModContent.GetInstance<SunnyDayBiome>().Type];

            Banner = Type;
            BannerItem = Mod.Find<ModItem>("AngrySunBanner").Type;

            #region Audio pitch variance
            NPC.HitSound = SoundID.NPCHit30 with
            {
                PitchVariance = 0.25f
            };
            NPC.DeathSound = SoundID.NPCDeath33 with
            {
                PitchVariance = 0.25f
            };
            #endregion
        }

        #region Animation
        // Move: 0-4
        // Attack: 5-6
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

                        if (NPC.frame.Y >= 4 * frameHeight)
                        {
                            NPC.frame.Y = 0;
                        }
                    }

                    break;

                case (float)ActionState.Attack:

                    NPC.frameCounter++;

                    #region Frames
                    if (NPC.frameCounter < 10)
                    {
                        NPC.frame.Y = (int)Frame.Attack1 * frameHeight;
                    }
                    else if (NPC.frameCounter < 20)
                    {
                        NPC.frame.Y = (int)Frame.Attack2 * frameHeight;
                    }
                    else
                    {
                        NPC.frameCounter = 0;
                        AI_State = (float)ActionState.Move;
                    }
                    #endregion

                    break;
            }
        }
        #endregion

        public override void AI()
        {
            #region Movement
            NPC.noGravity = true;
            NPC.TargetClosest();
            float num766 = 4f;
            float num767 = 0.25f;
            Vector2 vector96 = new Vector2(NPC.Center.X, NPC.Center.Y);
            float num768 = Main.player[NPC.target].Center.X - vector96.X;
            float num769 = Main.player[NPC.target].Center.Y - vector96.Y - 200f;
            float num770 = (float)Math.Sqrt(num768 * num768 + num769 * num769);

            if (num770 < 20f)
            {
                num768 = NPC.velocity.X;
                num769 = NPC.velocity.Y;
            }
            else
            {
                num770 = num766 / num770;
                num768 *= num770;
                num769 *= num770;
            }
            if (NPC.velocity.X < num768)
            {
                NPC.velocity.X += num767;
                if (NPC.velocity.X < 0f && num768 > 0f)
                {
                    NPC.velocity.X += num767 * 2f;
                }
            }
            else if (NPC.velocity.X > num768)
            {
                NPC.velocity.X -= num767;
                if (NPC.velocity.X > 0f && num768 < 0f)
                {
                    NPC.velocity.X -= num767 * 2f;
                }
            }
            if (NPC.velocity.Y < num769)
            {
                NPC.velocity.Y += num767;
                if (NPC.velocity.Y < 0f && num769 > 0f)
                {
                    NPC.velocity.Y += num767 * 2f;
                }
            }
            else if (NPC.velocity.Y > num769)
            {
                NPC.velocity.Y -= num767;
                if (NPC.velocity.Y > 0f && num769 < 0f)
                {
                    NPC.velocity.Y -= num767 * 2f;
                }
            }
            if (NPC.position.X + (float)NPC.width > Main.player[NPC.target].position.X && NPC.position.X < Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width && NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && Main.netMode != 1)
            {
                NPC.ai[0] += 1f;
                if (NPC.ai[0] > 8f)
                {
                    NPC.ai[0] = 0f;
                    int num771 = (int)(NPC.position.X + 10f + (float)Main.rand.Next(NPC.width - 20));
                    int num772 = (int)(NPC.position.Y + (float)NPC.height + 4f);
                }
            }
            #endregion

            #region Projectile
            if (shootTimer > 0)
            {
                shootTimer--;
            }
            else
            {
                if (NPC.position.X + (float)NPC.width > Main.player[NPC.target].position.X && NPC.position.X < Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width && NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && Main.netMode != 1)
                {
                    AI_State = (float)ActionState.Attack;

                    SoundEngine.PlaySound(SoundID.Item45, NPC.Center);

                    shootTimer = shootTimerMax;
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.position + new Vector2(NPC.width / 2, NPC.height), new Vector2(0f, 7.5f), ModContent.ProjectileType<AngrySunProjectile>(), (int)(NPC.damage * 0.25f), 0f, Main.myPlayer);
                }
            }
            #endregion

            Lighting.AddLight(NPC.Center, new Color(255, 244, 204).ToVector3());
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (NPC.position.Y > Main.worldSurface * 16.0)
            {
                NPC.active = false;
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunshineFragment>(), 2)); //50% drop rate

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WaterBalloon>(), 2, 5, 10)); //50% drop rate, 5-10

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunBeam>(), 10)); //10% drop rate
        }
    }
}