using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Projectiles.Weapons
{
    public class WaterBalloonProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.2f;

            if (Projectile.direction == 1)
            {
                Projectile.rotation += 0.5f;
            }
            else
            {
                Projectile.rotation -= 0.5f;
            }

            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Water, 0, 0, 0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Wet, 300);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Splash with { MaxInstances = 10 }, Projectile.position);
            SoundEngine.PlaySound(SoundID.SplashWeak with { MaxInstances = 10 }, Projectile.position);

            #region Dust
            for (int i = 0; i < 35; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position + new Vector2(0, 10), Projectile.width, Projectile.height, DustID.Water, 0, -5, 0, default, Main.rand.NextFloat(1.25f, 1.75f));
                dust.noGravity = true;
                dust.velocity *= 2.5f;
                dust.scale *= 0.9f;
            }
            #endregion
        }
    }
}