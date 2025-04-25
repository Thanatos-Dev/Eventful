using Eventful.Items.Miscellaneous;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Eventful.Tiles
{
    public class SunshineInABottle : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.MultiTileSway[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.addTile(Type);

            DustType = -1;

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(255, 231, 0), name);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                return;
            }

            Main.SceneMetrics.HasSunflower = true;
        }

        private readonly int animationFrameWidth = 18;

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 1;
            g = 0.95f;
            b = 0.5f;
        }

        public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
        {
            // Flips the sprite if x coord is odd. Makes the tile more interesting
            if (i % 2 == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            // Tweak the frame drawn by x position so tiles next to each other are off-sync and look much more interesting
            int uniqueAnimationFrame = Main.tileFrame[Type] + i;
            if (i % 2 == 0)
                uniqueAnimationFrame += 3;
            if (i % 3 == 0)
                uniqueAnimationFrame += 3;
            if (i % 4 == 0)
                uniqueAnimationFrame += 3;
            uniqueAnimationFrame %= 6;

            // frameYOffset = modTile.AnimationFrameHeight * Main.tileFrame[type] will already be set before this hook is called
            // But we have a horizontal animated texture, so we use frameXOffset instead of frameYOffset
            frameXOffset = uniqueAnimationFrame * animationFrameWidth;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            frame = Main.tileFrame[TileID.FireflyinaBottle];
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];

            if (TileObjectData.IsTopLeft(tile))
            {
                // Makes this tile sway in the wind and with player interaction when used with TileID.Sets.MultiTileSway
                Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileDrawing.TileCounterType.MultiTileVine);
            }

            // We must return false here to prevent the normal tile drawing code from drawing the default static tile. Without this a duplicate tile will be drawn.
            return false;
        }

        #region Item
        internal class SunshineInABottleItem : ModItem
        {
            public override void SetDefaults()
            {
                Item.CloneDefaults(ItemID.FireflyinaBottle);
                Item.createTile = ModContent.TileType<SunshineInABottle>();
            }

            public override void AddRecipes()
            {
                CreateRecipe()
                    .AddIngredient<SunshineFragment>()
                    .AddIngredient(ItemID.Bottle)
                    .Register();
            }
        }
        #endregion
    }
}