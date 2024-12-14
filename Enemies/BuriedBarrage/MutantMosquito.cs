using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Eventful.Items.Accessories;
using Eventful.Invasions;
using Eventful.Dusts;
using EasingLibrary;
using Eventful.Items.Miscellaneous;

namespace Eventful.Enemies.BuriedBarrage
{
    public class MutantMosquito : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 8;

            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }

        public override void SetDefaults()
        {
            NPC.width = 66;
            NPC.height = 40;
            NPC.scale = Main.rand.NextFloat(0.75f, 1.25f);
            NPC.damage = 8;
            NPC.lifeMax = 25;
            NPC.defense = 4;
            NPC.knockBackResist = 0.4f;
            NPC.value = 50;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.aiStyle = NPCAIStyleID.FlyingFish;

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
            #region Direction
            NPC.TargetClosest(true);
            NPC.spriteDirection = NPC.direction;
            #endregion

            #region Rotation
            NPC.rotation = NPC.velocity.X * 0.1f;
            #endregion

            #region Variation
            NPC.oldVelocity *= -NPC.scale; //Smaller = faster, bigger = slower
            #endregion

            #region Dust
            if (Main.rand.NextBool(2))
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<MutantDust>(), 0, 10, 150, default, 1);
            }
            #endregion

            #region Lighting
            Lighting.AddLight(NPC.Center, 0.25f, 0.25f, 0.25f);
            #endregion

            base.AI();
        }

        public override void FindFrame(int frameHeight)
        {
            int frameSpeed = 5;

            NPC.frameCounter++;

            if (NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }
        
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            #region Trail
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Eventful/Enemies/BuriedBarrage/MutantMosquito");
            int frameHeight = texture.Height / Main.npcFrameCount[NPC.type];
            int startY = NPC.frame.Y;
            Rectangle sourceRectangle = new Rectangle(0, startY, NPC.width, NPC.height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            origin.X = (float)(NPC.spriteDirection == 1 ? sourceRectangle.Width - 20 : 20);
            SpriteEffects spriteEffects = SpriteEffects.None;

            if (NPC.spriteDirection == 1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            for (int k = 0; k < NPC.oldPos.Length; k += 2)
            {
                Vector2 drawPos = (NPC.oldPos[k] - Main.screenPosition) + origin + new Vector2(0, NPC.gfxOffY);
                Color color = NPC.GetAlpha(drawColor * 0.25f) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);

                Main.spriteBatch.Draw(texture, drawPos, sourceRectangle, color, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            }
            #endregion

            return true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MosquitoSack>(), 50)); //5% drop rate

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MutatedFlesh>(), 1, 1, 5)); //100% drop rate, 1-5
        }

        public override void OnKill()
        {
            #region Gore
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MutantMosquitoGore1").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MutantMosquitoGore2").Type, NPC.scale);
            #endregion

            #region Dust
            //Mutant Dust
            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<MutantDust>(), 0, 0, 175, default, Main.rand.NextFloat(1.5f, 2));
            }

            //Smoke
            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(NPC.position, 0, 0, DustID.Smoke, 0, 0, 235, Color.White, Main.rand.NextFloat(1.5f, 2));
                Main.dust[d].velocity *= 0.025f;
                Main.dust[d].noGravity = true;
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
    }
}