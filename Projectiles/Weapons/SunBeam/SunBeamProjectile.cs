using Eventful.Dusts;
using Eventful.Projectiles.Weapons.SunBeam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Projectiles.Weapons.SunBeam
{
    public class SunBeamProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;

            ProjectileID.Sets.TrailingMode[Projectile.type] = 5;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = (int)(60 * 2f);
            Projectile.alpha = 1;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;

            // Animation
            if (++Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;

                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            // Kill projectile
            if (Projectile.ai[0] >= Projectile.timeLeft)
            {
                Projectile.Kill();
            }

            // Direction and rotation
            Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
                // For vertical sprites use MathHelper.PiOver2
            }

            // Lighting
            Lighting.AddLight(Projectile.Center, new Color(255, 191, 0).ToVector3());
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Trail
            default(TrailShader).Draw(Projectile);

            // Dust
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Main.CurrentDrawnEntity.position, Main.CurrentDrawnEntity.width, Main.CurrentDrawnEntity.height, ModContent.DustType<SunBeamDust>(), Projectile.velocity.X, Projectile.velocity.Y);
            }

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            #region Main Texture
            // SpriteEffects helps to flip texture horizontally and vertically
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            // Getting texture of projectile
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeight = texture.Height / Main.projFrames[Type];
            int startY = frameHeight * Projectile.frame;

            // Get this frame on texture
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

            // Alternatively, you can skip defining frameHeight and startY and use this:
            // Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangle.Size() / 2f;

            // If image isn't centered or symmetrical you can specify origin of the sprite
            // (0,0) for the upper-left corner
            float offsetX = 15f;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);

            // Draw projectile
            Color drawColor = new Color(255, 255, 255, 50);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
            #endregion

            #region Glow Textures
            float scaleMultiplier = 0.65f;
            float opacityMultiplier = 0.01f;

            Texture2D glowTexture = ModContent.Request<Texture2D>("Eventful/Projectiles/Weapons/SunBeam/SunBeamGlow").Value;
            Color glowDrawColor = new Color(255, 175, 0, 1) * opacityMultiplier;
            Rectangle glowSourceRectangle = new Rectangle(0, 0, glowTexture.Width, glowTexture.Height);
            Vector2 glowOrigin = glowSourceRectangle.Size() / 2f;

            Main.EntitySpriteDraw(glowTexture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                glowSourceRectangle, glowDrawColor, Projectile.rotation, glowOrigin, Projectile.scale * scaleMultiplier, SpriteEffects.None, 0);

            Main.EntitySpriteDraw(glowTexture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                glowSourceRectangle, glowDrawColor * 5f, Projectile.rotation, glowOrigin, Projectile.scale * scaleMultiplier * 0.75f, SpriteEffects.None, 0);

            Main.EntitySpriteDraw(glowTexture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                glowSourceRectangle, glowDrawColor * 10f, Projectile.rotation, glowOrigin, Projectile.scale * scaleMultiplier * 0.5f, SpriteEffects.None, 0);

            Main.EntitySpriteDraw(glowTexture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                glowSourceRectangle, glowDrawColor * 15f, Projectile.rotation, glowOrigin, Projectile.scale * scaleMultiplier * 0.25f, SpriteEffects.None, 0);
            #endregion
        }

        public override void OnKill(int timeLeft)
        {
            // SFX
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            // Dust
            for (int i = 0; i < 50; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(1f, 1f) * Main.rand.NextFloat(2.5f, 7.5f);

                Dust.NewDustPerfect(Projectile.position, ModContent.DustType<SunBeamExplosionDust>(), speed);
            }

            // Explosion Projectile
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SunBeamExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Buff
            target.AddBuff(BuffID.OnFire, 60 * 5);
        }

        #region Trail Shader
        public struct TrailShader
        {
            private static VertexStrip _vertexStrip = new VertexStrip();
            private float transitToDark;

            public void Draw(Projectile proj)
            {
                this.transitToDark = Utils.GetLerpValue(0.0f, 6f, proj.localAI[0], true);
                MiscShaderData miscShaderData = GameShaders.Misc["FlameLash"];
                miscShaderData.UseSaturation(-2f);
                miscShaderData.UseOpacity(MathHelper.Lerp(4f, 8f, this.transitToDark));
                miscShaderData.Apply(new DrawData?());
                TrailShader._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f);
                TrailShader._vertexStrip.DrawTrail();
                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            }

            private Color StripColors(float progressOnStrip)
            {
                float lerpValue = Utils.GetLerpValue((float)(0.0 - 0.100000001490116 * (double)this.transitToDark), (float)(0.699999988079071 - 0.200000002980232 * (double)this.transitToDark), progressOnStrip, true);
                Color color = Color.Lerp(Color.Lerp(Color.White, new Color(255, 200, 0), this.transitToDark * 0.5f), new Color(255, 200, 0), lerpValue) * (1f - Utils.GetLerpValue(0.0f, 0.98f, progressOnStrip, false));
                color.A /= (byte)8;
                return color;
            }

            private float StripWidth(float progressOnStrip)
            {
                return MathHelper.SmoothStep(0f, 20f, progressOnStrip * 5f);
            }
        }
        #endregion
    }
}