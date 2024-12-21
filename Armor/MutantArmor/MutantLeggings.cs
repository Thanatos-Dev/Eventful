using Eventful.Items.Miscellaneous;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Eventful.Armor.MutantArmor
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Legs)]
    public class MutantLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22; // Width of the item
            Item.height = 18; // Height of the item
            Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
            Item.rare = ItemRarityID.Blue; // The rarity of the item
            Item.defense = 6; // The amount of defense the item will give when equipped
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<MutatedFlesh>(25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
