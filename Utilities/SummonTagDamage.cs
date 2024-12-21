using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Utilities
{
    public class SummonTagDamage : ModBuff
    {
        public static int TagDamage = 1;

        public override void SetStaticDefaults()
        {
            //This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
            BuffID.Sets.IsATagBuff[Type] = true;
        }
    }

    public class SummonTagDamageNPC : GlobalNPC
    {
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            //Only player attacks should benefit from this buff, hence the NPC and trap checks.
            if (projectile.npcProj || projectile.trap || !projectile.IsMinionOrSentryRelated)
                return;


            //SummonTagDamageMultiplier scales down tag damage for some specific minion and sentry projectiles for balance purposes.
            var projTagMultiplier = ProjectileID.Sets.SummonTagDamageMultiplier[projectile.type];
            if (npc.HasBuff<SummonTagDamage>())
            {
                //Apply a flat bonus to every hit
                modifiers.FlatBonusDamage += SummonTagDamage.TagDamage * projTagMultiplier;
            }
        }
    }
}