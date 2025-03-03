using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventful.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Projectiles.Weapons
{
    public class RatStaffProjectile : ModProjectile
    {
        #region Variables
        public static float spawnTimer = 15;
        public static float spawnTimerMax = 15;
        #endregion

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 10;
        }

        public override void SetDefaults()
        {
            Projectile.width = 156;
            Projectile.height = 16;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3600;
            Projectile.netImportant = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            #region Animation
            Projectile.frameCounter += 2;

            if (Projectile.frameCounter >= 11)
            {
                Projectile.frameCounter = 0;

                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
            #endregion

            if (spawnTimer > 0)
            {
                spawnTimer--;

                Projectile.velocity = new Vector2(0, 100);
            }
            else
            {
                Projectile.alpha = 0;

                Dust.NewDust(Projectile.position + new Vector2(25, 0), (int)(Projectile.width * 0.65f), Projectile.height, ModContent.DustType<MutantDust>(), 0, Main.rand.Next(5, 10), 150, default, Main.rand.NextFloat(1, 1.25f));
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Getting texture of projectile
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeight = texture.Height / Main.projFrames[Type];
            int startY = frameHeight * Projectile.frame;

            // Get this frame on texture
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

            Vector2 origin = sourceRectangle.Size() / 2f;

            // If image isn't centered or symmetrical you can specify origin of the sprite
            // (0,0) for the upper-left corner
            float offsetX = Projectile.width / 2;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);


            // Draw current frame
            if (spawnTimer <= 0)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRectangle, new Color(0, 0, 0, 100), Projectile.rotation, origin, Projectile.scale * 0.65f, SpriteEffects.FlipHorizontally, 0);
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), sourceRectangle, new Color(0, 0, 0, 25), Projectile.rotation, origin, Projectile.scale * 1.35f, SpriteEffects.None, 0);
            }

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            #region Dust
            for (int i = 0; i < 25; i++)
            {
                Dust.NewDust(Projectile.position + new Vector2(25, 0), (int)(Projectile.width * 0.65f), Projectile.height, ModContent.DustType<MutantDust>(), Main.rand.Next(-15, 15), Main.rand.Next(-15, 15), 150, default, Main.rand.NextFloat(1, 1.5f));
            }
            #endregion
        }

        #region Tile collision
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            width = 10;

            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        #endregion
    }
}