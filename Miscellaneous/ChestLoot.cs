using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Eventful.Items.Miscellaneous;
using Eventful.Items.Summons;

namespace Eventful.Miscellaneous
{
    public class ChestLoot : ModSystem
    {
        public override void PostWorldGen()
        {
            int[] itemsToPlaceInGoldChests = { ModContent.ItemType<MutatedHeart>() };
            int itemsToPlaceInGoldChestsChoice = 0;
            int itemsPlaced = 0;

            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest == null)
                {
                    continue;
                }
                Tile chestTile = Main.tile[chest.x, chest.y];
                if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 1 * 36 && Main.rand.NextBool(1, 2))
                {
                    for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(itemsToPlaceInGoldChests[itemsToPlaceInGoldChestsChoice]);
                            itemsToPlaceInGoldChestsChoice = (itemsToPlaceInGoldChestsChoice + 1) % itemsToPlaceInGoldChests.Length;
                            itemsPlaced++;
                            break;
                        }
                    }
                }
            }
        }
    }
}
