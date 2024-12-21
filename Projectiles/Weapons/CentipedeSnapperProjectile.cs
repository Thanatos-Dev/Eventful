using Eventful.Dusts;
using Eventful.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Eventful.Projectiles.Weapons
{
    public class CentipedeSnapperProjectile : ModProjectile
    {
        #region Variables
        public static int whipSegments = 15; //Can't go under 10
        public static float rangeMultiplier = 0.4f;

        public static float whipScale = 0.75f;
        public static Color lineColor = new Color(156, 62, 98); //RGB values
        
        public static int summonTagDamage = 5;
        public static int summonTagDamageTime = 60 * 4; //Multiply by how many seconds it should last
        public static float multihitPenalty = 0.5f; //Decrease the damage the more enemies the whip hits
        #endregion

        public override void SetStaticDefaults()
        {
            //This makes the projectile use whip collision detection and allows flasks to be applied to it.
            ProjectileID.Sets.IsAWhip[Type] = true;

            SummonTagDamage.TagDamage = summonTagDamage;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 96;

            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true; //This prevents the projectile from hitting through solid tiles.
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            Projectile.WhipSettings.Segments = whipSegments;
            Projectile.WhipSettings.RangeMultiplier = rangeMultiplier;
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2; //Without PiOver2, the rotation would be off by 90 degrees counterclockwise.

            Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * Timer;
            //Vanilla uses Vector2.Dot(Projectile.velocity, Vector2.UnitX) here. Dot Product returns the difference between two vectors, 0 meaning they are perpendicular.
            //However, the use of UnitX basically turns it into a more complicated way of checking if the projectile's velocity is above or equal to zero on the X axis.
            Projectile.spriteDirection = Projectile.velocity.X >= 0f ? 1 : -1;

            Timer++;

            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;
            if (Timer >= swingTime || owner.itemAnimation <= 0)
            {
                Projectile.Kill();
                return;
            }

            owner.heldProj = Projectile.whoAmI;
            if (Timer == swingTime / 2)
            {
                //Plays a whipcrack sound at the tip of the whip.
                List<Vector2> points = Projectile.WhipPointsForCollision;
                Projectile.FillWhipControlPoints(Projectile, points);
                SoundEngine.PlaySound(SoundID.Item153, points[points.Count - 1]);
            }

            //Spawn Dust along the whip path
            //This is the dust code used by Durendal. Consult the Terraria source code for even more examples, found in Projectile.AI_165_Whip.
            float swingProgress = Timer / swingTime;
            //This code limits dust to only spawn during the the actual swing.
            if (Utils.GetLerpValue(0.1f, 0.7f, swingProgress, clamped: true) * Utils.GetLerpValue(0.9f, 0.7f, swingProgress, clamped: true) > 0.5f && !Main.rand.NextBool(3))
            {
                List<Vector2> points = Projectile.WhipPointsForCollision;
                points.Clear();
                Projectile.FillWhipControlPoints(Projectile, points);
                int pointIndex = Main.rand.Next(points.Count - 10, points.Count);
                Rectangle spawnArea = Utils.CenteredRectangle(points[pointIndex], new Vector2(30, 30));
                int dustType = ModContent.DustType<MutantDust>();
                if (Main.rand.NextBool(1))
                    dustType = ModContent.DustType<MutantDust>();

                //After choosing a randomized dust and a whip segment to spawn from, dust is spawned.
                Dust dust = Dust.NewDustDirect(spawnArea.TopLeft(), spawnArea.Width, spawnArea.Height, dustType, 0, 0, 100, default, Main.rand.NextFloat(1, 1.5f));
                dust.position = points[pointIndex];
                dust.fadeIn = 0.3f;
                Vector2 spinningPoint = points[pointIndex] - points[pointIndex - 1];
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                //This math causes these dust to spawn with a velocity perpendicular to the direction of the whip segments, giving the impression of the dust flying off like sparks.
                dust.velocity += spinningPoint.RotatedBy(owner.direction * ((float)Math.PI / 2));
                dust.velocity *= 0.5f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SummonTagDamage>(), summonTagDamageTime); //Summon tag damage

            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * multihitPenalty); //Multihit penalty. Decrease the damage the more enemies the whip hits.
        }

        private void DrawLine(List<Vector2> list)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new Vector2(frame.Width / 2, 2);

            Vector2 pos = list[0];
            for (int i = 0; i < list.Count - 1; i++)
            {
                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2;
                Color color = Lighting.GetColor(element.ToTileCoordinates(), lineColor);
                Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

                pos += diff;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);

            DrawLine(list);

            //Main.DrawWhip_WhipBland(Projectile, list);
            //The code below is for custom drawing.
            //If you don't want that, you can remove it all and instead call one of vanilla's DrawWhip methods, like above.
            //However, you must adhere to how they draw if you do.

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 pos = list[0];

            for (int i = 0; i < list.Count - 1; i++)
            {
                //These two values are set to suit this projectile's sprite, but won't necessarily work for your own.
                //You can change them if they don't!
                Rectangle frame = new Rectangle(0, 0, 24, 24); // The size of the Handle (measured in pixels)
                Vector2 origin = new Vector2(12, 12); // Offset for where the player's hand will start measured from the top left of the image.
                float scale = 1;

                //These statements determine what part of the spritesheet to draw for the current segment.
                //They can also be changed to suit your sprite.
                if (i == list.Count - 2)
                {
                    //This is the head of the whip. You need to measure the sprite to figure out these values.
                    frame.Y = 72; //Distance from the top of the sprite to the start of the frame.
                    frame.Height = 24; //Height of the frame.

                    //For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Timer / timeToFlyOut;
                    scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 10)
                {
                    //Third segment
                    frame.Y = 72;
                    frame.Height = 16;
                }
                else if (i > 5)
                {
                    //Second Segment
                    frame.Y = 48;
                    frame.Height = 14;
                }
                else if (i > 0)
                {
                    //First Segment
                    frame.Y = 24;
                    frame.Height = 18;
                }

                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2; //This projectile's sprite faces down, so PiOver2 is used to correct rotation.
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale * whipScale, flip, 0);

                pos += diff;
            }
            return false;
        }
    }
}