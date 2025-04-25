using Eventful.Dusts;
using Eventful.Events;
using Eventful.Items.Miscellaneous;
using Eventful.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Enemies.SunnyDay
{
    public class LivingSunflower : ModNPC
    {
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

                new FlavorTextBestiaryInfoElement("Some sunflowers started sprouting to life in a different way")
            });
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }
        
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 68;
            NPC.damage = 12;
            NPC.lifeMax = 45;
            NPC.defense = 6;
            NPC.knockBackResist = 0.35f;
            NPC.value = 50;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            AIType = NPCID.GoblinScout;

            SpawnModBiomes = [ModContent.GetInstance<SunnyDayBiome>().Type];

            Banner = Type;
            BannerItem = Mod.Find<ModItem>("LivingSunflowerBanner").Type;

            #region Audio pitch variance
            NPC.HitSound = SoundID.NPCHit1 with
            {
                PitchVariance = 0.25f
            };
            NPC.DeathSound = SoundID.NPCDeath39 with
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
            NPC.spriteDirection = -NPC.direction;
        }

        public override void OnKill()
        {
            #region Gore
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("LivingSunflowerGore1").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("LivingSunflowerGore2").Type, NPC.scale);
            #endregion

            #region Dust
            for (int d = 0; d < 10; d++)
            {
                Dust.NewDust(NPC.position, 0, 0, DustID.Smoke, 0, 0, 235, Color.White, Main.rand.NextFloat(1.5f, 2));
                Main.dust[d].velocity *= 0.025f;
                Main.dust[d].noGravity = true;
            }
            #endregion
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunshineFragment>(), 1, 1, 2)); //100% drop rate, 1-2

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WaterBalloon>(), 2, 5, 10)); //100% drop rate, 1-2

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Bloomerang>(), 5)); //20% drop rate
        }
    }
}