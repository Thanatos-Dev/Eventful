using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Projectiles.Weapons
{
    public class BloomerangProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.aiStyle = ProjAIStyleID.Boomerang;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            // Dust
            int sunflowerDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Sunflower, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
            Main.dust[sunflowerDust].noGravity = true;

            int grassDust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f);
            Main.dust[grassDust].noGravity = true;

            // Lighting
            Lighting.AddLight(Projectile.Center, new Vector3(255, 231, 0) / 255f * 0.35f);
        }
    }
}