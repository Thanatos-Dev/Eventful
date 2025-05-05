using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Eventful.Invasions;

namespace Eventful
{
    public class Eventful : Mod
    {
        public static Eventful Instance;

        public Eventful()
        {
            Instance = this;
        }

        public void DrawEventUI(SpriteBatch spriteBatch)
        {
            #region Buried Barrage
            if (BuriedBarrageInvasion.isActive && Main.LocalPlayer.ZoneNormalCaverns)
            {
                const float Scale = 1;
                const float Alpha = 0.5f;
                const int InternalOffset = 6;
                const int OffsetX = 20;
                const int OffsetY = 20;
                const int InfoOffsetY = 2;

                int progress = (int)(100 * BuriedBarrageInvasion.killCount / ((float)BuriedBarrageInvasion.killsNeeded));

                Texture2D EventIcon = Assets.Request<Texture2D>("Invasions/BuriedBarrageBiome_Icon", AssetRequestMode.ImmediateLoad).Value;
                Color descColor = new Color(119, 51, 77);
                Color waveColor = new Color(255, 241, 51);

                int width = (int)(200f * Scale);
                int height = (int)(50f * Scale);

                Rectangle waveBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - OffsetX - 100f, Main.screenHeight - OffsetY - 23f), new Vector2(width, height));
                Utils.DrawInvBG(spriteBatch, waveBackground, new Color(63, 65, 151, 255) * 0.785f);

                string waveText = "Cleared " + progress + "%";
                Utils.DrawBorderString(spriteBatch, waveText, new Vector2(waveBackground.Center.X, waveBackground.Y + 5), Color.White, Scale * 0.85f, 0.5f, -0.1f);
                Rectangle waveProgressBar = Utils.CenteredRectangle(new Vector2(waveBackground.Center.X, waveBackground.Y + waveBackground.Height * 0.75f), TextureAssets.ColorBar.Size());

                var waveProgressAmount = new Rectangle(0, 0, (int)(TextureAssets.ColorBar.Width() * 0.01f * MathHelper.Clamp(progress, 0f, 100f)), TextureAssets.ColorBar.Height());
                var offset = new Vector2((waveProgressBar.Width - (int)(waveProgressBar.Width * Scale)) * 0.5f, (waveProgressBar.Height - (int)(waveProgressBar.Height * Scale)) * 0.5f - InfoOffsetY);

                spriteBatch.Draw(TextureAssets.ColorBar.Value, waveProgressBar.Location.ToVector2() + offset, null, Color.White * Alpha, 0f, new Vector2(0f), Scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(TextureAssets.ColorBar.Value, waveProgressBar.Location.ToVector2() + offset, waveProgressAmount, waveColor, 0f, new Vector2(0f), Scale, SpriteEffects.None, 0f);

                Vector2 descSize = new Vector2(235, 50) * Scale * 0.75f;
                Rectangle barrierBackground = Utils.CenteredRectangle(new Vector2(Main.screenWidth - OffsetX - 100f, Main.screenHeight - OffsetY - 19f), new Vector2(width, height));
                Rectangle descBackground = Utils.CenteredRectangle(new Vector2(barrierBackground.Center.X, barrierBackground.Y - InternalOffset - descSize.Y * 0.5f), descSize * 0.9f);
                Utils.DrawInvBG(spriteBatch, descBackground, descColor * Alpha);

                int descOffset = (descBackground.Height - (int)(32f * Scale)) / 2;
                var icon = new Rectangle(descBackground.X + descOffset + 7, descBackground.Y + descOffset, (int)(32 * Scale), (int)(32 * Scale));
                spriteBatch.Draw(EventIcon, icon, Color.White);
                Utils.DrawBorderString(spriteBatch, "Buried Barrage", new Vector2(barrierBackground.Center.X - 5, barrierBackground.Y - InternalOffset - descSize.Y * 0.5f), Color.White, 0.8f, 0.3f, 0.4f);
            }
            #endregion
        }
    }
}