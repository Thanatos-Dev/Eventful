using Eventful.Dusts;
using Eventful.Invasions;
using Eventful.Items.Miscellaneous;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Enemies.BuriedBarrage
{
    public class MutantBeetle : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;

            NPCID.Sets.TrailCacheLength[NPC.type] = 6;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }

        public override void SetDefaults()
        {
            NPC.width = 28;
            NPC.height = 24;
            NPC.damage = 6;
            NPC.lifeMax = 25;
            NPC.defense = 8;
            NPC.knockBackResist = 0.5f;
            NPC.value = 50;
            NPC.aiStyle = NPCAIStyleID.Bat;

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

        #region Animation
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
        #endregion

        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;

            Lighting.AddLight(NPC.Center, 0.15f, 0.15f, 0.15f);

            #region Dust
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<MutantDust>(), 0, 10, 150, default, 1);
            }
            #endregion
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            #region Trail
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Eventful/Enemies/BuriedBarrage/MutantBeetle");
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
                Color color = NPC.GetAlpha(drawColor * 0.35f) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);

                Main.spriteBatch.Draw(texture, drawPos, sourceRectangle, color, NPC.rotation, origin, NPC.scale, spriteEffects, 0);
            }
            #endregion

            return true;
        }

        public override void OnKill()
        {
            #region Gore
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MutantBeetleGore1").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MutantBeetleGore2").Type, NPC.scale);
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

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MutatedFlesh>(), 1, 1, 2)); //100% drop rate, 1-2
        }
    }
}