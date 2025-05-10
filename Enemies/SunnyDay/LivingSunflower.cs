using Eventful.Events;
using Eventful.Items.Miscellaneous;
using Eventful.Weapons;
using Microsoft.Xna.Framework;
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

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                Scale = 0.75f,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }
        
        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 68;
            NPC.damage = 8;
            NPC.lifeMax = 50;
            NPC.defense = 4;
            NPC.knockBackResist = 0.35f;
            NPC.value = 100;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            AIType = NPCID.ZombieMushroom;

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

        public override void HitEffect(NPC.HitInfo hit)
        {
            #region Dust
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sunflower);
            Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass);
            #endregion
        }

        public override void OnKill()
        {
            #region Gore
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("LivingSunflowerGore1").Type, NPC.scale);
            Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("LivingSunflowerGore2").Type, NPC.scale);
            #endregion

            #region Dust
            for (int d = 0; d < 5; d++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Sunflower);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Grass);
            }
            #endregion
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunshineFragment>(), 2)); //50% drop rate

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<WaterBalloon>(), 2, 5, 10)); //50% drop rate, 5-10

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Bloomerang>(), 10)); //10% drop rate
        }
    }
}