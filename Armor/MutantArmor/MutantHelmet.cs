using Eventful.Items.Miscellaneous;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Eventful.Armor.MutantArmor
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Head)]
    public class MutantHelmet : ModItem
    {
        public static LocalizedText SetBonusText { get; private set; }

        public override void SetStaticDefaults()
        {
            SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs("Grants immunity to Poison");
        }

        public override void SetDefaults()
        {
            Item.width = 22; // Width of the item
            Item.height = 22; // Height of the item
            Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
            Item.rare = ItemRarityID.Blue; // The rarity of the item
            Item.defense = 6; // The amount of defense the item will give when equipped
        }

        // IsArmorSet determines what armor pieces are needed for the setbonus to take effect
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<MutantBreastplate>() && legs.type == ModContent.ItemType<MutantLeggings>();
        }

        // UpdateArmorSet allows you to give set bonuses to the armor.
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = SetBonusText.Value;
            player.buffImmune[BuffID.Poisoned] = true;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<MutatedFlesh>(20)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}