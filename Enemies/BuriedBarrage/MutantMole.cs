using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Eventful.Items.Accessories;
using Eventful.Invasions;
using Eventful.Dusts;

namespace Eventful.Enemies.BuriedBarrage
{
    public class MutantMole : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 44;
            NPC.damage = 8;
            NPC.lifeMax = 35;
            NPC.defense = 8;
            NPC.knockBackResist = 0.25f;
            NPC.value = 50;
            NPC.aiStyle = NPCAIStyleID.Fighter;

            #region Audio pitch variance
            NPC.HitSound = SoundID.NPCHit26 with
            {
                PitchVariance = 0.25f
            };
            NPC.DeathSound = SoundID.NPCDeath29 with
            {
                PitchVariance = 0.25f
            };
            #endregion
        }

        public override void AI()
        {
            NPC.spriteDirection = -NPC.direction;

            #region Dust
            if (Main.rand.NextBool(3))
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<MutantDust>(), 0, 10, 150, default, 1);
            }
            #endregion
        }

        public override void OnKill()
        {
            #region Gore
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MutantMoleGore1").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MutantMoleGore2").Type, NPC.scale);
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