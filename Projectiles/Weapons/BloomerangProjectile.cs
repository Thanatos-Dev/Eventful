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
            Projectile.penetrate = 999;
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

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // If collide with tile, reduce the penetrate.
            // So the projectile can reflect at most 5 times
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

                // If the projectile hits the left or right side of the tile, reverse the X velocity
                if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }

                // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }

            return false;
        }
    }
}